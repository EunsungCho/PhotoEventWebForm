using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PhotographyEvent.Administration
{
    public partial class EventDetails : System.Web.UI.Page
    {
        private List<string> thumImageList;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string eventId = Request.QueryString["eventid"].ToString();
                DataBindField(eventId);
                DataBindGrid(eventId);
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
                }
            }
        }

        private void DataBindGrid(string eid)
        {
            string select = @"select ThumbnailPhoto from EventUserPhotos where EventId = @eventId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                saveThumNailImageInList(reader);
            }

            select = @"Select A.EventId, a.UserId, C.FirstName, A.PhotoTitle, D.cnt as NoOfVotes
                        from EventUserPhotos as A
                        right outer join Events as B on A.EventId = B.EventId and A.EventId = @eventId
                        inner join Users as C on A.UserId = C.UserId
                        inner join (select a.EventId, a.UserIdToVote, count(a.UserIdToVote) as cnt from EventUserPhotos as a
				                        group by a.EventId, a.UserIdToVote) as D
                        on A.EventId = D.EventId and A.UserId = D.UserIdToVote";
            pList = new Dictionary<string, string>();
            pList.Add("eventId", eid);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvParticipants.DataSource = reader;
                gvParticipants.DataBind();
            }
        }

        protected void btnReplace_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void gvParticipants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        private void saveThumNailImageInList(System.Data.SqlClient.SqlDataReader rd)
        {
            thumImageList = new List<string>();
            while (rd.Read())
            {
                byte[] bytes = (byte[])rd["ThumbnailPhoto"];
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                thumImageList.Add(base64String);
            }
        }

        protected void gvParticipants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rv = (DataRowView)e.Row.DataItem;
                foreach (ImageButton btn in e.Row.Cells[3].Controls.OfType<ImageButton>())
                {
                    btn.ImageUrl = "data:image/png;base64," + thumImageList[e.Row.RowIndex];
                    btn.Attributes.Add("onclick", "return popup(" + rv["EventId"].ToString() + ", " + rv["UserId"].ToString() + ");");
                }
            }
        }
    }
}