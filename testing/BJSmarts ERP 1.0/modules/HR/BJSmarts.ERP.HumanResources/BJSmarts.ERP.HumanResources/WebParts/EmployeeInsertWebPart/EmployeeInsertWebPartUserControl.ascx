<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeInsertWebPartUserControl.ascx.cs" Inherits="BJSmarts.ERP.HumanResources.WebParts.EmployeeInsertWebPart.EmployeeInsertWebPartUserControl" %>
<%@ Register TagPrefix="spuc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"  %>


<script type = "text/javascript">
    var counter = 0;
    function AddFileUpload() {
        var div = document.createElement('DIV');
        div.innerHTML = '<input id="file' + counter + '" name = "file' + counter + '" type="file" />' + '<input id="Button' + counter + '" type="button" ' + 'value="Remove" onclick = "RemoveFileUpload(this)" />';
        document.getElementById("InsertFileUploadContainer").appendChild(div);
        counter++;
    }
    function RemoveFileUpload(div) {
        document.getElementById("InsertFileUploadContainer").removeChild(div.parentNode);
    }

</script>
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

<table  width="100%">

<tr>	
<td colspan="2" width="100%"><div class="listSubTitle">Group 1</div></td>
</tr>
<tr>		
	<td><asp:label id="labelFirstName" runat="server" /></td><td><asp:TextBox ID="txtFirstName" runat="server" Width="386px" alt="First_x0020_Name" MaxLength="255"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidatorFirstName" runat="server" Display="Dynamic"  ErrorMessage="*" ControlToValidate="txtFirstName"  ForeColor="Red"></asp:RequiredFieldValidator></td>
</tr>
<tr>		
	<td><asp:label id="labelLastName" runat="server" /></td><td><asp:TextBox ID="txtLastName" runat="server" Width="386px" alt="Last_x0020_Name" MaxLength="255"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidatorLastName" runat="server" Display="Dynamic"  ErrorMessage="*" ControlToValidate="txtLastName"  ForeColor="Red"></asp:RequiredFieldValidator></td>
</tr>
<tr>		
	<td><asp:label id="labelEmail" runat="server" /></td><td><asp:TextBox ID="txtEmail" runat="server" Width="386px" alt="Email" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomePhoneNumber" runat="server" /></td><td><asp:TextBox ID="txtHomePhoneNumber" runat="server" Width="386px" alt="Home_x0020_Phone_x0020_Number" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkPhoneNumber" runat="server" /></td><td><asp:TextBox ID="txtWorkPhoneNumber" runat="server" Width="386px" alt="Work_x0020_Phone_x0020_Number" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelMovilPhoneNumber" runat="server" /></td><td><asp:TextBox ID="txtMovilPhoneNumber" runat="server" Width="386px" alt="Movil_x0020_Phone_x0020_Number" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelDateOfBirth" runat="server" /></td><td><spuc:DateTimeControl runat="server" ID="DateOfBirth" DateOnly="true" /></td>
</tr>								
<tr>		
	<td><asp:label id="labelHireDate" runat="server" /></td><td><spuc:DateTimeControl runat="server" ID="HireDate" DateOnly="true" /></td>
</tr>								
<tr>		
	<td><asp:label id="labelSalary" runat="server" /></td><td><asp:TextBox ID="txtSalary" runat="server" Width="386px" alt="Salary" MaxLength="255"></asp:TextBox></td>
</tr>
<tr>		
	<td><asp:label id="labelEmployeeGovernmentId" runat="server" /></td><td><asp:TextBox ID="txtEmployeeGovernmentId" runat="server" Width="386px" alt="EmployeeGovernmentId" MaxLength="255"></asp:TextBox><asp:RequiredFieldValidator id="RequiredFieldValidatorEmployeeGovernmentId" runat="server" Display="Dynamic"  ErrorMessage="*" ControlToValidate="txtEmployeeGovernmentId"  ForeColor="Red"></asp:RequiredFieldValidator></td>
</tr>
<tr>		
	<td><asp:label id="labelBiometricEmployeeId" runat="server" /></td><td><asp:TextBox ID="txtBiometricEmployeeId" runat="server" Width="386px" alt="BiometricEmployeeId" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>	
<td colspan="2" width="100%"><div class="listSubTitle">Group 2</div></td>
</tr>
	<tr>
		<td><asp:label id="labelEmployeeType" runat="server" /></td><td><asp:DropDownList ID="ddlEmployeeType" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelResourceType" runat="server" /></td><td><asp:DropDownList ID="ddlResourceType" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelGender" runat="server" /></td><td><asp:DropDownList ID="ddlGender" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelCurrency" runat="server" /></td><td><asp:DropDownList ID="ddlCurrency" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelMaritalStatus" runat="server" /></td><td><asp:DropDownList ID="ddlMaritalStatus" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelEmployeeTermination_x0020" runat="server" /></td><td><asp:DropDownList ID="ddlEmployeeTermination_x0020" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelDepartment" runat="server" /></td><td><asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList></td>
	</tr>
<tr>	
<td colspan="2" width="100%"><div class="listSubTitle">Group 3</div></td>
</tr>
<tr>		
	<td><asp:label id="labelHomeStreetAddress_" runat="server" /></td><td><asp:TextBox ID="txtHomeStreetAddress_" runat="server" Width="386px" alt="Home_x0020_Street_x0020_Address_" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeStreetAddress_0" runat="server" /></td><td><asp:TextBox ID="txtHomeStreetAddress_0" runat="server" Width="386px" alt="Home_x0020_Street_x0020_Address_0" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomePostalCode" runat="server" /></td><td><asp:TextBox ID="txtHomePostalCode" runat="server" Width="386px" alt="Home_x0020_Postal_x0020_Code" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeCity" runat="server" /></td><td><asp:TextBox ID="txtHomeCity" runat="server" Width="386px" alt="Home_x0020_City" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeStateProvince" runat="server" /></td><td><asp:TextBox ID="txtHomeStateProvince" runat="server" Width="386px" alt="Home_x0020_State_x0020_Province" MaxLength="255"></asp:TextBox></td>
</tr>				 
	<tr>
		<td><asp:label id="labelHomeCountry" runat="server" /></td><td><asp:DropDownList ID="ddlHomeCountry" runat="server"></asp:DropDownList></td>
	</tr>
<tr>	
<td colspan="2" width="100%"><div class="listSubTitle">Group 4</div></td>
</tr>
<tr>		
	<td><asp:label id="labelWorkStreetAddress_" runat="server" /></td><td><asp:TextBox ID="txtWorkStreetAddress_" runat="server" Width="386px" alt="Work_x0020_Street_x0020_Address_" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkStreetAddress_0" runat="server" /></td><td><asp:TextBox ID="txtWorkStreetAddress_0" runat="server" Width="386px" alt="Work_x0020_Street_x0020_Address_0" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkPostalCode" runat="server" /></td><td><asp:TextBox ID="txtWorkPostalCode" runat="server" Width="386px" alt="Work_x0020_Postal_x0020_Code" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkCity" runat="server" /></td><td><asp:TextBox ID="txtWorkCity" runat="server" Width="386px" alt="Work_x0020_City" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkStateProvince" runat="server" /></td><td><asp:TextBox ID="txtWorkStateProvince" runat="server" Width="386px" alt="Work_x0020_State_x0020_Province" MaxLength="255"></asp:TextBox></td>
</tr>				 
	<tr>
		<td><asp:label id="labelWorkCountry" runat="server" /></td><td><asp:DropDownList ID="ddlWorkCountry" runat="server"></asp:DropDownList></td>
	</tr>
<tr class="altercolortable">
    <td class="font">Attachments</td>
    <td colspan="3">
        <span>Click to add files</span>&nbsp;&nbsp;
        <input id="Button2" type="button" value="Add Attachment" onclick = "AddFileUpload()" />
        <br /><br />
    <div id = "InsertFileUploadContainer">
        
    </div>
    </td>
</tr>
<tr height="60">
	<td align="right"  colspan="2">				
		<asp:Button ID="lbOK" runat="server" Text="Submit" Onclick="btnOK_Click" CssClass="aspButtonClass" />
		<asp:Button ID="lbCancel" runat="server" Text="Cancel" OnClientClick="javascript:history.back(); return false;"  CssClass="aspButtonClass"/>
	</td>
</tr>

</table>