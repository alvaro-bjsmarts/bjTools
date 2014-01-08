using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.BusinessData;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BJSmarts.ERP.SharePoint.Entities;
using System.Data.SqlClient;
using System.Data;

namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeInsertWebPart
{
    public partial class EmployeeInsertWebPartUserControl : SharePointUserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

		protected void Page_Load(object sender, EventArgs e)
        {
			this.Page.Form.Enctype = "multipart/form-data";

            if (!Page.IsPostBack)
            {
			 loadLocalizedValueFields();
								
									LoadingLookupOneToOne("Employee Types", ddlEmployeeType,"Name","Sort_Order", "EmployeeTypeId", true, intLCID);
									
								
									LoadingLookupOneToOne("Resource Types", ddlResourceType,"Name","Sort_Order", "ResourceTypeId", true, intLCID);
									
								
									LoadingLookupOneToOne("Genders", ddlGender,"Name","Sort_Order", "GenderId", true, intLCID);
									
								
									LoadingLookupOneToOne("Currency Type", ddlCurrency,"Name","Sort_Order", "CurrencyTypeId", true, intLCID);
									
								
									LoadingLookupOneToOne("Marital Status", ddlMaritalStatus,"Name","Sort_Order", "MaritalStatusId", true, intLCID);
									
								
									LoadingLookupOneToOne("Employee Termination Type", ddlEmployeeTermination_x0020,"Name","Sort_Order", "EmployeeTerminationTypeId", true, intLCID);
									
								
									LoadingLookupOneToOne("Departments", ddlDepartment,"Name","Sort_Order", "DepartmentId", true, intLCID);
									
								
									LoadingLookupOneToOne("Countries", ddlHomeCountry,"Name","Sort_Order", "CountryId", true, intLCID);
									
								
									LoadingLookupOneToOne("Countries", ddlWorkCountry,"Name","Sort_Order", "CountryId", true, intLCID);
									
            }
		}

		private void loadLocalizedValueFields()
        {
			labelFirstName.Text = getLocalizedValue("FirstNameText", intLCID,"EmployeesResourceFile");
			labelLastName.Text = getLocalizedValue("LastNameText", intLCID,"EmployeesResourceFile");
			labelEmail.Text = getLocalizedValue("EmailText", intLCID,"EmployeesResourceFile");
			labelHomePhoneNumber.Text = getLocalizedValue("HomePhoneNumberText", intLCID,"EmployeesResourceFile");
			labelWorkPhoneNumber.Text = getLocalizedValue("WorkPhoneNumberText", intLCID,"EmployeesResourceFile");
			labelMovilPhoneNumber.Text = getLocalizedValue("MovilPhoneNumberText", intLCID,"EmployeesResourceFile");
			labelDateOfBirth.Text = getLocalizedValue("DateOfBirthText", intLCID,"EmployeesResourceFile");
			labelHireDate.Text = getLocalizedValue("HireDateText", intLCID,"EmployeesResourceFile");
			labelSalary.Text = getLocalizedValue("SalaryText", intLCID,"EmployeesResourceFile");
			labelEmployeeGovernmentId.Text = getLocalizedValue("EmployeeGovernmentIdText", intLCID,"EmployeesResourceFile");
			labelBiometricEmployeeId.Text = getLocalizedValue("BiometricEmployeeIdText", intLCID,"EmployeesResourceFile");
			labelEmployeeType.Text = getLocalizedValue("EmployeeTypeText", intLCID,"EmployeesResourceFile");
			labelResourceType.Text = getLocalizedValue("ResourceTypeText", intLCID,"EmployeesResourceFile");
			labelGender.Text = getLocalizedValue("GenderText", intLCID,"EmployeesResourceFile");
			labelCurrency.Text = getLocalizedValue("CurrencyText", intLCID,"EmployeesResourceFile");
			labelMaritalStatus.Text = getLocalizedValue("MaritalStatusText", intLCID,"EmployeesResourceFile");
			labelEmployeeTermination_x0020.Text = getLocalizedValue("EmployeeTermination_x0020Text", intLCID,"EmployeesResourceFile");
			labelDepartment.Text = getLocalizedValue("DepartmentText", intLCID,"EmployeesResourceFile");
			labelHomeStreetAddress_.Text = getLocalizedValue("HomeStreetAddress_Text", intLCID,"EmployeesResourceFile");
			labelHomeStreetAddress_0.Text = getLocalizedValue("HomeStreetAddress_0Text", intLCID,"EmployeesResourceFile");
			labelHomePostalCode.Text = getLocalizedValue("HomePostalCodeText", intLCID,"EmployeesResourceFile");
			labelHomeCity.Text = getLocalizedValue("HomeCityText", intLCID,"EmployeesResourceFile");
			labelHomeStateProvince.Text = getLocalizedValue("HomeStateProvinceText", intLCID,"EmployeesResourceFile");
			labelHomeCountry.Text = getLocalizedValue("HomeCountryText", intLCID,"EmployeesResourceFile");
			labelWorkStreetAddress_.Text = getLocalizedValue("WorkStreetAddress_Text", intLCID,"EmployeesResourceFile");
			labelWorkStreetAddress_0.Text = getLocalizedValue("WorkStreetAddress_0Text", intLCID,"EmployeesResourceFile");
			labelWorkPostalCode.Text = getLocalizedValue("WorkPostalCodeText", intLCID,"EmployeesResourceFile");
			labelWorkCity.Text = getLocalizedValue("WorkCityText", intLCID,"EmployeesResourceFile");
			labelWorkStateProvince.Text = getLocalizedValue("WorkStateProvinceText", intLCID,"EmployeesResourceFile");
			labelWorkCountry.Text = getLocalizedValue("WorkCountryText", intLCID,"EmployeesResourceFile");
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

            SPListItem item = listItems.Add();

			item["First_x0020_Name"] = txtFirstName.Text;			
			item["Last_x0020_Name"] = txtLastName.Text;			
			item["Email"] = txtEmail.Text;			
			item["Home_x0020_Phone_x0020_Number"] = txtHomePhoneNumber.Text;			
			item["Work_x0020_Phone_x0020_Number"] = txtWorkPhoneNumber.Text;			
			item["Movil_x0020_Phone_x0020_Number"] = txtMovilPhoneNumber.Text;			

			if (!DateOfBirth.IsDateEmpty)
			{
			item["Date_x0020_Of_x0020_Birth"] = DateOfBirth.SelectedDate;
			}									

			if (!HireDate.IsDateEmpty)
			{
			item["Hire_x0020_Date"] = HireDate.SelectedDate;
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
			SetExternalFieldValue(item, "Department", ddlDepartment.SelectedItem.Text);

			item["Home_x0020_Street_x0020_Address_"] = txtHomeStreetAddress_.Text;			
			item["Home_x0020_Street_x0020_Address_0"] = txtHomeStreetAddress_0.Text;			
			item["Home_x0020_Postal_x0020_Code"] = txtHomePostalCode.Text;			
			item["Home_x0020_City"] = txtHomeCity.Text;			
			item["Home_x0020_State_x0020_Province"] = txtHomeStateProvince.Text;			
								
			SetExternalFieldValue(item, "Home Country", ddlHomeCountry.SelectedItem.Text);
			item["Work_x0020_Street_x0020_Address_"] = txtWorkStreetAddress_.Text;			
			item["Work_x0020_Street_x0020_Address_0"] = txtWorkStreetAddress_0.Text;			
			item["Work_x0020_Postal_x0020_Code"] = txtWorkPostalCode.Text;			
			item["Work_x0020_City"] = txtWorkCity.Text;			
			item["Work_x0020_State_x0020_Province"] = txtWorkStateProvince.Text;			
								
			SetExternalFieldValue(item, "Work Country", ddlWorkCountry.SelectedItem.Text);
			item["Deleted"]= 0;

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


        protected SPListItem GetLastItem()
        {

            using (SPSite oSPsite = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb oWeb = oSPsite.OpenWeb())
                {
                    SPList oList = oWeb.Lists["Employees"];
                    SPQuery query = new SPQuery();
                    query.RowLimit = 1;
                    query.Query = @"<OrderBy>
                            <FieldRef Name='ID' Ascending='False'></FieldRef>
                            </OrderBy>";
                    
                    SPListItemCollection col = oList.GetItems(query);
                    
                    if (col.Count > 0)
                    {
                        return col[0];
                    }
                }
            }

            return null;
        }

		protected void addRecordtoDB(SPWeb web)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SqlConnection connection = new SqlConnection(SharePointContext.Current.Settings.DatabaseConnectionString);
				SqlCommand cmd = new SqlCommand();

                connection.Open();

                try
                {
                    
                    SPListItem item = GetLastItem();

                    if (item != null)
                    {

                        string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] (FirstName,LastName,Email) " +
                        "VALUES (@FirstName,@LastName,@Email) ";

                        //string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] (EmployeeId,FirstName,LastName,Email,EmployeeGovernmentId,HomePhoneNumber,WorkPhoneNumber,MovilPhoneNumber,HireDate,DateOfBirth,BiometricEmployeeId,Salary,EmployeTypeId,EmployeeType,ResourceTypeId,ResourceType,GenderId,Gender,CurrencyTypeId,CurrencyType,MaritalStatusId,MaritalStatus,ManagerId,EmployeeTerminationTypeId,EmployeeTerminationType,HomeStreetAddress1,HomeStreetAddress2,HomePostalCode,HomeCity,HomeStateProvince,HomeCountryId,WorkStreetAddress1,WorkStreetAddress2,WorkPostalCode,WorkCity,WorkStateProvince,WorkCountryId,DepartmentId,Department,CompanyId,OrganizationId,Organization,Deleted) " +
                        //"VALUES (@EmployeeId,@FirstName,@LastName,@Email,@EmployeeGovernmentId,@HomePhoneNumber,@WorkPhoneNumber,@MovilPhoneNumber,@HireDate,@DateOfBirth,@BiometricEmployeeId,@Salary,@EmployeTypeId,@EmployeeType,@ResourceTypeId,@ResourceType,@GenderId,@Gender,@CurrencyTypeId,@CurrencyType,@MaritalStatusId,@MaritalStatus,@ManagerId,@EmployeeTerminationTypeId,@EmployeeTerminationType,@HomeStreetAddress1,@HomeStreetAddress2,@HomePostalCode,@HomeCity,@HomeStateProvince,@HomeCountryId,@WorkStreetAddress1,@WorkStreetAddress2,@WorkPostalCode,@WorkCity,@WorkStateProvince,@WorkCountryId,@DepartmentId,@Department,@CompanyId,@OrganizationId,@Organization,@Deleted)";

                        cmd.CommandText = sql;
                        cmd.Connection = connection;

                        if (item["First_x0020_Name"] != null)
                        {
                            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = item["First_x0020_Name"].ToString();
                        }
                        else
                        {
                            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        }

                        if (item["Last_x0020_Name"] != null)
                        {
                            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = item["Last_x0020_Name"].ToString();
                        }
                        else
                        {
                            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        }

                        if (item["Email"] != null)
                        {
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = item["Email"].ToString();
                        }
                        else
                        {
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        }

                        //if ( item["EmployeeGovernmentId"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@EmployeeGovernmentId", SqlDbType.NVarChar, 50).Value = item["EmployeeGovernmentId"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@EmployeeGovernmentId", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_Phone_x0020_Number"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomePhoneNumber", SqlDbType.NVarChar, 50).Value = item["Home_x0020_Phone_x0020_Number"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomePhoneNumber", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_Phone_x0020_Number"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkPhoneNumber", SqlDbType.NVarChar, 50).Value = item["Work_x0020_Phone_x0020_Number"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkPhoneNumber", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Movil_x0020_Phone_x0020_Number"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@MovilPhoneNumber", SqlDbType.NVarChar, 50).Value = item["Movil_x0020_Phone_x0020_Number"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@MovilPhoneNumber", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Hire_x0020_Date"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = DateTime.Parse(item["Hire_x0020_Date"].ToString());
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = DBNull.Value;
                        //}

                        //if ( item["Date_x0020_Of_x0020_Birth"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateTime.Parse(item["Date_x0020_Of_x0020_Birth"].ToString());
                        //}
                        //else {
                        //    cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DBNull.Value;
                        //}

                        //if ( item["BiometricEmployeeId"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@BiometricEmployeeId", SqlDbType.NVarChar, 50).Value = item["BiometricEmployeeId"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@BiometricEmployeeId", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Salary"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = decimal.Parse(item["Salary"].ToString());
                        //}
                        //else {
                        //    cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = DBNull.Value;
                        //}
                        //if ( item["Employee_x0020_Type"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@EmployeeType", SqlDbType.NVarChar, 50).Value = item["Employee_x0020_Type"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@EmployeeType", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Resource_x0020_Type"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@ResourceType", SqlDbType.NVarChar, 50).Value = item["Resource_x0020_Type"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@ResourceType", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Gender"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 50).Value = item["Gender"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Marital_x0020_Status"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@MaritalStatus", SqlDbType.NVarChar, 50).Value = item["Marital_x0020_Status"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@MaritalStatus", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Employee_x0020_Termination_x0020"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@EmployeeTerminationType", SqlDbType.NVarChar, 50).Value = item["Employee_x0020_Termination_x0020"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@EmployeeTerminationType", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_Street_x0020_Address_"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomeStreetAddress1", SqlDbType.NVarChar, 50).Value = item["Home_x0020_Street_x0020_Address_"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomeStreetAddress1", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_Street_x0020_Address_0"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomeStreetAddress2", SqlDbType.NVarChar, 50).Value = item["Home_x0020_Street_x0020_Address_0"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomeStreetAddress2", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_Postal_x0020_Code"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomePostalCode", SqlDbType.NVarChar, 50).Value = item["Home_x0020_Postal_x0020_Code"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomePostalCode", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_City"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomeCity", SqlDbType.NVarChar, 50).Value = item["Home_x0020_City"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomeCity", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Home_x0020_State_x0020_Province"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@HomeStateProvince", SqlDbType.NVarChar, 50).Value = item["Home_x0020_State_x0020_Province"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@HomeStateProvince", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_Street_x0020_Address_"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkStreetAddress1", SqlDbType.NVarChar, 50).Value = item["Work_x0020_Street_x0020_Address_"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkStreetAddress1", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_Street_x0020_Address_0"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkStreetAddress2", SqlDbType.NVarChar, 50).Value = item["Work_x0020_Street_x0020_Address_0"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkStreetAddress2", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_Postal_x0020_Code"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkPostalCode", SqlDbType.NVarChar, 50).Value = item["Work_x0020_Postal_x0020_Code"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkPostalCode", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_City"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkCity", SqlDbType.NVarChar, 50).Value = item["Work_x0020_City"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkCity", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Work_x0020_State_x0020_Province"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@WorkStateProvince", SqlDbType.NVarChar, 50).Value = item["Work_x0020_State_x0020_Province"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@WorkStateProvince", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}
                        //if ( item["Department"] != null ) 
                        //{
                        //    cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = item["Department"].ToString();
                        //}
                        //else {
                        //    cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                        //}

                        cmd.ExecuteNonQuery();

                    }
                }
                catch (SqlException ex)
                {
                    SharePointContext.Current.Log.Error(ex.Message, ex.StackTrace);
                }
                finally
                {
                    // Cleanup objects
                    if (null != cmd)
                        cmd.Dispose();
                    if (null != connection)
                        connection.Dispose();
                }
            });
        }
    }
}
