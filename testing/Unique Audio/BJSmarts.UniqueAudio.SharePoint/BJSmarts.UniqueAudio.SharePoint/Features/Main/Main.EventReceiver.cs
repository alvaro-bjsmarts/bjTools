using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace BJSmarts.UniqueAudio.SharePoint.Features.Main
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("d2a041d4-43fc-4c90-bd2c-4c5a95d17856")]
    public class MainEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPWeb web = properties.Feature.Parent as SPWeb)
                {

                    try
                    {
                        //enable BJSmarts Menu Bar Site Feature
                        web.Site.Features.Add(new Guid("0d016c8c-6b32-4ba7-babd-74ff96a105d0"));
                    }
                    catch { }



                    try
                    {
                        //enable BJSmarts Contact Web Part
                        web.Site.Features.Add(new Guid("ea4ec4d6-1b1b-4f82-8137-bceecae8d365"));
                    }
                    catch { }

                    try
                    {
                        //enable BJSmarts Navigation Items
                        web.Features.Add(new Guid("cebf4aea-2e3c-41b8-83d9-93ba2cd7db0e"));
                    }
                    catch { }

                    try
                    {
                        //enable Master Page Configuration Settings feature
                        web.Features.Add(new Guid("c197fe19-941f-4aae-9c02-9d99e6851f11"));
                    }
                    catch { }

                    Uri siteMaster = new Uri(string.Format("{0}/_catalogs/masterpage/UniqueMasterPage.master", web.Url));
                    web.CustomMasterUrl = siteMaster.AbsolutePath;
                    web.MasterUrl = siteMaster.AbsolutePath;
                    web.SiteLogoUrl = "/_layouts/BJSmarts.UniqueAudio.SharePoint/Images/logo_unique-audio.png";
                    web.Update();

                    var rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = @"Pages/Home.aspx";
                    rootFolder.Update();

                    AddNavigationItem("Home", "Inicio", "/projects/unique/Pages/home.aspx", "Level 1", 1, "Yes", web);
                    AddNavigationItem("About Us", "Acerca de Nosotros", "/projects/unique/Pages/about.aspx", "Level 1", 2, "Yes", web);
                    AddNavigationItem("Services", "Servicios", "/projects/unique/Pages/services.aspx", "Level 1", 3, "Yes", web);
                    AddNavigationItem("Gallery", "Galeria", "/projects/unique/Pages/gallery.aspx", "Level 1", 4, "Yes", web);
                    AddNavigationItem("Reviews", "Reviews", "/projects/unique/Pages/reviews.aspx", "Level 1", 5, "Yes", web);                    
                    AddNavigationItem("Contact Us", "Contactarse", "/projects/axis/Pages/contact.aspx", "Level 1", 6, "Yes", web);
                    AddNavigationItem("Faqs", "Faqs", "/projects/unique/Pages/faqs.aspx", "Level 1", 7, "Yes", web);
                    AddNavigationItem("Specials", "Specials", "/projects/unique/Pages/specials.aspx", "Level 1", 8, "Yes", web);
                }
            });
        }

        private void AddNavigationItem(String Title, String AlternativeTitle, String LinkUrl, String ItemLevel, int LinkOrder, String Display, SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + Title + "</Value></Eq></Where>";
            SPListItemCollection Items = web.Lists["Navigation List"].GetItems(query);

            if (Items.Count == 0)
            {
                SPListItem item = Items.Add();

                item["Title"] = Title;
                item["AlternativeTitle"] = AlternativeTitle;
                item["Link_x0020_URL"] = LinkUrl;
                item["Link_x0020_Order"] = LinkOrder;
                item["Item_x0020_Level"] = ItemLevel;
                item["Display0"] = Display;

                item.Update();
            }

            web.AllowUnsafeUpdates = false;
        }

        void enableFeature(String Guid, SPSite site)
        {
            try
            {
                site.Features.Add(new Guid(Guid));
            }
            catch (Exception ex)
            {

            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPWeb web = properties.Feature.Parent as SPWeb)
                {
                    Uri siteMaster = new Uri(string.Format("{0}/_catalogs/masterpage/v4.master", web.Url));
                    web.CustomMasterUrl = siteMaster.AbsolutePath;
                    web.MasterUrl = siteMaster.AbsolutePath;
                    web.Update();

                    var rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = @"Default.aspx";
                    rootFolder.Update();
                }
            });
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
