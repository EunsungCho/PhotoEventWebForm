using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace PhotographyEvent.Administration
{

    // This page shows all the events we held
    public partial class EventsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieveEventsList();   // retrieves and shows Event list data
            }
        }

        protected void btnRetieve_Click(object sender, EventArgs e)
        {
            retrieveEventsList();       // retrieves and shows Event list data
        }

        // creates and saves new event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string eventTitle = txtTitle.Text.Trim();
            string eventRule = txtRule.Text.Trim();
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;

            // saves image data into database
            Stream fs = upPrevImage.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            Models.Event newEvent = new Models.Event(eventTitle, eventRule, fromDate, toDate, bytes);
            if (newEvent.CreateNewEvent())
            {
                // creating new event, show list again
                retrieveEventsList();
            }
        }

        /// <summary>
        /// Retrieves and shows all events
        /// </summary>
        private void retrieveEventsList()
        {            
            string select = @"select ROW_NUMBER() OVER(ORDER BY A.EventId) AS RowNo,
                                a.EventId, a.EventName, (a.StartDate + ' ~ ' + a.EndDate) as EventDate,
                                a.winner, COUNT(b.UserId) as NoParticipants,
                                CASE a.IsClosed WHEN 1 THEN CONVERT(CHAR(3), 'YES') ELSE CONVERT(CHAR(2), 'NO') END AS IsClosed
                            from Events as a
                            left outer join EventUserPhotos as b
                            on a.EventId = b.EventId
                            group by a.EventId, a.EventName, a.StartDate, a.EndDate, a.winner, a.IsClosed";

            using (System.Data.DataSet ds = Libs.DbHandler.getResultAsDataSetDicParam(select, null))
            {
                gvEventsList.DataSource = ds;
                gvEventsList.DataBind();
            }
        }
    }
}