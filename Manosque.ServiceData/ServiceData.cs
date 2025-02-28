using ComlineApp.Services;
using ComLineCommon;
using ComLineData;
using ComlineServices;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Manosque.ServiceData
{
    public class ServiceData : ComlineServices.ServiceData, IServiceData
    {
        public ServiceData(string connectionString) : base(connectionString)
        {
            DataTable t = new();
            Query.CommandText = "select * from Absence";
            try
            {
                Adapter.Fill(t);
            }
            catch (Exception)
            {
                return;
            }
            foreach (DataRow row in t.Rows)
            {
                CronTools.JoursExclus.Add(new Absence { Date = DateOnly.FromDateTime((DateTime)row["Jour"]), Personne = row["Personne"] as Guid? });
            }
        }
        public override void Execute(ComlineData command)
        {
            // Init
            Command = command;

            // Pre Process : Build Query
            Before();

            // Execute builded query
            if (Command.TableName != "")
                try
                {
                    Adapter.Fill(Command.Results, Command.TableName);
                    SerializeResults();
                }
                catch (Exception ex)
                {
                    Command.Results.AddError($"ServiceData.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()} : \n{Query.CommandText}\n {ex.Message}", ErrorCodeEnum.QueryError);
                    Command.ErrorCode = ErrorCodeEnum.QueryError;
                    return;
                }
            // Post Process
            //After();
        }

        private void SerializeResults()
        {
            if (Command.ModeDebug && Command.Results.Tables.Count > 0 && Directory.Exists(WorkingDirectory))
            {
                string fileName = "Results_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var ds = Command.Results; var path = Path.Combine(WorkingDirectory, fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(ResultList));
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, ds);
                }
            }
        }

        //private void After()
        //{
        //}

        private void Before()
        {
            switch (Command.Verb)
            {
                case "Get":
                    switch (Command.Noun)
                    {
                        case "Version": Query.CommandText = $"select '{Global.VersionDatabase}' Version"; break;
                        case "Execution":
                            if (Command.ContainsQueryParameter("Id"))
                            {
                                Query.CommandText = QueryFactory.Select(Command);
                            }
                            else
                            {
                                Query.CommandText = QueryFactory.Select(Command);
                            }
                            break;
                        default: Query.CommandText = QueryFactory.Select(Command); break;
                    }
                    break;
                case "Add":
                    Query.CommandText = QueryFactory.Add(Command);
                    break;
                case "Update":
                    Query.CommandText = QueryFactory.Update(Command);
                    break;
                case "Remove":
                    Query.CommandText = QueryFactory.Remove(Command); break;
                case "New":
                    Query.CommandText = QueryFactory.Insert(Command);
                    break;
                case "Delete":
                    Query.CommandText = QueryFactory.Delete(Command); break;
                case "Generate":
                    if (Command.Noun == "Execution")
                        Query.CommandText = QueryFactory.GenerateExecution(Command);
                    break;
                default:
                    Query.CommandText = $"select -1001 [Index], 'Service Data.cs.{new StackTrace(true).GetFrame(0)?.GetFileLineNumber()}: la requête {Command.QueryName} n''existe pas !' Libelle";
                    Command.TableName = "Error";
                    Command.ErrorCode = ErrorCodeEnum.ServiceData_UnexistedCommand;
                    break;
            }
        }
    }
}
