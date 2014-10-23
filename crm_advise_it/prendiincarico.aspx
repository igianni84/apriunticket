<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="prendiincarico.aspx.vb" Inherits="crm_advise_it.prendiincarico" MasterPageFile ="~/Site.Master"  %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"> 
<meta content='4;url=/tickets.aspx' http-equiv='Refresh'>
<asp:ScriptManager ID ="ScripManager1" runat ="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">    
        <ContentTemplate>

                <div id="content" >
	                <div id="login-area">        
		                <span class="image"></span>
		                <div class="container">
			                <div class="intro" >
				                <h1 class="title">ApriUnTicket.it</h1>
				                <p>Soluzioni per la tua azienda</p>
			                </div>
                            <div class="login" >
                                    <asp:Panel ID="Panel2" runat="server" >
                                    <h1 class="section-title">Prendi In Carico Ticket</h1>
                                    
                                        <div class ="field">
                                            <asp:Image ID="imgOk" runat="server" AlternateText ="OK" ImageUrl ="~/images/V.png" width="20%" ImageAlign ="Middle"    /><br />
                                            <asp:Label ID="lblOk" CssClass ="order-status confirmed" runat="server" Text="Ticket preso in carico. </br> Saranno avvisati tutti gli utenti coinvolti"  ></asp:Label>
                                            <asp:Label ID="lblIdTickets" runat="server" visible="false" ></asp:Label>
                                        </div> 
                                    
                                    </asp:Panel>               
                            </div> 
                        </div> 
                        <div class="clear"></div>		
	                </div>
                </div>
             </ContentTemplate>                
    </asp:UpdatePanel>    
</asp:Content>
