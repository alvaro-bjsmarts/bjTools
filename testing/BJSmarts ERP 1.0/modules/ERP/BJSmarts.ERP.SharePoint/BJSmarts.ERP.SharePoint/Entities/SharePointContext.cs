using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public static class SharePointContext
    {

        static SharePointContext()
        {
            Current = new SharePointCurrent();
        }        

        public static ICurrent Current { get; set; }
    }
}
