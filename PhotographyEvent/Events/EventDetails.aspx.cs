using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace PhotographyEvent.Events
{
    public partial class EventDetails : System.Web.UI.Page
    {
        private string curEventId = string.Empty;
        private List<string> thumImageList;
        private List<string> userIdList;

        protected void Page_Load(object sender, EventArgs e)
        {
            curEventId = Request.QueryString["eid"];
            if (!IsPostBack)
            {
                ShowPreviewImageAndEventRule(curEventId);
                BindUserPhotos(curEventId);
            }

        }        

        protected void gvEventUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }
        
        private void ShowPreviewImageAndEventRule(string curEventId)
        {
            string select = @"Select IntroImage, EventRule From Events Where EventId = @eventId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", curEventId);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader.Read())
                {
                    byte[] bytes = (byte[])reader["IntroImage"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    imgPreview.ImageUrl = "data:image/png;base64," + base64String;
                    lblRule.Text = reader["EventRule"].ToString();
                }
            }
        }

        private void BindUserPhotos(string curEventId)
        {
            string select = @"select a.EventId, a.UserId, a.ThumbnailPhoto, a.PhotoTitle
                                from EventUserPhotos As a                                
                                where a.EventId = @eventId order by a.UserId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", curEventId);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                saveThumNailImageInList(reader);
            }

            select = @"select a.EventId, a.UserId, c.FirstName, b.score, a.PhotoTitle
                                from EventUserPhotos As a
                                left outer join 
                                (select EventId, UserIdToVote, count(UserIdToVote) as score
                                from EventUserPhotos group by EventId, UserIdToVote) as b
                                on a.EventId = b.EventId and a.UserId = b.UserIdToVote
                                inner join Users as C on a.UserId = c.UserId
                                where a.EventId = @eventId
                                order by a.UserId";
            pList = new Dictionary<string, string>();
            pList.Add("eventId", curEventId);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                gvEventUsers.DataSource = reader;
                gvEventUsers.DataBind();
            }
        }

        private void saveThumNailImageInList(SqlDataReader rd)
        {
            thumImageList = new List<string>();
            userIdList = new List<string>();
            while (rd.Read())
            {
                byte[] bytes = (byte[])rd["ThumbnailPhoto"];
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                thumImageList.Add(base64String);
                userIdList.Add(rd["UserId"].ToString());
            }
        }

        protected void gvEventUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DataRowView rv = (DataRowView)e.Row.DataItem;
                foreach (ImageButton btn in e.Row.Cells[2].Controls.OfType<ImageButton>())
                {
                    btn.ImageUrl = "data:image/png;base64," + thumImageList[e.Row.RowIndex];
                    //btn.Attributes.Add("onclick", "return popup(" + rv["EventId"].ToString() + ", " + rv["UserId"].ToString() + ");");
                    btn.Attributes.Add("onclick", "return popup('" + curEventId + "', '" + userIdList[e.Row.RowIndex] + "');");
                }
            }
        }
    }
}