<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="listino.aspx.vb" Inherits="crm_advise_it.listino1" MasterPageFile ="~/Site.Master"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    
    </asp:ScriptManager>
    <div>
    </div>    
    <div class="container">
        <h1 class="page-title">I Listini</h1>
			&nbsp;<asp:Panel id="PanelRicercaListino" runat="server" visible="false" groupingtext="Ricerca">
               <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtRicercaListino" runat="server" CssClass ="input-text" placeholder="Inserisci il listino"></asp:TextBox>
                            <asp:Button ID="btnRicercaListino" runat="server" Text="Ricerca" CssClass ="button black" />
                        </div> 
                </div> 
                <div class="profile-column right ">
                        <div class="field">    
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> 
            </asp:Panel>
            <br />
            <h1 class="page-title"></h1> 
            <section id="subaziende" class="content-section">
            <asp:Panel ID="PanelEliminaLis" runat="server" visible="false" groupingtext="Vuoi cancellare questo listino?">
                <div class="profile-column left">
                <br />
                        <div class="field">
                            <asp:Label ID="lblCodiceElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblListinoElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblCodEsternoElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                                <asp:Button ID="btnConferma" runat="server" CssClass="button" Text="Conferma" /><br />
                                <asp:Button ID="btnAnnullaCanc" runat="server" CssClass="button" Text="Annulla" /> 
                        </div>
                </div> 
                
            </asp:Panel> 
                <asp:Panel ID="Panel1" runat="server" visible="false" groupingtext="">
                    <h1 class="section-title"></h1>
                    <div class="csslistview ">
                                <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblCodice1" runat="server" Text="Codice"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblDescrizione1" runat="server" Text="Descrizione"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblCodEsterno1" runat="server" Text="Codice Esterno"></asp:Label>
                                                    </th>                                                    
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th7" runat="server" align="center">
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tr ID="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td class="a-left">
                                                    <asp:Label ID="lblCodice" runat="server" Text='<%# Eval("codice")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDescrizione" runat="server" Text='<%# Eval("descrizione")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCodEsterno" runat="server" Text='<%# Eval("codesterno")%>'></asp:Label>
                                                </td>
                                                <td class="dimtdim" >
                                                    <asp:ImageButton ID="imgMostra" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip="Mostra" AlternateText ='<%# eval("id") %>' CssClass ="dimim"  />
                                                </td>                                                
                                                <td class="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                <td class="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" 
                                    PageSize="100">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                        </div>
                    
                </asp:Panel>
            </section>
            <section id="profilo-view" class="content-section">
               
                <asp:Panel ID="PanelModificaListino" runat="server" visible="false">
                    <asp:Label ID="lblIdListino" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdTariffa" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblCodice" runat="server" CssClass="label " 
                                Text="Codice"></asp:Label>
                            <asp:TextBox ID="txtCodice" runat="server" class="input-text" name="codice" 
                                ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCodice" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>                      
                       
                       
                    </div>
                    <div class="profile-column right">
                        
                        <div class="field">
                            <asp:Label ID="lblCodEsterno" runat="server" CssClass="label " text="Codice Esterno"></asp:Label>
                            <asp:TextBox ID="txtCodEsterno" runat="server" class="input-text" name="codiceesterno" 
                                ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCodEsterno" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                         
                        
                    </div>
                     
                        
                    <div class="clear">
                    <div class="field">
                            <asp:Label ID="lblDescrizione" runat="server" CssClass="label " Text="Descrizione"></asp:Label>
                            <asp:TextBox ID="txtDescrizione" runat="server" class="input-text" name="descrizione" 
                                ReadOnly="true" type="text" TextMode ="MultiLine" width="80%" style="max-width:100%" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtDescrizione" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div style="vertical-align :top" >
                    <asp:Label ID="lblConferma" runat="server"  CssClass="order-status executed"  ></asp:Label></div>
                    <div>
                        <asp:Button ID="btnModifica" runat="server" CssClass="button" Text="Modifica" />
                        <asp:Button ID="btnSalva" runat="server" CssClass="button" Text="Salva" Visible="false" />
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="button" Text="Annulla" />
                    </div>
                </asp:Panel>
                    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
                    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
                    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" rel="stylesheet" type="text/css" />
                    <script type="text/javascript">
                        function ShowPopup(message) {
                            $(function () {
                                $("#dialog").html(message);
                                $("#dialog").dialog({
                                    title: "ApriUnTicket.it",
                                    buttons: {
                                        Close: function () {
                                            $(this).dialog('close');
                                        }
                                    },
                                    modal: true
                                });
                            });
                        };
                    </script>
   

                    

                    <div id="dialog" style="display: none">
                    </div>
                    
                    
                    <div>
                        
                    
                    
                    </div>



                    <asp:Panel ID="PanelTariffe" runat="server" Visible="false" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                    
                      <ContentTemplate >
                    
                    <div class ="barra" >Tariffe</div>
                    <div class="csslistview ">
                      
                        <asp:ListView ID="ListView2" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblTipoTariffa1" runat="server" Text="Tipo Tariffa"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblPrezzoUnitario1" runat="server" Text="Prezzo Unitario (&#8364;)"></asp:Label>
                                                    </th> 
                                                    <th ID="Th2" runat="server" align="center">
                                                        <asp:Label ID="lblDirittoChiamata1" runat="server" Text="Diritto Chiamata (&#8364;)"></asp:Label>
                                                    </th>      
                                                    <th ID="Th3" runat="server" align="center">
                                                        <asp:Label ID="lblPrezzoExtra1" runat="server" Text="Prezzo Extra (&#8364;)"></asp:Label>
                                                    </th> 
                                                    <th ID="Th4" runat="server" align="center">
                                                        <asp:Label ID="lblPercExtra1" runat="server" Text="Percentuale Extra (%)"></asp:Label>
                                                    </th> 
                                                    <th ID="Th5" runat="server" align="center">
                                                        <asp:Label ID="lblMisura1" runat="server" Text="Misura"></asp:Label>
                                                    </th> 
                                                    <th ID="Th6" runat="server" align="center">
                                                        <asp:Label ID="lblCosto1" runat="server" Text="Costo (&#8364;)"></asp:Label>
                                                    </th>                                               
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tr ID="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tbody>
                                            <tr>
                                                <td class="a-left">
                                                    <asp:Label ID="lblTipoTariffa" runat="server" Text='<%# Eval("tariffazione")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPrezzoUnitario" runat="server" Text='<%# Eval("prezzounitario")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblDirittoChiamata" runat="server" Text='<%# Eval("dirittochiamata")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblPrezzoExtra" runat="server" Text='<%# Eval("prezzoextra")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblPercExtra" runat="server" Text='<%# Eval("percextra")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblMisura" runat="server" Text='<%# Eval("misura")%>' Width ="100%"></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblCosto" runat="server" Text='<%# Eval("costo")%>'></asp:Label>
                                                </td>                                                
                                                <td class ="dimtdim" >
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                         
                                                      
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager2" runat="server" PagedControlID="ListView2" 
                                    PageSize="5">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                      
                    </div>
                    
                    <hr />
                    <div class="profile-column left ">
                         <div class="field">                         
                             <asp:Label ID="lblSicuro" runat="server" Text="Eliminare il contatto " Visible ="false"></asp:Label><%If lblSicuro.Visible = True Then%><% =ddlTariffazione.SelectedItem%><%End If%>
                             &nbsp;<asp:Button ID="btnElimina" runat="server" Text="Elimina" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                              &nbsp;<asp:Button ID="btnAnnullaTariffa" runat="server" Text="Annulla" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                         </div> 
                     </div>  
                    <div class="profile-column inter">
                        <div class="field">                            
                            <asp:DropDownList ID="ddlTariffazione" runat="server" class="input-droplist" style="width:13%; margin-bottom:14px; height:27px"></asp:DropDownList>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="-1" ControlToValidate="ddlTariffazione" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtPrezzoUnitario" runat="server" class="input-textlist" name="prezzounitario" type="text" style="width:13%;" placeholder="prezzo unitario" />
                            <asp:TextBox ID="txtDirittoChiamata" runat="server" class="input-textlist" name="dirittochiamata" type="text" style="width:13%;"  placeholder="costo diritto di chiamata" />
                            <asp:TextBox ID="txtPrezzoExtra" runat="server" class="input-textlist" name="prezzoextra" type="text" style="width:13%;" placeholder="prezzo extra" />
                            <asp:TextBox ID="txtPercExtra" runat="server" class="input-textlist" name="percextra" type="text" style="width:13%;" placeholder="percentuale extra" />
                            <asp:DropDownList ID="ddlMisura" runat="server" class="input-droplist" style="width:13%; margin-bottom:14px; height:27px"></asp:DropDownList>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue="-1" ControlToValidate="ddlMisura" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtCosto" runat="server" class="input-textlist" name="costo" type="text" style="width:13%;" placeholder="costo" />
                        </div>  
                    
                        <div class="field">
                        &nbsp;<asp:Button ID="btnMemorizza" runat="server" Text="Memorizza" Cssclass="button black" Width="30%"></asp:Button>
                             
                            &nbsp;<asp:Button ID="btnAnnulla1" runat="server" Text="Annulla" Cssclass="button black" Width="30%"></asp:Button>
                        </div>
                    </div>
                        
                    </ContentTemplate>                    
                  </asp:UpdatePanel>
                    </asp:Panel>
                   
                
                
            </section>
            
      




    </div>
   

  
</asp:Content>
