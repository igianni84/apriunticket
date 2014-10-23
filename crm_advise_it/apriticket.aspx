<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="apriticket.aspx.vb" Inherits="crm_advise_it.apriticket" MasterPageFile ="~/Layout.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>    
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">



    <%--<script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);


        function endRequestHandler(sender, args) {
            tinyMCE.idCounter = 0;
            tinyMCE.execCommand('mceAddControl', false, 'htmlContent');
        }


        function beginRequestHandler(sender, args) {
        }


        function UpdateTextArea() {
            tinyMCE.triggerSave(false, true);
        }
</script>--%>
   <%-- <script type ="text/javascript" src ="js/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    
    <script type="text/javascript">
        tinyMCE.init({            
            mode: "textareas",
            theme: "advanced",
            plugins: "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",
            theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_resizing: true,
            
//            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,|,sub,sup,|,charmap,emotions,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
//            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
//            theme_advanced_toolbar_location: "top",
//            theme_advanced_toolbar_align: "left",
//            theme_advanced_statusbar_location: "bottom",
//            

            content_css: "js/tinymce/examples/css/content.css",

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
    </script>--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContentTick">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
    
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
                        function ShowPopupRedirect(message) {
                            $(function () {
                                $("#dialog").html(message);
                                $("#dialog").dialog({
                                    title: "ApriUnTicket.it",
                                    buttons: {
                                        Close: function () {
                                            top.location.href = "tickets.aspx";
                                            //                                            $(this).dialog('close');
                                        }
                                    },
                                    modal: true
                                });
                            });
                        };
                        
                    </script>
   

                    

                    
                       
                    <!-- BEGIN PAGE CONTENT-->

			<div class="row">
				<div class="col-md-12">
					<div class="portlet box blue" id="form_wizard_1">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-gift"></i> Apri Ticket - <span class="step-title">
								Step 1 of 4 </span>
							</div>
							<div class="tools hidden-xs">
								<a href="javascript:;" class="collapse">
								</a>
								<a href="#portlet-config" data-toggle="modal" class="config">
								</a>
								<a href="javascript:;" class="reload">
								</a>
								<a href="javascript:;" class="remove">
								</a>
							</div>
						</div>
						<div class="portlet-body form">
                            <div class="form-horizontal form-bordered">
                                <div class="form-body">
							        <form action="#" runat="server"  class="form-horizontal" id="submit_form" method="POST">
                                    
                                    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
                                        <div id="dialog" style="display: none">
                                        </div>
                                        <div class="form-group">
										    <label class="control-label col-md-3">Data Apertura</label>
										    <div class="col-md-3">
                                                <asp:TextBox ID="txtDataAp" runat="server" class="form-control form-control-inline input-medium date-picker" type="date" name="dataap" />     
										    </div>
									    </div>                
                                        <div class="form-group">
										    <label class="control-label col-md-3">24hr Orario Apertura</label>
										    <div class="col-md-3">
											    <div class="input-group">
												    <asp:TextBox ID="txtOraAp" runat="server" class="form-control timepicker timepicker-24" name="orapa" type="time" />
                            					    <span class="input-group-btn">
												    <button class="btn default" type="button"><i class="fa fa-clock-o"></i></button>
												    </span>
											    </div>
										    </div>
									    </div>
                                        
                                        <div class="form-wizard">
									<div class="form-body">
										<ul class="nav nav-pills nav-justified steps">
											<li>
												<a href="#tab1" data-toggle="tab" class="step">
												<span class="number">
												1 </span>
												<span class="desc">
												<i class="fa fa-check"></i> Account Setup </span>
												</a>
											</li>
											<li>
												<a href="#tab2" data-toggle="tab" class="step">
												<span class="number">
												2 </span>
												<span class="desc">
												<i class="fa fa-check"></i> Profile Setup </span>
												</a>
											</li>
											<li>
												<a href="#tab3" data-toggle="tab" class="step active">
												<span class="number">
												3 </span>
												<span class="desc">
												<i class="fa fa-check"></i> Billing Setup </span>
												</a>
											</li>
											<li>
												<a href="#tab4" data-toggle="tab" class="step">
												<span class="number">
												4 </span>
												<span class="desc">
												<i class="fa fa-check"></i> Confirm </span>
												</a>
											</li>
										</ul>
										<div id="bar" class="progress progress-striped" role="progressbar">
											<div class="progress-bar progress-bar-success">
											</div>
										</div>
										<div class="tab-content">
											<div class="alert alert-danger display-none">
												<button class="close" data-dismiss="alert"></button>
												Ci sono degli errori. Per favore controllare.
											</div>
											<div class="alert alert-success display-none">
												<button class="close" data-dismiss="alert"></button>
												I dati inseriti sono corretti!
											</div>
											<div class="tab-pane active" id="tab1">
                                               
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >                    
                                            <ContentTemplate >
												<h3 class="block">Inserisci il dettaglio per l'apertura ticket</h3>
												<div class="form-group">
													<asp:Label ID="lblCliente" runat="server" CssClass="control-label col-md-3 " >Cliente
                                                    <span class="required">
													* </span>
													</asp:Label>
													<div class="col-md-4">
														 <asp:DropDownList ID="ddlCliente" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                                                        <a href="cliente.aspx" target ="_blank" ><asp:Image ID="imgPiuCliente" runat="server" width="4%" ToolTip ="Aggiungi Cliente" ImageAlign ="Bottom"     ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d"/></a>
                                                        <asp:ImageButton ID="imgRefreshCliente" runat="server" width="3%" ImageAlign ="AbsMiddle"  ToolTip ="Aggiorna Cliente" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919"/>
                             
													</div>
												</div>
                                                <div class="form-group">
                                                 <asp:Label ID="lblSubCliente" runat="server" CssClass="control-label col-md-3 " >Sub - Cliente
                           
													<%-- <span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
														  <asp:DropDownList ID="ddlSubCliente" runat="server" class="form-control" AutoPostBack="True" ></asp:DropDownList>
                                                        
                                                        <a href="subcliente.aspx" target ="_blank" ><asp:Image ID="imgPiuSubCliente" runat="server" width="4%" ToolTip ="Aggiungi SubCliente" ImageAlign ="Bottom" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d"/></a>
                                                        <asp:ImageButton ID="imgRefreshSubCliente" runat="server" width="3%" ImageAlign ="AbsMiddle" ToolTip ="Aggiorna SubCliente" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919"/>
                                                        <asp:Image ID="imgAppPiuSubCliente" runat="server" width="4%" Visible="false"   />
                                                        <asp:Image ID="imgAppRefreshSubCliente" runat="server" width="3%" Visible="false" />
                       
													</div>
												</div>
                                                <div class="form-group">
                                                 <asp:Label ID="lblUtente" runat="server" CssClass="control-label col-md-3 " >Utente
                           
													<%-- <span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
														   <asp:DropDownList ID="ddlUtente" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList> 
                                                        <a href="utente.aspx" target ="_blank" ><asp:Image ID="imgPiuUtente" runat="server" width="4%" ToolTip ="Aggiungi Utente" ImageAlign ="Bottom" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d"/></a>
                                                        <asp:ImageButton ID="imgRefreshUtente" runat="server" width="3%" ImageAlign ="AbsMiddle" ToolTip ="Aggiorna Utente" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919"/>
                                                        <asp:Image ID="imgAppPiuUtente" runat="server" width="4%" Visible="false"   />
                                                        <asp:Image ID="imgAppRefreshUtente" runat="server" width="3%" Visible="false" />                           
                               
													</div>
												</div>
                                                <div class="form-group">
                                                 <asp:Label ID="lblPerContoDi" runat="server" CssClass="control-label col-md-3 " >Per Conto Di
													<%-- <span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
														   <asp:DropDownList ID="ddlPerContoDi" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>
                                                        
                                                         <asp:Image ID="imgPiuPerContoDi" runat="server" width="4%"  />
                                                         <asp:Image ID="imgRefreshPerContoDi" runat="server" width="3%"  /> 
                               

                       
													</div>
												</div>
												<div class="form-group">
                                                    <asp:Label ID="lblContratto" runat="server" CssClass="control-label col-md-3 " >Contratto
                            
													<%--<span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
														<asp:DropDownList ID="ddlContratto" runat="server" class="form-control" AutoPostBack="True"></asp:DropDownList>  
                                                        <a href="contratto.aspx" target ="_blank" ><asp:Image ID="imgPiuContratto" runat="server" width="4%" ImageAlign ="Bottom" ToolTip ="Aggiungi Contratto" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d"/></a>
                                                        <asp:ImageButton ID="imgRefreshContratto" runat="server" width="3%" ImageAlign ="AbsMiddle" ToolTip ="Aggiorna Contratto" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919"/>
                                  
														
													</div>
												</div>
                                                <div class="form-group">
                                                     <asp:Label ID="lblListino" runat="server" CssClass="control-label col-md-3 " Text="Listino" Visible="false">
                                                   
													
													</asp:Label>
													<div class="col-md-4">
														<asp:DropDownList ID="ddlListino" runat="server" class="form-control" AutoPostBack="True"  Visible="false"></asp:DropDownList>   
                                                        <a href="listino.aspx" target ="_blank" ><asp:Image ID="imgPiuListino" runat="server" width="4%" ToolTip ="Aggiungi Listino" ImageAlign ="Bottom" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d" Visible ="false"/></a>
                                                        <asp:ImageButton ID="imgRefreshListino" runat="server" width="3%" ImageAlign ="AbsMiddle" ToolTip ="Aggiorna Listino" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919" Visible ="false"/>
                                       
														
													</div>
												</div>
												<div class="form-group">
													<asp:Label ID="lblScadenza" runat="server" CssClass ="control-label col-md-3" >Scadenza
                            
                                                     <%--<span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
														<asp:TextBox ID="txtScadenza" runat="server" CssClass ="form-control" Enabled="false"  ></asp:TextBox> 
                                                        <asp:Image ID="imgPiuScadenza" runat="server" width="4%"  />
                                                        <asp:Image ID="imgRefreshScadenza" runat="server" width="3%"  />
														
													</div>
												</div>
												<div class="form-group">
                                                     <asp:Label ID="lblSoglia" runat="server" CssClass="control-label col-md-3 " Text="Soglia" >
                            
													</asp:Label>
													<div class="col-md-4">
                                                        <asp:DropDownList ID="ddlSoglia" runat="server" class="form-control" visible="false" ></asp:DropDownList>
                                                        
														
                                                        <table id="table" runat="server"  ></table> 
													</div>
												</div>
                                                </ContentTemplate> 
                                            </asp:UpdatePanel> 
											</div>
                                           
											<div class="tab-pane" id="tab2">
                                             <asp:UpdatePanel ID="UpdatePanel2" runat="server"  >                    
                                            <ContentTemplate >
												<h3 class="block">Provide your profile details</h3>
												<div class="form-group">
                                                     <asp:Label ID="lblInventario" runat="server" CssClass="control-label col-md-3 " >Inventario
                                                     <%--<span class="required">
													* </span>--%>
													</asp:Label>
													<div class="col-md-4">
                                                        <asp:DropDownList ID="ddlInventario" runat="server" class="form-control" AutoPostBack="True" ></asp:DropDownList>
														<a href="inventario.aspx" target ="_blank" ><asp:Image ID="imgPiuInventario" runat="server" width="4%" ToolTip ="Aggiungi Inventario" ImageAlign ="Bottom" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAJOgAACToB8GSSSgAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAACsSURBVDiNpZNhCsIwDIW/6NhBegFFQfB4/vQ6O4Q/Boon2EFEef4wkxa6btpAIOWlr3lJapKosVXV7TkCMwtmFooMkrIOBODpHqbyShXsgLX74R8Jj2Lpbk18cL3ja9sIOprZGPeShi+S0awZT3pSPUaLF8kl7Pno3wAnh87ABWiBWywh6YEDg5PF0F1Sl6ugJKFdIqEpYFfg5XE/lWSlzzSucTK2XwiWWPUY32GcZ0YvTRx9AAAAAElFTkSuQmCCa78fcc7da7d6fb0bbce5911ae678c80d"/></a>
                                                        <asp:ImageButton ID="imgRefreshInventario" runat="server" width="3%" ImageAlign ="AbsMiddle" ToolTip ="Aggiorna Inventario" ImageUrl ="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAAzklEQVR4AYXRsUpCYRzG4UdDaGo8u3kBLdJmQzo5SbcQgjcQ6GZTXUI4NBW0eAtRNDgEQVtTQ6MXIGbn8O9ACR9n8fmt7/bWbB0aOSpbeyp79CXRMLESSbmhxFwozHRlmoYe/AiXmMJYWGpLDXwLr4KWwkZHVV8uRN2Zujsvqo7tAQuhp2oq/qrZoW6n+O9NhlRPWGwHzw5U3QoX20FfVcdGoUX4ENYGUm1LYQzXaq6E3L1zTZmumUKYS4zkImllogHp3adOyva9l934BH4BRiJYE7w0PeIAAAAASUVORK5CYIIf53c1ced0f7bc2e6df264e1b599f6919"/>
                    
														
													</div>
                                                     
                                                </div> 
                                                <div class="form-group">
                                                    <div class="col-md-4">


                                                        <asp:Label ID="Label9" runat="server" Text="Dispositivo" CssClass="control-label col-md-3 " Visible="false"  ></asp:Label> 
                                                        <asp:TextBox  ID="lblDispositivo" runat="server" CssClass="form-control " Enabled ="false" Visible ="false"  ></asp:TextBox>
                                      
                
                                                        <asp:Label ID="Label10" runat="server" Text="Marchio" CssClass="control-label col-md-3 " Visible="false" ></asp:Label>
                                                        <asp:TextBox ID="lblMarchio" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                
                                                        <asp:Label ID="Label11" runat="server" Text="Modello" CssClass="control-label col-md-3 " Visible="false" ></asp:Label>
                                                        <asp:TextBox ID="lblModello" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                                                                                                                            
												    </div>
                                                     <div class="col-md-4">

                                                         <asp:Label ID="Label12" runat="server" Text="Seriale" CssClass="control-label col-md-3 " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblSeriale" runat="server" CssClass="form-control" Enabled ="false" Visible ="false" ></asp:TextBox>
               
                                                         <asp:Label ID="Label13" runat="server" Text="Ubicazione" CssClass="control-label col-md-3 " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblUbicazione" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                
                                                        <asp:Label ID="Label14" runat="server" Text="Utente" CssClass="control-label col-md-3 " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblUtenteInv" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
               
                                                       
                                                     </div> 
                                                      <div class="col-md-4">
                                                        <asp:Label ID="Label15" runat="server" Text="Forn Org" CssClass="control-label col-md-3 " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblFornOrg" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                                     
                                                        <asp:Label ID="Label16" runat="server" Text="Forn Cliente" CssClass="control-label col-md-3 " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblFornCli" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                
                                                      </div> 
                                                </div> 
												<div class="form-group">
                                                     <asp:Label ID="lblInventario1" runat="server" CssClass="control-label col-md-3 ">Altro Inventario
                                                     <span class="required">
													* </span>
													</asp:Label>
													<div class="col-md-4">
                                                       <asp:DropDownList ID="ddlInventario1" runat="server" class="form-control" AutoPostBack="True" ></asp:DropDownList>
														
													</div>

                                                   
                                                </div>
                                                <div class="form-group">
                                                     <div class="col-md-4">              
                                                        <asp:Label ID="Label1" runat="server" Text="Dispositivo" CssClass="control-label col-md-3   " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblDispositivo1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                    
                                                        <asp:Label ID="Label2" runat="server" Text="Marchio" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblMarchio1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
               
                                                        <asp:Label ID="Label3" runat="server" Text="Modello" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblModello1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
               
												    </div>
                                                    <div class="col-md-4"> 
                                                         <asp:Label ID="Label4" runat="server" Text="Seriale" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblSeriale1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                
                                                        <asp:Label ID="Label5" runat="server" Text="Ubicazione" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblUbicazione1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                
                                                        <asp:Label ID="Label6" runat="server" Text="Utente" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblUtenteInv1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                  
                                                    </div> 
                                                    <div class="col-md-4">  
                                                         <asp:Label ID="Label7" runat="server" Text="Forn Org" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblFornOrg1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
               
                                                        <asp:Label ID="Label8" runat="server" Text="Forn Cliente" CssClass="control-label col-md-3  " Visible="false"></asp:Label>
                                                        <asp:TextBox ID="lblFornCli1" runat="server" CssClass="form-control" Enabled ="false" Visible ="false"></asp:TextBox>
                 
                                                    </div> 
                                                </div>  
                                             </ContentTemplate> 
                                            </asp:UpdatePanel> 
											</div>
                                           
											<div class="tab-pane" id="tab3">
												<h3 class="block">Provide your billing and credit card details</h3>
												<div class="form-group">
													<asp:Label ID="lblOggetto" runat="server" CssClass="control-label col-md-3" >Oggetto
                                                    
                                                    <span class="required">
													* </span>
													</asp:Label>
													<div class="col-md-4">
														 <asp:TextBox ID="txtOggetto" runat="server" CssClass="form-control" ></asp:TextBox>
                      
														
													</div>
												</div>
												
												<div class="form-group">
                                                    <div class="col-md-4">
													    <asp:CheckBox ID="cbxStealth" runat="server" checked ="false" Text="Stealth"/>
			                                        </div> 
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblBloccante" runat="server" CssClass="label " text="Bloccante"></asp:Label>
                                                        <asp:RadioButton ID="rbUtente" runat="server" CssClass =" radio  " GroupName ="bloccante" data-rif="login-field-utente" checked ="true" Text ="Per Utente" />                        
					                                    <asp:RadioButton ID="rbTutti" runat="server" CssClass =" radio  " GroupName ="bloccante" data-rif="login-field-tutti" checked ="false" Text ="Per Tutti" />                     
					                                </div> 
                                                     <div class="col-md-4">
                                                        <asp:Label ID="lblGuasto" runat="server" CssClass="label " text="Guasto"></asp:Label>
                                                        <asp:RadioButton ID="rbSporadico" runat="server" CssClass ="radio " GroupName ="guasto" data-rif="login-field-sporadico" checked ="true" Text ="Sporadico" />                   
					                                    <asp:RadioButton ID="rbFisso" runat="server" CssClass =" radio  " GroupName ="guasto" data-rif="login-field-fisso" checked ="false" Text ="Fisso" />                        
					        
                                                     </div> 
												</div>
												<div class="form-group">
                                                    <asp:Label ID="lblDescrizione" runat="server" CssClass="control-label col-md-3" >Descrizione                            
													<span class="required">
													* </span>
													</asp:Label>
													<div class="col-md-4">
                                                        <asp:TextBox ID="txtDescrizione" runat="server" CssClass="form-control" TextMode ="MultiLine" type="textarea"  ></asp:TextBox>
														<span class="help-block">
														</span>
													</div>
												</div>
											</div>
											<div class="tab-pane" id="tab4">
												<h3 class="block">Confirm your account</h3>
												<h4 class="form-section">Account</h4>
												<div class="form-group">
													<label class="control-label col-md-3">Username:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="username">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Email:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="email">
														</p>
													</div>
												</div>
												<h4 class="form-section">Profile</h4>
												<div class="form-group">
													<label class="control-label col-md-3">Fullname:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="fullname">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Gender:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="gender">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Phone:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="phone">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Address:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="address">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">City/Town:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="city">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Country:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="country">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Remarks:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="remarks">
														</p>
													</div>
												</div>
												<h4 class="form-section">Billing</h4>
												<div class="form-group">
													<label class="control-label col-md-3">Card Holder Name:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="card_name">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Card Number:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="card_number">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">CVC:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="card_cvc">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Expiration:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="card_expiry_date">
														</p>
													</div>
												</div>
												<div class="form-group">
													<label class="control-label col-md-3">Payment Options:</label>
													<div class="col-md-4">
														<p class="form-control-static" data-display="payment">
														</p>
													</div>
												</div>
											</div>
										</div>
									</div>
									<div class="form-actions">
										<div class="row">
											<div class="col-md-offset-3 col-md-9">
												<a href="javascript:;" class="btn default button-previous">
												<i class="m-icon-swapleft"></i> Back </a>
												<a href="javascript:;" class="btn blue button-next">
												Continue <i class="m-icon-swapright m-icon-white"></i>
												</a>
												<%--<a href="javascript:;" class="btn green button-submit">
												Submit <i class="m-icon-swapright m-icon-white"></i>
												</a>--%>
                                                <asp:Button ID="btnApri" runat="server" Text="Apri Ticket" Cssclass="btn green" ></asp:Button>  
                   
											</div>
										</div>
									</div>
								</div>
							        </form>
						    </div>
					    </div>
                    </div> 
				</div>
			</div>
			<!-- END PAGE CONTENT-->

            </div>


        </asp:Content>
        

       







       
    


