using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeTimeSheetWebPart
{
    [ToolboxItemAttribute(false)]
    public class EmployeeTimeSheetWebPart : WebPart
    {

        private TextBox [] arrTxt;
        private Label[] arrLbl;
        private SPListItemCollection entries = null;
        private Button btnOk;
        private Button btnClose;
        private Button btnAddAcct;
        private Button btnSubmit;

        protected override void OnInit(EventArgs e)
        {
            arrTxt = new TextBox[45];
            arrLbl = new Label[45];
            base.OnInit(e);
        }

        public EmployeeTimeSheetWebPart()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Timesheets"];

                        //SPQuery query = new SPQuery();

                        //query.Query = "<Where><And>" +
                        //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Current</Value></Eq>" +
                        //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                        //              "</And></Where>";

                        //entries = list.GetItems(query);

                        entries = list.Items;
                    }
                }
            });
        }

        protected SPUser GetCurrentUser()
        {
            SPUser user;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    user = web.CurrentUser;
                }
            }

            return user;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (entries.Count > 0)
            {

                writer.Write("<table border='0' width='80%'>");

                writer.Write("<tr><td colspan='9'><div class='listSubTitle'>Employee TimeSheet</div></td></tr>");

                //print labels

                writer.Write("<tr>");

                for (int i = 0; i < 9; i++)
                {
                    writer.Write("<td align='center'>");
                    arrLbl[i].RenderControl(writer);
                    writer.Write("</td>");
                }

                writer.Write("</tr>");

                //print information

                for (int x = 0; x < entries.Count; x++)
                {
                    writer.Write("<tr>");

                    int startIndex = x == 0 ? 0 : 9 * x;
                    int endIndex = x == 0 ? 9 : 9 * ( x + 1 ) ; 

                    for (int y = startIndex; y < endIndex; y++)
                    {
                        writer.Write("<td align='center'>");
                        arrTxt[y].RenderControl(writer);
                        writer.Write("</td>");
                    }

                    writer.Write("</tr>");
                }

                

                writer.Write("</table>");
                writer.Write("<br><br>");

                writer.Write("<table border='0' width='80%'>");
                writer.Write("<tr><td>&nbsp;</td><td align='right'>");
                btnSubmit.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                btnAddAcct.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                btnOk.RenderControl(writer);
                writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                btnClose.RenderControl(writer);
                writer.Write("</td></tr>");
                writer.Write("</table>");
            }
        }

        protected override void CreateChildControls()
        {

            int i = 0;

            foreach (SPListItem entry in entries)
            {
                arrLbl[i] = new Label();
                arrLbl[i].Text = "Accounts";

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 60;
                arrTxt[i].Text = "Regular";

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day1Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay1");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day2Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay2");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day3Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay3");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day4Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay4");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day5Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay5");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day6Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay6");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = GetItemDateText(entry, "Day7Time");

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "_x0044_ay7");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;

                arrLbl[i] = new Label();
                arrLbl[i].Text = "Total Hours";

                arrTxt[i] = new TextBox();
                arrTxt[i].Width = 50;
                arrTxt[i].Text = GetItemText(entry, "Total_x0020_Hours");

                Controls.Add(arrLbl[i]);
                Controls.Add(arrTxt[i]);

                i++;
            }

            btnOk = new Button();
            btnOk.CssClass = "aspButtonClass";
            btnOk.Text = "Save";
            btnOk.Click += new EventHandler(btnOk_Click);

            btnClose = new Button();
            btnClose.Text = "Cancel";
            btnClose.CssClass = "aspButtonClass";
            btnClose.Click += new EventHandler(btnClose_Click);

            btnAddAcct = new Button();
            btnAddAcct.Text = "Add Account";
            btnAddAcct.CssClass = "aspButtonClass";
            btnAddAcct.Click += new EventHandler(btAddAcctk_Click);

            btnSubmit = new Button();
            btnSubmit.Text = "Submit";
            btnSubmit.CssClass = "aspButtonClass";
            btnSubmit.Click += new EventHandler(btnSubmit_Click);

            Controls.Add(btnOk);
            Controls.Add(btnClose);
            Controls.Add(btnAddAcct);
            Controls.Add(btnSubmit);
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        void btnOk_Click(object sender, EventArgs e)
        {

        }

        void btAddAcctk_Click(object sender, EventArgs e)
        {

        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url + "/_layouts/BJSmarts.ERP.HumanResources/SubmitTimesheet.aspx");
        }

        protected String GetItemText(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Item[colName].ToString();
                    return Text;
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }

        protected String GetItemDateText(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = DateTime.Parse(Item[colName].ToString()).ToShortDateString();
                    return Text;
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }
    }
}
