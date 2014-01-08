using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointSettings : ISettings
    {
        public string DatabaseConnectionString
        {
            get { return "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;"; }
        }
    }
}
