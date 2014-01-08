
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace BJSmarts.ERP
{
	[Serializable]
	public partial class InsertSummaryRisksControl : UserControl
	{
		private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

		protected void Page_Load(object sender, EventArgs e)
        {
			this.Page.Form.Enctype = "multipart/form-data";

            if (!Page.IsPostBack)
            {
			 loadLocalizedValueFields();
            }
		}

		private void loadLocalizedValueFields()
        {
			textTITLE.Text = getLocalizedValue("TITLEText", intLCID);
			textDESCRIPTION.Text = getLocalizedValue("DESCRIPTIONText", intLCID);
			textSORT_ORDER.Text = getLocalizedValue("SORT_ORDERText", intLCID);
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
                        catch { }
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
                        catch { }
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
                        catch { }
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

            SPListItemCollection listItems = web.Lists["Summary Risks"].Items;

            SPListItem item = listItems.Add();

				item["TITLE"] = txtTITLE.Text;			
				item["DESCRIPTION"] = txtDESCRIPTION.Text;			
				item["SORT_ORDER"] = Convert.ToInt64(txtSORT_ORDER.Text);

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

		protected void lnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Site.Url);       
        }

		private string getLocalizedValue(string strInput, int intLCID)
        {            
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "ResourceFile", (uint)intLCID);
        }

	}

}
