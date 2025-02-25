using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using ComLineCommon;
using ComlineServices;

namespace ComlineApp.Manager
{
    public partial class CoreComline(IServiceData serviceData) : ICoreComline
    {
        public IServiceData MonServiceData = serviceData;


        #region Properties
        private readonly Regex TheRegex = MyRegex();
        public ComlineData Command = new();
        public ContinueEnum Continue = ContinueEnum.StopOnError;

        public ResultList Results
        {
            get { return Command.Results; }
        }
        public string SingleCommand
        {
            get { return Command.SingleCommand; }
        }

        #endregion

        #region Execute
        public void Execute(List<string?> prompts)
        {
            if (prompts.Count == 0) return; var prompt = prompts[0];
            if (prompt != null && string.IsNullOrEmpty(prompt.Trim())) return;
            //if (prompt != null && prompt.StartsWith('#')) return;

            // Execute prompt
            if (prompt != null) Command.Prompt = prompt.Trim();
            Results.Tables.Clear();
            Command.ErrorCode = ErrorCodeEnum.None;
            Continue = ContinueEnum.StopOnError;

            AnalyzePrompt();
            var res = SelectService();
            if (res == ErrorCodeEnum.None && res != ErrorCodeEnum.NothingToDo) ExecuteService();

            if (Continue == ContinueEnum.Stop || (Command.ErrorCode != ErrorCodeEnum.None && Command.ErrorCode != ErrorCodeEnum.NothingToDo))
            {
                prompts.RemoveRange(1, prompts.Count - 1);
            }

            // Execute-File ----------------------------
            if (Command.ErrorCode == 0 && SingleCommand == "Execute-File")
            {
                var table = Results.Tables["Commande"];
                if (table != null) prompts.InsertRange(1, table.AsEnumerable().Select(row => row.Field<string>("Libelle")));
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
                    new ServiceApi(Command).Execute([Command.Prompt]);
                    break;
                default:
                    Command.Results.AddError($"Comline.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()}: la commande {Command.QueryName} n'est pas associé à un service ou le service [{ServiceSystem.Options["Service"]}] n'existe pas !", ErrorCodeEnum.UnexistedService);
                    Command.ErrorCode = ErrorCodeEnum.UnexistedService;
                    break;
            }
        }
        #endregion

        #region Analyze
        private void AnalyzePrompt()
        {
            // Regex error
            var matches = TheRegex.Match(Command.Prompt);
            if (!matches.Success)
            {
                Command.Results.AddError($"Comline.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : La commande {Command.QueryName} n'existe pas !", ErrorCodeEnum.UnexistedCommand);
                Command.ErrorCode = ErrorCodeEnum.UnexistedCommand;
                return;
            }

            // Verb & Noun
            Command.Verb = matches.Groups["verb"].Value.Capitalize();
            Command.Noun = matches.Groups["noun"].Value.Capitalize();

            // Parameters
            Command.Parameters.Clear();
            var parameters = matches.Groups["param"].Captures;
            var values = matches.Groups["value"].Captures;
            for (int i = 0; i < parameters.Count; i++)
            {
                var val = "";
                if (i < values.Count) val = values[i].Value;
                Command.Parameters.Add(
                    parameters[i].Value.Capitalize(),
                    val);
            }
            Command.Parameters = Command.Parameters.OrderBy(p => p.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            // Continue
            ContinueEnum c = ContinueEnum.None;
            if (Command.Parameters.ContainsKey("Continue"))
            {
                _ = Enum.TryParse<ContinueEnum>(Command.Parameters["Continue"], out c);
                if (c != ContinueEnum.None) Continue = c;
                Command.Parameters.Remove("Continue");
            }

            Command.QueryName = SingleCommand;

            if (Command.Parameters.Count > 0)
            {
                switch (Command.Verb)
                {
                    case "Get":
                        Command.QueryName = Command.SingleCommand; break;
                    case "New":
                        if (Command.Parameters.ContainsKey("Reference"))
                            Command.QueryName = $"{SingleCommand}.InsertFull";
                        //else
                        //    Command.QueryName += "error";
                        break;
                    default:
                        Command.QueryName += "." + string.Join('.', Command.Parameters.Select(x => x.Key.ToString()).OrderBy(x => x)); break;
                }
            }
        }
        private ErrorCodeEnum SelectService()
        {
            if (!ServiceSystem.Options.ContainsKey("Service")) ServiceSystem.Options["Service"] = "System";

            if (!Global.Services.Contains(ServiceSystem.Options["Service"]))
            {
                Command.Results.AddError("Service non existant !", ErrorCodeEnum.UnexistedService);
            }
            else if (Command.SingleCommand == "Set-Option" && Command.Parameters.ContainsKey("Service"))
            {
                ServiceSystem.Options["Service"] = Command.Parameters["Service"];
                Command.Results.AddInfo($"Service {ServiceSystem.Options["Service"]} ok", "Info");
                Command.ErrorCode = ErrorCodeEnum.NothingToDo;

            }
            return Command.ErrorCode;
        }

        public void Reset()
        {
            Command = new ComlineData();
        }

        [GeneratedRegex(@"(?<verb>\w+)-(?<noun>\w+)((?:\s+-(?<param>\w+)(?:[\f\n\r\t\v\s\p{Z}]+(?<value>""[^""]+""|\S+(?:\.\d+)?))?)*)")]
        private static partial Regex MyRegex();
        #endregion
    }

    public interface ICoreComline
    {
        void Execute(List<string?> prompts);
        ResultList Results { get; }
    }

    public enum ContinueEnum { None, StopOnError, Stop, ContinueOnError }
}
