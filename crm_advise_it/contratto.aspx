<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="contratto.aspx.vb" Inherits="crm_advise_it.contratto1" MasterPageFile ="~/Site.Master"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>    
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
                    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
                    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css" rel="stylesheet" type="text/css" />
                    <script type="text/javascript">
                        function ShowPopup(message) {
                            $(function () {
                                $("#dialog").html(message);
                                $("#dialog").dialog({
                                    title: "ApriUnTcket.it",
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
    <div class="container">
    <h1 class="page-title">I Contratti</h1>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server"  >                    
                      <ContentTemplate >
       
			&nbsp;<asp:Panel id="PanelRicercaContratto" runat="server" visible="true" groupingtext="Filtro">
                <div class="fieldgroup">
					<div class="field checkbox">
						<asp:RadioButton ID="rbTutti" runat="server" data-rif="login-field-tutti" checked ="true" Text ="Tutti" AutoPostBack ="true"  />                        
					</div>
					<div class="field checkbox">
						<asp:RadioButton ID="rbValidi" runat="server" data-rif="login-field-validi" checked ="false" Text ="Validi" AutoPostBack ="true" />                        
					</div>
					<div class="field checkbox">
						<asp:RadioButton ID="rbScaduti" runat="server" data-rif="login-field-scaduti" checked ="false" Text ="Scaduti" AutoPostBack ="true" />                        
					</div>
				</div>
                <%--<div class="profile-column right ">
                        <div class="field">    
                            <asp:Button ID="btNuovo" runat="server" Text="Nuovo" CssClass ="button black" />
                        </div> 
                </div> --%>
            </asp:Panel>
            <h1 class="page-title"></h1> 
            <section id="aziende" class="content-section">
            <asp:Panel ID="PanelEliminaContratto" runat="server" visible="false" groupingtext="Vuoi cancellare questo contratto?">
                <div class="profile-column left">
                <br />
                        <div class="field">
                            <asp:Label ID="lblCodiceElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblCodEsternoElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                             <asp:Label ID="lblClienteElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                                <asp:Button ID="btnConferma" runat="server" CssClass="button" Text="Conferma" /><br />
                                <asp:Button ID="btnAnnullaCanc" runat="server" CssClass="button" Text="Annulla" /> 
                        </div>
                </div> 
                
            </asp:Panel> 
                <asp:Panel ID="Panel1" runat="server" visible="false" groupingtext="" >

                    <h1 class="section-title"></h1>
                     
                   <div class ="csslistview">
                                <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblCodice1" runat="server" Text="Codice"></asp:Label>
                                                    </th>
                                                    <th ID="Th10" runat="server" align="center" >
                                                        <asp:Label ID="lblCliente1" runat="server" Text="Cliente"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" align="center">
                                                        <asp:Label ID="lblDaData1" runat="server" Text="Valido Da"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblAData1" runat="server" Text="Valido A"></asp:Label>
                                                    </th>  
                                                   
                                                     
                                                    <th ID="Th3" runat="server" align="center">
                                                    <% If (Session("tipoutente") = "Operatore" And Session("tipoutente") = "Utente") Or Session("isadmin") = 1 Then%>                                               
                                                        <asp:Label ID="lblImporto1" runat="server" Text="Importo"></asp:Label>
                                                     <% End If%>           
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
                                                    <asp:Label ID="lblCodice" runat="server" Text='<%# Eval("codice")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("ragsoc")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDaData" runat="server" Text='<%# Eval("dadata")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAData" runat="server" Text='<%# Eval("adata")%>'></asp:Label>
                                                </td> 
                                                <% If (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente") And Session("isadmin") = 1 Then%>                                               
                                                <td>
                                                    <asp:Label ID="lblImporto" runat="server" Text='<%# Eval("importo")%>'></asp:Label>
                                                </td>  
                                                <% End If%>                                             
                                                 <%If (Session("tipoutente") = "Operatore" And Session("isadmin") = 0) Or Session("tipoutente") = "Utente" Then%>
                                                 <td class ="dimtdim" >
                                                    <asp:ImageButton ID="imgMostra" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ="Dettagli" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                 <%Else%>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip ="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip ="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                   
                                                </td>
                                                 <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgDuplica" runat="server" 
                                                        ImageUrl="~/images/frecciasx.png" ToolTip ="Duplica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"/>
                                                         
                                                <%End If%>
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
                <asp:Panel ID="PanelModificaContratto" runat="server" visible="false">
                    <asp:Label ID="lblIdContratto" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdSoglia" runat="server" Text="-1" Visible="false"></asp:Label>
                    <br />
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblCodice" runat="server" CssClass="label " 
                                Text="Codice Interno"></asp:Label>
                            <asp:TextBox ID="txtCodice" runat="server" class="input-text" 
                                name="codice" ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCodice" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div> 
                         <div class="field">
                            <asp:Label ID="lblTipoScadenza" runat="server" CssClass="label " Text="Tipo Scadenza"></asp:Label>
                            <asp:DropDownList ID="ddlTipoScadenza" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"                              
                                ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="-1" ControlToValidate="ddlTipoScadenza" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblImporto" runat="server" CssClass="label " Text="Totale Contratto"></asp:Label>
                            <asp:TextBox ID="txtImporto" runat="server" class="input-text" 
                                name="importo" ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtImporto" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>                         
                        <div class="field">
                            <asp:Label ID="lblDaData" runat="server" CssClass="label " text="Data Inizio"></asp:Label>
                            <asp:TextBox ID="txtDaData" runat="server" class="input-text" 
                                name="dadata" ReadOnly="true" type="text" />
                             <cc1:CalendarExtender ID="CalendarExtender" runat="server" Format="dd/MM/yyyy"
                             TargetControlID="txtDaData" PopupButtonID="txtDaData" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtDaData" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        
                    </div>
                    
                    <div class="profile-column right">
                        
                        <div class="field">
                            <asp:Label ID="lblCodEsterno" runat="server" CssClass="label " text="Codice Esterno"></asp:Label>
                            <asp:TextBox ID="txtCodEsterno" runat="server" class="input-text" name="codest" 
                                ReadOnly="true" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCodEsterno" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblTipoContratto" runat="server" CssClass="label " Text="Tipo Contratto"></asp:Label>
                            <asp:DropDownList ID="ddlTipoContratto" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"                              
                                ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="-1" ControlToValidate="ddlTipoContratto" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="field">
                            <asp:Label ID="lblTipoFatturazione" runat="server" CssClass="label " Text="Tipo Fatturazione"></asp:Label>
                            <asp:DropDownList ID="ddlTipoFatturazione" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px"                              
                                ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" InitialValue="-1" ControlToValidate="ddlTipoFatturazione" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>    
                        <div class="field">
                            <asp:Label ID="lblAData" runat="server" CssClass="label " Text="Data Fine"></asp:Label>
                            <asp:TextBox ID="txtAData" runat="server" class="input-text" name="cap" 
                                ReadOnly="true" type="text" />
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                             TargetControlID="txtAData" PopupButtonID="txtAData"    />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtAData" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                        </div>      
                        
                                            
                        
                    </div>
                    <div class="fieldgroup">
					    <div class="field checkbox" >
                            <asp:CheckBoxList ID="cblSubCliente" runat="server" Enabled ="false" ></asp:CheckBoxList>
                        </div>
                        
                        
                     </div> 
                    <div class="clear">
                    </div>
                    <div style="vertical-align :top" ><%--<asp:Label ID="lblConferma" runat="server"  CssClass="order-status confirmed "  style="background-image:none " visible="false"  ></asp:Label>--%>
                    <asp:Label ID="lblConferma" runat="server"  CssClass="order-status executed"  ></asp:Label></div>
                    <div>
                    <% If Session("tipoutente") = "Operatore" And Session("isadmin") = 1 Then%>
                        <asp:Button ID="btnModifica" runat="server" CssClass="button" Text="Modifica" />                         
                        <asp:Button ID="btnSalva" runat="server" CssClass="button" Text="Salva" Visible="false" />
                    <%End If%>
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="button" Text="Annulla" />
                    </div>

                    
                    <hr />


                    <div class ="barra" ><asp:LinkButton ID="lbRighe" runat="server" Text ="Righe Fatturate" ForeColor ="White" ></asp:LinkButton></div>
                    <div >
                        
                         <asp:Panel ID="PanelRighe"  runat="server" Visible="false"  >
                                  <div style="border:0; width:150; border-color:Blue; background-color:#DDDDDD;  vertical-align:middle; text-align:center">
                                  <%--<strong><asp:Label ID="Label4" runat="server" Text="Lista"></asp:Label></strong>
                                     <hr />--%>
                                     <asp:ListView ID="ListView3" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left">
                                                        <asp:Label ID="lblPeriodo1" runat="server" Text="Periodo"></asp:Label>
                                                    </th>                             
                                                    <th ID="Td6" runat="server" align="center" colspan ="2">
                                                        <asp:Label ID="lblFatturato" runat="server" Text="Fatturato"></asp:Label>
                                                    </th>
                                                   <%-- <th ID="Th1" runat="server" align="center">
                                                    </th>--%>
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
                                                    <asp:Label ID="lblData" runat="server" Text='<%# Eval("periodo")%>'></asp:Label>
                                                </td>                                                
                                                <td>                                               
                                                    <asp:CheckBox ID="rd1" runat="server" CssClass="checkbox"  GroupName="scelta" AutoPostBack="true" Checked ="false"  ToolTip ='<%# eval("id") %>'  />
                                                                                                 
                                                
                                                </td>
                                                                                              
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                            
                                            
                                  </div>
                                 
                                 
                        </asp:Panel>
                    </div>






















                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >                    
                      <ContentTemplate >--%>
                     <hr />
                     <div class ="barra" ><asp:LinkButton ID="lbSoglie" runat="server" Text ="Soglie" ForeColor ="White" ></asp:LinkButton></div>
                    
                    <asp:Panel ID="PanelSoglie" runat="server" Visible="false" >
                    <div class ="csslistview " >
                        <asp:ListView ID="ListView2" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table">
                                            <thead>
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Th2" runat="server" >
                                                        <asp:Label ID="lblTipoSoglia1" runat="server" Text="Tipo Soglia"></asp:Label>
                                                    </th> 
                                                    <th ID="Th8" runat="server"  >
                                                        <asp:Label ID="lblTariffazione1" runat="server" Text="Tariffazione"></asp:Label>
                                                    </th>
                                                    <th ID="Td111" runat="server"  >
                                                        <asp:Label ID="lblSoglia1" runat="server" Text="Soglia"></asp:Label>
                                                    </th>
                                                    <th ID="Th9" runat="server"  >
                                                        <asp:Label ID="lblTempoRes" runat="server" Text="Residuo Soglia"></asp:Label>
                                                    </th>
                                                    <th ID="Td112" runat="server" >
                                                        <asp:Label ID="lblAvviso1" runat="server" Text="Avviso"></asp:Label>
                                                    </th> 
                                                    <th ID="Th4" runat="server"  >
                                                        <asp:Label ID="lblFuori1" runat="server" Text="Fuori Soglia"></asp:Label>
                                                    </th> 
                                                    <th ID="Th5" runat="server"  >
                                                        <asp:Label ID="lblCostoFisso1" runat="server" Text="Costo Fisso (&#8364;)"></asp:Label>
                                                    </th>
                                                    <th ID="Th6" runat="server" >
                                                        <asp:Label ID="lblCostoVar1" runat="server" Text="Costo Variabile (&#8364;)"></asp:Label>
                                                    </th> 
                                                    <th ID="Th7" runat="server" >
                                                        <asp:Label ID="lblSLA1" runat="server" Text="SLA (ore lav.)"></asp:Label>
                                                    </th>
                                                                                                       
                                                    <th ID="Td6" runat="server" >
                                                    </th>
                                                    <th ID="Th1" runat="server" >
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
                                                    <asp:Label ID="lblTipoSoglia" runat="server" Text='<%# Eval("tiposoglia")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblTariffazione1" runat="server" Text='<%# Eval("tariffazione")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblSoglia" runat="server" Text='<%# Eval("soglia")%>'></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:Label ID="lblTempoRes" runat="server" Text='<%# Eval("tempores")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAvviso" runat="server" Text='<%# Eval("avviso")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFuori" runat="server" Text='<%# Eval("fuorisoglia")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCostoFisso" runat="server" Text='<%# Eval("costofisso")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCostoVar" runat="server" Text='<%# Eval("costovar")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSLA" runat="server" Text='<%# Eval("sla")%>'></asp:Label>
                                                </td>
                                                                                               
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" AlternateText ='<%# eval("id") %>' CssClass ="dimim"    />
                                                </td>
                                                <td class ="dimtdim">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip="Elimina" AlternateText ='<%# eval("id") %>' CssClass ="dimim" />
                                                         
                                                      
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
                             <asp:Label ID="lblSicuro" runat="server" Text="Eliminare la soglia " Visible ="false"></asp:Label><%If lblSicuro.Visible = True Then%><% =ddlTipoSoglia.SelectedItem%><%End If%>
                             &nbsp;<asp:Button ID="btnElimina" runat="server" Text="Elimina" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                            &nbsp;<asp:Button ID="btnAnnullaSoglia" runat="server" Text="Annulla" Cssclass="button black" Width="25%" Visible ="false" ></asp:Button>
                         </div> 
                     </div>     
                     <div class="profile-column inter">
                        <div class="field">                            
                            <asp:DropDownList ID="ddlTipoSoglia" runat="server" class="input-droplist" style="width:15%; margin-bottom:14px; height:27px" tooltip="tipo soglia"></asp:DropDownList>                            
                            <asp:Label ID="Label1" runat="server" Text="*" ForeColor ="Red" Visible="false" ></asp:Label>
                            <asp:DropDownList ID="ddlTipoTariffazione" runat="server" class="input-droplist" style="width:15%; margin-bottom:14px; height:27px" tooltip="tipo tariffazione" ></asp:DropDownList>                            
                            <asp:Label ID="Label2" runat="server" Text="*" ForeColor ="Red" Visible="false" ></asp:Label>
                            <asp:TextBox ID="txtSoglia" runat="server" class="input-textlist" name="soglia" type="text" style="width:15%;" placeholder="soglia" ToolTip ="soglia" />
                            <asp:TextBox ID="txtAvviso" runat="server" class="input-textlist" name="avvso" type="text" style="width:15%;"  placeholder="preavviso soglia" ToolTip ="preavviso soglia" />
                            
                        </div>   
                        <div class="field">
                            <asp:DropDownList ID="ddlFuori" runat="server" class="input-droplist" 
                                style="width:15%; margin-bottom:14px; height:27px" AutoPostBack="True" ToolTip ="fuori soglia"></asp:DropDownList>                            
                            <asp:Label ID="Label3" runat="server" Text="*" ForeColor ="Red" Visible="false" ></asp:Label>
                            <asp:DropDownList ID="ddlListino" runat="server" class="input-droplist" style="width:15%; margin-bottom:14px; height:27px" visible="false" ToolTip ="listino"></asp:DropDownList>                            
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" InitialValue="-1" ControlToValidate="ddlListino" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>--%>
                            <asp:TextBox ID="txtCostoFisso" runat="server" class="input-textlist" name="costofisso" type="text" style="width:15%;" placeholder="costo fisso (&#8364;)" ToolTip ="costo fisso" />
                            <asp:TextBox ID="txtCostoVar" runat="server" class="input-textlist" name="costovar" type="text" style="width:15%;" placeholder="costo variabile (&#8364;)" ToolTip ="costo variabile"/>
                            <asp:TextBox ID="txtSLA" runat="server" class="input-textlist" name="sla" type="time" style="width:15%;" placeholder="sla (ore)" ToolTip ="sla (ore)" />
                        </div>
                        <div class="field">
                            &nbsp;<asp:Button ID="btnMemorizza" runat="server" Text="Memorizza" Cssclass="button black" Width="30%"></asp:Button> 
                            &nbsp;<asp:Button ID="btnAnnulla1" runat="server" Text="Annulla" Cssclass="button black" Width="30%"></asp:Button>
                        </div>
                        
                    </div> 
                     </asp:Panel>
                   <%-- </ContentTemplate>
                  </asp:UpdatePanel>--%>
                </asp:Panel>
                
            </section>
     
    </ContentTemplate> 
    </asp:UpdatePanel>
    </div>
</asp:Content>
