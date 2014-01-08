using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointLog : ILog
    {
        public void Error(string message, string StackTrace)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList ErrorLogList = web.Lists["Application Errors"];

                        SPListItemCollection items = ErrorLogList.Items;
                        SPListItem item = items.Add();

                        item["Title"] = message;
                        item["StackTrace"] = StackTrace;

                        item.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
