using ComLineCommon;
using ComLineData;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ComlineServices
{
    public class ServiceApi : IServiceApi
    {
        public static string RemoteService = "System";
        public static JwtToken? Token;
        public EnumUrlParam UrlParam = EnumUrlParam.Comline;
        public static string Url = Global.RemoteUrl;
        public ComlineData Command { get; set; }
        public ServiceApi(ComlineData command)
        {
            Command = command;
            switch (command.Name)
            {
                case "Connect-Server":
                    UrlParam = EnumUrlParam.Auth;
                    command.Parameters.TryGetValue("Name", out Tuple<string, string>? server);
                    Url = server == null || server.Item2.Equals("ionos", StringComparison.CurrentCultureIgnoreCase) ? Global.RemoteUrl : Global.LocalUrl;

                    break;
            }
        }

        #region Execute
        public void Execute(EnumFakeApi simul = EnumFakeApi.None)
        {
            if (simul == EnumFakeApi.None) ExecuteApi(); else ExecuteApi_Fake(simul);
        }

        private void ExecuteApi_Fake(EnumFakeApi simul)
        {
            Command.Reset();
            string path = Path.Combine(Global.WorkingDirectory_ServiceApi, $"{simul}.xml");
            if (File.Exists(path))
            {
                XmlSerializer serializer = new(typeof(DataTable));
                using StreamReader reader = new(path);
                var dt = (DataTable?)serializer.Deserialize(reader);
                if (dt != null) Command.Results.Tables.Add(dt);
            }
        }

        private void ExecuteApi()
        {
            //Command.Reset();

            // Init
            using var client = new HttpClient();
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // content
            string s = "", json = ""; StringContent content; string urlParam = "api/comline";
            try
            {
                switch (UrlParam)
                {
                    case EnumUrlParam.Auth:
                        urlParam = "api/auth/login";
                        Command.Parameters.TryGetValue("Login", out Tuple<string, string>? user);
                        Command.Parameters.TryGetValue("Password", out Tuple<string, string>? password);
                        var login = new UserLogin { Username = user?.Item2.Trim('"') ?? "", Password = password?.Item2.Trim('"') ?? "" };
                        json = System.Text.Json.JsonSerializer.Serialize(login);
                        break;
                    case EnumUrlParam.Comline:
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token?.token);
                        s = $"Set-Option -Service {RemoteService};";
                        s += string.Join(';', Command.Prompts.Select(x => x.Replace("\"", "\\\"")));
                        json = $"{{\"Script\":\"{s}\"}}";
                        break;
                }
                content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Post, urlParam) { Content = content };

                // Send
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<

                // Result
                if (response.Result != null)
                {
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            switch (UrlParam)
                            {
                                case EnumUrlParam.Auth:
                                    var jsonResult1 = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    Token = System.Text.Json.JsonSerializer.Deserialize<JwtToken>(jsonResult1);
                                    Command.Results.AddInfo($"ok * - * {Token?.token}", "Info");
                                    break;
                                case EnumUrlParam.Comline:
                                    var jsonResult = Regex.Unescape(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult()).Trim('"');
                                    var result = (ResultList?)JsonConvert.DeserializeObject(jsonResult, typeof(ResultList));
                                    if (result != null) Command.Results = result;
                                    // Error
                                    else
                                        Command.Results.AddError($"Erreur d'un appel api : {response.Result.ReasonPhrase}", ErrorCodeEnum.AppelApi);
                                    break;
                            }
                            break;
                        default:
                            Command.Results.AddError($"Erreur d'un appel api : {response.Result.ReasonPhrase}", ErrorCodeEnum.AppelApi);
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Command.Results.AddError($"Erreur de connexion au service WebApi : {ex.Message}", ErrorCodeEnum.AppelApi);
            }
        }

        public static void Deconnect()
        {
            if (Token != null) Token.token = "";
        }
        #endregion
    }
    public interface IServiceApi
    {
        ComlineData Command { get; set; }

    }
    public enum EnumFakeApi { None, ListeSites, Password }
    public class JwtToken
    {
        public string token { get; set; } = "";
    }
    public enum EnumUrlParam { Comline, Auth }
}
