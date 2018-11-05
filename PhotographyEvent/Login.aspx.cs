using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;

namespace PhotographyEvent
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Authentication
        protected void btnLogin_Click(object sender, EventArgs e)
        {            
            string userId = txtUserId.Text.Trim();
            string pass = txtPassword.Text.Trim();
            if (Models.User.AuthenticateUser(userId, pass))
            {
                // user id authenticated
                if (Models.User.isAdministrator(userId))
                {
                    // administrator logged in, so redirect to administator's main page, EventList
                    FormsAuthentication.SetAuthCookie(userId, true);
                    Response.Redirect("~/Administration/EventsList.aspx");
                }
                else
                {
                    // general user authenticated, so issues authentication ticket and redirect
                    FormsAuthentication.RedirectFromLoginPage(userId, true);
                }
                
            }
            else
            {
                lblWarnning.Text = "Your User ID or password is wrong. Please try again!";
            }
        }
    }
}