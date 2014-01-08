using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface ILog
    {
        void Error(string message, string StackTrace);
    }
}
