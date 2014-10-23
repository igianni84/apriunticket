Public Class Site
    Inherits System.Web.UI.MasterPage

#Region "Declare"
    Private MyAzienda As Azienda
    Private MyUtente As Utente
    Private MyTicket As Tickets
    Private MyCGlobal As CGlobal
    Public MyGest As MNGestione
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.MyGest = New MNGestione(CGlobal.cs)
        Me.MyGest.Connetti()

        lblUtente.Text = Session("utente")
        Select Case Request.Url.AbsolutePath
            Case "/subcliente.aspx"
                'Menu1.CssClass.BackColor = Drawing.Color.Black
        End Select
        Dim sql As String = "Select * from Versione"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Session("release") = tab.Rows(0)("release")
        End If

    End Sub

    

    Private Sub Apri()
        If Session("tipoutente") = "Utente" Then
            Me.MyCGlobal = New CGlobal
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.SubCliente.ID <> "-1" Then
                If Me.MyCGlobal.VerificaBloccoCliente(MyUtente.SubCliente.ID, "SubCliente") Then
                    Response.Redirect("apriticket.aspx")
                Else
                    Dim message As String = "SubCliente bloccato. Contatti la sua organizzazione per ulteriori informmazioni"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                End If
            Else
                If Me.MyCGlobal.VerificaBloccoCliente(MyUtente.Cliente.ID, "Cliente") Then
                    Response.Redirect("apriticket.aspx")
                Else
                    Dim message As String = "Cliente bloccato. Contatti la sua organizzazione per ulteriori informmazioni"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                End If
            End If
        Else
            Response.Redirect("apriticket.aspx")
        End If
    End Sub

    'Protected Sub Menu1_MenuItemClick(sender As Object, e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
    '    Dim a = Menu1.SelectedValue
    'End Sub

   
   

    
End Class