<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="collaudo.aspx.vb" Inherits="crm_advise_it.collaudo" MasterPageFile="~/Site.Master"%>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"  runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                &nbsp;<CR:CrystalReportViewer ID="cws" runat="server" AutoDataBind="true" /> 
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
