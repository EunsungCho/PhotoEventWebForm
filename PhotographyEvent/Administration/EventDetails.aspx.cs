using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace PhotographyEvent.Administration
{
    // This class is for showing Event details to users
    public partial class EventDetails : System.Web.UI.Page
    {
        private List<string> thumImageList;     // variable for storing thumbnail image
        private List<string> userIdList;        // variable for storing userId to show who had participated this event
        string eventId;                         // specific event id to show event details

        protected void Page_Load(object sender, EventArgs e)
        {
            eventId = Request.QueryString["eventid"].ToString();    // gets the event id from request
            if (!IsPostBack)                    
            {                
                DataBindField(eventId);         // Detailed event information binding
                DataBindGrid(eventId);          // Showing participants information such as uploaded picture and number of votes
            }
        }

        /// <summary>
        /// Binds specific event data to each field
        /// </summary>
        /// <param name="eid">event id to get data</param>
        private void DataBindField(string eid)
        {
            string select = @"SELECT EventId, EventName, StartDate, EndDate, IsClosed, Winner, EventRule
                                From Events
                            Where EventId = @eventId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader != null & reader.HasRows)
                {
                    reader.Read();
                    txtTitle.Text = reader["EventName"].ToString();
                    txtRule.Text = reader["EventRule"].ToString();
                    txtStartDate.Text = reader["StartDate"].ToString();
                    txtToDate.Text = reader["EndDate"].ToString();
                    chClose.Checked = Convert.ToBoolean(reader["isClosed"]);
                    hdnWinner.Value = reader["Winner"].ToString();
                }
            }

            // Puts a javascript function to pop up a new window showing Event preview image 
            btnShowImage.Attributes.Add("OnClick", "return popup('" + eventId + "', null);");
        }

        /// <summary>
        /// Gets and shows participants's data
        /// </summary>
        /// <param name="eid"></param>
        private void DataBindGrid(string eid)
        {
            // First, gets userid and thumbnail photo data to store in the List varibles
            string select = @"select UserId, ThumbnailPhoto from EventUserPhotos where EventId = @eventId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                saveThumNailImageInList(reader);
            }

            // Second, gets more participants' data again to show in the table
            select = @"Select A.EventId, a.UserId, C.FirstName, A.PhotoTitle, ISNULL(D.cnt, 0) as NoOfVotes
                        from EventUserPhotos as A
                        inner join Events as B on A.EventId = B.EventId and A.EventId = @eventId
                        inner join Users as C on A.UserId = C.UserId
                        left outer join (select a.EventId, a.UserIdToVote, count(a.UserIdToVote) as cnt from EventUserPhotos as a
				                        group by a.EventId, a.UserIdToVote) as D
                        on A.EventId = D.EventId and A.UserId = D.UserIdToVote";
            pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvParticipants.DataSource = reader;
                gvParticipants.DataBind();
                SetWinnerColor(hdnWinner.Value);    // if winner exists, highlights it
            }
        }

        // Changes uploaded Event preview image
        protected void btnReplace_Click(object sender, EventArgs e)
        {
            // check if uploading file is empty or nothing
            if (upImage.PostedFile.FileName == "" || upImage.PostedFile.ContentLength == 0)
            {
                Response.Write("<script>alert('No file to replace.');</script>");
                return;
            }
                
            // replaces and show the result
            if (Models.Event.updateIntroImage(int.Parse(eventId), upImage.PostedFile))
            {
                Response.Write("<script>alert('Succeeded to replace');</script>");
            }
            else
            {
                Response.Write("<script>alert('Failed to replace');</script>");
            }
        }

        // updates modified data
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string eventName = txtTitle.Text.Trim();
            string eventRule = txtRule.Text.Trim();
            string startDate = txtStartDate.Text.Trim();
            string endDate = txtToDate.Text.Trim();

            string update = @"update Events Set EventName = @ename, StartDate = @sdate, EndDate = @edate, EventRule = @erule
                                Where eventId = @eid";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("ename", eventName);
            pList.Add("sdate", startDate);
            pList.Add("edate", endDate);
            pList.Add("erule", eventRule);
            pList.Add("eid", eventId);

            if (Libs.DbHandler.updateData(update, pList))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Saved successfully');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Failed to save');", true);
            }
        }

        // Table contains button setting winner and this event catches it and treats it
        protected void gvParticipants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "WINNER")  // catches when winner setting
            {
                string winnerId = e.CommandArgument.ToString(); // userId to set to winner
                string update = string.Empty;
                Dictionary<string, string> pList = new Dictionary<string, string>();

                string hdnValue = hdnWinner.Value.ToString();
                if (hdnValue.Equals(winnerId))
                {
                    update = @"Update Events Set Winner = NULL Where EventId = @eid";
                }
                else
                {                    
                    update = @"Update Events Set Winner = @winner Where EventId = @eid";
                    pList.Add("winner", winnerId);
                }
                
                pList.Add("eid", eventId);
                if (Libs.DbHandler.updateData(update, pList))
                {
                    //SetWinnerColor(winnerId);   // Highlights the winner set by admin
                    DataBindField(eventId);
                    DataBindGrid(eventId);
                }
                else
                {
                    foreach (GridViewRow row in gvParticipants.Rows)
                    {
                        row.Cells[0].Attributes.Remove("style");  // removes previous winner highlighting
                    }
                }
            }
        }

        /// <summary>
        /// puts a highlighting effect by adding style attribute to a winner when winner is set 
        /// </summary>
        /// <param name="winnerId">winner's user id to be highlighted</param>
        private void SetWinnerColor(string winnerId)
        {
            foreach (GridViewRow row in gvParticipants.Rows)
            {
                row.Cells[0].Attributes.Remove("style");    // removes previous style
                //((Button)row.Cells[5].FindControl("btnWinner")).Enabled = true; // if disabled, enable the button
                ((Button)row.Cells[5].FindControl("btnWinner")).Text = "Set";
                if (row.Cells[0].Text == winnerId)
                {
                    row.Cells[0].Attributes.Add("style", "color: red;");    // sets the color of winner's userid to red
                    //((Button)row.Cells[5].FindControl("btnWinner")).Enabled = false;    // diable button to avoid unnecessary post back
                    ((Button)row.Cells[5].FindControl("btnWinner")).Text = "Unset";
                }
            }
        }

        /// <summary>
        /// Uses memory variable to improve efficiency
        /// </summary>
        /// <param name="rd">data got from database</param>
        private void saveThumNailImageInList(System.Data.SqlClient.SqlDataReader rd)
        {
            thumImageList = new List<string>();
            userIdList = new List<string>();

            //if (rd.HasRows && rd.Read())
            while(rd.Read())
            {
                byte[] bytes = (byte[])rd["ThumbnailPhoto"];    //ThumbnailPhoto column is type of varbinary
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                thumImageList.Add(base64String);
                userIdList.Add(rd["UserId"].ToString());
            }
        }

        // when row dagt binding, puts thumbnail image on button and javascript function
        protected void gvParticipants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //for image button
                foreach (ImageButton btn in e.Row.Cells[3].Controls.OfType<ImageButton>())
                {
                    btn.ImageUrl = "data:image/png;base64," + thumImageList[e.Row.RowIndex];
                    //btn.Attributes.Add("onclick", "return popup(" + rv["EventId"].ToString() + ", " + rv["UserId"].ToString() + ");");
                    btn.Attributes.Add("onclick", "return popup('" + eventId + "', '" + userIdList[e.Row.RowIndex] + "');");
                }

                // puts javascript function to confirm to set winner
                foreach (Button btn in e.Row.Cells[5].Controls.OfType<Button>())
                {
                    btn.Attributes.Add("onclick", "return confirm('Are you going to set winner to " + e.Row.Cells[0].Text + "?');");
                }
            }
        }

        // Event closing check box event updates isClosed column of events table to mark event finished
        protected void chClose_CheckedChanged(object sender, EventArgs e)
        {
            string update = @"update events set isClosed = @isclosed where eventid = @eid";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("isclosed", chClose.Checked.ToString());
            pList.Add("eid", eventId);

            if (Libs.DbHandler.updateData(update, pList))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Saved successful');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "save", "alert('Failed to save');", true);
            }
        }
    }
}