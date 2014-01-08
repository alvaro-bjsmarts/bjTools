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

namespace BJSmarts.ERP.HumanResources.WebParts.DepartmentViewWebPart
{
    public partial class DepartmentViewWebPartUserControl : UserControl
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

                                ListItem = GetItemByBdcId();
                                lblName.Text = GetItemText(ListItem, "Name");
                                lblDescription.Text = GetItemText(ListItem, "Description");
                                lblStreeAddress1.Text = GetItemText(ListItem, "StreeAddress1");
                                lblStreeAddress2.Text = GetItemText(ListItem, "StreeAddress2");
                                lblCity.Text = GetItemText(ListItem, "City");
                                lblState_x002f_Province.Text = GetItemText(ListItem, "State_x002f_Province");
                                lblZip_x002f_PostalCode.Text = GetItemText(ListItem, "Zip_x002f_Postal_x0020_Code");
                                lblSort_Order.Text = GetItemText(ListItem, "Sort_Order");

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
            labelStreeAddress1.Text = getLocalizedValue("StreeAddress1Text", intLCID);
            labelStreeAddress2.Text = getLocalizedValue("StreeAddress2Text", intLCID);
            labelCity.Text = getLocalizedValue("CityText", intLCID);
            labelState_x002f_Province.Text = getLocalizedValue("State_x002f_ProvinceText", intLCID);
            labelZip_x002f_PostalCode.Text = getLocalizedValue("Zip_x002f_PostalCodeText", intLCID);
            labelSort_Order.Text = getLocalizedValue("Sort_OrderText", intLCID);
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
                        SPList list = web.Lists["Departments"];

                        foreach (SPListItem externalItem in list.Items)
                        {
                            if (int.Parse(externalItem["DepartmentId"].ToString()) == RecordId)
                            {
                                item = externalItem;
                            }
                        }
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

        private String GetItemTextExternalData(String listname, SPListItem Item, String colName, String showfield)
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

                                    query.Query = "<Where><Eq><FieldRef Name='" + colName + "'/><Value Type='Number'>" + Convert.ToInt64(Text) + "</Value></Eq></Where>";

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
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "DepartmentsResourceFile", (uint)intLCID);
        }

        protected void b_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["RecordID"] != null)
            {
                Response.Redirect("/Pages/DepartmentsUpdatePage.aspx?RecordId=" + Page.Request.QueryString["RecordID"].ToString());
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
