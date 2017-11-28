using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent
{
    public partial class CreateNewAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();
            string password = txtPassword.Text;
            string repassword = txtRetypePassword.Text;
            string emailAddr = txtEmail.Text.Trim();
            string adminKeyCode = txtAdminCode.Text.Trim();

            Models.User newUser = new Models.User(userId, password, emailAddr);
            Boolean result = false;

            if (chkAdmin.Checked)
            {
                newUser.isAdmin = true;
                result = newUser.CreateAdminUser(adminKeyCode);
            }
            else
            {
                if (hdIdChecked.Value == "OK")
                {
                    result = newUser.CreateUser();
                }
                else
                {
                    lblResult.Text = "Please choose other ID.";
                    return;
                }                
            }

            if (result)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "creation", "showSuccess();", true);
            }
            else
            {
                lblResult.Text = "There was an error in creating new user. Please try again.";
            }
        }

        protected void btnCheckId_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();
            if (userId == null || userId.Equals(string.Empty))
            {
                lblCheckId.Text = "Please type your ID";
                lblCheckId.Visible = true;
                hdIdChecked.Value = "";
                return;
            }

            if (Models.User.CheckId(userId))
            {
                lblCheckId.Text = "You can use this ID.";
                lblCheckId.Visible = true;
                hdIdChecked.Value = "OK";
            }
            else
            {
                lblCheckId.Text = "This id is being used by another user.";
                lblCheckId.Visible = true;
                txtUserId.Text = string.Empty;
                hdIdChecked.Value = "";
            }
        }
    }
}