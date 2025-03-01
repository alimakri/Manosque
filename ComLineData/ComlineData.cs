using System.Text;

namespace ComLineData
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
        public Dictionary<string, Tuple<string, string>> Parameters = [];

        public string Name
        {
            get { return $"{Verb}-{Noun}"; }
        }
        public List<string> Prompts = new();
        public ErrorCodeEnum ErrorCode = 0;
        public ResultList Results = new();
        public string TableName = "";
        public bool ModeDebug = false;
        public string Filter = "";

        public bool ContainsAllQueryParameters(params string[] searchparameters)
        {
            return searchparameters.All(param => Parameters.ContainsKey(param));
        }
        public bool ContainsOneOfExtraParameters(params string[] searchparameters)
        {
            return searchparameters.Any(param => Parameters.ContainsKey(param));
        }
        public bool ContainsQueryParameter(string param)
        {
            return Parameters.ContainsKey(param);
        }
    }
}
