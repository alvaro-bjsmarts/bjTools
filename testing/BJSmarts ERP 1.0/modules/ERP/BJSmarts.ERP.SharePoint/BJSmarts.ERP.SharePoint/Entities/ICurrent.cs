using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface ICurrent
    {
        ISite Site { get; set; }

        IWeb Web { get; set; }

        ISettings Settings { get; set; }

        IOrganization Organization { get; set; }

        ILog Log { get; set; }

        IDebug Debug { get; set; }

        ILanguageFactory LanguageFactory { get; set; }        
    }
}
