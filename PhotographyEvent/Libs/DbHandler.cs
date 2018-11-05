using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PhotographyEvent.Libs
{
    
    // class for handling data processes
    public class DbHandler
    {
        // stores connection information in static variable from web.config file
        private static string connString = WebConfigurationManager.ConnectionStrings["PhotographyEventData"].ConnectionString;
        
        /// <summary>
        /// select data from db with the given sql and paramters
        /// </summary>
        /// <param name="select">sql expression to be used in selection of data</param>
        /// <param name="pList">parameter collection to be used in selection of data</param>
        /// <returns></returns>
        public static SqlDataReader getResultAsDataReader(string select, List<SqlParameter> pList)
        {
            // open the connection
            SqlConnection connection = new SqlConnection(connString);
            connection.Open();
            SqlCommand com = new SqlCommand(select, connection);
            if (pList != null)
            {
                foreach (SqlParameter p in pList)
                {  
                    // set selection parameters
                    com.Parameters.Add(p);
                }
            }

            // return SqlDataReader object
            return com.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// gets the result of query and returns as SqlDataReader
        /// </summary>
        /// <param name="select">select expression</param>
        /// <param name="pList">parameters collection of type Dictionary</param>
        /// <returns></returns>
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

        /// <summary>
        /// get result as dataset and return it
        /// </summary>
        /// <param name="select">select expression</param>
        /// <param name="pList">paramter collection of type List</param>
        /// <returns></returns>
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

        /// <summary>
        /// gets result as dataset
        /// </summary>
        /// <param name="select">select expression</param>
        /// <param name="pList">parameters collection of type Dictionary</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update function - returns true if succeeds or false if fails
        /// </summary>
        /// <param name="upSql">update expression</param>
        /// <param name="pList">paramters collection of type List</param>
        /// <returns></returns>
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

        /// <summary>
        /// update functionn - returns true if succeeds or false if fails
        /// </summary>
        /// <param name="upSql">update expression</param>
        /// <param name="pList">parameter collection of type Dictionary</param>
        /// <returns></returns>
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