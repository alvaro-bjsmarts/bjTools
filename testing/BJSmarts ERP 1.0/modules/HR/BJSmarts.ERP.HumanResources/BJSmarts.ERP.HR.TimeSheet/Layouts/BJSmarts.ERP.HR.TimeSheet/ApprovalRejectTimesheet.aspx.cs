using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BJSmarts.ERP.HumanResources.Layouts.BJSmarts.ERP.HumanResources
{
    public partial class ApprovalRejectTimesheet : LayoutsPageBase
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
            
            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Timesheets"];

                        SPQuery query = new SPQuery();

                        //query.Query = "<Where><And>" +
                        //              "<Eq><FieldRef Name='Status'/><Value Type='Text'>Submitted</Value></Eq>" +
                        //              "<Eq><FieldRef Name='Employee' LookupId='TRUE' /><Value Type='User'>" + web.CurrentUser.ID + "</Value></Eq>" +
                        //              "</And></Where>";

                        //SPListItemCollection items = list.GetItems(query);

                        SPListItemCollection items = list.GetItems();

                        int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);

                        SPFieldUserValue userValue = new SPFieldUserValue(
                                                        web,
                                                        items[0]["Employee"].ToString());

                        SPUser employee = getUser(userValue.LookupValue, web);

                        AddSimpleTableRowCell("Employee: ", employee.Name);
                        AddSimpleTableRowCell("Pay Period: ", GetStartDateWeek(CurrentWeek).AddDays(0).ToShortDateString() + " - " + GetEndDateWeek(CurrentWeek).ToShortDateString());

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
        }

        void AddSimpleTableRowCell(String Text, String Value)
        {
            TableRow newRow = new TableRow();
            TableCell newCellText = new TableCell();
            TableCell newCellValue = new TableCell();

            EmployeeDetails.Rows.Add(newRow);
            newRow.Cells.Add(newCellText);
            newRow.Cells.Add(newCellValue);
            newCellText.Text = Text;
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {

            UpdateTimesheetRecord();

            loadTimeSheetOnDatabase();

            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList list = web.Lists["Timesheets"];
                        item = list.Items.GetItemById(RecordId);

                        item["Status"] = "Rejected";
                        SPFieldUrlValue spFieldUrlValue = new SPFieldUrlValue();
                        spFieldUrlValue.Url = "/_layouts/BJSmarts.ERP.HumanResources/RejectTimesheet.aspx?RecordID=" + item["ID"].ToString();
                        spFieldUrlValue.Description = "Rejected";
                        item["ActionLink2"] = spFieldUrlValue;

                        item.Update();

                        web.AllowUnsafeUpdates = false;

                        SendNotification();
                    }
                }
            }

            Page.Response.Redirect(SPContext.Current.Web.Url);
        }

        protected void UpdateTimesheetRecord()
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList list = web.Lists["Timesheets"];
                        item = list.Items.GetItemById(RecordId);

                        item["Status"] = "Approved";
                        item.Update();

                        web.AllowUnsafeUpdates = false;

                    }
                }
            }
        }

        protected void loadTimeSheetOnDatabase()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    String connectionString = "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;";

                    SqlConnection connection = new SqlConnection(connectionString);

                    connection.Open();

                    try
                    {
                        if (Page.Request.QueryString["RecordId"] != null)
                        {
                            int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    web.AllowUnsafeUpdates = true;

                                    SPList list = web.Lists["Timesheets"];
                                    SPListItem item = list.Items.GetItemById(RecordId);

                                    string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Timesheet] (EmployeeAccount, WeekNumber, EmployeeTimeAccount, day1, day1time, day2, day2time, day3, day3time, day4, day4time, day5, day5time) " +
                                                                                            "VALUES (@EmployeeAccount, @WeekNumber, @EmployeeTimeAccount, @day1, @day1time, @day2, @day2time, @day3, @day3time, @day4, @day4time, @day5, @day5time)";
                                    SqlCommand cmd = new SqlCommand(sql, connection);
                                    cmd.Parameters.Add("@EmployeeAccount", SqlDbType.NVarChar, 50).Value = item["Email"].ToString();
                                    cmd.Parameters.Add("@WeekNumber", SqlDbType.Int).Value = int.Parse(item["WeekNumber"].ToString());
                                    cmd.Parameters.Add("@EmployeeTimeAccount", SqlDbType.NVarChar, 50).Value = "Regular Time";
                                    cmd.Parameters.Add("@day1", SqlDbType.Int).Value = item[""] != null ? int.Parse(item["_x0044_ay1"].ToString()) : 0;
                                    cmd.Parameters.Add("@day1time", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Day1Time"].ToString()) : DateTime.Now;
                                    cmd.Parameters.Add("@day2", SqlDbType.Int).Value = item[""] != null ? int.Parse(item["_x0044_ay2"].ToString()) : 0;
                                    cmd.Parameters.Add("@day2time", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Day2Time"].ToString()) : DateTime.Now;
                                    cmd.Parameters.Add("@day3", SqlDbType.Int).Value = item[""] != null ? int.Parse(item["_x0044_ay3"].ToString()) : 0;
                                    cmd.Parameters.Add("@day3time", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Day3Time"].ToString()) : DateTime.Now;
                                    cmd.Parameters.Add("@day4", SqlDbType.Int).Value = item[""] != null ? int.Parse(item["_x0044_ay4"].ToString()) : 0;
                                    cmd.Parameters.Add("@day4time", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Day4Time"].ToString()) : DateTime.Now;
                                    cmd.Parameters.Add("@day5", SqlDbType.Int).Value = item[""] != null ? int.Parse(item["_x0044_ay5"].ToString()) : 0;
                                    cmd.Parameters.Add("@day5time", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Day5Time"].ToString()) : DateTime.Now;

                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception(ex.ToString(), ex);
                        Console.Write(ex.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                });            
        }        

        void SendNotification()
        {

        }

        private SPUser getUser(string name, SPWeb thisWeb)
        {
            foreach (SPUser user in thisWeb.SiteUsers)
            {
                if (String.Equals(user.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
