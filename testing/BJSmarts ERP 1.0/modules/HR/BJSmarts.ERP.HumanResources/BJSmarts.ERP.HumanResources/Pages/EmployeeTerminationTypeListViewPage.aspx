<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %> 

<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 

<%@ Import Namespace="Microsoft.SharePoint" %> 

<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,multipages_homelink_text%>" EncodeMethod="HtmlEncode"/> - <SharePoint:ProjectProperty ID="ProjectProperty1" Property="Title" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageImage" runat="server"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></asp:Content><asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
    <label class="ms-hidden"><SharePoint:ProjectProperty ID="ProjectProperty2" Property="Title" runat="server"/></label>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
		ControlId="SmallSearchInputBox" />
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server"/>

<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">
    
</asp:Content>

<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderMain" runat="server">	
    
    <table width="100%" border="0" cellpadding=0 cellspacing=0 style="padding: 5px 10px 10px 10px;">
	<tr>
		<td valign="top" width="100%">
			<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Left" Title="loc:Left" />
			&nbsp;
		</td>
    </tr>    
    <tr>
		<td>	
            <table border="0" width="100%">
            <tr>
                <td valign="top" width="50%">
                    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="One" Title="loc:One" />
			        &nbsp;
                </td>
                <td valign="top" width="50%">
                    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Two" Title="loc:Two" />
			        &nbsp;
                </td>                                
            </tr>
            </table>
        </td>
    </tr>                          
    </table>    
</asp:Content>