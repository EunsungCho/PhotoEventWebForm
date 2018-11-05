using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.Administration
{
    
    // This page shows users enrolled in our web site and their acrivities
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // shows users list
                UsersBind();
            }
        }

        // When userid is clicked, it shows user's activity below the table such as the event the user won
        // the event the user participated in
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userId = e.CommandArgument.ToString();
            SetUserEventsGrid(userId);
            lblUser.Text = userId + @"'s participation information";
        }

        /// <summary>
        /// Gets users list and bind to the table
        /// </summary>
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

        /// <summary>
        /// shows user-related data: event the user won, event the user took part in
        /// </summary>
        /// <param name="uid">user id to find activities</param>
        private void SetUserEventsGrid(string uid)
        {
            // finding event user have won
            string select = @"SELECT EventId, EventName From Events Where Winner = @userId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("userId", uid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvWonEvent.DataSource = reader;
                gvWonEvent.DataBind();
            }

            // finds event the user participated
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