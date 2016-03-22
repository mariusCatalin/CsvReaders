using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLayer;
using log4net;


namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        DB db = new DB();
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //Setez textbox-urile readonly
            TextBoxStartDate.Attributes.Add("readonly", "readonly");
            TextBoxEndDate.Attributes.Add("readonly", "readonly");

            try
            {
                if (!IsPostBack)
                {
                    log.Info("Se incarca pagina");
                    GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text);
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                log.Error("Au fost probleme la incarcarea paginii.", ex);
            }

        }

        //Metoda care se apeleaza la apasarea butonul Submit
        protected void submitButton_Click(object sender, EventArgs e)
        {
            log.Info("Se filtreaza gridul.");
            try
            {
                //Data source-ul grid-ului este lista de obiecte adusa din baza
                GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text);
                GridView1.DataBind();

                //Am creat un viewState care devine null la apasarea butonului. ViewState-ul il folosesc pentru sortarea dupa coloane;
                ViewState["SortedColumn"] = null;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Au fost probleme la cautare. Parametrii folositi: {0}, {1}, {2}", TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text), ex);
            }

        }

        //Metoda care se apeleaza la schimbarea paginii grid-ului. Folosesc ViewState pentru a pastra 
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Info(String.Format("Se schimba pagina gridului in {0}", e.NewPageIndex));
            try
            {
                if (ViewState["SortedColumn"] != null)
                {
                    if (ViewState["SortDirection"].ToString() == SortDirection.Ascending.ToString())
                        GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderByDescending(o => o.GetType().GetProperty(ViewState["SortedColumn"].ToString()).GetValue(o, null)).ToList();
                    else
                        GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderBy(o => o.GetType().GetProperty(ViewState["SortedColumn"].ToString()).GetValue(o, null)).ToList();
                }
                else
                {
                    GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text);
                }
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("Au fost probleme la schimbarea paginii gridului: ", ex);
            }



        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            
            log.Info(String.Format("Se face sortarea gridului dupa coloana: {0}", e.SortExpression));
            try
            {
                
                if (ViewState["SortDirection"] != null && ViewState["SortDirection"].ToString() == SortDirection.Descending.ToString())
                {
                    ViewState["SortDirection"] = SortDirection.Ascending;
                    GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderByDescending(o => o.GetType().GetProperty(e.SortExpression).GetValue(o, null)).ToList();
                    ViewState["SortedColumn"] = e.SortExpression;
                    GridView1.PageIndex = 0;
                }
                else
                {
                    ViewState["SortDirection"] = SortDirection.Descending;
                    GridView1.DataSource = db.readDatabase(TextBoxStartDate.Text, TextBoxEndDate.Text, TextBoxIp.Text).OrderBy(o => o.GetType().GetProperty(e.SortExpression).GetValue(o, null)).ToList();
                    ViewState["SortedColumn"] = e.SortExpression;
                    GridView1.PageIndex = 0;
                }
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Au fost probleme la sortarea gridului dupa coloana: {0}", e.SortExpression), ex);
            }



        }
    }
}