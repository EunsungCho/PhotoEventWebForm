using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.MasterPages
{
    
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Models.User.isAdministrator(Page.User.Identity.Name))
            {
                // when the user is administrator, redirects to administrator's main page
                Response.Redirect("~/Administration/EventsList.aspx");
            }
        }
    }
}