<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyListViewWebPartUserControl.ascx.cs" Inherits="BJSmarts.ERP.SharePoint.WebParts.Currency.CurrencyListViewWebPart.CurrencyListViewWebPartUserControl" %>

<style type="text/css">
 .listSubTitle {
		background-color: #3466A8;
		border-bottom: 0 none !important;
		color: #FFFFFF;
		font: bold 9pt Verdana,Arial,Helvetica,sans-serif;
		margin: 0;
		padding: 5px;
	}
 .altercolor
 {
     background-color:#B9CCE5;
 }
 
 .gridViewStyle
 {
     min-width:500px;
 }
 </style>
<table cellspacing="0" cellpadding="0">
<tr><td><div class="listSubTitle">Currency Type</div></td></tr>
<tr>
<td>
<asp:GridView ID="GridView1" CssClass="gridViewStyle" runat="server"  DataKeyNames="CurrencyTypeId" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#E8EDF2"  OnRowDeleting="GridView1_RowDeleting" OnRowDataBound="OnRowDataBound" OnRowEditing="GridView1_EditIndexChanged" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
    <Columns>
        <asp:CommandField  ShowSelectButton="true" EditImageUrl="/_layouts/Images/BJSmarts.ERP.SharePoint/edit.gif" DeleteImageUrl="/_layouts/Images/BJSmarts.ERP.SharePoint/delete.gif" SelectImageUrl="/_layouts/Images/BJSmarts.ERP.SharePoint/View.gif" ShowDeleteButton="true" ShowEditButton="true" ButtonType="Image" />
        <asp:BoundField DataField="Name" HeaderText="Name" />       
        <asp:BoundField DataField="Description" HeaderText="Description" />       
        <asp:BoundField DataField="Sort_Order" HeaderText="Sort_Order" />       
     </Columns>
</asp:GridView>
</td>
</tr>
<tr>
<td>
    <asp:LinkButton ID="LinkButton1" PostBackUrl="/Pages/CurrencyTypeInsertPage.aspx" runat="server">Add Item</asp:LinkButton>
</td>
</tr>
</table>