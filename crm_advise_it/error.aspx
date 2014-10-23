<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="error.aspx.vb" Inherits="crm_advise_it._error" MasterPageFile ="~/Site.Master"  %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<link href="assets2/admin/pages/css/error.css" rel="stylesheet" type="text/css"/>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" >    			
			
        <!-- BEGIN PAGE HEADER-->
        <h3 class="page-title">
        Errore 404
        </h3>
       
        <!-- END PAGE HEADER-->
        <!-- BEGIN PAGE CONTENT-->
        <div class="row">
	        <div class="col-md-12 page-404">
		        <div class="number">
				        404
		        </div>
		        <div class="details">
			        <h3>Oops! Ti sei perso.</h3>
			        <p>
					        Non è stata trovata la pagina da te ricercata.<br/>
				        <a href="tickets.aspx">
				        Ritorna alla home </a>				        
			        </p>			        
		        </div>
	        </div>
        </div>
        <!-- END PAGE CONTENT-->

  </asp:Content> 