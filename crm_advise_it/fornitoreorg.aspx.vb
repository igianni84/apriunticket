Public Class fornorg
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyFornitore As Fornitore
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Mygest = New MNGestione(CGlobal.cs)
        Mygest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaFornitore.Visible = True
                    PanelRicercaFornitore.Visible = True
                    Me.CaricaFornitore()
                    'Me.CaricaTipoFornitore()
                    'Me.CaricaCliente()
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    'Private Sub CaricaTipoFornitore()
    '    ddlFornitore.Items.Add("...")
    '    ddlFornitore.Items.Add("Fornitore Organizzazione")
    '    ddlFornitore.Items.Add("Fornitore Cliente")
    'End Sub
    'Private Sub CaricaCliente(Optional ByVal idc As String = "-1")
    '    Dim str As String = "select ragsoc ,id from Cliente"

    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlCliente.DataSource = tab
    '    Me.ddlCliente.DataTextField = "ragsoc"
    '    Me.ddlCliente.DataValueField = "id"
    '    Me.ddlCliente.DataBind()
    '    If idc <> "-1" Then
    '        Me.ddlCliente.SelectedValue = idc
    '    Else
    '        Me.ddlCliente.SelectedValue = "-1"
    '    End If
    'End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaFornitore(txtRicercaFornitore.Text)
    End Sub

    Private Sub CaricaFornitore(Optional ByVal ragsoc As String = "")
        Dim tab As New DataTable
        Dim bool As Boolean = False
        Dim sqlStr = "SELECT distinct(Fornitore.ragsoc),Fornitore.id ,Fornitore.codice,Fornitore.pariva,Fornitore.codiceest,Regioni.nome as regione,Province.nome as provincia " & _
                     "FROM Fornitore " & _
                     "inner join Regioni on Regioni.idRegione=Fornitore.idregione " & _
                     "inner join Province on Province.idProvincia=Fornitore.idProvincia " & _
                     "where codice like 'FO%'"

        'sqlStr = sqlStr & "inner join Fornitore_Esterno on Fornitore_Esterno.idfornitore=Fornitore.id " & _
        '                          "where Fornitore_Esterno.tipo like '%Clienti%' "

        'sqlStr = sqlStr & "inner join Fornitore_Esterno on Fornitore_Esterno.idfornitore=Fornitore.id " & _
        '                  "where Fornitore_Esterno.tipo like '%Organizzazioni%' "
        'End If
        bool = True
        'End If
        If ragsoc <> "" Then
            If bool Then
                sqlStr = sqlStr & "and "
            Else
                sqlStr = sqlStr & "where "
            End If
            sqlStr = sqlStr & " ragsoc like '%" & ragsoc & "%'"
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
        AddHandler btn.Click, AddressOf CancellaFornitore

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaFornitore

        Dim btn2 As ImageButton = e.Item.FindControl("imgLega")
        AddHandler btn2.Click, AddressOf LegaFornitore
    End Sub

    Private Sub ModificaFornitore(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyFornitore = New Fornitore
        lblIdFornitore.Text = bt.AlternateText
        Me.MyFornitore.Load(lblIdFornitore.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo

        txtCodice.Text = Me.MyFornitore.Codice
        txtCap.Text = Me.MyFornitore.Cap
        txtIndirizzo.Text = Me.MyFornitore.Indirizzo
        txtPiva.Text = Me.MyFornitore.Pariva
        txtIndirizzo.Text = Me.MyFornitore.Indirizzo
        Me.CaricaRegione(Me.MyFornitore.IDRegione)
        Me.CaricaProvincia(Me.MyFornitore.IDProvincia, Me.MyFornitore.IDRegione)
        'Me.CaricaComune(Me.MyFornitore.IDComune, Me.MyFornitore.IDProvincia)
        ddlComune.Text = Me.MyFornitore.IDComune
        txtCodEst.Text = Me.MyFornitore.Codest


        'Me.CaricaCliente(Me.MyFornCliente.Cliente.ID)
        txtNote.Text = Me.MyFornitore.Note
        'CaricaOrganizzazione(Me.MyFornCliente.Azienda.ID)
        txtRagSoc.Text = Me.MyFornitore.RagSoc
        'CaricaSubOrganizzazione(Me.MyFornCliente.SubAzienda.ID)

        PanelRicercaFornitore.Visible = False
        Panel1.Visible = False
        PanelModificaFornitore.Visible = True
        Me.DisabilitaCampi()
    End Sub

    Private Sub LegaFornitore(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdFornitore.Text = bt.AlternateText
        PanelAggiungiFornitore.Visible = True
        Panel1.Visible = False
        PanelRicercaFornitore.Visible = False
        Me.CaricaLegami()

    End Sub

    Private Sub Caricalegami()
        Dim sql As String
        Dim tab As DataTable
        Dim TAB2 As DataTable
        If Session("tipoutente") = "SuperAdmin" Then
            sql = "SELECT * FROM Fornitore_Esterno where Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text
            tab = Me.MyGest.GetTab(sql)
            If tab.Rows.Count > 0 Then


                sql = "select Fornitore_Esterno.id,Azienda.descrizione as ragsoc from Fornitore_Esterno " & _
                      "inner join Azienda on Azienda.id=Fornitore_Esterno.idesterno " & _
                      "where Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & " and tipo like 'Organizzazioni' " & _
                      "UNION " & _
                      "select Fornitore_Esterno.id,SubAzienda.descrizione as ragsoc from Fornitore_Esterno " & _
                      "inner join SubAzienda on SubAzienda.id=Fornitore_Esterno.idesterno " & _
                      "where Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & " and tipo like 'SubOrganizzazioni' "
                '      "UNION " & _
                'sql = "select Fornitore_Esterno.id,Cliente.ragsoc from Fornitore_Esterno " & _
                '      "inner join Cliente on Cliente.id=Fornitore_Esterno.idesterno " & _
                '      "where Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & " and tipo like 'Clienti' " & _
                '      "UNION " & _
                '      "select Fornitore_Esterno.id,SubCliente.ragsoc from Fornitore_Esterno " & _
                '      "inner join SubCliente on SubCliente.id=Fornitore_Esterno.idesterno " & _
                '      "where Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & " and tipo like 'SubClienti' "
            End If
        ElseIf Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sql = "select Fornitore_Esterno.id,Azienda.descrizione as ragsoc from Fornitore_Esterno " & _
                   "inner join Azienda on Azienda.id=Fornitore_Esterno.idesterno " & _
                   "where Azienda.id = " & Me.MyUtente.Azienda.ID & " and tipo like 'Organizzazioni' " & _
                   "and Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & _
                   "UNION " & _
                   "select Fornitore_Esterno.id,SubAzienda.descrizione as ragsoc from Fornitore_Esterno " & _
                   "inner join SubAzienda on SubAzienda.id=Fornitore_Esterno.idesterno " & _
                   "where SubAZienda.id = " & Me.MyUtente.Azienda.ID & " and tipo like 'SubOrganizzazioni' " & _
                   "and Fornitore_Esterno.idfornitore = " & lblIdFornitore.Text & ""
        End If
        TAB2 = Me.MyGest.GetTab(sql)
        Me.BulletedList1.DataValueField = "id"
        Me.BulletedList1.DataTextField = "ragsoc"
        BulletedList1.DataSource = TAB2
        BulletedList1.DataBind()




    End Sub

    Private Sub CaricaOrganizzazione()
        Dim str As String = "select Azienda.descrizione ,id from Azienda "
        If Session("tipoutente") = "Operatore" Then
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

        Me.ddldrop.DataSource = tab
        Me.ddldrop.DataTextField = "descrizione"
        Me.ddldrop.DataValueField = "id"
        Me.ddldrop.DataBind()
        Me.ddldrop.SelectedValue = "-1"
    End Sub

    Private Sub CaricaSubOrganizzazione()
        Dim str As String = "select SubAzienda.descrizione ,id from SubAzienda "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where SubAzienda.idazienda=" & MyUtente.Azienda.ID

        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddldrop.DataSource = tab
        Me.ddldrop.DataTextField = "descrizione"
        Me.ddldrop.DataValueField = "id"
        Me.ddldrop.DataBind()
        Me.ddldrop.SelectedValue = "-1"
    End Sub

    'Private Sub CaricaCliente()
    '    Dim str As String = "select Cliente.ragsoc ,id from Cliente"

    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddldrop.DataSource = tab
    '    Me.ddldrop.DataTextField = "ragsoc"
    '    Me.ddldrop.DataValueField = "id"
    '    Me.ddldrop.DataBind()
    '    Me.ddldrop.SelectedValue = "-1"
    'End Sub

    'Private Sub CaricaSubCliente()
    '    Dim str As String = "select SubCliente.ragsoc ,id from SubCliente"

    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddldrop.DataSource = tab
    '    Me.ddldrop.DataTextField = "ragsoc"
    '    Me.ddldrop.DataValueField = "id"
    '    Me.ddldrop.DataBind()
    '    Me.ddldrop.SelectedValue = "-1"
    'End Sub

    Private Sub CancellaFornitore(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdFornitore.Text = bt.AlternateText
        PanelEliminaOrg.Visible = True
        PanelRicercaFornitore.Visible = False
        Me.MyFornitore = New Fornitore
        Me.MyFornitore.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyFornitore.Codice
        lblFornitoreElimina.Text = Me.MyFornitore.RagSoc
        lblPartitaIvaElimina.Text = Me.MyFornitore.Pariva
    End Sub

    Private Sub CaricaRegione(Optional ByVal idr As String = "-1")
        Dim str As String = "select nome ,idRegione from Regioni"

        Dim tab As DataTable
        tab = Me.Mygest.GetTab(str)
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
        PanelModificaFornitore.Visible = False
        Panel1.Visible = True
        PanelRicercaFornitore.Visible = True
    End Sub

    Protected Sub btnRicercaCliente_Click(sender As Object, e As EventArgs) Handles btnRicercaFornitore.Click
        Me.CaricaFornitore(txtRicercaFornitore.Text)
        Panel1.Visible = True
        PanelModificaFornitore.Visible = False
    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If Me.IsValid Then

            Me.MyFornitore = New Fornitore
            'Me.MyFornCliente.Cliente = New Cliente
            'Me.MySubCliente.Azienda = New Azienda
            'Me.MySubCliente.SubAzienda = New SubAzienda


            Me.MyFornitore.Load(lblIdFornitore.Text)
            Me.MyFornitore.IDRegione = ddlRegione.SelectedValue
            Me.MyFornitore.IDProvincia = ddlProvincia.SelectedValue
            Me.MyFornitore.IDComune = ddlComune.Text
            'Me.MyFornCliente.Azienda.ID = ddlOrganizzazione.SelectedValue
            ' Me.MySubCliente.Cliente.ID = ddlCliente.SelectedValue
            'If ddlSubOrganizzazione.SelectedValue <> "" Then
            '    Me.MySubCliente.SubAzienda.ID = ddlSubOrganizzazione.SelectedValue
            'Else
            '    Me.MySubCliente.SubAzienda.ID = -1
            'End If

            Me.MyFornitore.Note = txtNote.Text
            Me.MyFornitore.Tipo = ""
            Me.MyFornitore.Codest = txtCodEst.Text
            Me.MyFornitore.Cap = txtCap.Text
            Me.MyFornitore.Codice = txtCodice.Text
            Me.MyFornitore.Indirizzo = txtIndirizzo.Text
            Me.MyFornitore.Pariva = txtPiva.Text
            Me.MyFornitore.RagSoc = txtRagSoc.Text
            Me.MyFornitore.SalvaData()

            lblIdFornitore.Text = Me.MyFornitore.ID
            Dim message2 As String = "Fornitore Organizzazione creato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            'Threading.Thread.Sleep(5000)
            Me.CaricaFornitore()
            Panel1.Visible = True
            PanelRicercaFornitore.Visible = True
            PanelModificaFornitore.Visible = False
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

    Private Sub InserisciCodice()
        Dim idmax
        Dim sql As String = "select Max(id),codice from Fornitore where codice like '%FO%' group by id,codice"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Substring(2))
        Else
            idmax = 0
        End If
        idmax = "FO" & idmax + 1
        txtCodice.Text = idmax
    End Sub

    Private Sub AbilitaCampi()
        txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        txtCodEst.ReadOnly = False

        txtNote.ReadOnly = False
        txtIndirizzo.ReadOnly = False
        txtPiva.ReadOnly = False

        txtRagSoc.ReadOnly = False

        btnModifica.Visible = False
        btnSalva.Visible = True


        ddlComune.Enabled = True
        ddlProvincia.Enabled = True
        ddlRegione.Enabled = True

        'ddlOrganizzazione.Enabled = True
        'ddlSubOrganizzazione.Enabled = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCap.ReadOnly = True
        'txtCodice.ReadOnly = True
        txtCodEst.ReadOnly = True

        txtNote.ReadOnly = True

        txtIndirizzo.ReadOnly = True
        txtPiva.ReadOnly = True

        txtRagSoc.ReadOnly = True

        btnModifica.Visible = True
        btnSalva.Visible = False


        ddlComune.Enabled = False
        ddlProvincia.Enabled = False
        ddlRegione.Enabled = False

        'ddlOrganizzazione.Enabled = False
        'ddlSubOrganizzazione.Enabled = False
    End Sub

    'Private Sub CaricaOrganizzazione(Optional ByVal ido As String = "-1")
    '    Dim str As String = "select descrizione ,id from Azienda"

    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlOrganizzazione.DataSource = tab
    '    Me.ddlOrganizzazione.DataTextField = "descrizione"
    '    Me.ddlOrganizzazione.DataValueField = "id"
    '    Me.ddlOrganizzazione.DataBind()
    '    If ido <> "-1" Then
    '        Me.ddlOrganizzazione.SelectedValue = ido
    '    Else
    '        Me.ddlOrganizzazione.SelectedValue = "-1"
    '    End If
    'End Sub

    'Private Sub CaricaSubOrganizzazione(Optional ByVal ido As String = "-1", Optional ByVal idso As String = "-1")
    '    Dim str As String = "select SubAzienda.descrizione as nomeSubAzienda ,SubAzienda.id as idSubAzienda from SubAzienda"
    '    If ido <> "-1" Then
    '        str = str & " inner join Azienda on SubAzienda.idAzienda=Azienda.id where Azienda.id=" & ido
    '    End If
    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlSubOrganizzazione.DataSource = tab
    '    Me.ddlSubOrganizzazione.DataTextField = "nomeSubAzienda"
    '    Me.ddlSubOrganizzazione.DataValueField = "idSubAzienda"
    '    Me.ddlSubOrganizzazione.DataBind()
    '    If idso <> "-1" Then
    '        Me.ddlSubOrganizzazione.SelectedValue = idso
    '    Else
    '        Me.ddlSubOrganizzazione.SelectedValue = "-1"
    '    End If
    'End Sub

    'Protected Sub ddlOrganizzazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOrganizzazione.SelectedIndexChanged
    '    Me.CaricaSubOrganizzazione(ddlOrganizzazione.SelectedValue)
    'End Sub


    'Protected Sub lbCliente_Click(sender As Object, e As EventArgs) Handles lbCliente.Click
    '    lbldrop.Text = "Clienti"
    '    Me.CaricaCliente()
    'End Sub

    'Protected Sub lbSubCliente_Click(sender As Object, e As EventArgs) Handles lbSubCliente.Click
    '    lbldrop.Text = "SubClienti"
    '    Me.CaricaSubCliente()
    'End Sub

    Protected Sub lbOrganizzazione_Click(sender As Object, e As EventArgs) Handles lbOrganizzazione.Click
        lbldrop.Text = "Organizzazioni"
        Me.CaricaOrganizzazione()
    End Sub

    Protected Sub lbSubOrganizzazione_Click(sender As Object, e As EventArgs) Handles lbSubOrganizzazione.Click
        lbldrop.Text = "SubOrganizzazioni"
        Me.CaricaSubOrganizzazione()
    End Sub

    Protected Sub imgAggiungi_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgAggiungi.Click
        If ddldrop.SelectedValue <> "-1" Then
            If Me.MemorizzaAssociazione() Then
                BulletedList1.Items.Add(ddldrop.SelectedItem)
                ddldrop.SelectedValue = "-1"
            End If
        End If
    End Sub

    Private Function MemorizzaAssociazione()
        Dim risposta As Boolean = False
        Try
            Dim sql As String = "Insert Into Fornitore_Esterno values('" & lbldrop.Text & "'," & ddldrop.SelectedValue & "," & lblIdFornitore.Text & ")"
            MyGest.GetReader(sql)
            risposta = True
        Catch
        End Try
        Return risposta
    End Function

    Protected Sub BulletedList1_Click(sender As Object, e As System.Web.UI.WebControls.BulletedListEventArgs) Handles BulletedList1.Click
        Dim sql As String = ""

        Try
            sql = "Delete Fornitore_Esterno where idfornitore=" & lblIdFornitore.Text & " and id=" & BulletedList1.Items(e.Index).Value
            MyGest.GetTab(sql)

            BulletedList1.Items.Clear()
            Me.Caricalegami()
        Catch
        End Try


    End Sub

    'Protected Sub ddlFornitore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFornitore.SelectedIndexChanged
    '    Me.CaricaFornitore()
    'End Sub

    Private Sub SvuotaCampi()
        txtCodice.Text = ""
        txtCap.Text = ""
        txtRagSoc.Text = ""
        Me.CaricaRegione()
        Me.CaricaProvincia()
        'Me.CaricaComune()
        ddlComune.Text = ""
        txtPiva.Text = ""
        txtCodEst.Text = ""
        txtNote.Text = ""
        txtIndirizzo.Text = ""
        imgLogo.ImageUrl = ""
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        PanelModificaFornitore.Visible = True
        Panel1.Visible = False
        Me.svuotacampi()
        Me.AbilitaCampi()
        Me.CaricaRegione()
        'Me.CaricaOrganizzazione()
        Me.InserisciCodice()
    End Sub

    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaOrg.Visible = False
        PanelRicercaFornitore.Visible = True
        lblIdFornitore.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyFornitore = New Fornitore
        If Me.MyFornitore.Delete(lblIdFornitore.Text) Then
            Me.CaricaFornitore()
            lblIdFornitore.Text = "-1"
            PanelEliminaOrg.Visible = False
            PanelRicercaFornitore.Visible = True
            Dim message As String = "Fornitore Organizzazione eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub
End Class