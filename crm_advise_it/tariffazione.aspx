<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="tariffazione.aspx.vb" Inherits="crm_advise_it.tariffazione" MasterPageFile ="~/Site.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>


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
        <h1 class="page-title">Tipo Tariffazioni e Unità di Misura</h1>	           
            <section id="profilo-view" class="content-section">
                           <br /><br />
                <asp:Panel ID="Panel" runat="server">
                    <asp:Label ID="lblIdOperatore" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdRecapito" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                    <br />
                        <br />
                        <br />  
                        <div class="field">
                            <asp:Label ID="lblTipoTariffazione" runat="server" CssClass="label " Text="Tipo Tariffazione"></asp:Label>
                            <asp:DropDownList ID="ddlTipoTariffazione" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px" ></asp:DropDownList>  
                            <asp:ImageButton ID="imgElTipoTariffazione" runat="server" ImageUrl ="~/images/meno.png" Width="3%" Height ="3%" ToolTip ="Elimina" ImageAlign ="Middle"  ></asp:ImageButton>                            
                        </div>
                        <br />
                        <br />
                        <br />  
                        <div class="field">
                            <asp:Label ID="lblUnitàMisura" runat="server" CssClass="label " text="Unità di Misura"></asp:Label>
                            <asp:DropDownList ID="ddlUnitàMisura" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                               ></asp:DropDownList>
                            <asp:ImageButton ID="imgElUnitàMisura" runat="server" ImageUrl ="~/images/meno.png" Width="3%" Height ="3%" ToolTip ="Elimina" ImageAlign ="Middle"  ></asp:ImageButton>   
                        </div> 
                        <br />                       
                                
                            <br />
                        
                            
                    </div>
                    <div class="profile-column right">
                        <br />
                        <br />
                        <br /> 
                        <div class="field">
                            <asp:Label ID="lblTipoTariffazione2" runat="server" CssClass="label " text="Tipo Tariffazione"></asp:Label>
                            <asp:TextBox ID="txtTipoTariffazione" runat="server" class="input-text" name="tipotariffazione" type="text" placeholder="Inserisci nuova tariffazione"  />&nbsp;
                            <asp:ImageButton ID="imgTipoTariffazione" runat="server" ImageUrl ="~/images/piu.png" Width="3%" Height ="3%" ToolTip ="Aggiungi" ImageAlign ="Middle"  ></asp:ImageButton>
                        </div>
                        <br />
                        <br />
                        <br /> 
                        <div class="field">
                            <asp:Label ID="lblUnitaMisura2" runat="server" CssClass="label " text="Unità di Misura"></asp:Label>
                            <asp:TextBox ID="txtUnitaMisura" runat="server" class="input-text" name="unitamisura"  type="text" placeholder="Inserisci nuova unità di misura"/>&nbsp;
                            <asp:ImageButton ID="imgUnitaMisura" runat="server" ImageUrl ="~/images/piu.png" Width="3%" Height ="3%" ToolTip ="Aggiungi" ImageAlign ="Middle" ></asp:ImageButton>
                        </div>
                        <br />
                                 
                    </div>
                    <div class="clear"></div>
                              
                </asp:Panel>
                
            </section>
            
      




    </div>
    <%--</ContentTemplate>
  
    </asp:UpdatePanel>--%>

  
</asp:Content>
