using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Data;
using System.Text;
using System.Web;
using System.Collections;
using BJSmarts.ERP.SharePoint.Entities;

namespace BJSmarts.ERP.SharePoint.WebParts.LinkWebPart
{
    public partial class LinkWebPartUserControl : UserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;        

        protected void Page_Load(object sender, EventArgs e)
        {

            SharePointContext.Current.Debug.Stmt("Current Organization Name: " + SharePointContext.Current.Organization.Name + "<br>", this.Page);
            SharePointContext.Current.Debug.Stmt("Current Organization Id: " + SharePointContext.Current.Organization.ID + "<br>", this.Page);           

            ModuleCollection modules = new ModuleCollection();                       

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SharePointContext.Current.Debug.Stmt("Site Id: " + web.Site.ID + "<br>", this.Page);

                    SPList list = web.Lists["Application Modules"];

                    foreach ( SPListItem item in list.Items ) 
                    {
                        if (item["Title"].ToString().Equals("Human Resources"))
                        {
                            if (item["Installed"].ToString().Equals("Yes"))
                            {
                                modules.Add(new Module("Human Resources", "Recursos Humanos", "Employee Directory", "Directorios de Empleados", "Employee Directory"));
                                modules.Add(new Module("Human Resources", "Recursos Humanos", "TimeSheets", "Planillas", "TimeSheets"));
                            }
                        }

                        if (item["Title"].ToString().Equals("CRM"))
                        {
                            if (item["Installed"].ToString().Equals("Yes"))
                            {
                                modules.Add(new Module("CRM", "CRM", "Sales", "Ventas", "Sales"));
                                modules.Add(new Module("CRM", "CRM", "Marketing", "Marketing", "Marketing"));
                                modules.Add(new Module("CRM", "CRM", "Services", "Servicios", "Services"));
                            }
                        }
                    }
                }
            }

            ProcessModules(modules);
                
        }

        void ProcessModules(ModuleCollection modules)
        {

            bool ShowModule = false;

            for (int i = 0; i < modules.Count; i++)
            {
                if (i % 2 != 0)
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append("<table border='0' width='800px'>");
                    builder.Append("<tr>");
                    
                    int startIndex = i - 1;
                    
                    for (int x = startIndex; x <= i; x++)
                    {
                        Module module = modules[x];
                        builder.Append("<td valign='top' width='50%'>");
                        builder.Append("<table><tr><td class='dashboardColumnTitle'>" + module.Title + "</td></tr></table>");

                        builder.Append("<table>");
                        builder.Append("<tr>");
                        builder.Append("<td valign='top'style='width:60px; height:32px'>");
                        builder.Append("<img src='/_layouts/Images/BJSmarts.ERP.SharePoint/link_list-48x48.png' alt='" + module.SubTitle + "'>");
                        builder.Append("</td>");
                        builder.Append("<td>");
                        builder.Append("<table border='0'>");
                        builder.Append( loadModuleLinkData( module ) );
                        builder.Append("</table>");
                        builder.Append("</td>");
                        builder.Append("</tr>");
                        builder.Append("</table>");
                        builder.Append("</td>");

                        ShowModule = module.ShowModule;
                    }

                    builder.Append("</tr>");
                    builder.Append("</table>");

                    if ( ShowModule )
                    {
                        this.Controls.Add(new LiteralControl(builder.ToString()));
                    }
                }
            }

            if (modules.Count % 2 != 0)
            {
                Module module = modules[modules.Count - 1];

                StringBuilder builder = new StringBuilder();

                String MainTitle = intLCID == 3082 ? module.AlternativeTitle : module.Title;

                builder.Append("<table border='0' width='800px'>");
                builder.Append("<tr>");

                builder.Append("<td valign='top' width='50%'>");
                builder.Append("<table><tr><td class='dashboardColumnTitle'>" + MainTitle + "</td></tr></table>");

                builder.Append("<table>");
                builder.Append("<tr>");
                builder.Append("<td valign='top'style='width:60px; height:32px'>");
                builder.Append("<img src='/_layouts/Images/BJSmarts.ERP.SharePoint/link_list-48x48.png' alt='" + MainTitle + "'>");
                builder.Append("</td>");
                builder.Append("<td>");
                builder.Append("<table border='0'>");
                builder.Append( loadModuleLinkData( module ) );
                builder.Append("</table>");
                builder.Append("</td>");
                builder.Append("</tr>");
                builder.Append("</table>");
                builder.Append("</td>");

                builder.Append("</tr>");
                builder.Append("</table>");                

                if (module.ShowModule)
                {
                    this.Controls.Add(new LiteralControl(builder.ToString()));
                }
            }
        }

        String loadModuleLinkData(Module module)
        {
            StringBuilder builder = new StringBuilder();
            
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["Application Links"];

                    SPQuery query = new SPQuery();

                    if (intLCID == 3082)
                    {

                        query.Query = "" +
                                "<Where><And><Eq><FieldRef Name='Language' /><Value Type='Integer'>1</Value></Eq>" +
                                "<Eq><FieldRef Name='SubModule' /><Value Type='Lookup'>" + module.SubModule + "</Value></Eq></And></Where>" +
                                "<OrderBy><FieldRef Ascending='TRUE' Name='Sort_x0020_Order'/></OrderBy>";
                    }
                    else
                    {

                        query.Query = "" +
                                "<Where><And><Eq><FieldRef Name='Language' /><Value Type='Integer'>0</Value></Eq>" +
                                "<Eq><FieldRef Name='SubModule' /><Value Type='Lookup'>" + module.SubModule + "</Value></Eq></And></Where>" +
                                "<OrderBy><FieldRef Ascending='TRUE' Name='Sort_x0020_Order'/></OrderBy>";
                    }

                    SPListItemCollection items = list.GetItems(query);

                    SharePointUser user = new SharePointUser();

                    if (user.IsSiteAdmin)
                    {
                        builder.Append(createSiteAdminLinkView(module, items));
                        module.ShowModule = true;   
                    }
                    else
                    {
                        String MainSubTitle = intLCID == 3082 ? module.AlternativeSubTitle : module.SubTitle;
                        builder.Append("<tr><td class='dashboardColumnTitle'>" + MainSubTitle + "</td></tr>"); 

                        if (user.IsEmployee)
                        {
                            DebugStmt("Current User: " + user.Name + " is a regular user <br>");
                            DebugStmt("Total Items: " + items.Count + "<br>");

                            foreach (SPListItem item in items)
                            {
                                if (item["Permissions"].ToString().Contains("Employees"))
                                {
                                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    string URL = value.Url;

                                    DebugStmt("item added: " + value.Description + "<br>");

                                    builder.Append("<tr>");
                                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                                    builder.Append("</tr>");

                                    module.ShowModule = true;
                                }
                            }     
                        }

                        if (user.IsSupervisor)
                        {
                            DebugStmt("Current User: " + user.Name + " is a regular user <br>");
                            DebugStmt("Total Items: " + items.Count + "<br>");                            

                            foreach (SPListItem item in items)
                            {
                                if (item["Permissions"].ToString().Contains("Employees") || item["Permissions"].ToString().Contains("Supervisors"))
                                {
                                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    string URL = value.Url;

                                    DebugStmt("item added: " + value.Description + "<br>");

                                    builder.Append("<tr>");
                                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                                    builder.Append("</tr>");

                                    module.ShowModule = true;
                                }
                            }     
                        }

                        if (user.IsSalesPerson)
                        {
                            DebugStmt("Current User: " + user.Name + " is a regular user <br>");
                            DebugStmt("Total Items: " + items.Count + "<br>");
                            
                            foreach (SPListItem item in items)
                            {
                                if (item["Permissions"].ToString().Contains("Sales"))
                                {
                                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    string URL = value.Url;

                                    DebugStmt("item added: " + value.Description + "<br>");

                                    builder.Append("<tr>");
                                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                                    builder.Append("</tr>");

                                    module.ShowModule = true;
                                }
                            }     
                        }

                        if (user.IsMarketingPerson)
                        {
                            DebugStmt("Current User: " + user.Name + " is a regular user <br>");
                            DebugStmt("Total Items: " + items.Count + "<br>");
                            
                            foreach (SPListItem item in items)
                            {
                                if (item["Permissions"].ToString().Contains("Marketing"))
                                {
                                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    string URL = value.Url;

                                    DebugStmt("item added: " + value.Description + "<br>");

                                    builder.Append("<tr>");
                                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                                    builder.Append("</tr>");

                                    module.ShowModule = true;
                                }
                            }     
                        }

                        if (user.IsServicePerson)
                        {
                            DebugStmt("Current User: " + web.CurrentUser.Name + " is a regular user <br>");
                            DebugStmt("Total Items: " + items.Count + "<br>");
                            
                            foreach (SPListItem item in items)
                            {
                                if (item["Permissions"].ToString().Contains("Services"))
                                {
                                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    string URL = value.Url;

                                    DebugStmt("item added: " + value.Description + "<br>");

                                    builder.Append("<tr>");
                                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                                    builder.Append("</tr>");

                                    module.ShowModule = true;
                                }
                            }     
                        }
                    }                                       
                }
            }

            

            return builder.ToString();
        }

        protected String createSiteAdminLinkView(Module module, SPListItemCollection items)
        {
            StringBuilder builder = new StringBuilder();

            String MainSubTitle = intLCID == 3082 ? module.AlternativeSubTitle : module.SubTitle;

            builder.Append("<tr><td class='dashboardColumnTitle'>" + MainSubTitle + "</td></tr>");

            foreach (SPListItem item in items)
            {
                SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));

                string URL = value.Url;

                builder.Append("<tr>");
                builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                builder.Append("</tr>");
            }

            return builder.ToString();
        }

        protected String createContextLinkView(String SubTitle, String AlternativeSubTitle, SPListItemCollection items, String PermissionEntry)
        {
            StringBuilder builder = new StringBuilder();            

            String MainSubTitle = intLCID == 3082 ? AlternativeSubTitle : SubTitle;
            builder.Append("<tr><td class='dashboardColumnTitle'>" + MainSubTitle + "</td></tr>");            

            foreach (SPListItem item in items)
            {                
                if (item["Permissions"].ToString().Contains(PermissionEntry))
                {
                    SPFieldUrlValue value = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                    string URL = value.Url;

                    DebugStmt("item added: " + value.Description + "<br>");

                    builder.Append("<tr>");
                    builder.Append("<td width='20%' class='dashboardColumnTitle'><a href=" + System.Web.HttpUtility.UrlPathEncode(URL) + ">" + value.Description + "</a></td>");
                    builder.Append("</tr>");
                }
            }        

            return builder.ToString();
        }

        protected bool IsUserInThisGroup(String groupName, SPWeb web)
        {
            Boolean IsUserInThisGroup = false;
            SPGroupCollection GroupCollection = web.Groups;

            try
            {
                SPGroup Group = GroupCollection[groupName];

                if (Group.ContainsCurrentUser)
                {
                    IsUserInThisGroup = true;
                }
            }
            catch { }

            return IsUserInThisGroup;

        }

        private void DebugStmt(String stmt)
        {
            if ((Page.Request.QueryString["Debug"] != null) && (Page.Request.QueryString["Debug"].ToString().Equals("True")))
            {
                Page.Response.Write(stmt);
            }
        }
    }
}
