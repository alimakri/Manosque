using ComLineCommon;
using ComLineData;
using Newtonsoft.Json;
using System;
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
        //public static string Url = "https://makrisoft.net/";
        public static string Url = "https://localhost:7250/";

        public static ComlineData Command { get; set; }
        public  ServiceApi(ComlineData command)
        {
            Command = command;
        }

        #region Execute
        public void Execute(EnumFakeApi simul = EnumFakeApi.None)
        {
            if (simul == EnumFakeApi.None) ExecuteApi(); else ExecuteApi_Fake(simul);
        }
        public bool ConnectApi(UserLogin login)
        {
            // Init
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(Url);
            Token = null;

            // content
            string json = ""; StringContent content;
            try
            {
                json = System.Text.Json.JsonSerializer.Serialize(login);

                content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Post, "api/comline/login") { Content = content };

                // Send
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<

                // Result
                if (response.Result != null)
                {
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var jsonResult1 = response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            Token = System.Text.Json.JsonSerializer.Deserialize<JwtToken>(jsonResult1);
                            if(Token?.userId != null) Command.Results.AddInfo(Token.userId.ToString(), "Info");
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
            return Token != null;
        }

        private  void ExecuteApi()
        {
            // Init
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token?.token);
            client.BaseAddress = new Uri(Url);

            // content
            string s = "", json = ""; StringContent content;
            try
            {
                s = $"Connect-Service -Name {RemoteService};";
                s += Command.Prompts[0].Replace("\"", "\\\"").Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

                json = $"{{\"Script\":\"{s}\"}}";

                content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new(HttpMethod.Post, "api/comline") { Content = content };

                // Send
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<

                // Result
                if (response.Result != null)
                {
                    switch (response.Result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var jsonResult = Regex.Unescape(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult()).Trim('"');
                            var result = (ResultList?)JsonConvert.DeserializeObject(jsonResult, typeof(ResultList));

                            if (result != null) 
                                Command.Results = result;
                            else
                                Command.Results.AddError($"Erreur d'un appel api : {response.Result.ReasonPhrase}", ErrorCodeEnum.AppelApi);
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

        private  void ExecuteApi_Fake(EnumFakeApi simul)
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


        public  void DeconnectApi()
        {
            if (Token != null) Token.token = "";
        }

        #endregion
    }
    public interface IServiceApi
    {
        static ComlineData Command { get; set; }

    }
    public enum EnumFakeApi { None, ListeSites, Password }
    public class JwtToken
    {
        public Guid userId { get; set; } = default;
        public string token { get; set; } = "";
    }
    public enum EnumUrlParam { Comline, Auth }
}
