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
        protected static SqlConnection Cnx;
        protected SqlCommand Query = new();
        protected SqlDataAdapter Adapter = new();
        public static string ConnectionString = "";
        #endregion

        public ServiceData()
        {
            if (Cnx == null)
            {
                Cnx = new()
                {
                    ConnectionString = ConnectionString
                };
                Cnx.Open();
            }
            Query.Connection = Cnx;
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
