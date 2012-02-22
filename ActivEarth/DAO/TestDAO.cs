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
            const string sql = "select * from dbo.users";
            return GetDataTableFromSql(sql);
        }

        public static DataTable GetUserDetails(string username, string password)
        {
            var sql = "select * from profile p inner join users u on p.user_id = u.id where user_name = '" + username + "' and password = '" + password +
            "'";
            return GetDataTableFromSql(sql);
        }

        private static DataTable GetDataTableFromSql(string sql)
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