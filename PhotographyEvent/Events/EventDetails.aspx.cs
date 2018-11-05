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
    
    // This page shows event details selected.
    public partial class EventDetails : System.Web.UI.Page
    {
        private string curEventId = string.Empty;
        private List<string> thumImageList;
        private List<string> userIdList;

        protected void Page_Load(object sender, EventArgs e)
        {
            curEventId = Request.QueryString["eid"];    // gets target event id from request
            if (!IsPostBack)
            {
                // shows detail information
                ShowPreviewImageAndEventRule(curEventId);
                BindUserPhotos(curEventId);
            }

        }        

        protected void gvEventUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }
        
        /// <summary>
        /// show preview image and event rule on control
        /// </summary>
        /// <param name="curEventId">target event id to select preview image and rule</param>
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
                    imgPreview.ImageUrl = "data:image/png;base64," + base64String;  // put image on the image control
                    lblRule.Text = reader["EventRule"].ToString();  // shows event rule
                }
            }
        }

        /// <summary>
        /// shows user photo data on the table
        /// </summary>
        /// <param name="curEventId"></param>
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

        /// <summary>
        /// saves thumbnail photo and userid data into memory
        /// </summary>
        /// <param name="rd"></param>
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

        // When photo data is bound to grid, put thumbnail photo on the button control in table row
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