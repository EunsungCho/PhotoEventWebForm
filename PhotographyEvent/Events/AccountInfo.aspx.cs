using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.Events
{
    
    // Page for modification of user information: password, first name, last name, email address.
    public partial class AccountInfo : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // When the page loads for the first time, shows user information stored in db
                BindData();
            }            
        }

        // Event when user tries to save altered data
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // firstly, check email address validity.
            if (!Models.User.CheckEmail(txtEmail.Text.Trim(), User.Identity.Name))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Email addres you input is being used by other user. Please choose other email address.');", true);
                return;
            }

            // updates user information to the db
            string update = @"update Users set EmailAddress = @email, FirstName = @fName, LastName = @lName
                                where UserId = @uid";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("email", txtEmail.Text.Trim());
            pList.Add("fName", txtFirstName.Text.Trim());
            pList.Add("lName", txtLastName.Text.Trim());
            pList.Add("uid", User.Identity.Name);

            // saves data and shows the result
            if (Libs.DbHandler.updateData(update, pList))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Saved successfully'); location.href='../Main.aspx';", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Failed to save');", true);
            }
        }

        // Changes user's password
        protected void btnChngPass_Click(object sender, EventArgs e)
        {
            string update = @"update Users set password = @pword
                                where UserId = @uid";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("pword", txtPassword.Text);
            pList.Add("uid", User.Identity.Name);

            // updates password and shows result
            if (Libs.DbHandler.updateData(update, pList))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Changed successfully');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Failed to change password');", true);
            }
        }

        /// <summary>
        /// Retrieves and shows user information in text boxes
        /// </summary>
        private void BindData()
        {
            string select = @"Select Password, FirstName, LastName, EmailAddress From Users Where UserId = @uid and IsAdmin = 0";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("uid", User.Identity.Name);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader.Read())
                {
                    lblUserId.Text = User.Identity.Name;    // User Id is fixed.
                    txtPassword.Text = reader["Password"].ToString();
                    txtRetypePass.Text = reader["Password"].ToString();
                    txtEmail.Text = reader["EmailAddress"].ToString();
                    txtFirstName.Text = reader["FirstName"] == DBNull.Value ? string.Empty : reader["FirstName"].ToString();
                    txtLastName.Text = reader["LastName"] == DBNull.Value ? string.Empty : reader["LastName"].ToString();
                }
            }
        }
    }
}