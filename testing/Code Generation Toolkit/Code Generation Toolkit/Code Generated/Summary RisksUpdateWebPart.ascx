
<add script tag>

 function DeleteItem(file)
    {
          var product = file;		  
          <add server tag> Page.ClientScript.GetCallbackEventReference(this, "product", "ReceiveServerData",null)<add server tag>;
    }

    function ReceiveServerData(rValue)
    {
         document.getElementById('datalist').innerHTML = rValue;
    }

    var counter = 0;

    function AddFileUpload() {

        var div = document.createElement('DIV');

        div.innerHTML = '<input id="file' + counter + '" name = "file' + counter +'" type="file" />' +'<input id="Button' + counter + '" type="button" ' + 'value="Remove" onclick = "RemoveFileUpload(this)" />';

        document.getElementById("UpdateFileUploadContainer").appendChild(div);

        counter++;
    }

    function RemoveFileUpload(div) {

        document.getElementById("UpdateFileUploadContainer").removeChild(div.parentNode);

    }

<add script end tag>

<table>
<tr>		
	<td><asp:label id="textTITLE" runat="server" /></td><td><asp:TextBox ID="txtTITLE" runat="server" Width="386px" alt="TITLE" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="textDESCRIPTION" runat="server" /></td><td><asp:TextBox ID="txtDESCRIPTION" runat="server" Width="386px" alt="DESCRIPTION" MaxLength="255"></asp:TextBox></td>
</tr>				 
<tr>		
	<td><asp:label id="textSORT_ORDER" runat="server" /></td><td><asp:TextBox ID="txtSORT_ORDER" runat="server" Width="386px" alt="SORT_ORDER" MaxLength="255"></asp:TextBox></td>
</tr>
<tr>
    <td class="font" width="200px">Attachments</td>
    <td>
    <div id="datalist">    
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
                                    <asp:LinkButton ID="lbdoc" runat="server" OnClientClick=<add server tag>"window.open('" + Eval("FilenameUrl")+ "')"<add server tag> Text='<add server tag>Eval("Filename")<add server tag>' />
                            </span></b>
                        </td>
                        <td>&nbsp;&nbsp;<img src="/_layouts/images/rect.gif" alt="Delete">&nbsp;<asp:LinkButton ID="lbdelete" runat="server" OnClientClick= <add server tag>"DeleteItem('" + Eval("Filename") + "'); return false;" <add server tag>>Delete</asp:LinkButton></td>
                    </tr>            
            </ItemTemplate>
            <FooterTemplate>  
                </table>  
            </FooterTemplate> 
        </asp:DataList>

    </div>
    </td>
</tr>
<tr>
    <td width="200px"></td>
    <td>
        <span style ="font-family:Arial">Click to add files</span>&nbsp;&nbsp;
        <input id="Button2" type="button" value="add" onclick = "AddFileUpload()" />
        <br /><br />
        <div id = "UpdateFileUploadContainer">
            
        </div>
    </td>
</tr>
<tr height="60">
	<td align="right"  colspan="2">				
		<asp:LinkButton ID="lbOK" runat="server" Text="Save" onclick="btnOK_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;" />
		<asp:LinkButton ID="lbCancel" runat="server" Text="Cancel" OnClick="lnCancel_Click" style="margin:20px;padding:6px 24px; background:#ededed; font-family:arial;font-size:15px;font-weight:bold; border:1px solid #dcdcdc; color:#777777;text-decoration:none;text-shadow:1px 1px 0px #ffffff;"/>
	</td>
</tr>
</table>
					


		
