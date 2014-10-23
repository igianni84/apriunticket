<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="utente.aspx.vb" Inherits="crm_advise_it.utente1" MasterPageFile ="~/Site.Master" MaintainScrollPositionOnPostBack="True" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
                    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
                    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" rel="stylesheet" type="text/css" />
                    <script type="text/javascript">
//                        function showLoading() {
//                            document.getElementById('loadingmsg').style.display = 'block';
//                            document.getElementById('loadingover').style.display = 'block';
//                        }
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


                    

                    <div id="dialog" style="display: none" >
                    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
    <ContentTemplate>
    <div>
    
       
     <div class="container">
        <h1 class="page-title">Gli Utenti</h1>
			<asp:Panel id="PanelRicercaUtente" runat="server" visible="false" groupingtext="Ricerca">
                <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtRicercaUtente" runat="server" CssClass ="input-text" placeholder="Inserisci l'utente"></asp:TextBox>
                            <asp:Button ID="btnRicercaUtente" runat="server" Text="Ricerca" CssClass ="button black" />
                        </div> 
                </div> 
                <div class="profile-column right ">
                        <div class="field">    
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> 
            </asp:Panel>
            <h1 class="page-title"></h1> 
            <section id="utente" class="content-section">
                <asp:Panel ID="PanelEliminaUtente" runat="server" visible="false" groupingtext="Vuoi cancellare questo utente?">
                <div class="profile-column left">
                <br />
                        <div class="field2">
                            <asp:Label ID="lblUseridElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblNomeElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblCognomeElimina" runat="server" CssClass="label " 
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
                                                        <asp:Label ID="lblUserid1" runat="server" Text="Userid" ></asp:Label>
                                                    </th>
                                                                                            
                                                    <th ID="Th7" runat="server" align="center">
                                                        <asp:Label ID="lblNome1" runat="server" Text="Nome"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblCognome1" runat="server" Text="Cognome"></asp:Label>
                                                    </th>    
                                                    
                                                    <th ID="Th6" runat="server" align="center">
                                                        <asp:Label ID="lblRepartot1" runat="server" Text="Reparto"></asp:Label>
                                                    </th> 
                                                                                                       
                                                     <th ID="Th2" runat="server" align="center">
                                                        <asp:Label ID="lblCliente1" runat="server" Text="Cliente"></asp:Label>
                                                    </th>  
                                                                                                
                                                    <th ID="Td6" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th1" runat="server" align="center">
                                                    </th>
                                                    <th ID="Th3" runat="server" align="center">
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
                                                    <asp:Label ID="lblUserid1" runat="server" Text='<%# Eval("userid")%>'></asp:Label>
                                                </td>                                                
                                                <td>
                                                    <asp:Label ID="lblNome1" runat="server" Text='<%# Eval("nome")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCognome1" runat="server" Text='<%# Eval("cognome")%>'></asp:Label>
                                                </td>    
                                                <td>
                                                    <asp:Label ID="lblReparto1" runat="server" Text='<%# Eval("reparto")%>'></asp:Label>
                                                </td> 
                                                <td>
                                                    <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("ragsoc")%>'></asp:Label>
                                                </td>                                                                                         
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"  />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgRinvia" runat="server" 
                                                        ImageUrl="~/images/frecciasx.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
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
                
                <asp:Panel ID="PanelModificaUtente" runat="server" visible="false">
                    <asp:Label ID="lblIdUtente" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdRecapito" runat="server" Text="-1" Visible="false"></asp:Label>
                     
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblTipo" runat="server" CssClass="label " Text="Tipo"></asp:Label>
                             <asp:DropDownList ID="ddlTipo" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True"></asp:DropDownList>                            
                        </div>
                        <div class="field">
                            <asp:Label ID="lblUserid" runat="server" CssClass="label " text="Mail"></asp:Label>
                            <asp:TextBox ID="txtUserid" runat="server" class="input-text" name="userid" type="text" ReadOnly ="true" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtUserid" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="button-area">
                             <asp:Label ID="lblErrore" runat="server" ForeColor ="Red"  CssClass="order-status error" Visible ="false"  Text="Mail non corretta"  ></asp:Label> 
                        </div> 
                        <div class="field">
                            <asp:Label ID="lblPassword" runat="server" CssClass="label " Text="Password"></asp:Label>
                            <asp:TextBox ID="txtPassword" runat="server" class="input-text" name="password" 
                                TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtPassword" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblConfermaPassword" runat="server" CssClass="label " Text="Conferma"  ></asp:Label>
                            <asp:TextBox ID="txtConfermaPassword" runat="server" class="input-text" 
                                name="confermapassword" placeholder="reinserisci la password" 
                                TextMode="Password" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtConfermaPassword" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div> 
                         <div class="button-area">
                             <asp:CompareValidator ID="PasswordCompare" runat="server" CssClass ="order-status waiting" 
                                        ControlToCompare="txtPassword" ControlToValidate="txtConfermaPassword" 
                                        Display="Dynamic" 
                                        ErrorMessage="Password di Conferma Errata" 
                                        ValidationGroup="CreateUserWizard1" ForeColor="Red"></asp:CompareValidator> 
                         </div>
                        <div class="field">
                            <asp:Label ID="lblNome" runat="server" CssClass="label " 
                                Text="Nome"></asp:Label>
                            <asp:TextBox ID="txtNome" runat="server" class="input-text" 
                                name="cap" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="txtNome" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCognome" runat="server" CssClass="label " 
                                Text="Cognome"></asp:Label>
                            <asp:TextBox ID="txtCognome" runat="server" class="input-text" 
                                name="cognome" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="txtCognome" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
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
                            <asp:Label ID="lblCap" runat="server" CssClass="label " 
                                Text="Cap"></asp:Label>
                            <asp:TextBox ID="txtCap" runat="server" class="input-text" 
                                name="cap" type="text" />
                        </div>     
                        
                            
                    </div>
                   
                    <div class="profile-column right">
                        <div >                                                  
                                <asp:CheckBox ID="cbxAmministratore" runat="server" CssClass ="checkbox "  Text="AMMINISTRATORE" ></asp:CheckBox>                            
                         </div>      
                        <br />
                        <div class="field">
                            <asp:Label ID="lblOrganizzazione" runat="server" CssClass="label " text="Organizzazione" Visible="false"></asp:Label>
                           <asp:DropDownList ID="ddlOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True" Visible="false" ></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblSubOrganizzazione" runat="server" CssClass="label " text="Sub - Organizzazione" Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlSubOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px" Visible="false"
                                ></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblCliente" runat="server" CssClass="label " Text="Cliente"></asp:Label>
                            <asp:DropDownList ID="ddlCliente" runat="server" class="input-drop" 
                                style="width:220px; margin-bottom:14px; height:27px" AutoPostBack="True"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlCliente" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            
                        </div> 
                        <div class="field">
                            <asp:Label ID="lblSubCliente" runat="server" CssClass="label " Text="SubCliente"></asp:Label>
                            <asp:DropDownList ID="ddlSubCliente" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddlSubCliente" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            
                        </div> 
                                                                
                        <div class="field">
                            <asp:Label ID="lblReparto" runat="server" CssClass="label " 
                                Text="Reparto"></asp:Label>
                            <asp:DropDownList ID="ddlReparto" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblAbilitato" runat="server" CssClass="label " 
                                Text="Abilitato"></asp:Label>
                            <asp:DropDownList ID="ddlAbilitato" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>
                        </div>
                        
                    </div>
                    <div class="clear"></div>
             <div>
         
           <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode ="Conditional" >                    
                      <ContentTemplate >
            <div class="button-area"> 
                    <asp:LinkButton ID="lbModifica" runat="server" Text="Modifica Password"  Visible="false" CssClass ="order-status blu" ></asp:LinkButton>
            </div>
            <asp:Panel ID="PanelModPass" runat="server" visible="false" > 
            <div class ="field">
                <asp:TextBox ID="txtVecchiaPsw" runat="server" TextMode="Password" CssClass ="input-text" placeholder="Inserisci la vecchia password" Visible="false" Width="50%"></asp:TextBox>
                
            </div>
            <div class="button-area"> 
                <asp:Button ID="btnVerifica" runat="server" Text="Verifica" CssClass ="button black" visible="false" />
                <asp:Label ID="lblError" runat="server" Text="Password Errata" CssClass ="order-status error"  ForeColor ="Red" Visible="false"></asp:Label>
            </div>
       
           
            <div class="field">
				<asp:TextBox ID="txtNuovaPassword" runat="server" TextMode="Password" CssClass ="input-text" Width ="50%" placeholder="Inserisci nuova password" Visible="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="txtNuovaPassword" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red"  Visible="false"></asp:RequiredFieldValidator>	   
            </div> 
            <div class="field">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass ="input-text" Width="50%" placeholder="Conferma nuova password" Visible="false"></asp:TextBox>  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtConfirmPassword" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red"  Visible="false"></asp:RequiredFieldValidator>
            </div>
            <div class="button-area" >
                <asp:Button ID="btnConfermo" runat="server" Text="Conferma" CssClass ="button black" Visible ="false"  /><br /><br />
                <%--<asp:CompareValidator ID="PasswordCompare" runat="server" CssClass ="order-status error" 
                                    ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" 
                                    Display="Dynamic" 
                                    ErrorMessage="Password di Conferma Errata" 
                                    ValidationGroup="CreateUserWizard1" ForeColor="Red"></asp:CompareValidator>--%> 
                <asp:Label ID="lblerror2" runat="server" Text="Password Errata" CssClass ="order-status error"  ForeColor ="Red" Visible="false"></asp:Label>
            </div>
            <div class="button-area" >
                <asp:Panel ID="Panel3" runat="server" visible="false" >                        
                  
                      <h7 class="order-status executed">Password modificata con successo</h7>
               
            </asp:Panel>
             </asp:Panel> 
			</div> 
            </ContentTemplate>                    
            </asp:UpdatePanel> 
           
            </div>                       
                    <%--<div class="clear">
                    </div>--%>
                    <div class="button-area" style="vertical-align :top" ><%--<asp:Label ID="lblConferma" runat="server"  CssClass="order-status confirmed "  style="background-image:none " visible="false"  ></asp:Label>--%>
                    <asp:Label ID="lblConferma" runat="server"  CssClass="order-status executed"  ></asp:Label></div>
                    <div>
                        <asp:Button ID="btnModifica" runat="server" CssClass="button" Text="Modifica" />
                        <asp:Button ID="btnSalva" runat="server" CssClass="button" Text="Salva" Visible="false" />
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="button" Text="Annulla" />
                    </div>
                   <hr />
                    <div class ="barra" ><asp:LinkButton ID="lbRecapiti" runat="server" Text ="Recapiti" ForeColor ="White" ></asp:LinkButton></div>
                    <asp:Panel ID="Panel2" runat="server">
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
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" CssClass ="dimim" AlternateText ='<%# eval("id") %>' />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" CssClass ="dimim" AlternateText ='<%# eval("id") %>'/>
                                                         
                                                      
                                                </td>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager2" runat="server" PagedControlID="ListView2" 
                                    PageSize="10">
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
                        <div class="button-area"> 
                            
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
