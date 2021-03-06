﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateFormUserControl.ascx.cs" Inherits="CFTC.PMLC.Update.Webpart.UpdateForm.UpdateFormUserControl" %>
<%@ Register TagPrefix="spuc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"  %>

<script type = "text/javascript">

    function DeleteItem(file)
    {
          var product = file;
          <%= Page.ClientScript.GetCallbackEventReference(this, "product", "ReceiveServerData",null)%>;
    }

    function ReceiveServerData(rValue)
    {
         document.getElementById('datalist').innerHTML = rValue;
    }

    var counter = 0;

    function AddFileUpload() {

        var div = document.createElement('DIV');

        div.innerHTML = '<input id="file' + counter + '" name = "file' + counter +
                     '" type="file" />' +
                     '<input id="Button' + counter + '" type="button" ' +
                     'value="Remove" onclick = "RemoveFileUpload(this)" />';

        document.getElementById("UpdateFileUploadContainer").appendChild(div);

        counter++;
    }

    function RemoveFileUpload(div) {

        document.getElementById("UpdateFileUploadContainer").removeChild(div.parentNode);

    }

</script>

<style type="text/css">
    
    .text {
	 font-family: Ms Shell Dlg;
	 font-size: 13.3333px;
	}
	.altercolortable {
	 background-color:#EDEDED;
	}
	.font {	 
	 font-weight:bold;
	}

</style>

<table border="0" width="100%">
<tr>
    <td>
        <table border="0" cellspacing="0">
        <tr class="altercolortable">
            <td class="font">Project Name</td>
            <td>
                <asp:TextBox ID="txtProjectName" runat="server" Width="386px" MaxLength="255" alt="Project Name"></asp:TextBox><br />
                <asp:Label ID="lblProjectNameValidator" runat="server" ForeColor="Red" Visible="false" Text="You must specify a value for this required field." />
            </td>
            <td class="font" align="right">Project Type&nbsp;&nbsp</td>
            <td>
                <asp:DropDownList  ID="ddlProjectType" runat="server" ToolTip="Project Type"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="font">Short Description</td>
            <td colspan="3">
                <asp:TextBox ID="txtShortDescription" Width="100%" class="text" TextMode="MultiLine" HtmlEncode="false" runat="server" MaxLength="255" alt="Description"></asp:TextBox>
            </td>
        </tr>
        <tr class="altercolortable">
            <td class="font">Detailed Description of Business<br />Need and Scope</td>
            <td colspan="3">
                <asp:TextBox ID="txtDescription" Width="100%" class="text" Height="240px"  TextMode="MultiLine" HtmlEncode="false" runat="server" alt="Description"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <hr width="100%" />
            </td>
        </tr>
        <tr>
            <td class="font">Related Projects</td>
            <td>Is this project a follow-on effort to a previously approved parent project?<br />If yes, please select the preceding project from the drop down list to the right.</td>
            <td>
                <asp:RadioButtonList runat="server" ID="rdParentProjectChoice"  RepeatColumns="2" ToolTip="Parent Project Choice" />
            </td>
            <td><asp:DropDownList ID="ddlParentProject" runat="server" ToolTip="Parent Project" ></asp:DropDownList></td>
        </tr>
        <tr class="altercolortable">
            <td class="font">Strategic Alignment</td>
            <td colspan="3">
                <table>
                <tr>
                    <td class="font">Primary Alignment:</td>
                </tr>
                <tr>
                    <td><asp:DropDownList ID="ddlPrimary" runat="server" ToolTip="Primary"></asp:DropDownList></td>
                </tr>
                <tr>                   
                     <td class="font">Secondary Alignment:</td>
                </tr>
                <tr>
                    <td><asp:DropDownList ID="ddlSecondary" runat="server" ToolTip="Secondary"></asp:DropDownList></td>
                </tr>
                </table>                                                
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
            <hr width="100%" />
            </td>
        </tr>
        
        <tr valign="top">
            <td class="font">Project Stakeholders (select all that apply)</td>
            <td>
                <asp:CheckBoxList RepeatColumns="7" runat="server" ID="chkSponsor" ToolTip="Sponsor List"></asp:CheckBoxList>
                &nbsp;&nbsp;<asp:TextBox ID="txtSponsor" MaxLength="255" runat="server" Width="350px" alt="Sponsor"></asp:TextBox>
            </td>
            <td class="font">Project Sponsor</td>
            <td>
                <wssawc:PeopleEditor ID="peProjectSponsor" runat="server" width="300px" AllowEmpty="true" MultiSelect="true" SelectionSet="User" BorderColor="Black"  />                
            </td>   
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
        </tr>
        <tr class="altercolortable">
            <td class="font">Final Project Output<br/>(select all that apply)</td>
            <td colspan="1">
               <asp:CheckBoxList runat="server" ID="chkFinalProject" RepeatColumns="3" ToolTip="Final Project List"  ></asp:CheckBoxList>
            </td>
            <td>If other, please<br />describe</td>
            <td>
                <asp:TextBox runat="server" class="text" ID="txtFinalProject" Width="336px" TextMode="MultiLine" MaxLength="255" alt="Final Project"></asp:TextBox></td>
         </tr>
         <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
         </tr>
         <tr>
            <td class="font" rowspan="10">Summary of Risks<br />(select all that apply)</td>
            <td>
                <asp:CheckBox runat="server" ID="chkRiskSchedule" ToolTip="Risk Schedule" />
                Schedule
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskSchedule" Width="300" runat="server" MaxLength="255" alt="Risk Schedule Description"></asp:TextBox>
            </td>
        </tr>
        <tr class="altercolortable">
            <td>
                <asp:CheckBox runat="server" ID="chkRiskResources" ToolTip="Risk Resources" />
                Resources
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskResources" Width="300" runat="server" MaxLength="255" alt="Risk Resources Description"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkRiskCostFunding" ToolTip="Risk Cost / Funding" />
                Cost / Funding
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskCostFunding" Width="300" runat="server" MaxLength="255" alt="Risk Cost Funding Description"></asp:TextBox>
            </td>
        </tr>
         <tr class="altercolortable">
            <td>
                <asp:CheckBox runat="server" ID="chkRiskAccesstoData" ToolTip="Risk Access to Data" />
                Access to Data
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskAccesstoData" Width="300" runat="server" MaxLength="255" alt="Risk Access to Data Description"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkRiskSecurityorPrivacy" ToolTip="Risk Security or Privacy" />
                Security or Privacy
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskSecurityorPrivacy" Width="300" runat="server" MaxLength="255" alt="Risk Security or Privacy Description"></asp:TextBox>
            </td>
        </tr>
         <tr class="altercolortable">
            <td>
                <asp:CheckBox runat="server" ID="chkRiskTechnology" ToolTip="Risk Technology" />
                Technology
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskTechnology" Width="300" runat="server" MaxLength="255" alt="Risk Technology Description"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkRiskInteroperability" ToolTip="Risk Interoperability" />
                Interoperability
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskInteroperability" Width="300" runat="server" MaxLength="255" alt="Risk Interoperability Description"></asp:TextBox>
            </td>
        </tr>
        <tr class="altercolortable">
            <td>
                <asp:CheckBox runat="server" ID="chkRiskFeasibility" ToolTip="Risk Feasibility" />
                Feasibility
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskFeasibility" Width="300" runat="server" MaxLength="255" alt="Risk Feasibility Description"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkRiskExecutiveSupport" ToolTip="Risk Executive Support" />
                Executive Support
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskExecutiveSupport" Width="300" runat="server" MaxLength="255" alt="Risk Executive Support Description"></asp:TextBox>
            </td>
        </tr>
        <tr class="altercolortable">
            <td>
                <asp:CheckBox runat="server" ID="chkRiskOther" ToolTip="Risk Other" />
                Other
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRiskOther" Width="300" runat="server" MaxLength="255" alt="Risk Other Description"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
        </tr>         
        <tr class="altercolortable">
            <td class="font">Summary of Benefits<br/>(select all that apply)</td>
            <td colspan="2">
                <asp:CheckBoxList ID="chkBenefits" RepeatColumns="3" runat="server" ToolTip="Benefits List"></asp:CheckBoxList>
            </td>
            <td>
                <asp:TextBox runat="server" Width="250" class="text" ID="txtSummaryOther" MaxLength="255" TextMode="MultiLine" alt="Summary Other"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <hr width="100%" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="4">
                <b>STOP:</b> Do not enter budget estimates directly in this field as the information is procurement-sensitive. Complete an Independent Government Cost Estimate <br /> and upload it to the Project Budget Estimates library. Once completed, insert a link to your cost estimate in this field.
            </td>
        </tr>
        <tr>
            <td class="font" rowspan="2">Budget Estimate</td>
            <td colspan="3">
                <asp:TextBox runat="server" ID="txtDescriptionRoughOrder" class="text" MaxLength="255" Width="900px" alt="Description Rough Order"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Is this project budgeted?&nbsp;&nbsp;<asp:RadioButtonList  ID="rbtnyonBudget" RepeatColumns="2" runat="server" ToolTip="Budget"/></td>
            <td class="font" align="right">ProcurID&nbsp;&nbsp</td>
            <td> 
                    <asp:TextBox runat="server" ID="txtIDprocedure" MaxLength="255" Width="200px" alt="ID Procedure"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
        </tr>
        
        <tr class="altercolortable">
            <td class="font">Estimated Project Duration</td>
            <td colspan="2">
                <asp:RadioButtonList runat="server" RepeatColumns="4" ID="rbtnProjectDuration" ToolTip="Project Duration"></asp:RadioButtonList>
            </td>
            <td>
                <table>
                <tr>
                    <td class="font">Suggested Start Date <br /><spuc:DateTimeControl runat="server" ID="DataTimeStartDate" DateOnly="true" /></td>
                    <td class="font">Suggested End Date <br /><spuc:DateTimeControl runat="server" ID="DateTimeEndDate" DateOnly="true" /></td>
                </tr>
                </table>
                <asp:Label ID="lblDateValidator" runat="server" ForeColor="Red" Visible="false" Text="Start Date must be before End Date." />  
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
        </tr>

        <tr>
            <td></td>
            <td colspan="3">
            Select all PMLC Advisory Forum members that may share a dependency with the project.  Briefly describe the dependency in the <br />
            corresponding text box(es). Dependencies entered here will be automatically populated to the Project Dependencies list.  You may add new <br />
            dependencies and update existing dependencies after PIF submissions.<br />     
            </td>
         </tr>
         <tr>
            <td class="font" rowspan="12">Project Coordination</td>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkConfigurationManagement" ToolTip="Configuration Management" />
                Change Management
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtCMDetails" Width="300" runat="server" MaxLength="255" alt="Configuration Management Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkDataManagement" ToolTip="Data Management" />
                Data Management and DEAP
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtDMDetails" Width="300" runat="server" MaxLength="255" alt="Data Management Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkEnterpriseArchitecture" ToolTip="Enterprise Architecture" />
                Enterprise Architecture
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtEADetails" Width="300" runat="server" MaxLength="255" alt="Enterprise Architecture Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkEnterpriseServices" ToolTip="Enterprise Services" />
                Enterprise Solutions
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtEnterpriseServices" Width="300" runat="server" MaxLength="255" alt="Enterprise Services Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkInfrastructureCustomer" ToolTip="Infrastructure Customer" />
                Information Governance
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtInfrastructureCustomer" Width="300" runat="server" MaxLength="255" alt="Infrastructure Customer Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkInfrastructureNetwork" ToolTip="Infrastructure Network" />
                Infrastructure: Customer Support/Helpdesk
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtInfrastructureNetwork" Width="300" runat="server" MaxLength="255" alt="Infrastructure Network Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkInfrastructureTelecom" ToolTip="Infrastructure Telecom" />
                Infrastructure: Network
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtInfrastructureTelecom" Width="300" runat="server" MaxLength="255" alt="Infrastructure Telecom Dependency Details"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkODTCommunications" ToolTip="ODT Communications" />
                Infrastructure: Telecom
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtODTCommunications" Width="300" runat="server" MaxLength="255" alt="ODT Communications Dependency Details"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkPrivacyOffice" ToolTip="Privacy Office" />
                ODT Communications
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtPrivacyOffice" Width="300" runat="server" MaxLength="255" alt="Privacy Office Dependency Details"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkRecordManagement" ToolTip="Record Management" />
                Procurement
            </td>
            <td  align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtRecordManagement" Width="300" runat="server" MaxLength="255" alt="Record Management Dependency Details"></asp:TextBox>
            </td>
         </tr>
          <tr>
            <td class="altercolortable">
                <asp:CheckBox runat="server" ID="chkSection" ToolTip="Section 508" />
                Section 508
            </td>
            <td class="altercolortable" align="right">Description&nbsp;&nbsp</td>
            <td class="altercolortable">
                <asp:TextBox ID="txtSection" Width="300" runat="server" MaxLength="255" alt="Section 508 Details"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td>
                <asp:CheckBox runat="server" ID="chkSecurityImpact" ToolTip="Security Impact" />
                Web and Intranet
            </td>
            <td align="right">Description&nbsp;&nbsp</td>
            <td>
                <asp:TextBox ID="txtSecurityImpact" Width="300" runat="server" MaxLength="255" alt="Security Impact Dependency Details"></asp:TextBox>
            </td>
         </tr>
        
        
         <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
         </tr>         
         <tr class="altercolortable">
            <td class="font">Proposed Project Manager</td>
            <td>
                <wssawc:PeopleEditor ID="peProposedManager" runat="server" width="350px" AllowEmpty="true" MultiSelect="false" SelectionSet="User"  />
                <asp:Label ID="lblvalidatorProposedManager" runat="server" ForeColor="Red" Visible="false" Text="You must specify a value for this required field." />          
            </td>
            <td class="font" align="right">PM Branch</td>
            <td>
                <asp:RadioButtonList runat="server" ID="rbtnPmBranch"  RepeatColumns="2" ToolTip="PM Branch List" />
                <asp:Label ID="lblvalidatorPmBranch" runat="server" ForeColor="Red" Visible="false" Text="You must choose a value for this required field." />
            </td>
        </tr>
        <tr>
            <td class="font">Proposed Project Team</td>
            <td colspan="3">
                <wssawc:PeopleEditor ID="peTeam" runat="server" width="350px" AllowEmpty="true" MultiSelect="true" SelectionSet="User"  />
            </td>
        </tr>
        <tr class="altercolortable">
            <td class="font" width="200px">Attachments</td>
            <td colspan="3">
            <div id="datalist">    
                <asp:DataList ID="dlfiles" runat="server" RepeatLayout="Table" RepeatColumns="1"
                    CellPadding="0" CellSpacing="0" >
                    <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">  
                    <tr>  
                        <th>  
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>  
                        </th>  
                        <th>  
                            <asp:Label ID="lblBlank" runat="server"></asp:Label>  
                        </th>    
                    </tr>  
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>                    
                                <td>
                                    <b><span class="name">
                                            <asp:LinkButton ID="lbdoc" runat="server" OnClientClick=<%#"window.open('" + Eval("FilenameUrl")+ "')" %> Text='<%# Eval("Filename") %>' />
                                    </span></b>
                                </td>
                                <td>&nbsp;&nbsp;<img src="/_layouts/images/rect.gif" alt="Delete">&nbsp;<asp:LinkButton ID="lbdelete" runat="server" OnClientClick=<%# "DeleteItem('" + Eval("Filename") + "'); return false;" %>  >Delete</asp:LinkButton></td>
                            </tr>            
                    </ItemTemplate>
                    <FooterTemplate>  
                        </table>  
                    </FooterTemplate> 
                </asp:DataList>

            </div>
            </td>
        </tr>

        <tr>
                <td width="200px"></td>
                <td>
                    <span style ="font-family:Arial">Click to add files</span>&nbsp;&nbsp;
                    <input id="Button2" type="button" value="add" onclick = "AddFileUpload()" />
                    <br /><br />
                    <div id = "UpdateFileUploadContainer">
                        <!--FileUpload Controls will be added here -->
                    </div>
                </td>
        </tr>
        
        <tr class="altercolortable">
            <td class="font" width="200px">Submitted By</td><td colspan="3"><asp:Label ID="lbluserPIF" runat="server"></asp:Label>
        </td>
        </tr>
        <tr>
            <td class="font" width="200px">Submitted Date</td><td><asp:Label ID="lblDate" runat="server"></asp:Label>
            </td>
        </tr>
         <tr>
            <td></td>
            <td colspan="3">
                <hr  width="100%"/>
            </td>
         </tr>
        
        </table>
    </td>
</tr>
<tr height="60">
    <td align="right">
        <asp:LinkButton ID="lbChangeDecision" runat="server" Visible="false" Text="Change Decision" onclick="btnChangeDecision_Click" onClientClick="return confirm('Do you want to save your changes?')" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
        <asp:LinkButton ID="lbSubmitApproval" runat="server" Visible="false" Text="Submit for Review" onclick="btnSubmitApproval_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
        <asp:LinkButton ID="lbOK" runat="server" Text="Save" onclick="btnOK_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
        <asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>        
    </td>
</tr>
</table>
