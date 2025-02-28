using ComLineCommon;
using ComLineData;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComlineServices
{
    /// <summary>
    /// Service Data travaillant avec un ComlineData
    /// Le résultat d'une requête SQL est toujours rangé dans un DataSet
    /// </summary>
    public class ServiceData
    {
        #region Properties
        protected ComlineData Command = new();
        public static string WorkingDirectory = "";

        // SQL SERVER
        protected SqlCommand Query = new();
        protected SqlDataAdapter Adapter = new();
        #endregion

        public ServiceData(string connectionString)
        {
            //Command = command;
            SqlConnection cnx = new()
            {
                ConnectionString = connectionString
            };
            cnx.Open();
            Query.Connection = cnx;
            Query.CommandType = System.Data.CommandType.Text;
            Adapter.SelectCommand = Query;
        }

        public virtual void Execute(ComlineData command) { }


        protected bool CheckParameters(string query, string checks)
        {
            var b = true;
            Query.CommandText = query;
            try
            {
                Adapter.Fill(Command.Results, "Param");
            }
            catch (Exception ex)
            {
                Command.Results.AddError($"[{query}]\n{ex.Message}", ErrorCodeEnum.QueryError);
                Command.ErrorCode = ErrorCodeEnum.QueryError;
                return false;
            }
            var tableParam = Command.Results.Tables["Param"] ?? throw new Exception($"CheckParameters [{query}] [{checks}]");
            var checksTab = checks.Split('|');
            for (int i = 0; i < tableParam.Columns.Count; i++)
            {
                // Check not null
                if (!checksTab[i].Contains('?') && tableParam.Rows[0][i] is DBNull)
                {
                    Command.Results.AddError($"Pas de {tableParam.Columns[i].ColumnName} pour {Command.Parameters.Keys.ElementAt(i)} !", ErrorCodeEnum.WrongParameter);
                    Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                    b = false;
                }
                // Check type
                if (tableParam.Columns[i].DataType.Name != checksTab[i])
                {
                    Command.Results.AddError($"Le Paramètre {tableParam.Columns[i].ColumnName} n'est pas du type {checksTab[i]} !", ErrorCodeEnum.WrongParameter);
                    Command.ErrorCode = ErrorCodeEnum.WrongParameter;
                    b = false;
                }
            }
            return b;
        }
    }

    public interface IServiceData
    {
        void Execute(ComlineData command);
    }
}
