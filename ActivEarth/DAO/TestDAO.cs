using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ActivEarth.DAO
{
    public class TestDAO
    {

        public static DataTable GetUserNames()
        {
            var sql = "select * from dbo.users";
            return GetDataTableFromSql(sql);

        }

        public static DataTable GetDataTableFromSql(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (
                    SqlConnection conn =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["DevDB"].ToString()))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(dt);
                    }
                }
                return dt;
            }
            catch
            {
                throw;
            }
        }
    }

}