using System;
using System.Web.UI;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Configuration;


namespace CFTC.PMLC.Insert.Webpart.InsertForm
{
    public partial class InsertFormUserControl : UserControl
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            if (!Page.IsPostBack)
            {

                LoadingLookupTable("PIF List", ddlParentProject, "ID", true);
                LoadingLookupTable("Project Type List", ddlProjectType, "Sort_x0020_Order", true);
                LoadingLookupTable("Strategic Alignment List", ddlPrimary, "Sort_x0020_Order", true);
                LoadingLookupTable("Strategic Alignment List", ddlSecondary, "Sort_x0020_Order", true);
                LoadingChoiceTable("Project Sponsor List", chkSponsor);
                LoadingChoiceTable("Final Project Output List", chkFinalProject);
                LoadingChoiceTable("Benefits List", chkBenefits);
                LoadingRadioChoiceTable("Project Duration List", rbtnProjectDuration);
                chkPmBranch_load();
                rbtnParentProject_load();
                rbtnyonBudgett_load();
              
                lblDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

                if (SPContext.Current.Site.Url.Length > 0)
                {
                    using (SPSite site = new SPSite(PMLCSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPContext currentContext = SPContext.Current;
                            SPUser sUser = web.CurrentUser;

                            if (currentContext != null && currentContext.Web.CurrentUser != null)
                            {

                                lbluserPIF.Text = sUser.Name;
                            }
                            else
                            {
                                lbluserPIF.Text = System.Web.HttpContext.Current.User.Identity.Name;
                            }
                        }
                    }
                }
            }
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
                        item.Selected = true;
                        ddl.Items.Add(item);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = items[i]["Title"].ToString();
                        item.Value = items[i]["ID"].ToString();
                        item.Attributes.Add("title", items[i]["Title"].ToString() + "_" + items[i]["ID"].ToString());
                        ddl.Items.Add(item);
                    }
                }
            }                       
        }


        void LoadingChoiceTable(String listname,CheckBoxList chk)
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
                        ListItem item = new ListItem();
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
                        ListItem item = new ListItem();
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

        protected SPUser GetCurrentUser()
        {
            SPUser user;

            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    user = web.CurrentUser;
                }
            }

            return user;
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

        protected void btnSaveAsDraft_Click(object sender, EventArgs e)
        {
            if (txtProjectName.Text.Length <= 0)
            {
                lblProjectNameValidator.Visible = true;
            }
            else
            {
                SPUser currentUser = GetCurrentUser();

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(PMLCSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            addPIFRecord(currentUser, web, true);
                            Page.Response.Redirect(SPContext.Current.Site.Url);
                        }
                    }
                });
            }
        }

        

        protected void btnOK_Click(object sender, EventArgs e)
        {

            if (IsValidForm())
            {
                SPUser currentUser = GetCurrentUser();

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                        using (SPSite site = new SPSite(PMLCSiteUrl))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                addPIFRecord(currentUser, web, false);
                                Page.Response.Redirect(SPContext.Current.Site.Url);
                            }
                        }
                });
            }
        }

        protected void lnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Site.Url);       
        }

        private void addPIFRecord(SPUser author, SPWeb web, Boolean IsDraft)
        {
            web.AllowUnsafeUpdates = true;

            SPListItemCollection listItems = web.Lists["PIF List"].Items;

            SPListItem item = listItems.Add();

            item["Title"] = txtProjectName.Text;
            item["Project_x0020_Type"] = new SPFieldLookupValue(Convert.ToInt16(ddlProjectType.SelectedValue), ddlProjectType.SelectedItem.Text);
            item["Description"] = Server.HtmlEncode(txtDescription.Text);
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
            item["Estimated_x0020_Project_x0020_Du"] = rbtnProjectDuration.SelectedItem;

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


            //saved Proposed Project Team
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

            if (IsDraft)
                item["Workflow_x0020_Status"] = "Draft";

            item["Author"] = author;
            item["Editor"] = author;

            item.Update();

            web.AllowUnsafeUpdates = false;
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
