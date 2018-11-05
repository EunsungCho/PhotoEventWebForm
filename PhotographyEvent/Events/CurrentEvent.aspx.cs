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
    
    // This page shows current Event details and enables user to upload photo
    public partial class CurrentEvent : System.Web.UI.Page
    {
        private string curEventId = string.Empty;
        private List<string> thumImageList;
        private List<string> userIdList;

        protected void Page_Load(object sender, EventArgs e)
        {
            curEventId = getCurrentEventId();   // gets current event id to use in selection
            if (!IsPostBack)
            {                
                ShowPreviewImageAndEventRule(curEventId);
                BindUserPhotos(curEventId);
            }
            btnUpdateTitle.Enabled = !btnUpload.Enabled;
        }

        // Uploads user's photo
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Stream fs = fuPhoto.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);  // gets the bytes array to store in database(original size photo)

                // to make thumbnail image...
                System.Drawing.Image tmpImg = System.Drawing.Image.FromStream(fs);
                int width = tmpImg.Width;
                int height = tmpImg.Height;

                // gets the size of thumbnale photo, it fixes height to 50px and makes thumbnale image to show in grid
                System.Drawing.Image thumImg = tmpImg.GetThumbnailImage((int)((float)width / (float)height * 50), 50, null, IntPtr.Zero);
                Byte[] thumBytes = null;

                // reads image into memory stream
                using (MemoryStream ms = new MemoryStream())
                {
                    thumImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    thumBytes = ms.ToArray();
                }

                string sql = @"insert into EventUserPhotos(EventId, UserId, ThumbnailPhoto, Photo, PhotoTitle)
                            values(@eventId, @userId, @thumbPhoto, @photo, @title)";
                List<SqlParameter> ps = new List<SqlParameter>();
                ps.Add(new SqlParameter("eventId", curEventId));
                ps.Add(new SqlParameter("userId", User.Identity.Name));
                ps.Add(new SqlParameter("title", txtPhotoTitle.Text.Trim()));

                // image column is of type VarBinary.
                SqlParameter image = new SqlParameter("thumbPhoto", System.Data.SqlDbType.VarBinary);
                image.Value = thumBytes;
                ps.Add(image);

                image = new SqlParameter("photo", System.Data.SqlDbType.VarBinary);
                image.Value = bytes;
                ps.Add(image);

                if (Libs.DbHandler.updateData(sql, ps))
                {
                    BindUserPhotos(curEventId);
                }
            } catch (Exception ex)
            {
                // When error occurrs
                lblResult.Text = ex.Message;
                return;
            }
            
        }

        protected void gvEventUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // when user clicks Vote button
            if (e.CommandName == "VOTE")
            {
                // EventUserPhotos table's pk are eventId and userID and these are set to CommandArgument attribute
                string[] pks = e.CommandArgument.ToString().Split(new char[] { '|' });
                string update = @"update EventUserPhotos set UserIdToVote = @votedId where eventId = @eid and userId = @uid";
                Dictionary<string, string> pList = new Dictionary<string, string>();
                pList.Add("votedId", pks[1]);
                pList.Add("eid", pks[0]);
                pList.Add("uid", User.Identity.Name);
                if (Libs.DbHandler.updateData(update, pList))
                {
                    // to show user's vote
                    lblVoteResponse.ForeColor = System.Drawing.Color.RoyalBlue;
                    lblVoteResponse.Text = "You voted to " + Models.User.getUserFirstName(pks[1]);
                    BindUserPhotos(curEventId);
                }
            }
        }

        /// <summary>
        /// Gets the current event id from database
        /// </summary>
        /// <returns>current event id as string</returns>
        private string getCurrentEventId()
        {
            string select = @"SELECT TOP 1 EventId From Events Where IsClosed = 0";
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReader(select, null))
            {
                if (reader.Read())
                {
                    pnUpload.Visible = true;
                    return reader["EventId"].ToString();
                }
                else
                {
                    // when there is no current event, return empty string
                    pnUpload.Visible = false;
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Shows current event's preview image in the image control and event rule
        /// </summary>
        /// <param name="curEventId">event id to get current event information</param>
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
                    imgPreview.ImageUrl = "data:image/png;base64," + base64String;  // sets image to image control
                    lblRule.Text = reader["EventRule"].ToString();                  // sets EventRule data to Label control
                }
            }
        }       
        
        /// <summary>
        /// shows current event users Photo and relate information
        /// </summary>
        /// <param name="curEventId">current event id used to select current event</param>
        private void BindUserPhotos(string curEventId)
        {
            // first, select thumbnalePhoto data and UserID data and save them into memory temporarily
            string select = @"select a.EventId, a.UserId, a.ThumbnailPhoto, a.PhotoTitle
                                from EventUserPhotos As a                                
                                where a.EventId = @eventId order by a.UserId";
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("eventId", curEventId);
            using (System.Data.SqlClient.SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam(select, pList))
            {
                // saves thumbnalephoto and user id into memory temporarily
                saveThumNailImageInList(reader);
            }

            // current event users data selection and binding
            select = @"select a.EventId, a.UserId, c.FirstName, b.score, a.PhotoTitle, a.UserIdToVote
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
        /// saves thumbnail and userid into memory to be used when gvEventUsers_RowDataBound event
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

        // puts photo on the image button when data is bound to grid rows
        protected void gvEventUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (ImageButton btn in e.Row.Cells[2].Controls.OfType<ImageButton>())
                {
                    btn.ImageUrl = "data:image/png;base64," + thumImageList[e.Row.RowIndex];
                    // call javascripr to see photo in large size
                    btn.Attributes.Add("onclick", "return popup('" + curEventId + "', '" + userIdList[e.Row.RowIndex] + "');");
                    if (User.Identity.Name == userIdList[e.Row.RowIndex])   //
                    {
                        // disable upload button if user already uploaded
                        btnUpload.Enabled = false;
                        string photoTitle = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "photoTitle"));
                        txtPhotoTitle.Text = photoTitle;
                    }
                }

                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UserId")) == User.Identity.Name)
                {
                    string votedId = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UserIdToVote"));
                    if (votedId != null && votedId != string.Empty)
                    {
                        // if user has voted, show to whom user voted.
                        lblVoteResponse.Text = "You voted to " + Models.User.getUserFirstName(votedId);
                    }
                }                
            }
        }

        protected void btnUpdateTitle_Click(object sender, EventArgs e)
        {
            string update = @"update EventUserPhotos set PhotoTitle = @title where eventId = @eid and userId = @uid";
            string photoTitle = txtPhotoTitle.Text.Trim();
            Dictionary<string, string> pList = new Dictionary<string, string>();
            pList.Add("title", photoTitle);
            pList.Add("eid", curEventId);
            pList.Add("uid", User.Identity.Name);

            if (Libs.DbHandler.updateData(update, pList))
            {
                lblTitleResult.Text = "Title updated!";
                BindUserPhotos(curEventId);
            }
            else
            {
                lblTitleResult.Text = "Title update failed!";
            }
        }
    }
}