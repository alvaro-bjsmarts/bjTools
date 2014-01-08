using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointCurrent : ICurrent
    {
        public SharePointCurrent()
        {
            Site = new SharePointSite();
            Web = new SharePointWeb();
            Settings = new SharePointSettings();
            Organization = new SharePointOrganization();
            Log = new SharePointLog();
            Debug = new SharePointDebug();
            LanguageFactory = new SharePointLanguageFactory();            
        }
        
        public ISite Site { get; set; }

        public IWeb Web { get; set; }

        public ISettings Settings { get; set; }

        public IOrganization Organization { get; set; }

        public ILog Log { get; set; }

        public IDebug Debug { get; set; }

        public ILanguageFactory LanguageFactory { get; set; }        
    }
}
