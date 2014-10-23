Public Class tariffazione
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyTipoTariffazione As Tariffa
    Private MyMisura As Misura
    Private MyTipoSoglia As TipoSoglia

    Private MyCGlobal As CGlobal
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyCGlobal = New CGlobal
        Mygest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Select Case Session("tipoutente")
                        Case "SuperAdmin"
                            Me.CaricaTariffazione()
                            Me.CaricaMisura()
                        Case "Operatore"
                            Me.CaricaTariffazione(, Me.RestituisciOrganizzazione())
                            Me.CaricaMisura(, Me.RestituisciOrganizzazione())
                        Case Else
                            Response.Redirect("~/login.aspx")
                    End Select

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
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

    Private Sub CaricaTariffazione(Optional ByVal idt As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tariffazione,id,idazienda from Tariffazione"
        If idorg <> "-1" Then
            str = str & " where idazienda=" & idorg
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoTariffazione.DataSource = tab
        Me.ddlTipoTariffazione.DataTextField = "tariffazione"
        Me.ddlTipoTariffazione.DataValueField = "id"
        Me.ddlTipoTariffazione.DataBind()
        If idt <> "-1" Then
            Me.ddlTipoTariffazione.SelectedValue = idt
        Else
            Me.ddlTipoTariffazione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaMisura(Optional ByVal idt As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select misura,id,idazienda from Misura"
        If idorg <> "-1" Then
            str = str & " where idazienda=" & idorg
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlUnitàMisura.DataSource = tab
        Me.ddlUnitàMisura.DataTextField = "misura"
        Me.ddlUnitàMisura.DataValueField = "id"
        Me.ddlUnitàMisura.DataBind()
        If idt <> "-1" Then
            Me.ddlUnitàMisura.SelectedValue = idt
        Else
            Me.ddlUnitàMisura.SelectedValue = "-1"
        End If
    End Sub


    Protected Sub imgTipoTariffazione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgTipoTariffazione.Click
        Me.MyTipoTariffazione = New Tariffa
        Me.MyTipoTariffazione.Azienda = New Azienda
        Me.MyCGlobal = New CGlobal
        If txtTipoTariffazione.Text <> "" And MyCGlobal.Verifier("Tariffazione", "tariffazione", txtTipoTariffazione.Text, Me.RestituisciOrganizzazione()) Then
            Me.MyTipoTariffazione.Tariffazione = txtTipoTariffazione.Text
            Me.MyTipoTariffazione.Azienda.ID = Me.RestituisciOrganizzazione
            Me.MyTipoTariffazione.SalvaData()
            txtTipoTariffazione.Text = ""
            Me.CaricaTariffazione()
            Dim message As String = "Tariffazione Inserita"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Else
            Dim message As String = "Tariffazione già Presente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If

    End Sub

   


    Protected Sub imgMisura_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgUnitaMisura.Click
        Me.MyMisura = New Misura
        Me.MyMisura.Azienda = New Azienda
        Me.MyTipoSoglia = New TipoSoglia
        Me.MyTipoSoglia.Azienda = New Azienda
        Me.MyCGlobal = New CGlobal
        If txtUnitaMisura.Text <> "" And MyCGlobal.Verifier("Misura", "misura", txtUnitaMisura.Text, Me.RestituisciOrganizzazione()) Then
            Me.MyMisura.Misura = txtUnitaMisura.Text
            Me.MyMisura.Azienda.ID = Me.RestituisciOrganizzazione
            Me.MyMisura.SalvaData()

            Me.MyTipoSoglia.tiposoglia = txtUnitaMisura.Text
            Me.MyTipoSoglia.Azienda.ID = Me.RestituisciOrganizzazione
            Me.MyTipoSoglia.SalvaData()

            txtUnitaMisura.Text = ""
            Me.CaricaMisura()
            Dim message As String = "Unità di Misura Inserita"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Else
            Dim message As String = "Unità di Misura già Presente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    

    Protected Sub imgElTariffazione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgElTipoTariffazione.Click
        If MyCGlobal.VerifierLegami("Tariffazione", "Tariffa", "idtariffazione") Then
            'If MyCGlobal.VerifierLegami("TipoDispositivo", "Marchio", "idtipodispositivo") Then
            If ddlTipoTariffazione.SelectedValue <> "-1" Then
                Me.MyTipoTariffazione = New Tariffa
                Me.MyTipoTariffazione.Delete(ddlTipoTariffazione.SelectedValue)
                Me.CaricaTariffazione()
            End If
            'Else
            '    Dim message As String = "Attenzione! Ci sono dei legami tra questo dispositivo e i marchi"
            '    ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            'End If
        Else
            Dim message As String = "Attenzione! Ci sono dei legami con questa tariffazionee i listini"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub imgElMisura_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgElUnitàMisura.Click
        If MyCGlobal.VerifierLegami("Misura", "Tariffa", "idmisura") Then
            'If MyCGlobal.VerifierLegami("Marchio", "Modello", "idmarchio") Then
            If ddlUnitàMisura.SelectedValue <> "-1" Then
                Me.MyMisura = New Misura
                Me.MyMisura.Delete(ddlUnitàMisura.SelectedValue)
                Me.CaricaMisura()
            End If
            'Else
            '    Dim message As String = "Attenzione! Ci sono dei legami tra questo marchio e i modelli"
            '    ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            'End If
        Else
            Dim message As String = "Attenzione! Ci sono dei legami con questa unità di misura e i listini"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    
End Class