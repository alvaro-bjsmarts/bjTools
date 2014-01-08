using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BJSmarts.ERP.HR.TimeSheet.WebParts.EmployeeTimeSheetWebPart
{
    [ToolboxItemAttribute(false)]
    public class EmployeeTimeSheetWebPart : WebPart
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
        private TextBox[] arrTxt;
        private Label[] arrLbl;
        private Label[] arrLblAccounts;
        private static int TotalInputBoxes;
        private SPListItemCollection entries = null;
        private Button btnOk;
        private Button btnClose;
        private Button btnAddAcct;
        private Button btnSubmit;
        private String ErrorMessage;

        public EmployeeTimeSheetWebPart()
        {
            try
            {
                
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Timesheets"];

                        SPQuery query = new SPQuery();

                        //query.Query = "<Where><And>" +
                        //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Current</Value></Eq>" +
                        //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                        //              "</And></Where>";

                        //entries = list.GetItems(query);

                        entries = list.Items;
                    }
                }
                

            }
            catch { }
        }

        protected override void OnInit(EventArgs e)
        {
            arrTxt = new TextBox[45];
            arrLbl = new Label[45];
            arrLblAccounts = new Label[45];
            base.OnInit(e);
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
            if ( entries != null && entries.Count > 0)
            {

                writer.Write("<table border='0' width='80%'>");

                writer.Write("<tr><td colspan='9'><div class='listSubTitle'>Employee TimeSheet</div></td></tr>");

                //print labels

                writer.Write("<tr>");

                for (int i = 0; i < 9; i++)
                {
                    writer.Write("<td align='center'><b>");
                    arrLbl[i].RenderControl(writer);
                    writer.Write("</b></td>");
                }

                writer.Write("</tr>");

                //print information

                for (int x = 0; x < entries.Count; x++)
                {
                    writer.Write("<tr>");

                    int startIndex = x == 0 ? 0 : 9 * x;
                    int endIndex = x == 0 ? 9 : 9 * (x + 1);

                    for (int y = startIndex; y < endIndex; y++)
                    {
                        
                        if (y == startIndex)
                        {
                            writer.Write("<td align='left'>");
                            arrLblAccounts[y].RenderControl(writer);
                            arrTxt[y].Text = "";
                        }
                        else
                        {
                            writer.Write("<td align='center'>");
                            arrTxt[y].RenderControl(writer);
                        }
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

            if (entries != null && entries.Count > 0)
            {

                foreach (SPListItem entry in entries)
                {
                    arrLbl[i] = new Label();
                    if (intLCID == 1033)
                    {
                        arrLbl[i].Text = "Accounts";
                    }
                    if (intLCID == 3082)
                    {
                        arrLbl[i].Text = "Cuentas";
                    }

                    arrLblAccounts[i] = new Label();
                    arrLblAccounts[i].Width = 100;
                    arrLblAccounts[i].Text = GetItemText(entry, "Account_x0020_Type");

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 60;
                    arrTxt[i].Text = ""; 

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrLblAccounts[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 0);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay1");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 1);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay2");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 2);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay3");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 3);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay4");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 4);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay5");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 5);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay6");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();
                    arrLbl[i].Text = GetItemDateText(entry, "WeekNumber", 6);

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "_x0044_ay7");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;

                    arrLbl[i] = new Label();                    

                    if (intLCID == 1033)
                    {
                        arrLbl[i].Text = "Total Hours";
                    }
                    if (intLCID == 3082)
                    {
                        arrLbl[i].Text = "Horas Totales";
                    }

                    arrTxt[i] = new TextBox();
                    arrTxt[i].Width = 50;
                    arrTxt[i].Text = GetItemText(entry, "Total_x0020_Hours");

                    Controls.Add(arrLbl[i]);
                    Controls.Add(arrTxt[i]);

                    i++;
                }

                TotalInputBoxes = i;

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
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            if (isValidForm())
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList list = web.Lists["Timesheets"];

                        SPQuery query = new SPQuery();

                        //query.Query = "<Where><And>" +
                        //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Current</Value></Eq>" +
                        //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                        //              "</And></Where>";

                        //SPListItemCollection items = list.GetItems(query);

                        SPListItemCollection items = list.Items;

                        for (int x = 0; x < items.Count; x++)
                        {
                            int startIndex = x == 0 ? 0 : 9 * x;
                            int endIndex = x == 0 ? 9 : 9 * (x + 1);

                            startIndex = startIndex + 1;
                            endIndex = endIndex - 1;

                            SPListItem item = items[x];

                            int z = 1;

                            for (int y = startIndex; y < endIndex; y++)
                            {
                                if (arrTxt[y].Text.Length > 0)
                                {
                                    item["_x0044_ay" + z] = arrTxt[y].Text;
                                }
                                else
                                {
                                    item["_x0044_ay" + z] = "";
                                }
                                z++;
                            }

                            item.Update();
                        }

                        web.AllowUnsafeUpdates = false;

                        Page.Response.Redirect(SPContext.Current.Web.Url + "/Pages/EmployeeTimesheetPage.aspx");

                    }
                }                
            }
        }

        void btAddAcctk_Click(object sender, EventArgs e)
        {

        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url + "/_layouts/BJSmarts.ERP.HR.TimeSheet/SubmitTimesheet.aspx");
        }

        bool isValidForm()
        {

            for (int i = 0; i < TotalInputBoxes; i++)
            {
                if (arrTxt[i].Text.Length > 0)
                {
                    int input;
                    if (!int.TryParse(arrTxt[i].Text.ToString(), out input))
                    {
                        ErrorMessage = "Invalid Numeric Data!.  Alphanumeric data is not allowed in this form.";
                        return false;
                    }
                }                
            }            
            
            return true;
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

        protected String GetItemDateText(SPListItem Item, String colName, int addDays)
        {
            if (Item != null)
            {
                try
                {

                    DateTime date = GetMondayWeek(int.Parse(GetItemText(Item, colName))).AddDays(addDays);

                    String DayOfWeek = date.DayOfWeek.ToString();

                    if (intLCID == 3082)
                    {
                        DayOfWeek = date.ToString("D", new CultureInfo("es-ES"));
                        DayOfWeek = DayOfWeek.Substring(0, DayOfWeek.IndexOf(','));
                    }
                    
                    String Text =  DayOfWeek + "<br>" + date.Month + "/" + date.Day;
                    return Text;
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }

        private static DateTime GetMondayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);

            DateTime StartDate;

            if (WeekNum > 1)
            {
                int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
                WeekNum = WeekNum - 1;
                StartDate = firstMonday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }
    }
}
