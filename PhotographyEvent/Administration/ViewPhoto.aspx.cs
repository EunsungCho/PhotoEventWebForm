using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhotographyEvent.Administration
{
    public partial class ViewPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string eventId = Request.QueryString["eid"];
            string userId = Request.QueryString["uid"];

            //if (eventId == null || userId == null)
            //    return;

            string select = string.Empty;
            string photoColName = string.Empty;
            string titleColName = string.Empty;
            Dictionary<string, string> pList = new Dictionary<string, string>();
            if (userId == "null")
            {
                select = @"Select EventName, IntroImage From Events Where EventId = @eid";
                photoColName = "IntroImage";
                titleColName = "EventName";
            }
            else
            {
                select = @"Select PhotoTitle, Photo From EventUserPhotos Where EventId = @eid and UserId = @uid";
                photoColName = "Photo";
                titleColName = "PhotoTitle";
                pList.Add("uid", userId);
            }
                        
            pList.Add("eid", eventId);
            
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                if (reader.Read())
                {
                    byte[] bytes = (byte[])reader[photoColName];
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    imgPhoto.ImageUrl = "data:image/png;base64," + base64String;
                    imgPhoto.Attributes.Add("alt", reader[titleColName].ToString());
                    lblPhotoTitle.Text = reader[titleColName].ToString();
                }
            }
        }
    }
}