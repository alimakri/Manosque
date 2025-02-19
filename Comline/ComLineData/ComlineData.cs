

using System.Text;

namespace ComlineApp.Manager
{
    public enum ServiceEnum { None, System, Data, File }
    public enum ErrorCodeEnum
    {
        None,
        WrongParameter,
        ServiceData_UnexistedCommand,
        UnexistedService,
        UnexistedCommand,
        UnexistedFile,
        UnexistedDirectory,
        QueryError,
        UnExistedTable,
        NothingToDo,
        AppelApi
    }
    public class ComlineData
    {
        public string Verb = "";
        public string Noun = "";
        public Dictionary<string, string> Parameters = [];

        public string SingleCommand
        {
            get { return $"{Verb}-{Noun}"; }
        }
        public string QueryName = "";
        public string Prompt = "";
        public ErrorCodeEnum ErrorCode = 0;
        public ResultList Results = new();
        public string TableName = "";
        public string ModeDebug = "";

        public bool ContainsAllParameters(params string[] searchparameters)
        {
            return searchparameters.All(param => Parameters.ContainsKey(param));
        }
        public bool ContainsOneOfParameters(params string[] searchparameters)
        {
            return searchparameters.Any(param => Parameters.ContainsKey(param));
        }
        public bool ContainsParameter(string param)
        {
            return Parameters.ContainsKey(param);
        }
    }
}
