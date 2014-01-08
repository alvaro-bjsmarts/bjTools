using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using Microsoft.SharePoint.BusinessData.Infrastructure;

namespace BJSmarts.ERP.HRTimeSheetsCreation
{
    public class TimeSheetsCreation : SPJobDefinition
    {
        public TimeSheetsCreation()
            : base()
        {

        }

        public TimeSheetsCreation(string jobName, SPService service, SPServer server, SPJobLockType lockType)
            : base(jobName, service, server, lockType)
        {
            this.Title = "Weekly Timeheet Creation";
        }

        public TimeSheetsCreation(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "Weekly Timeheet Creation";
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public override void Execute(Guid targetInstanceId)
        {
            StringBuilder sb = new StringBuilder(1024);
            SPWebApplication webapp = this.Parent as SPWebApplication;
            SPContentDatabase contentDb = webapp.ContentDatabases[targetInstanceId];

            SPWeb web = contentDb.Sites[0].RootWeb;

            SPList list = web.Lists["Employees"];

            foreach (SPListItem item in list.Items)
            {
                String AccountName = item["Email"].ToString();

                if (AccountName.Length > 0)
                {

                    SPUser EmployeeAccount = null;

                    try
                    {
                        //get user account using email
                        EmployeeAccount = web.EnsureUser(AccountName);
                    }
                    catch 
                    { 
                        //get user account by 
                        EmployeeAccount = web.EnsureUser("DISNEYLAND\\" + AccountName.Substring(0, AccountName.IndexOf('@')));
                    }                   

                    if (EmployeeAccount != null)
                    {
                        CreateCurrentTimesheet(EmployeeAccount, web);
                    }
                }
            }
        }

        protected void CreateCurrentTimesheet(SPUser AccountName, SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            foreach (SPListItem account in web.Lists["Employee Accounts"].Items)
            {
                if (account != null)
                {
                    SPListItemCollection listItems = web.Lists["TimeSheets"].Items;
                    SPListItem item = listItems.Add();
                    int WeekNumber = GetIso8601WeekOfYear(DateTime.Today);

                    item["Title"] = "";
                    item["WeekNumber"] = WeekNumber;
                    item["Employee"] = AccountName;
                    //item["Start_x0020_Period"] = GetSundayWeek(WeekNumber);
                    //item["End_x0020_Period"] = GetSaturdayWeek(WeekNumber);
                    SetExternalFieldValue(item, "Account Type", account["Name"].ToString());
                    item.Update();
                }
            }

            web.AllowUnsafeUpdates = false;
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
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }        
    }
}
