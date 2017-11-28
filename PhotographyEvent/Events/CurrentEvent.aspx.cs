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
    public partial class CurrentEvent : System.Web.UI.Page
    {
        private string curEventId = string.Empty;
        private List<string> thumImageList;
        private List<string> userIdList;

        protected void Page_Load(object sender, EventArgs e)
        {
            curEventId = getCurrentEventId();
            if (!IsPostBack)
            {                
                ShowPreviewImageAndEventRule(curEventId);
                BindUserPhotos(curEventId);
            }            
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Stream fs = fuPhoto.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                System.Drawing.Image tmpImg = System.Drawing.Image.FromStream(fs);
                int width = tmpImg.Width;
                int height = tmpImg.Height;
                string calWidth = "not available";
                try
                {
                    calWidth = ((int)((float)width / (float)height * 50)).ToString();
                }
                catch (Exception ex)
                {
                    calWidth = ex.Message;
                    Response.Write(calWidth);
                }

                System.Drawing.Image thumImg = tmpImg.GetThumbnailImage((int)((float)width / (float)height * 50), 50, null, IntPtr.Zero);
                Byte[] thumBytes = null;

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
                //ClientScript.RegisterStartupScript(this.GetType(), "fileError", "alert('Sorry, File error occurred. " + ex.Message + "')", true);
                lblResult.Text = ex.Message;
                return;
            }
            
        }

        protected void gvEventUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VOTE")
            {
                string[] pks = e.CommandArgument.ToString().Split(new char[] { '|' });
                string update = @"update EventUserPhotos set UserIdToVote = @votedId where eventId = @eid and userId = @uid";
                Dictionary<string, string> pList = new Dictionary<string, string>();
                pList.Add("votedId", pks[1]);
                pList.Add("eid", pks[0]);
                pList.Add("uid", User.Identity.Name);
                if (Libs.DbHandler.updateData(update, pList))
                {
                    lblVoteResponse.ForeColor = System.Drawing.Color.RoyalBlue;
                    lblVoteResponse.Text = "You voted to " + Models.User.getUserFirstName(pks[1]);
                    BindUserPhotos(curEventId);
                }
            }
        }

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
                    pnUpload.Visible = false;
                    return string.Empty;
                }
            }
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
                    if (User.Identity.Name == userIdList[e.Row.RowIndex])
                    {
                        btnUpload.Enabled = false;
                    }
                }

                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UserId")) == User.Identity.Name)
                {
                    string votedId = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "UserIdToVote"));
                    if (votedId != null && votedId != string.Empty)
                    {
                        lblVoteResponse.Text = "You voted to " + Models.User.getUserFirstName(votedId);
                    }
                }                
            }
        }
    }
}