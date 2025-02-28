using ComLineCommon;
using ComLineData;
using System.Diagnostics;

namespace ComlineServices
{
    public class ServiceSystem(ComlineData command)
    {
        public static Dictionary<string, string> Options = [];
        private readonly ComlineData Command = command;
        public string QueryName { get { return Command.QueryName; } }
        public static string WorkingDirectory = "";
        public void Execute()
        {
            switch (QueryName)
            {
                case "Set-Option.Service":
                    Options["Service"] = Command.Parameters["Service"].Item2.Trim('"');
                    Command.Results.AddInfo($"Service {Command.Parameters["Service"]}", "Info");
                    break;
                case "Set-Option.DisplayMode":
                    Options["DisplayMode"] = Command.Parameters["DisplayMode"].Item2.Trim('"');
                    Command.Results.AddInfo($"DisplayMode {Command.Parameters["DisplayMode"]}", "Info");
                    break;
                case "Get-Version":
                    Command.Results.AddInfo(Global.VersionSystem, "Info");
                    break;
                case "Execute-File.Name":
                    if (CheckParameter_File(Path.Combine(WorkingDirectory, Command.Parameters["Name"].Item2)))
                    {
                        Command.Results.AddInfos(
                            File.ReadAllLines(
                                Path.Combine(WorkingDirectory, Command.Parameters["Name"].Item2))
                            //.Where(l => !l.StartsWith('#')).ToArray()
                            , "Commande");
                    }
                    break;
                default:
                    Command.Results.AddError($"ComlineServiceSystem.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : Le service {Options["Service"]} n'a pas la commande {Command.QueryName} !", ErrorCodeEnum.UnexistedCommand);
                    Command.ErrorCode = ErrorCodeEnum.UnexistedCommand;
                    break;
            }
        }
        #region CheckParameter
        private bool CheckParameter_File(string path)
        {
            if (!File.Exists(path))
            {
                Command.ErrorCode = ErrorCodeEnum.UnexistedFile;
                Command.Results.AddError($"File [{path}] does not exist.", ErrorCodeEnum.UnexistedFile);
                return false;
            }
            return true;
        }
        //private bool CheckParameter_Directory()
        //{
        //    if (!Directory.Exists(Command.Parameters["Path"]))
        //    {
        //        Command.ErrorCode = ErrorCodeEnum.UnexistedDirectory;
        //        Command.Results.AddError($"Directory [{Command.Parameters["Path"]}] does not exist.", ErrorCodeEnum.UnexistedDirectory);
        //    }
        //    return true;
        //}
        #endregion
    }
}
