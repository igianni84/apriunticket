<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="fornitoreorg.aspx.vb" Inherits="crm_advise_it.fornorg" MasterPageFile ="~/Site.Master" MaintainScrollPositionOnPostBack="True" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
    <div>
    </div>
  
     <div class="container">
        <h1 class="page-title">I Fornitori Organizzazioni</h1>
        
			&nbsp;<asp:Panel id="PanelRicercaFornitore" runat="server" visible="false" groupingtext="Ricerca">
            <%--<div class="field" >                            
                    <asp:DropDownList ID="ddlFornitore" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px;"                                
                        AutoPostBack="True" ></asp:DropDownList>
            </div>--%>
                <div class="profile-column left">
                        <div class="field">
                            <asp:TextBox ID="txtRicercaFornitore" runat="server" CssClass ="input-text" placeholder="Inserisci il fornitore"></asp:TextBox>
                            <asp:Button ID="btnRicercaFornitore" runat="server" Text="Ricerca" CssClass ="button black" />
                        </div> 
                </div> 
                <div class="profile-column right ">
                        <div class="field">    
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> 
            </asp:Panel>
            
            <h1 class="page-title"></h1> 
            <section id="fornitore" class="content-section">
                <asp:Panel ID="PanelEliminaOrg" runat="server" visible="false" groupingtext="Vuoi cancellare questo fornitore">
                <div class="profile-column left">
                <br />
                        <div class="field">
                            <asp:Label ID="lblCodiceElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblFornitoreElimina" runat="server" CssClass="label " 
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
                                                    <th ID="Th2" runat="server" align="center">
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
                                                <td class ="dimtdim" >
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' cssclass ="dimim" />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' cssclass ="dimim"/>
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgLega" runat="server" 
                                                        ImageUrl="~/images/piu.png" ToolTip="Lega" AlternateText ='<%# eval("id") %>'  cssclass ="dimim"/>
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
                <%--<form action="#">--%>
                <asp:Panel ID="PanelModificaFornitore" runat="server" visible="false">
                    <asp:Label ID="lblIdFornitore" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblCodice" runat="server" CssClass="label " text="Codice"></asp:Label>
                            <asp:TextBox ID="txtCodice" runat="server" class="input-text" name="codice" type="text" ReadOnly ="true"  />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCodice" 
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
                        <%--<div class="field">
                            <asp:Label ID="lblOrganizazione" runat="server" CssClass="label " text="Organizzazione"></asp:Label>
                           <asp:DropDownList ID="ddlOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblSubOrganizzazione" runat="server" CssClass="label " text="Sub - Organizzazione"></asp:Label>
                            <asp:DropDownList ID="ddlSubOrganizzazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"
                                ></asp:DropDownList>
                        </div>            --%>            
                        <div class="field">
                            <asp:Label ID="lblCodEst" runat="server" CssClass="label " 
                                Text="Cod.Esterno"></asp:Label>
                             <asp:TextBox ID="txtCodEst" runat="server" class="input-text" name="codest" type="text" />
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtCodEst" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <%--<div class="field">
                            <asp:Label ID="lblCliente" runat="server" CssClass="label " Text="Cliente"></asp:Label>
                            <asp:DropDownList ID="ddlCliente" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddlCliente" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            
                        </div>  --%>
                        <div class="field">
                            <asp:Label ID="lblNote" runat="server" CssClass="label " 
                                Text="Note"></asp:Label>
                            <asp:TextBox ID="txtNote" runat="server" class="input-text" 
                                name="note" type="text" TextMode ="MultiLine"  />
                        </div>
                        <div class="field"> 
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="label " Visible ="false"></asp:FileUpload>
                            <asp:Button ID="btnCarica" runat="server" Text="Carica" CssClass ="button " visible ="false"></asp:Button>
                            <asp:TextBox ID="txtLogo" runat="server" class="input-text" name="logo" visible="false" type="text" CssClass="label "/>
                                                    
                        </div>
                    </div>
                    <div class="profile-column right">
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
                              <asp:Image ID="imgLogo" runat="server"></asp:Image>
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
                    
                    
                                       
                </asp:Panel>
                <%--</form>--%>                 
            </section>
            
           






            <section id="profilo-view2" class="content-section">
                
                <asp:Panel ID="PanelAggiungiFornitore" runat="server" visible="false">
                    
                    <div >
                        <div class="button-area"> 
                        
                       <%--<asp:LinkButton ID="lbCliente" runat="server" Text="Cliente"  Visible="true" CssClass ="order-status blu" ></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lbSubCliente" runat="server" Text="SubCliente"  Visible="true" CssClass ="order-status blu" ></asp:LinkButton>&nbsp;
                        --%>
                        <asp:LinkButton ID="lbOrganizzazione" runat="server" Text="Organizzazione"  Visible="true" CssClass ="order-status blu" ></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lbSubOrganizzazione" runat="server" Text="SubOrganizzazione"  Visible="true" CssClass ="order-status blu" ></asp:LinkButton>&nbsp;
                        
                    </div>
                       <br />
                        <div class="field">
                            <asp:Label ID="lbldrop" runat="server" CssClass="label " text=""></asp:Label>
                            <asp:DropDownList ID="ddldrop" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>&nbsp;
                            <asp:ImageButton ID="imgAggiungi" runat="server" ImageUrl="~/images/piu.png" ToolTip="Aggiungi" AlternateText ='<%# eval("id") %>'  Width="2%"/>
                        </div>  
                        <div>
                            <asp:BulletedList ID="BulletedList1" ForeColor= "Black"   runat="server" style=" color:Black " autoposback="true" DisplayMode="LinkButton"
                                                         BulletStyle="CustomImage" >
                                           </asp:BulletedList>
                        </div>                                        
                        
                        
                        
                       
                    </div>
                    <div class="profile-column right">
                          
                    </div>
                    <div class="clear">
                    </div>
                    
                   
                 
                                   
                </asp:Panel>
                
            </section>

      




    </div>
    <%--</ContentTemplate>
   
    </asp:UpdatePanel>--%>

  
</asp:Content>
