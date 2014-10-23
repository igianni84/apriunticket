<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="inventario.aspx.vb" Inherits="crm_advise_it.Inventario" MasterPageFile ="~/Site.Master"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization ="true"></asp:ScriptManager>
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


    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
    <ContentTemplate>
    <div>
    
     
    </div>
    
    <div class="container">
        <h1 class="page-title">Gli Inventari</h1>
			&nbsp;<asp:Panel id="PanelRicercaInventario" runat="server" visible="false" groupingtext="Ricerca" CssClass ="panelRicerca " >
                <div style="float:center ">
                <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtCodiceRicerca" runat="server" CssClass ="input-text" placeholder="Inserisci il codice"></asp:TextBox>
                            <asp:TextBox ID="txtDescrizioneRicerca" runat="server" CssClass ="input-text" placeholder="Inserisci la descrizione"></asp:TextBox>
                            <asp:TextBox ID="txtSerialeRicerca" runat="server" CssClass ="input-text" placeholder="Inserisci il seriale"></asp:TextBox>
                            <asp:TextBox ID="txtTipoRicerca" runat="server" CssClass ="input-text" placeholder="Inserisci il tipo"></asp:TextBox>
                            
                        </div> 
                </div> 
                <div class="profile-column right ">
                        <div class="field">
                            <asp:Button ID="btnRicercaInventario" runat="server" Text="Ricerca" CssClass ="button black" /> <br />   
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> 
            </div> 
            </asp:Panel>
            <h1 class="page-title"></h1> 
            <section id="inventario" class="content-section">
                <asp:Panel ID="PanelEliminaInv" runat="server" visible="false" groupingtext="Vuoi cancellare questo inventario?" CssClass ="panelRicerca" >
                <div class="profile-column left">
                <br />
                        <div class="field">
                            <asp:Label ID="lblCodiceElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblSerialeElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblClienteElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblSubClienteElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                                <asp:Button ID="btnConferma" runat="server" CssClass="button" Text="Conferma" /><br />
                                <asp:Button ID="btnAnnullaCanc" runat="server" CssClass="button" Text="Annulla" /> 
                        </div>
                </div> 
                
            </asp:Panel> 
            
                <asp:Panel ID="Panel1" runat="server" groupingtext="" CssClass ="panelLista " >
                    <h1 class="section-title"></h1>
                    <div class ="csslistview ">
                                <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Th4" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblCodice" runat="server" Text="Codice"></asp:Label>
                                                    </th>
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblCliente" runat="server" Text="Cliente"></asp:Label>
                                                    </th>
                                                    <th ID="Th7" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblSubCliente" runat="server" Text="SubCliente"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblSeriale" runat="server" Text="Seriale"></asp:Label>
                                                    </th>
                                                    <th ID="Th2" runat="server" align="center">
                                                        <asp:Label ID="lblTipo" runat="server" Text="Tipo" ></asp:Label>
                                                    </th>
                                                    <th ID="Th3" runat="server" align="center">
                                                        <asp:Label ID="lblMarchio" runat="server" Text="Marchio"></asp:Label>
                                                    </th> 
                                                    <th ID="Th6" runat="server" align="center">
                                                        <asp:Label ID="lblModello" runat="server" Text="Modello"></asp:Label>
                                                    </th>                                                    
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>                                                    
                                                    <th ID="Th8" runat="server" align="center">
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
                                                <td>
                                                    <asp:Label ID="lblCodice" runat="server" Text='<%# Eval("codice")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("ragsoccli")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblSubCliente" runat="server" Text='<%# Eval("ragsocsubcli")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDescrizione" runat="server" Text='<%# Eval("descrizione")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSeriale" runat="server" Text='<%# Eval("seriale")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("tipodispositivo")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMarchio" runat="server" Text='<%# Eval("marchio")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblModello" runat="server" Text='<%# Eval("modello")%>'></asp:Label>
                                                </td> 
                                               <%If Session("tipoutente") <> "Utente" Then%>                                               
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" tooltip="Modifica" alternatetext='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip ="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                <%Else%>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgDettagli" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ="Dettagli" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                <%End If%>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgApriTicket" runat="server" 
                                                        ImageUrl="~/images/new_ticket.png" ToolTip ="Apri Ticket" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
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
            <asp:Panel ID="PanelModificaInventario" runat="server" visible="false" CssClass ="panelLista ">
                
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode ="Conditional" >
                 <ContentTemplate >
                    <asp:Label ID="lblIdInventario" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdCredenziali" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblCodice" runat="server" CssClass="label " 
                                Text="Codice" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtCodice" runat="server" class="input-text" 
                                name="codice" ReadOnly="true" type="text" Visible="false"  />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCodice" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCliente" runat="server" CssClass="label " 
                                Text="Cliente"></asp:Label>
                           <asp:DropDownList ID="ddlCliente" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" 
                                AutoPostBack="True"></asp:DropDownList> 
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue="-1" ControlToValidate="ddlCliente" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>                           
                        </div>
                                                  
                            
                         <div class="field">
                                <asp:Label ID="lblSubCliente" runat="server" CssClass="label " text="SubCliente"></asp:Label>
                                <asp:DropDownList ID="ddlSubCliente" runat="server" class="input-drop" 
                                    style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" 
                                    ></asp:DropDownList> 
                            </div>
                        
                        <div class="field">
                            <asp:Label ID="lblFornitoreOrg" runat="server" CssClass="label " 
                                Text="Fornitore Org" ></asp:Label>
                           <asp:DropDownList ID="ddlFornitoreOrg" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" 
                                AutoPostBack="True"></asp:DropDownList> 
                                                      
                        </div>
                        </div>
                        <div class="profile-column right">
                            <div >                                                  
                                    <asp:CheckBox ID="cbxMobile" runat="server" CssClass ="checkbox "  
                                        Text="OGGETTO MOBILE" AutoPostBack="True" ></asp:CheckBox>                            
                             </div>      
                            <br />  
                            <%--<asp:Label ID="Label2" runat="server" CssClass="label " text="app" visible="false"></asp:Label>
                                <asp:DropDownList ID="DropDownList1" runat="server" class="input-drop" 
                                    style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True"  visible="false"
                                    ></asp:DropDownList>--%> 
                            
                            <%--<div class ="field" style="height :30px"></div>--%> 
                            <div class="field">
                                <asp:Label ID="lblUtente" runat="server" CssClass="label " Text="Utente"></asp:Label>
                                <asp:DropDownList ID="ddlUtente" runat="server" class="input-drop" 
                                    style="width:220px;margin-bottom:14px;height:27px" 
                                    ></asp:DropDownList>
                            
                            </div>
                            <div class="field">
                            <asp:Label ID="lblFornitoreCli" runat="server" CssClass="label " 
                                Text="Fornitore Cliente"></asp:Label>
                           <asp:DropDownList ID="ddlFornitoreCli" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" 
                                AutoPostBack="True"></asp:DropDownList> 
                                                      
                        </div>
                        </div> 
                        <div class="clear ></div>
                        <div class="field">
                            <asp:Label ID="lblDescrizione" runat="server" CssClass="label " text="Descrizione"></asp:Label>
                            <asp:TextBox ID="txtDescrizione" runat="server" class="input-text" name="descrizione" 
                                ReadOnly="true" type="text" TextMode ="MultiLine" Width ="30%" />                            
                        </div>
                        <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblUbicazione" runat="server" CssClass="label " Text="Ubicazione"></asp:Label>
                            <asp:TextBox ID="txtUbicazione" runat="server" class="input-text" name="ubicazione" 
                                ReadOnly="true" type="text" />    
                        </div>
                        
                        <div class="field">
                            <asp:Label ID="lblDispositivo" runat="server" CssClass="label " Text="Dispositivo"></asp:Label>
                            <asp:DropDownList ID="ddlDispositivo" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" 
                                ></asp:DropDownList>  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="-1" ControlToValidate="ddlDispositivo" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>                          
                        </div>
                         <div class="field">
                            <asp:Label ID="lblMarchio" runat="server" CssClass="label " Text="Marchio"></asp:Label>
                            <asp:DropDownList ID="ddlMarchio" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" 
                                ></asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="-1" ControlToValidate="ddlMarchio" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>--%>                            
                        </div>
                        <div class="field">
                            <asp:Label ID="lblModello" runat="server" CssClass="label " Text="Modello"></asp:Label>
                            <asp:DropDownList ID="ddlModello" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px" 
                                ></asp:DropDownList>   
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" InitialValue="-1" ControlToValidate="ddlModello" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>--%>                         
                        </div>
                        <div class="field">
                            <asp:Label ID="lblNote" runat="server" CssClass="label " text="Note"></asp:Label>
                            <asp:TextBox ID="txtNote" runat="server" class="input-text" name="note" 
                                type="text" TextMode ="MultiLine" Width="65%"   />                            
                        </div>
                        
                       <%-- <div class="field"> 
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="label " Visible ="false"></asp:FileUpload>
                            <asp:Button ID="btnCarica" runat="server" Text="Carica" CssClass ="button " visible ="false"></asp:Button>
                            <asp:TextBox ID="txtLogo" runat="server" class="input-text" name="logo" visible="false" type="text" CssClass="label "/>
                                                    
                        </div>--%>
                    </div>
                    <div class="profile-column right">                        
                         <div class="field">
                            <asp:Label ID="lblSeriale" runat="server" CssClass="label " Text="Seriale"></asp:Label>
                            <asp:TextBox ID="txtSeriale" runat="server" class="input-text" name="seriale" 
                                ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSeriale" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div> 
                        <div class="field">
                            <asp:Label ID="lblIp" runat="server" CssClass="label " Text="IP"></asp:Label>
                            <asp:TextBox ID="txtIp" runat="server" class="input-text" name="ip" 
                                ReadOnly="true" type="text" />    
                        </div>  
                       
                         <div class="field">
                            <asp:Label ID="lblSubnet" runat="server" CssClass="label " Text="SubNet"></asp:Label>
                            <asp:TextBox ID="txtSubNet" runat="server" class="input-text" name="subnet" 
                                ReadOnly="true" type="text" />    
                        </div>   
                         <div class="field">
                            <asp:Label ID="lblGateway" runat="server" CssClass="label " Text="Gateway"></asp:Label>
                            <asp:TextBox ID="txtGateway" runat="server" class="input-text" name="gateway" 
                                ReadOnly="true" type="text" />    
                        </div>  
                        
                        
                        <div class="field ">
                            <asp:Label ID="lbldataSca" runat="server" CssClass="label " Text="Data Scadenza"></asp:Label>
                            <asp:TextBox ID="txtDataSca" runat="server" class="input-text" name="datasca" 
                                type="text" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtDataSca" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator> --%>
                            <cc1:CalendarExtender ID="CalendarExtender" runat="server" Format="dd/MM/yyyy"
                             TargetControlID="txtDataSca" PopupButtonID="txtDataSca" />   
                        </div>                       
                        
                    </div>
                    
                    <div class="clear">

                    </div>
                    
                    
                    <div style="vertical-align :top" ><%--<asp:Label ID="lblConferma" runat="server"  CssClass="order-status confirmed "  style="background-image:none " visible="false"  ></asp:Label>--%>
                    <asp:Label ID="lblConferma" runat="server"  CssClass="order-status executed"  ></asp:Label></div>
                    <div>
                        <asp:Button ID="btnModifica" runat="server" CssClass="button" Text="Modifica" />
                        <asp:Button ID="btnSalva" runat="server" CssClass="button" Text="Salva" Visible="false" />
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="button" Text="Annulla" />
                    </div>
                    </ContentTemplate> 
                </asp:UpdatePanel>
                </asp:Panel>
                    
                    
                    
                    <div>
                        
                    
                    
                    </div>



                    
                    <asp:Panel ID="PanelBarra" runat="server" >
                    <hr />
                        <div class ="barra" ><asp:LinkButton ID="lbCredenziali" runat="server" Text ="Credenziali" ForeColor ="White" Visible="false"  ></asp:LinkButton></div>
                    </asp:Panel> 
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Always"  >
                    
                      <ContentTemplate >--%>
                     <asp:Panel ID="PanelCredenziali" runat="server" CssClass ="panelCredenziali" visible="false">
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                 <ContentTemplate >
                    <div class="csslistview " style ="width:50%">
                      
                        <asp:ListView ID="ListView2" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblDescrizioneCre" runat="server" Text="Descrizione"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblUtenteCre" runat="server" Text="Utente"></asp:Label>
                                                    </th>     
                                                     <th ID="Th5" runat="server" align="center">
                                                        <asp:Label ID="lblPassCre" runat="server" Text="Password"></asp:Label>
                                                    </th>                                                    
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th9" runat="server" align="center">
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
                                                    <asp:Label ID="lblDescrizioneCre" runat="server" Text='<%# Eval("descrizione")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUtenteCre" runat="server" Text='<%# Eval("utente")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblPassCre" runat="server" Text='********'></asp:Label>
                                                </td>                                                
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip ="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip ="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgVedi" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ='<%# eval("pass") %>' AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                         
                                                      <asp:Panel ID="Panel166" runat="server" style="display:none; background-color:#ffffff; width:150px; height:50px; border:1">
                                                      <div style="border:1; width:150px; vertical-align:middle; text-align:center">
                                                      <asp:Label ID="Label1" runat="server" Text='<%# eval("pass") %>'></asp:Label>                                                         
                                                        </div>
                                                     </asp:Panel>
                                                     <cc1:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="imgVedi" 
                                                    PopupControlID="Panel166" 
                                                    Position="Bottom"                                                     
                                                    CommitProperty="value" 
                                                    OffsetX="40"
                                                    Offsety="10"> 
                                                     </cc1:PopupControlExtender>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager2" runat="server" PagedControlID="ListView2" 
                                    PageSize="100">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                      
                    </div>
                    
                    <hr />
                    <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtDescrizioneCre" runat="server" class="input-text" name="descrizionecre" type="text" placeholder="inserisci la nuova descrizione" />
                            &nbsp;<asp:Button ID="btnMemorizza" runat="server" Text="Memorizza" Cssclass="button black" Width="30%"></asp:Button>
                        </div>  
                    
                        <div class="field">
                             <asp:TextBox ID="txtUtenteCre" runat="server" class="input-text" name="utentecre" type="text" placeholder="inserisci l'username" />
                            &nbsp;<asp:Button ID="btnAnnulla1" runat="server" Text="Annulla" Cssclass="button black" Width="30%"></asp:Button>
                        </div>
                        <div class="field">
                             <asp:TextBox ID="txtPassCre" runat="server" class="input-text" name="passcre" type="text" placeholder="inserisci la password" />
                            
                        </div>
                    </div>
                     <div class="profile-column right ">
                         <div class="field">                         
                             <asp:Label ID="lblSicuro" runat="server" Text="Eliminare il contatto " Visible ="false"></asp:Label>
                             &nbsp;<asp:Button ID="btnElimina" runat="server" Text="Elimina" Cssclass="button black" Width="30%" Visible ="false" ></asp:Button>
                         </div> 
                     </div>     
                    </ContentTemplate>
                    
                  </asp:UpdatePanel>
                    </asp:Panel> 
                   
                
                
            </section>
            
      




    </div>
    </ContentTemplate>
      </asp:UpdatePanel>

  
</asp:Content>