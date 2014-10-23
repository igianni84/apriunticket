Public Class Prova
    
        Inherits System.Web.UI.Page


    Protected Sub btnOpenMe_Click(sender As Object, e As EventArgs) Handles btnOpenMe.Click

        If (Not ClientScript.IsStartupScriptRegistered("alert")) Then
            Page.ClientScript.RegisterStartupScript _
            (Me.GetType(), "alert", "MyFunction('ciao');", True)
        End If
    End Sub
    Protected Sub btnShowPopup_Click(sender As Object, e As System.EventArgs) Handles btnShowPopup.Click
        Dim message As String = "Message from server side"
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "Prova();", True)
    End Sub
End Class