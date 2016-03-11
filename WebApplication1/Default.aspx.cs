using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLayer;


namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        DB db = new DB();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            TextBoxStartDate.Attributes.Add("readonly", "readonly");
            TextBoxEndDate.Attributes.Add("readonly", "readonly");

            if (!IsPostBack)
            {
                GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text);
                GridView1.DataBind();
            }
        }


        protected void submitButton_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text);
            GridView1.DataBind();
            ViewState["SortedGrid"] = null;
         }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            if (ViewState["SortedGrid"] != null)
            {
                GridView1.DataSource = ViewState["SortedGrid"];
                
            }   
            else
            {   
                GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text); 
            }
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            
            if (ViewState["SortDirection"] != null && ViewState["SortDirection"].ToString() == SortDirection.Descending.ToString())
            {
                ViewState["SortDirection"] = SortDirection.Ascending;
                GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderByDescending(o => o.GetType().GetProperty(e.SortExpression).GetValue(o, null)).ToList();
                ViewState["SortedGrid"] = GridView1.DataSource;
            }
            else
            {
                ViewState["SortDirection"] = SortDirection.Descending;
                GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderBy(o => o.GetType().GetProperty(e.SortExpression).GetValue(o, null)).ToList();
                ViewState["SortedGrid"] = GridView1.DataSource;
            }
            GridView1.DataBind();
            
           
        }
    }
}