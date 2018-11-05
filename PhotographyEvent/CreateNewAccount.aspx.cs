using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent
{
    
    // Page to create new user account
    public partial class CreateNewAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        // create new account with userid, password and email address
        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();
            string password = txtPassword.Text;
            string repassword = txtRetypePassword.Text;
            string emailAddr = txtEmail.Text.Trim();
            string adminKeyCode = txtAdminCode.Text.Trim();

            Models.User newUser = new Models.User(userId, password, emailAddr);
            // check availiability of email addres before creation of new user
            if (!Models.User.CheckEmail(emailAddr))
            {
                lblResult.Text = "Email addres you input is being used by other user. Please choose other email address.";
                return;
            }

            Boolean result = false;

            if (chkAdmin.Checked)
            {
                // when creating new administrator
                newUser.isAdmin = true;
                result = newUser.CreateAdminUser(adminKeyCode);
            }
            else
            {   // when creating general user
                if (hdIdChecked.Value == "OK")
                {
                    // id check is necessary to create user
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
                // user creation succeeded, so send email
                string mailSubject = "PhotographyEvent User Created";
                string mailMesage = "Hi " + userId + " "
                        + "\n\nYour account has been created successfully.\n\nPlease remember your login id and you are welcome to our event. "
                        + "\n\n Thank you!";
                
                Libs.Email.SendMailTo(mailSubject, mailMesage, emailAddr);
                ClientScript.RegisterStartupScript(this.GetType(), "creation", "showSuccess();", true);
            }
            else
            {
                lblResult.Text = "There was an error in creating new user. Please try again.";
            }
        }

        // check the availibity of new user id
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