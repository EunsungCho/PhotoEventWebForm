using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PhotographyEvent.Models
{
    public class Event
    {
        private int eventId;
        public int EventID { get { return eventId; } }
        public string EventName { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public Boolean IsClosed { get; set; }
        public byte[] PreviewImage { get; set; }
        //public byte[] ThumbPrevImage { get; set; }
        public string EventRule { get; set; }

        public Event(string eventName, string eventRule, string startDate, string endDate, byte[] pImage)
        {
            this.EventName = eventName;
            this.EventRule = eventRule;
            this.EventStartDate = startDate;
            this.EventEndDate = endDate;
            this.PreviewImage = pImage;
        }

        public Boolean CreateNewEvent()
        {
            string createSql = "insert into Events(EventName, StartDate, EndDate, IntroImage, EventRule) " +
                "values(@eventName, @eventStartDate, @eventEndDate, @pImage, @eventRule)";
            List<SqlParameter> pList = new List<SqlParameter>();
            pList.Add(new SqlParameter("eventName", this.EventName));
            pList.Add(new SqlParameter("eventRule", this.EventRule));
            pList.Add(new SqlParameter("eventStartDate", this.EventStartDate));
            pList.Add(new SqlParameter("eventEndDate", this.EventEndDate));
            SqlParameter imageParam = new SqlParameter("pImage", SqlDbType.VarBinary);
            if (this.PreviewImage == null)
            {
                imageParam.Value = DBNull.Value;
            }
            else
            {
                imageParam.Value = this.PreviewImage;
            }
            
            pList.Add(imageParam);
            return Libs.DbHandler.updateData(createSql, pList);
        }

        public static Boolean updateIntroImage(int eventId, HttpPostedFile pfile)
        {
            Stream fs = pfile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            string updateSql = "update Events set introImage = @introImage where eventId = @id";
            List<SqlParameter> pList = new List<SqlParameter>();
            SqlParameter imgParam = new SqlParameter("introImage", SqlDbType.VarBinary);
            imgParam.Value = bytes;
            pList.Add(imgParam);
            pList.Add(new SqlParameter("id", eventId));
            return Libs.DbHandler.updateData(updateSql, pList);
        }       
    }
}