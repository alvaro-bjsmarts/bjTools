using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace TestWorkflow.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            workflowProperties.Item["ProjectId"] = GetEmployeeId();

            //get the url location of edit form
            String editform = "/Pages/EmployeesUpdatePage.aspx";


            //set the project title
            SPFieldUrlValue spFieldUrlValue = new SPFieldUrlValue();
            spFieldUrlValue.Url = editform + "?RecordID=" + workflowProperties.Item["ID"].ToString();
            spFieldUrlValue.Description = workflowProperties.Item["Last_x0020_Name"].ToString() + ", " + workflowProperties.Item["First_x0020_Name"].ToString();
            workflowProperties.Item["Edit_x0020_Title"] = spFieldUrlValue;

            //get the url location of view form
            String viewform = "/Pages/EmployeesViewPage.aspx";

            SPFieldUrlValue spViewFieldUrlValue = new SPFieldUrlValue();
            spViewFieldUrlValue.Url = viewform + "?RecordID=" + workflowProperties.Item["ID"].ToString();
            spViewFieldUrlValue.Description = workflowProperties.Item["Last_x0020_Name"].ToString() + ", " + workflowProperties.Item["First_x0020_Name"].ToString();
            workflowProperties.Item["View_x0020_Title"] = spViewFieldUrlValue;

            workflowProperties.Item.Update();
        }

        private String GetEmployeeId()
        {
            String newId = String.Empty;

            long IdValue = long.Parse(workflowProperties.Item["ID"].ToString());
            String StartNum = IdValue < 10 ? "00" : "0";
            StartNum = IdValue > 99 ? "" : StartNum;
            String TrackingId = StartNum + IdValue.ToString();

            newId = TrackingId.ToString();

            return newId;
        }
    }
}
