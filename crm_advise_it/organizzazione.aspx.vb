Imports System.IO

Public Class anagrafica
    Inherits System.Web.UI.Page
#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyAzienda As Azienda
    Private MyRecapito As Recapito
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        Me.Form.Enctype = "multipart/form-data"
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaOrganizzazioni.Visible = True
                    Me.CaricaOrganizzazioni()
                    Me.DisabilitaCampi()
                    Me.CaricaRegione()
                    Me.CaricaProvincia()
                    'Me.CaricaComune()
                    ddlComune.Text = ""
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    

    Private Sub CaricaOrganizzazioni(Optional ByVal descrizione As String = "")
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,Regioni.nome as regione,Province.nome as provincia " & _
                     "FROM Azienda inner join Regioni on Regioni.idRegione=Azienda.idregione " & _
                     "inner join Province on Province.idProvincia=Azienda.idProvincia " & _
                     "where Azienda.id<>-1"
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sqlStr = sqlStr & "and Azienda.id=" & MyUtente.Azienda.ID
            If Session("isadmin") = 0 Then

            End If
            PanelRicercaOrganizzazioni.Visible = False
        Else
            PanelRicercaOrganizzazioni.Visible = True
        End If

        If descrizione <> "" Then
            sqlStr = sqlStr & "and descrizione like '%" & descrizione & "%'"
        End If


        tab = MyGest.GetTab(sqlStr)

        ListView1.DataSource = tab
        ListView1.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager1.Visible = True
        Else
            DataPager1.Visible = False
        End If
    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaOrganizzazione

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaOrganizzazione

        Dim btn2 As ImageButton = e.Item.FindControl("imgMostra")
        AddHandler btn2.Click, AddressOf ModificaOrganizzazione

        

    End Sub

    

    

    Private Sub ModificaOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelRecapiti.Visible = True
        lbRecapiti.Visible = True
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyAzienda = New Azienda
        lblIdAzienda.Text = bt.AlternateText
        Me.MyAzienda.Load(lblIdAzienda.Text)
        If MyAzienda.Logo <> "" Then
            Dim nome As String = MyAzienda.Descrizione.Replace(".", "").Replace("'", "")
            imgLogo.ImageUrl = "\logo\" & nome & "\" & MyAzienda.Logo
        End If
        txtCodice.Text = Me.MyAzienda.Codice
        txtCap.Text = Me.MyAzienda.Cap
        txtIndirizzo.Text = Me.MyAzienda.Indirizzo
        txtPiva.Text = Me.MyAzienda.Pariva
        txtIndirizzo.Text = Me.MyAzienda.Indirizzo
        Me.CaricaRegione(Me.MyAzienda.IDRegione)
        Me.CaricaProvincia(Me.MyAzienda.IDProvincia, Me.MyAzienda.IDRegione)
        'Me.CaricaComune(Me.MyAzienda.IDComune, Me.MyAzienda.IDProvincia)
        ddlComune.Text = Me.MyAzienda.IDComune
        txtRagSociale.Text = Me.MyAzienda.Descrizione

        Panel1.Visible = False
        PanelModificaOrganizzazione.Visible = True
        PanelRicercaOrganizzazioni.Visible = False
        Me.DisabilitaCampi()
        Me.CaricaTipologiaContatto()
        Me.CaricaContatti()

        Select Case Session("tipoutente")
            Case "Operatore"
                btnModifica.Visible = False
                If Session("isadmin") = 1 Then
                    btnModifica.Visible = True
                End If
            Case Else
                btnModifica.Visible = True
        End Select

    End Sub

    Private Sub CancellaOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)

        lblIdAzienda.Text = bt.AlternateText
        PanelEliminaOrg.Visible = True
        PanelRicercaOrganizzazioni.Visible = False
        Me.MyAzienda = New Azienda
        Me.MyAzienda.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyAzienda.Codice
        lblSubOrganizzazioneElimina.Text = Me.MyAzienda.Descrizione
        lblPartitaIvaElimina.Text = Me.MyAzienda.Pariva
    End Sub

    Private Sub CaricaRegione(Optional ByVal idr As String = "-1")
        Dim str As String = "select nome ,idRegione from Regioni"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlRegione.DataSource = tab
        Me.ddlRegione.DataTextField = "nome"
        Me.ddlRegione.DataValueField = "idRegione"
        Me.ddlRegione.DataBind()
        If idr <> "-1" Then
            Me.ddlRegione.SelectedValue = idr
        Else
            Me.ddlRegione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaProvincia(Optional ByVal idp As String = "-1", Optional ByVal idr As String = "-1")
        Dim str As String = "select Province.nome ,idProvincia from Province"
        'If idr <> "-1" Then
        str = str & " inner join Regioni on Province.idRegione=Regioni.idRegione where Regioni.idRegione=" & idr
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlProvincia.DataSource = tab
        Me.ddlProvincia.DataTextField = "nome"
        Me.ddlProvincia.DataValueField = "idProvincia"
        Me.ddlProvincia.DataBind()
        If idp <> "-1" Then
            Me.ddlProvincia.SelectedValue = idp
        Else
            Me.ddlProvincia.SelectedValue = "-1"
        End If
    End Sub

    'Private Sub CaricaComune(Optional ByVal idc As String = "-1", Optional ByVal idp As String = "-1")
    '    Dim str As String = "select Comuni.nome+' - '+Comuni.CAP as nom ,idComune from Comuni"
    '    'If idp <> "-1" Then
    '    str = str & " inner join Province on Comuni.idProvincia=Province.idProvincia where Province.idProvincia=" & idp
    '    'End If
    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlComune.DataSource = tab
    '    Me.ddlComune.DataTextField = "nom"
    '    Me.ddlComune.DataValueField = "idComune"
    '    Me.ddlComune.DataBind()
    '    If idc <> "-1" Then
    '        Me.ddlComune.SelectedValue = idc
    '    Else
    '        Me.ddlComune.SelectedValue = "-1"
    '    End If
    'End Sub

    Protected Sub btnModifica_Click(sender As Object, e As EventArgs) Handles btnModifica.Click
        'txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        'ddlComune.Enabled = True
        'txtIndirizzo.ReadOnly = False
        'txtPiva.ReadOnly = False
        'ddlProvincia.Enabled = True
        'txtRagSociale.ReadOnly = False
        'ddlRegione.Enabled = True
        'txtRicercaOrganizzazioni.ReadOnly = False
        'btnModifica.Visible = False
        'btnSalva.Visible = True
        Me.AbilitaCampi()
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        PanelModificaOrganizzazione.Visible = False
        Panel1.Visible = True
        PanelRicercaOrganizzazioni.Visible = True
    End Sub

    Protected Sub btnRicercaOrganizzazioni_Click(sender As Object, e As EventArgs) Handles btnRicercaOrganizzazioni.Click
        Me.CaricaOrganizzazioni(txtRicercaOrganizzazioni.Text)
        Panel1.Visible = True
        PanelModificaOrganizzazione.Visible = False
    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If IsValid Then
            Me.MyAzienda = New Azienda
            Me.MyAzienda.Load(lblIdAzienda.Text)
            Me.MyAzienda.IDRegione = ddlRegione.SelectedValue
            Me.MyAzienda.IDProvincia = ddlProvincia.SelectedValue
            Me.MyAzienda.IDComune = ddlComune.Text
            Me.MyAzienda.Descrizione = txtRagSociale.Text
            Me.MyAzienda.Cap = txtCap.Text
            Me.MyAzienda.Codice = txtCodice.Text
            Me.MyAzienda.Indirizzo = txtIndirizzo.Text
            Me.MyAzienda.Pariva = txtPiva.Text
            Me.MyAzienda.Logo = txtLogo.Text
            Me.MyAzienda.SalvaData()
            lblIdAzienda.Text = Me.MyAzienda.ID
            lblConferma.Visible = True
            lblConferma.Text = "Organizzazione Creata"
            Me.CaricaOrganizzazioni()

            PanelModificaOrganizzazione.Visible = False
            Panel1.Visible = True
            PanelRicercaOrganizzazioni.Visible = True
            Dim message2 As String = "Organizzazione creata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        Else
            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If

    End Sub

    Protected Sub ddlRegione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegione.SelectedIndexChanged
        Me.CaricaProvincia("-1", ddlRegione.SelectedValue)
        'Me.CaricaComune()
    End Sub

    'Protected Sub ddlProvincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvincia.SelectedIndexChanged
    '    Me.CaricaComune("-1", ddlProvincia.SelectedValue)
    'End Sub

    Private Sub SvuotaCampi()
        lblIdAzienda.Text = "-1"
        imgLogo.ImageUrl = ""
        txtCodice.Text = ""
        txtCap.Text = ""
        txtIndirizzo.Text = ""
        txtPiva.Text = ""
        txtIndirizzo.Text = ""
        Me.CaricaRegione()
        Me.CaricaProvincia()
        'Me.CaricaComune()
        ddlComune.Text = ""
        lblConferma.Text = ""
        txtRagSociale.Text = ""
    End Sub

    Private Sub InserisciCodice()
        Dim idmax
        Dim sql As String = "select Max(id),codice from Azienda group by id,codice"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Substring(1))
        Else
            idmax = 0
        End If
        idmax = "O" & idmax + 1
        txtCodice.Text = idmax
    End Sub


    Private Sub AbilitaCampi()
        txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        ddlComune.Enabled = True
        txtIndirizzo.ReadOnly = False
        txtPiva.ReadOnly = False
        ddlProvincia.Enabled = True
        txtRagSociale.ReadOnly = False
        ddlRegione.Enabled = True
        btnModifica.Visible = False
        btnCarica.Visible = True
        FileUpload1.Visible = True
        btnSalva.Visible = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCap.ReadOnly = True
        'txtCodice.ReadOnly = True
        ddlComune.Enabled = False
        txtIndirizzo.ReadOnly = True
        txtPiva.ReadOnly = True
        ddlProvincia.Enabled = False
        txtRagSociale.ReadOnly = True
        ddlRegione.Enabled = False
        btnModifica.Visible = True
        btnCarica.Visible = False
        FileUpload1.Visible = False
        btnSalva.Visible = False
    End Sub

    Protected Sub btnCarica_Click(sender As Object, e As EventArgs) Handles btnCarica.Click
        Me.MyAzienda = New Azienda
        Me.MyAzienda.Load(lblIdAzienda.Text)
        If (FileUpload1.HasFile) Then
            Dim thefilename As String = ""
            Dim nome As String = MyAzienda.Descrizione.Replace(".", "").Replace("'", "")
            If Directory.Exists(Server.MapPath("\logo\" & nome & "\")) = False Then
                '    ' ...Creo la cartella al percorso specificato
                Directory.CreateDirectory(Server.MapPath("\logo\" & nome & "\"))
            End If
            If Not File.Exists(Server.MapPath("\logo\" & nome & "\" & FileUpload1.FileName)) Then
                thefilename = FileUpload1.FileName
            Else
                thefilename = Now.Date.ToString("dd/MM/yyyy").Replace("/", "") & FileUpload1.FileName
            End If
            Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & nome & "\" & thefilename))
            'Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & FileUpload1.FileName))
            imgLogo.ImageUrl = "\logo\" & nome & "\" & thefilename
            'imgLogo.ImageUrl = "\logo\" & FileUpload1.FileName
            txtLogo.Text = thefilename
            Me.MyAzienda.Logo = txtLogo.Text
            Me.MyAzienda.SalvaData()
        End If
        FileUpload1 = New FileUpload
    End Sub

#Region "Rubrica"
    Private Sub CaricaTipologiaContatto(Optional ByVal idtc As String = "-1")
        Dim sql As String = "Select descrizione,id from TipoContatto"

        If idtc <> "-1" Then
            sql = sql & " where TipoContatto.id=" & idtc
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipologiaContatto.DataSource = tab
        Me.ddlTipologiaContatto.DataTextField = "descrizione"
        Me.ddlTipologiaContatto.DataValueField = "id"
        Me.ddlTipologiaContatto.DataBind()
        If idtc <> "-1" Then
            Me.ddlTipologiaContatto.SelectedValue = idtc
        Else
            Me.ddlTipologiaContatto.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
        Me.MyRecapito = New Recapito

        Try
            Me.MyRecapito.ID = lblIdRecapito.Text
            Me.MyRecapito.Contatto = txtContatto.Text
            Me.MyRecapito.Descrizione = txtDescrizioneContatto.Text
            Me.MyRecapito.IDTipo = ddlTipologiaContatto.SelectedValue
            Me.MyRecapito.SalvaData()
            If lblIdRecapito.Text = "-1" Then
                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdAzienda.Text & "'," & Me.MyRecapito.ID & ",'Organizzazione')"
                MyGest.GetTab(sql)
            End If

        Catch
        End Try
        Me.CaricaContatti()
        Me.SvuotaCampiRecapito()
    End Sub
    Private Sub SvuotaCampiRecapito()
        lblIdRecapito.Text = "-1"
        txtContatto.Text = ""
        txtDescrizioneContatto.Text = ""
        CaricaTipologiaContatto()
    End Sub

    Private Sub CaricaContatti()
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,TipoContatto.descrizione as descr " & _
                     "FROM Recapito inner join Recapito_Utente on Recapito_Utente.idRecapito=Recapito.id " & _
                     "inner join Azienda on Azienda.id=Recapito_Utente.idUtente " & _
                     "inner join TipoContatto on TipoContatto.id=Recapito.idtipo " & _
                     "where Recapito_Utente.idUtente = " & lblIdAzienda.Text & " and Recapito_Utente.tipoAssociazione like 'Organizzazione' "

        tab = MyGest.GetTab(sqlStr)

        ListView2.DataSource = tab
        ListView2.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager2.Visible = True
        Else
            DataPager2.Visible = False
        End If
    End Sub

    Private Sub ListView2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView2.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaRecapito

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaRecapito

    End Sub

    Private Sub CancellaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyRecapito = New Recapito
        lblIdRecapito.Text = bt.AlternateText
        Me.MyRecapito.Load(lblIdRecapito.Text)
        txtContatto.Text = Me.MyRecapito.Contatto
        txtDescrizioneContatto.Text = Me.MyRecapito.Descrizione
        ddlTipologiaContatto.SelectedValue = Me.MyRecapito.IDTipo
        btnMemorizza.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True
        btnAnnulla1.Visible = False
        btnAnnullaContatto.Visible = True
    End Sub

    Private Sub ModificaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyRecapito = New Recapito
        lblIdRecapito.Text = bt.AlternateText
        Me.MyRecapito.Load(lblIdRecapito.Text)
        txtContatto.Text = Me.MyRecapito.Contatto
        txtDescrizioneContatto.Text = Me.MyRecapito.Descrizione
        ddlTipologiaContatto.SelectedValue = Me.MyRecapito.IDTipo
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        btnAnnulla.Visible = True
        lblSicuro.Visible = False
    End Sub

#End Region

    Protected Sub btnAnnulla1_Click(sender As Object, e As EventArgs) Handles btnAnnulla1.Click
        Me.SvuotaCampiRecapito()
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        lblSicuro.Visible = False

    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
        Me.MyRecapito = New Recapito
        Me.MyRecapito.Delete(lblIdRecapito.Text)
        Me.SvuotaCampiRecapito()
        Me.CaricaContatti()
        Dim message As String = "Contatto eliminato correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        btnAnnulla1.Visible = True
        btnAnnullaContatto.Visible = False
        btnElimina.Visible = False
        lblSicuro.Visible = False
        btnMemorizza.Visible = True
    End Sub



    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaOrg.Visible = False
        PanelRicercaOrganizzazioni.Visible = True
        lblIdAzienda.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyAzienda = New Azienda
        If Me.MyAzienda.Delete(lblIdAzienda.Text) Then
            Me.CaricaOrganizzazioni()
            lblIdAzienda.Text = "-1"
            PanelEliminaOrg.Visible = False
            PanelRicercaOrganizzazioni.Visible = True
            Dim message As String = "SubOrganizzazione eliminata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        Me.SvuotaCampi()
        PanelRecapiti.Visible = False
        lbRecapiti.Visible = False
        PanelModificaOrganizzazione.Visible = True
        Panel1.Visible = False

        Me.CaricaRegione()
        Me.AbilitaCampi()
        Me.InserisciCodice()
    End Sub

    Protected Sub btnAnnullaContatto_Click(sender As Object, e As EventArgs) Handles btnAnnullaContatto.Click
        Me.SvuotaCampiRecapito()
        lblSicuro.Visible = False
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        btnMemorizza.Visible = True
        btnAnnulla1.Visible = True
    End Sub

    Protected Sub lbRecapiti_Click(sender As Object, e As EventArgs) Handles lbRecapiti.Click
        If PanelRecapiti.Visible = True Then
            PanelRecapiti.Visible = False
        Else
            PanelRecapiti.Visible = True
            Me.CaricaContatti()
        End If
    End Sub
End Class