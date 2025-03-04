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
        AppelApi,
        Authentication
    }
    public class ComlineData
    {
        public string Verb="";
        public string Noun = "";
        public string Name = "";
        public string TableName = "";

        // string1 = cle, string2=val trimé, string3 val pour sql
        public Dictionary<string, Tuple<string, string, string>> Parameters = [];
        public List<string> Prompts = [];

        public ErrorCodeEnum ErrorCode = 0;
        public bool ModeDebug = false;

        public ResultList Results = new();
        public string Filter = "";
        public string ConnectionString;

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

        public void Reset()
        {
            Results.Tables.Clear(); 
            ErrorCode = ErrorCodeEnum.None;
            Parameters.Clear();
        }
    }
}
