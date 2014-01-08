
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace BJSmarts.ERP
{
	[Serializable]
	public partial class ViewSummaryRisksControl : UserControl
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

								try
								{
                                ListItem = GetCurrentItem();
								}
								catch
								{
								ListItem = GetItemByBdcId();
								}
									lblTITLE.Text = GetItemText(ListItem, "TITLE");
									lblDESCRIPTION.Text = GetItemText(ListItem, "DESCRIPTION");
									lblSORT_ORDER.Text =  GetItemText(ListItem, "SORT_ORDER");
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
