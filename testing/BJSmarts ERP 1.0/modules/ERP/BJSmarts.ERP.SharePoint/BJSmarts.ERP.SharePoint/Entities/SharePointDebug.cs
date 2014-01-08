using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointDebug : IDebug
    {
        public void Stmt(String stmt, System.Web.UI.Page Page)
        {
            if ((Page.Request.QueryString["Debug"] != null) && (Page.Request.QueryString["Debug"].ToString().Equals("True")))
            {
                Page.Response.Write(stmt + "<br>");
            }
        }
    }
}
