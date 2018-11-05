using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.Events
{
    
    // This page shows user's photo in real size on popup window
    public partial class PopupPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // gets event id and user id from request object
            string eventId = Request.QueryString["eid"];
            string userId = Request.QueryString["uid"];

            if (eventId == null || userId == null)
                return;

            string select = @"Select PhotoTitle, Photo From EventUserPhotos Where EventId = @eid and UserId = @uid";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eid", eventId);
            pList.Add("uid", userId);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader.Read())
                {
                    byte[] bytes = (byte[])reader["Photo"];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    imgPhoto.ImageUrl = "data:image/png;base64," + base64String;
                    imgPhoto.Attributes.Add("alt", reader["PhotoTitle"].ToString());
                    lblPhotoTitle.Text = reader["PhotoTitle"].ToString();
                }
            }
        }
    }
}