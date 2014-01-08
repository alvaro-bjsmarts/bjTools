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
using Microsoft.SharePoint;
using Microsoft.BusinessData;
using Microsoft.SharePoint.BusinessData.Infrastructure;

namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeAcctsUpdateWebPart
{
    public partial class EmployeeAcctsUpdateWebPartUserControl : UserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

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

                                ListItem = GetItemByBdcId();

                                txtName.Text = GetItemText(ListItem, "Name");
                                txtDescription.Text = GetItemText(ListItem, "Description");
                                txtSort_Order.Text = GetItemText(ListItem, "Sort_Order");

                            }
                        }
                    });
                }
            }
        }

        private void loadLocalizedValueFields()
        {
            labelName.Text = getLocalizedValue("NameText", intLCID);
            labelDescription.Text = getLocalizedValue("DescriptionText", intLCID);
            labelSort_Order.Text = getLocalizedValue("Sort_OrderText", intLCID);
        }

        protected void btnOK_Click(object sender, EventArgs e)
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


        public SPListItem GetItemByBdcId()
        {
            SPListItem item = null;

            if (Page.Request.QueryString["RecordId"] != null)
            {
                int RecordId = int.Parse(Page.Request.QueryString["RecordId"]);

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Employee Accounts"];

                        foreach (SPListItem externalItem in list.Items)
                        {
                            if (int.Parse(externalItem["EmployeeAccountsId"].ToString()) == RecordId)
                            {
                                item = externalItem;
                            }
                        }
                    }
                }
            }
            return item;
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

            SPListItemCollection listItems = web.Lists["Employee Accounts"].Items;

            SPListItem item;
            try
            {
                int RecordId = int.Parse(Context.Request.QueryString["RecordID"].ToString());
                item = listItems.GetItemById(RecordId);
            }
            catch
            {
                item = GetItemByBdcId();
            }

            item["Name"] = txtName.Text;
            item["Description"] = txtDescription.Text;
            Decimal Sort_Order = 0;

            try
            {
                Sort_Order = Decimal.Parse(txtSort_Order.Text);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
            }

            item["Sort_Order"] = Sort_Order;
            if (intLCID == 1033)
            {
                item["Language"] = 0;
            }

            if (intLCID == 3082)
            {
                item["Language"] = 1;
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


        void GetItemCheckBox(SPListItem Item, String fieldname, CheckBox chkbox)
        {
            if (Item[fieldname] != null)
            {
                string value = Item[fieldname].ToString();
                if (value == "True")
                {
                    chkbox.Checked = true;
                }
            }
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

        void GetItemCheckedRadioOnetoOne(SPListItem Item, String fieldname, RadioButtonList rbtn)
        {
            if (Item[fieldname] != null)
            {
                string value = Item[fieldname].ToString();
                if (value != null)
                {
                    ListItem listItems = rbtn.Items.FindByValue(value);
                    if (listItems != null) listItems.Selected = true;
                }
            }

        }

        private int GetItemIndex(SPListItem Item, DropDownList ddl, String colName)
        {
            String text = GetItemText(Item, colName);

            SPFieldLookupValue value = new SPFieldLookupValue(text);

            int index = ddl.Items.IndexOf(ddl.Items.FindByValue(value.LookupId.ToString()));

            return index;
        }

        private int GetItemIndexBCSField(SPListItem Item, DropDownList ddl, String colName)
        {
            String text = GetItemText(Item, colName);

            int index = ddl.Items.IndexOf(ddl.Items.FindByText(text));

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


        private String GetItemTextUser(SPListItem Item, String colName)
        {
            if (Item != null)
            {
                try
                {
                    String Text = Regex.Replace(Item[colName].ToString(), "<[^>]*>", string.Empty);

                    string[] newchoices = null;
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
                                newaux += choices[i] + " ";
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
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "EmployeeAccountsResourceFile", (uint)intLCID);
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
