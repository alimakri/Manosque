using ComLineCommon;
using ComLineData;
using System.Diagnostics;
using System.IO;

namespace ComlineServices
{
    public class ServiceSystem(ComlineData command)
    {
        public static Dictionary<string, string> Options = [];
        private readonly ComlineData Command = command;

        #region Execute
        public void Execute()
        {
            switch (Command.Name)
            {
                case "Set-Option":
                    // Set-Option -DisplayMode
                    if (Command.Parameters.TryGetValue("DisplayMode", out Tuple<string, string, string>? value2) && value2 != null)
                    {
                        Options["DisplayMode"] = value2.Item2;
                        Command.Results.AddInfo($"DisplayMode {Command.Parameters["DisplayMode"]}", "Info");
                    }
                    break;
                case "Get-Version":
                    Command.Results.AddInfo(Global.VersionSystem, "Info");
                    break;
                case "Execute-File":
                    var path = Path.Combine(Global.WorkingDirectory_ServiceSystem, Command.Parameters["Name"].Item2);
                    if (!File.Exists(path))
                    {
                        Command.ErrorCode = ErrorCodeEnum.UnexistedFile;
                        Command.Results.AddError($"File [{path}] does not exist.", ErrorCodeEnum.UnexistedFile);
                    }
                    else
                    {
                        Command.Results.AddInfos(File.ReadAllLines(path), "Commande");
                    }
                    break;
                default:
                    Command.Results.AddError($"ComlineServiceSystem.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : Le service {Options["Service"]} n'a pas la commande {Command.Name} !", ErrorCodeEnum.UnexistedCommand);
                    Command.ErrorCode = ErrorCodeEnum.UnexistedCommand;
                    break;
            }
        }
        #endregion
    }
}
