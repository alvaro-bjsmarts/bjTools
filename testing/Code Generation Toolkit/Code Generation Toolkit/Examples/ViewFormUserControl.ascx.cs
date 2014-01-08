using System;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Data;

namespace CFTC.PMLC.View.Webpart.ViewForm
{
    public partial class ViewFormUserControl : UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
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

                                

                                lblProjectName.Text = GetItemText(ListItem, "Title");
                                lblProjectType.Text = GetItemTextDropdownlist(ListItem, "Project_x0020_Type");
                                lblShortDescription.Text = GetItemText(ListItem, "Short_x0020_Description");
                                lblDescription.Text = GetItemText(ListItem, "Description").Replace(Environment.NewLine, "<br />");
                                lblParentProjectChoice.Text = GetItemTextYesNoDropdownList(ListItem, "Parent_x0020_Project_x0020_Choic");
                                lblParentProject.Text = GetItemTextDropdownlist(ListItem, "Parent_x0020_Project");
                                lblPrimary.Text = GetItemTextDropdownlistSubstring(ListItem, "Strategic_x0020_Alignment");
                                lblSecondary.Text = GetItemTextDropdownlistSubstring(ListItem, "Secondary_x0020_Strategic_x0020_Alignment");
                                lblSponsor.Text = GetitemChecked(ListItem, "Project_x0020_Sponsor_x0020_and_");
                                lbltxtSponsorOther.Text = GetItemText(ListItem, "Project_x0020_Sponsor_x0020_and_0");
                                lbltxtSponsor.Text = GetItemTextUser(ListItem, "Project_x0020_Sponsor");
                                lblFinalProject.Text = GetitemChecked(ListItem, "Final_x0020_Project_x0020_Output");
                                lbltxtFinalProject.Text = GetItemText(ListItem, "Final_x0020_Project_x0020_Output0");
                                lblRiskSchedule.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Schedule");
                                lblRiskScheduleDescription.Text = GetItemText(ListItem, "Risk_Schedule_Details");
                                lblRiskResources.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Resources");
                                lblRiskResourcesDescription.Text = GetItemText(ListItem, "Risk_Resources_Details");
                                lblRiskCostFunding.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Cost_Funding");
                                lblRiskCostFundingDescpription.Text = GetItemText(ListItem, "Risk_Cost_Funding_Details");
                                lblAccessData.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Access_Data");
                                lblAccessDataDescription.Text = GetItemText(ListItem, "Risk_Access_Data_Details");
                                lblSecurityPrivacy.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Security_Privacy");
                                lblSecurityPrivacyDescription.Text = GetItemText(ListItem, "Risk_Security_Privacy_Details");
                                lblTechnology.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Technology");
                                lblTechnologyDescription.Text = GetItemText(ListItem, "Risk_Technology_Details");
                                lblInteroperability.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Interoperability");
                                lblInteroperabilityDescription.Text = GetItemText(ListItem, "Risk_Interoperability_Details");
                                lblFeasibility.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Feasibility");
                                lblFeasabilityDescription.Text = GetItemText(ListItem, "Risk_Feasibility_Details");
                                lblExecutiveSupport.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Executive_Support");
                                lblExecutiveSupportDescription.Text = GetItemText(ListItem, "Risk_Executive_Support_Details");
                                lblRiskOther.Text = GetItemTextYesNoDropdownList(ListItem, "Risk_Other");
                                lblRiskOtherDescription.Text = GetItemText(ListItem, "Risk_Other_Details");
                                lblBenefits.Text = GetitemChecked(ListItem, "Summary_x0020_of_x0020_Benefits");
                                lbltxtSummaryOther.Text = GetItemText(ListItem, "Summary_x0020_of_x0020_Benefits_");
                                lbltxtBudgetEstimate.Text = GetItemText(ListItem, "Rough_x0020_Order_x0020_of_x00200");
                                lblyonBudget.Text = GetItemTextYesNoDropdownList(ListItem, "Is_x0020_this_x0020_project_x002");
                                lbltxtIDprocedure.Text = GetItemText(ListItem, "ProcureID");
                                lblProjectDuration.Text = GetItemTextYesNoDropdownList(ListItem, "Estimated_x0020_Project_x0020_Du");

                                if (ListItem["Suggested_x0020_Start_x0020_Date"] != null)
                                {
                                    DateTime SuggestedStartDate;
                                    DateTime.TryParse(GetItemText(ListItem, "Suggested_x0020_Start_x0020_Date"), out SuggestedStartDate);
                                    lblDataTimeStartDate.Text = SuggestedStartDate.ToString("MM/dd/yyyy");
                                }
                                if (ListItem["Suggested_x0020_End_x0020_Date"] != null)
                                {
                                    DateTime SuggestedEndDate;
                                    DateTime.TryParse(GetItemText(ListItem, "Suggested_x0020_End_x0020_Date"), out SuggestedEndDate);
                                    lblDateTimeEndDate.Text = SuggestedEndDate.ToString("MM/dd/yyyy");
                                }
                              
                                lblConfiguration.Text = GetItemTextYesNoDropdownList(ListItem, "Configuration_x0020_Management");
                                lblConfigurationDescription.Text = GetItemText(ListItem, "Configuration_x0020_Management_x");
                                lblDataManagement.Text = GetItemTextYesNoDropdownList(ListItem, "Data_x0020_Management");
                                lblDataManagementDescription.Text = GetItemText(ListItem, "Data_x0020_Management_x0020_Deta");
                                lblEnterprise.Text = GetItemTextYesNoDropdownList(ListItem, "Enterprise_x0020_Architecture");
                                lblEnterpriseDescription.Text = GetItemText(ListItem, "Enterprise_x0020_Architecture_x0");
                                lblServices.Text = GetItemTextYesNoDropdownList(ListItem, "Enterprise_x0020_Services");
                                lblServicesDescription.Text = GetItemText(ListItem, "Enterprise_x0020_Services_x0020_");
                                lblInfrastructure.Text = GetItemTextYesNoDropdownList(ListItem, "Infrastructure_x0020_Customer");
                                lblInfrastructureDescription.Text = GetItemText(ListItem, "Infrastructure_x0020_Customer_x0");
                                lblNetwork.Text = GetItemTextYesNoDropdownList(ListItem, "Infrastructure_x0020_Network");
                                lblNetworkDescription.Text = GetItemText(ListItem, "Infrastructure_x0020_Network_x00");
                                lblTelecom.Text = GetItemTextYesNoDropdownList(ListItem, "Infrastructure_x0020_Telecom");
                                lblTelecomDescription.Text = GetItemText(ListItem, "Infrastructure_x0020_Telecom_x00");
                                lblODTCommunications.Text = GetItemTextYesNoDropdownList(ListItem, "ODT_x0020_Communications");
                                lblODTCommunicationsDescription.Text = GetItemText(ListItem, "ODT_x0020_Communications_x0020_D");
                                lblPrivacyOffice.Text = GetItemTextYesNoDropdownList(ListItem, "Privacy_x0020_Office");
                                lblPrivacyOfficeDescription.Text = GetItemText(ListItem, "Privacy_x0020_Office_x0020_Detai");
                                lblRecordsManagement.Text = GetItemTextYesNoDropdownList(ListItem, "Records_x0020_Management");
                                lblRecordsManagementDescription.Text = GetItemText(ListItem, "Records_x0020_Management_x0020_D");
                                lblSection508.Text = GetItemTextYesNoDropdownList(ListItem, "Section_x0020_508");
                                lblSection508Description.Text =GetItemText(ListItem, "Section_x0020_508_x0020_Details");
                                lblSecurityImpact.Text = GetItemTextYesNoDropdownList(ListItem, "Security_x0020_Impact_x0020_Asse");
                                lblSecurityImpactDescription.Text = GetItemText(ListItem, "Security_x0020_Impact_x0020_Asse0");                                
                                lblProposedPm.Text = GetItemTextUser(ListItem, "Proposed_x0020_Project_x0020_Man");
                                lblPMBranch.Text = GetItemTextChoice(ListItem, "PM_x0020_Branch");
                                lblpeTeam.Text = GetItemTextUser(ListItem, "Proposed_x0020_Project_x0020_Tea");
                                lblSubmittedBy.Text = GetItemTextUser(ListItem, "Author");

                                DateTime FinalDecisionDate;
                                DateTime.TryParse(GetItemText(ListItem, "Final_x0020_Decision_x0020_Date"), out FinalDecisionDate);

                                if (GetItemTextUser(ListItem, "Final_x0020_Decision_x0020_By") != string.Empty)
                                {
                                    string input = GetItemText(ListItem, "Comments");
                                    lblComments.Text = input.Replace(Environment.NewLine, "<br>");
                                }

                                if(GetItemTextChoice(ListItem, "Workflow_x0020_Status")=="Approved")
                                {
                                    lblMessage.Text = GetItemTextChoice(ListItem, "Workflow_x0020_Status");                          
                                    lblMessage.CssClass = "colorApprove";
                                }

                                if(GetItemTextChoice(ListItem, "Workflow_x0020_Status")=="Rejected")
                                {
                                    lblMessage.Text = GetItemTextChoice(ListItem, "Workflow_x0020_Status");                          
                                    lblMessage.CssClass = "colorRejected";                               
                                }

                                if(GetItemTextChoice(ListItem, "Workflow_x0020_Status")=="Put On Hold")
                                {
                                    lblMessage.Text = GetItemTextChoice(ListItem, "Workflow_x0020_Status");
                                    lblMessage.CssClass = "colorPutOnHold";
                                }

                                if (GetItemTextChoice(ListItem, "Workflow_x0020_Status").ToUpper() == "DEFER TO ITL")
                                {
                                    lblMessage.Text = GetItemTextChoice(ListItem, "Workflow_x0020_Status");
                                    lblMessage.CssClass = "colorDeferToITL";
                                }                                        

                                DateTime SubmittedDate;
                                DateTime.TryParse(GetItemText(ListItem, "Created"), out SubmittedDate);
                                lblSubmittedDate.Text = SubmittedDate.ToString("MM/dd/yyyy");
                                BindAttachmentData();       

                                SPUser author = getAuthor(ListItem, web);
                                SPUser projectManager = getProjectPM(ListItem, web);

                                EnableWorkflowButtons(currentUser, author, projectManager, ListItem, IsCurrentUserITLeader);
                            }
                        }
                    });
                }
            }                       
        }

        private void EnableWorkflowButtons(SPUser CurrentUser, SPUser Author, SPUser ProjectManager, SPListItem item, Boolean IsCurrentUserITLeader)      
        {

            DebugStmt("Workflow Status: " + item["Workflow_x0020_Status"].ToString() + "<br>");
            DebugStmt("Current User belongs to IT Leadership: " + IsCurrentUserITLeader + "<br>");
            DebugStmt("Current User is a site collection administrator: " + CurrentUser.IsSiteAdmin + "<br>");
            DebugStmt("Current User Name: " + CurrentUser.Name + "<br>");
            DebugStmt("Creator Name: " + Author.Name + "<br>");
            DebugStmt("Project Manager: " + ProjectManager.Name + "<br>");

            
            if ( ( CurrentUser.Name.Equals( Author.Name ) ) || ( CurrentUser.Name.Equals(ProjectManager.Name ) ) )
            {
                lbPrintPreview.Visible = true;                
                lbEdit.Visible = true;
                lbVersionHistory.Visible = true;

                if (item["Workflow_x0020_Status"].ToString().Equals("Rejected"))
                {
                    lbReSubmit.Visible = true;
                }
            }

            if ((IsCurrentUserITLeader) || (CurrentUser.IsSiteAdmin))
            {
                if (item["Workflow_x0020_Status"].ToString().Equals("Approved"))
                {
                    lbPrintPreview.Visible = true;
                    lbVersionHistory.Visible = true;
                    lbEdit.Visible = true;
                }
                else if (item["Workflow_x0020_Status"].ToString().Equals("Rejected"))
                {
                    lbPrintPreview.Visible = true;
                    lbVersionHistory.Visible = true;
                    lbEdit.Visible = true;
                }
                else if (item["Workflow_x0020_Status"].ToString().Equals("Submitted"))
                {
                    lbPrintPreview.Visible = true;
                    lbVersionHistory.Visible = true;
                    lbEdit.Visible = true;
                    lbReject.Visible = true;
                    lbApprove.Visible = true;
                    lbPutonHold.Visible = true;
                    lbDeferToITL.Visible = true;
                }
                else if (item["Workflow_x0020_Status"].ToString().Equals("Put On Hold"))
                {
                    lbPrintPreview.Visible = true;
                    lbVersionHistory.Visible = true;
                    lbEdit.Visible = true;
                    lbReject.Visible = true;
                    lbApprove.Visible = true;
                }
                else if (item["Workflow_x0020_Status"].ToString().ToUpper().Equals("DEFER TO ITL"))
                {
                    lbPrintPreview.Visible = true;
                    lbVersionHistory.Visible = true;
                    lbEdit.Visible = true;
                    lbReject.Visible = true;
                    lbApprove.Visible = true;
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

        private SPUser getProjectPM(SPListItem item, SPWeb web)
        {
            SPUser ProjectPM = null;



            SPFieldUserValue userValue = new SPFieldUserValue(
                                                            web,
                                                            item["Proposed_x0020_Project_x0020_Man"].ToString());

            ProjectPM = getUser(userValue.LookupValue, web);

            return ProjectPM;
        }

        private SPUser getFinalDecisionUser(SPListItem item, SPWeb web)
        {
            
            SPUser DecisionUser = null;

            SPFieldUserValue userValue = new SPFieldUserValue(
                                                        web,
                                                        item["Final_x0020_Decision_x0020_By"].ToString());

            DecisionUser = getUser(userValue.LookupValue, web);

            return DecisionUser;
                            
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
        
        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {

            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem item = GetCurrentItem();

                    String printPreviewForm = GetConfigurationValue("Print Preview Form");

                    Page.Response.Redirect( web.Url + printPreviewForm + "?RecordID=" + item["ID"].ToString());
                }
            }
        }

        protected void btnVersionHistory_Click(object sender, EventArgs e)
        {

            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String SourceUrl = item.Web.Url.ToString() +
                                        GetConfigurationValue("View PIF Form") + "?RecordID=" +
                                        item["ID"].ToString();

                    String versionForm = GetConfigurationValue("Version Form");

                    Page.Response.Redirect(web.Url + versionForm + "?ID=" + item["ID"].ToString() + "&List=" + item.ParentList.ID + "&Source=" + SourceUrl);
                }
            }
        }

        protected void btnReSubmit_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Confirmation PIF Form");

                    string SiteUrl = web.Url + confirmationform + "?RecordID=" + item["ID"].ToString() + "&Action=ReSubmit";

                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Update PIF Form");

                    string SiteUrl = confirmationform + "?RecordID=" + item["ID"].ToString();

                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Confirmation PIF Form");

                    string SiteUrl = web.Url + confirmationform + "?RecordID=" + item["ID"].ToString() + "&Action=Reject";

                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {

            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Confirmation PIF Form");

                    String SiteUrl = web.Url + confirmationform + "?RecordID=" + item["ID"].ToString() + "&Action=Approval";

                    Page.Response.Redirect(SiteUrl);
                }
            }

        }

        protected void btnPutOnHold_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Confirmation PIF Form");

                    string SiteUrl = web.Url + confirmationform + "?RecordID=" + item["ID"].ToString() + "&Action=PutOnHold";

                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        protected void btnDeferToITL_Click(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPListItem item = GetCurrentItem();

                    String confirmationform = GetConfigurationValue("Confirmation PIF Form");

                    string SiteUrl = web.Url + confirmationform + "?RecordID=" + item["ID"].ToString() + "&Action=DeferToITL";

                    Page.Response.Redirect(SiteUrl);
                }
            }
        }

        private String GetConfigurationValue(String key)
        {
            String value = String.Empty;

            using (SPSite site = new SPSite(PMLCSiteUrl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["PMLC Configuration List"];

                    SPQuery query = new SPQuery();

                    query.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + key + "</Value></Eq></Where>";

                    SPListItemCollection items = list.GetItems(query);

                    value = items[0]["Value"].ToString();

                    return value;
                }
            }
        }

        public static System.Guid PMLCSiteUrl
        {
            get
            {
                return SPContext.Current.Site.ID;                
            }
        }

        private int GetItemIndex(SPListItem Item, DropDownList ddl, String colName)
        {
            String text = GetItemText(Item, colName);

            SPFieldLookupValue value = new SPFieldLookupValue(text);

            int index = ddl.Items.IndexOf(ddl.Items.FindByValue(value.LookupId.ToString()));

            return index;
        }


        private String GetitemChecked(SPListItem Item, String fielname)
        {
            if (Item[fielname] != null)
            {
                string values = Item[fielname].ToString();
                string[] choices = null;
                string aux=null;

                if (values != null)
                {
                    choices = values.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                }

                for (int i = 0; i < choices.Length; i++)
                {
                    aux += choices[i]+" ";
                }
                aux = aux.Remove(aux.Length - 2);
                return aux.Substring(1);
            }
            return String.Empty;

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

        private String GetItemTextUser(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

                    string[] newchoices=null;
                    string[] choices = null;
                    string aux = null;
                    string newaux = null;

                    if (Text != null)
                    {
                        choices = Text.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                                        
                    for (int i = 0; i < choices.Length; i++)
                    {
                        if (i != 0)
                        {
                            decimal res = i % 2;
                            if (res != 0)
                            {
                              newaux += choices[i]+ " ";
                            }
                        }
                    }

                    if (Text != null)
                    {
                        newchoices = newaux.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    
                    for (int i = 0; i < newchoices.Length; i++)
                    {
                        aux += newchoices[i] + " ";
                    }
                    aux = aux.Remove(aux.Length - 1);

                    return aux;
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }


        private String GetItemTextYesNoDropdownList(SPListItem Item, String colName)
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

        private String GetItemTextDropdownlist(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
                    return Text.Substring(3);                    
                }
                catch
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }



        private String GetItemTextDropdownlistSubstring(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
                    int index = Text.IndexOf("#");
                    if (index > 0)
                        Text = Text.Substring(index+1);
                    return Text;
                }
                catch
                {
                    return String.Empty;
                }
            }
            return String.Empty;
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


        private String GetItemTextChoice(SPListItem Item, String colName)
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

        private void DebugStmt(String stmt)
        {
            if ((Page.Request.QueryString["Debug"] != null) && (Page.Request.QueryString["Debug"].ToString().Equals("True")))
            {
                Page.Response.Write(stmt);
            }
        }
    }
}
