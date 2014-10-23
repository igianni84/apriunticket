Public Class setupemail
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyContenutoMail As ContenutoMail
    Private MyParametriMail As parametrimail
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" And Session("isadmin") = 1 Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else

                If Not IsPostBack Then
                    Me.CaricaTipoMail()
                    Me.CaricaMail()
                    Me.caricaParametrizzazione()
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CaricaModelloMail(ByVal idtipomail As String)

        Dim sql As String = "Select * from ContenutoMail where idazienda=" & Me.RestituisciOrganizzazione & " and idtipomail=" & idtipomail
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Me.MyContenutoMail = New ContenutoMail
            Me.MyContenutoMail.Load(tab.Rows(0)("id"))
            txtOggetto.Text = Me.MyContenutoMail.Oggetto
            txtCorpo.Text = Me.MyContenutoMail.Corpo
            txtFirma.Text = Me.MyContenutoMail.Firma
            lblErrore.Visible = False
        Else
            lblErrore.Visible = True
        End If
    End Sub

    Private Sub CaricaMail()
        Dim sql As String = "Select * from ContenutoMail inner join TipoMail on TipoMail.id=ContenutoMail.idtipomail  where idazienda=" & Me.RestituisciOrganizzazione
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim tstruct As New DataTable
        Dim c As New DataColumn("tipomail", GetType(String), "")
        tstruct.Columns.Add(c)
        Dim c1 As New DataColumn("oggetto", GetType(String), "")
        tstruct.Columns.Add(c1)
        Dim c2 As New DataColumn("corpo", GetType(String), "")
        tstruct.Columns.Add(c2)
        Dim c3 As New DataColumn("firma", GetType(String), "")
        tstruct.Columns.Add(c3)
        Dim c4 As New DataColumn("id", GetType(String), "")
        tstruct.Columns.Add(c4)

        For i As Integer = 0 To TAB.Rows.Count - 1
            tstruct.Rows.Add(i)
            tstruct.Rows(i)("tipomail") = tab.Rows(i)("tipomail")
            tstruct.Rows(i)("id") = tab.Rows(i)("id")
            If tab.Rows(i)("oggetto").ToString.Length > 30 Then
                tstruct.Rows(i)("oggetto") = tab.Rows(i)("oggetto").ToString.Substring(0, 30) & "..."
            Else
                tstruct.Rows(i)("oggetto") = tab.Rows(i)("oggetto")
            End If
            'If tab.Rows(i)("corpo").ToString.Length > 30 Then
            '    tstruct.Rows(i)("corpo") = tab.Rows(i)("corpo").ToString.Substring(0, 30) & "..."
            'Else
            '    tstruct.Rows(i)("corpo") = tab.Rows(i)("corpo")
            'End If
            'If tab.Rows(i)("firma").ToString.Length > 30 Then
            '    tstruct.Rows(i)("firma") = tab.Rows(i)("firma").ToString.Substring(0, 30) & "..."
            'Else
            '    tstruct.Rows(i)("firma") = tab.Rows(i)("firma")
            'End If
        Next
        ListView1.DataSource = tstruct
        ListView1.DataBind()

        If TAB.Rows.Count > 10 Then
            DataPager1.Visible = True
        Else
            DataPager1.Visible = False
        End If
    End Sub

    Private Sub CaricaTipoMail()
        Dim str As String = "select tipomail ,id from TipoMail"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoMail.DataSource = tab
        Me.ddlTipoMail.DataTextField = "tipomail"
        Me.ddlTipoMail.DataValueField = "id"
        Me.ddlTipoMail.DataBind()
            Me.ddlTipoMail.SelectedValue = "-1"

    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If IsValid Then
            Me.MyContenutoMail = New ContenutoMail
            Me.MyContenutoMail.Azienda = New Azienda
            If Session("tipoutente") = "Operatore" And Session("isadmin") = 1 Then
                Dim sql As String = "Select * from ContenutoMail where idazienda=" & Me.RestituisciOrganizzazione & " and idtipomail=" & ddlTipoMail.SelectedValue
                Dim tab As DataTable = Me.MyGest.GetTab(sql)
                If tab.Rows.Count > 0 And Not lblUpdate.Text = True Then
                    Dim message As String = ddlTipoMail.SelectedItem.Text & " già presente"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                Else
                    Me.MyContenutoMail.ID = lblIdMail.Text
                    Me.MyContenutoMail.Oggetto = txtOggetto.Text
                    Me.MyContenutoMail.Corpo = txtCorpo.Text
                    Me.MyContenutoMail.Firma = txtFirma.Text
                    Me.MyContenutoMail.TipoMail = ddlTipoMail.SelectedValue
                    Me.MyContenutoMail.Azienda.ID = Me.RestituisciOrganizzazione
                    Me.MyContenutoMail.SalvaData()
                    Me.CaricaMail()
                    Dim message As String = "Modello aggiornato correttamente"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                End If
            ElseIf Session("tipoutente") = "SuperAdmin" Then
                Me.MyContenutoMail.ID = lblIdMail.Text
                Me.MyContenutoMail.Oggetto = txtOggetto.Text
                Me.MyContenutoMail.Corpo = txtCorpo.Text
                Me.MyContenutoMail.Firma = txtFirma.Text
                Me.MyContenutoMail.TipoMail = ddlTipoMail.SelectedValue
                Me.MyContenutoMail.Azienda.ID = Me.RestituisciOrganizzazione
                Me.MyContenutoMail.SalvaData()
            End If
        Else
            Dim message As String = "Tipologia mail obbligatoria"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
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

    Protected Sub ddlTipoMail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoMail.SelectedIndexChanged
        Me.PulisciCampi()
        Me.CaricaModelloMail(ddlTipoMail.SelectedValue)
    End Sub
    Private Sub PulisciCampi()
        txtOggetto.Text = ""
        txtCorpo.Text = ""
        txtFirma.Text = ""
        lblIdMail.Text = "-1"
        lblUpdate.Text = False
    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaMail

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaMail
       
    End Sub

    Private Sub ModificaMail(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdMail.Text = bt.AlternateText
        Me.MyContenutoMail = New ContenutoMail
        Me.MyContenutoMail.Load(lblIdMail.Text)
        ddlTipoMail.SelectedValue = Me.MyContenutoMail.TipoMail
        txtOggetto.Text = Me.MyContenutoMail.Oggetto
        txtCorpo.Text = Me.MyContenutoMail.Corpo
        txtFirma.Text = Me.MyContenutoMail.Firma
        lblErrore.Visible = False
        lblUpdate.Text = True
    End Sub

    Private Sub CancellaMail(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdMail.Text = bt.AlternateText

        PanelEliminaCliente.Visible = True
        Me.MyContenutoMail = New ContenutoMail
        Me.MyContenutoMail.Load(bt.AlternateText)
        
        If Me.MyContenutoMail.Oggetto.ToString.Length > 30 Then
            lblOggettoElimina.Text = Me.MyContenutoMail.Oggetto.Substring(0, 30) & "..."
        Else
            lblOggettoElimina.Text = Me.MyContenutoMail.Oggetto
        End If
        Dim sql As String = "select * from TipoMail where TipoMail.id=" & Me.MyContenutoMail.TipoMail
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            lblTipoMailElimina.Text = tab.Rows(0)("tipomail")
        End If
        lblAziendaElimina.Text = Me.MyContenutoMail.Azienda.Descrizione

    End Sub

    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaCliente.Visible = False
        lblIdMail.Text = "-1"
        lblUpdate.Text = False
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyContenutoMail = New ContenutoMail
        If Me.MyContenutoMail.Delete(lblIdMail.Text) Then
            Me.CaricaMail()
            lblIdMail.Text = "-1"
            lblUpdate.Text = False
            PanelEliminaCliente.Visible = False
            Dim message As String = "Modello mail eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            Me.PulisciCampi()
        End If
    End Sub

    Private Sub btnMemorizzaPar_Click(sender As Object, e As System.EventArgs) Handles btnMemorizzaPar.Click
        Me.MyParametriMail = New ParametriMail
        Me.MyParametriMail.ID = lblIdParametrizzazione.Text
        Me.MyParametriMail.Account = txtAccount.Text
        Me.MyParametriMail.Mittente = txtMittente.Text
        Me.MyParametriMail.Pass = txtPassword.Text
        Me.MyParametriMail.Porta = txtPorta.Text
        Me.MyParametriMail.Smtp = txtSmtp.Text
        Me.MyParametriMail.Azienda.ID = Me.RestituisciOrganizzazione
        Me.MyParametriMail.SalvaData()
        lblIdParametrizzazione.Text = Me.MyParametriMail.ID
    End Sub

    Private Sub CaricaParametrizzazione()
        'Dim idazienda = Me.RestituisciOrganizzazione
        Dim sql As String = "select * from ParametriMail where idazienda=" & Me.RestituisciOrganizzazione
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            lblIdParametrizzazione.Text = tab.Rows(0)("id")
            txtAccount.Text = tab.Rows(0)("account")
            txtMittente.Text = tab.Rows(0)("mittente")
            txtPassword.Text = tab.Rows(0)("pass")
            txtPorta.Text = tab.Rows(0)("porta")
            txtSmtp.Text = tab.Rows(0)("smtp")
        End If
    End Sub


    Private Sub imgInfo_Click(sender As Object, e As System.EventArgs) Handles imgInfo.Click
        Dim message As String = "Utilizzare i seguenti parametri:         $Operatore$->nome e cognome utente;         $Userid$->mail utente;         $Password$->password;         $Organizzazione$->organizzazione di riferimento;         $Stato$->stato ticket          $DataEvento$->Data Evento         $DataInvio$->Data Apertuta Ticket         $UtenteEvento$->Utente Evento         $UtenteTicket$->Utente Ticket"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
    End Sub
End Class