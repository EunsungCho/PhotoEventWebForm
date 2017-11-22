using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PhotographyEvent.Libs
{
    public class DbHandler
    {
        private static string connString = WebConfigurationManager.ConnectionStrings["PhotographyEventData"].ConnectionString;
        
        public static SqlDataReader getResultAsDataReader(string select, List<SqlParameter> pList)
        {
            SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            SqlCommand com = new SqlCommand(select, connection);
            if (pList != null)
            {
                foreach (SqlParameter p in pList)
                {
                    com.Parameters.Add(p);
                }
            }
            
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static SqlDataReader getResultAsDataReaderDicParam(string select, Dictionary<string, string> pList)
        {
            SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            SqlCommand com = new SqlCommand(select, connection);
            if (pList != null)
            {
                foreach (KeyValuePair<string, string> p in pList)
                {
                    SqlParameter sparam = new SqlParameter(p.Key, p.Value);
                    com.Parameters.Add(sparam);
                }
            }

            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DataSet getResultAsDataSet(string select, List<SqlParameter> pList)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand com = new SqlCommand(select, connection);
                if (pList != null)
                {
                    foreach (SqlParameter p in pList)
                    {
                        com.Parameters.Add(p);
                    }
                }

                SqlDataAdapter adp = new SqlDataAdapter(com);
                DataSet dSet = new DataSet();
                adp.Fill(dSet);
                return dSet;
            }
        }

        public static DataSet getResultAsDataSetDicParam(string select, Dictionary<string, string> pList)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand com = new SqlCommand(select, connection);
                if (pList != null)
                {
                    foreach (KeyValuePair<string, string> p in pList)
                    {
                        SqlParameter sparam = new SqlParameter(p.Key, p.Value);
                        com.Parameters.Add(sparam);
                    }
                }

                SqlDataAdapter adp = new SqlDataAdapter(com);
                DataSet dSet = new DataSet();
                adp.Fill(dSet);
                return dSet;
            }
        }

        public static Boolean updateData(string upSql, List<SqlParameter> pList)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand com = new SqlCommand(upSql, conn);
                if (pList != null)
                {
                    foreach (SqlParameter p in pList)
                    {
                        com.Parameters.Add(p);
                    }
                }
                int result = com.ExecuteNonQuery();
                if (result > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Boolean updateData(string upSql, Dictionary<string, string> pList)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand com = new SqlCommand(upSql, conn);                
                if (pList != null)
                {
                    foreach (KeyValuePair<string, string> p in pList)
                    {
                        SqlParameter sp = new SqlParameter(p.Key, p.Value);
                        com.Parameters.Add(sp);
                    }
                }
                int result = com.ExecuteNonQuery();
                if (result > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}