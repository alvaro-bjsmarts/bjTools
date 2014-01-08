<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeViewWebPartUserControl.ascx.cs" Inherits="BJSmarts.ERP.HumanResources.WebParts.EmployeeViewWebPart.EmployeeViewWebPartUserControl" %>



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
<table width="100%">
			<tr>	
				 <td colspan="2" width="100%"><div class="listSubTitle">Personal Information</div></td>
			</tr>
						<tr>		
						 <td  width="165px"><asp:label id="labelEmployeeId" runat="server" /></td><td><asp:Label runat="server" ID="lblEmployeeId"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelFirstName" runat="server" /></td><td><asp:Label runat="server" ID="lblFirstName"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelLastName" runat="server" /></td><td><asp:Label runat="server" ID="lblLastName"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelEmail" runat="server" /></td><td><asp:Label runat="server" ID="lblEmail"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomePhone" runat="server" /></td><td><asp:Label runat="server" ID="lblHomePhone"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkPhone" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkPhone"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelMovilPhone" runat="server" /></td><td><asp:Label runat="server" ID="lblMovilPhone"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHireDate" runat="server" /></td><td><asp:Label runat="server" ID="lblHireDate"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelBirthDate" runat="server" /></td><td><asp:Label runat="server" ID="lblBirthDate"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelSalary" runat="server" /></td><td><asp:Label runat="server" ID="lblSalary"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelEmployeeGovernmentId" runat="server" /></td><td><asp:Label runat="server" ID="lblEmployeeGovernmentId"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelBiometricEmployeeId" runat="server" /></td><td><asp:Label runat="server" ID="lblBiometricEmployeeId"></asp:Label></td>
						</tr>		
			<tr>	
				 <td colspan="2" width="100%"><div class="listSubTitle">Employee Information</div></td>
			</tr>
						<tr>		
						 <td  width="165px"><asp:label id="labelEmployeeType" runat="server" /></td><td><asp:Label runat="server" ID="lblEmployeeType"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelResourceType" runat="server" /></td><td><asp:Label runat="server" ID="lblResourceType"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelGender" runat="server" /></td><td><asp:Label runat="server" ID="lblGender"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelCurrency" runat="server" /></td><td><asp:Label runat="server" ID="lblCurrency"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelMaritalStatus" runat="server" /></td><td><asp:Label runat="server" ID="lblMaritalStatus"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelEmployeeTermination_x0020" runat="server" /></td><td><asp:Label runat="server" ID="lblEmployeeTermination_x0020"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelDepartment" runat="server" /></td><td><asp:Label runat="server" ID="lblDepartment"></asp:Label></td>
						</tr>		
			<tr>	
				 <td colspan="2" width="100%"><div class="listSubTitle">Home Address</div></td>
			</tr>
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressStreet_" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressStreet_"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressStreet_0" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressStreet_0"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressCity" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressCity"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressState_x" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressState_x"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressPostal_" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressPostal_"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelHomeAddressCountry" runat="server" /></td><td><asp:Label runat="server" ID="lblHomeAddressCountry"></asp:Label></td>
						</tr>		
			<tr>	
				 <td colspan="2" width="100%"><div class="listSubTitle">Work Address</div></td>
			</tr>
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressStreet_" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressStreet_"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressStreet_0" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressStreet_0"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressCity" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressCity"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressState_x" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressState_x"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressPostal_" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressPostal_"></asp:Label></td>
						</tr>		
						<tr>		
						 <td  width="165px"><asp:label id="labelWorkAddressCountry" runat="server" /></td><td><asp:Label runat="server" ID="lblWorkAddressCountry"></asp:Label></td>
						</tr>		
	
		<tr>
        <td width="165px">Attachments</td>
        <td>
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
                                <b><span>
																		
                                	
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
<tr height="60">
	<td align="right"  colspan="2">		
		<asp:Button ID="lbEdit" runat="server" Text="Edit" OnClick="b_Click"  CssClass="aspButtonClass" />
		<asp:Button ID="lbBack" runat="server" Text="Back" OnClientClick="javascript:history.back(); return false;"  CssClass="aspButtonClass"/>
	</td>
</tr> 	
</table>
					


		
