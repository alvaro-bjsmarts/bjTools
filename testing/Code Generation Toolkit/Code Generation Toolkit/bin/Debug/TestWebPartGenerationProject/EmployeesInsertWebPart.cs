

using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace TestWebPartGenerationProject
{
	[Serializable]
	public partial class InsertEmployeesControl : UserControl
	{
		private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

		protected void Page_Load(object sender, EventArgs e)
        {
			this.Page.Form.Enctype = "multipart/form-data";

            if (!Page.IsPostBack)
            {
			 loadLocalizedValueFields();
						LoadingBCSLookupTable("HR EmployeeTypes", ddlEmployeeType, "SORT_ORDER", true);
						LoadingBCSLookupTable("HR ResourceTypes", ddlResourceType, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Genders", ddlGender, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Currency Type", ddlCurrency, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Marital Status", ddlMaritalStatus, "SORT_ORDER", true);
						LoadingBCSLookupTable("HR EmployeeTerminationType", ddlEmployeeTermination_x0020, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Countries", ddlHomeAddressCountry, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Countries", ddlWorkAddressCountry, "SORT_ORDER", true);
						LoadingBCSLookupTable("HR Departments", ddlDepartment, "SORT_ORDER", true);
						LoadingBCSLookupTable("ERP Organizations", ddlOrganization, "SORT_ORDER", true);
				LoadingDropdownTable("Employees", "Deleted", ddlDeleted, "Sort_x0020_Order", false);
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

		protected void btnOK_Click(object sender, EventArgs e)
        {
			if(Page.IsValid)
            {             
				SPUser currentUser = GetCurrentUser();

				SPSecurity.RunWithElevatedPrivileges(delegate()
				{
					using (SPSite site = new SPSite(SPContext.Current.Site.Url))
					{
						using (SPWeb web = site.OpenWeb())
						{
							addRecord(currentUser, web, false);
							Page.Response.Redirect(SPContext.Current.Site.Url);
						}
					}
				});
			}           
        }

		 protected SPUser GetCurrentUser()
        {
            SPUser user;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    user = web.CurrentUser;
                }
            }

            return user;
        }

		void LoadingDropdownTable(String listname, String fieldname, DropDownList ddl, string sortfield, Boolean addInitialValue)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    List<string> fieldList=new List<String>();

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPFieldChoice field = (SPFieldChoice)oList.Fields[fieldname];

                            fieldList = new List<string>();

                            foreach (string str in field.Choices)
                            {                                
                                fieldList.Add(str);
                            }

                        }
                        catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}
                    }

                    if (addInitialValue)
                    {
                        ListItem item = new ListItem();
                        item.Text = " ";
                        item.Selected = true;                   
                        ddl.Items.Add(item);
                    }

                    foreach (string item in fieldList)
                    {
                        ddl.Items.Add(new ListItem(item));
                    }
                }
            }
        }


	void LoadingLookupOneToOne(String listname, DropDownList ddl, string showfield, string sortfield, string IDfield, Boolean addInitialValue, int intLCID)
        {
            if (intLCID == 1033)
            {
                intLCID = 0;
            }

            if (intLCID == 3082)
            {
                intLCID = 1;
            }

            bool comprove = false;
            bool sortcomprove = false;
            bool deletedcomprove = false;

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

                            for (int i = 0; i < oList.Fields.Count; i++)
                            {
                                if (oList.Fields[i].Title == "Language")
                                {
                                    comprove = true;
                                }
                                if (oList.Fields[i].Title == sortfield)
                                {
                                    sortcomprove = true;
                                }
                                if (oList.Fields[i].Title == "Deleted")
                                {
                                    deletedcomprove = true;
                                }
                            }

                            if (comprove == true && sortcomprove == true && deletedcomprove == true)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == true && deletedcomprove == false)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == true && deletedcomprove == true)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == true && sortcomprove == false && deletedcomprove == true)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == true && sortcomprove == false && deletedcomprove == false)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == true && sortcomprove == true && deletedcomprove == false)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == false && deletedcomprove == true)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == false && sortcomprove == false && deletedcomprove == false)
                            {
                                items = oList.GetItems();

                            }

                        }
                        catch (Exception ex)
                        {
                            LogError(ex.Message, ex.StackTrace);
                        }
                    }

                    if (addInitialValue)
                    {
                        ListItem item = new ListItem();
                        item.Text = "(None)";
                        item.Value = "0";
                        item.Attributes.Add("showfield", "None_0");
                        item.Selected = true;
                        ddl.Items.Add(item);
                    }

                    for (int i = 0; i < items.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = items[i][showfield].ToString();
                        item.Value = items[i][IDfield].ToString();
                        item.Attributes.Add(showfield, items[i][showfield].ToString() + "_" + items[i][IDfield].ToString());
                        ddl.Items.Add(item);
                    }
                }
            }
        }

		void LoadingRadioChoiceTableOneToOne(String listname, RadioButtonList rbtn, string showfield, string sortfield, string IDfield, Boolean addInitialValue, int intLCID)
        {
            if (intLCID == 1033)
            {
                intLCID = 0;
            }

				if (intLCID == 3082)
            {
                intLCID = 1;
            }

            bool comprove = false;
            bool sortcomprove = false;
			bool deletedcomprove = false;

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

                            for (int i = 0; i < oList.Fields.Count; i++)
                            {
                                if (oList.Fields[i].Title == "Language")
                                {
                                    comprove = true;
                                }
                                if (oList.Fields[i].Title == sortfield)
                                {
                                    sortcomprove = true;
                                }
                                if (oList.Fields[i].Title == "Deleted")
                                {
                                    deletedcomprove = true;
                                }
                            }

                            if (comprove == true && sortcomprove == true && deletedcomprove == true)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == true && deletedcomprove == false)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == true && deletedcomprove == true)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == true && sortcomprove == false && deletedcomprove == true)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == true && sortcomprove == false && deletedcomprove == false)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == true && sortcomprove == true && deletedcomprove == false)
                            {
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq></Where>" + "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                                items = oList.GetItems(query);
                            }
                            if (comprove == false && sortcomprove == false && deletedcomprove == true)
                            {

                                SPQuery query = new SPQuery();

                                query.Query = query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>";

                                items = oList.GetItems(query);

                            }
                            if (comprove == false && sortcomprove == false && deletedcomprove == false)
                            {
                                items = oList.GetItems();

                            }

                        }
                        catch (Exception ex)
                        {
                            LogError(ex.Message, ex.StackTrace);
                        }
                    }


                    for (int i = 0; i < items.Count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = items[i][showfield].ToString();
                        item.Value = items[i][IDfield].ToString();
                        item.Attributes.Add(showfield, items[i][showfield].ToString() + "_" + items[i][IDfield].ToString());
                        rbtn.Items.Add(item);
                    }
                }
            }
        }

		
        void LoadingChoiceTable(String listname,String fieldname , CheckBoxList chk)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    List<string> fieldList = new List<String>();

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPFieldChoice field = (SPFieldChoice)oList.Fields[fieldname];

                            fieldList = new List<string>();

                            foreach (string str in field.Choices)
                            {
                                fieldList.Add(str);
                            }
                        }
                        catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}
                    }

                    foreach (string item in fieldList)
                    {
                        chk.Items.Add(new ListItem(item));
                    }
                }
            }
        }


        void LoadingRadioChoiceTable(String listname,String fieldname, RadioButtonList rbtn)
        {
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    List<string> fieldList = new List<String>();

                    if (web != null)
                    {
                        try
                        {
                            SPList oList = web.Lists[listname];

                            SPFieldChoice field = (SPFieldChoice)oList.Fields[fieldname];

                            fieldList = new List<string>();

                            foreach (string str in field.Choices)
                            {
                                fieldList.Add(str);
                            }
                        }
                        catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}
                    }

                    foreach (string item in fieldList)
                    {
                        rbtn.Items.Add(new ListItem(item));
                    }
                }
            }
        }


		void LoadingLookupTable(String listname, DropDownList ddl, string sortfield, Boolean addInitialValue)
        {

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

                            query.Query = "" +
                                    "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                            items = oList.GetItems(query);
                        }
                        catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}
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

		void LoadingBCSLookupTable(String listname, DropDownList ddl, string sortfield, Boolean addInitialValue)
        {

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

                            query.Query = "" +
                                    "<OrderBy><FieldRef Ascending='TRUE' Name='" + sortfield + "'/></OrderBy>";

                            items = oList.GetItems(query);
                        }
                        catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}
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
                        item.Text = items[i]["TITLE"].ToString();
                        item.Value = items[i]["Id"].ToString();
                        item.Attributes.Add("title", items[i]["TITLE"].ToString() + "_" + items[i]["Id"].ToString());
                        ddl.Items.Add(item);
                    }
                }
            }
        }

		private void addRecord(SPUser author, SPWeb web, Boolean IsDraft)
        {
            web.AllowUnsafeUpdates = true;

            SPListItemCollection listItems = web.Lists["Employees"].Items;

            SPListItem item = listItems.Add();

				item["Title"] = txtTitle.Text;			
				item["EmployeeId"] = txtEmployeeId.Text;			
				item["First_x0020_Name"] = txtFirstName.Text;			
				item["Last_x0020_Name"] = txtLastName.Text;			
				item["Email"] = txtEmail.Text;			
				item["Home_x0020_Phone"] = txtHomePhone.Text;			
				item["Work_x0020_Phone"] = txtWorkPhone.Text;			
				item["Movil_x0020_Phone"] = txtMovilPhone.Text;			

				if (!HireDate.IsDateEmpty)
				{
				item["Hire_x0020_Date"] = HireDate.SelectedDate;
				}									

				if (!BirthDate.IsDateEmpty)
				{
				item["Birth_x0020_Date"] = BirthDate.SelectedDate;
				}									
				Decimal Salary=0;

						try
						{
							  Salary = Decimal.Parse(txtSalary.Text);
						}
						catch (Exception ex)
						{
							LogError(ex.Message, ex.StackTrace);
						}

				item["Salary"] = Salary;
				item["EmployeeGovernmentId"] = txtEmployeeGovernmentId.Text;			
				item["BiometricEmployeeId"] = txtBiometricEmployeeId.Text;			
				       SetExternalFieldValue(item, "Employee Type", ddlEmployeeType.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Resource Type", ddlResourceType.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Gender", ddlGender.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Currency", ddlCurrency.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Marital Status", ddlMaritalStatus.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Employee Termination_x0020", ddlEmployeeTermination_x0020.SelectedItem.Text);				
				item["Home_x0020_Address_x0020_Street_"] = txtHomeAddressStreet_.Text;			
				item["Home_x0020_Address_x0020_Street_0"] = txtHomeAddressStreet_0.Text;			
				item["Home_x0020_Address_x0020_City"] = txtHomeAddressCity.Text;			
				item["Home_x0020_Address_x0020_State_x"] = txtHomeAddressState_x.Text;			
				item["Home_x0020_Address_x0020_Postal_"] = txtHomeAddressPostal_.Text;			
				       SetExternalFieldValue(item, "Home Address Country", ddlHomeAddressCountry.SelectedItem.Text);				
				item["Work_x0020_Address_x0020_Street_"] = txtWorkAddressStreet_.Text;			
				item["Work_x0020_Address_x0020_Street_0"] = txtWorkAddressStreet_0.Text;			
				item["Work_x0020_Address_x0020_City"] = txtWorkAddressCity.Text;			
				item["Work_x0020_Address_x0020_State_x"] = txtWorkAddressState_x.Text;			
				item["Work_x0020_Address_x0020_Postal_"] = txtWorkAddressPostal_.Text;			
				       SetExternalFieldValue(item, "Work Address Country", ddlWorkAddressCountry.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Department", ddlDepartment.SelectedItem.Text);				
				       SetExternalFieldValue(item, "Organization", ddlOrganization.SelectedItem.Text);				

				if(ddlDeleted.Items.Count > 0)
				{
				if(!ddlDeleted.SelectedValue.Equals("0"))
				{
				item["Deleted"] = ddlDeleted.SelectedItem.Text;
				}
				}				

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
			
			item.Update();

            web.AllowUnsafeUpdates = false;
		}

		public static void SetExternalFieldValue(SPListItem item, string fieldInternalName, string newValue)
        {
            if (item.Fields[fieldInternalName].TypeAsString == "BusinessData")
            {
                SPField myField = item.Fields[fieldInternalName];
                XmlDocument xmlData = new XmlDocument();
                xmlData.LoadXml(myField.SchemaXml);
                //Get teh internal name of the SPBusinessDataField's identity column.
                String entityName = xmlData.FirstChild.Attributes["RelatedFieldWssStaticName"].Value;

                //Set the value of the identity column.
                item[entityName] = EntityInstanceIdEncoder.EncodeEntityInstanceId(new object[] { newValue });
                item[fieldInternalName] = newValue;
            }
            else
            {
                throw new InvalidOperationException(fieldInternalName + " is not of type BusinessData");
            }
        }

		private string getLocalizedValue(string strInput, int intLCID)
        {            
           return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "EmployeesResourceFile", (uint)intLCID);
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
