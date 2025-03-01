using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using ComLineCommon;
using ComlineServices;
using ComLineData;

namespace ComlineApp.Manager
{
    public partial class CoreComline(IServiceData serviceData) : ICoreComline
    {
        public IServiceData MonServiceData = serviceData;

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
                // Analyze
                AnalyzePrompt();
                // SelectService
                var res = SelectService();
                // ExecuteService
                if (res == ErrorCodeEnum.None && res != ErrorCodeEnum.NothingToDo) ExecuteService();
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

            if (Command.ErrorCode != ErrorCodeEnum.None && Command.ErrorCode != ErrorCodeEnum.NothingToDo)
            {
                if (Continue == ContinueEnum.ContinueOnError)
                    Command.Results.AddError("Executing continue even if there's an error", ErrorCodeEnum.None);
                else if (Continue == ContinueEnum.StopOnError)
                    Command.Results.AddError("Executing terminated on error", ErrorCodeEnum.None);
                return;
            }
            switch (ServiceSystem.Options["Service"])
            {
                case "System":
                    new ServiceSystem(Command).Execute();
                    break;
                case "Data":
                    MonServiceData.Execute(Command);
                    break;
                case "Api":
                    new ServiceApi(Command).Execute();
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
            if (!ServiceSystem.Options.TryGetValue("Service", out string? option)) ServiceSystem.Options["Service"] = "System";

            if (Command.Name == "Set-Option" && option != null)
            {
                Command.Parameters.TryGetValue("Service", out Tuple<string, string>? askedService);

                if (askedService != null)
                {
                    // askedService non existant
                    if (!Global.Services.Contains(option))
                    {
                        Command.Results.AddError("Service non existant !", ErrorCodeEnum.UnexistedService);
                    }
                    // Sortir de Service Api
                    else if (option == "Api" && askedService?.Item2 == "Api")
                    {
                        ServiceSystem.Options["Service"] = "System";
                        Command.Results.AddInfo($"Api déconnecté", "Info");
                    }
                    // Changement Service distant
                    else if (option == "Api" && askedService != null)
                    {
                        ServiceApi.RemoteService = askedService.Item2;
                        Command.Results.AddInfo($"Remote Service {ServiceApi.RemoteService} ok", "Info");
                    }
                    // Changement Service local
                    else
                    {
                        ServiceSystem.Options["Service"] = askedService.Item2;
                        Command.Results.AddInfo($"Service {option} ok", "Info");
                        Command.ErrorCode = ErrorCodeEnum.NothingToDo;

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
