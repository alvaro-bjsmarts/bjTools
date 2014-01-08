using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using System.Globalization;
using System.Xml;


namespace PopulateTimesheet
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //ProcessRequest();
                loadHoursData();
            }
            else
            {
                ProcessRequest(args);
            }
        }

        static void ProcessRequest()
        {
            try
            {
                //BroadcastDailyRequests();
            }
            catch (Exception ex)
            {
                //WriteToLog("Method Name : 'AlertExpiringOpportunity()' \n" + ex.Message, ex.StackTrace);
            }
        }

        static void ProcessRequest(String[] args)
        {
            switch (args[0].ToUpper())
            {
                case "-WEEKS":
                    loadWeeksData();
                    break;
                case "-HOURS":
                    loadHoursData();
                    break;
                case "-HELP":
                    DisplayUsage();
                    break;
                default:
                    DisplayWrongMessage();
                    break;
            }
        }

        static void DisplayUsage()
        {
            string usage =
                "\n" +
                "Usage: \n" +
                "To load only Week Test data:                     PopulateTimesheet.exe -WEEKS \n" +
                "To load hours into Database and Sharepoint:      PopulateTimesheet.exe -HOURS \n";

            Console.WriteLine(usage);
        }

        static void DisplayWrongMessage()
        {
            Console.WriteLine("Operation not valid. Entered wrong argument !!!!!");
            DisplayUsage();
        }

        static void loadWeeksData()
        {

            String connectionString = "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;";

            SqlConnection connection = new SqlConnection(connectionString);
            
            connection.Open();

            try
            {

                int PreviousWeek = GetIso8601WeekOfYear(DateTime.Today) - 1;

                for (int i = 0; i < PreviousWeek; i++)
                {

                    string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[WeekNumber] (WeekNumber, StartDate, EndDate, Year, Language) VALUES (@WeekNumber, @StartDate, @EndDate, @Year, @Language)";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add("@WeekNumber", SqlDbType.Int).Value = i;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = GetStartDateWeek(i);
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = GetEndDateWeek(i);
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = 2013;
                    cmd.Parameters.Add("@Language", SqlDbType.Int).Value = 0;
                    cmd.ExecuteNonQuery();
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
            
        }

        static void loadHoursData()
        {

            Random rd = new Random();
            
            String connectionString = "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;";

            SqlConnection connection = new SqlConnection(connectionString);
            
            connection.Open();

            try
            {

                int PreviousWeek = GetIso8601WeekOfYear(DateTime.Today) - 1;

                //connect to SharePoint
                SPSite site = new SPSite("http://goofy:51816");

                SPWeb web = site.OpenWeb();
                        
                //get pif information from SharePoint
                SPList employeeList = web.Lists["Employees"];
                
                web.AllowUnsafeUpdates = true;

                for (int i = 0; i < 5; i++)
                {

                    foreach (SPListItem item in employeeList.Items)
                    {

                        int hours = rd.Next(7, 9);

                        if (IsEven(hours))
                            hours = 8;                        

                        string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Timesheet] (EmployeeAccount, WeekNumber, EmployeeTimeAccount, day1, day1time, day2, day2time, day3, day3time, day4, day4time, day5, day5time) " +
                                                                                    "VALUES (@EmployeeAccount, @WeekNumber, @EmployeeTimeAccount, @day1, @day1time, @day2, @day2time, @day3, @day3time, @day4, @day4time, @day5, @day5time)";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.Add("@EmployeeAccount", SqlDbType.NVarChar, 50).Value = item["Email"].ToString();
                        cmd.Parameters.Add("@WeekNumber", SqlDbType.Int).Value = i;
                        cmd.Parameters.Add("@EmployeeTimeAccount", SqlDbType.NVarChar, 50).Value = "Regular Time";
                        cmd.Parameters.Add("@day1", SqlDbType.Int).Value = hours;
                        cmd.Parameters.Add("@day1time", SqlDbType.DateTime).Value = GetMondayWeek(i);
                        cmd.Parameters.Add("@day2", SqlDbType.Int).Value = hours;
                        cmd.Parameters.Add("@day2time", SqlDbType.DateTime).Value = GetTuesdayWeek(i);
                        cmd.Parameters.Add("@day3", SqlDbType.Int).Value = hours;
                        cmd.Parameters.Add("@day3time", SqlDbType.DateTime).Value = GetWednesdayWeek(i);
                        cmd.Parameters.Add("@day4", SqlDbType.Int).Value = hours;
                        cmd.Parameters.Add("@day4time", SqlDbType.DateTime).Value = GetThursdayWeek(i);
                        cmd.Parameters.Add("@day5", SqlDbType.Int).Value = hours;
                        cmd.Parameters.Add("@day5time", SqlDbType.DateTime).Value = GetFridayWeek(i);

                        cmd.ExecuteNonQuery();

                        String AccountName = item["Email"].ToString();

                        if (AccountName.Length > 0)
                        {

                            SPUser EmployeeAccount = null;

                            try
                            {
                                EmployeeAccount = web.EnsureUser(AccountName);
                            }
                            catch { }

                            if (EmployeeAccount != null)
                            {
                                SPListItemCollection TimeSheetItems = web.Lists["Timesheets"].Items;
                                SPListItem timeSheetItem = TimeSheetItems.Add();

                                timeSheetItem["Title"] = "Test";
                                timeSheetItem["WeekNumber"] = i;
                                timeSheetItem["Employee"] = EmployeeAccount;
                                SetExternalFieldValue(timeSheetItem, "Account Type", "Regular Time");
                                timeSheetItem["_x0044_ay1"] = hours;
                                timeSheetItem["Day1Time"] = GetMondayWeek(i);
                                timeSheetItem["_x0044_ay2"] = hours;
                                timeSheetItem["Day2Time"] = GetTuesdayWeek(i);
                                timeSheetItem["_x0044_ay3"] = hours;
                                timeSheetItem["Day3Time"] = GetWednesdayWeek(i);
                                timeSheetItem["_x0044_ay4"] = hours;
                                timeSheetItem["Day4Time"] = GetThursdayWeek(i);
                                timeSheetItem["_x0044_ay5"] = hours;
                                timeSheetItem["Day5Time"] = GetFridayWeek(i);
                                timeSheetItem["Total_x0020_Hours"] = hours * 5;
                                timeSheetItem.Update();
                            }
                        }                        
                    }
                }

                web.AllowUnsafeUpdates = false;
            
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
        }

        public static void SetExternalFieldValue(SPListItem item, string fieldInternalName, string EntityValue)
        {
            if (item.Fields[fieldInternalName].TypeAsString == "BusinessData")
            {
                SPField myField = item.Fields[fieldInternalName];
                XmlDocument xmlData = new XmlDocument();
                xmlData.LoadXml(myField.SchemaXml);
                //Get teh internal name of the SPBusinessDataField's identity column.
                String entityName = xmlData.FirstChild.Attributes["RelatedFieldWssStaticName"].Value;

                //Set the value of the identity column.
                item[entityName] = EntityInstanceIdEncoder.EncodeEntityInstanceId(new object[] { EntityValue });
                item[fieldInternalName] = EntityValue;
            }
            else
            {
                throw new InvalidOperationException(fieldInternalName + " is not of type BusinessData");
            }
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        static int GetIso8601WeekOfYear(DateTime time)
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

        static DateTime GetMondayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
                StartDate = firstMonday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }

        static DateTime GetTuesdayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;
                DateTime firstTuesday = jan1.AddDays(daysOffset);
                StartDate = firstTuesday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }

        static DateTime GetWednesdayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Wednesday - jan1.DayOfWeek;
                DateTime firstTuesday = jan1.AddDays(daysOffset);
                StartDate = firstTuesday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }

        static DateTime GetThursdayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
                DateTime firstTuesday = jan1.AddDays(daysOffset);
                StartDate = firstTuesday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }

        static DateTime GetFridayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Friday - jan1.DayOfWeek;
                DateTime firstTuesday = jan1.AddDays(daysOffset);
                StartDate = firstTuesday.AddDays(WeekNum * 7);
            }
            else
            {
                StartDate = jan1;
            }

            return StartDate;

        }

        static DateTime GetStartDateWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime StartDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
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
            DateTime jan1 = new DateTime(2013, 1, 1);

            DateTime EndDate;

            if (WeekNum > 0)
            {
                int daysOffset = DayOfWeek.Friday - jan1.DayOfWeek;
                DateTime firstMonday = jan1.AddDays(daysOffset);
                EndDate = firstMonday.AddDays(WeekNum * 7);
            }
            else
            {
                int daysOffset = DayOfWeek.Friday - jan1.DayOfWeek;
                DateTime firstFriday = jan1.AddDays(daysOffset);
                EndDate = firstFriday;
            }

            return EndDate;
        }

        static void WriteToLog(string message, string stacktrace)
        {
            //connect to SharePoint
            SPSite site = new SPSite(SiteUrl);

            site.AllWebs[SiteName].AllowUnsafeUpdates = true;

            //get contract information from SharePoint
            SPList errorLogList = site.AllWebs[SiteName].Lists["Error Log List"];

            SPListItem item = errorLogList.Items.Add();
            item["Title"] = message;
            item["StackTrace"] = stacktrace;

            item.Update();
        }

        public static String SiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteUrl"].ToString();
            }
        }

        public static String SiteName
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteName"].ToString();
            }
        }

    }
}
