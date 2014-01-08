using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SharePoint;
using Microsoft.BusinessData;
using Microsoft.SharePoint.BusinessData.Infrastructure;

namespace BJSmarts.ERP.SharePoint.WebParts.Organization.OrganizationsInsertWebPart
{
    public partial class OrganizationsInsertWebPartUserControl : UserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            if (!Page.IsPostBack)
            {
                loadLocalizedValueFields();

                LoadingLookupOneToOne("Currency Type", ddlCurrencyId, "Name", "Sort_Order", "CurrencyTypeId", true, intLCID);
                LoadingLookupOneToOne("Industry", ddlIndustryId, "Name", "Sort_Order", "IndustryId", true, intLCID);
                LoadingLookupOneToOne("Language", ddlLanguage, "Name", "Sort_Order", "LanguageId", true, intLCID);

            }
        }

        private void loadLocalizedValueFields()
        {
            labelName.Text = getLocalizedValue("NameText", intLCID);
            labelDescription.Text = getLocalizedValue("DescriptionText", intLCID);
            labelCurrencyId.Text = getLocalizedValue("CurrencyIdText", intLCID);
            labelIndustryId.Text = getLocalizedValue("IndustryIdText", intLCID);
            labelLanguage.Text = getLocalizedValue("LanguageText", intLCID);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SPUser currentUser = GetCurrentUser();
           
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    //add record to the database
                    addRecord(currentUser, web, false);                                       
                }

                using (SPWeb web = site.RootWeb )
                {
                    SPFolder rootFolder = web.RootFolder;
                    rootFolder.WelcomePage = @"Pages/Home.aspx";
                    rootFolder.Update();
                }

                //Redirect page to home page
                Page.Response.Redirect(SPContext.Current.Site.Url);
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

        void LoadingChoiceTable(String listname, String fieldname, CheckBoxList chk)
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


        void LoadingRadioChoiceTable(String listname, String fieldname, RadioButtonList rbtn)
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

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                String connectionString = "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;";

                try
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sql = "INSERT INTO [BJSmarts.ERP.Database].[dbo].[Organizations] ([Name],[Description],[SiteId],[SiteUrl],[CurrencyId],[IndustryId],[Industry],[Language],[Deleted])" +
                                     "VALUES (@Name, @Description, @SiteId, @SiteUrl, @CurrencyId, @IndustryId, @Industry, @Language, @Deleted)";

                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = txtName.Text;
                            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = txtDescription.Text;
                            cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.ID.ToString();
                            cmd.Parameters.Add("@SiteUrl", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.Url;
                            cmd.Parameters.Add("@CurrencyId", SqlDbType.Int).Value = Convert.ToInt64(ddlCurrencyId.SelectedValue);
                            cmd.Parameters.Add("@IndustryId", SqlDbType.Int).Value = Convert.ToInt64(ddlIndustryId.SelectedValue);
                            cmd.Parameters.Add("@Industry", SqlDbType.NVarChar, 50).Value = ddlIndustryId.SelectedItem.Text;
                            cmd.Parameters.Add("@Language", SqlDbType.Int).Value = Convert.ToInt64(ddlLanguage.SelectedValue);
                            cmd.Parameters.Add("@Deleted", SqlDbType.Int).Value = 0;

                            cmd.ExecuteNonQuery();
                        }
                    }                                                                    
                }
                catch (SqlException sqlEx)
                {
                    LogError(sqlEx.Message, sqlEx.StackTrace);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, ex.StackTrace);
                }                
            });
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
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "OrganizationsResourceFile", (uint)intLCID);
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
