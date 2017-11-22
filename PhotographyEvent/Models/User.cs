using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace PhotographyEvent.Models
{
    public class User
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public Boolean isAdmin { get; set; }
        public string password { get; set; }
        public static string adminKeyCode { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["adminkeycode"]; } }

        public User(string userId, string password, string email)
        {
            this.userId = userId;
            this.password = password;
            this.emailAddress = email;
        }

        public User(string userId, string password, string email, string firstName, string lastName, Boolean isAdmin) : this(userId, password, email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.isAdmin = isAdmin;
        }

        public User getUser(string newUserId)
        {
            string selectSql = "select * from Users where userId = @findingId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("findingId", newUserId);
            using (SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(selectSql, pList))
            {
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        User newUser = new User(newUserId, reader["password"].ToString(), reader["emailAddress"].ToString(),
                            reader["firstName"] == null ? null : reader["emailAddress"].ToString(),
                            reader["lastName"] == null ? null : reader["lastName"].ToString(),
                            Convert.ToBoolean(reader["isAdmin"].ToString()));
                        return newUser;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static Boolean CheckId(string userId)
        {
            string findsql = "select userId from Users Where userId = @userId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("userId", userId);
            using (SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(findsql, pList))
            {
                if (reader != null && reader.HasRows == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public Boolean CreateUser()
        {
            string updateSql = "INSERT INTO USERS(userId, password, emailAddress) VALUES(@userId, @password, @emailAddress)";
            List<SqlParameter> pList = new List<SqlParameter>();
            pList.Add(new SqlParameter("userId", this.userId));
            pList.Add(new SqlParameter("password", this.password));
            pList.Add(new SqlParameter("emailAddress", this.emailAddress));

            return Libs.DbHandler.updateData(updateSql, pList);
        }

        public Boolean CreateAdminUser(string keyCode)
        {
            if (adminKeyCode != keyCode)
                return false;

            string updateSql = "INSERT INTO USERS(userId, password, emailAddress, IsAdmin) VALUES(@userId, @password, @emailAddress, @isAdmin)";
            List<SqlParameter> pList = new List<SqlParameter>();
            pList.Add(new SqlParameter("userId", this.userId));
            pList.Add(new SqlParameter("password", this.password));
            pList.Add(new SqlParameter("emailAddress", this.emailAddress));
            pList.Add(new SqlParameter("isAdmin", this.isAdmin));

            return Libs.DbHandler.updateData(updateSql, pList);
        }

        public Boolean UpdateUser(string emailAddress, string firstName, string lastName)
        {
            return true;
        }

        public static Boolean AuthenticateUser(string userId, string password)
        {
            List<SqlParameter> pList = new List<SqlParameter>();
            string select = "Select Password From Users Where userId = @userid";
            pList.Add(new SqlParameter("@userid", userId));
            using (SqlDataReader reader = Libs.DbHandler.getResultAsDataReader(select, pList))
            {
                if (reader != null && reader.HasRows == true)
                {
                    if (reader.Read())
                    {
                        if (password == reader["Password"].ToString())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static User[] getAllUsers()
        {
            return null;
        }

        public static string getAdminKeyCode()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["adminkeycode"];
        }

        public static Boolean isAdministrator(string userId)
        {
            string select = "Select IsAdmin From Users where userid = @findingId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("findingId", userId);
            using (SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader != null && reader.HasRows)
                {
                    reader.Read();
                    return Convert.ToBoolean(reader["IsAdmin"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }
    }
}