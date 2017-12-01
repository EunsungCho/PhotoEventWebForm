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
    /// <summary>
    /// This class is for showing Event details to users
    /// </summary>
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
                DataBindGrid(eventId);          // showing Participants information such uploaded picture and 
            }
        }

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

            btnShowImage.Attributes.Add("OnClick", "return popup('" + eventId + "', null);");
        }

        private void DataBindGrid(string eid)
        {
            string select = @"select UserId, ThumbnailPhoto from EventUserPhotos where EventId = @eventId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                saveThumNailImageInList(reader);
            }

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
                SetWinnerColor(hdnWinner.Value);
            }
        }

        protected void btnReplace_Click(object sender, EventArgs e)
        {
            if (upImage.PostedFile.FileName == "" || upImage.PostedFile.ContentLength == 0)
            {
                Response.Write("<script>alert('No file to replace.');</script>");
                return;
            }
                
            if (Models.Event.updateIntroImage(int.Parse(eventId), upImage.PostedFile))
            {
                Response.Write("<script>alert('Succeeded to replace');</script>");
            }
            else
            {
                Response.Write("<script>alert('Failed to replace');</script>");
            }
        }

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

        protected void gvParticipants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "WINNER")
            {
                string winnerId = e.CommandArgument.ToString();
                string update = @"Update Events Set Winner = @winner Where EventId = @eid";
                Dictionary<string, string> pList = new Dictionary<string, string>();
                pList.Add("winner", winnerId);
                pList.Add("eid", eventId);
                if (Libs.DbHandler.updateData(update, pList))
                {
                    SetWinnerColor(winnerId);
                }
                else
                {
                    foreach (GridViewRow row in gvParticipants.Rows)
                    {
                        row.Cells[0].Attributes.Remove("style");                        
                    }
                }
            }
        }

        private void SetWinnerColor(string winnerId)
        {
            foreach (GridViewRow row in gvParticipants.Rows)
            {
                row.Cells[0].Attributes.Remove("style");
                ((Button)row.Cells[5].FindControl("btnWinner")).Enabled = true;
                if (row.Cells[0].Text == winnerId)
                {
                    row.Cells[0].Attributes.Add("style", "color: red;");
                    ((Button)row.Cells[5].FindControl("btnWinner")).Enabled = false;
                }
            }
        }

        private void saveThumNailImageInList(System.Data.SqlClient.SqlDataReader rd)
        {
            thumImageList = new List<string>();
            userIdList = new List<string>();

            //if (rd.HasRows && rd.Read())
            while(rd.Read())
            {
                byte[] bytes = (byte[])rd["ThumbnailPhoto"];
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                thumImageList.Add(base64String);
                userIdList.Add(rd["UserId"].ToString());
            }
        }

        protected void gvParticipants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DataRowView rv = (DataRowView)e.Row.DataItem;
                foreach (ImageButton btn in e.Row.Cells[3].Controls.OfType<ImageButton>())
                {
                    btn.ImageUrl = "data:image/png;base64," + thumImageList[e.Row.RowIndex];
                    //btn.Attributes.Add("onclick", "return popup(" + rv["EventId"].ToString() + ", " + rv["UserId"].ToString() + ");");
                    btn.Attributes.Add("onclick", "return popup('" + eventId + "', '" + userIdList[e.Row.RowIndex] + "');");
                }

                foreach (Button btn in e.Row.Cells[5].Controls.OfType<Button>())
                {
                    btn.Attributes.Add("onclick", "return confirm('Are you going to set winner to " + e.Row.Cells[0].Text + "?');");
                }
            }
        }

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