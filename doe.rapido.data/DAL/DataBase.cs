using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace doe.rapido.data.DAL
{
    public class DataBase
    {
        private SqlConnection conn = new SqlConnection(@"<connection>");

        #region Public Methods
        public async Task ExecuteProcedure(string nameProc, SqlParameter[] paramProc)
        {
            using (SqlCommand cmd = new SqlCommand(nameProc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (paramProc != null && paramProc.Length > 0)
                {
                    cmd.Parameters.AddRange(paramProc);
                }

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task<DataSet> GetRecords(string nameProc, SqlParameter[] paramProc)
        {
            using (SqlCommand cmd = new SqlCommand(nameProc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (paramProc != null && paramProc.Length > 0)
                {
                    cmd.Parameters.AddRange(paramProc);
                }

                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();

                    conn.Open();
                    da.Fill(ds);
                    conn.Close();

                    return ds;
                }
            }
        }
        #endregion
    }
}
