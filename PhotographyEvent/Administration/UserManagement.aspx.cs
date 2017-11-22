using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.Administration
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UsersBind();
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userId = e.CommandArgument.ToString();
            SetUserEventsGrid(userId);
        }

        private void UsersBind()
        {
            string select = @"select ROW_NUMBER() over(order by userId) as [No],
	                            UserId, FirstName, a.EmailAddress, CONVERT(CHAR(10), a.EntryDate, 103) As RegDate, a.IsAdmin
                            From Users as a";
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReader(select, null))
            {
                gvUsers.DataSource = reader;
                gvUsers.DataBind();
            }
        }

        private void SetUserEventsGrid(string uid)
        {
            string select = @"SELECT EventId, EventName From Events Where Winner = @userId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("userId", uid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvWonEvent.DataSource = reader;
                gvWonEvent.DataBind();
            }

            select = @"select A.EventId, A.EventName
                        from Events as A
                        Inner Join EventUserPhotos as B On a.EventId = B.EventId Where B.userId = @userId";
            pList = new Dictionary<string, string>();
            pList.Add("userId", uid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvParticipant.DataSource = reader;
                gvParticipant.DataBind();
            }
        }
    }
}