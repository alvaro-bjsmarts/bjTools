
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace TestWebPartGenerationProject
{
    public class SharePointSettings : ISettings
    {
        public string DatabaseConnectionString
        {
            get { return "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;"; }
        }
    }
}
