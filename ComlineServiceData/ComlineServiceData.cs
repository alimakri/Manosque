using ComLineCommon;
using ComLineData;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComlineServices
{
    public class ServiceData
    {
        #region Properties
        protected ComlineData Command = new();

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
    }

    public interface IServiceData
    {
        void Execute(ComlineData command);
    }
}
