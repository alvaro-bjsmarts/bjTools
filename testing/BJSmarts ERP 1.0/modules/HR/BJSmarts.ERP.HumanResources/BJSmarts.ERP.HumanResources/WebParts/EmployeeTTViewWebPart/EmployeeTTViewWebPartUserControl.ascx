<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeTTViewWebPartUserControl.ascx.cs" Inherits="BJSmarts.ERP.HumanResources.WebParts.EmployeeTTViewWebPart.EmployeeTTViewWebPartUserControl" %>


<style type="text/css">
    
  .listSubTitle {
		background-color: #3466A8;
		border-bottom: 0 none !important;
		color: #FFFFFF;
		font: bold 9pt Verdana,Arial,Helvetica,sans-serif;
		margin: 0;
		padding: 5px;
	}

	 .aspButtonClass {
        height: 20px;
        border: 1px solid #666;
        margin-bottom: 5px;
    }

</style>
<table width="50%">
			<tr>	
				 <td colspan="2" width="100%"><div class="listSubTitle">Group 1</div></td>
			</tr>
						<tr>		
						 <td  width="165px"><asp:label id="labelName" runat="server" /></td><td><asp:Label runat="server" ID="lblName"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelDescription" runat="server" /></td><td><asp:Label runat="server" ID="lblDescription"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelSort_Order" runat="server" /></td><td><asp:Label runat="server" ID="lblSort_Order"></asp:Label></td>
						</tr>		
	
<tr height="60">
	<td align="right"  colspan="2">		
		<asp:Button ID="lbEdit" runat="server" Text="Edit" OnClick="b_Click"  CssClass="aspButtonClass" />
		<asp:Button ID="lbBack" runat="server" Text="Back" OnClientClick="javascript:history.back(); return false;"  CssClass="aspButtonClass"/>
	</td>
</tr> 		
</table>