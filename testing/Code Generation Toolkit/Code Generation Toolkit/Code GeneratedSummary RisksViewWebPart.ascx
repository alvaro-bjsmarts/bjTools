
<table>
		<tr>		
             <td  width="165px"><asp:label id="textTITLE" runat="server" /></td><td><asp:Label runat="server" ID="lblTITLE"></asp:Label></td>
	    </tr>		
		<tr>		
             <td  width="165px"><asp:label id="textDESCRIPTION" runat="server" /></td><td><asp:Label runat="server" ID="lblDESCRIPTION"></asp:Label></td>
	    </tr>		
		<tr>		
             <td  width="165px"><asp:label id="textSORT_ORDER" runat="server" /></td><td><asp:Label runat="server" ID="lblSORT_ORDER"></asp:Label></td>
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
																		
                                	<asp:LinkButton ID="lbdoc" runat="server" OnClientClick=<add server tag>"window.open('" + Eval("FilenameUrl")+ "')" <add server tag> Text= '<add server tag>Eval("Filename")<add server tag>'/>									
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
</table>
					


		
