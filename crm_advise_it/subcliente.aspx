<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="subcliente.aspx.vb" Inherits="crm_advise_it.subcliente1" MasterPageFile ="~/Site.Master" MaintainScrollPositionOnPostBack="True"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
    <ContentTemplate>
    <div>
    
     <%--<% If Request.Url.AbsolutePath = "/anagrafica.aspx" Then%>
        <asp:Menu ID="Menu2" runat="server" CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal">
                    <Items>
                        <asp:MenuItem Text="Aziende" >                   		
                            <asp:MenuItem NavigateUrl="~/anagrafica.aspx" Text="Organizzazioni" />
                            <asp:MenuItem NavigateUrl="~/suborganizzazioni.aspx" Text="Sub - Organizzazioni"/>
                        </asp:MenuItem> 
                        <asp:MenuItem Text="Clienti" >
                            <asp:MenuItem NavigateUrl="~/clienti.aspx" Text="Clienti"/>
                            <asp:MenuItem NavigateUrl="~/subclienti.aspx" Text="Sub - Clienti"/>
                         </asp:MenuItem>
                         <asp:MenuItem NavigateUrl="~/utenti.aspx" Text="Utenti"/>
                         <asp:MenuItem Text="Fornitori" >
                            <asp:MenuItem NavigateUrl="~/fornclienti.aspx" Text="Fornitori Clienti"/>
                            <asp:MenuItem NavigateUrl="~/fornorg.aspx" Text="Fornitori Organizzazioni"/>
                         </asp:MenuItem>                                      
                    </Items>
                </asp:Menu>
        <%End If%> --%>
        

        <%--<asp:Button ID="btnOrganizzazione" runat="server" Text="Organizzazione" CssClass ="button black" />&nbsp;
        <asp:Button ID="btnClente" runat="server" Text="Cliente" CssClass ="button black" />&nbsp;
        <asp:Button ID="btnUtente" runat="server" Text="Utente" CssClass ="button black" />&nbsp;
        <asp:Button ID="btnFornitore" runat="server" Text="Fornitore" CssClass ="button black" />&nbsp;--%>
    </div>
    <%--<div>
        <asp:Image ID="Image1" runat="server" ImageUrl ="~/assets/img/freccia.png" Width ="3%" Height ="3%"   />
        <asp:Button ID="btnSubOrganizzazione" runat="server" Text="Sub - Organizzazione" CssClass ="button black" style="margin-left :25px" />&nbsp; 
    </div>--%>
     <div class="container">
        <h1 class="page-title">I SubClienti</h1>
			&nbsp;<asp:Panel id="PanelRicercaSubCliente" runat="server" visible="false" groupingtext="Ricerca">               
               <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtRicercaSubCliente" runat="server" CssClass ="input-text" placeholder="Inserisci il subcliente"></asp:TextBox>
                            <asp:Button ID="btnRicercaSubCliente" runat="server" Text="Ricerca" CssClass ="button black" />
                        </div> 
                </div> 
                <div class="profile-column right ">
                        <div class="field">    
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> 
            </asp:Panel>
            <h1 class="page-title"></h1> 
            <section id="subcliente" class="content-section">
                 <asp:Panel ID="PanelEliminaOrg" runat="server" visible="false" groupingtext="Vuoi cancellare questa organizzazione">
                <div class="profile-column left">
                <br />
                        <div class="field">
                            <asp:Label ID="lblCodiceElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblSubClienteElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblPartitaIvaElimina" runat="server" CssClass="label " 
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
                                        <table class="content-table" >
                                            <thead>
                                                <tr ID="Tr11" runat="server" >
                                                    <th ID="Td111" runat="server" align="center" class="a-left" >
                                                        <asp:Label ID="lblCodice1" runat="server" Text="Codice" ></asp:Label>
                                                    </th>
                                                                                            
                                                    <th ID="Th7" runat="server" align="center">
                                                        <asp:Label ID="lblRagSoc" runat="server" Text="Ragione Sociale"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblPariva1" runat="server" Text="P.Iva"></asp:Label>
                                                    </th>    
                                                    <th ID="Th6" runat="server" align="center">
                                                        <asp:Label ID="lblCodEst1" runat="server" Text="Codice Esterno"></asp:Label>
                                                    </th>                                                
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th4" runat="server" align="center">
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
                                                    <asp:Label ID="lblCodice1" runat="server" Text='<%# Eval("codice")%>'></asp:Label>
                                                </td>                                                
                                                <td>
                                                    <asp:Label ID="lblRagSoc1" runat="server" Text='<%# Eval("ragsoc")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPariva1" runat="server" Text='<%# Eval("pariva")%>'></asp:Label>
                                                </td>    
                                                <td>
                                                    <asp:Label ID="lblCodEst1" runat="server" Text='<%# Eval("codiceest")%>'></asp:Label>
                                                </td> 
                                                 <%If Session("tipoutente") = "Utente" And Session("isadmin") = 0 Then%>
                                                 <td class ="dimtdim" >
                                                    <asp:ImageButton ID="imgMostra" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ="Dettagli" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                 <%Else%>                                            
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
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
                <form action="#">
                <asp:Panel ID="PanelModificaSubCliente" runat="server" visible="false">
                    <asp:Label ID="lblIdSubCliente" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdRecapito" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="blCliente" runat="server" CssClass="label " Text="Cliente"></asp:Label>
                            <asp:DropDownList ID="ddlCliente" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlCliente" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            
                        </div>
                        
                        
                        <div class="field">
                            <asp:Label ID="lblRagSoc" runat="server" CssClass="label " Text="Ragione Sociale"></asp:Label>
                            <asp:TextBox ID="txtRagSoc" runat="server" class="input-text" name="ragsoc" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtRagSoc" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblPiva" runat="server" CssClass="label " Text="P.Iva"></asp:Label>
                            <asp:TextBox ID="txtPiva" runat="server" class="input-text" name="piva" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtPiva" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblOrganizazione" runat="server" CssClass="label " text="Organizzazione"></asp:Label>
                           <asp:DropDownList ID="ddlOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblSubOrganizzazione" runat="server" CssClass="label " text="Sub - Organizzazione"></asp:Label>
                            <asp:DropDownList ID="ddlSubOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"
                                ></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCommittenza" runat="server" CssClass="label " 
                                Text="Committenza"></asp:Label>
                             <asp:TextBox ID="txtCommittenza" runat="server" class="input-text" name="committenza" 
                                 type="text" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCodEst" runat="server" CssClass="label " 
                                Text="Cod.Esterno"></asp:Label>
                             <asp:TextBox ID="txtCodEst" runat="server" class="input-text" name="codest" type="text" />
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtCodEst" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                          
                        <div class="field">
                            <asp:Label ID="lblNote" runat="server" CssClass="label " 
                                Text="Note"></asp:Label>
                            <asp:TextBox ID="txtNote" runat="server" class="input-text" 
                                name="note" type="text" TextMode ="MultiLine"  />
                        </div>
                    </div>
                    <div class="profile-column right">
                        <div class="field">
                            <asp:Label ID="lblCodice" runat="server" CssClass="label " text="Codice"></asp:Label>
                            <asp:TextBox ID="txtCodice" runat="server" class="input-text" name="codice" type="text" ReadOnly ="true"  />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCodice" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCap" runat="server" CssClass="label " 
                                Text="Cap"></asp:Label>
                            <asp:TextBox ID="txtCap" runat="server" class="input-text" 
                                name="cap" type="text" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblRegione" runat="server" CssClass="label " Text="Regione"></asp:Label>
                            <asp:DropDownList ID="ddlRegione" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="-1" ControlToValidate="ddlRegione" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblProvincia" runat="server" CssClass="label " Text="Provincia"></asp:Label>
                            <asp:DropDownList ID="ddlProvincia" runat="server" class="input-drop"  style="width:220px;margin-bottom:14px;height:27px"                               
                                AutoPostBack="True"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="-1" ControlToValidate="ddlProvincia" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblComune" runat="server" CssClass="label " Text="Comune"></asp:Label>
                            <asp:TextBox ID="ddlComune" runat="server" class="input-text" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" InitialValue="-1" ControlToValidate="ddlComune" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            
                        </div>   
                        <div class="field">
                            <asp:Label ID="lblIndirizzo" runat="server" CssClass="label " 
                                Text="Indirizzo"></asp:Label>
                            <asp:TextBox ID="txtIndirizzo" runat="server" class="input-text" 
                                name="indirizzo"  type="text" />
                        </div>                     
                        <div class="field">
                            <asp:Label ID="lblVicinoa" runat="server" CssClass="label " 
                                Text="Vicino a"></asp:Label>
                            <asp:TextBox ID="txtVicinoa" runat="server" class="input-text" 
                                name="vicinoa" type="text" />
                        </div>                        
                        <div class="field">
                            <asp:Label ID="lblListino" runat="server" CssClass="label " 
                                Text="Listino"></asp:Label>
                            <asp:DropDownList ID="ddlListino" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblBloccoAmm" runat="server" CssClass="label " 
                                Text="Blocco Amministrativo" Width="70%"></asp:Label>
                            <asp:DropDownList ID="ddlBloccoAmm" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div style="vertical-align :top" ><%--<asp:Label ID="lblConferma" runat="server"  CssClass="order-status confirmed "  style="background-image:none " visible="false"  ></asp:Label>--%>
                    <asp:Label ID="lblConferma" runat="server"  CssClass="order-status executed" Visible="false"   ></asp:Label></div>
                    <div>
                        <asp:Button ID="btnModifica" runat="server" CssClass="button" Text="Modifica" />
                        <asp:Button ID="btnSalva" runat="server" CssClass="button" Text="Salva" Visible="false" />
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="button" Text="Annulla" />
                    </div>
                    
                    <hr />
                    <div class ="barra" ><asp:LinkButton ID="lbContratti" runat="server" Text ="Contratti" ForeColor ="White" ></asp:LinkButton></div>
                     <asp:Panel ID="Panel2" runat="server">
                                                  

                    <h1 class="section-title"></h1>                     
                    <div class="csslistview ">
                                <asp:ListView ID="ListView3" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblCodice1" runat="server" Text="Codice"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblDaData1" runat="server" Text="Valido Da"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblAData1" runat="server" Text="Valido A"></asp:Label>
                                                    </th>                                                   
                                                    <th ID="Th3" runat="server" align="center">
                                                        <asp:Label ID="lblCodEsterno1" runat="server" Text="Codice Esterno"></asp:Label>
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
                                                    <asp:Label ID="lblCodice" runat="server" Text='<%# Eval("codice")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDaData" runat="server" Text='<%# Eval("dadata")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAData" runat="server" Text='<%# Eval("adata")%>'></asp:Label>
                                                </td>                                                
                                                <td>
                                                    <asp:Label ID="lblImporto" runat="server" Text='<%# Eval("codesterno")%>'></asp:Label>
                                                </td>                                               
                                                 
                                               <%If Session("tipoutente") = "Operatore" And Session("isadmin") = 1 Then%>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip ="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"  />
                                                </td>                                               
                                                <%else %>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgLente" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ="Mostra" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>  
                                                <%End If%>                                           
                                                
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager3" runat="server" PagedControlID="ListView3" 
                                    PageSize="100">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                               
                           </div>
                        
                    
                        
                        <div class="field">                  
                             
                             &nbsp;<asp:Button ID="btnAggiungi" runat="server" Text="Aggiungi" Cssclass="button black" Width="25%" Visible ="true" ></asp:Button>
                         </div> 
                    </asp:Panel>
                    <hr />
                    <div class ="barra" ><asp:LinkButton ID="lbRecapiti" runat="server" Text ="Recapiti" ForeColor ="White" ></asp:LinkButton></div>
                    <asp:Panel ID="Panel3" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >                    
                      <ContentTemplate >
                       <div class="csslistview " style ="width:60%">
                        <asp:ListView ID="ListView2" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Th2" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblDescrizione1" runat="server" Text="Descrizione"></asp:Label>
                                                    </th>
                                                    <th ID="Td111" runat="server" align="center" >
                                                        <asp:Label ID="lblContatto1" runat="server" Text="Contatto"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblTipo1" runat="server" Text="Tipo"></asp:Label>
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
                                                    <asp:Label ID="lblDescrizione" runat="server" Text='<%# Eval("descrizione")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblContatto" runat="server" Text='<%# Eval("contatto")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("descr")%>'></asp:Label>
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
                                    PageSize="100">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                    </div>
                    <hr />
                    <div class="profile-column left ">
                         <div class="field">                         
                             <asp:Label ID="lblSicuro" runat="server" Text="Eliminare il contatto " Visible ="false"></asp:Label><%If lblSicuro.Visible = True Then%><% =ddlTipologiaContatto.SelectedItem%><%End If%>
                             &nbsp;<asp:Button ID="btnElimina" runat="server" Text="Elimina" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                             &nbsp;<asp:Button ID="btnAnnullaContatto" runat="server" Text="Annulla" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                         </div> 
                     </div>    
                    <div class="profile-column inter">
                        <div class="field">
                            <asp:TextBox ID="txtDescrizioneContatto" runat="server" class="input-text" name="descrizione" type="text" placeholder="inserisci la descrizione" />
                            <asp:TextBox ID="txtContatto" runat="server" class="input-text" name="contatto" type="text" placeholder="inserisci il nuovo contatto" />
                            <asp:DropDownList ID="ddlTipologiaContatto" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"></asp:DropDownList>                            
                            
                        </div>  
                    
                        <div class="field">
                           &nbsp;<asp:Button ID="btnMemorizza" runat="server" Text="Memorizza" Cssclass="button black" Width="30%"></asp:Button>
                           &nbsp;<asp:Button ID="btnAnnulla1" runat="server" Text="Annulla" Cssclass="button black" Width="30%"></asp:Button>
                        </div>
                        
                    </div> 
                     
                    </ContentTemplate>
                    <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnElimina" />
                    </Triggers>
                  </asp:UpdatePanel> 
                    </asp:Panel>               
                </asp:Panel>
                
            </section>
            
      




    </div>
   </ContentTemplate>   
    </asp:UpdatePanel>

  
</asp:Content>