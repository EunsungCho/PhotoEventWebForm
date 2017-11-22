using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;

namespace PhotographyEvent.Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Dictionary<string, string> p = new Dictionary<string, string>();
            //p.Add("id", "10");
            //using (SqlDataReader reader = Libs.DbHandler.getResultAsDataReaderDicParam("select * from Test where id < @id", p))
            //{
            //    testGrid.DataSource = reader;
            //    testGrid.DataBind();
            //}
            using (System.Data.DataSet ds = Libs.DbHandler.getResultAsDataSetDicParam("select * from Test", null))
            {
                testGrid.DataSource = ds;
                testGrid.DataBind();                
            }
        }

        protected void btnDataCreate_Click(object sender, EventArgs e)
        {
            SqlParameter p = new SqlParameter();
            string sql = "insert into Test values(@col1, @col2, @col3, @col4, @col5)";
            
            for(int i = 1; i <= 100; i++)
            {
                List<SqlParameter> pList = new List<SqlParameter>();
                for (int j = 1; j <= 5; j++)
                {
                    pList.Add(new SqlParameter("col" + j.ToString(), "Value" + i.ToString() + "," + j.ToString()));
                }
                if (Libs.DbHandler.updateData(sql, pList) == false)
                {
                    Response.Write("Loop " + i.ToString() + " has error. Stop inserting.");
                    break;
                }
            }
            Response.Write("Insert completed!");
        }

        protected void btnAuth_Click(object sender, EventArgs e)
        {
            Response.Write("AuthenticateUser: " + Models.User.AuthenticateUser("escho", "111"));
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Models.User newUser = new Models.User("escho", "111", "test@email.com");
            Response.Write(newUser.CreateUser());
        }

        protected void btnCreateEvent_Click(object sender, EventArgs e)
        {
            Models.Event newEvent = new Models.Event("test Event", "event Rule", "19/11/2017", "30/11/2017", null);
            Boolean result = newEvent.CreateNewEvent();
            if (result)
                Response.Write("Create New Event Succeeded!");
            else
                Response.Write("Create New Event Failed!");
        }

        protected void btnReplace_Click(object sender, EventArgs e)
        {
            Boolean result = Models.Event.updateIntroImage(1, upImage.PostedFile);
            if (result)
            {
                Response.Write("Image Replace succeeded!");
            }
            else
            {
                Response.Write("Image replace failed!");
            }
        }

        protected void testGrid_PageIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void testGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.NewPageIndex = testGrid.PageIndex + 1;
            using (System.Data.DataSet ds = Libs.DbHandler.getResultAsDataSetDicParam("select * from Test", null))
            {
                testGrid.DataSource = ds;
                testGrid.DataBind();
            }
        }
    }
}