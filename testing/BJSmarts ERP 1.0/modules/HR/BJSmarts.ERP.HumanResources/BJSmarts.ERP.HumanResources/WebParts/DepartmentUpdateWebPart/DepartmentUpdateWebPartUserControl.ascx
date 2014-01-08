<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentUpdateWebPartUserControl.ascx.cs" Inherits="BJSmarts.ERP.HumanResources.WebParts.DepartmentUpdateWebPart.DepartmentUpdateWebPartUserControl" %>


<style type="text/css">
    
   .aspButtonClass {
        height: 20px;
        border: 1px solid #666;
        margin-bottom: 5px;
    }

	  .listSubTitle {
		background-color: #3466A8;
		border-bottom: 0 none !important;
		color: #FFFFFF;
		font: bold 9pt Verdana,Arial,Helvetica,sans-serif;
		margin: 0;
		padding: 5px;
	}

</style>

<table width="50%">
<tr>	
<td colspan="2" width="100%"><div class="listSubTitle">Department</div></td>
</tr>
		<tr>		
			<td><asp:label id="labelName" runat="server" /></td><td><asp:TextBox ID="txtName" runat="server" Width="386px" alt="Name" MaxLength="255"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidatorName" runat="server" Display="Dynamic"  ErrorMessage="*" ControlToValidate="txtName"  ForeColor="Red"></asp:RequiredFieldValidator></td>
		</tr>
		<tr>		
			<td><asp:label id="labelDescription" runat="server" /></td><td><asp:TextBox ID="txtDescription" runat="server" Width="386px" alt="Description" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelStreeAddress1" runat="server" /></td><td><asp:TextBox ID="txtStreeAddress1" runat="server" Width="386px" alt="StreeAddress1" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelStreeAddress2" runat="server" /></td><td><asp:TextBox ID="txtStreeAddress2" runat="server" Width="386px" alt="StreeAddress2" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelCity" runat="server" /></td><td><asp:TextBox ID="txtCity" runat="server" Width="386px" alt="City" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelState_x002f_Province" runat="server" /></td><td><asp:TextBox ID="txtState_x002f_Province" runat="server" Width="386px" alt="State_x002f_Province" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelZip_x002f_PostalCode" runat="server" /></td><td><asp:TextBox ID="txtZip_x002f_PostalCode" runat="server" Width="386px" alt="Zip_x002f_Postal_x0020_Code" MaxLength="255"></asp:TextBox></td>
		</tr>				 
		<tr>		
			<td><asp:label id="labelSort_Order" runat="server" /><td><asp:TextBox ID="txtSort_Order" runat="server" Width="386px" alt="Sort_Order" MaxLength="255"></asp:TextBox></td>
		</tr>
<tr height="60">
	<td align="right"  colspan="2">				
		<asp:Button ID="lbOK" runat="server" Text="Save" Onclick="btnOK_Click" CssClass="aspButtonClass"/>
		<asp:Button ID="lbBack" runat="server" Text="Back" OnClientClick="javascript:history.back(); return false;" CssClass="aspButtonClass"/>
	</td>
</tr>
</table>