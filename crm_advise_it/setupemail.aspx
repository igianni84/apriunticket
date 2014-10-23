<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="setupemail.aspx.vb" Inherits="crm_advise_it.setupemail" MasterPageFile ="~/Site.Master"  ValidateRequest ="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
 <script type ="text/javascript" src ="js/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
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
                    <script type="text/javascript">
                        tinyMCE.init({
                            mode: "textareas",
                            theme: "advanced",
                            plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

                            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
                            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
                            /*theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,|,sub,sup,|,charmap,emotions,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
                            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",*/
                            theme_advanced_toolbar_location: "top",
                            theme_advanced_toolbar_align: "left",
                            theme_advanced_statusbar_location: "bottom",
                            theme_advanced_resizing: true,

                            content_css: "css/content.css",

                            template_external_list_url: "lists/template_list.js",
                            external_link_list_url: "lists/link_list.js",
                            external_image_list_url: "lists/image_list.js",
                            media_external_list_url: "lists/media_list.js",
                            template_replace_values: {
                                username: "Jack Black",
                                staffid: "991234"
                            },
                            template_templates:
            [{
                title: "Editor Details",
                src: "template.htm",
                description: "Adds Editor Name and Staff ID"
            },
                {
                    title: "Timestamp",
                    src: "time.htm",
                    description: "Adds an editing timestamp."
                }]
                        });        
    </script>
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"> 

    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    
                    <div id="dialog" style="display: none">
                    </div>
              
                    <div class="container">
                    <h1 class="page-title">Configurazione Email</h1>
                    <section id="email" class="content-section">
            <asp:Panel ID="PanelEliminaCliente" runat="server" visible="false" groupingtext="Vuoi cancellare questo modello?">
                <div class="profile-column left">
                <br />
                        <div class="field">
                         <asp:Label ID="lblTipoMailElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                            <asp:Label ID="lblOggettoElimina" runat="server" CssClass="label " 
                                ></asp:Label>                           
                             <asp:Label ID="lblAziendaElimina" runat="server" CssClass="label " 
                                ></asp:Label>
                                <asp:Button ID="btnConferma" runat="server" CssClass="button" Text="Conferma" /><br />
                                <asp:Button ID="btnAnnullaCanc" runat="server" CssClass="button" Text="Annulla" /> 
                        </div>
                </div> 
                
            </asp:Panel> 
                <asp:Panel ID="Panel3" runat="server" >                    
                    <asp:Label ID="lblIdMail" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblUpdate" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="csslistview ">
                                <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table" >
                                            <thead>
                                                <tr ID="Tr11" runat="server" >
                                                    <th ID="Td111" runat="server" >
                                                        <asp:Label ID="lblTipoMail1" runat="server" Text="Tipo Mail" ></asp:Label>
                                                    </th>                                                                                            
                                                    <th ID="Th7" runat="server" >
                                                        <asp:Label ID="lblOggetto1" runat="server" Text="Oggetto"></asp:Label>
                                                    </th>
                                                    <%--<th ID="Td3" runat="server" >
                                                        <asp:Label ID="lblCorpo1" runat="server" Text="Corpo"></asp:Label>
                                                    </th>    
                                                    <th ID="Th6" runat="server" >
                                                        <asp:Label ID="lblFirma1" runat="server" Text="Firma"></asp:Label>
                                                    </th>                             --%>                   
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
                                                <td >
                                                    <asp:Label ID="lblTipoMail" runat="server" Text='<%# Eval("tipomail")%>'></asp:Label>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lblOggetto" runat="server" Text='<%# Eval("oggetto")%>'></asp:Label>
                                                </td>
                                                <%--<td>
                                                    <asp:Label ID="lblCorpo" runat="server" Text='<%# Eval("corpo")%>'></asp:Label>
                                                </td>    
                                                <td>
                                                    <asp:Label ID="lblFirma" runat="server" Text='<%# Eval("firma")%>'></asp:Label>
                                                </td> --%> 
                                                <%--<%If Session("tipoutente") = "Utente" And Session("isadmin") = 0 Then%>
                                                 <td>
                                                    <asp:ImageButton ID="imgMostra" runat="server" 
                                                        ImageUrl="~/images/lente.png" ToolTip ="Dettagli" AlternateText ='<%# eval("id") %>' />
                                                </td>
                                                 <%Else%>   --%>                                    
                                                <td class ="dimtdim" >
                                                    <asp:ImageButton ID="imgModifica" runat="server" 
                                                        ImageUrl="~/images/modifica.png" ToolTip="Modifica" CssClass ="dimim" AlternateText ='<%# eval("id") %>' />
                                                </td>
                                                <td class ="dimtdim ">
                                                    <asp:ImageButton ID="imgCancella" runat="server" 
                                                        ImageUrl="~/images/cestino.png" ToolTip ="Elimina" CssClass ="dimim" AlternateText ='<%# eval("id") %>'  />
                                                </td>
                                                <%--<%End If%>--%>
                                            </tr>
                                        </tbody>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" 
                                    PageSize="10">
                                    <Fields>
                                        <asp:NumericPagerField NumericButtonCssClass="numeric_button" />
                                    </Fields>
                                </asp:DataPager>
                    </div> 
                    
                </asp:Panel>
            </section>
                    <%--<section id="email" class="content-section">--%>
                   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                       <ContentTemplate>     --%>                    
                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">                         
                         
                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Configurazione" >
                            <ContentTemplate> 
                             
                        <asp:Panel ID="Panel1" runat="server" >
                                                  
                           <div >
                             <div class="field">
                            <asp:Label ID="lblTipoMail" runat="server" Text="Tipologia Mail" CssClass="label"></asp:Label>
                            <asp:DropDownList ID="ddlTipoMail" runat="server" class="input-drop" style="width:220px; margin-bottom:14px; height:27px" AutoPostBack="True" ></asp:DropDownList>                           
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" InitialValue="-1" ControlToValidate="ddlTipoMail" 
                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                             <asp:ImageButton ID="imgInfo" runat="server" ImageUrl ="~/images/info.png" width="5%"  />
                          </div>    
                             <div class="field">
                            <asp:Label ID="lblErrore" runat="server" Text="Dati non ancora inseriti" 
                                     CssClass="order-status waiting  " Visible="False"></asp:Label>
                           </div> 
                             <div class="field">
                                    <asp:Label ID="lblOggetto" runat="server" Text="Oggetto" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtOggetto" runat="server" CssClass="input-text " Width="300px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtOggetto" 
                                                        Display="Dynamic" EnableClientScript="False" runat="server"  
                                                        ErrorMessage="*" ForeColor ="Red" ></asp:RequiredFieldValidator>
                            </div> 
                             <div class="field">
                                   <asp:Label ID="lblCorpo" runat="server" Text="Corpo" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtCorpo" runat="server" CssClass="input-text" Width="300px" Height ="100px" TextMode ="MultiLine" ></asp:TextBox>
                                        
                             </div> 
                             <div class="field">
                                   <asp:Label ID="lblFirma" runat="server" Text="Firma" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtFirma" runat="server" CssClass="input-text" Width="300px" Height ="50px" TextMode ="MultiLine" ></asp:TextBox>
                                        
                             </div> 
                             <div class="field">                               
                                        <asp:Button ID="btnSalva" runat="server" Text="Memorizza" CssClass ="button black "  /></td>
                             </div>         
                           </div>    
                        </asp:Panel>
                                          
                         </ContentTemplate>                  
                              </cc1:TabPanel>
                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Parametri" >
                            <ContentTemplate>   
                                
                        <asp:Panel ID="Panel2" runat="server"  >
                            <asp:Label ID="lblIdParametrizzazione" runat="server" visible="false" Text="-1" ></asp:Label>
                            <div  >
                              <div class="field">
                                    <asp:Label ID="lblMittente" runat="server" Text="Mittente" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtMittente" runat="server" CssClass="input-text " ></asp:TextBox>                                        
                                </div> 
                                <div class="field">
                                    <asp:Label ID="lblAccount" runat="server" Text="Account" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtAccount" runat="server" CssClass="input-text " ></asp:TextBox>                                        
                                </div> 
                                <div class="field">
                                    <asp:Label ID="lblPassword" runat="server" Text="Password" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-text " ></asp:TextBox>                                        
                                </div> 
                                <div class="field">
                                    <asp:Label ID="lblSmtp" runat="server" Text="SMTP" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtSmtp" runat="server" CssClass="input-text " ></asp:TextBox>                                        
                                </div> 
                                <div class="field">
                                    <asp:Label ID="lblPorta" runat="server" Text="Porta" CssClass ="label"></asp:Label>
                                    <asp:TextBox ID="txtPorta" runat="server" CssClass="input-text " ></asp:TextBox>                                        
                                </div>
                                <div class="field">                               
                                    <asp:Button ID="btnMemorizzaPar" runat="server" Text="Memorizza" CssClass ="button black "  /></td>
                                </div>   
                            </div> 
                        </asp:Panel>
                                
                                </ContentTemplate>                    
                              </cc1:TabPanel> 
                                                           
                         </cc1:TabContainer>
                       
                    <%--</section>--%>
                    </div>  
                
</asp:Content>