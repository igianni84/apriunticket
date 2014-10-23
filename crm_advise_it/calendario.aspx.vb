Public Class calendario
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyGiorLavorativa As GiorLavorativa
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then
                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Me.VerificaGiornataLavorativa()
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub VerificaGiornataLavorativa()
        Me.MyGiorLavorativa = New GiorLavorativa
        Dim sql As String = "select * from GiorLavorativa where idazienda=" & RestituisciOrganizzazione()
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Me.CaricaGiornataLavorativa(tab)
        Else
            Dim sq As String = "select * from GiorLavorativa where idazienda=-1"
            Dim ta As DataTable = Me.MyGest.GetTab(sq)
            Me.CaricaGiornataLavorativa(ta)
        End If
    End Sub

    Private Sub CaricaGiornataLavorativa(ByVal tab As DataTable)
        For i As Integer = 0 To tab.rows.count - 1
            Select Case tab.Rows(i)("giorno")
                Case "Lunedì"
                    cbxLunedi.Checked = True
                    txtOrarioApLun.Text = tab.Rows(i)("oraap")
                    txtOrarioChLun.Text = tab.Rows(i)("orach")
                    txtPausaInLun.Text = tab.Rows(i)("pausain")
                    txtPausaFiLun.Text = tab.Rows(i)("pausafi")
                Case "Martedì"
                    cbxMartedi.Checked = True
                    txtOrarioApMar.Text = tab.Rows(i)("oraap")
                    txtOrarioChMar.Text = tab.Rows(i)("orach")
                    txtPausaInMar.Text = tab.Rows(i)("pausain")
                    txtPausaFiMar.Text = tab.Rows(i)("pausafi")
                Case "Mercoledì"
                    cbxMercoledi.Checked = True
                    txtOrarioApMer.Text = tab.Rows(i)("oraap")
                    txtOrarioChMer.Text = tab.Rows(i)("orach")
                    txtPausaInMer.Text = tab.Rows(i)("pausain")
                    txtPausaFiMer.Text = tab.Rows(i)("pausafi")
                Case "Giovedì"
                    cbxGiovedi.Checked = True
                    txtOrarioApGio.Text = tab.Rows(i)("oraap")
                    txtOrarioChGio.Text = tab.Rows(i)("orach")
                    txtPausaInGio.Text = tab.Rows(i)("pausain")
                    txtPausaFiGio.Text = tab.Rows(i)("pausafi")
                Case "Venerdì"
                    cbxVenerdi.Checked = True
                    txtOrarioApVen.Text = tab.Rows(i)("oraap")
                    txtOrarioChVen.Text = tab.Rows(i)("orach")
                    txtPausaInVen.Text = tab.Rows(i)("pausain")
                    txtPausaFiVen.Text = tab.Rows(i)("pausafi")
                Case "Sabato"
                    cbxSabato.Checked = True
                    txtOrarioApSab.Text = tab.Rows(i)("oraap")
                    txtOrarioChSab.Text = tab.Rows(i)("orach")
                    txtPausaInSab.Text = tab.Rows(i)("pausain")
                    txtPausaFiSab.Text = tab.Rows(i)("pausafi")
                Case "Domenica"
                    cbxDomenica.Checked = True
                    txtOrarioApDom.Text = tab.Rows(i)("oraap")
                    txtOrarioChDom.Text = tab.Rows(i)("orach")
                    txtPausaInDom.Text = tab.Rows(i)("pausain")
                    txtPausaFiDom.Text = tab.Rows(i)("pausafi")
            End Select
        Next
    End Sub

    Private Function RestituisciOrganizzazione() As Integer
        Dim sql As String = "select idazienda from utente where id=" & Session("id")
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim res As Integer = -1
        If tab.Rows.Count > 0 Then
            res = tab.Rows(0)("idazienda")
        End If
        Return res
    End Function

    Protected Sub btnReplica_Click(sender As Object, e As EventArgs) Handles btnReplica.Click
        txtOrarioApMar.Text = txtOrarioApLun.Text
        txtOrarioApMer.Text = txtOrarioApLun.Text
        txtOrarioApGio.Text = txtOrarioApLun.Text
        txtOrarioApVen.Text = txtOrarioApLun.Text
        txtOrarioApSab.Text = txtOrarioApLun.Text
        txtOrarioApDom.Text = txtOrarioApLun.Text

        txtOrarioChMar.Text = txtOrarioChLun.Text
        txtOrarioChMer.Text = txtOrarioChLun.Text
        txtOrarioChGio.Text = txtOrarioChLun.Text
        txtOrarioChVen.Text = txtOrarioChLun.Text
        txtOrarioChSab.Text = txtOrarioChLun.Text
        txtOrarioChDom.Text = txtOrarioChLun.Text

        txtPausaInMar.Text = txtPausaInLun.Text
        txtPausaInMer.Text = txtPausaInLun.Text
        txtPausaInGio.Text = txtPausaInLun.Text
        txtPausaInVen.Text = txtPausaInLun.Text
        txtPausaInSab.Text = txtPausaInLun.Text
        txtPausaInDom.Text = txtPausaInLun.Text

        txtPausaFiMar.Text = txtPausaFiLun.Text
        txtPausaFiMer.Text = txtPausaFiLun.Text
        txtPausaFiGio.Text = txtPausaFiLun.Text
        txtPausaFiVen.Text = txtPausaFiLun.Text
        txtPausaFiSab.Text = txtPausaFiLun.Text
        txtPausaFiDom.Text = txtPausaFiLun.Text

    End Sub

    Private Sub btnMemorizza_Click(sender As Object, e As System.EventArgs) Handles btnMemorizza.Click
        Me.MyGiorLavorativa = New GiorLavorativa
        Try
            Me.MyGiorLavorativa.Delete(Me.RestituisciOrganizzazione)
            If cbxLunedi.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Lunedì"
                Me.MyGiorLavorativa.OraAp = txtOrarioApLun.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChLun.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInLun.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiLun.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxMartedi.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Martedì"
                Me.MyGiorLavorativa.OraAp = txtOrarioApMar.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChMar.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInMar.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiMar.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxMercoledi.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Mercoledì"
                Me.MyGiorLavorativa.OraAp = txtOrarioApMer.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChMer.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInMer.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiMer.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxGiovedi.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Giovedì"
                Me.MyGiorLavorativa.OraAp = txtOrarioApGio.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChGio.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInGio.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiGio.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxVenerdi.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Venerdì"
                Me.MyGiorLavorativa.OraAp = txtOrarioApVen.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChVen.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInVen.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiVen.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxSabato.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Sabato"
                Me.MyGiorLavorativa.OraAp = txtOrarioApSab.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChSab.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInSab.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiSab.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            If cbxDomenica.Checked = True Then
                Me.MyGiorLavorativa = New GiorLavorativa
                Me.MyGiorLavorativa.Azienda = New Azienda
                Me.MyGiorLavorativa.Giorno = "Domenica"
                Me.MyGiorLavorativa.OraAp = txtOrarioApDom.Text
                Me.MyGiorLavorativa.OraCh = txtOrarioChDom.Text
                Me.MyGiorLavorativa.PausaIn = txtPausaInDom.Text
                Me.MyGiorLavorativa.PausaFi = txtPausaFiDom.Text
                Me.MyGiorLavorativa.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyGiorLavorativa.SalvaData()
            End If
            Dim message As String = "Giornata Lavorativa inserita"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Catch
            Dim message As String = "Errore nell'inserimento"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click
        txtOrarioApLun.Text = ""
        txtOrarioApMar.Text = ""
        txtOrarioApMer.Text = ""
        txtOrarioApGio.Text = ""
        txtOrarioApVen.Text = ""
        txtOrarioApSab.Text = ""
        txtOrarioApDom.Text = ""

        txtOrarioChLun.Text = ""
        txtOrarioChMar.Text = ""
        txtOrarioChMer.Text = ""
        txtOrarioChGio.Text = ""
        txtOrarioChVen.Text = ""
        txtOrarioChSab.Text = ""
        txtOrarioChDom.Text = ""

        txtPausaInLun.Text = ""
        txtPausaInMar.Text = ""
        txtPausaInMer.Text = ""
        txtPausaInGio.Text = ""
        txtPausaInVen.Text = ""
        txtPausaInSab.Text = ""
        txtPausaInDom.Text = ""

        txtPausaFiLun.Text = ""
        txtPausaFiMar.Text = ""
        txtPausaFiMer.Text = ""
        txtPausaFiGio.Text = ""
        txtPausaFiVen.Text = ""
        txtPausaFiSab.Text = ""
        txtPausaFiDom.Text = ""

        cbxLunedi.Checked = False
        cbxMartedi.Checked = False
        cbxMercoledi.Checked = False
        cbxGiovedi.Checked = False
        cbxVenerdi.Checked = False
        cbxSabato.Checked = False
        cbxDomenica.Checked = False

    End Sub
End Class