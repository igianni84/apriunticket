<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dispositivo.aspx.vb" Inherits="crm_advise_it.dispositivo" MasterPageFile ="~/Site.Master" %>
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
        <h1 class="page-title">I Dispositivi</h1>	           
            <section id="profilo-view" class="content-section">
                           <br /><br />
                <asp:Panel ID="Panel" runat="server">
                    <asp:Label ID="lblIdOperatore" runat="server" Text="-1" Visible="false"></asp:Label>
                    <asp:Label ID="lblIdRecapito" runat="server" Text="-1" Visible="false"></asp:Label>
                    <div class="profile-column left">
                        <div class="field">
                            <asp:Label ID="lblTipoDispositivo" runat="server" CssClass="label " Text="Tipo Dispositivo"></asp:Label>
                            <asp:DropDownList ID="ddlTipoDispositivo" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px" AutoPostBack="True"></asp:DropDownList>  
                            <asp:ImageButton ID="imgElTipoDispositivo" runat="server" ImageUrl ="~/images/meno.png" Width="3%" Height ="3%" ToolTip ="Elimina" ImageAlign ="Middle"  ></asp:ImageButton>                            
                        </div>
                        <br />  
                        <div class="field">
                            <asp:Label ID="lblMarchio" runat="server" CssClass="label " text="Marchio"></asp:Label>
                            <asp:DropDownList ID="ddlMarchio" runat="server" class="input-drop" style="width:220px;margin-bottom:14px;height:27px"                                
                                AutoPostBack="True"></asp:DropDownList>
                            <asp:ImageButton ID="imgElMarchio" runat="server" ImageUrl ="~/images/meno.png" Width="3%" Height ="3%" ToolTip ="Elimina" ImageAlign ="Middle"  ></asp:ImageButton>   
                        </div> 
                        <br />                       
                        <div class="field">
                            <asp:Label ID="lblModello" runat="server" CssClass="label " Text="Modello"></asp:Label>
                            <asp:DropDownList ID="ddlModello" runat="server" class="input-drop" 
                                style="width:220px;margin-bottom:14px;height:27px"></asp:DropDownList>
                            <asp:ImageButton ID="imgElModello" runat="server" ImageUrl ="~/images/meno.png" Width="3%" Height ="3%" ToolTip ="Elimina" ImageAlign ="Middle"  ></asp:ImageButton>      
                        </div>             
                            <br />
                        
                            
                    </div>
                    <div class="profile-column right">
                        
                        <div class="field">
                            <asp:Label ID="lblTipoDispositivo2" runat="server" CssClass="label " text="Tipo Dispositivo"></asp:Label>
                            <asp:TextBox ID="txtTipoDispositivo" runat="server" class="input-text" name="tipodispositivo" type="text" placeholder="Inserisci nuovo dispositivo"  />&nbsp;
                            <asp:ImageButton ID="imgTipoDispositivo" runat="server" ImageUrl ="~/images/piu.png" Width="3%" Height ="3%" ToolTip ="Aggiungi" ImageAlign ="Middle"  ></asp:ImageButton>
                        </div>
                        <br />
                        <div class="field">
                            <asp:Label ID="lblMarchio2" runat="server" CssClass="label " text="Marchio"></asp:Label>
                            <asp:TextBox ID="txtMarchio" runat="server" class="input-text" name="marchio"  type="text" placeholder="Inserisci nuovo marchio"/>&nbsp;
                            <asp:ImageButton ID="imgMarchio" runat="server" ImageUrl ="~/images/piu.png" Width="3%" Height ="3%" ToolTip ="Aggiungi" ImageAlign ="Middle" ></asp:ImageButton>
                        </div>
                        <br />
                        <div class="field">
                            <asp:Label ID="lblModello2" runat="server" CssClass="label " Text="Modello"></asp:Label>
                            <asp:TextBox ID="txtModello" runat="server" class="input-text" name="modello"  type="text" placeholder="Inserisci nuovo modello"/>&nbsp;
                            <asp:ImageButton ID="imgModello" runat="server" ImageUrl ="~/images/piu.png" Width="3%" Height ="3%" ToolTip ="Aggiungi" ImageAlign ="Middle" ></asp:ImageButton>
                            
                        </div> 
                        
                                                                
                        
                        
                    </div>
                    <div class="clear"></div>
                    <div>
                        <asp:Panel ID="Panel1" runat="server" visible="true" >
                    <h1 class="barra">Dispositivi Presenti</h1>
                    <div class="csslistview ">
                                <asp:ListView ID="ListView1" runat="server">
                                    <LayoutTemplate>
                                        <table class="content-table" >
                                            <thead>
                                                <tr ID="Tr11" runat="server" >
                                                    <th ID="Td111" runat="server" align="center" class="a-left" >
                                                        <asp:Label ID="lblDispositivo1" runat="server" Text="Dispositivo" ></asp:Label>
                                                    </th>
                                                                                            
                                                    <th ID="Th7" runat="server" align="center">
                                                        <asp:Label ID="lblMarchio1" runat="server" Text="Marchio"></asp:Label>
                                                    </th>
                                                    <th ID="Td3" runat="server" align="center">
                                                        <asp:Label ID="lblModello1" runat="server" Text="Modello"></asp:Label>
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
                                                    <asp:Label ID="lblDispositivo1" runat="server" Text='<%# Eval("tipodispositivo")%>'></asp:Label>
                                                </td>
                                                
                                                <td>
                                                    <asp:Label ID="lblMarchio1" runat="server" Text='<%# Eval("marchio")%>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblModello1" runat="server" Text='<%# Eval("modello")%>'></asp:Label>
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
                    </div>          
                </asp:Panel>
                
            </section>
            
      




    </div>
    <%--</ContentTemplate>
  
    </asp:UpdatePanel>--%>

  
</asp:Content>
