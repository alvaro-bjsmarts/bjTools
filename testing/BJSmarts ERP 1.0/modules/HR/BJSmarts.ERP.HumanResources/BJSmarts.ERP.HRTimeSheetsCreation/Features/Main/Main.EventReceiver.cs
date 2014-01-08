using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace BJSmarts.ERP.HRTimeSheetsCreation.Features.Main
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("a5b10f7e-0698-4442-b013-250d34d7a5e2")]
    public class MainEventReceiver : SPFeatureReceiver
    {

        const string JobName = "Weekly Timeheet Creation";
        
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            DeleteJob(site); // Delete Job if already Exists

            CreateJob(site); // Create new Job
        }        

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            DeleteJob(properties.Feature.Parent as SPSite);
        }

        private static void DeleteJob(SPSite site)
        {
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
                if (job.Name == JobName)
                    job.Delete();
        }

        private static void CreateJob(SPSite site)
        {
            TimeSheetsCreation job = new TimeSheetsCreation(JobName, site.WebApplication);
            
            SPWeeklySchedule schedule = new SPWeeklySchedule();
            schedule.BeginDayOfWeek = DayOfWeek.Sunday;
            schedule.EndDayOfWeek = DayOfWeek.Sunday;
            schedule.BeginHour = 01;
            schedule.EndHour = 02;
            schedule.BeginMinute = 01;
            schedule.EndMinute = 01;
            schedule.BeginSecond = 00;
            schedule.EndSecond = 00;

            job.Schedule = schedule;
            job.Update();
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
