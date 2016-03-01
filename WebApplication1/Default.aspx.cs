using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBoxStartDate.Attributes.Add("readonly", "readonly");
            TextBoxEndDate.Attributes.Add("readonly", "readonly");
        }



        protected void submitButton_Click(object sender, EventArgs e)
        {
            //DateTime startDateTime;
            //DateTime endDateTime;

            //string startDate = null;
            //string endDate = null;
            //string ip = TextBox3.Text;

            //if( DateTime.TryParse(textBoxStartDate.Text,out startDateTime))
            //{
            //    startDate = startDateTime.ToShortDateString();
            //}

            //if (DateTime.TryParse(textBoxEndDate.Text, out endDateTime))
            //{
            //    endDate = endDateTime.ToShortDateString();
            //}
            //if (TextBox3.Text == "")
            //{
            //    ip = null;
            //}
            //SqlDataSource2.SelectParameters.Clear();
            //SqlDataSource2.SelectCommand = "selectProcedure";
            //SqlDataSource2.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            //SqlDataSource2.
            //SqlDataSource2.SelectParameters.Add("startDate", startDate);
            //SqlDataSource2.SelectParameters.Add("endDate", endDate);
            //SqlDataSource2.SelectParameters.Add("Ip", ip);
            //SqlDataSource2.SelectParameters.
         
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TextBoxStartDate_TextChanged(object sender, EventArgs e)
        {
            TextBoxStartDate.Attributes["on click"] = "";
        }
    }
}