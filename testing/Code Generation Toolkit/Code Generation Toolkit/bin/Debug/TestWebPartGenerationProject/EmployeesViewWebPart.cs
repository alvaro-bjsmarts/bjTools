



using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace TestWebPartGenerationProject
{
	[Serializable]
	public partial class ViewEmployeesControl : UserControl
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
								
									lblTitle.Text = GetItemText(ListItem, "Title");
									lblEmployeeId.Text = GetItemText(ListItem, "EmployeeId");
									lblFirstName.Text = GetItemText(ListItem, "First_x0020_Name");
									lblLastName.Text = GetItemText(ListItem, "Last_x0020_Name");
									lblEmail.Text = GetItemText(ListItem, "Email");
									lblHomePhone.Text = GetItemText(ListItem, "Home_x0020_Phone");
									lblWorkPhone.Text = GetItemText(ListItem, "Work_x0020_Phone");
									lblMovilPhone.Text = GetItemText(ListItem, "Movil_x0020_Phone");
									if(ListItem["Hire_x0020_Date"] != null)
									{
										DateTime SuggestedStartDate;
										DateTime.TryParse(GetItemText(ListItem, "Hire_x0020_Date"), out SuggestedStartDate);
										lblHireDate.Text  = SuggestedStartDate.ToString("MM/dd/yyyy");
									}									
									if(ListItem["Birth_x0020_Date"] != null)
									{
										DateTime SuggestedStartDate;
										DateTime.TryParse(GetItemText(ListItem, "Birth_x0020_Date"), out SuggestedStartDate);
										lblBirthDate.Text  = SuggestedStartDate.ToString("MM/dd/yyyy");
									}									
									lblSalary.Text = GetItemText(ListItem, "Salary");									
									lblEmployeeGovernmentId.Text = GetItemText(ListItem, "EmployeeGovernmentId");
									lblBiometricEmployeeId.Text = GetItemText(ListItem, "BiometricEmployeeId");
										lblEmployeeType.Text = GetItemTextDropdownlistExternal(ListItem, "Employee_x0020_Type");
										lblResourceType.Text = GetItemTextDropdownlistExternal(ListItem, "Resource_x0020_Type");
										lblGender.Text = GetItemTextDropdownlistExternal(ListItem, "Gender");
										lblCurrency.Text = GetItemTextDropdownlistExternal(ListItem, "Currency");
										lblMaritalStatus.Text = GetItemTextDropdownlistExternal(ListItem, "Marital_x0020_Status");
										lblEmployeeTermination_x0020.Text = GetItemTextDropdownlistExternal(ListItem, "Employee_x0020_Termination_x0020");
									lblHomeAddressStreet_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_");
									lblHomeAddressStreet_0.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_0");
									lblHomeAddressCity.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_City");
									lblHomeAddressState_x.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_State_x");
									lblHomeAddressPostal_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Postal_");
										lblHomeAddressCountry.Text = GetItemTextDropdownlistExternal(ListItem, "Home_x0020_Address_x0020_Country");
									lblWorkAddressStreet_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_");
									lblWorkAddressStreet_0.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_0");
									lblWorkAddressCity.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_City");
									lblWorkAddressState_x.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_State_x");
									lblWorkAddressPostal_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Postal_");
										lblWorkAddressCountry.Text = GetItemTextDropdownlistExternal(ListItem, "Work_x0020_Address_x0020_Country");
										lblDepartment.Text = GetItemTextDropdownlistExternal(ListItem, "Department");
										lblOrganization.Text = GetItemTextDropdownlistExternal(ListItem, "Organization");
									lblDeleted.Text = GetItemTextChoice(ListItem, "Deleted");
								try
                                {
                                    BindAttachmentData();
                                }
                                catch (Exception ex)
								{
									LogError(ex.Message, ex.StackTrace);
								}
							}
						}
                    });	
				}
			}
		}


		private void loadLocalizedValueFields()
        {
			labelTitle.Text = getLocalizedValue("TitleText", intLCID);
			labelEmployeeId.Text = getLocalizedValue("EmployeeIdText", intLCID);
			labelFirstName.Text = getLocalizedValue("FirstNameText", intLCID);
			labelLastName.Text = getLocalizedValue("LastNameText", intLCID);
			labelEmail.Text = getLocalizedValue("EmailText", intLCID);
			labelHomePhone.Text = getLocalizedValue("HomePhoneText", intLCID);
			labelWorkPhone.Text = getLocalizedValue("WorkPhoneText", intLCID);
			labelMovilPhone.Text = getLocalizedValue("MovilPhoneText", intLCID);
			labelHireDate.Text = getLocalizedValue("HireDateText", intLCID);
			labelBirthDate.Text = getLocalizedValue("BirthDateText", intLCID);
			labelSalary.Text = getLocalizedValue("SalaryText", intLCID);
			labelEmployeeGovernmentId.Text = getLocalizedValue("EmployeeGovernmentIdText", intLCID);
			labelBiometricEmployeeId.Text = getLocalizedValue("BiometricEmployeeIdText", intLCID);
			labelEmployeeType.Text = getLocalizedValue("EmployeeTypeText", intLCID);
			labelResourceType.Text = getLocalizedValue("ResourceTypeText", intLCID);
			labelGender.Text = getLocalizedValue("GenderText", intLCID);
			labelCurrency.Text = getLocalizedValue("CurrencyText", intLCID);
			labelMaritalStatus.Text = getLocalizedValue("MaritalStatusText", intLCID);
			labelEmployeeTermination_x0020.Text = getLocalizedValue("EmployeeTermination_x0020Text", intLCID);
			labelHomeAddressStreet_.Text = getLocalizedValue("HomeAddressStreet_Text", intLCID);
			labelHomeAddressStreet_0.Text = getLocalizedValue("HomeAddressStreet_0Text", intLCID);
			labelHomeAddressCity.Text = getLocalizedValue("HomeAddressCityText", intLCID);
			labelHomeAddressState_x.Text = getLocalizedValue("HomeAddressState_xText", intLCID);
			labelHomeAddressPostal_.Text = getLocalizedValue("HomeAddressPostal_Text", intLCID);
			labelHomeAddressCountry.Text = getLocalizedValue("HomeAddressCountryText", intLCID);
			labelWorkAddressStreet_.Text = getLocalizedValue("WorkAddressStreet_Text", intLCID);
			labelWorkAddressStreet_0.Text = getLocalizedValue("WorkAddressStreet_0Text", intLCID);
			labelWorkAddressCity.Text = getLocalizedValue("WorkAddressCityText", intLCID);
			labelWorkAddressState_x.Text = getLocalizedValue("WorkAddressState_xText", intLCID);
			labelWorkAddressPostal_.Text = getLocalizedValue("WorkAddressPostal_Text", intLCID);
			labelWorkAddressCountry.Text = getLocalizedValue("WorkAddressCountryText", intLCID);
			labelDepartment.Text = getLocalizedValue("DepartmentText", intLCID);
			labelEditTitle.Text = getLocalizedValue("EditTitleText", intLCID);
			labelViewTitle.Text = getLocalizedValue("ViewTitleText", intLCID);
			labelOrganization.Text = getLocalizedValue("OrganizationText", intLCID);
			labelDeleted.Text = getLocalizedValue("DeletedText", intLCID);
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

		

		private String GetItemTextExternalDataSP(String listname, SPListItem Item, String colName,String Idfield, String showfield)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
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

                                    query.Query = "<Where><Eq><FieldRef Name='" + Idfield + "'/><Value Type='Number'>" + Convert.ToInt64(Text) + "</Value></Eq></Where>";

                                    items = oList.GetItems(query);

                                    for (int i = 0; i < items.Count; i++)
                                    {
                                        Text = items[i][showfield].ToString();
                                    }

                                    return Text;
                                }
                                catch (Exception ex)
								{
									LogError(ex.Message, ex.StackTrace);
								}
                            }

                        }
                    }
                }
                catch
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }
		
		private String GetItemTextDropdownlistExternal(SPListItem Item, String colName)
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

		private string getLocalizedValue(string strInput, int intLCID)
        {            
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "EmployeesResourceFile", (uint)intLCID);
        }

		protected void b_Click(object sender, EventArgs e)
        {
             if (Page.Request.QueryString["RecordID"] != null)
             {
                 Response.Redirect("/Pages/EmployeesUpdatePage.aspx?RecordId=" + Page.Request.QueryString["RecordID"].ToString());
             }
        }

		private void LogError(String Message, String StackTrace)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
						web.AllowUnsafeUpdates = true;
                        SPList ErrorLogList = web.Lists["Application Errors"];

                        SPListItemCollection items = ErrorLogList.Items;
                        SPListItem item = items.Add();

                        item["Title"] = Message;
                        item["StackTrace"] = StackTrace;

                        item.Update();
						web.AllowUnsafeUpdates = false;
                    }
                }

            });
        }
	}
}
