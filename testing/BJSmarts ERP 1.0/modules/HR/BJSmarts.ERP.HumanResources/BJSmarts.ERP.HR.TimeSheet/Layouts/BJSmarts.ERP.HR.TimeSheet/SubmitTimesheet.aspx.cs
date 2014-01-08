using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BJSmarts.ERP.HumanResources.Layouts.BJSmarts.ERP.HumanResources
{
    public partial class SubmitTimesheet : LayoutsPageBase
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadTimeSheetData();
            }
        }        

        void loadTimeSheetData()
        {

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["TimeSheets"];

                    SPQuery query = new SPQuery();

                    //query.Query = "<Where><And>" +
                    //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Current</Value></Eq>" +
                    //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                    //              "</And></Where>";

                    //SPListItemCollection items = list.GetItems(query);

                    SPListItemCollection items = list.GetItems();

                    int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
                                        
                    AddSimpleTableRowCell("Employee: ", web.CurrentUser.Name);
                    AddSimpleTableRowCell("Pay Period: ", GetStartDateWeek(CurrentWeek).AddDays(0).ToShortDateString() + " - " + GetEndDateWeek(CurrentWeek).ToShortDateString() );

                    TableRow newRow = new TableRow();

                    TableCell AccountText = new TableCell();
                    newRow.Cells.Add(AccountText);
                    AccountText.HorizontalAlign = HorizontalAlign.Center;
                    AccountText.Text = "<b>Accounts</b>";
                    
                    Timesheet.Rows.Add(newRow);
                    for (int i = 0; i < 7; i++)
                    {
                        TableCell newCell = new TableCell();
                        newRow.Cells.Add(newCell);                        
                        newCell.HorizontalAlign = HorizontalAlign.Center;

                        DateTime date = GetStartDateWeek(CurrentWeek).AddDays(i);

                        String DayOfWeek = date.DayOfWeek.ToString();

                        if (intLCID == 3082)
                        {
                            DayOfWeek = date.ToString("D", new CultureInfo("es-ES"));
                            DayOfWeek = DayOfWeek.Substring(0, DayOfWeek.IndexOf(','));
                        }

                        newCell.Text = "<b>" + DayOfWeek + "<br>" + date.Month + "/" + date.Day + "</b>";                                               
                    }

                    TableCell totalText = new TableCell();
                    newRow.Cells.Add(totalText);
                    totalText.HorizontalAlign = HorizontalAlign.Center;
                    totalText.Text = "<b>Total Hours</b>";

                    for (int i = 0; i < items.Count; i++)
                    {

                        TableRow newRow1 = new TableRow();

                        TableCell accountValue = new TableCell();
                        newRow1.Cells.Add(accountValue);
                        accountValue.HorizontalAlign = HorizontalAlign.Center;
                        accountValue.Text = items[i]["Account_x0020_Type"] != null ? items[i]["Account_x0020_Type"].ToString() : "";

                        Timesheet.Rows.Add(newRow1);
                        for (int x = 1; x <= 7; x++)
                        {
                            TableCell newCell = new TableCell();
                            newRow1.Cells.Add(newCell);
                            newCell.HorizontalAlign = HorizontalAlign.Center;
                            newCell.Text = items[i]["_x0044_ay" + x] != null ? items[i]["_x0044_ay" + x].ToString() : "";
                        }

                        TableCell totalValue = new TableCell();
                        newRow1.Cells.Add(totalValue);
                        totalValue.HorizontalAlign = HorizontalAlign.Center;
                        totalValue.Text = items[i]["Total_x0020_Hours"] != null ? items[i]["Total_x0020_Hours"].ToString() : "";

                    }
                }
            }                       
        }        

        void AddSimpleTableRowCell(String Text, String Value)
        {
            TableRow newRow = new TableRow();
            TableCell newCellText = new TableCell();
            TableCell newCellValue = new TableCell();

            EmployeeDetails.Rows.Add(newRow);
            newRow.Cells.Add(newCellText);
            newRow.Cells.Add(newCellValue);
            newCellText.Text = "<b>" + Text + "</b>";
            newCellValue.Text = Value;
        }

        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        static DateTime GetStartDateWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
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

        static DateTime GetEndDateWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);

            DateTime EndDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
                DateTime firstSunday = jan1.AddDays(daysOffset);
                EndDate = firstSunday.AddDays(WeekNum * 7);
            }
            else
            {
                int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
                DateTime firstSunday = jan1.AddDays(daysOffset);
                EndDate = firstSunday;
            }

            return EndDate;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists["TimeSheets"];

                    SPQuery query = new SPQuery();

                    //query.Query = "<Where><And>" +
                    //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Current</Value></Eq>" +
                    //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                    //              "</And></Where>";

                    //SPListItemCollection items = list.GetItems(query);

                    SPListItemCollection items = list.GetItems();

                    SPListItem item = items[0];

                    item["Status"] = "Submitted";
                    SPFieldUrlValue spFieldUrlValue = new SPFieldUrlValue();
                    spFieldUrlValue.Url = "/_layouts/BJSmarts.ERP.HR.TimeSheet/ApprovalRejectTimesheet.aspx?RecordID=" + item["ID"].ToString();
                    spFieldUrlValue.Description = "Approve/Reject";
                    item["ActionLink1"] = spFieldUrlValue;

                    item.Update();

                    web.AllowUnsafeUpdates = false;

                    SendEmailNotification();
                }
            }

            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        void SendEmailNotification()
        {

        }
    }
}
