using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BJSmarts.ERP.SharePoint.WebParts.Marital.MaritalStatusInsertWebPart
{
    [ToolboxItemAttribute(false)]
    public class MaritalStatusInsertWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/BJSmarts.ERP.SharePoint.WebParts.Marital/MaritalStatusInsertWebPart/MaritalStatusInsertWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
