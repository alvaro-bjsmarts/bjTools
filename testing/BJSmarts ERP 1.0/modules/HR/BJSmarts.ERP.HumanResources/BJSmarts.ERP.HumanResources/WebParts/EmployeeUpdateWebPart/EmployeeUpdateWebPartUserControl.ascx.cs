using System;
using System.Web;
using System.IO;
using System.Data;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Collections;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using System.Text.RegularExpressions;
using BJSmarts.ERP.SharePoint.Entities;
using System.Data.SqlClient;
using System.Data;


namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeUpdateWebPart
{
    public partial class EmployeeUpdateWebPartUserControl : SharePointUserControl, ICallbackEventHandler
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;
		private string[] args = null;

		protected void Page_Load(object sender, EventArgs e)
        {
			
            this.Page.Form.Enctype = "multipart/form-data";
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
									
								txtFirstName.Text = GetItemText(ListItem, "First_x0020_Name");
								txtLastName.Text = GetItemText(ListItem, "Last_x0020_Name");
								txtEmail.Text = GetItemText(ListItem, "Email");
								txtHomePhone.Text = GetItemText(ListItem, "Home_x0020_Phone");
								txtWorkPhone.Text = GetItemText(ListItem, "Work_x0020_Phone");
								txtMovilPhone.Text = GetItemText(ListItem, "Movil_x0020_Phone");
								if(ListItem["Hire_x0020_Date"] != null)
								{
								DateTime SuggestedStartDate;
								DateTime.TryParse(GetItemText(ListItem, "Hire_x0020_Date"), out SuggestedStartDate);
								HireDate.SelectedDate = SuggestedStartDate;
								}									
								if(ListItem["Birth_x0020_Date"] != null)
								{
								DateTime SuggestedStartDate;
								DateTime.TryParse(GetItemText(ListItem, "Birth_x0020_Date"), out SuggestedStartDate);
								BirthDate.SelectedDate = SuggestedStartDate;
								}									
								txtSalary.Text = GetItemText(ListItem, "Salary");									
								txtEmployeeGovernmentId.Text = GetItemText(ListItem, "EmployeeGovernmentId");
								txtBiometricEmployeeId.Text = GetItemText(ListItem, "BiometricEmployeeId");
								
														LoadingLookupOneToOne("Employee Types", ddlEmployeeType,"Name","Sort_Order", "EmployeeTypeId", true, intLCID );
														ddlEmployeeType.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlEmployeeType, "Employee_x0020_Type");
								
														LoadingLookupOneToOne("Resource Types", ddlResourceType,"Name","Sort_Order", "ResourceTypeId", true, intLCID );
														ddlResourceType.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlResourceType, "Resource_x0020_Type");
								
														LoadingLookupOneToOne("Genders", ddlGender,"Name","Sort_Order", "GenderId", true, intLCID );
														ddlGender.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlGender, "Gender");
								
														LoadingLookupOneToOne("Currency Type", ddlCurrency,"Name","Sort_Order", "CurrencyTypeId", true, intLCID );
														ddlCurrency.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlCurrency, "Currency");
								
														LoadingLookupOneToOne("Marital Status", ddlMaritalStatus,"Name","Sort_Order", "MaritalStatusId", true, intLCID );
														ddlMaritalStatus.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlMaritalStatus, "Marital_x0020_Status");
								
														LoadingLookupOneToOne("Employee Termination Type", ddlEmployeeTermination_x0020,"Name","Sort_Order", "EmployeeTerminationTypeId", true, intLCID );
														ddlEmployeeTermination_x0020.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlEmployeeTermination_x0020, "Employee_x0020_Termination_x0020");
								txtHomeAddressStreet_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_");
								txtHomeAddressStreet_0.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Street_0");
								txtHomeAddressCity.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_City");
								txtHomeAddressState_x.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_State_x");
								txtHomeAddressPostal_.Text = GetItemText(ListItem, "Home_x0020_Address_x0020_Postal_");
								
														LoadingLookupOneToOne("Countries", ddlHomeAddressCountry,"Name","Sort_Order", "CountryId", true, intLCID );
														ddlHomeAddressCountry.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlHomeAddressCountry, "Home_x0020_Address_x0020_Country");
								txtWorkAddressStreet_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_");
								txtWorkAddressStreet_0.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Street_0");
								txtWorkAddressCity.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_City");
								txtWorkAddressState_x.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_State_x");
								txtWorkAddressPostal_.Text = GetItemText(ListItem, "Work_x0020_Address_x0020_Postal_");
								
														LoadingLookupOneToOne("Countries", ddlWorkAddressCountry,"Name","Sort_Order", "CountryId", true, intLCID );
														ddlWorkAddressCountry.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlWorkAddressCountry, "Work_x0020_Address_x0020_Country");
								
														LoadingLookupOneToOne("Departments", ddlDepartment,"Name","Sort_Order", "DepartmentId", true, intLCID );
														ddlDepartment.SelectedIndex = GetItemIndexBCSField(ListItem,  ddlDepartment, "Department");
								try
								{
									BindAttachmentData();
								}
								catch (Exception ex)
								{
									 SharePointContext.Current.Log.Error(ex.Message, ex.StackTrace);
								}
							}
                        }
                    });
				}
            }
		}

		private void loadLocalizedValueFields()
        {
			labelFirstName.Text = getLocalizedValue("FirstNameText", intLCID,"EmployeesResourceFile");
			labelLastName.Text = getLocalizedValue("LastNameText", intLCID,"EmployeesResourceFile");
			labelEmail.Text = getLocalizedValue("EmailText", intLCID,"EmployeesResourceFile");
			labelHomePhone.Text = getLocalizedValue("HomePhoneText", intLCID,"EmployeesResourceFile");
			labelWorkPhone.Text = getLocalizedValue("WorkPhoneText", intLCID,"EmployeesResourceFile");
			labelMovilPhone.Text = getLocalizedValue("MovilPhoneText", intLCID,"EmployeesResourceFile");
			labelHireDate.Text = getLocalizedValue("HireDateText", intLCID,"EmployeesResourceFile");
			labelBirthDate.Text = getLocalizedValue("BirthDateText", intLCID,"EmployeesResourceFile");
			labelSalary.Text = getLocalizedValue("SalaryText", intLCID,"EmployeesResourceFile");
			labelEmployeeGovernmentId.Text = getLocalizedValue("EmployeeGovernmentIdText", intLCID,"EmployeesResourceFile");
			labelBiometricEmployeeId.Text = getLocalizedValue("BiometricEmployeeIdText", intLCID,"EmployeesResourceFile");
			labelEmployeeType.Text = getLocalizedValue("EmployeeTypeText", intLCID,"EmployeesResourceFile");
			labelResourceType.Text = getLocalizedValue("ResourceTypeText", intLCID,"EmployeesResourceFile");
			labelGender.Text = getLocalizedValue("GenderText", intLCID,"EmployeesResourceFile");
			labelCurrency.Text = getLocalizedValue("CurrencyText", intLCID,"EmployeesResourceFile");
			labelMaritalStatus.Text = getLocalizedValue("MaritalStatusText", intLCID,"EmployeesResourceFile");
			labelEmployeeTermination_x0020.Text = getLocalizedValue("EmployeeTermination_x0020Text", intLCID,"EmployeesResourceFile");
			labelHomeAddressStreet_.Text = getLocalizedValue("HomeAddressStreet_Text", intLCID,"EmployeesResourceFile");
			labelHomeAddressStreet_0.Text = getLocalizedValue("HomeAddressStreet_0Text", intLCID,"EmployeesResourceFile");
			labelHomeAddressCity.Text = getLocalizedValue("HomeAddressCityText", intLCID,"EmployeesResourceFile");
			labelHomeAddressState_x.Text = getLocalizedValue("HomeAddressState_xText", intLCID,"EmployeesResourceFile");
			labelHomeAddressPostal_.Text = getLocalizedValue("HomeAddressPostal_Text", intLCID,"EmployeesResourceFile");
			labelHomeAddressCountry.Text = getLocalizedValue("HomeAddressCountryText", intLCID,"EmployeesResourceFile");
			labelWorkAddressStreet_.Text = getLocalizedValue("WorkAddressStreet_Text", intLCID,"EmployeesResourceFile");
			labelWorkAddressStreet_0.Text = getLocalizedValue("WorkAddressStreet_0Text", intLCID,"EmployeesResourceFile");
			labelWorkAddressCity.Text = getLocalizedValue("WorkAddressCityText", intLCID,"EmployeesResourceFile");
			labelWorkAddressState_x.Text = getLocalizedValue("WorkAddressState_xText", intLCID,"EmployeesResourceFile");
			labelWorkAddressPostal_.Text = getLocalizedValue("WorkAddressPostal_Text", intLCID,"EmployeesResourceFile");
			labelWorkAddressCountry.Text = getLocalizedValue("WorkAddressCountryText", intLCID,"EmployeesResourceFile");
			labelDepartment.Text = getLocalizedValue("DepartmentText", intLCID,"EmployeesResourceFile");
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
							addRecordtoDB(web);
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

				

		private void addRecord(SPUser author, SPWeb web, Boolean IsDraft)
        {
            web.AllowUnsafeUpdates = true;

            SPListItemCollection listItems = web.Lists["Employees"].Items;

            SPListItem item;
           
				int RecordId = int.Parse(Context.Request.QueryString["RecordID"].ToString());
				item = listItems.GetItemById(RecordId);
         
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
							 SharePointContext.Current.Log.Error(ex.Message, ex.StackTrace);
						}

				item["Salary"] = Salary;
				item["EmployeeGovernmentId"] = txtEmployeeGovernmentId.Text;			
				item["BiometricEmployeeId"] = txtBiometricEmployeeId.Text;			
	
								SetExternalFieldValue(item, "Employee Type", ddlEmployeeType.SelectedItem.Text);				
	
								SetExternalFieldValue(item, "Resource Type", ddlResourceType.SelectedItem.Text);				
	
								SetExternalFieldValue(item, "Gender", ddlGender.SelectedItem.Text);				
	
								SetExternalFieldValue(item, "Currency", ddlCurrency.SelectedItem.Text);				
	
								SetExternalFieldValue(item, "Marital Status", ddlMaritalStatus.SelectedItem.Text);				
	
								SetExternalFieldValue(item, "Employee Termination Type", ddlEmployeeTermination_x0020.SelectedItem.Text);				
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
		
				
		protected void addRecordtoDB(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                String connectionString = "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;";

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                try
                {
                    if (Page.Request.QueryString["RecordId"] != null)
                    {
                        int RecordId = int.Parse(Page.Request.QueryString["RecordId"].ToString());
                     
                                web.AllowUnsafeUpdates = true;

                                SPList list = web.Lists["Employees"];
                                SPListItem item = list.Items.GetItemById(RecordId);

                                string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] (EmployeeId,FirstName,LastName,Email,EmployeeGovernmentId,HomePhoneNumber,WorkPhoneNumber,MovilPhoneNumber,HireDate,DateOfBirth,BiometricEmployeeId,Salary,EmployeTypeId,EmployeeType,ResourceTypeId,ResourceType,GenderId,Gender,CurrencyTypeId,CurrencyType,MaritalStatusId,MaritalStatus,ManagerId,EmployeeTerminationTypeId,EmployeeTerminationType,HomeStreetAddress1,HomeStreetAddress2,HomePostalCode,HomeCity,HomeStateProvince,HomeCountryId,WorkStreetAddress1,WorkStreetAddress2,WorkPostalCode,WorkCity,WorkStateProvince,WorkCountryId,DepartmentId,Department,CompanyId,OrganizationId,Organization,Deleted) " +
                                "VALUES (@EmployeeId,@FirstName,@LastName,@Email,@EmployeeGovernmentId,@HomePhoneNumber,@WorkPhoneNumber,@MovilPhoneNumber,@HireDate,@DateOfBirth,@BiometricEmployeeId,@Salary,@EmployeTypeId,@EmployeeType,@ResourceTypeId,@ResourceType,@GenderId,@Gender,@CurrencyTypeId,@CurrencyType,@MaritalStatusId,@MaritalStatus,@ManagerId,@EmployeeTerminationTypeId,@EmployeeTerminationType,@HomeStreetAddress1,@HomeStreetAddress2,@HomePostalCode,@HomeCity,@HomeStateProvince,@HomeCountryId,@WorkStreetAddress1,@WorkStreetAddress2,@WorkPostalCode,@WorkCity,@WorkStateProvince,@WorkCountryId,@DepartmentId,@Department,@CompanyId,@OrganizationId,@Organization,@Deleted)";

								SqlCommand cmd = new SqlCommand(sql, connection);

													  cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = item["First_x0020_Name"].ToString();
													  cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = item["Last_x0020_Name"].ToString();
													  cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = item["Email"].ToString();

                                                      if ( item["Email"] != null )
													  cmd.Parameters.Add("@EmployeeGovernmentId", SqlDbType.NVarChar, 50).Value = item["EmployeeGovernmentId"].ToString();

                                                        
													  cmd.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = item[""] != null ? DateTime.Parse(item["Hire_x0020_Date"].ToString()) : DateTime.Now;
													  cmd.Parameters.Add("@BiometricEmployeeId", SqlDbType.NVarChar, 50).Value = item["BiometricEmployeeId"].ToString();
													  cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = decimal.Parse(item["Salary"].ToString());
													  cmd.Parameters.Add("@EmployeeType", SqlDbType.NVarChar, 50).Value = item["Employee_x0020_Type"].ToString();
													  cmd.Parameters.Add("@ResourceType", SqlDbType.NVarChar, 50).Value = item["Resource_x0020_Type"].ToString();
													  cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 50).Value = item["Gender"].ToString();
													  cmd.Parameters.Add("@MaritalStatus", SqlDbType.NVarChar, 50).Value = item["Marital_x0020_Status"].ToString();
													  cmd.Parameters.Add("@EmployeeTerminationType", SqlDbType.NVarChar, 50).Value = item["Employee_x0020_Termination_x0020"].ToString();
													  cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = item["Department"].ToString();
                                                         

                                cmd.ExecuteNonQuery();                      
                   }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.ToString(), ex);
                    Console.Write(ex.ToString());

                }
                finally
                {
                    connection.Close();
                }
            });
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
    }
}
