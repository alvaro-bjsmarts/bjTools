using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public interface IDebug
    {
        void Stmt(String stmt, System.Web.UI.Page Page);
    }
}
