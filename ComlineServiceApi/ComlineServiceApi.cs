using ComlineApp.Manager;
using ComLineCommon;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ComlineServices
{
    public enum EnumDataSimulation
    {
        None, ListeSites,
        Password
    }
    public class ServiceApi : IServiceApi
    {
        public ComlineData Command { get; set; }
        public ServiceApi(ComlineData command)
        {
            Command = command;
        }
        public void Execute(EnumDataSimulation simul = EnumDataSimulation.None)
        {
            Command.Results.Tables.Clear();
            if (simul == EnumDataSimulation.None)
                ExecuteWithoutSimul();
            else
            {
                string path = Path.Combine(Global.WorkingDirectory, $"{simul.ToString()}.xml");
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
        }
        private void ExecuteWithoutSimul()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(Global.Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = $@"{{""Command"":""{Command.Prompt.Replace("\"", "\\\"")}""}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Post, "api/comline") { Content = content };
            try
            {
                Task<HttpResponseMessage> response = client.SendAsync(request);
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
    }
    public interface IServiceApi
    {
        void Execute(EnumDataSimulation simul = EnumDataSimulation.None);
        ComlineData Command { get; set; }

    }
}
