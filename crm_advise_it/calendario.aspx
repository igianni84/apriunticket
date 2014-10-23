<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="calendario.aspx.vb" Inherits="crm_advise_it.calendario" MasterPageFile ="~/Site.Master"  %>

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
    <div class="container">
    <h1 class="page-title">Giornata Lavorativa</h1>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server"  >                    
       <ContentTemplate >       
        <%--<section id="profilo-view" class="content-section">   --%>     
         <div class="profile-column inter"> 
                 <div class="field" >
                     <asp:Label ID="lblOrarioAp" runat="server" CssClass="labellist2 " Text="Apertura" style="padding-left :38%"></asp:Label>
                     <asp:Label ID="lblPausain" runat="server" CssClass="labellist2 " Text="In.Pausa"></asp:Label>
                     <asp:Label ID="lblPausafi" runat="server" CssClass="labellist2 " Text="Fine Pausa"></asp:Label>
                     <asp:Label ID="lblOrarioCh" runat="server" CssClass="labellist2 " Text="Chiusura"></asp:Label>  
                                            
                 </div>                       
		        <div class="field" >
                    <asp:CheckBox ID="cbxLunedi" runat="server" text="Lunedì" ToolTip ="Seleziona i giorni da inserire" ></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApLun" runat="server" class="input-text" name="orain" type="time"  Width="10%" ToolTip ="Orario Apertura"  />                           
                    <asp:TextBox ID="txtPausaInLun" runat="server" class="input-text" name="pausain" type="time"  Width="10%" ToolTip ="Inizio Pausa" />
                    <asp:TextBox ID="txtPausaFiLun" runat="server" class="input-text" name="pausafi" type="time"  Width="10%" ToolTip ="Fine Pausa" />
                    <asp:TextBox ID="txtOrarioChLun" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    &nbsp;<asp:Button ID="btnReplica" runat="server" CssClass ="button black" Width="20%" Text="Replica" />
        
                </div>                 
                <div class="field" >
                    <asp:CheckBox ID="cbxMartedi" runat="server" text="Martedì" ToolTip ="Seleziona i giorni da inserire" ></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApMar" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura" />                           
                    <asp:TextBox ID="txtPausaInMar" runat="server" class="input-text" name="pausain" type="time"  Width="10%" ToolTip ="Inizio Pausa" /> 
                    <asp:TextBox ID="txtPausaFiMar" runat="server" class="input-text" name="pausafi" type="time"  Width="10%" ToolTip ="Fine Pausa" />                                                     
                    <asp:TextBox ID="txtOrarioChMar" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                          
                    
                </div> 
                <div class="field" >
                    <asp:CheckBox ID="cbxMercoledi" runat="server" text="Mercoledì" ToolTip ="Seleziona i giorni da inserire"></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApMer" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura"/>                           
                    <asp:TextBox ID="txtPausaInMer" runat="server" class="input-text" name="pausain" type="time"  Width="10%" ToolTip ="Inizio Pausa" />                           
                    <asp:TextBox ID="txtPausaFiMer" runat="server" class="input-text" name="pausafi" type="time"  Width="10%" ToolTip ="Fine Pausa" />                           
                    <asp:TextBox ID="txtOrarioChMer" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    
                </div> 
                <div class="field" >
                    <asp:CheckBox ID="cbxGiovedi" runat="server" text="Giovedì" ToolTip ="Seleziona i giorni da inserire"></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApGio" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura" />                           
                    <asp:TextBox ID="txtPausaInGio" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Inizio Pausa" />                           
                    <asp:TextBox ID="txtPausaFiGio" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Fine Pausa" />                           
                    <asp:TextBox ID="txtOrarioChGio" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    
                </div>      
                <div class="field" >
                    <asp:CheckBox ID="cbxVenerdi" runat="server" text="Venerdì" ToolTip ="Seleziona i giorni da inserire"></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApVen" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura" />                           
                    <asp:TextBox ID="txtPausaInVen" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Inizio Pausa" />                           
                    <asp:TextBox ID="txtPausaFiVen" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Fine Pausa" />                           
                    <asp:TextBox ID="txtOrarioChVen" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    
                </div> 
                <div class="field" >
                    <asp:CheckBox ID="cbxSabato" runat="server" text="Sabato" ToolTip ="Seleziona i giorni da inserire"></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApSab" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura" />                           
                    <asp:TextBox ID="txtPausaInSab" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Inizio Pausa" />                           
                    <asp:TextBox ID="txtPausaFiSab" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Fine Pausa" />                           
                    <asp:TextBox ID="txtOrarioChSab" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    
                </div> 
                <div class="field" >
                    <asp:CheckBox ID="cbxDomenica" runat="server" text="Domenica" ToolTip ="Seleziona i giorni da inserire"></asp:CheckBox>              
                    <asp:TextBox ID="txtOrarioApDom" runat="server" class="input-text" name="orain" type="time"  Width="10%"  ToolTip ="Orario Apertura" />                           
                    <asp:TextBox ID="txtPausaInDom" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Inizio Pausa" />                           
                    <asp:TextBox ID="txtPausaFiDom" runat="server" class="input-text" name="pausa" type="time"  Width="10%" ToolTip ="Fine Pausa" />                           
                    <asp:TextBox ID="txtOrarioChDom" runat="server" class="input-text" name="orafine" type="time"  Width="10%" ToolTip ="Orario Chiusura" />                           
                    
                </div> 

                <div class="field">
                    <asp:Button ID="btnMemorizza" runat="server" Text="Memorizza" Cssclass="button black" ></asp:Button>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" Cssclass="button black" ></asp:Button>
                </div>
        <%--</section> --%>
        </div> 
        
       </ContentTemplate> 
    </asp:UpdatePanel>
    </div>
</asp:Content>