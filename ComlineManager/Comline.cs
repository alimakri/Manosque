using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using ComLineCommon;
using ComlineServices;
using ComLineData;
using System.Linq;

namespace ComlineApp.Manager
{
    public partial class CoreComline() : ICoreComline
    {
        public Manosque.ServiceData.ServiceData? MonServiceData;
        public ServiceApi? MonServiceApi;
        public ServiceSystem? MonServiceSystem;

        #region Properties
        private readonly Regex TheRegex = MyRegex();
        public ComlineData Command = new();
        public ContinueEnum Continue = ContinueEnum.StopOnError;

        #endregion

        #region Execute
        public void Execute()
        {
            Continue = ContinueEnum.StopOnError;

            var prompt = Command.Prompts[0].Trim();
            if (!string.IsNullOrEmpty(prompt) && !prompt.StartsWith('#'))
            {
                // Init Services
                MonServiceApi = new ServiceApi(Command);
                MonServiceSystem = new ServiceSystem(Command);
                MonServiceData = new Manosque.ServiceData.ServiceData();
                MonServiceData.Command = Command;

                // Analyze
                AnalyzePrompt();
                // SelectService
                var res = SelectService();
                // ExecuteService
                if (res == ErrorCodeEnum.None && res != ErrorCodeEnum.NothingToDo) 
                    ExecuteService();
            }
            if (Continue == ContinueEnum.Stop || (Command.ErrorCode != ErrorCodeEnum.None && Command.ErrorCode != ErrorCodeEnum.NothingToDo))
                Command.Prompts.RemoveRange(1, Command.Prompts.Count - 1);

            // Execute-File ----------------------------
            if (Command.ErrorCode == 0 && Command.Name == "Execute-File")
            {
                DataTable? table = Command.Results.Tables["Commande"];
                if (table != null)
                {
                    var list = table?.AsEnumerable().Select(row => row.Field<string>("Libelle")).Cast<string>().Where(x => x != null);
                    if (list != null && list.Any()) Command.Prompts.InsertRange(1, list);
                }
            }
        }
        private void ExecuteService()
        {
            // Continue ?
            if (Command.ErrorCode != ErrorCodeEnum.None && Command.ErrorCode != ErrorCodeEnum.NothingToDo)
            {
                if (Continue == ContinueEnum.ContinueOnError)
                    Command.Results.AddError("Executing continue even if there's an error", ErrorCodeEnum.None);
                else if (Continue == ContinueEnum.StopOnError)
                    Command.Results.AddError("Executing terminated on error", ErrorCodeEnum.None);
                return;
            }
            // Execute Service
            switch (ServiceSystem.Options["Service"])
            {
                case "System":
                    MonServiceSystem?.Execute();
                    break;
                case "Data":
                    MonServiceData?.Execute();
                    break;
                case "Api":
                    MonServiceApi?.Execute();
                    break;
                default:
                    Command.Results.AddError($"Comline.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()}: la commande n'est pas associé à un service ou le service [{ServiceSystem.Options["Service"]}] n'existe pas !", ErrorCodeEnum.UnexistedService);
                    Command.ErrorCode = ErrorCodeEnum.UnexistedService;
                    break;
            }
        }
        #endregion

        #region Analyze
        private void AnalyzePrompt()
        {
            // Regex error
            var matches = TheRegex.Match(Command.Prompts[0]);
            if (!matches.Success)
            {
                Command.Results.AddError($"Comline.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : La commande {Command.Name} n'existe pas !", ErrorCodeEnum.UnexistedCommand);
                Command.ErrorCode = ErrorCodeEnum.UnexistedCommand;
                return;
            }

            // Verb & Noun
            Command.Verb = matches.Groups["verb"].Value.Capitalize();
            Command.Noun = matches.Groups["noun"].Value.Capitalize();
            Command.Name = $"{Command.Verb}-{Command.Noun}";
            // Parameters
            var parameters = matches.Groups["param"].Captures;
            var values = matches.Groups["value"].Captures;
            for (int i = 0; i < parameters.Count; i++)
            {
                var val = "";
                if (i < values.Count) val = values[i].Value;
                Command.Parameters.Add(parameters[i].Value.Capitalize(), new Tuple<string, string>(parameters[i].Value.Capitalize(), val));
            }
            //Command.Parameters = Command.Parameters.OrderBy(p => p.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Continue
            ContinueEnum c = ContinueEnum.None;
            if (Command.Parameters.ContainsKey("Continue"))
            {
                _ = Enum.TryParse<ContinueEnum>(Command.Parameters["Continue"].Item2, out c);
                if (c != ContinueEnum.None) Continue = c;
                Command.Parameters.Remove("Continue");
            }
        }
        private ErrorCodeEnum SelectService()
        {
            // Service par défaut
            if (!ServiceSystem.Options.TryGetValue("Service", out string? actualService)) ServiceSystem.Options["Service"] = "System";

            if (Command.Name == "Connect-Service" && actualService != null)
            {
                Command.Parameters.TryGetValue("Name", out Tuple<string, string>? askedService);

                if (askedService != null)
                {
                    // askedService non existant
                    if (!Global.Services.Contains(actualService))
                    {
                        Command.Results.AddError("Service non existant !", ErrorCodeEnum.UnexistedService);
                    }
                    // Changement Service distant
                    else if (askedService.Item2 != "Api" && actualService == "Api")
                    {
                        ServiceApi.RemoteService = askedService.Item2;
                        Command.Results.AddInfo($"Remote Service {ServiceApi.RemoteService} ok", "Info");
                    }
                    else if (MonServiceApi != null)
                    {
                        // Deconnect Api
                        if (askedService?.Item2 == "Api" && actualService == "Api")
                        {
                            ServiceSystem.Options["Service"] = "System";
                            Command.Results.AddInfo($"Api déconnecté", "Info");
                            MonServiceApi.DeconnectApi();
                        }
                        // Connect to Api
                        else if (askedService?.Item2 == "Api")
                        {
                            if (Command.Parameters.TryGetValue("Login", out Tuple<string, string>? login) &&
                                Command.Parameters.TryGetValue("Password", out Tuple<string, string>? password) &&
                                MonServiceApi.ConnectApi(new UserLogin { Username = login.Item2, Password = password.Item2 }))
                            {
                                ServiceSystem.Options["Service"] = askedService.Item2;
                                Command.Results.AddInfo($"Service {askedService.Item2} ok", "Info");
                            }
                            else
                            {
                                Command.Results.AddError($"Identification nécessaire", ErrorCodeEnum.Authentication);
                            }
                        }
                        // Changement Service local
                        else
                        {
                            if (Global.Services.Contains(askedService.Item2))
                            {
                                ServiceSystem.Options["Service"] = askedService.Item2;
                                Command.Results.AddInfo($"Service {askedService.Item2} ok", "Info");
                            }
                            else
                                Command.Results.AddError($"Service {askedService.Item2} non existant", ErrorCodeEnum.UnexistedService);
                        }
                    }
                }
                Command.ErrorCode = ErrorCodeEnum.NothingToDo;
            }
            return Command.ErrorCode;
        }


        [GeneratedRegex(@"(?<verb>\w+)-(?<noun>\w+)((?:\s+-(?<param>\w+)(?:[\f\n\r\t\v\s\p{Z}]+(?<value>""[^""]+""|\S+(?:\.\d+)?))?)*)")]
        private static partial Regex MyRegex();

        #endregion
    }

    public interface ICoreComline
    {
        void Execute();
    }

    public enum ContinueEnum { None, StopOnError, Stop, ContinueOnError }
}
