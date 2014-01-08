using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace BJSmarts.ERP.HumanResources.Lists.Timesheets.TimeSheetEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class TimeSheetEventReceiver : SPItemEventReceiver
    {
       /// <summary>
       /// An item is being added.
       /// </summary>
       public override void ItemAdding(SPItemEventProperties properties)
       {
           try
           {
               int TotalHours = 0;

               if (properties.AfterProperties["_x0044_ay1"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day1Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay1"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay2"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day2Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay2"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay3"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day3Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay3"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay4"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day4Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay4"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay5"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day5Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay5"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay6"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day6Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay6"].ToString());
               }
               if (properties.AfterProperties["_x0044_ay7"] != null && properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
               {
                   properties.AfterProperties["Day7Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                   TotalHours += int.Parse(properties.AfterProperties["_x0044_ay7"].ToString());
               }

               properties.AfterProperties["Total_x0020_Hours"] = TotalHours;

           }
           catch (Exception ex)
           {
               // log error 
               LogError(ex.Message, ex.StackTrace, properties);
           }

           base.ItemAdding(properties);
       }

       /// <summary>
       /// An item is being updated.
       /// </summary>
       public override void ItemUpdating(SPItemEventProperties properties)
       {
           try
           {


               if (properties.ListItem["_x0044_ay1"] != null)
               {               
                   if (properties.AfterProperties["_x0044_ay1"].ToString() != properties.ListItem["_x0044_ay1"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay1"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day1Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               if (properties.ListItem["_x0044_ay2"] != null)
               {                   
                   if (properties.AfterProperties["_x0044_ay2"].ToString() != properties.ListItem["_x0044_ay2"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay2"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day2Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               if (properties.ListItem["_x0044_ay3"] != null)
               {                   
                   if (properties.AfterProperties["_x0044_ay3"].ToString() != properties.ListItem["_x0044_ay3"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay3"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day3Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               if (properties.ListItem["_x0044_ay4"] != null)
               {                   
                   if (properties.AfterProperties["_x0044_ay4"].ToString() != properties.ListItem["_x0044_ay4"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay4"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day4Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               if (properties.ListItem["_x0044_ay5"] != null)
               {               
                   if (properties.AfterProperties["_x0044_ay5"].ToString() != properties.ListItem["_x0044_ay5"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay5"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day5Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               if (properties.ListItem["_x0044_ay6"] != null)
               {               
                   if (properties.AfterProperties["_x0044_ay6"].ToString() != properties.ListItem["_x0044_ay6"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay6"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day6Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }


               if (properties.ListItem["_x0044_ay7"] != null)
               {               
                   if (properties.AfterProperties["_x0044_ay7"].ToString() != properties.ListItem["_x0044_ay7"].ToString())
                   {
                       if (properties.AfterProperties["_x0044_ay7"].ToString().Length > 0)
                       {
                           properties.AfterProperties["Day7Time"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now).ToString();
                       }
                   }
               }

               properties.AfterProperties["Total_x0020_Hours"] = GetTotalHours(properties);

           }
           catch (Exception ex)
           {
               LogError(ex.Message, ex.StackTrace, properties);
           }

           base.ItemUpdating(properties);
       }

       private int GetTotalHours(SPItemEventProperties properties)
       {
           int TotalHours = 0;

           for (int i = 1; i <= 7; i++)
           {
               if (properties.AfterProperties["_x0044_ay" + i] != null)
               {
                   if (properties.AfterProperties["_x0044_ay" + i].ToString().Length > 0)
                   {
                       TotalHours += int.Parse(properties.AfterProperties["_x0044_ay" + i].ToString());
                   }
               }

           }

           return TotalHours;
       }

       private void LogError(String Message, String StackTrace, SPItemEventProperties properties)
       {
           SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               using (SPWeb web = properties.Web)
               {

                   SPList ErrorLogList = web.Lists["Application Errors"];

                   SPListItemCollection items = ErrorLogList.Items;
                   SPListItem item = items.Add();

                   item["Title"] = Message;
                   item["StackTrace"] = StackTrace;

                   item.Update();
               }

           });
       }



       /// <summary>
       /// An item is being deleted.
       /// </summary>
       public override void ItemDeleting(SPItemEventProperties properties)
       {
           base.ItemDeleting(properties);
       }


    }
}
