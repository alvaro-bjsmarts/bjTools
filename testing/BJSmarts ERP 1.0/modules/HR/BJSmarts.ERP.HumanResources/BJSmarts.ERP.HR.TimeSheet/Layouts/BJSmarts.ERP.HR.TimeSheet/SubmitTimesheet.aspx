<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitTimesheet.aspx.cs" Inherits="BJSmarts.ERP.HumanResources.Layouts.BJSmarts.ERP.HumanResources.SubmitTimesheet" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<style type="text/css">
.aspButtonClass
{
    height: 20px;
    border: 1px solid #666;
    margin-bottom: 5px;
}
</style>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<h2>Employee Hours</h2>

<asp:Table ID="EmployeeDetails" runat="server">
</asp:Table>
<br />
<asp:Table ID="Timesheet" GridLines="Both" runat="server" Width="100%">
</asp:Table>
<br /><br />
<table border="0" width="100%" >
<tr>
    <td align="right">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" onClientClick="return confirm('Are you sure you want to submit your TimeSheet?')" CssClass="aspButtonClass" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" CssClass="aspButtonClass" />
    </td>
</tr>
</table>


</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
