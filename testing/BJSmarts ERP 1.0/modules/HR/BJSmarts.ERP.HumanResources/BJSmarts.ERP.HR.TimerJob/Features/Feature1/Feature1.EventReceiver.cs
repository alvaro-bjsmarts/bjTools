using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace BJSmarts.ERP.HR.TimerJob.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("221d62f5-bf66-45b0-b146-3e3265065c5a")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        const string JobName = "Employee TimeSheet Creation Job";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            DeleteJob(site); // Delete Job if already Exists

            CreateJob(site); // Create new Job
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            DeleteJob(properties.Feature.Parent as SPSite); // Delete the Job
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
            schedule.BeginHour = 2;
            schedule.BeginMinute = 1;
            schedule.BeginSecond = 0;
            schedule.EndSecond = 5;
            schedule.EndMinute = 1;
            schedule.EndHour = 2;
            schedule.EndDayOfWeek = DayOfWeek.Sunday;


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
