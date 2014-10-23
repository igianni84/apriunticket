Public Class _error
    Inherits System.Web.UI.Page

#Region "Variabili"
    Private MyGest As MNGestione
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then
                Response.Redirect("~/login.aspx")
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub


End Class