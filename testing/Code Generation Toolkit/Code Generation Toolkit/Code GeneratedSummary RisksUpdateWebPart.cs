using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace BJSmarts.ERP
{
	[Serializable]
	public partial class UpdateSummaryRisksControl : UserControl, ICallbackEventHandler
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
                                
									try
									{
										ListItem = GetCurrentItem();
									}
									catch
									{
										ListItem = GetItemByBdcId();
									}   
									txtTITLE.Text = GetItemText(ListItem, "TITLE");
									txtDESCRIPTION.Text = GetItemText(ListItem, "DESCRIPTION");
									txtSORT_ORDER.Text = GetItemText(ListItem, "SORT_ORDER");									
									try
									{
										BindAttachmentData();
									}
									catch
									{
									}
							}
                        }
                    });
				}
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


		public SPListItem GetItemByBdcId()
        {
            SPListItem item = null;
            if (Page.Request.QueryString["RecordId"] != null)
            {
                string RecordId = Page.Request.QueryString["RecordId"].ToString();

                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Project Summary Risks"];

                        foreach (SPListItem listitem in list.Items)
                        {
                            if (listitem["BDC Identity"].ToString() == RecordId)
                            {
                                item = listitem;
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

		protected void lnCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(SPContext.Current.Site.Url);       
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
                        
                        SPList list = web.Lists["Summary Risks"];
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
                       SPList list = web.Lists["Summary Risks"];
                       item = list.Items.GetItemById(RecordId);
                   }
               }
           }

           return item;
       }

	    void GetItemCheckBox(SPListItem Item, String fieldname, CheckBox chkbox)
        {
            if (Item[fieldname] != null)
            {
                string value = Item[fieldname].ToString();
               if (value=="True")
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
            return Microsoft.SharePoint.Utilities.SPUtility.GetLocalizedString("$Resources: " + strInput, "ResourceFile", (uint)intLCID);
        }

		}

	}
