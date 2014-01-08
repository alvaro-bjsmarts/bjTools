using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface ISettings
    {
        string DatabaseConnectionString { get; }
    }
}
