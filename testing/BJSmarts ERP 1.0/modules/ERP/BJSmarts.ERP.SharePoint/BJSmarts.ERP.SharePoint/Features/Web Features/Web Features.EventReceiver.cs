using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace BJSmarts.ERP.SharePoint.Features.Web_Features
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fdeca84b-27a5-41e3-81ab-9516653b41b9")]
    public class Web_FeaturesEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public enum AssociatedGroupTypeEnum { Members, Visitors };

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var web = properties.Feature.Parent as SPWeb)
            {
                //enable Home Page
                var rootFolder = web.RootFolder;
                //rootFolder.WelcomePage = @"Pages/Wizard.aspx";
                rootFolder.WelcomePage = @"Pages/Home.aspx";
                rootFolder.Update();

                AddGroup(web, "ERP Portal Employees", false);
                AddGroup(web, "ERP Portal Supervisors", false);
                AddGroup(web, "ERP Portal Sales", false);
                AddGroup(web, "ERP Portal Marketing", false);
                AddGroup(web, "ERP Portal Services", false);
            }
        }

        private SPGroup SearchGroup(string group, SPWeb web)
        {

            SPGroup groupObject = null;

            foreach (SPGroup singleGroup in web.Groups)
            {
                if (group == singleGroup.Name)
                {
                    groupObject = singleGroup;
                }

            }

            return groupObject;

        }

        private void AddGroup(SPWeb web, String GroupName, bool copyUsersFromParent)
        {

            SPGroup group = SearchGroup(GroupName, web);

            if (group != null)
                return;

            SPGroup parentAssociatedMemberGroup = null;
            SPGroup parentAssociatedVisitorGroup = null;

            if (web.ParentWeb != null)
            {
                parentAssociatedMemberGroup = web.ParentWeb.AssociatedMemberGroup;
                parentAssociatedVisitorGroup = web.ParentWeb.AssociatedVisitorGroup;
            }
            else
            {
                parentAssociatedMemberGroup = web.AssociatedMemberGroup;
                parentAssociatedVisitorGroup = web.AssociatedVisitorGroup;
            }

            AssociatedGroupTypeEnum associateGroupType = AssociatedGroupTypeEnum.Visitors;

            switch (associateGroupType)
            {
                case AssociatedGroupTypeEnum.Members:
                    AddGroup(web, GroupName, "Use this group to give people contribute permissions to the SharePoint site: {0}", "Contribute", "vti_associatemembergroup", parentAssociatedMemberGroup, copyUsersFromParent);
                    break;
                default:
                    AddGroup(web, GroupName, "Use this group to give people read permissions to the SharePoint site: {0}", "Read", "vti_associatevisitorgroup", parentAssociatedVisitorGroup, copyUsersFromParent);;
                    break;
            }
        }

        public SPGroup AddGroup(SPWeb spWeb, string groupName, string descriptionFormatString, string roleDefinitionName, string associatedGroupName, SPGroup parentAssociatedGroup, bool copyUsersFromParent)
        {

            SPGroup owner = parentAssociatedGroup;

            if (associatedGroupName != "vti_associateownergroup")

                owner = spWeb.SiteGroups.GetByID(int.Parse(spWeb.Properties["vti_associateownergroup"]));

            try
            {
                spWeb.SiteGroups.Add(groupName, owner, null, string.Format(descriptionFormatString, spWeb.Name));
            }
            catch { }


            SPGroup group = spWeb.SiteGroups[groupName];

            if (descriptionFormatString.IndexOf("{0}") != -1)
            {

                SPListItem item = spWeb.SiteUserInfoList.GetItemById(group.ID);
                item["Notes"] = string.Format(descriptionFormatString, string.Format("<a href=\"{0}\">{1}</a>", spWeb.Url, spWeb.Name));
                item.Update();

            }

            if (roleDefinitionName != null)
            {

                SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
                SPRoleDefinition roleDefinition = spWeb.RoleDefinitions[roleDefinitionName];
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                spWeb.RoleAssignments.Add(roleAssignment);

            }

            if (copyUsersFromParent && parentAssociatedGroup != null)
                foreach (SPUser user in parentAssociatedGroup.Users)
                    group.AddUser(user);

            if (associatedGroupName != null)
            {
                spWeb.Properties[associatedGroupName] = group.ID.ToString();
                spWeb.Properties.Update();
            }

            spWeb.Update();

            return group;

        }

        public static void SetAssociatedGroups(SPWeb spWeb, SPGroup[] groups)
        {

            string formatString = "";

            object[] ids = new object[groups.Length];

            for (int i = 0; i < groups.Length; i++)
            {
                formatString += string.Format("{{{0}}};", i);
                ids[i] = groups[i].ID;
            }

            spWeb.Properties["vti_associategroups"] = string.Format(formatString.TrimEnd(new char[] { ';' }), ids);
            spWeb.Properties.Update();

        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
