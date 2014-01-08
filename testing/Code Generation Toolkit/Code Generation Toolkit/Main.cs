using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateMaschine;
using Microsoft.SharePoint;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;

namespace Code_Generation_Toolkit
{
    public partial class Main : Form
    {
        String[] showFields = new String[100];
        String[] lookupOneToOne = new String[100];
        String[] lookupManyToMany = new String[100];
        String[] groupCache= new String[20];
        String[] savegroup = new String[100];
        String[] deletedAndlanguajeDetect = new String[100];
        String[] typeDataBase = new String[100];
        public Main()
        {
            InitializeComponent();           
        }

        public void fiillDdl(string[] array, int count)
        {
            string[] MyGroup = array;
            
            for(int i=0;i<=count;i++)
            {
                ddlGroups.Items.Add(MyGroup[i]);                
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            String[] controls = new String[100];
            String[] displayName = new string[100];
            String[] controlsType = new String[100];
            String[] formatType= new String[100];
            String[] lookupListname = new String[100];
            String[] lookupExternalListname = new String[100];
            String[] requiredFields = new String[100];
            String[] groupNames = new String[100];
            int[] stringLength = new int[100];
            String idfield;
            String PrimaryKeyColumnName="";
            String keycomprove="";

            if (ExternalData.Checked == true)
            {
                idfield = ddlSelectId.SelectedItem.ToString();
            }
            else
            {
                idfield = "";
            }
            //get access to a Sharepoint List

            using (SPSite site = new SPSite(txtSite.Text))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPList lists = web.Lists[ddlListName.SelectedItem.ToString()];          

                    int c=0;

                    foreach (SPField item in lists.Fields)
                    {                        
                        if (!item.Hidden)
                        {
                            SPFieldType fieldType = item.Type;
                            
                            switch (item.InternalName)
                            {
                                case "DocIcon":                                 
                                    break;
                                case "_UIVersionString":
                                    break;
                                case "LinkTitleNoMenu":
                                    break;
                                case "Edit":
                                    break;
                                case "Attachments":
                                    break;
                                case "Editor":
                                    break;
                                case "Sindicat":
                                    break;
                                case "Modified":
                                    break;
                                case "ContentType":
                                    break;
                                case "LinkTitle":
                                    break;
                                case "ItemChildCount":
                                    break;
                                case "FolderChildCount":
                                    break;
                                case "Author":
                                    break;
                                case "Created":
                                    break;
                                case "BdcIdentity":
                                    break;
                                default:

                                    foreach (var it in lstdefaultFields.Items)
                                    {
                                        if (it.ToString() == item.Title)
                                        {
                                            controls[c] = item.InternalName;
                                            displayName[c] = item.Title;
                                            controlsType[c] = item.TypeAsString;

                                            if (item.TypeAsString == "Text")
                                            {
                                                SPFieldText sptext = (SPFieldText)lists.Fields[item.Title];
                                                stringLength[c] = sptext.MaxLength;
                                            }
                                            else
                                            {
                                                stringLength[c] = 0;
                                            }
                                            if (item.TypeAsString == "Lookup")
                                            {
                                                SPFieldLookup fldLook = (SPFieldLookup)web.Lists[ddlListName.SelectedItem.ToString()].Fields[item.Title];
                                                SPList lookuplist = web.Lists[new Guid(fldLook.LookupList)];
                                                lookupListname[c] = lookuplist.Title;
                                            }
                                            else
                                            {
                                                lookupListname[c] = "none";
                                            }
                                            if (item.TypeAsString == "BusinessData")

                                            {
                                                try
                                                {
                                                    SPBusinessDataField bizDataField = (SPBusinessDataField)lists.Fields[item.Title];
                                                    lookupExternalListname[c] = bizDataField.EntityName;
                                                }
                                                catch
                                                {
                                                    lookupExternalListname[c] = "none";
                                                }
                                            }
                                            else
                                            {
                                                lookupExternalListname[c] = "none";
                                            }
                                            if (item.TypeAsString == "Choice")
                                            {
                                                SPFieldChoice chFldGender = (SPFieldChoice)lists.Fields[item.Title];
                                                formatType[c] = chFldGender.EditFormat.ToString();
                                            }
                                            else
                                            {
                                                formatType[c] = "none";
                                            }

                                            requiredFields[c] = "No";

                                            foreach (var listitem in lstAllRequiredFields.Items)
                                            {
                                                if (item.Title == listitem.ToString())
                                                {
                                                    requiredFields[c] = "Yes";
                                                }
                                            }                                            

                                            c = c + 1;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }




            SqlConnection cn = new SqlConnection("Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;");
            cn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [" + ddlListName.SelectedItem.ToString() + "]", cn);
            SqlDataReader rdr = cmd.ExecuteReader();
            
         

            while (rdr.Read())
            {
                for (int c = 0; c < rdr.VisibleFieldCount; c++)
                {
                    //System.Type type = rdr.GetFieldType(c);
                    String type = rdr.GetDataTypeName(c).ToString();
                    
                    typeDataBase[c] = rdr.GetName(c)+","+type;
                }
            }

            cn.Close();

            if (GetprimaryKey(ddlListName.SelectedItem.ToString(), "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;") != null)
            {
                PrimaryKeyColumnName = GetprimaryKey(ddlListName.SelectedItem.ToString(), "Initial Catalog=BJSmarts.ERP.Database;Data Source=(local);Integrated Security=SSPI;");

                if (PrimaryKeyColumnName != "")
                {
                    keycomprove = "true";
                }
                else
                {
                    keycomprove = "false";
                }
            }
            else
            {
                keycomprove = "false";
            }
        

            int count = 0;

            for (int n = 0; n < controls.Count(); n++)
            {
                if (controls[n] != null) 
                {
                    count++; 
                }
            }

                
            int aux = 0;

            for (int j = 0; j < savegroup.Count(); j++)
            {
                if (savegroup[j] != null)
                {
                    aux++;
                }
            }

            int number = 0;

            foreach (var name in ddlGroups.Items)
            {
                groupNames[number] = name.ToString();

                number = number + 1;
            }

            int auxnum = 0;

            for (int j = 0; j < lookupOneToOne.Count(); j++)
            {
                if (lookupOneToOne[j] != null)
                {
                    auxnum++;
                }
            }


            int countdb = 0;

            for (int j = 0; j < typeDataBase.Count(); j++)
            {
                if (typeDataBase[j] != null)
                {
                    countdb++;
                }
            }
            

            int deletedNumber = lstExcludeFields.Items.Count;

            for (int j = 0; j < lstExcludeFields.Items.Count; j++)
            {

                deletedAndlanguajeDetect[j] = lstExcludeFields.Items[j].ToString();
                
            }
          

            String viewClass = ConfigurationManager.AppSettings["viewClassPath"];
            String ExternalDataviewClass = ConfigurationManager.AppSettings["ExternalDataviewClassPath"];
            String viewForm = ConfigurationManager.AppSettings["viewFormPath"];
            String ExternalDataviewForm = ConfigurationManager.AppSettings["ExternalDataviewFormPath"];
            String insertClass = ConfigurationManager.AppSettings["insertClassPath"];
            String ExternalDatainsertClass = ConfigurationManager.AppSettings["ExternalDatainsertClassPath"]; 
            String insertForm = ConfigurationManager.AppSettings["insertFormPath"];
            String ExternalDatainsertForm = ConfigurationManager.AppSettings["ExternalDatainsertFormPath"]; 
            String updateClass = ConfigurationManager.AppSettings["updateClassPath"];
            String ExternalDataupdateClass = ConfigurationManager.AppSettings["ExternalDataupdateClassPath"]; 
            String updateForm = ConfigurationManager.AppSettings["updateFormPath"];
            String ExternalDataupdateForm = ConfigurationManager.AppSettings["ExternalDataupdateFormPath"];
            String resourceFileEs = ConfigurationManager.AppSettings["resourceFileEs"];
            String resourceFile = ConfigurationManager.AppSettings["resourceFile"];
            String ListView = ConfigurationManager.AppSettings["ListView"];
            String ListViewClass = ConfigurationManager.AppSettings["ListViewClass"];

            //Library
            String ICurrent = ConfigurationManager.AppSettings["ICurrent"];
            String IDebug = ConfigurationManager.AppSettings["IDebug"];
            String IField = ConfigurationManager.AppSettings["IField"];
            String IGroup = ConfigurationManager.AppSettings["IGroup"];
            String ILanguage = ConfigurationManager.AppSettings["ILanguage"];
            String ILanguageFactory = ConfigurationManager.AppSettings["ILanguageFactory"];
            String IList = ConfigurationManager.AppSettings["IList"];
            String ILog = ConfigurationManager.AppSettings["ILog"];
            String IOrganization = ConfigurationManager.AppSettings["IOrganization"];
            String ISettings = ConfigurationManager.AppSettings["ISettings"];
            String ISite = ConfigurationManager.AppSettings["ISite"];
            String IUser = ConfigurationManager.AppSettings["IUser"];
            String IUserControl = ConfigurationManager.AppSettings["IUserControl"];
            String IWeb = ConfigurationManager.AppSettings["IWeb"];
            String SharePointContext = ConfigurationManager.AppSettings["SharePointContext"];
            String SharePointEnglishLanguage = ConfigurationManager.AppSettings["SharePointEnglishLanguage"];
            String SharePointField = ConfigurationManager.AppSettings["SharePointField"];
            String SharePointGroup = ConfigurationManager.AppSettings["SharePointGroup"];
            String SharePointCurrent = ConfigurationManager.AppSettings["SharePointCurrent"];
            String SharePointDebug = ConfigurationManager.AppSettings["SharePointDebug"];
            String SharePointLanguage = ConfigurationManager.AppSettings["SharePointLanguage"];
            String SharePointLanguageFactory = ConfigurationManager.AppSettings["SharePointLanguageFactory"];
            String SharePointList = ConfigurationManager.AppSettings["SharePointList"];
            String SharePointLog = ConfigurationManager.AppSettings["SharePointLog"];
            String SharePointOrganization = ConfigurationManager.AppSettings["SharePointOrganization"];
            String SharePointSettings = ConfigurationManager.AppSettings["SharePointSettings"];
            String SharePointSite = ConfigurationManager.AppSettings["SharePointSite"];
            String SharePointSpanishLanguage = ConfigurationManager.AppSettings["SharePointSpanishLanguage"];
            String SharePointUser = ConfigurationManager.AppSettings["SharePointUser"];
            String SharePointUserControl = ConfigurationManager.AppSettings["SharePointUserControl"];
            String SharePointWeb = ConfigurationManager.AppSettings["SharePointWeb"];        


            

            String ClassName = ddlListName.SelectedItem.ToString();
            String NameSpace = txtNamespace.Text;
            String Site = txtSite.Text;
            String ListName = ddlListName.SelectedItem.ToString();          

            Template viewClassTemplate = new Template(viewClass);
            Template ExternalDataviewClassTemplate = new Template(ExternalDataviewClass);
            Template viewFormTemplate = new Template(viewForm);
            Template ExternalDataviewFormTemplate = new Template(ExternalDataviewForm);
            Template insertClassTemplate = new Template(insertClass);
            Template ExternalDatainsertClassTemplate = new Template(ExternalDatainsertClass);
            Template insertFormTemplate = new Template(insertForm);
            Template ExternalDatainsertFormTemplate = new Template(ExternalDatainsertForm);
            Template updateClassTemplate = new Template(updateClass);
            Template ExternalDataupdateClassTemplate = new Template(ExternalDataupdateClass);
            Template updateFormTemplate = new Template(updateForm);
            Template ExternalDataupdateFormTemplate = new Template(ExternalDataupdateForm);
            Template resourceFileEsTemplate = new Template(resourceFileEs);
            Template resourceFileTemplate = new Template(resourceFile);
            Template ListViewTemplate = new Template(ListView);
            Template ListViewClassTemplate = new Template(ListViewClass);

            //Library
            Template ICurrentTemplate = new Template(ICurrent);
            Template IDebugTemplate = new Template(IDebug);
            Template IFieldTemplate = new Template(IField);
            Template IGroupTemplate = new Template(IGroup);
            Template ILanguageTemplate = new Template(ILanguage);
            Template ILanguageFactoryTemplate = new Template(ILanguageFactory);
            Template IListTemplate = new Template(IList);
            Template ILogTemplate = new Template(ILog);
            Template IOrganizationTemplate = new Template(IOrganization);
            Template ISettingsTemplate = new Template(ISettings);
            Template ISiteTemplate = new Template(ISite);
            Template IUserTemplate = new Template(IUser);
            Template IUserControlTemplate = new Template(IUserControl);
            Template IWebTemplate = new Template(IWeb);
            Template SharePointContextTemplate = new Template(SharePointContext);
            Template SharePointEnglishLanguageTemplate = new Template(SharePointEnglishLanguage);
            Template SharePointFieldTemplate = new Template(SharePointField);
            Template SharePointGroupTemplate = new Template(SharePointGroup);
            Template SharePointCurrentTemplate = new Template(SharePointCurrent);
            Template SharePointDebugTemplate = new Template(SharePointDebug);
            Template SharePointLanguageTemplate = new Template(SharePointLanguage);
            Template SharePointLanguageFactoryTemplate = new Template(SharePointLanguageFactory);
            Template SharePointListTemplate = new Template(SharePointList);
            Template SharePointLogTemplate = new Template(SharePointLog);
            Template SharePointOrganizationTemplate = new Template(SharePointOrganization);
            Template SharePointSettingsTemplate = new Template(SharePointSettings);
            Template SharePointSiteTemplate = new Template(SharePointSite);
            Template SharePointSpanishLanguageTemplate = new Template(SharePointSpanishLanguage);
            Template SharePointUserTemplate = new Template(SharePointUser);
            Template SharePointUserControlTemplate = new Template(SharePointUserControl);
            Template SharePointWebTemplate = new Template(SharePointWeb);

            //Library
            if (chkLibrary.Checked == true)
            {

                if (!Directory.Exists(txtLocation.Text + "/Library"))
                {
                    Directory.CreateDirectory(txtLocation.Text + "/Library");
                }

                ICurrentTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ICurrent.cs");
                IDebugTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IDebug.cs");
                IFieldTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IField.cs");
                IGroupTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IGroup.cs");
                ILanguageTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ILanguage.cs");
                ILanguageFactoryTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ILanguageFactory.cs");
                IListTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IList.cs");
                ILogTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ILog.cs");
                IOrganizationTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IOrganization.cs");
                ISettingsTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ISettings.cs");
                ISiteTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "ISite.cs");
                IUserTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IUser.cs");
                IUserControlTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IUserControl.cs");
                IWebTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "IWeb.cs");
                SharePointContextTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointContext.cs");
                SharePointEnglishLanguageTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointEnglishLanguage.cs");
                SharePointFieldTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointField.cs");
                SharePointGroupTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointGroup.cs");
                SharePointCurrentTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointCurrent.cs");
                SharePointDebugTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointDebug.cs");
                SharePointLanguageTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointLanguage.cs");
                SharePointLanguageFactoryTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointLanguageFactory.cs");
                SharePointListTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointList.cs");
                SharePointLogTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointLog.cs");
                SharePointOrganizationTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointOrganization.cs");
                SharePointSettingsTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointSettings.cs");
                SharePointSiteTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointSite.cs");
                SharePointSpanishLanguageTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointSpanishLanguage.cs");
                SharePointUserTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointUser.cs");
                SharePointUserControlTemplate.Generate(new object[] { NameSpace ,ListName}, txtLocation.Text + "/Library/" + "SharePointUserControl.cs");
                SharePointWebTemplate.Generate(new object[] { NameSpace }, txtLocation.Text + "/Library/" + "SharePointWeb.cs");
            }


            if (ExternalData.Checked != true)
            {
                viewClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, NameSpace, lookupOneToOne, auxnum, displayName, chkAttachments.Checked.ToString(), ClassName }, txtLocation.Text + "/" + ClassName + "ViewWebPart.cs");

                viewFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, groupNames, savegroup, aux, number, displayName, chkAttachments.Checked.ToString(), ClassName }, txtLocation.Text + "/" + ClassName + "ViewWebPart.ascx");

                insertClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, NameSpace, lookupListname, lookupExternalListname, displayName, chkAttachments.Checked.ToString(), auxnum, lookupOneToOne, idfield, ClassName, deletedAndlanguajeDetect, deletedNumber, typeDataBase, countdb, PrimaryKeyColumnName, keycomprove.ToString() }, txtLocation.Text + "/" + ClassName + "InsertWebPart.cs");

                insertFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, groupNames, savegroup, aux, number, displayName, requiredFields, lookupOneToOne, auxnum, chkAttachments.Checked.ToString(), ClassName }, txtLocation.Text + "/" + ClassName + "InsertWebPart.ascx");

                updateClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, NameSpace, lookupListname, lookupExternalListname, chkAttachments.Checked.ToString(), auxnum, lookupOneToOne, idfield, displayName, ClassName, deletedAndlanguajeDetect,  deletedNumber, typeDataBase, countdb, PrimaryKeyColumnName, keycomprove.ToString() }, txtLocation.Text + "/" + ClassName + "UpdateWebPart.cs");

                updateFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, groupNames, savegroup, aux, number, displayName, requiredFields, lookupOneToOne, auxnum, chkAttachments.Checked.ToString(), ClassName }, txtLocation.Text + "/" + ClassName + "UpdateWebPart.ascx");

                resourceFileEsTemplate.Generate(new object[] { count, controls, controlsType, ListName, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ResourceFile.es-ES.resx");

                resourceFileTemplate.Generate(new object[] { count, controls, controlsType, ListName, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ResourceFile.resx");

                if (chkAttachments.Checked == true)
                {

                    string fileName = txtLocation.Text + "/" + ClassName + "InsertWebPart.ascx";

                    createEntry("    var counter = 0;", "<script type = \"text/javascript\">", fileName);
                    createEntry("<style type=\"text/css\">", "</script>", fileName);

                    fileName = txtLocation.Text + "/" + ClassName + "ViewWebPart.ascx";

                    createEntry("                                </span></b>", "<asp:LinkButton ID=\"lbdoc\" runat=\"server\" OnClientClick=<%#\"window.open('\" + Eval(\"FilenameUrl\")+ \"')\" %> Text='<%# Eval(\"Filename\") %>' />", fileName);

                    fileName = txtLocation.Text + "/" + ClassName + "UpdateWebPart.ascx";

                    createEntry(" function DeleteItem(file)", "<script type = \"text/javascript\">", fileName);
                    createEntry("<style type=\"text/css\">", "</script>", fileName);
                    createEntry("    }function ReceiveServerData(rValue)", "<%= Page.ClientScript.GetCallbackEventReference(this, \"product\", \"ReceiveServerData\",null)%>;", fileName);
                    createEntry("                            </span></b>", "<asp:LinkButton ID=\"lbdoc\" runat=\"server\" OnClientClick=<%#\"window.open('\" + Eval(\"FilenameUrl\")+ \"')\" %> Text='<%# Eval(\"Filename\") %>' />", fileName);
                    createEntry("						>Delete</asp:LinkButton></td>", "<asp:LinkButton ID=\"lbdelete\" runat=\"server\" OnClientClick=<%# \"DeleteItem('\" + Eval(\"Filename\") + \"'); return false;\" %>", fileName);
                }

            }
            else
            {
                ExternalDataviewClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, NameSpace, idfield, lookupOneToOne, auxnum, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ViewWebPart.cs");

                ExternalDataviewFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, groupNames, savegroup, aux, number, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ViewWebPart.ascx");

                ExternalDatainsertClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, NameSpace, lookupListname, lookupExternalListname, lookupOneToOne, auxnum, displayName, idfield, ClassName, deletedAndlanguajeDetect, deletedNumber }, txtLocation.Text + "/" + ClassName + "InsertWebPart.cs");

                ExternalDatainsertFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, groupNames, savegroup, aux, number, displayName, requiredFields, lookupOneToOne, auxnum, ClassName, stringLength }, txtLocation.Text + "/" + ClassName + "InsertWebPart.ascx");

                ExternalDataupdateClassTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, NameSpace, lookupListname, lookupExternalListname, lookupOneToOne, auxnum, displayName, idfield, ClassName, deletedAndlanguajeDetect, deletedNumber }, txtLocation.Text + "/" + ClassName + "UpdateWebPart.cs");

                ExternalDataupdateFormTemplate.Generate(new object[] { count, controls, controlsType, ListName, formatType, groupNames, savegroup, aux, number, displayName, requiredFields, lookupOneToOne, auxnum, ClassName, stringLength }, txtLocation.Text + "/" + ClassName + "UpdateWebPart.ascx");

                resourceFileEsTemplate.Generate(new object[] { count, controls, controlsType, ListName, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ResourceFile.es-ES.resx");

                resourceFileTemplate.Generate(new object[] { count, controls, controlsType, ListName, displayName, ClassName }, txtLocation.Text + "/" + ClassName + "ResourceFile.resx");

                ListViewTemplate.Generate(new object[] { count, controls, controlsType, ListName, groupNames, savegroup, aux, number, displayName, idfield, ClassName }, txtLocation.Text + "/" + ClassName + "ListView.ascx");

                ListViewClassTemplate.Generate(new object[] { ListName, NameSpace, auxnum, displayName, idfield, ClassName }, txtLocation.Text + "/" + ClassName + "ListView.cs");

            }

            if (MessageBox.Show("The code has been generated", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                Application.Exit();
            }
            
        }
               

        private void ddlListName_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[] displayName = buildArrayExternalData(ddlListName.SelectedItem.ToString());
            String[] externalName = buildArray(ddlListName.SelectedItem.ToString());
            deletedAndlanguajeDetect = buildArrayExternalData(ddlListName.SelectedItem.ToString());

            int count = 0;

            for (int n = 0; n < displayName.Count(); n++)
            {
                if (displayName[n] != null)
                {
                    count++;
                }
            }

            int counter = 0;

            for (int n = 0; n < externalName.Count(); n++)
            {
                if (externalName[n] != null)
                {
                    counter++;
                }
            }        

         
            for (int x = 0; x < count; x++)
            {
                lstdefaultFields.Items.Add(displayName[x]);           
            }

            for (int y = 0; y < counter; y++)
            {
                ddlSelectId.Items.Add(externalName[y]);
            }      
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSite.Text))
            {
                try
                {
                    lblmessage.ForeColor = Color.DarkRed;
                    lblmessage.Text = "Establish connection ..... Please wait";
                    
                    using (SPSite site = new SPSite(txtSite.Text))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPListCollection lists = web.Lists;

                            foreach (SPList item in lists)
                            {
                                if (item.GetType().ToString() == "Microsoft.SharePoint.SPList")
                                {
                                    ddlListName.Items.Add(item.Title);
                                    ddlOnetoMany.Items.Add(item.Title);
                                    ddlLookupList.Items.Add(item.Title);
                                    ddllookupMany.Items.Add(item.Title);
                                }
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(2500);
                    lblmessage.Text = "Connection has been established ";
                    lblmessage.ForeColor = Color.Green;                   

                }
                catch
                {
                    lblmessage.Text = "";
                    MessageBox.Show("Code Generator has failed to connect to the SharePoint Site. Please check your URL or contact your System Administrator", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Please enter a Site","Alert" ,MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSite_TextChanged(object sender, EventArgs e)
        {

        }

        public void createEntry(String npcName, String lineToAdd, String fileName)
{
    //npcName = @"[/item1]"; <-- note the '/'.
   
   
    List<string> txtLines = new List<string>();

    //Fill a List<string> with the lines from the txt file.
    foreach (string str in File.ReadAllLines(fileName))
    {
        txtLines.Add(str);
    }

    //Insert the line you want to add last under the tag 'item1'.
    txtLines.Insert(txtLines.IndexOf(npcName), lineToAdd);

    //Clear the file. The using block will close the connection immediately.
    using (File.Create(fileName)) { }

    //Add the lines including the new one.
    foreach (string str in txtLines)
    {
        File.AppendAllText(fileName, str + Environment.NewLine);
    }
}

        private void btnCreateGroup_Click(object sender, EventArgs e)
        {
            CreateGroup formgroup = new CreateGroup();

            int c = 0;
            try
            {
                foreach (var listitem in ddlGroups.Items)
                {

                    groupCache[c] = listitem.ToString();

                    c = c + 1;
                }

                CreateGroup form2 = (CreateGroup)Application.OpenForms["CreateGroup"];

                form2.name = groupCache;
            }
            catch
            {
            }
            
            formgroup.Show();
        }

        private void btnToRight_Click(object sender, EventArgs e)
        {      
            int count = 0;

            for (int n = 0; n < savegroup.Length; n++)
            {
                if (savegroup[n] != null)
                {
                    count++;
                }
            }

            try
            {

                if (count != 0)
                {
                    savegroup[count] = ddlGroups.SelectedItem.ToString() + "," + lstFields.SelectedItem.ToString(); 

                }
                else
                {
                    savegroup[0] = ddlGroups.SelectedItem.ToString() + "," + lstFields.SelectedItem.ToString(); 
                }
                lstFieldsInGroup.Items.Add(lstFields.SelectedItem);
                lstFields.Items.Remove(lstFields.SelectedItem);
            }
            catch
            {
            }
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {         

            int count = 0;

            for (int n = 0; n < savegroup.GetLength(0); n++)
            {
                if (savegroup[n] != null)
                {
                    count++;
                }
            }

            if (count != 0)
            {           
                for (int x = 0; x < count; x++)
                {
                    string values = savegroup[x].ToString();

                    string[] choices = null;

                    choices = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                    if (ddlGroups.SelectedItem.ToString() == choices[0].ToString() && choices[1].ToString() == lstFieldsInGroup.SelectedItem.ToString())
                    {                     
                        savegroup = savegroup.Where(w => w != savegroup[x]).ToArray();
                        count = count - 1;                              
                    }
                }
            }

            lstFields.Items.Add(lstFieldsInGroup.SelectedItem);
            lstFieldsInGroup.Items.Remove(lstFieldsInGroup.SelectedItem);

        }

        private void btnAddRequired_Click(object sender, EventArgs e)
        {     
            lstAllRequiredFields.Items.Add( lstAllFields.SelectedItem);
            lstAllFields.Items.Remove(lstAllFields.SelectedItem);            
        }

        private void btnRemoveRquired_Click(object sender, EventArgs e)
        {
            lstAllFields.Items.Add(lstAllRequiredFields.SelectedItem);
            lstAllRequiredFields.Items.Remove(lstAllRequiredFields.SelectedItem);
        }       

        private void ddlOnetoMany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFieldsOne.Items.Clear();

            String[] displayName = buildArray(ddlOnetoMany.SelectedItem.ToString());

            int count = 0;

            for (int n = 0; n < displayName.Count(); n++)
            {
                if (displayName[n] != null)
                {
                    count++;
                }
            }


            for (int x = 0; x < count; x++)
            {
                ddlFieldsOne.Items.Add(displayName[x]);                
            }          
        }


        private string[] buildArray(string listname)
        {
            string[] displayName = new string[100];

            int c = 0;

            using (SPSite site = new SPSite(txtSite.Text))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists[listname];

                    foreach (SPField item in list.Fields)
                    {
                        if (!item.Hidden)
                        {
                            SPFieldType fieldType = item.Type;

                            switch (item.InternalName)
                            {
                                case "DocIcon":
                                    break;
                                case "_UIVersionString":
                                    break;
                                case "LinkTitleNoMenu":
                                    break;
                                case "Edit":
                                    break;
                                case "Attachments":
                                    break;
                                case "Editor":
                                    break;
                                case "Sindicat":
                                    break;
                                case "Modified":
                                    break;
                                case "ContentType":
                                    break;
                                case "LinkTitle":
                                    break;
                                case "ItemChildCount":
                                    break;
                                case "FolderChildCount":
                                    break;
                                case "Author":
                                    break;
                                case "Created":
                                    break;
                                case "BdcIdentity":
                                    break;
                                default:
                                   

                                        displayName[c] = item.Title;

                                        c = c + 1;

                                    
                                    break;
                            }
                        }
                    }
                    return displayName;
                }
            }
        }


        private string[] buildArrayExternalData(string listname)
        {
            string[] displayName = new string[100];

            int c = 0;

            using (SPSite site = new SPSite(txtSite.Text))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists[listname];

                    foreach (SPField item in list.Fields)
                    {
                        if (!item.Hidden)
                        {
                            SPFieldType fieldType = item.Type;

                            switch (item.InternalName)
                            {
                                case "DocIcon":
                                    break;
                                case "_UIVersionString":
                                    break;
                                case "LinkTitleNoMenu":
                                    break;
                                case "Edit":
                                    break;
                                case "Attachments":
                                    break;
                                case "Editor":
                                    break;
                                case "Sindicat":
                                    break;
                                case "Modified":
                                    break;
                                case "ContentType":
                                    break;
                                case "LinkTitle":
                                    break;
                                case "ItemChildCount":
                                    break;
                                case "FolderChildCount":
                                    break;
                                case "Author":
                                    break;
                                case "Created":
                                    break;
                                case "BdcIdentity":
                                    break;
                                default:
                                    
                                     if (item.ReadOnlyField == false)
                                    {

                                        displayName[c] = item.Title;

                                        c = c + 1;

                                    }
                                    break;
                            }
                        }
                    }
                    return displayName;
                }
            }
        }

        private void ddlFieldsOne_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage2);            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage4);          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage2); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage7);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = 0;

            for (int n = 0; n < lookupOneToOne.GetLength(0); n++)
            {
                if (lookupOneToOne[n] != null)
                {
                    count++;
                }
            }


            if (count != 0)
            {
                string aux="false";

                for (int i = 0; i < count; i++)
                {
                    string[] choices = null;

                    string values = lookupOneToOne[i].ToString();

                    choices = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    if (listboxAllFields.SelectedItem.ToString() == choices[0])
                    {
                        lookupOneToOne=lookupOneToOne.Where(w => w != lookupOneToOne[i]).ToArray();

                        if (radioButton1.Checked == true)
                        {
                            lookupOneToOne[count-1] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "dropdownlist" + "," + ddlFieldsOne.SelectedItem.ToString() + "," +ddlFieldsOne.Items[0].ToString();
                        }
                        if (radioButton2.Checked == true)
                        {
                            lookupOneToOne[count - 1] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "radiobutton" + "," + ddlFieldsOne.SelectedItem.ToString() + "," + ddlFieldsOne.Items[0].ToString();
                        }
                        aux = "true";
                    }
                }

                if (aux == "false")
                {

                    if (radioButton1.Checked == true)
                    {
                        lookupOneToOne[count] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "dropdownlist" + "," + ddlFieldsOne.SelectedItem.ToString() + "," + ddlFieldsOne.Items[0].ToString();
                    }
                    if (radioButton2.Checked == true)
                    {
                        lookupOneToOne[count] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "radiobutton" + "," + ddlFieldsOne.SelectedItem.ToString() + "," + ddlFieldsOne.Items[0].ToString();
                    }
                }
            }            
            else
            {
                if (radioButton1.Checked == true)
                {
                    lookupOneToOne[0] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "dropdownlist" + "," + ddlFieldsOne.SelectedItem.ToString() + "," + ddlFieldsOne.Items[0].ToString();
                }
                if (radioButton2.Checked == true)
                {
                    lookupOneToOne[0] = listboxAllFields.SelectedItem.ToString() + "," + ddlOnetoMany.SelectedItem.ToString() + "," + "radiobutton" + "," + ddlFieldsOne.SelectedItem.ToString() + "," + ddlFieldsOne.Items[0].ToString();
                }      
            }          
        }

        private void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstFieldsInGroup.Items.Clear();
            int count = 0;

            for (int n = 0; n < savegroup.GetLength(0); n++)
            {
                if (savegroup[n] != null)
                {
                    count++;
                }
            }

            if (count != 0)
            {
                for (int x = 0; x <count; x++)
                 {
                     string values = savegroup[x].ToString();

                     string[] choices = null;

                     choices = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                     if (ddlGroups.SelectedItem.ToString() == choices[0].ToString())
                     {
                         lstFieldsInGroup.Items.Add(choices[1].ToString());
                     }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int count = 0;

            for (int n = 0; n < lookupManyToMany.GetLength(0); n++)
            {
                if (lookupManyToMany[n] != null)
                {
                    count++;
                }
            }

            if (count != 0)
            {
                lookupManyToMany[count] = ddlLookupList.SelectedItem.ToString()+","+ddllookupMany.SelectedItem.ToString()+","+ddlBindList.SelectedItem.ToString();         
            }

            else
            {
                lookupManyToMany[0] = ddlLookupList.SelectedItem.ToString() + "," + ddllookupMany.SelectedItem.ToString() + "," + ddlBindList.SelectedItem.ToString();         
            }
        }

        private void listboxAllFields_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ExternalData_CheckedChanged(object sender, EventArgs e)
        {
            if (ExternalData.Checked == true)
            {
                ddlSelectId.Visible = true;
                lblSelectId.Visible = true;
                chkAttachments.Enabled = false;
                chkAttachments.Checked = false;
            }
            else
            {
                chkAttachments.Enabled = true;
                ddlSelectId.Visible = false;
                lblSelectId.Visible = false;
                chkAttachments.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                lstExcludeFields.Items.Add(lstdefaultFields.SelectedItem);   
                lstdefaultFields.Items.Remove(lstdefaultFields.SelectedItem);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lstdefaultFields.Items.Add(lstExcludeFields.SelectedItem);       
            lstExcludeFields.Items.Remove(lstExcludeFields.SelectedItem);

            lstFields.Items.Clear();
            lstAllFields.Items.Clear();
            listboxAllFields.Items.Clear();
            foreach (var item in lstdefaultFields.Items)
            {
                lstFields.Items.Add(item);
                lstAllFields.Items.Add(item);
                listboxAllFields.Items.Add(item);
            }
        }

        private void lstdefaultFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstFields.Items.Clear();
            lstAllFields.Items.Clear();
            listboxAllFields.Items.Clear();
            foreach (var item in lstdefaultFields.Items)
            {
                lstFields.Items.Add(item);
                lstAllFields.Items.Add(item);
                listboxAllFields.Items.Add(item);
            }
        }

        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button16_Click(object sender, EventArgs e)
        {

            int aux = 0;

            for (int j = 0; j < savegroup.Count(); j++)
            {
                if (savegroup[j] != null)
                {
                    aux++;
                }
            }

            if (aux == 0)
            {
                if (lstExcludeFields.Items.Count <= 0)
                {
                    lstFields.Items.Clear();
                    lstAllFields.Items.Clear();
                    listboxAllFields.Items.Clear();
                    foreach (var item in lstdefaultFields.Items)
                    {
                        lstFields.Items.Add(item);
                        lstAllFields.Items.Add(item);
                        listboxAllFields.Items.Add(item);
                    }

                }
            }

            tabControl.SelectTab(tabPage3);   
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(tabPage1); 
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblSelectId_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void listboxAllFields_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            lblFieldSelected.Text = listboxAllFields.SelectedItem.ToString();
            int count=0;

            for (int n = 0; n < lookupOneToOne.Count(); n++)
            {
                if (lookupOneToOne[n] != null)
                {
                    count++;
                }
            }


            string aux = "false";

            for (int i = 0; i < count; i++)
            {
                string[] choices = null;

                string values = lookupOneToOne[i].ToString();

                choices = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if(listboxAllFields.SelectedItem.ToString()== choices[0])
                {
                    String[] displayName = buildArrayExternalData(choices[1].ToString());

                    int countt = 0;

                    for (int n = 0; n < displayName.Count(); n++)
                    {
                        if (displayName[n] != null)
                        {
                            countt++;
                        }
                    }


                    for (int x = 0; x < countt; x++)
                    {
                        ddlFieldsOne.Items.Add(displayName[x]);
                    }          

                    ddlOnetoMany.SelectedItem = choices[1].ToString();
                    ddlFieldsOne.SelectedItem = choices[3].ToString();
                    if (choices[2] == "dropdownlist")
                    {
                        radioButton1.Checked = true;
                        radioButton2.Checked = false;
                    }
                    if (choices[2] == "radiobutton")
                    {
                        radioButton1.Checked = false;
                        radioButton2.Checked = true;
                    }

                    aux = "true";
                }

            }

            if (aux == "false")
            {
                ddlFieldsOne.Items.Clear();
                ddlOnetoMany.Items.Clear();
                ddlFieldsOne.Text = "";
                ddlOnetoMany.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                using (SPSite site = new SPSite(txtSite.Text))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPListCollection lists = web.Lists;

                        foreach (SPList item in lists)
                        {
                            if (item.GetType().ToString() == "Microsoft.SharePoint.SPList")
                            {
                                ddlOnetoMany.Items.Add(item.Title);
                            }
                        }
                    }
                }
            }
        }

        private void chkAttachments_CheckedChanged(object sender, EventArgs e)
        {

        }  
        
        public string GetprimaryKey(string tableName ,string cnnString)
        {      
            string ID = "";
            SqlDataReader mReader;
            SqlConnection mSqlConnection = new SqlConnection();
            SqlCommand mSqlCommand = new SqlCommand();
            string cnString= cnnString;
            mSqlConnection = new SqlConnection(cnString);
            mSqlConnection.Open();
            // sp_pkeys is SQL Server default stored procedure
            // you pass it only table Name, it will return
            // primary key column
            mSqlCommand = new SqlCommand("sp_pkeys",mSqlConnection);
            mSqlCommand.CommandType = CommandType.StoredProcedure;mSqlCommand.Parameters.Add
			            ("@table_name", SqlDbType.NVarChar).Value= tableName;
            mReader = mSqlCommand.ExecuteReader();
            while (mReader.Read())
            {
            //the primary key column resides at index 4 
            ID = mReader[3].ToString();
            }
            return ID;

         }
        
    }
}
