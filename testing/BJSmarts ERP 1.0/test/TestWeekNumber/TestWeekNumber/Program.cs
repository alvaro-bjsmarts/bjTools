using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using Microsoft.SharePoint;

namespace TestWeekNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            int weeknumber = GetIso8601WeekOfYear(DateTime.Now);
            Console.WriteLine(weeknumber);
            weeknumber = GetIso8601WeekOfYear(DateTime.Now);
            Console.WriteLine(weeknumber);
            DateTime sunday = GetMondayWeek(weeknumber);
            DateTime saturday = GetSundayWeek(weeknumber);
            Console.WriteLine(sunday);
            Console.WriteLine(saturday);


            //SPSite oSite = new SPSite("http://donald:51816/");

            //SPWeb web = oSite.OpenWeb();

            //SPList list = web.Lists["Employees"];

            //String AccountName = list.Items[0]["Email"].ToString();

            //SPUser EmployeeAccount = web.EnsureUser("DISNEYLAND\\" + AccountName.Substring(0, AccountName.IndexOf('@')));

            //if (EmployeeAccount != null)
            //{
            //    Console.WriteLine(EmployeeAccount.Name);
            //}

            //connect to SharePoint
            SPSite site = new SPSite("http://donald:51816");

            SPWeb web = site.OpenWeb();

            SPList list = web.Lists["Employees"];

            String AccountName = list.Items[0]["Email"].ToString();

            Console.WriteLine("Done");

            
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

        private static DateTime GetSundayWeek(int WeekNum)
        {
            DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);

            DateTime EndDate;

            if (WeekNum > 1)
            {
                int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
                DateTime LastSaturday = jan1.AddDays(daysOffset);                
                EndDate = LastSaturday.AddDays(WeekNum * 7);
            }
            else
            {
                int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
                DateTime FirstSaturday = jan1.AddDays(daysOffset);
                EndDate = FirstSaturday;
            }

            return EndDate;

        }
    }
}
