using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;

namespace BJSmarts.ERP.HumanResources.WebParts.EmployeeTypesListWebPart
{
    public partial class EmployeeTypesListWebPartUserControl : UserControl
    {
        private int intLCID = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (intLCID == 1033)
            {
                intLCID = 0;
            }

            if (intLCID == 3082)
            {
                intLCID = 1;
            }

            bool comproveDeleted = false;
            bool comproveLanguage = false;

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList oList = web.Lists["Employee Types"];

                    for (int i = 0; i < oList.Fields.Count; i++)
                    {
                        if (oList.Fields[i].Title == "Language")
                        {
                            comproveLanguage = true;
                        }
                        if (oList.Fields[i].Title == "Deleted")
                        {
                            comproveDeleted = true;
                        }
                    }

                    DataTable data = new DataTable();

                    SPQuery query = new SPQuery();


                    if (comproveDeleted == true && comproveLanguage == true)
                    {
                        query.Query = "<Where><And><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></And></Where>";
                    }

                    if (comproveDeleted == false && comproveLanguage == true)
                    {
                        query.Query = "<Where><Eq><FieldRef Name='Language'/><Value Type='Number'>" + intLCID + "</Value></Eq></Where>";
                    }

                    if (comproveDeleted == true && comproveLanguage == false)
                    {
                        query.Query = "<Where><Eq><FieldRef Name='Deleted'/><Value Type='Number'>0</Value></Eq></Where>";
                    }

                    if (comproveDeleted == false && comproveLanguage == false)
                    {
                        query.Query = "";
                    }


                    data = oList.GetItems(query).GetDataTable();

                    GridView1.DataSource = data;

                    GridView1.DataBind();
                }
            }
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intId = Convert.ToInt32(GridView1.DataKeys[GridView1.SelectedIndex]["EmployeeTypeId"]);

                Response.Redirect("/Pages/EmployeeTypesViewPage.aspx?RecordID=" + intId, false);
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
            }
        }

        protected void GridView1_EditIndexChanged(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridView1.EditIndex = e.NewEditIndex;

                int intId = Convert.ToInt32(GridView1.DataKeys[GridView1.EditIndex]["EmployeeTypeId"]);

                Response.Redirect("/Pages/EmployeeTypesUpdatePage.aspx?RecordID=" + intId, false);
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
            }
        }


        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int intId = Convert.ToInt32(GridView1.DataKeys[e.Row.DataItemIndex]["EmployeeTypeId"]);

                    string item = e.Row.Cells[1].Text;
                    foreach (ImageButton button in e.Row.Cells[0].Controls.OfType<ImageButton>())
                    {
                        if (button.CommandName == "Delete")
                        {
                            button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int intId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex]["EmployeeTypeId"]);

            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // Fetch the List
                    SPList oList = web.Lists["Employee Types"];


                    // Update the List item by ID
                    SPListItem itemToUpdate = oList.GetItemById(intId);
                    itemToUpdate["Deleted"] = 1;
                    itemToUpdate.Update();

                    web.AllowUnsafeUpdates = false;
                }
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
