using ComLineCommon;
using ComLineData;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComlineServices
{
    public class ServiceData
    {
        #region Properties
        public ComlineData Command = new();

        // SQL SERVER
        protected SqlCommand Query = new();
        protected SqlDataAdapter Adapter = new();
        public static string ConnectionString = "";
        #endregion

        public ServiceData()
        {
            //Command = command;
            SqlConnection cnx = new()
            {
                ConnectionString = ConnectionString
            };
            cnx.Open();
            Query.Connection = cnx;
            Query.CommandType = System.Data.CommandType.Text;
            Adapter.SelectCommand = Query;
        }

        public virtual void Execute() { }
    }

    public interface IServiceData
    {
        void Execute();
    }
}
