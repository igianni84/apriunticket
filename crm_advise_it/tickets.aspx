<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="tickets.aspx.vb" Inherits="crm_advise_it.tickets1" MasterPageFile ="~/Site.Master"  MaintainScrollPositionOnPostback ="true"  %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css"/>
<link href="assets2/global/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
<link href="assets2/global/plugins/simple-line-icons/simple-line-icons.min.css" rel="stylesheet" type="text/css"/>
<link href="assets2/global/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="assets2/global/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css"/>
<link href="assets2/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css" rel="stylesheet" type="text/css"/>
<!-- END GLOBAL MANDATORY STYLES -->
<!-- BEGIN PAGE LEVEL STYLES -->
<link rel="stylesheet" type="text/css" href="assets2/global/plugins/select2/select2.css"/>
<link rel="stylesheet" type="text/css" href="assets2/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css"/>
<!-- END PAGE LEVEL STYLES -->
<!-- BEGIN THEME STYLES -->
<link href="assets2/global/css/components.css" rel="stylesheet" type="text/css"/>
<link href="assets2/global/css/plugins.css" rel="stylesheet" type="text/css"/>
<link href="assets2/admin/layout/css/layout.css" rel="stylesheet" type="text/css"/>
<link id="style_color" href="assets2/admin/layout/css/themes/default.css" rel="stylesheet" type="text/css"/>
<link href="assets2/admin/layout/css/custom.css" rel="stylesheet" type="text/css"/>
<!-- END THEME STYLES -->
<link rel="shortcut icon" href="favicon.ico"/>--%>





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


                        function ShowPopupCancel(message) {

                            if (confirm("Vuoi eliminare il ticket n: " + message)) {
                                window.location = "tickets.aspx?id=" + message;
                            }
                            else {
                                window.location = "javascript:history.go(-1)";
                            }


                        };

                    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" onload="ScrollBanOK()" onunload="ScrollBanNO()">    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


  <%--<div class="col-md-6 col-sm-6">
				<div class="portlet box purple-wisteria">
					<div class="portlet-title">
						<div class="caption">
							<i class="fa fa-calendar"></i>General Stats
						</div>
						<div class="actions">
							<a href="javascript:;" class="btn btn-sm btn-default easy-pie-chart-reload">
							<i class="fa fa-repeat"></i> Reload </a>
						</div>
					</div>
					<div class="portlet-body">
						<div class="row">
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number transactions" data-percent="55">
										<span>
										+55 </span>
										%
									</div>
									<a class="title" href="#">
									Transactions <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
							<div class="margin-bottom-10 visible-sm">
							</div>
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number visits" data-percent="85">
										<span>
										+85 </span>
										%
									</div>
									<a class="title" href="#">
									New Visits <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
							<div class="margin-bottom-10 visible-sm">
							</div>
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number bounce" data-percent="46">
										<span>
										-46 </span>
										%
									</div>
									<a class="title" href="#">
									Bounce <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>


            <div class="row">
				<div class="col-md-12">
					<!-- BEGIN EXAMPLE TABLE PORTLET-->
					<div class="portlet box blue">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-edit"></i>Editable Table
							</div>
							<div class="tools">
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
						<div class="portlet-body">
							<div class="table-toolbar">
								<div class="row">
									<div class="col-md-6">
										<div class="btn-group">
											<button id="sample_editable_1_new" class="btn green">
											Add New <i class="fa fa-plus"></i>
											</button>
										</div>
									</div>
									<div class="col-md-6">
										<div class="btn-group pull-right">
											<button class="btn dropdown-toggle" data-toggle="dropdown">Tools <i class="fa fa-angle-down"></i>
											</button>
											<ul class="dropdown-menu pull-right">
												<li>
													<a href="#">
													Print </a>
												</li>
												<li>
													<a href="#">
													Save as PDF </a>
												</li>
												<li>
													<a href="#">
													Export to Excel </a>
												</li>
											</ul>
										</div>
									</div>
								</div>
							</div>
							<table class="table table-striped table-hover table-bordered" id="sample_editable_1">
							<thead>
							<tr>
								<th>
									 Username
								</th>
								<th>
									 Full Name
								</th>
								<th>
									 Points
								</th>
								<th>
									 Notes
								</th>
								<th>
									 Edit
								</th>
								<th>
									 Delete
								</th>
							</tr>
							</thead>
							<tbody>
							<tr>
								<td>
									 alex
								</td>
								<td>
									 Alex Nilson
								</td>
								<td>
									 1234
								</td>
								<td class="center">
									 power user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							<tr>
								<td>
									 lisa
								</td>
								<td>
									 Lisa Wong
								</td>
								<td>
									 434
								</td>
								<td class="center">
									 new user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							<tr>
								<td>
									 nick12
								</td>
								<td>
									 Nick Roberts
								</td>
								<td>
									 232
								</td>
								<td class="center">
									 power user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							<tr>
								<td>
									 goldweb
								</td>
								<td>
									 Sergio Jackson
								</td>
								<td>
									 132
								</td>
								<td class="center">
									 elite user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							<tr>
								<td>
									 webriver
								</td>
								<td>
									 Antonio Sanches
								</td>
								<td>
									 462
								</td>
								<td class="center">
									 new user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							<tr>
								<td>
									 gist124
								</td>
								<td>
									 Nick Roberts
								</td>
								<td>
									 62
								</td>
								<td class="center">
									 new user
								</td>
								<td>
									<a class="edit" href="javascript:;">
									Edit </a>
								</td>
								<td>
									<a class="delete" href="javascript:;">
									Delete </a>
								</td>
							</tr>
							</tbody>
							</table>
						</div>
					</div>
					<!-- END EXAMPLE TABLE PORTLET-->
				</div>
			</div>--%>





    
   

                    <div id="dialog" style="display: none">
                    </div>

                 

			<%--<div class="page-bar">
				<ul class="page-breadcrumb">
					<li>
						<i class="fa fa-home"></i>
						<a href="tickets.aspx">Home</a>
						<i class="fa fa-angle-right"></i>
					</li>					
				</ul>
				<div class="page-toolbar">
                    <div>
                        <asp:DropDownList ID="ddlCliente" runat="server" CssClass ="pull-right tooltips btn btn-fit-height grey-salt" style="width:200px; margin-bottom:14px; height:27px" AutoPostBack ="true"></asp:DropDownList>
                    </div>
					<div id="dashboard-report-range" class="pull-right tooltips btn btn-fit-height grey-salt" data-placement="top" data-original-title="Change dashboard date range">
						<i class="icon-calendar"></i>&nbsp; <span class="thin uppercase visible-lg-inline-block"></span>&nbsp; <i class="fa fa-angle-down"></i>
					</div>
				</div>
			</div>--%>

            <%--<div class="col-md-6 col-sm-6">
				<div class="portlet box purple-wisteria">
					<div class="portlet-title">
						<div class="caption">
							<i class="fa fa-calendar"></i>General Stats
						</div>
						<div class="actions">
							<a href="javascript:;" class="btn btn-sm btn-default easy-pie-chart-reload">
							<i class="fa fa-repeat"></i> Reload </a>
						</div>
					</div>
					<div class="portlet-body">
						<div class="row">
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number transactions" data-percent="55">
										<span>
										+55 </span>
										%
									</div>
									<a class="title" href="#">
									Transactions <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
							<div class="margin-bottom-10 visible-sm">
							</div>
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number visits" data-percent="85">
										<span>
										+85 </span>
										%
									</div>
									<a class="title" href="#">
									New Visits <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
							<div class="margin-bottom-10 visible-sm">
							</div>
							<div class="col-md-4">
								<div class="easy-pie-chart">
									<div class="number bounce" data-percent="46">
										<span>
										-46 </span>
										%
									</div>
									<a class="title" href="#">
									Bounce <i class="icon-arrow-right"></i>
									</a>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>--%>

            <div class="row ">
				<div class="col-md-6 col-sm-6" style="width:100%">
					<div class="portlet box purple-wisteria">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-calendar"></i>Riepilogo
							</div>
							<%--<div class="actions">
								<a href="javascript:;" class="btn btn-sm btn-default easy-pie-chart-reload">
								<i class="fa fa-repeat"></i> Reload </a>
							</div>--%>
						</div>
						<div class="portlet-body">
							<div class="row">
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    <% Dim a = CInt(lblaperti.Text) * 100 / CInt(lblTutti.Text)
                                        %>
										<div class="number aperto" data-percent="<%=a %>">
											<span>
                                                <asp:Label ID="lblAperti" runat="server" ></asp:Label>
                                             </span>
											
										</div>
										<a class="title" href="#">
                                            <asp:LinkButton ID="Label1" runat="server" Height="25px" Width="100px" Text="Aperti" ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
										</a>
									</div>
								</div>
								<div class="margin-bottom-10 visible-sm">
								</div>
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    <% Dim b = CInt(lblInCarico.Text) * 100 / CInt(lblTutti.Text)
                                        %>
										<div class="number carico" data-percent="<%=b %>">
											<span>											    
                                                <asp:Label ID="lblInCarico" runat="server" ></asp:Label> 
                                            </span>
											
										</div>
										<a class="title" href="#">
										<asp:LinkButton ID="Label2" runat="server" Text="In Carico" Height="25px" Width="100px"  ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
										</a>
									</div>
								</div>
								<div class="margin-bottom-10 visible-sm">
								</div>
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    <% Dim c = CInt(lblRisCliente.Text) * 100 / CInt(lblTutti.Text)
                                        %>
										<div class="number riscliente" data-percent="<%=c %>">
											<span>
											    <asp:Label ID="lblRisCliente" runat="server" ></asp:Label>
                                            </span>
										</div>
										<a class="title" href="#">
										<asp:LinkButton ID="Label3" runat="server" Text="Risp.Cliente" Height="25px" Width="100px" ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
                                                
										</a>
									</div>
								</div>
                                <div class="margin-bottom-10 visible-sm">
								</div>
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    <% Dim d = CInt(lblRisOperatore.Text) * 100 / CInt(lblTutti.Text)
                                        %>
										<div class="number risoperatore" data-percent="<%=d %>">
											<span>
											      <asp:Label ID="lblRisOperatore" runat="server" ></asp:Label>
                                            </span>
										</div>
										<a class="title" href="#">
										<asp:LinkButton ID="Label4" runat="server" Text="Risp.Operatore" Height="25px" Width="100px" ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
                                                
										</a>
									</div>
								</div>
                                   <div class="margin-bottom-10 visible-sm">
								</div>
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    
										<div class="number tutto" data-percent="100">
											<span>
											     <asp:Label ID="lblTutti" runat="server" ></asp:Label>               
                                            </span>
										</div>
										<a class="title" href="#">
										<asp:LinkButton ID="Label6" runat="server" Text="Tutti" Height="25px" Width="100px" ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
                                                                    
										</a>
									</div>
								</div>
                                <div class="margin-bottom-10 visible-sm">
								</div>
								<div class="col-md-4" style="width:16%">
									<div class="easy-pie-chart">
                                    <% Dim e = CInt(lblChiuso.Text) * 100 / CInt(lblTutti.Text)
                                        %>
										<div class="number chiuso" data-percent="<%=e %>">
											<span>
											        <asp:Label ID="lblChiuso" runat="server" ></asp:Label>
                                            </span>
										</div>
										<a class="title" href="#">
										<asp:LinkButton ID="Label5" runat="server" Text="Chiuso" Height="25px" Width="100px" ForeColor ="Black"></asp:LinkButton><%--&nbsp;<i class="icon-arrow-right"></i>--%>
                                                           
										</a>
									</div>
								</div>
                             
							</div>
						</div>
					</div>
				</div>
				
			</div>


            

			<div class="clearfix">
			</div>

             <div class="row">
				<div class="col-md-12">
					<!-- BEGIN EXAMPLE TABLE PORTLET-->
					<div >						
						<div class="portlet-body">	
                            <asp:ListView ID="ListView2" runat="server" GroupItemCount="1" Visible="true">
                                <LayoutTemplate>
                                <div runat="server" id="Products" class="mainLayout">
                                      <div runat="server" id="groupPlaceholder">
                                    </div>
                                </div>
                             </LayoutTemplate>
                              <GroupTemplate>
                                <div runat="server" id="ProductsGroup" class="group">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </div>
                            </GroupTemplate>
                            <GroupSeparatorTemplate>
                            </GroupSeparatorTemplate>
                              <ItemTemplate>
                          
                                <div id="Td1" align="center" runat="server" class="item" >  
                                        <asp:Button ID="Button3" runat="server" Text='<%# Eval("ragsoc1") %>'  ToolTip ='<%# Eval("id") &"-"& Eval("codice")%>' CssClass ="btn btn-primary" Width="180px" ></asp:Button>
                                        
                                  </div>
                              </ItemTemplate>
                          </asp:ListView>
                          
						</div>
					</div>
					<!-- END EXAMPLE TABLE PORTLET-->
				</div>
			</div>
             
            <div class="clearfix">
			</div>
            
            <asp:Panel ID="Panel1" runat="server" Visible ="true" style="margin-top :30px" >  
              <div class="row">
				<div class="col-md-12">
					<!-- BEGIN EXAMPLE TABLE PORTLET-->
					<div class="portlet box blue">
						<div class="portlet-title">
							<div class="caption">
								<i class="fa fa-edit"></i>Tickets  <asp:Label ID="lblClienteFil" Text="" runat="server" CssClass ="primary" style="margin-left :50px;" ></asp:Label>
							</div>
							<div class="tools">
								<a href="javascript:;" class="collapse">
								</a>								
								<a href="javascript:;" class="reload">
								</a>
								<a href="javascript:;" class="remove">
								</a>
							</div>
                             <div class="btn-group pull-right" style="height :10px">
											<button class="btn dropdown-toggle" title ="Tools" data-toggle="dropdown" >
                                                <asp:Label ID="Label7" runat="server" Text="Tools" ></asp:Label> <i class="fa fa-angle-down"></i>
											</button>
                                            
                                            
											<ul class="dropdown-menu pull-right">
												<li>
                                                    <asp:LinkButton ID="lbStampa" runat="server">Stampa</asp:LinkButton>
													<%--<a href="#">
													Print </a>--%>
												</li>
												<li>
													<asp:LinkButton ID="lbPdf" runat="server">Esporta PDF</asp:LinkButton>
												</li>
												<li>
													<asp:LinkButton ID="lbExcel" runat="server">Esporta Excel</asp:LinkButton>
												</li>
											</ul>
										</div>                            
						</div>
                       
                        <div class="portlet-body" >
                            <div class="btn-group">
                                <asp:Button ID="btnGiorno" runat="server" Text="Giorno"  CssClass ="btn btn-success" BackColor ="Coral"/>                                                        
						    </div>
                            <div class="btn-group">
                                <asp:Button ID="btnSettimana" runat="server" Text="Sett."  CssClass ="btn btn-success" BackColor ="Coral" />                                   
                            </div>
                            <div class="btn-group"  >
                                <asp:Button ID="btnMese" runat="server" Text="Mese"  CssClass ="btn btn-success" BackColor ="Coral"/>   
                            </div>  
                            <div class="btn-group" style="width :50px;"> 
                                <asp:TextBox ID="TextBox1" runat="server" class="form-control date-picker" Text ="Da" BorderColor="White"  /> 
                            </div> 
							<div class="btn-group" >           
                                <asp:TextBox ID="txtDataDa" runat="server" class="form-control date-picker"  ToolTip ="Da" name="datada" type="date" AutoPostBack ="true" />                        
                            </div> 
                            <div class="btn-group" style="width :50px">   
                                <asp:TextBox ID="TextBox2" runat="server" class="form-control date-picker" Text ="A" BorderColor="White" />   
                            </div>           
                            <div class="btn-group" >                                                     
                                <asp:TextBox ID="txtDataA" runat="server" class="form-control date-picker" ToolTip="A" name="dataa" type="date" AutoPostBack ="true" /> 							   
                            </div> 
                        </div>
						<div class="portlet-body">
							<%--<div class="table-toolbar" style="width:20px">
								<div class="row">
									<div class="col-md-6">
										<div class="btn-group">											 
                                            <asp:Button ID="Button9" runat="server" Text="Apri Ticket" CssClass="btn green" />  
                                        </div>
                                        
									</div>
									<div class="col-md-6">
                                    
										
									</div>
								</div>
							</div>--%>

                            

                            <asp:ListView ID="ListView1" runat="server" >
                            <LayoutTemplate >
							<table class="table table-striped table-hover" id="sample_editable_1" >
							<thead>
                                
							    <tr >
                                    <th >
                                        <asp:Label ID="lblTicket" runat="server" Text="N. Ticket" ></asp:Label><br />
                                        <asp:Label ID="lblStato" runat="server" Text="Stato"></asp:Label>
                                    </th>   
                                    <th >
                                        <asp:Label ID="lblOggetto" runat="server" Text="Oggetto"></asp:Label><br />
                                        <asp:Label ID="lblInventario" runat="server" Text="Inventario"></asp:Label>
                                    </th>               
                                    <th >
                                        <asp:Label ID="lblDataAp" runat="server" Text="Data Apertura"></asp:Label><br />
                                        <asp:Label ID="lblDataCh" runat="server" Text="Data Chiusura"></asp:Label>
                                    </th>                                                                                                                
                                    <th >
                                        <asp:Label ID="lblDataSca" runat="server" Text="Data Scadenza"></asp:Label><br />
                                        <asp:Label ID="lblDataUlt" runat="server" Text="Data Ultimo Stato"></asp:Label>
                                    </th>  

                                    <th>
                                        <asp:Label ID="lblCliente" runat="server" Text="Cliente"></asp:Label><br />
                                        <asp:Label ID="lblOperatore" runat="server" Text="Operatore"></asp:Label>
                                    </th> 
                                    <th >
                                        <asp:Label ID="lblSubCliente" runat="server" Text="SubCliente"></asp:Label><br />
                                        <asp:Label ID="lblUtente" runat="server" Text="Utente"></asp:Label>
                                    </th>
                                      
                                                                                                
                                    
                                </tr>

                                <%--<tr ID="itemPlaceholder" runat="server">
                                </tr>--%>
							</thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </tbody> 
							</table>
                             </LayoutTemplate>                             
                             <ItemTemplate>
                                <tr>
                                    <td style="  text-align :center ;background-color :<%# Eval("colore")%>">
                                        <asp:Button ID="btnGestisci" runat="server" Text='<%# Eval("id")%>' ToolTip='<%# eval("id") %>'  Cssclass="btn btn-link" ForeColor ="White"  >    
                                        <%--<asp:Label ID="lblId" runat="server" Text='<%# Eval("id")%>' ForeColor ="White" ></asp:Label><br />
                                        <asp:Label ID="lblStato" runat="server" Text='<%# Eval("stato")%>' ForeColor ="White"></asp:Label>--%>
                                        </asp:Button>
                                    </td> 
                                    <td >
                                        <asp:Label ID="lblOggetto" runat="server" Text='<%# Eval("oggetto")%>'></asp:Label><br />
                                        <asp:Label ID="lblTipoDispositivo" runat="server" Text='<%# Eval("tipodispositivo")%>'></asp:Label>
                                    </td>                         
                                    <td >
                                        <asp:Label ID="lblDataAp" runat="server" Text='<%# Eval("dataapertura")%>'></asp:Label><br />
                                        <asp:Label ID="lblDataCh" runat="server" Text='<%# Eval("datachiusura")%>'></asp:Label>
                                    </td>
                                    <td >
                                        <asp:Label ID="lbldataSca" runat="server" Text='<%# Eval("datascadenza")%>'></asp:Label><br />
                                        <asp:Label ID="lblDataUlt" runat="server" Text='<%# Eval("dataultimo")%>'></asp:Label>
                                    </td>    
                                    <td >
                                        <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("ragsoc")%>'></asp:Label><br />
                                        <asp:Label ID="lblOperatore" runat="server" Text='<%# Eval("nomecognomeop")%>'></asp:Label>
                                    </td>   
                                    <td >
                                        <asp:Label ID="lblSubCliente" runat="server" Text='<%# Eval("ragsocsub")%>'></asp:Label><br />
                                        <asp:Label ID="lblUtente" runat="server" Text='<%# Eval("nomecognomeut")%>'></asp:Label>
                                    </td>  
                                                                                                                          
                                    
                                </tr>   
                           
                           </ItemTemplate>
                           
                          </asp:ListView>

                            


                         

						</div>
					</div>
					<!-- END EXAMPLE TABLE PORTLET-->
				</div>
			</div> 
             </asp:Panel>
            

           
          

           
            <div class="clearfix">
			</div>

           <%-- <div class="row" >
				<div class="col-md-12" >
					<!-- BEGIN EXAMPLE TABLE PORTLET-->
					<div class="portlet box blue" >
						<div class="portlet-title" >
							<div class="caption">
								<i class="fa fa-edit"></i>Filtraggio Avanzato
							</div>
							<div class="tools">
								<a href="javascript:;" class="collapse">
								</a>								
								<a href="javascript:;" class="reload">
								</a>
								<a href="javascript:;" class="remove">
								</a>
							</div>
						</div>
						 
               




                            



                            

                        </div>
					</div>
					<!-- END EXAMPLE TABLE PORTLET-->
				</div>--%>
			
            


            <div class="clearfix">
			</div>

               
            <asp:Panel ID="Panel2" runat="server" Visible="false" >                                     
                  <div style ="margin-top :100px" >
                        <asp:Label ID="lblNon" runat="server" Text="Nessun Ticket Presente" Font-Size="30px" CssClass ="labellist"></asp:Label>
                </div>
                </asp:Panel>
            <asp:Label ID="lblIdTicket" runat="server" Text="-1" Visible="false" ></asp:Label>               
                

      
    </asp:Content> 

