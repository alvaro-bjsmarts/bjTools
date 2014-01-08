using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using BJSmarts.ERP.SharePoint.Entities;

namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeViewWebPart
{
    public partial class EmployeeViewWebPartUserControl : SharePointUserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

        protected void Page_Load(object sender, EventArgs e)
        {
            SPContext currentContext = SPContext.Current;

            if (!Page.IsPostBack)
            {
                if (Page.Request.QueryString["RecordID"] != null)
                {

                    loadLocalizedValueFields();

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPListItem ListItem;

                                ListItem = GetCurrentItem();

                                lblEmployeeId.Text = GetItemText(ListItem, "EmployeeId");
                                lblFirstName.Text = GetItemText(ListItem, "First_x0020_Name");
                                lblLastName.Text = GetItemText(ListItem, "Last_x0020_Name");
                                lblEmail.Text = GetItemText(ListItem, "Email");
                                lblHomePhone.Text = GetItemText(ListItem, "Home_x0020_Phone");
                                lblWorkPhone.Text = GetItemText(ListItem, "Work_x0020_Phone");
                                lblMovilPhone.Text = GetItemText(ListItem, "Movil_x0020_Phone");
                                if (ListItem["Hire_x0020_Date"] != null)
                                {
                                    DateTime SuggestedStartDate;
                                    DateTime.TryParse(GetItemText(ListItem, "Hire_x0020_Date"), out SuggestedStartDate);
                                    lblHireDate.Text = SuggestedStartDate.ToString("MM/dd/yyyy");
                                }
                                if (ListItem["Birth_x0020_Date"] != null)
                                {
                                    DateTime SuggestedStartDate;
                                    DateTime.TryParse(GetItemText(ListItem, "Birth_x0020_Date"), out SuggestedStartDate);
                                    lblBirthDate.Text = SuggestedStartDate.ToString("MM/dd/yyyy");
                                }
                                lblSalary.Text = GetItemText(ListItem, "Salary");
                                lblEmployeeGovernmentId.Text = GetItemText(ListItem, "EmployeeGovernmentId");
                                lblBiometricEmployeeId.Text = GetItemText(ListItem, "BiometricEmployeeId");

                                lblEmployeeType.Text = GetItemTextExternalDataSP("Employee Types", ListItem, "Employee_x0020_Type", "EmployeeTypeId", "Name");

                                lblResourceType.Text = GetItemTextExternalDataSP("Resource Types", ListItem, "Resource_x0020_Type", "ResourceTypeId", "Name");

                                lblGender.Text = GetItemTextExternalDataSP("Genders", ListItem, "Gender", "GenderId", "Name");

                                lblCurrency.Text = GetItemTextExternalDataSP("Currency Type", ListItem, "Currency", "CurrencyTypeId", "Name");

                                lblMaritalStatus.Text = GetItemTextExternalDataSP("Marital Status", ListItem, "Marital_x0020_Status", "MaritalStatusId", "Name");

                                lblEmployeeTermination_x0020.Text = GetItemTextExternalDataSP("Employee Termination Type", ListItem, "Employee_x0020_Termination_x0020", "EmployeeTerminationTypeId", "Name");
                                lblHomeAddressStreet_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_");
                                lblHomeAddressStreet_0.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_0");
                                lblHomeAddressCity.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_City");
                                lblHomeAddressState_x.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_State_x");
                                lblHomeAddressPostal_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Postal_");

                                lblHomeAddressCountry.Text = GetItemTextExternalDataSP("Countries", ListItem, "Home_x0020_Address_x0020_Country", "CountryId", "Name");
                                lblWorkAddressStreet_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_");
                                lblWorkAddressStreet_0.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_0");
                                lblWorkAddressCity.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_City");
                                lblWorkAddressState_x.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_State_x");
                                lblWorkAddressPostal_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Postal_");

                                lblWorkAddressCountry.Text = GetItemTextExternalDataSP("Countries", ListItem, "Work_x0020_Address_x0020_Country", "CountryId", "Name");

                                lblDepartment.Text = GetItemTextExternalDataSP("Departments", ListItem, "Department", "DepartmentId", "Name");
                                try
                                {
                                    BindAttachmentData();
                                }
                                catch (Exception ex)
                                {
                                    SharePointContext.Current.Log.Error(ex.Message, ex.StackTrace);
                                    //LogError(ex.Message, ex.StackTrace);
                                }
                            }
                        }
                    });
                }
            }
        }


        private void loadLocalizedValueFields()
        {
            labelEmployeeId.Text = getLocalizedValue("EmployeeIdText", intLCID, "EmployeesResourceFile");
            labelFirstName.Text = getLocalizedValue("FirstNameText", intLCID, "EmployeesResourceFile");
            labelLastName.Text = getLocalizedValue("LastNameText", intLCID, "EmployeesResourceFile");
            labelEmail.Text = getLocalizedValue("EmailText", intLCID, "EmployeesResourceFile");
            labelHomePhone.Text = getLocalizedValue("HomePhoneText", intLCID, "EmployeesResourceFile");
            labelWorkPhone.Text = getLocalizedValue("WorkPhoneText", intLCID, "EmployeesResourceFile");
            labelMovilPhone.Text = getLocalizedValue("MovilPhoneText", intLCID, "EmployeesResourceFile");
            labelHireDate.Text = getLocalizedValue("HireDateText", intLCID, "EmployeesResourceFile");
            labelBirthDate.Text = getLocalizedValue("BirthDateText", intLCID, "EmployeesResourceFile");
            labelSalary.Text = getLocalizedValue("SalaryText", intLCID, "EmployeesResourceFile");
            labelEmployeeGovernmentId.Text = getLocalizedValue("EmployeeGovernmentIdText", intLCID, "EmployeesResourceFile");
            labelBiometricEmployeeId.Text = getLocalizedValue("BiometricEmployeeIdText", intLCID, "EmployeesResourceFile");
            labelEmployeeType.Text = getLocalizedValue("EmployeeTypeText", intLCID, "EmployeesResourceFile");
            labelResourceType.Text = getLocalizedValue("ResourceTypeText", intLCID, "EmployeesResourceFile");
            labelGender.Text = getLocalizedValue("GenderText", intLCID, "EmployeesResourceFile");
            labelCurrency.Text = getLocalizedValue("CurrencyText", intLCID, "EmployeesResourceFile");
            labelMaritalStatus.Text = getLocalizedValue("MaritalStatusText", intLCID, "EmployeesResourceFile");
            labelEmployeeTermination_x0020.Text = getLocalizedValue("EmployeeTermination_x0020Text", intLCID, "EmployeesResourceFile");
            labelHomeAddressStreet_.Text = getLocalizedValue("HomeAddressStreet_Text", intLCID, "EmployeesResourceFile");
            labelHomeAddressStreet_0.Text = getLocalizedValue("HomeAddressStreet_0Text", intLCID, "EmployeesResourceFile");
            labelHomeAddressCity.Text = getLocalizedValue("HomeAddressCityText", intLCID, "EmployeesResourceFile");
            labelHomeAddressState_x.Text = getLocalizedValue("HomeAddressState_xText", intLCID, "EmployeesResourceFile");
            labelHomeAddressPostal_.Text = getLocalizedValue("HomeAddressPostal_Text", intLCID, "EmployeesResourceFile");
            labelHomeAddressCountry.Text = getLocalizedValue("HomeAddressCountryText", intLCID, "EmployeesResourceFile");
            labelWorkAddressStreet_.Text = getLocalizedValue("WorkAddressStreet_Text", intLCID, "EmployeesResourceFile");
            labelWorkAddressStreet_0.Text = getLocalizedValue("WorkAddressStreet_0Text", intLCID, "EmployeesResourceFile");
            labelWorkAddressCity.Text = getLocalizedValue("WorkAddressCityText", intLCID, "EmployeesResourceFile");
            labelWorkAddressState_x.Text = getLocalizedValue("WorkAddressState_xText", intLCID, "EmployeesResourceFile");
            labelWorkAddressPostal_.Text = getLocalizedValue("WorkAddressPostal_Text", intLCID, "EmployeesResourceFile");
            labelWorkAddressCountry.Text = getLocalizedValue("WorkAddressCountryText", intLCID, "EmployeesResourceFile");
            labelDepartment.Text = getLocalizedValue("DepartmentText", intLCID, "EmployeesResourceFile");
        }

        public SPListItem GetCurrentItem()
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Employees"];
                        item = list.Items.GetItemById(RecordId);
                    }
                }
            }

            return item;
        }


        //private String GetItemText(SPListItem Item, String colName)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
        //            return Text;
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }

        //    }
        //    return String.Empty;
        //}

        //private String GetItemTextChoice(SPListItem Item, String colName)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

        //            return Text;
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }

        //    }
        //    return String.Empty;
        //}


        //private String GetItemTextDropdownlist(SPListItem Item, String colName)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
        //            return Text.Substring(3);
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }
        //    }
        //    return String.Empty;
        //}



        //private String GetItemTextExternalDataSP(String listname, SPListItem Item, String colName, String Idfield, String showfield)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

        //            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
        //            {
        //                using (SPWeb web = site.OpenWeb())
        //                {
        //                    SPListItemCollection items = null;

        //                    if (web != null)
        //                    {
        //                        try
        //                        {
        //                            SPList oList = web.Lists[listname];

        //                            SPQuery query = new SPQuery();

        //                            query.Query = "<Where><Eq><FieldRef Name='" + Idfield + "'/><Value Type='Number'>" + Convert.ToInt64(Text) + "</Value></Eq></Where>";

        //                            items = oList.GetItems(query);

        //                            for (int i = 0; i < items.Count; i++)
        //                            {
        //                                Text = items[i][showfield].ToString();
        //                            }

        //                            return Text;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            SharePointContext.Current.Log.Error(ex.Message, ex.StackTrace);
        //                            //LogError(ex.Message, ex.StackTrace);
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }

        //    }
        //    return String.Empty;
        //}

        //private String GetItemTextDropdownlistExternal(SPListItem Item, String colName)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);
        //            return Text;
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }
        //    }
        //    return String.Empty;
        //}

        public void BindAttachmentData()
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListItem currentItem = null;

                    if (Page.Request.QueryString["RecordId"] != null)
                    {
                        int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());

                        SPList list = web.Lists["Employees"];
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


        //private String GetItemTextUser(SPListItem Item, String colName)
        //{
        //    if (Item != null)
        //    {
        //        try
        //        {
        //            String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

        //            string[] newchoices = null;
        //            string[] choices = null;
        //            string aux = null;
        //            string newaux = null;

        //            if (Text != null)
        //            {
        //                choices = Text.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
        //            }

        //            for (int i = 0; i < choices.Length; i++)
        //            {
        //                if (i != 0)
        //                {
        //                    decimal res = i % 2;
        //                    if (res != 0)
        //                    {
        //                        newaux += choices[i] + " ";
        //                    }
        //                }
        //            }

        //            if (Text != null)
        //            {
        //                newchoices = newaux.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
        //            }

        //            for (int i = 0; i < newchoices.Length; i++)
        //            {
        //                aux += newchoices[i] + " ";
        //            }
        //            aux = aux.Remove(aux.Length - 1);

        //            return aux;
        //        }
        //        catch
        //        {
        //            return String.Empty;
        //        }

        //    }
        //    return String.Empty;
        //}

        //private string getLocalizedValue(string strInput, int intLCID)
        //{
        //    return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "EmployeesResourceFile", (uint)intLCID);
        //}

        protected void b_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RecordID"] != null)
            {
                Response.Redirect("/Pages/EmployeesUpdatePage.aspx?RecordId=" + Page.Request.QueryString["RecordID"].ToString());
            }
        }

        //private void LogError(String Message, String StackTrace)
        //{
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.Url))
        //        {
        //            using (SPWeb web = site.OpenWeb())
        //            {
        //                web.AllowUnsafeUpdates = true;
        //                SPList ErrorLogList = web.Lists["Application Errors"];

        //                SPListItemCollection items = ErrorLogList.Items;
        //                SPListItem item = items.Add();

        //                item["Title"] = Message;
        //                item["StackTrace"] = StackTrace;

        //                item.Update();
        //                web.AllowUnsafeUpdates = false;
        //            }
        //        }

        //    });
        //}

    }
}
