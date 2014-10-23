<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="gestticket.aspx.vb" Inherits="crm_advise_it.gestticket" MasterPageFile ="~/Site.Master"  ValidateRequest ="false"   %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">




<script type ="text/javascript" src ="js/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    
    <script type="text/javascript">

//        tinyMCE.init({

//            mode: "specific_textareas",
//            
//            editor_selector : "mce",
//            //theme_advanced_statusbar_location: "none",
//            entity_encoding: "raw",
//            width: "550px",
//            theme: "advanced",
//            plugins: "advhr,inlinepopups,paste,table,autolink",
//            dialog_type: "modal",
//            paste_auto_cleanup_on_paste: false,
//            paste_strip_class_attributes: "all",
//            paste_remove_spans: true,
//            paste_retain_style_properties: "",
//            table_styles: "Header 1=header1;Header 2=header2;Header 3=header3",
//            table_cell_styles: "Header 1=header1;Header 2=header2;Header 3=header3;Table Cell=tableCel1",
//            table_row_styles: "Header 1=header1;Header 2=header2;Header 3=header3;Table Row=tableRow1",
//            table_cell_limit: 100,
//            table_row_limit: 5,
//            table_col_limit: 5,
//            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,|,justifyleft,justifycenter,justifyright,fontselect,fontsizeselect,formatselect",
//            theme_advanced_buttons2: "cut,copy,paste,pasteword,selectall,|,bullist,numlist,|,outdent,indent,|,undo,redo,|,link,unlink,anchor,image,|,code,|,forecolor,backcolor",
//            theme_advanced_buttons3: "advhr,,removeformat,|,sub,sup,|,tablecontrols",
//            theme_advanced_toolbar_location: "top",
//            theme_advanced_toolbar_align: "left",
//            theme_advanced_statusbar_location: "bottom",
//            theme_advanced_resizing: true
//});

        tinyMCE.init({
            mode : "specific_textareas",
	        editor_selector : "mceEditor",
	        
            theme: "advanced",
            plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            //theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            //            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,|,sub,sup,|,charmap,emotions,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
            //            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
            //            theme_advanced_toolbar_location: "top",
            //            theme_advanced_toolbar_align: "left",
            //            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,
            
            content_css: "js/tinymce/examples/css/content.css",

            template_external_list_url: "js/tinymce/examples/lists/template_list.js",
            external_link_list_url: "js/tinymce/examples/lists/link_list.js",
            external_image_list_url: "js/tinymce/examples/lists/image_list.js",
            media_external_list_url: "js/tinymce/examples/lists/media_list.js",
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
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
                    
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate >--%>
  <section id="profilo-view" class="content-section">
  
                <asp:Panel ID="PanelTicket" runat="server" visible="true">                    
                    <div class="profile-column inter">
                        <div class="field3">
                            <asp:Label ID="lblDataApertura" runat="server" CssClass="labellist " Text="Data Apertura"></asp:Label>
                            <asp:Label ID="lblTicket" runat="server" CssClass="labellist " text="Ticket"></asp:Label>
                            <asp:Label ID="lblDataScadenza" runat="server" CssClass="labellist " text="Data Scadenza"></asp:Label>
                            <asp:Label ID="lblCliente" runat="server" CssClass="labellist " Text="Cliente"></asp:Label> 
                            <asp:Label ID="lblContratto" runat="server" CssClass="labellist " Text="Contratto"></asp:Label>
                        </div>
                        
                        
                        <div class="field3">
                            <asp:TextBox ID="txtDataAp" runat="server" class="input-text" name="dataap" type="date" Width ="13%" enabled="false" style="height:27px;margin-bottom:14px;"/>                        
                            <asp:TextBox ID="txtOraAp" runat="server" class="input-text" name="orapa" type="time" Width ="13%" enabled="false" style="height:27px;margin-bottom:14px;"/>
                            <asp:TextBox ID="txtTicket" runat="server" class="input-text" name="userid" type="text" Width ="13%" enabled="false" style="height:27px;margin-bottom:14px;"/>
                            <asp:TextBox ID="txtDataScadenza" runat="server" class="input-text" name="userid" type="date" style="height:27px;margin-bottom:14px;"/>
                            <asp:DropDownList ID="ddlCliente" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" ></asp:DropDownList>
                            <asp:DropDownList ID="ddlContratto" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" ></asp:DropDownList>
                        <%--</div>
                        <div class="field3">--%>
                            
                           
                            
                        </div>
                        <div class="field3">
                            
                            <asp:Label ID="lblUtente" runat="server" CssClass="labellist " Text="Utente"></asp:Label>
                            <asp:Label ID="lblInventario" runat="server" CssClass="labellist " Text="Inventario"></asp:Label>
                            
                            <asp:Label ID="lblSubCliente" runat="server" CssClass="labellist " text="SubCliente" ></asp:Label>
                            <asp:Label ID="lblTariffa" runat="server" CssClass="labellist " text="Tariffa" visible="false" ></asp:Label>
                            <asp:Label ID="lblPerContoDi" runat="server" CssClass="labellist " text="Per Conto Di" ></asp:Label>
                        </div>
                        <div class="field3">  
                        <asp:DropDownList ID="ddlUtente" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>
                                                         
                            <asp:DropDownList ID="ddlInventario" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True" ></asp:DropDownList>
                      <%--  </div>     
                    </div>
                   
                    <div class="profile-column right">
                        <div class="field3">--%>
                            
                            
                       <%-- </div>

                        <div class="field3">
                           
                        </div>
                        <div class="field3">--%>


                           <asp:DropDownList ID="ddlSubCliente" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True" ></asp:DropDownList>
                        
                           
                           <asp:DropDownList ID="ddlTariffa" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True" visible="false" ></asp:DropDownList>
                        <%--</div>
                        <div class="field3">--%>
                            
                           <asp:DropDownList ID="ddlPerContoDi" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True" ></asp:DropDownList>
                           
                           
                        </div>
                        
                        
                    </div>
                    <div class="profile-column inter ">
                        <div class="field2">
                            <asp:Label ID="lblOggetto" runat="server" CssClass="label " text="Oggetto" ></asp:Label>
                           <asp:TextBox ID="txtOggetto" runat="server" class="input-text" name="userid" type="text" enabled="false" style="height:27px;margin-bottom:14px;" />
                            
                        </div>
                        <div class="field2">
                            <asp:Label ID="lblDescrizione" runat="server" CssClass="label " text="Descrizione" ></asp:Label>
                            <asp:TextBox ID="txtDescrizione" runat="server" CssClass="mceEditor" 
                                TextMode ="MultiLine" Width ="71%" type="textareas" Columns="108" Rows="10" style="max-width :800px; " 
                                ></asp:TextBox>
                            
                        </div>
                        <div class="field2">
                            <asp:Label ID="lblOperatore" runat="server" CssClass="label " text="Operatore" ></asp:Label>
                            <asp:DropDownList ID="ddlOperatore" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>
                            
                        </div>
                    </div> 
                    
                    
                    
                    
                    <div class="clear"></div>
                    
                </asp:Panel>
                <div class="profile-column inter " style="text-align :center">                          
                        <asp:Button ID="btnNascondi" runat="server" Text="Nascondi" Cssclass="button black" style="left: 36%;"></asp:Button>&nbsp;                        
                        <asp:Button ID="btnAggiorna" runat="server" Text="Aggiorna" Cssclass="button black" style="left: 36%;"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnOnSite" runat="server" Text="Gestisci OnSite" Cssclass="button black" style="left: 50%;" ></asp:Button>&nbsp; 
                        <asp:Button ID="btnTelefonata" runat="server" Text="Gestisci Telefonata" Cssclass="button black" style="left: 50%;" ></asp:Button>                                            
                </div>
                <asp:Label ID="lblIdTickets" runat="server" Text="-1" Visible="false" ></asp:Label> 
                <hr /> 
                <div class ="barra" ><asp:LinkButton ID="lblEventi" runat="server" Text ="Eventi" ForeColor ="White" ></asp:LinkButton></div>                          
                <asp:Panel ID="PanelEventi" runat="server" visible="true"> 
                <h1 class="section-title"></h1>
                    <div class ="csslistview" >
                        <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table" >
                                            <thead >
                                                <tr ID="Tr11" runat="server">
                                                    <th ID="Td111" runat="server" align="center" class="a-left" width="15%"  >
                                                        <asp:Label ID="lblData" runat="server" Text="Data" ></asp:Label>                                                        
                                                    </th>                                                                                           
                                                    
                                                    <th ID="Td3" runat="server" align="center" width="45%" >
                                                        <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" ></asp:Label>
                                                    </th>    
                                                    
                                                    <th ID="Th4" runat="server" align="center" width="10%" >
                                                        <asp:Label ID="lblUsato" runat="server" Text="Tempo Usato" ></asp:Label>
                                                    </th>
                                                    <th ID="Th5" runat="server" align="center" width="10%" >
                                                        <asp:Label ID="lblTipo" runat="server" Text="Tipo" ></asp:Label>
                                                    </th>                                                                                                  
                                                     <th ID="Th2" runat="server" align="center" width="15%" >
                                                        <asp:Label ID="lblStato" runat="server" Text="Stato"></asp:Label>
                                                    </th>  
                                                    <th ID="Th3" runat="server" align="center" width="10%" >
                                                        <asp:Label ID="lblOperatore" runat="server" Text="Operatore" ></asp:Label>
                                                    </th>                                        
                                                   <th ID="Th1" runat="server" align="center" width="5%" >
                                                   <%If Session("tipoutente") <> "Utente" Then%>  
                                                        <asp:Label ID="lblStealth" runat="server" Text="Stealth" ></asp:Label>
                                                        <%End If%>
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
                                                    <asp:Label ID="lblData" runat="server" Text='<%# Eval("data")%>' Font-Size ="Small"  ></asp:Label>                                                    
                                                </td> 
                                                                        
                                                <td >
                                                                                                                                  
                                                  <asp:Label ID="lblDescrizione"  runat="server" Text='<%# Eval("descrizione")%>' Font-Size ="Small" ></asp:Label>
                                               
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTempo" runat="server" Text='<%# Eval("tempo")%>' Font-Size ="Small"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTariffazione" runat="server" Text='<%# Eval("tariffazione")%>' Font-Size ="Small"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStato" runat="server" Text='<%# Eval("stato")%>' Font-Size ="Small"></asp:Label>
                                                </td>    
                                                <td>
                                                    <asp:Label ID="lblOperatore" runat="server" Text='<%# Eval("cognome")%>' Font-Size ="Small"></asp:Label>
                                                </td>    
                                                <%If Session("tipoutente") <> "Utente" Then%>                                                                            
                                                <td class ="dimtdim ">
                                                    <asp:CheckBox ID="rd1" runat="server" CssClass="checkbox"  GroupName="scelta" Checked ="false" AutoPostBack="true"  ToolTip ='<%# eval("id") %>' />
                                                   
                                                </td>  
                                                <% End If%>                                              
                                            </tr>
                                            
                                            
                                        </tbody>
                                       
                                    </ItemTemplate>
                                </asp:ListView>
                    </div>
                    <div class="profile-column left">
                         <div class="field">
                            <asp:Label ID="lblDataAp" runat="server" CssClass="label " text="Data"></asp:Label>
                            <asp:TextBox ID="txtData" runat="server" class="input-text" name="dataap" type="date"  Width="30%" placeholder="data apertura ticket"/>&nbsp;&nbsp;                         
                            <asp:TextBox ID="txtOra" runat="server" class="input-text" name="orapa" type="time"  Width="20%" placeholder="orario apertura ticket" />
                           
                        </div>                        
                        <div class="field">
                            <asp:Label ID="lblTipoTariffazione" runat="server" CssClass="label " text="Tipo Assistenza"></asp:Label>
                            <asp:DropDownList ID="ddlTipoTariffazione" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>
                        </div>   
                        <div class="field">
                            <asp:Label ID="lblTempo" runat="server" CssClass="label " text="Tempo Gestione"></asp:Label>
                            <asp:TextBox ID="txtTempo" runat="server" class="input-text" name="pausain" type="time"  Width="20%" ToolTip ="Tempo Gestione" />
                        </div>
                     </div>  
                     <div class="profile-column right"> 
                        <div class="field">
                            <asp:Label ID="lblUtenti" runat="server" CssClass="label " text="Utente"></asp:Label>
                            <asp:DropDownList ID="ddlUtente1" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>                          
                        </div>
                        <div class="field">
                            <asp:Label ID="lblOperatore2" runat="server" CssClass="label " text="Operatore"></asp:Label>
                            <asp:DropDownList ID="ddlOperatore2" runat="server" class="input-drop" 
                            style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>                          
                        </div>
                        <div>
                            <asp:CheckBox ID="cbxStealth" runat="server" checked ="false" Text="Stealth"/>
                        </div>
                       
                    </div>
                     <div class="profile-column inter">
                        <div class="field">
                            <asp:Label ID="lblDescrizione1" runat="server" CssClass="label " text="Descrizione"></asp:Label>
                            <asp:TextBox ID="txtDescrizione1" runat="server" CssClass="mceEditor"  
                                TextMode ="MultiLine" Width ="71%" type="textarea" Columns="108" Rows="10" style="max-width :800px" 
                                ></asp:TextBox>
                        </div>  
                     </div> 
                     <div class="profile-column inter">
                        <div class="field" style="text-align :center">
                            <asp:Button ID="btnCarico" runat="server" Text="In Carico" Cssclass="button black" ></asp:Button>&nbsp;
                            <asp:Button ID="btRispondi" runat="server" Text="Rispondi" Cssclass="button black" BackColor ="Blue"  ></asp:Button>&nbsp;
                            <asp:Button ID="btnRispostaCli" runat="server" Text="RispostaCliente" Cssclass="button orange" BackColor ="Orange"  ></asp:Button>&nbsp;
                            <asp:Button ID="btnChiudi" runat="server" Text="Chiudi" Cssclass="button green " BackColor ="Green"  ></asp:Button>&nbsp;
                        </div> 
                    </div> 
                </asp:Panel>     
    </section>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>