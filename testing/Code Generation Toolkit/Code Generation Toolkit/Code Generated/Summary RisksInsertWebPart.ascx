


<add script tag>

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

<add script end tag>

<style type="text/css">
    
    .text {
	 font-family: Ms Shell Dlg;
	 font-size: 13.3333px;
	}

</style>

<table>
<tr>		
	<td><asp:label id="textTITLE" runat="server" /></td><td><asp:TextBox ID="txtTITLE" runat="server" Width="386px" alt="TITLE" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="textDESCRIPTION" runat="server" /></td><td><asp:TextBox ID="txtDESCRIPTION" runat="server" Width="386px" alt="DESCRIPTION" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="textSORT_ORDER" runat="server" /><td><asp:TextBox ID="txtSORT_ORDER" runat="server" Width="386px" alt="SORT_ORDER" MaxLength="255"></asp:TextBox></td>
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
		<asp:LinkButton ID="lbOK" runat="server" Text="Submit" onclick="btnOK_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
		<asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" OnClick="lnCancel_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>
	</td>
</tr>
</table>
					


		
