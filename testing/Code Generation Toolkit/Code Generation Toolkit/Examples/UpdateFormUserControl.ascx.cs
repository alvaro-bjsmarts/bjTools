using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;

namespace CFTC.PMLC.Update.Webpart.UpdateForm
{
    public partial class UpdateFormUserControl : UserControl, ICallbackEventHandler
    {
        ListItem item;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Form.Enctype = "multipart/form-data";
            SPContext currentContext = SPContext.Current;

            if (!Page.IsPostBack)
            {
                if (Page.Request.QueryString["RecordID"] != null)
                {                    
                    SPUser currentUser = SPContext.Current.Web.CurrentUser;

                    Boolean IsCurrentUserITLeader = IsUserInThisGroup("PMLC ITL Leadership", SPContext.Current.Web);

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(PMLCSiteUrl))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {

                                SPListItem ListItem = GetCurrentItem();

                                LoadingLookupTable("PIF List", ddlParentProject, "ID", true);
                                LoadingLookupTable("Project Type List", ddlProjectType, "Sort_x0020_Order", false);
                                LoadingLookupTable("Strategic Alignment List", ddlPrimary, "Sort_x0020_Order", true);
                                LoadingLookupTable("Strategic Alignment List", ddlSecondary, "Sort_x0020_Order", true);
                                LoadingChoiceTable("Project Sponsor List", chkSponsor);
                                LoadingChoiceTable("Final Project Output List", chkFinalProject);
                                LoadingChoiceTable("Benefits List", chkBenefits);

                                SPFieldUserValueCollection groupTeam = (SPFieldUserValueCollection)ListItem["Proposed_x0020_Project_x0020_Tea"];

                                if (groupTeam != null)
                                {
                                    String users = String.Empty;

                                    foreach (SPFieldUserValue value in groupTeam)
                                    {
                                        users += value.User.ToString() + ",";
                                    }

                                    peTeam.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
                                }

                                LoadingRadioChoiceTable("Project Duration List", rbtnProjectDuration);


                                SPFieldUserValueCollection groupSponsor = (SPFieldUserValueCollection)ListItem["Project_x0020_Sponsor"];

                                if (groupSponsor != null)
                                {
                                    String users = String.Empty;

                                    foreach (SPFieldUserValue value in groupSponsor)
                                    {
                                        users += value.User.ToString() + ",";
                                    }

                                    peProjectSponsor.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
                                }


                                rbtnyonBudgett_load();
                                rbtnParentProject_load();
                                chkPmBranch_load();
                                txtProjectName.Text = GetItemText(ListItem, "Title");
                                ddlProjectType.SelectedIndex = GetItemIndex(ListItem, ddlProjectType, "Project_x0020_Type");
                                txtDescription.Text = GetItemText(ListItem, "Description");
                                GetItemCheckedRadio(ListItem, "Parent_x0020_Project_x0020_Choic", rdParentProjectChoice);
                                ddlParentProject.SelectedIndex = GetItemIndex(ListItem, ddlParentProject, "Parent_x0020_Project");
                                ddlPrimary.SelectedIndex = GetItemIndex(ListItem, ddlPrimary, "Strategic_x0020_Alignment");
                                ddlSecondary.SelectedIndex = GetItemIndex(ListItem, ddlSecondary, "Secondary_x0020_Strategic_x0020_Alignment");
                                txtSponsor.Text = GetItemText(ListItem, "Project_x0020_Sponsor_x0020_and_0");
                                txtFinalProject.Text = GetItemText(ListItem, "Final_x0020_Project_x0020_Output0");
                                GetitemChecked(ListItem, "Project_x0020_Sponsor_x0020_and_", chkSponsor);
                                GetitemChecked(ListItem, "Final_x0020_Project_x0020_Output", chkFinalProject);
                                GetitemChecked(ListItem, "Summary_x0020_of_x0020_Benefits", chkBenefits);
                                txtFinalProject.Text = GetItemText(ListItem, "Final_x0020_Project_x0020_Output0");
                                txtSummaryOther.Text = GetItemText(ListItem, "Summary_x0020_of_x0020_Benefits_");
                                GetItemCheckedRadio(ListItem, "Estimated_x0020_Project_x0020_Du", rbtnProjectDuration);
                                GetItemCheckedRadio(ListItem, "Is_x0020_this_x0020_project_x002", rbtnyonBudget);
                                txtDescriptionRoughOrder.Text = GetItemText(ListItem, "Rough_x0020_Order_x0020_of_x00200");
                                DateTime SuggestedStartDate;
                                DateTime.TryParse(GetItemText(ListItem, "Suggested_x0020_Start_x0020_Date"), out SuggestedStartDate);
                                DataTimeStartDate.SelectedDate = SuggestedStartDate;
                                DateTime SuggestedEndDate;
                                DateTime.TryParse(GetItemText(ListItem, "Suggested_x0020_End_x0020_Date"), out SuggestedEndDate);
                                DateTimeEndDate.SelectedDate = SuggestedEndDate;
                                txtIDprocedure.Text = GetItemText(ListItem, "ProcureID");

                                GetItemCheckBox(ListItem, "Risk_Schedule", chkRiskSchedule);
                                GetItemCheckBox(ListItem, "Risk_Resources", chkRiskResources);
                                GetItemCheckBox(ListItem, "Risk_Cost_Funding", chkRiskCostFunding);
                                GetItemCheckBox(ListItem, "Risk_Technology", chkRiskTechnology);
                                GetItemCheckBox(ListItem, "Risk_Feasibility", chkRiskFeasibility);
                                GetItemCheckBox(ListItem, "Risk_Security_Privacy", chkRiskSecurityorPrivacy);
                                GetItemCheckBox(ListItem, "Risk_Access_Data", chkRiskAccesstoData);
                                GetItemCheckBox(ListItem, "Risk_Interoperability", chkRiskInteroperability);
                                GetItemCheckBox(ListItem, "Risk_Executive_Support", chkRiskExecutiveSupport);
                                GetItemCheckBox(ListItem, "Risk_Other", chkRiskOther);


                                txtRiskSchedule.Text = GetItemText(ListItem, "Risk_Schedule_Details");
                                txtRiskResources.Text = GetItemText(ListItem, "Risk_Resources_Details");
                                txtRiskCostFunding.Text = GetItemText(ListItem, "Risk_Cost_Funding_Details");
                                txtRiskTechnology.Text = GetItemText(ListItem, "Risk_Technology_Details");
                                txtRiskFeasibility.Text = GetItemText(ListItem, "Risk_Feasibility_Details");
                                txtRiskSecurityorPrivacy.Text = GetItemText(ListItem, "Risk_Security_Privacy_Details");
                                txtRiskAccesstoData.Text = GetItemText(ListItem, "Risk_Access_Data_Details");
                                txtRiskInteroperability.Text = GetItemText(ListItem, "Risk_Interoperability_Details");
                                txtRiskExecutiveSupport.Text = GetItemText(ListItem, "Risk_Executive_Support_Details");
                                txtRiskOther.Text = GetItemText(ListItem, "Risk_Other_Details");


                                GetItemCheckBox(ListItem, "Configuration_x0020_Management", chkConfigurationManagement);
                                GetItemCheckBox(ListItem, "Data_x0020_Management", chkDataManagement);
                                GetItemCheckBox(ListItem, "Enterprise_x0020_Architecture", chkEnterpriseArchitecture);
                                GetItemCheckBox(ListItem, "Enterprise_x0020_Services", chkEnterpriseServices);
                                GetItemCheckBox(ListItem, "Infrastructure_x0020_Customer", chkInfrastructureCustomer);
                                GetItemCheckBox(ListItem, "Infrastructure_x0020_Network", chkInfrastructureNetwork);
                                GetItemCheckBox(ListItem, "Infrastructure_x0020_Telecom", chkInfrastructureTelecom);
                                GetItemCheckBox(ListItem, "ODT_x0020_Communications", chkODTCommunications);
                                GetItemCheckBox(ListItem, "Privacy_x0020_Office", chkPrivacyOffice);
                                GetItemCheckBox(ListItem, "Records_x0020_Management", chkRecordManagement);
                                GetItemCheckBox(ListItem, "Section_x0020_508", chkSection);
                                GetItemCheckBox(ListItem, "Security_x0020_Impact_x0020_Asse", chkSecurityImpact);

                                txtCMDetails.Text = GetItemText(ListItem, "Configuration_x0020_Management_x");
                                txtDMDetails.Text = GetItemText(ListItem, "Data_x0020_Management_x0020_Deta");
                                txtEADetails.Text = GetItemText(ListItem, "Enterprise_x0020_Architecture_x0");
                                txtEnterpriseServices.Text = GetItemText(ListItem, "Enterprise_x0020_Services_x0020_");
                                txtInfrastructureCustomer.Text = GetItemText(ListItem, "Infrastructure_x0020_Customer_x0");
                                txtInfrastructureNetwork.Text = GetItemText(ListItem, "Infrastructure_x0020_Network_x00");
                                txtInfrastructureTelecom.Text = GetItemText(ListItem, "Infrastructure_x0020_Telecom_x00");
                                txtODTCommunications.Text = GetItemText(ListItem, "ODT_x0020_Communications_x0020_D");
                                txtPrivacyOffice.Text = GetItemText(ListItem, "Privacy_x0020_Office_x0020_Detai");
                                txtRecordManagement.Text = GetItemText(ListItem, "Records_x0020_Management_x0020_D");
                                txtSecurityImpact.Text = GetItemText(ListItem, "Security_x0020_Impact_x0020_Asse0");
                                txtSection.Text = GetItemText(ListItem, "Section_x0020_508_x0020_Details");
                                txtShortDescription.Text = GetItemText(ListItem, "Short_x0020_Description");

                                SPFieldUserValue groupManager = null;

                                if (ListItem["Proposed_x0020_Project_x0020_Man"] != null)
                                {
                                    groupManager = new SPFieldUserValue(web, ListItem["Proposed_x0020_Project_x0020_Man"].ToString());
                                }

                                if (groupManager != null)
                                {
                                    peProposedManager.CommaSeparatedAccounts = groupManager.User.ToString();
                                }

                                GetItemCheckedRadio(ListItem, "PM_x0020_Branch", rbtnPmBranch);

                                lblDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

                                BindAttachmentData();

                                SPUser author = getAuthor(ListItem, web);

                                EnableWorkflowButtons(currentUser, author, web, ListItem, IsCurrentUserITLeader);

                            }
                        }
                    });
                }
            }            
        }

        private void EnableWorkflowButtons(SPUser CurrentUser, SPUser Author, SPWeb web, SPListItem item, Boolean IsCurrentUserITLeader)
        {
            if (CurrentUser.Name.Equals(Author.Name) && item["Workflow_x0020_Status"].ToString().Equals("Draft"))
            {
                lbSubmitApproval.Visible = true;
            }
                    
            if ((IsCurrentUserITLeader) || (CurrentUser.IsSiteAdmin))
            {
                String workflowStatus = item["Workflow_x0020_Status"].ToString();

                Page.Response.Write(workflowStatus);

                if ((workflowStatus == "Approved") || (workflowStatus == "Rejected") || (workflowStatus == "Put On Hold") || (workflowStatus == "Defer to ITL"))
                {
                    lbChangeDecision.Visible = true;
                }
            }
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

        private SPUser getAuthor(SPListItem item, SPWeb web)
        {

            SPUser author = null;

            SPFieldUserValue userValue = new SPFieldUserValue(
                                                        web,
                                                        item["Author"].ToString());

            author = getUser(userValue.LookupValue, web);

            return author;

        }

        private SPUser getUser(string name, SPWeb thisWeb)
        {
            foreach (SPUser user in thisWeb.SiteUsers)
            {
                if (String.Equals(user.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return user;
                }
            }
            return null;
        }
        
        void LoadingLookupTable(String listname, DropDownList ddl, string sortfield, Boolean addInitialValue)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItemCollection items = null;

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPQuery query = new SPQuery();

                            query.Query = "" +
                                    "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                            items = oList.GetItems(query);
                        }
                        catch { }
                    }

                    if (addInitialValue)
                    {
                        ListItem item = new ListItem();
                        item.Text = "(None)";
                        item.Value = "0";
                        item.Attributes.Add("title", "None_0");
                        ddl.Items.Add(item);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        item = new ListItem();
                        item.Text = items[i]["Title"].ToString();
                        item.Value = items[i]["ID"].ToString();
                        item.Attributes.Add("title", items[i]["Title"].ToString() + "_" + items[i]["ID"].ToString());
                        ddl.Items.Add(item);
                    }
                }
            }            
        }


        void LoadingChoiceTable(String listname, CheckBoxList chk)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItemCollection items = null;

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPQuery query = new SPQuery();

                            query.Query = "" +
                                    "<OrderBy><FieldRef Ascending='TRUE' Name='Sort_x0020_Order'/></OrderBy>";

                            items = oList.GetItems(query);
                        }
                        catch { }
                    }


                    for (int i = 0; i < items.Count; i++)
                    {
                        item = new ListItem();
                        item.Text = items[i]["Title"].ToString();
                        item.Value = items[i]["ID"].ToString();
                        item.Attributes.Add("title", items[i]["Title"].ToString() + "_" + items[i]["ID"].ToString());
                        chk.Items.Add(item);
                    }
                }
            }            
        }


        void LoadingRadioChoiceTable(String listname, RadioButtonList rbtn)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItemCollection items = null;

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPQuery query = new SPQuery();

                            query.Query = "" +
                                    "<OrderBy><FieldRef Ascending='TRUE' Name='Sort_x0020_Order'/></OrderBy>";

                            items = oList.GetItems(query);
                        }
                        catch { }
                    }


                    for (int i = 0; i < items.Count; i++)
                    {
                        item = new ListItem();
                        item.Text = items[i]["Title"].ToString();
                        item.Value = items[i]["ID"].ToString();
                        item.Attributes.Add("title", items[i]["Title"].ToString() + "_" + items[i]["ID"].ToString());
                        rbtn.Items.Add(item);
                    }
                }
            }            
        }

        void rbtnParentProject_load()
        {
            rdParentProjectChoice.Items.Add("Yes");
            rdParentProjectChoice.Items.Add("No");
        }

               
        void rbtnyonBudgett_load()
        {
            rbtnyonBudget.Items.Add("Yes");
            rbtnyonBudget.Items.Add("No");

        }

        void chkPmBranch_load()
        {
            rbtnPmBranch.Items.Add("Data Management");
            rbtnPmBranch.Items.Add("Policy and Planning");
            rbtnPmBranch.Items.Add("Infrastructure");
            rbtnPmBranch.Items.Add("Systems");
        }

        private int GetItemIndex(SPListItem Item, DropDownList ddl, String colName)
        {
            String text = GetItemText(Item, colName);

            SPFieldLookupValue value = new SPFieldLookupValue(text);

            int index = ddl.Items.IndexOf(ddl.Items.FindByValue(value.LookupId.ToString()));

            return index;
        }

       
        private String GetItemText(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
                    return Text;
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }


        private void GetitemChecked(SPListItem Item, String fielname, CheckBoxList chk)
        {
            if (Item[fielname] != null) 
            {
                string values = Item[fielname].ToString();
                string[] choices = null;

                if (values != null)
                {
                    choices = values.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                }

                for (int i = 0; i < choices.Length; i++)
                {
                    ListItem listItem = chk.Items.FindByText(choices[i]);
                    if (listItem != null) listItem.Selected = true;
                }
            }

        }

        void GetItemCheckBox(SPListItem Item, String fieldname, CheckBox chkbox)
        {
            if (Item[fieldname] != null)
            {
                string value = Item[fieldname].ToString();
                if (value == "Yes")
                {
                    chkbox.Checked = true;
                }
            }
        }

        void GetItemCheckedRadio(SPListItem Item, String fieldname, RadioButtonList rbtn)
        {
            if (Item[fieldname] != null)
            {
                string value = Item[fieldname].ToString();
                if (value != null)
                {
                    ListItem listItems = rbtn.Items.FindByText(value);
                    if (listItems != null) listItems.Selected = true;
                }
            }

        }        

        protected Boolean IsValidForm()
        {
            if (txtProjectName.Text.Length > 0)
            {
                lblProjectNameValidator.Visible = false;

                if (DataTimeStartDate.SelectedDate <= DateTimeEndDate.SelectedDate)
                {
                    lblDateValidator.Visible = false;
                    if (peProposedManager.ResolvedEntities.Count > 0)
                    {
                        lblvalidatorProposedManager.Visible = false;
                        if (rbtnPmBranch.SelectedItem != null)
                        {
                            lblvalidatorPmBranch.Visible = false;
                            return true;
                        }
                        else
                        {
                            lblvalidatorPmBranch.Visible = true;
                            return false;
                        }
                    }
                    else
                    {
                        lblvalidatorProposedManager.Visible = true;
                        return false;
                    }
                }
                else
                {
                    lblDateValidator.Visible = true;
                    return false;
                }

            }
            else
            {
                lblProjectNameValidator.Visible = true;
                return false;
            }
        }        

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if ( IsDraftRecord() || IsValidForm() )
            {

                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(PMLCSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            updatePIFRecord(currentUser, web, false);
                            Page.Response.Redirect(SPContext.Current.Site.Url);
                        }
                    }
                });
            }
        }


        protected void btnSubmitApproval_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                SPUser currentUser = SPContext.Current.Web.CurrentUser;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(PMLCSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {

                            updatePIFRecord(currentUser, web, true);
                            Page.Response.Redirect(SPContext.Current.Site.Url);
                        }
                    }

                });
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    string SiteUrl = web.Url + "/Lists/PIF List/AllItems.aspx";
                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        protected void btnChangeDecision_Click(object sender, EventArgs e)
        {

            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(PMLCSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        updatePIFRecord(currentUser, web, false);

                        SPListItem item = GetCurrentItem();

                        String form = "/_layouts/CFTC.PMLC.Pages/ChangeDecision.aspx";

                        string SiteUrl = web.Url + form + "?RecordID=" + item["ID"].ToString() + "&WorkflowStatus=" + item["Workflow_x0020_Status"].ToString();

                        Page.Response.Redirect(SiteUrl);
                    }
                }
            });
        }

        private void updatePIFRecord(SPUser author, SPWeb web, Boolean isDraft)
        {
            web.AllowUnsafeUpdates = true;

            SPListItemCollection listItems = web.Lists["PIF List"].Items;

            int RecordId = int.Parse(Context.Request.QueryString["RecordID"].ToString());

            SPListItem item = listItems.GetItemById(RecordId);

            item["Title"] = txtProjectName.Text;

            //set the project title
            SPFieldUrlValue spFieldUrlValue = new SPFieldUrlValue();
            spFieldUrlValue.Url = item["Project_x0020_Title"].ToString().Substring(0, item["Project_x0020_Title"].ToString().IndexOf(","));
            spFieldUrlValue.Description = txtProjectName.Text;
            item["Project_x0020_Title"] = spFieldUrlValue;

            SPFieldUrlValue spViewFieldUrlValue = new SPFieldUrlValue();
            spViewFieldUrlValue.Url = item["Project_x0020_View_x0020_Title"].ToString().Substring(0, item["Project_x0020_View_x0020_Title"].ToString().IndexOf(","));
            spViewFieldUrlValue.Description = txtProjectName.Text;
            item["Project_x0020_View_x0020_Title"] = spViewFieldUrlValue;

            item["Project_x0020_Type"] = new SPFieldLookupValue(Convert.ToInt16(ddlProjectType.SelectedValue), ddlProjectType.SelectedItem.Text);
            item["Description"] = txtDescription.Text;
            item["Parent_x0020_Project_x0020_Choic"] = rdParentProjectChoice.SelectedItem;

            if (ddlParentProject.Items.Count > 0)
            {
                if (!ddlParentProject.SelectedValue.Equals("0"))
                {
                    item["Parent_x0020_Project"] = new SPFieldLookupValue(int.Parse(ddlParentProject.SelectedValue), ddlParentProject.SelectedItem.Text);
                }
            }

            if (!ddlPrimary.SelectedValue.Equals("0"))
            {
                item["Strategic_x0020_Alignment"] = new SPFieldLookupValue(int.Parse(ddlPrimary.SelectedValue), ddlPrimary.SelectedItem.Text);
            }

            if (!ddlSecondary.SelectedValue.Equals("0"))
            {
                item["Secondary_x0020_Strategic_x0020_Alignment"] = new SPFieldLookupValue(int.Parse(ddlSecondary.SelectedValue), ddlSecondary.SelectedItem.Text);
            }

            SPFieldMultiChoiceValue values = new SPFieldMultiChoiceValue();

            for (int i = 0; i < chkSponsor.Items.Count; i++)
            {
                if (chkSponsor.Items[i].Selected)
                {
                    values.Add(chkSponsor.Items[i].Text);

                }
            }

            item["Project_x0020_Sponsor_x0020_and_"] = values;
            item["Project_x0020_Sponsor_x0020_and_0"] = txtSponsor.Text;

            //save sponsor

            ArrayList arrSponsorUsers = new ArrayList();
            PickerEntity userSponsorEntity = new PickerEntity();
            SPFieldUserValueCollection groupTeamSponsor = new SPFieldUserValueCollection();

            int totalUsersSponsor = peProjectSponsor.Accounts.Count;

            for (int i = 0; i < totalUsersSponsor; i++)
            {
                userSponsorEntity = (PickerEntity)peProjectSponsor.Entities[i];
                arrSponsorUsers.Add(userSponsorEntity.Key);
            }

            foreach (object obj in arrSponsorUsers)
            {
                String login = obj.ToString();
                SPUser user = web.EnsureUser(login);
                SPFieldUserValue val = new SPFieldUserValue(web, user.ID, user.Name);
                groupTeamSponsor.Add(val);
            }

            item["Project_x0020_Sponsor"] = groupTeamSponsor;


            SPFieldMultiChoiceValue valuesFinalProject = new SPFieldMultiChoiceValue();

            for (int i = 0; i < chkFinalProject.Items.Count; i++)
            {
                if (chkFinalProject.Items[i].Selected)
                {
                    valuesFinalProject.Add(chkFinalProject.Items[i].Text);

                }
            }

            item["Short_x0020_Description"] = txtShortDescription.Text;

            item["Final_x0020_Project_x0020_Output"] = valuesFinalProject;
            item["Final_x0020_Project_x0020_Output0"] = txtFinalProject.Text;


            item["Risk_Schedule"] = chkRiskSchedule.Checked ? "Yes" : "No";
            item["Risk_Schedule_Details"] = txtRiskSchedule.Text;

            item["Risk_Resources"] = chkRiskResources.Checked ? "Yes" : "No";
            item["Risk_Resources_Details"] = txtRiskResources.Text;

            item["Risk_Cost_Funding"] = chkRiskCostFunding.Checked ? "Yes" : "No";
            item["Risk_Cost_Funding_Details"] = txtRiskCostFunding.Text;

            item["Risk_Technology"] = chkRiskTechnology.Checked ? "Yes" : "No";
            item["Risk_Technology_Details"] = txtRiskTechnology.Text;

            item["Risk_Feasibility"] = chkRiskFeasibility.Checked ? "Yes" : "No";
            item["Risk_Feasibility_Details"] = txtRiskFeasibility.Text;

            item["Risk_Security_Privacy"] = chkRiskSecurityorPrivacy.Checked ? "Yes" : "No";
            item["Risk_Security_Privacy_Details"] = txtRiskSecurityorPrivacy.Text;

            item["Risk_Access_Data"] = chkRiskAccesstoData.Checked ? "Yes" : "No";
            item["Risk_Access_Data_Details"] = txtRiskAccesstoData.Text;

            item["Risk_Interoperability"] = chkRiskInteroperability.Checked ? "Yes" : "No";
            item["Risk_Interoperability_Details"] = txtRiskInteroperability.Text;

            item["Risk_Executive_Support"] = chkRiskExecutiveSupport.Checked ? "Yes" : "No";
            item["Risk_Executive_Support_Details"] = txtRiskExecutiveSupport.Text;

            item["Risk_Other"] = chkRiskOther.Checked ? "Yes" : "No";
            item["Risk_Other_Details"] = txtRiskOther.Text;


            SPFieldMultiChoiceValue valuesBenefits = new SPFieldMultiChoiceValue();

            for (int i = 0; i < chkBenefits.Items.Count; i++)
            {
                if (chkBenefits.Items[i].Selected)
                {
                    valuesBenefits.Add(chkBenefits.Items[i].Text);

                }
            }

            item["Summary_x0020_of_x0020_Benefits"] = valuesBenefits;
            item["Summary_x0020_of_x0020_Benefits_"] = txtSummaryOther.Text;

            //saved Proposed Project Team
            ArrayList arrUsers = new ArrayList();
            PickerEntity userEntity = new PickerEntity();
            SPFieldUserValueCollection groupTeam = new SPFieldUserValueCollection();

            int totalUsers = peTeam.Accounts.Count;

            for (int i = 0; i < totalUsers; i++)
            {
                userEntity = (PickerEntity)peTeam.Entities[i];
                arrUsers.Add(userEntity.Key);
            }

            foreach (object obj in arrUsers)
            {
                String login = obj.ToString();
                SPUser user = web.EnsureUser(login);
                SPFieldUserValue val = new SPFieldUserValue(web, user.ID, user.Name);
                groupTeam.Add(val);
            }

            item["Proposed_x0020_Project_x0020_Tea"] = groupTeam;

            item["Rough_x0020_Order_x0020_of_x00200"] = txtDescriptionRoughOrder.Text;
            item["Is_x0020_this_x0020_project_x002"] = rbtnyonBudget.SelectedItem;
            item["ProcureID"] = txtIDprocedure.Text;
            //This is a user control
            //item["Proposed_x0020_Project_x0020_Tea"] = txtProposedProject.Text;
            item["Estimated_x0020_Project_x0020_Du"] = rbtnProjectDuration.SelectedItem;
            //ask for start and end date

            if (!DataTimeStartDate.IsDateEmpty)
            {
                item["Suggested_x0020_Start_x0020_Date"] = DataTimeStartDate.SelectedDate;
            }

            if (!DateTimeEndDate.IsDateEmpty)
            {
                item["Suggested_x0020_End_x0020_Date"] = DateTimeEndDate.SelectedDate;
            }

            item["Configuration_x0020_Management"] = chkConfigurationManagement.Checked ? "Yes" : "No";
            item["Configuration_x0020_Management_x"] = txtCMDetails.Text;

            item["Data_x0020_Management"] = chkDataManagement.Checked ? "Yes" : "No";
            item["Data_x0020_Management_x0020_Deta"] = txtDMDetails.Text;

            item["Enterprise_x0020_Architecture"] = chkEnterpriseArchitecture.Checked ? "Yes" : "No";
            item["Enterprise_x0020_Architecture_x0"] = txtEADetails.Text;

            item["Enterprise_x0020_Services"] = chkEnterpriseServices.Checked ? "Yes" : "No";
            item["Enterprise_x0020_Services_x0020_"] = txtEnterpriseServices.Text;

            item["Infrastructure_x0020_Customer"] = chkInfrastructureCustomer.Checked ? "Yes" : "No";
            item["Infrastructure_x0020_Customer_x0"] = txtInfrastructureCustomer.Text;

            item["Infrastructure_x0020_Network"] = chkInfrastructureNetwork.Checked ? "Yes" : "No";
            item["Infrastructure_x0020_Network_x00"] = txtInfrastructureNetwork.Text;

            item["Infrastructure_x0020_Telecom"] = chkInfrastructureTelecom.Checked ? "Yes" : "No";
            item["Infrastructure_x0020_Telecom_x00"] = txtInfrastructureTelecom.Text;

            item["ODT_x0020_Communications"] = chkODTCommunications.Checked ? "Yes" : "No";
            item["ODT_x0020_Communications_x0020_D"] = txtODTCommunications.Text;

            item["Privacy_x0020_Office"] = chkPrivacyOffice.Checked ? "Yes" : "No";
            item["Privacy_x0020_Office_x0020_Detai"] = txtPrivacyOffice.Text;

            item["Records_x0020_Management"] = chkRecordManagement.Checked ? "Yes" : "No";
            item["Records_x0020_Management_x0020_D"] = txtRecordManagement.Text;

            item["Section_x0020_508"] = chkSection.Checked ? "Yes" : "No";
            item["Section_x0020_508_x0020_Details"] = txtSection.Text;

            item["Security_x0020_Impact_x0020_Asse"] = chkSecurityImpact.Checked ? "Yes" : "No";
            item["Security_x0020_Impact_x0020_Asse0"] = txtSecurityImpact.Text;

            //saved Proposed PM
            ArrayList arrProposedUsers = new ArrayList();
            PickerEntity userProposedEntity = new PickerEntity();
            SPFieldUserValueCollection groupProposedTeam = new SPFieldUserValueCollection();

            for (int i = 0; i < peProposedManager.Accounts.Count; i++)
            {
                userProposedEntity = (PickerEntity)peProposedManager.Entities[i];
                arrProposedUsers.Add(userProposedEntity.Key);
            }

            foreach (object obj in arrProposedUsers)
            {
                String login = obj.ToString();
                SPUser user = web.EnsureUser(login);
                SPFieldUserValue val = new SPFieldUserValue(web, user.ID, user.Name);
                groupProposedTeam.Add(val);
            }

            item["Proposed_x0020_Project_x0020_Man"] = groupProposedTeam;

            item["PM_x0020_Branch"] = rbtnPmBranch.SelectedItem;

            //attach files
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFile PostedFile = Request.Files[i];

                if (PostedFile.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(PostedFile.FileName);

                    Stream fStream = PostedFile.InputStream;
                    byte[] contents = new byte[fStream.Length];
                    fStream.Position = 0;

                    fStream.Read(contents, 0, (int)fStream.Length);
                    fStream.Close();

                    item.Attachments.Add(FileName, contents);
                }
            }

            item["Editor"] = author;

            if ( isDraft )
                item["Workflow_x0020_Status"] = "Submitted";

            item.Update();

            web.AllowUnsafeUpdates = false;
        }
        
        public void BindAttachmentData()
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem currentItem = null;

                    if (Page.Request.QueryString["RecordId"] != null)
                    {
                        int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());
                    
                        SPList list = web.Lists["PIF List"];
                        currentItem = list.Items.GetItemById(RecordId);                        
                    }

                    DataTable dt = new DataTable();

                    dt.Columns.Add("Filename");
                    dt.Columns.Add("FilenameUrl");

                    for (int i = 0; i < currentItem.Attachments.Count; i++)
                    {
                        String filename = currentItem.Attachments[i];

                        String attachmentAbsoluteURL =
                                currentItem.Attachments.UrlPrefix
                                + filename;

                        SPFile attachmentFile = web.GetFile(attachmentAbsoluteURL);

                        String listUrl = web.Url + "/" + currentItem.ParentList.RootFolder.Url;
                        String attachmentUrl = listUrl + "/attachments/" + currentItem.ID + "/" + attachmentFile.Name;

                        dt.Rows.Add();
                        dt.Rows[i]["Filename"] = attachmentFile.Name;
                        dt.Rows[i]["FilenameUrl"] = attachmentUrl;
                    }

                    dlfiles.DataSource = dt;
                    dlfiles.DataBind();
                }
            }
            
        }

        private string[] args = null;

        public string GetCallbackResult()
        {

            StringWriter sr = new StringWriter();
            HtmlTextWriter htm = new HtmlTextWriter(sr);
            this.dlfiles.RenderControl(htm);
            return sr.ToString();
            htm.Close();
            sr.Close();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            this.args = eventArgument.Split(',');

            DeleteAttachment(args[0].ToString());

            BindAttachmentData();
        }

        private void DeleteAttachment(String leafName)
        {
            SPListItem item = GetCurrentItem();

            item.Attachments.Delete(leafName);

            item.Update();
        }

        public SPListItem GetCurrentItem()
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(PMLCSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        SPList list = web.Lists["PIF List"];

                        item = list.Items.GetItemById(RecordId);
                    }
                }
            }

            return item;
        }

        public Boolean IsDraftRecord()
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(PMLCSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["PIF List"];
                        item = list.Items.GetItemById(RecordId);
                    }
                }
            }

            if (item["Workflow_x0020_Status"].ToString().Equals("Draft"))
                return true;
            else
                return false;

        }

        public static System.Guid PMLCSiteUrl
        {
            get
            {
                return SPContext.Current.Site.ID;
            }
        }
    }
}
