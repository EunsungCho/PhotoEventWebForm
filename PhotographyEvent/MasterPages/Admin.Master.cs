using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.MasterPages
{
    
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Models.User.isAdministrator(Page.User.Identity.Name))
            {
                // When user is not adminitrator, redirects to User Main page
                Response.Redirect("~/Main.aspx");
            }
        }
    }
}