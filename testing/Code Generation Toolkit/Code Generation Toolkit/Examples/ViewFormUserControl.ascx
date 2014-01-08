<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewFormUserControl.ascx.cs" Inherits="CFTC.PMLC.View.Webpart.ViewForm.ViewFormUserControl" %>



<style type="text/css">
    
    .fieldName {
		font-weight: bold;
		font-size: 8pt;
		color: #000000;
		white-space: nowrap;
		text-indent: 3px;
	}
	
	.fieldNameTitle
	{
	    width: 20%;
	    font-weight: bold;
	    font-size: 8pt;
	    color: #000000;
	    text-indent: 3px;  
	}
	
	.fieldNameDescription
	{
	    width: 20%;
	    font-weight: bold;
	    font-size: 8pt;
	    color: #000000;	    
	    text-align: center;  
	}
	
	.AlternatefieldName 
	{
	    font-size: 8pt;
	    background-color: #EDEDED;   
	}
	
	.AlternatefieldNameTitle
	{
	    width: 20%;
	    font-weight: bold;
	    font-size: 8pt;
	    color: #000000;
	    text-indent: 3px;
	    background-color: #EDEDED;
	}	
	
	.AlternatefieldNameDescription
	{
	    width: 20%;
	    font-weight: bold;
	    font-size: 8pt;
	    color: #000000;
	    text-align: center; 
	    background-color: #EDEDED;
	}	
	
	.tableStyle 
	{
	    height: 30px;   
	}
	
	.tableAlternatingStyle {
		background-color: #EDEDED;
		height: 30px;
	}
	
	.tableDescriptionAlternatingStyle {
		background-color: #EDEDED;
		height: 120px;
	    
	}
	
	.tableAlternatingStyle tr td {
		font-size: 8pt;
	}

	.tableItemStyle, .tableSubHeader {
		background-color: #c9d0c3;
	}
	
	.breakline {
	    color:#9AC6FF;
	}
	
	.colorApprove {
	    color:#0CCA00;
	    font-size:30px;
	    font-weight:bold;
	}
	
	.colorRejected{
	    color:#B61600;
	    font-size:30px;
	    font-weight:bold;
	}
	
	.colorPutOnHold	{
	    color:#FFDA2F;
	    font-size:30px;
	    font-weight:bold;	    
	}
	
	.colorDeferToITL {
	    color:#0CCA00;
	    font-size:30px;
	    font-weight:bold;
	}
		
</style>



<table border="0" width="100%">
<tr height="60">
    <td align="right">
        <table border="0">
        <tr>
            <td>
                <asp:LinkButton ID="lbPrintPreview" runat="server" Visible="false" Text="Print Preview" onclick="btnPrintPreview_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
            </td>
            <td>
                <asp:LinkButton ID="lbVersionHistory" runat="server" Visible="false" Text="PIF Version History" onclick="btnVersionHistory_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
            </td>
        </tr>
        </table>
        
    </td>
</tr>
<tr>
    <td>
        <table border="0" cellpadding="0" cellspacing="0" width="1240px">
        <tr class="tableAlternatingStyle">
            <td class="fieldName">Project Name</td>
            <td>
                <asp:Label ID="lblProjectName" runat="server"></asp:Label>             
            </td>
            <td align="right" class="fieldName">Project Type&nbsp;&nbsp;&nbsp;</td>
            <td>
                <asp:Label ID="lblProjectType" runat="server"></asp:Label>                
            </td>
        </tr>
        <tr class="tableStyle">
            <td class="fieldName">Short Description</td>
            <td colspan="3">
                <asp:Label ID="lblShortDescription" runat="server"></asp:Label>
            </td>
        </tr>        
        
        <tr valign="top" class="tableDescriptionAlternatingStyle">
            <td class="fieldName">Detailed Description of Business<br />&nbsp;Need and Scope</td>
            <td colspan="3">
                <asp:Label ID="lblDescription" runat="server"></asp:Label>
            </td>
        </tr>        
        <tr class="tableStyle" >
            <td class="fieldName">Related Projects</td>
            <td class="fieldName">Is this project a follow-on effort to a previously approved<br /> &nbsp;project? if yes, please select the preceding project from <br /> &nbsp;drop down list to the right.&nbsp</td>
            <td>
                <asp:Label runat="server" ID="lblParentProjectChoice"></asp:Label>                
            </td>
            <td>
                <asp:Label ID="lblParentProject" runat="server"></asp:Label>              
            </td>
        </tr>
        <tr class="tableAlternatingStyle" valign="top">
            <td class="fieldName">Strategic Alignment</td>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldName">Primary Alignment</td>
                    <td class="fieldName">Secondary Alignment</td>
                </tr>
                <tr>
                    <td><asp:Label ID="lblPrimary" runat="server"></asp:Label></td>
                    <td><asp:Label ID="lblSecondary" runat="server"></asp:Label></td>
                </tr>
                </table>
            </td>
        </tr>
        <tr class="tableStyle">
            <td class="fieldName">Project Stakeholders</td>
            <td>
                <asp:Label  runat="server" ID="lblSponsor"></asp:Label>            
            </td>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldName">Project Stackholders (Other)</td>
                    <td class="fieldName">Project Sponsor</td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbltxtSponsorOther" runat="server"></asp:Label></td>
                    <td><asp:Label ID="lbltxtSponsor" runat="server"></asp:Label></td>
                </tr>
                </table>
            </td>                
        </tr>
        <tr valign="top" class="tableAlternatingStyle">
            <td class="fieldName">Final Project Output</td>
            <td colspan="2">
                <asp:Label   runat="server" ID="lblFinalProject"></asp:Label>
            </td>
            <td>
               <table width="100%" cellpadding="0" cellspacing="0">
               <tr>
                    <td class="fieldName">Final Project Output (Other)</td>
               </tr>
               <tr>
                    <td><asp:Label runat="server" ID="lbltxtFinalProject"></asp:Label></td>
               </tr>
               </table>                
            </td>
         </tr>        
         <tr>
            <td rowspan="10" class="fieldName">Summary of Risks</td>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Schedule</td>
                    <td><asp:Label runat="server" ID="lblRiskSchedule"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblRiskScheduleDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Resources</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblRiskResources"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblRiskResourcesDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Cost/Funding</td>
                    <td><asp:Label runat="server" ID="lblRiskCostFunding"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblRiskCostFundingDescpription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Access to Data</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblAccessData"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblAccessDataDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Security or Privacy</td>
                    <td><asp:Label runat="server" ID="lblSecurityPrivacy"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblSecurityPrivacyDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Technology</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblTechnology"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblTechnologyDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Interoperability</td>
                    <td><asp:Label runat="server" ID="lblInteroperability"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblInteroperabilityDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Feasability</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblFeasibility"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblFeasabilityDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Executive Support</td>
                    <td><asp:Label runat="server" ID="lblExecutiveSupport"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblExecutiveSupportDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Other</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblRiskOther"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblRiskOtherDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>                  
         <tr valign="top" class="tableAlternatingStyle">
            <td class="fieldName">Summary of Benefits</td>
            <td colspan="2">
                <asp:Label   runat="server" ID="lblBenefits"></asp:Label>
            </td>
            <td>
               <table width="100%">
               <tr>
                    <td class="fieldName">Summary of Benefits (Other)</td>
               </tr>
               <tr>
                    <td><asp:Label runat="server" ID="lbltxtSummaryOther"></asp:Label></td>
               </tr>
               </table>                
            </td>
         </tr>                       
        <tr class="tableStyle">
            <td class="fieldName">Budget Estimate</td>
            <td colspan="3">
                <asp:Label runat="server" ID="lbltxtBudgetEstimate"></asp:Label>               
            </td>
        </tr>
        <tr class="tableAlternatingStyle">
            <td class="fieldName">Is this project budgeted?</td>
            <td><asp:Label ID="lblyonBudget" runat="server"></asp:Label></td>
            <td class="fieldName">ProcurID</td>
            <td>
                <asp:Label  runat="server" ID="lbltxtIDprocedure"></asp:Label>                   
            </td>
        </tr>        
        <tr class="tableStyle">
            <td class="fieldName">Estimated Project Duration</td>
            <td colspan="2">
                <asp:Label  runat="server" ID="lblProjectDuration"></asp:Label>                 
            </td>
            <td>
                <table>
                <tr>
                    <td class="fieldName">Suggested Start Date</td> 
                    <td class="fieldName">Suggested End Date</td> 
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="lblDataTimeStartDate"></asp:Label></td>
                    <td><asp:Label runat="server"  ID="lblDateTimeEndDate"></asp:Label></td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td rowspan="12" class="fieldName">Project Coordination</td>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Change Management</td>
                    <td><asp:Label runat="server" ID="lblConfiguration"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblConfigurationDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Data Management and DEAP</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblDataManagement"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblDataManagementDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Enterprise Architecture</td>
                    <td><asp:Label runat="server" ID="lblEnterprise"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblEnterpriseDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Enterprise Solutions</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblServices"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblServicesDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Information Governance</td>
                    <td><asp:Label runat="server" ID="lblInfrastructure"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblInfrastructureDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Infrastructure: Customer Support/Helpdesk</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblNetwork"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblNetworkDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Infrastructure: Network</td>
                    <td><asp:Label runat="server" ID="lblTelecom"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblTelecomDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Infrastructure: Telecom</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblODTCommunications"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblODTCommunicationsDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">ODT Communications</td>
                    <td><asp:Label runat="server" ID="lblPrivacyOffice"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblPrivacyOfficeDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Procurement</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblRecordsManagement"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblRecordsManagementDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr> 
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="fieldNameTitle">Section 508</td>
                    <td><asp:Label runat="server" ID="lblSection508"></asp:Label></td>
                    <td class="fieldNameDescription">Description</td>
                    <td width="80%"><asp:Label runat="server" ID="lblSection508Description"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>     
         <tr>
            <td colspan="3">
                <table width="100%">
                <tr>
                    <td class="AlternatefieldNameTitle">Web and Intranet</td>
                    <td class="AlternatefieldName"><asp:Label runat="server" ID="lblSecurityImpact"></asp:Label></td>
                    <td class="AlternatefieldNameDescription">Description</td>
                    <td width="80%" class="AlternatefieldName"><asp:Label runat="server" ID="lblSecurityImpactDescription"></asp:Label></td>
                </tr>
                </table>
            </td>
         </tr>            
         
        <tr class="tableAlternatingStyle">
            <td class="fieldName">Proposed Project Manager</td>
            <td><asp:Label runat="server" ID="lblProposedPm" ></asp:Label></td>
            <td class="fieldName">PM Branch</td>
            <td>
                <asp:Label ID="lblPMBranch" runat="server"  ></asp:Label>             
            </td>
        </tr>
        <tr class="tableStyle">
            <td class="fieldName">Proposed Project Team</td>
            <td colspan="3">
                <asp:Label  ID="lblpeTeam" runat="server"></asp:Label>                
            </td>
         </tr>
        <tr class="tableAlternatingStyle">
            <td width="200px" class="fieldName">Attachments</td>
            <td colspan="3">
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
                            </tr>            
                    </ItemTemplate>
                    <FooterTemplate>  
                        </table>  
                    </FooterTemplate> 
                </asp:DataList>
            </td>
        </tr>
        <tr  class="tableStyle">
            <td width="200px" class="fieldName">Submitted By</td>
            <td colspan="3">
                <asp:Label ID="lblSubmittedBy" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="tableAlternatingStyle">
            <td width="200px" class="fieldName">Submitted Date</td>
            <td colspan="3">
                <asp:Label ID="lblSubmittedDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="200px" class="fieldName">Comments</td>
            <td colspan="3"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
         </tr>
        </table>
    </td>
</tr>
<tr>
    <td align="center"><asp:Label ID="lblMessage" runat="server"></asp:Label></td>
</tr>
<tr height="60">
    <td align="right">       
        
        <asp:LinkButton ID="lbEdit" runat="server" Visible="false" Text="Edit" onclick="btnEdit_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
        <asp:LinkButton ID="lbReSubmit" runat="server" Visible="false" Text="Resubmit" onclick="btnReSubmit_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
        <asp:LinkButton ID="lbApprove" runat="server" Visible="false" Text="Approve" OnClick="btnApprove_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>
        <asp:LinkButton ID="lbReject" runat="server" Visible="false" Text="Reject" OnClick="btnReject_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>        
        <asp:LinkButton ID="lbPutonHold" runat="server" Visible="false" Text="Put On Hold" OnClick="btnPutOnHold_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>
        <asp:LinkButton ID="lbDeferToITL" runat="server" Visible="false" Text="Defer to ITL" OnClick="btnDeferToITL_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>
    </td>
</tr>
</table>