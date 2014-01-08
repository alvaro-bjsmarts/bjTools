
<script type = "text/javascript">
    var counter = 0;
    function AddFileUpload() 
	{
		var div = document.createElement('DIV');	    
        div.innerHTML = '<input id="file' + counter + '" name = "file' + counter + '" type="file" />' + '<input id="Button' + counter + '" type="button" ' + 'value="Remove" onclick = "RemoveFileUpload(this)" />';
        document.getElementById("InsertFileUploadContainer").appendChild(div);		
        counter++;
	}
    function RemoveFileUpload(div) 
	{
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
	<td><asp:label id="labelTitle" runat="server" /></td><td><asp:TextBox ID="txtTitle" runat="server" Width="386px" alt="Title" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelEmployeeId" runat="server" /></td><td><asp:TextBox ID="txtEmployeeId" runat="server" Width="386px" alt="EmployeeId" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelFirstName" runat="server" /></td><td><asp:TextBox ID="txtFirstName" runat="server" Width="386px" alt="First_x0020_Name" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelLastName" runat="server" /></td><td><asp:TextBox ID="txtLastName" runat="server" Width="386px" alt="Last_x0020_Name" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelEmail" runat="server" /></td><td><asp:TextBox ID="txtEmail" runat="server" Width="386px" alt="Email" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomePhone" runat="server" /></td><td><asp:TextBox ID="txtHomePhone" runat="server" Width="386px" alt="Home_x0020_Phone" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkPhone" runat="server" /></td><td><asp:TextBox ID="txtWorkPhone" runat="server" Width="386px" alt="Work_x0020_Phone" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelMovilPhone" runat="server" /></td><td><asp:TextBox ID="txtMovilPhone" runat="server" Width="386px" alt="Movil_x0020_Phone" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHireDate" runat="server" /></td><td><spuc:DateTimeControl runat="server" ID="HireDate" DateOnly="true" /></td>
</tr>								
<tr>		
	<td><asp:label id="labelBirthDate" runat="server" /></td><td><spuc:DateTimeControl runat="server" ID="BirthDate" DateOnly="true" /></td>
</tr>								
<tr>		
	<td><asp:label id="labelSalary" runat="server" /></td><td><asp:TextBox ID="txtSalary" runat="server" Width="386px" alt="Salary" MaxLength="255"></asp:TextBox></td>
</tr>
<tr>		
	<td><asp:label id="labelEmployeeGovernmentId" runat="server" /></td><td><asp:TextBox ID="txtEmployeeGovernmentId" runat="server" Width="386px" alt="EmployeeGovernmentId" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelBiometricEmployeeId" runat="server" /></td><td><asp:TextBox ID="txtBiometricEmployeeId" runat="server" Width="386px" alt="BiometricEmployeeId" MaxLength="255"></asp:TextBox></td>
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
	<td><asp:label id="labelHomeAddressStreet_" runat="server" /></td><td><asp:TextBox ID="txtHomeAddressStreet_" runat="server" Width="386px" alt="Home_x0020_Address_x0020_Street_" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeAddressStreet_0" runat="server" /></td><td><asp:TextBox ID="txtHomeAddressStreet_0" runat="server" Width="386px" alt="Home_x0020_Address_x0020_Street_0" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeAddressCity" runat="server" /></td><td><asp:TextBox ID="txtHomeAddressCity" runat="server" Width="386px" alt="Home_x0020_Address_x0020_City" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeAddressState_x" runat="server" /></td><td><asp:TextBox ID="txtHomeAddressState_x" runat="server" Width="386px" alt="Home_x0020_Address_x0020_State_x" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelHomeAddressPostal_" runat="server" /></td><td><asp:TextBox ID="txtHomeAddressPostal_" runat="server" Width="386px" alt="Home_x0020_Address_x0020_Postal_" MaxLength="255"></asp:TextBox></td>
</tr>				 
	<tr>
		<td><asp:label id="labelHomeAddressCountry" runat="server" /></td><td><asp:DropDownList ID="ddlHomeAddressCountry" runat="server"></asp:DropDownList></td>
	</tr>
<tr>		
	<td><asp:label id="labelWorkAddressStreet_" runat="server" /></td><td><asp:TextBox ID="txtWorkAddressStreet_" runat="server" Width="386px" alt="Work_x0020_Address_x0020_Street_" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkAddressStreet_0" runat="server" /></td><td><asp:TextBox ID="txtWorkAddressStreet_0" runat="server" Width="386px" alt="Work_x0020_Address_x0020_Street_0" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkAddressCity" runat="server" /></td><td><asp:TextBox ID="txtWorkAddressCity" runat="server" Width="386px" alt="Work_x0020_Address_x0020_City" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkAddressState_x" runat="server" /></td><td><asp:TextBox ID="txtWorkAddressState_x" runat="server" Width="386px" alt="Work_x0020_Address_x0020_State_x" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="labelWorkAddressPostal_" runat="server" /></td><td><asp:TextBox ID="txtWorkAddressPostal_" runat="server" Width="386px" alt="Work_x0020_Address_x0020_Postal_" MaxLength="255"></asp:TextBox></td>
</tr>				 
	<tr>
		<td><asp:label id="labelWorkAddressCountry" runat="server" /></td><td><asp:DropDownList ID="ddlWorkAddressCountry" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelDepartment" runat="server" /></td><td><asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td><asp:label id="labelOrganization" runat="server" /></td><td><asp:DropDownList ID="ddlOrganization" runat="server"></asp:DropDownList></td>
	</tr>
<tr>		
	<td><asp:label id="labelDeleted" runat="server" /></td><td><asp:DropDownList ID="ddlDeleted" runat="server"></asp:DropDownList></td>
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
					


		
