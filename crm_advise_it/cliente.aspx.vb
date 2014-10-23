Public Class cliente1
    Inherits System.Web.UI.Page


#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyCliente As Cliente
    Private MyRecapito As Recapito
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Dim app = Request.Url.ToString.Split("?")
                    Dim miaStringadeCriptata As String
                    If app.Length > 1 Then

                        miaStringadeCriptata = VSTripleDES.DecryptData(Request.Url.ToString.Split("?")(1))
                        ViewState("idcliente") = miaStringadeCriptata.Split("&")(0).ToString.Split("=")(1)
                        Me.ModificaCliente(sender, e, ViewState("idcliente"))
                    Else
                        Panel1.Visible = True
                        PanelRicercaCliente.Visible = True
                        PanelRicercaCliente.Visible = True
                    End If
                    Me.CaricaClienti()
                    Me.CaricaBloccoAmm()
                    Me.CaricaListino()

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CaricaContratto(Optional ByVal descrizione As String = "")
        Dim tab As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM Contratto inner join Azienda on Azienda.id=Contratto.idazienda " & _
                     "inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id " & _
                     "inner join Cliente on Cliente.id=Contratto_Cliente.idcliente " & _
                     "inner join TipoScadenza on TipoScadenza.id=Contratto.idtiposcadenza " & _
                     "inner join TipoContratto on TipoContratto.id=Contratto.idtipocontratto " & _
                     "where Contratto_Cliente.tipocliente='cliente'  "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            sqlStr = sqlStr & "and Cliente.id=" & lblIdCliente.Text
            If Session("isadmin") = 1 Then
                btnAggiungi.Visible = True
            Else
                btnAggiungi.Visible = False
            End If

            '    PanelRicercaContratto.Visible = False
            'Else
            '    PanelRicercaContratto.Visible = True
        End If

        tab = MyGest.GetTab(sqlStr)

        ListView3.DataSource = tab
        ListView3.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager3.Visible = True
        Else
            DataPager3.Visible = False
        End If
    End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaClienti(txtRicercaCliente.Text)
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaContatti()
    End Sub

    Private Sub ListView3_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView3.PagePropertiesChanging
        Me.DataPager3.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaContratto()
    End Sub

    Private Sub CaricaClienti(Optional ByVal ragsoc As String = "")
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,Cliente.ragsoc ,Regioni.nome as regione,Province.nome as provincia " & _
                     "FROM Cliente " & _
                     "inner join Regioni on Regioni.idRegione=Cliente.idregione " & _
                     "inner join Province on Province.idProvincia=Cliente.idProvincia " & _
                     "where Cliente.id<>-1 "
        If Session("tipoutente") = "Operatore" Then
            btNuovo.Visible = True
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sqlStr = sqlStr & "and Cliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" Then
            btNuovo.Visible = False
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            sqlStr = sqlStr & " and Cliente.id=" & Me.MyUtente.Cliente.ID
        End If
        If ragsoc <> "" Then
            sqlStr = sqlStr & "and ragsoc like '%" & ragsoc & "%'"
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
        AddHandler btn.Click, AddressOf CancellaCliente

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaCliente

        Dim btn2 As ImageButton = e.Item.FindControl("imgMostra")
        AddHandler btn2.Click, AddressOf ModificaCliente

        Dim btn3 As ImageButton = e.Item.FindControl("imgApriTicket")
        AddHandler btn3.Click, AddressOf ApriTicket
    End Sub

    Private Sub ApriTicket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Dim str = "idcliente=" & bt.AlternateText & "&tipo=cliente"
        Dim miaStringaCriptata As String
        miaStringaCriptata = VSTripleDES.EncryptData(str)
        Response.Redirect("apriticket.aspx?" & miaStringaCriptata)
    End Sub


    Private Sub ModificaCliente(ByVal sender As Object, ByVal e As System.EventArgs, Optional idcliente As String = "-1")

        Me.MyCliente = New Cliente
        If idcliente <> "-1" Then
            lblIdCliente.Text = idcliente
        Else
            Dim bt As ImageButton = CType(sender, ImageButton)
            lblIdCliente.Text = bt.AlternateText
        End If
        Me.MyCliente.Load(lblIdCliente.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo

        txtCodice.Text = Me.MyCliente.Codice
        txtCap.Text = Me.MyCliente.Cap
        txtIndirizzo.Text = Me.MyCliente.Indirizzo
        txtPiva.Text = Me.MyCliente.Pariva
        txtIndirizzo.Text = Me.MyCliente.Indirizzo
        Me.CaricaRegione(Me.MyCliente.IDRegione)
        Me.CaricaProvincia(Me.MyCliente.IDProvincia)
        'Me.CaricaComune(Me.MyCliente.IDComune)
        ddlComune.Text = Me.MyCliente.IDComune
        ddlBloccoAmm.SelectedIndex = Me.MyCliente.Blocco_Amm
        txtCodEst.Text = Me.MyCliente.CodEst
        txtCommittenza.Text = Me.MyCliente.Committenza
        CaricaListino(Me.MyCliente.Listino.ID)
        txtNote.Text = Me.MyCliente.Note
        CaricaOrganizzazione(Me.MyCliente.Azienda.ID)
        txtRagSoc.Text = Me.MyCliente.RagSoc
        CaricaSubOrganizzazione(Me.MyCliente.SubAzienda.ID)
        txtVicinoa.Text = Me.MyCliente.VicinoA

        Panel2.Visible = True
        Panel3.Visible = True

        Panel1.Visible = False
        PanelModificaCliente.Visible = True
        PanelRicercaCliente.Visible = False
        Me.DisabilitaCampi()
        Me.CaricaTipologiaContatto()
        Me.CaricaContatti()
        Me.CaricaContratto()
        Select Case Session("tipoutente")
            Case "Utente"
                btnModifica.Visible = False
                btnAggiungi.Visible = False
                If Session("isadmin") = 1 Then
                    btnModifica.Visible = True
                End If
            Case Else
                btnModifica.Visible = True
                btnAggiungi.Visible = True
        End Select
    End Sub

    Private Sub ListView3_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView3.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn.Click, AddressOf ModificaContratto

        Dim btn1 As ImageButton = e.Item.FindControl("imgLente")
        AddHandler btn1.Click, AddressOf ModificaContratto
    End Sub

    Private Sub ModificaContratto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Dim str = "idcliente=" & lblIdCliente.Text & "&tipocliente=cliente&operazione=modifica&idcontratto=" & bt.AlternateText
        Dim miaStringaCriptata As String
        miaStringaCriptata = VSTripleDES.EncryptData(str)
        Response.Redirect("contratto.aspx?" & miaStringaCriptata)
    End Sub

    Private Sub CaricaBloccoAmm()
        ddlBloccoAmm.Items.Add("no")
        ddlBloccoAmm.Items.Add("si")
    End Sub

    Private Sub CaricaListino(Optional ByVal idl As String = "-1")
        Dim str As String = "select descrizione ,id from Listino"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlListino.DataSource = tab
        Me.ddlListino.DataTextField = "descrizione"
        Me.ddlListino.DataValueField = "id"
        Me.ddlListino.DataBind()
        If idl <> "-1" Then
            Me.ddlListino.SelectedValue = idl
        Else
            Me.ddlListino.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CancellaCliente(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdCliente.Text = bt.AlternateText
        PanelEliminaCliente.Visible = True
        PanelRicercaCliente.Visible = False
        Me.MyCliente = New Cliente
        Me.MyCliente.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyCliente.Codice
        lblClienteElimina.Text = Me.MyCliente.RagSoc
        lblPartitaIvaElimina.Text = Me.MyCliente.Pariva
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
        If idr <> "-1" Then
            str = str & " inner join Regioni on Province.idRegione=Regioni.idRegione where Regioni.idRegione=" & idr
        End If
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
    '    If idp <> "-1" Then
    '        str = str & " inner join Province on Comuni.idProvincia=Province.idProvincia where Province.idProvincia=" & idp
    '    End If
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
        PanelModificaCliente.Visible = False
        Panel1.Visible = True
        PanelRicercaCliente.Visible = True
    End Sub

    Protected Sub btnRicercaCliente_Click(sender As Object, e As EventArgs) Handles btnRicercaCliente.Click
        Me.CaricaClienti(txtRicercaCliente.Text)
        Panel1.Visible = True
        PanelModificaCliente.Visible = False
    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If Me.IsValid Then
            Me.MyCliente = New Cliente
            Me.MyCliente.Azienda = New Azienda
            Me.MyCliente.SubAzienda = New SubAzienda
            Me.MyCliente.Listino = New Listino

            Me.MyCliente.Load(lblIdCliente.Text)
            Me.MyCliente.IDRegione = ddlRegione.SelectedValue
            Me.MyCliente.IDProvincia = ddlProvincia.SelectedValue
            Me.MyCliente.IDComune = ddlComune.Text
            Me.MyCliente.Azienda.ID = ddlOrganizzazione.SelectedValue
            If ddlSubOrganizzazione.SelectedValue <> "" Then
                Me.MyCliente.SubAzienda.ID = ddlSubOrganizzazione.SelectedValue
            Else
                Me.MyCliente.SubAzienda.ID = -1
            End If
            Me.MyCliente.Blocco_Amm = ddlBloccoAmm.SelectedIndex
            Me.MyCliente.Listino.ID = ddlListino.SelectedValue
            Me.MyCliente.Note = txtNote.Text
            Me.MyCliente.Committenza = txtCommittenza.Text
            Me.MyCliente.CodEst = txtCodEst.Text
            Me.MyCliente.Cap = txtCap.Text
            Me.MyCliente.Codice = txtCodice.Text
            Me.MyCliente.Indirizzo = txtIndirizzo.Text
            Me.MyCliente.Pariva = txtPiva.Text
            Me.MyCliente.RagSoc = txtRagSoc.Text
            Me.MyCliente.VicinoA = txtVicinoa.Text
            Me.MyCliente.SalvaData()

            lblIdCliente.Text = Me.MyCliente.ID
            Dim message2 As String = "Cliente creato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            'Threading.Thread.Sleep(5000)
            Me.CaricaClienti()
            Panel1.Visible = True
            PanelRicercaCliente.Visible = True
            PanelModificaCliente.Visible = False
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
        lblIdCliente.Text = "-1"

        txtCodice.Text = ""
        txtCap.Text = ""
        txtIndirizzo.Text = ""
        txtPiva.Text = ""
        txtIndirizzo.Text = ""
        Me.CaricaRegione()
        Me.CaricaProvincia()
        'Me.CaricaComune()
        ddlComune.Text = ""
        ddlBloccoAmm.SelectedIndex = 0
        txtCodEst.Text = ""
        txtCommittenza.Text = ""
        CaricaListino()
        txtNote.Text = ""
        CaricaSubOrganizzazione()
        CaricaOrganizzazione()
        txtRagSoc.Text = ""
        txtVicinoa.Text = ""
        lblConferma.Text = ""
    End Sub

    Private Sub InserisciCodice()
        Dim idmax
        Dim sql As String = "select Max(id),codice from Cliente group by id,codice"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Substring(1))
        Else
            idmax = 0
        End If
        idmax = "C" & idmax + 1
        txtCodice.Text = idmax
    End Sub

    Private Sub AbilitaCampi()
        txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        txtCodEst.ReadOnly = False
        txtCommittenza.ReadOnly = False
        txtNote.ReadOnly = False
        txtIndirizzo.ReadOnly = False
        txtPiva.ReadOnly = False
        txtVicinoa.ReadOnly = False
        txtRagSoc.ReadOnly = False

        btnModifica.Visible = False
        btnSalva.Visible = True

        ddlBloccoAmm.Enabled = True
        ddlComune.Enabled = True
        ddlProvincia.Enabled = True
        ddlRegione.Enabled = True
        ddlListino.Enabled = True
        ddlOrganizzazione.Enabled = True
        ddlSubOrganizzazione.Enabled = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCap.ReadOnly = True
        'txtCodice.ReadOnly = True
        txtCodEst.ReadOnly = True
        txtCommittenza.ReadOnly = True
        txtNote.ReadOnly = True
        txtVicinoa.ReadOnly = True
        txtIndirizzo.ReadOnly = True
        txtPiva.ReadOnly = True

        txtRagSoc.ReadOnly = True

        btnModifica.Visible = True
        btnSalva.Visible = False

        ddlBloccoAmm.Enabled = False
        ddlComune.Enabled = False
        ddlProvincia.Enabled = False
        ddlRegione.Enabled = False
        ddlListino.Enabled = False
        ddlOrganizzazione.Enabled = False
        ddlSubOrganizzazione.Enabled = False
    End Sub

    Private Sub CaricaOrganizzazione(Optional ByVal ido As String = "-1")
        Dim str As String = "select Azienda.descrizione ,Azienda.id from Azienda "
        If Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            str = str & " inner join Cliente on Cliente.idAzienda=Azienda.id where Cliente.id=" & Me.MyUtente.Cliente.ID
        ElseIf Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where Azienda.id=" & MyUtente.Azienda.ID
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlOrganizzazione.DataSource = tab
        Me.ddlOrganizzazione.DataTextField = "descrizione"
        Me.ddlOrganizzazione.DataValueField = "id"
        Me.ddlOrganizzazione.DataBind()
        If ido <> "-1" Then
            Me.ddlOrganizzazione.SelectedValue = ido
        ElseIf tab.Rows(0)("id") <> "-1" Then
            Me.ddlOrganizzazione.SelectedValue = tab.Rows(0)("id")
            Me.CaricaSubOrganizzazione(ddlOrganizzazione.SelectedValue)
        Else
            Me.ddlOrganizzazione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaSubOrganizzazione(Optional ByVal ido As String = "-1", Optional ByVal idso As String = "-1")
        Dim str As String = "select SubAzienda.descrizione as nomeSubAzienda ,SubAzienda.id as idSubAzienda from SubAzienda"
        'If ido <> "-1" Then
        str = str & " inner join Azienda on SubAzienda.idAzienda=Azienda.id where Azienda.id=" & ido
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlSubOrganizzazione.DataSource = tab
        Me.ddlSubOrganizzazione.DataTextField = "nomeSubAzienda"
        Me.ddlSubOrganizzazione.DataValueField = "idSubAzienda"
        Me.ddlSubOrganizzazione.DataBind()
        If idso <> "-1" Then
            Me.ddlSubOrganizzazione.SelectedValue = idso
        ElseIf tab.Rows(0)("idSubAzienda") <> "-1" Then
            Me.ddlSubOrganizzazione.SelectedValue = tab.Rows(0)("idSubAzienda")
        Else
            Me.ddlSubOrganizzazione.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub ddlOrganizzazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOrganizzazione.SelectedIndexChanged
        Me.CaricaSubOrganizzazione(ddlOrganizzazione.SelectedValue)
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
            Me.MyRecapito.IDTipo = ddlTipologiaContatto.SelectedValue
            Me.MyRecapito.Descrizione = txtDescrizioneContatto.Text
            Me.MyRecapito.SalvaData()
            If lblIdRecapito.Text = "-1" Then
                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdCliente.Text & "'," & Me.MyRecapito.ID & ",'Cliente')"
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
        Me.CaricaContatti()
    End Sub

    Private Sub CaricaContatti()
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,TipoContatto.descrizione as descr " & _
                     "FROM Recapito inner join Recapito_Utente on Recapito_Utente.idRecapito=Recapito.id " & _
                     "inner join Cliente on Cliente.id=Recapito_Utente.idUtente " & _
                     "inner join TipoContatto on TipoContatto.id=Recapito.idtipo " & _
                     "where Recapito_Utente.idUtente = " & lblIdCliente.Text & " and Recapito_Utente.tipoAssociazione like 'Cliente' "

        tab = MyGest.GetTab(sqlStr)

        ListView2.DataSource = tab
        ListView2.DataBind()

        If tab.Rows.Count > 100 Then
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
        btnAnnulla1.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True
        btnAnnullaContatto.Visible = True
    End Sub

    Private Sub ModificaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyRecapito = New Recapito
        lblIdRecapito.Text = bt.AlternateText
        Me.MyRecapito.Load(lblIdRecapito.Text)
        txtContatto.Text = Me.MyRecapito.Contatto
        ddlTipologiaContatto.SelectedValue = Me.MyRecapito.IDTipo
        txtDescrizioneContatto.Text = Me.MyRecapito.Descrizione
        btnMemorizza.Visible = True
        btnAnnulla.Visible = True
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
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
        Dim message As String = "Contatto eliminato correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Me.SvuotaCampiRecapito()
        Me.CaricaContatti()

        btnAnnullaContatto.Visible = False
        btnElimina.Visible = False
        lblSicuro.Visible = False
        btnAnnulla1.Visible = True
        btnMemorizza.Visible = True
        lblIdRecapito.Text = "-1"
    End Sub



    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaCliente.Visible = False
        PanelRicercaCliente.Visible = True
        lblIdCliente.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyCliente = New Cliente
        If Me.MyCliente.Delete(lblIdCliente.Text) Then
            Me.CaricaClienti()
            lblIdCliente.Text = "-1"
            PanelEliminaCliente.Visible = False
            PanelRicercaCliente.Visible = True
            Dim message As String = "Cliente eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        Panel2.Visible = False
        Panel3.Visible = False
        Me.SvuotaCampi()
        Me.SvuotaCampiRecapito()
        PanelModificaCliente.Visible = True
        Panel1.Visible = False
        Me.AbilitaCampi()
        Me.CaricaRegione()
        Me.CaricaOrganizzazione()
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

    Protected Sub btnAggiungi_Click(sender As Object, e As EventArgs) Handles btnAggiungi.Click
        Dim str = "idcliente=" & lblIdCliente.Text & "&tipocliente=cliente&operazione=aggiungi"
        Dim miaStringaCriptata As String
        miaStringaCriptata = VSTripleDES.EncryptData(str)
        Response.Redirect("contratto.aspx?" & miaStringaCriptata)
    End Sub

    Protected Sub lbContratti_Click(sender As Object, e As EventArgs) Handles lbContratti.Click
        If Panel2.Visible = True Then
            Panel2.Visible = False
        Else
            Panel2.Visible = True
            Me.CaricaContatti()
        End If
    End Sub

    Protected Sub lbRecapiti_Click(sender As Object, e As EventArgs) Handles lbRecapiti.Click
        If Panel3.Visible = True Then
            Panel3.Visible = False
        Else
            Panel3.Visible = True
            Me.CaricaContatti()
        End If
    End Sub
End Class