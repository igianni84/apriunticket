Public Class attivazione
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUser As Utente

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("utente") <> "" Then


            Me.MyUser = New Utente
            Dim tempo() As String
            Dim miaPasswordDecriptata As String = VSTripleDES.DecryptData(Request.QueryString("utente"))
            tempo = miaPasswordDecriptata.Split("\")

            Me.MyUser.Load(tempo(1))
            Me.MyUser.Abilitato = 1
            Me.MyUser.SalvaData()
        End If
    End Sub

End Class