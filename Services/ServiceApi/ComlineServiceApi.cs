using ComlineApp.Manager;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace ComlineApp.Services
{
    public class ServiceApi : IServiceApi
    {
        public ComlineData Command { get; set; } 
        public ServiceApi(ComlineData command)
        {
            Command = command;
        }
        public void Execute()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7040/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = $@"{{""Command"":""{Command.Prompt.Replace("\"", "\\\"")}""}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new(HttpMethod.Post, "api/comline") { Content = content };
            try
            {
                HttpResponseMessage response = client.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    DataTable? dt = (DataTable?)JsonConvert.DeserializeObject(
                        Regex.Unescape(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).Trim('"'),
                        typeof(DataTable));
                    if (response.Headers.TryGetValues("X-Table-Name", out var tableNameValues) && dt != null)
                    {
                        string? tableName = tableNameValues.FirstOrDefault();
                        dt.TableName = tableName;
                    }
                    if (dt != null) Command.Results.Tables.Add(dt);
                }
                else
                    Command.Results.AddError($"Erreur d'un appel api : {response.ReasonPhrase}", ErrorCodeEnum.AppelApi);
            }
            catch (Exception ex)
            {
                Command.Results.AddError($"Erreur de connexion au service Web : {ex.Message}", ErrorCodeEnum.AppelApi);
            }
        }

    }
    public interface IServiceApi
    {
        void Execute();
        ComlineData Command { get; set; }

    }
}
