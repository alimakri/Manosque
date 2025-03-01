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
        public static string RemoteService="System";

        public ComlineData Command { get; set; }
        public ServiceApi(ComlineData command)
        {
            Command = command;
        }

        #region Execute
        public void Execute(EnumFakeApi simul = EnumFakeApi.None)
        {
            if (simul == EnumFakeApi.None) ExecuteApi(); else ExecuteApi_Fake(simul);
        }

        private void ExecuteApi_Fake(EnumFakeApi simul)
        {
            Command.Reset();
            string path = Path.Combine(Global.WorkingDirectory_ServiceApi, $"{simul.ToString()}.xml");
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
                using (StreamReader reader = new StreamReader(path))
                {
                    var dt = (DataTable?)serializer.Deserialize(reader);
                    if (dt != null) Command.Results.Tables.Add(dt);
                }
            }
        }

        private void ExecuteApi()
        {
            Command.Reset();
            using var client = new HttpClient();
            client.BaseAddress = new Uri(Global.Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string s = "";
            s = $"Set-Option -Service {RemoteService};";
            s += string.Join(';', Command.Prompts.Select(x => x.Replace("\"", "\\\"")));
            var json = $"{{\"Script\":\"{s}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Post, "api/comline") { Content = content };
            try
            {
                Task<HttpResponseMessage> response = client.SendAsync(request); // <<<<<<<<<<<<<<<<<
                if (response.Result.IsSuccessStatusCode)
                {
                    var jsonResult = Regex.Unescape(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult()).Trim('"');
                    var result = (ResultList?)JsonConvert.DeserializeObject(jsonResult, typeof(ResultList));
                    if (result != null) Command.Results = result;
                }
                else
                    Command.Results.AddError($"Erreur d'un appel api : {response.Result.ReasonPhrase}", ErrorCodeEnum.AppelApi);
            }
            catch (Exception ex)
            {
                Command.Results.AddError($"Erreur de connexion au service Web : {ex.Message}", ErrorCodeEnum.AppelApi);
            }
        }
        #endregion
    }
    public interface IServiceApi
    {
        void Execute(EnumFakeApi simul = EnumFakeApi.None);
        ComlineData Command { get; set; }

    }
    public enum EnumFakeApi { None, ListeSites, Password }
}
