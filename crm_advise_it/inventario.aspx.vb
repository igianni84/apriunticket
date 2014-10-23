Public Class Inventario
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyAzienda As Azienda
    Private MyCredenziali As Credenziali
    Private MyInventario As Inventari
    Private MyCliente As Cliente
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        'Me.Form.Enctype = "multipart/form-data"
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaInventario.Visible = True
                    Me.CaricaInventari()
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    'Private Sub CaricaInventario()
    '    Dim sql As String = "select * from Inventario inner join TipoDispositivo on TipoDispositivo.id=Iventario.idtipodispositivo inner join Cliente on Cliente.id=Inventario.idCliente"
    '    Dim tab As DataTable = Me.MyGest.GetTab(sql)
    '    ListView1.DataSource = tab
    '    ListView1.DataBind()
    '    If tab.Rows.Count > 10 Then
    '        DataPager1.Visible = True
    '    Else
    '        DataPager1.Visible = False
    '    End If
    'End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaInventari(txtCodiceRicerca.Text, txtDescrizioneRicerca.Text, txtSerialeRicerca.Text, txtTipoRicerca.Text)
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaCredenziali()
    End Sub


    Private Sub CaricaInventari(Optional ByVal codice As String = "", Optional ByVal descrizione As String = "", Optional ByVal seriale As String = "", Optional ByVal tipo As String = "")
        btNuovo.Visible = True
        lblSicuro.Visible = False
        Dim tab As New DataTable
        Dim azienda
        Dim sqlStr = "SELECT *,Cliente.ragsoc as ragsoccli,SubCliente.ragsoc as ragsocsubcli " & _
                     "FROM Inventario inner join Cliente on Cliente.id=Inventario.idcliente " & _
                     "inner join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
                     "left outer join Marchio on Marchio.id=Inventario.idMarchio " & _
                     "left outer join Modello on Modello.id=Inventario.idModello " & _
                     "left outer join SubCliente on SubCliente.id=Inventario.idsubcliente " & _
                     "where Inventario.id<>-1 "
        Select Case Session("tipoutente")
            Case "SuperAdmin"
            Case "Operatore"
                Me.MyUtente = New Utente
                Me.MyUtente.Load(Session("id"))
                If Me.MyUtente.SubAzienda.ID = "-1" Then
                    azienda = Me.MyUtente.Azienda.ID
                    sqlStr = sqlStr & "and Inventario.idazienda='" & azienda & "' "
                Else
                    azienda = Me.MyUtente.SubAzienda.ID
                    sqlStr = sqlStr & "and Inventario.idsubazienda='" & azienda & "' "
                End If
            Case "Utente"
                btNuovo.Visible = False
                    Dim cliente
                    Me.MyUtente = New Utente
                    Me.MyUtente.Load(Session("id"))
                If Me.MyUtente.SubCliente.ID = "-1" Then
                    cliente = Me.MyUtente.Cliente.ID
                    sqlStr = sqlStr & "and Inventario.idcliente='" & cliente & "' "
                Else
                    cliente = Me.MyUtente.SubCliente.ID
                    sqlStr = sqlStr & "and Inventario.idsubcliente='" & cliente & "' or (Inventario.idcliente=" & Me.MyUtente.Cliente.ID & " and Inventario.mobile=1) "

                End If

        End Select


        If codice <> "" Then
            sqlStr = sqlStr & "and Inventario.codice like '%" & codice & "%'"
        End If
        If descrizione <> "" Then
            sqlStr = sqlStr & "and Inventario.descrizione like '%" & descrizione & "%'"
        End If
        If descrizione <> "" Then
            sqlStr = sqlStr & "and Inventario.seriale like '%" & seriale & "%'"
        End If
        If descrizione <> "" Then
            sqlStr = sqlStr & "and Inventario.tipo like '%" & tipo & "%'"
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
        AddHandler btn.Click, AddressOf CancellaInventario

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaInventario

        Dim btn2 As ImageButton = e.Item.FindControl("imgDettagli")
        AddHandler btn2.Click, AddressOf ModificaInventario

        Dim btn3 As ImageButton = e.Item.FindControl("imgApriTicket")
        AddHandler btn3.Click, AddressOf apriticket
    End Sub

    Private Sub ApriTicket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Dim str = "id=" & bt.AlternateText & "&tipo=inventario"
        Dim miaStringaCriptata As String
        miaStringaCriptata = VSTripleDES.EncryptData(str)
        Response.Redirect("apriticket.aspx?" & miaStringaCriptata)
    End Sub

    Private Sub ModificaInventario(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyInventario = New Inventari
        lblIdInventario.Text = bt.AlternateText
        Me.MyInventario.Load(lblIdInventario.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo
        txtCodice.Visible = True
        lblCodice.Visible = True
        txtCodice.Text = Me.MyInventario.Codice
        'Me.CaricaOrganizzazione(Me.MyInventario.Azienda.ID)
        'Me.CaricaSubOrganizzazione(Me.MyInventario.SubAzienda.ID)
        Me.CaricaCliente(Me.MyInventario.Cliente.ID)
        Me.CaricaSubCliente(Me.MyInventario.SubCliente.ID, Me.MyInventario.Cliente.ID)
        txtseriale.Text = Me.MyInventario.Seriale
        Me.CaricaTipoDispositivo(Me.MyInventario.TipoDispositivo.ID)
        Me.CaricaMarchio(Me.MyInventario.Marchio.ID, Me.MyInventario.TipoDispositivo.ID)
        Me.CaricaModello(Me.MyInventario.Modello.ID, Me.MyInventario.Marchio.ID)
        txtdescrizione.Text = Me.MyInventario.Descrizione
        Me.CaricaUtente(Me.MyInventario.Utente.ID, Me.MyInventario.SubCliente.ID, Me.MyInventario.Cliente.ID)
        txtubicazione.Text = Me.MyInventario.Ubicazione
        txtip.Text = Me.MyInventario.IP
        txtsubnet.Text = Me.MyInventario.Subnet
        txtgateway.Text = Me.MyInventario.Gateway
        txtNote.Text = Me.MyInventario.Note
        Me.CaricaFornitoreCli(Me.MyInventario.FornitoreCli.ID)
        Me.CaricaFornitoreOrg(Me.MyInventario.FornitoreOrg.ID)
        If Me.MyInventario.Mobile = 1 Then
            cbxMobile.Checked = True
        Else
            cbxMobile.Checked = False
        End If

        If Me.MyInventario.DataScad <> "01/01/9999" Then
            txtDataSca.Text = Me.MyInventario.DataScad
        Else
            txtDataSca.Text = ""
        End If

        Panel1.Visible = False
        PanelModificaInventario.Visible = True
        PanelCredenziali.Visible = True
        PanelBarra.Visible = True
        PanelRicercaInventario.Visible = False
        Me.DisabilitaCampi()

        Select Case Session("tipoutente")
            Case "Utente"
                btnModifica.Visible = False
                PanelCredenziali.Visible = False
                If Session("isadmin") = 1 Then
                    PanelCredenziali.Visible = True
                    Me.CaricaCredenziali()
                End If
            Case Else
                Me.CaricaCredenziali()
                PanelCredenziali.Visible = True
        End Select

    End Sub

    Private Sub CancellaInventario(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdInventario.Text = bt.AlternateText
        PanelEliminaInv.Visible = True
        PanelRicercaInventario.Visible = False
        Me.MyInventario = New Inventari
        Me.MyInventario.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyInventario.Codice
        lblSerialeElimina.Text = Me.MyInventario.Seriale
        lblClienteElimina.Text = Me.MyInventario.Cliente.RagSoc
        lblSubClienteElimina.Text = Me.MyInventario.SubCliente.RagSoc
        'Dim message As String = "Cancellazione avvenuta correttamente"
        'ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopup('" + message + "');", True)
    End Sub

    Private Sub CaricaCliente(Optional ByVal idc As String = "-1")
        Dim str As String = "select ragsoc ,id from Cliente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where Cliente.idazienda=" & MyUtente.Azienda.ID
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlCliente.DataSource = tab
        Me.ddlCliente.DataTextField = "ragsoc"
        Me.ddlCliente.DataValueField = "id"
        Me.ddlCliente.DataBind()
        If idc <> "-1" Then
            Me.ddlCliente.SelectedValue = idc
        Else
            Me.ddlCliente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaUtente(Optional ByVal idu As String = "-1", Optional ByVal idsc As String = "-1", Optional ByVal idc As String = "-1")
        Dim str As String = "select cognome+' '+nome as utente ,Utente.id from Utente"
        If idsc <> "-1" Then
            str = str & " inner join SubCliente on Utente.idsubcliente=SubCliente.id where SubCliente.id=" & idsc
        Else
            str = str & " inner join Cliente on Utente.idcliente=Cliente.id where Cliente.id=" & idc
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlUtente.DataSource = tab
        Me.ddlUtente.DataTextField = "utente"
        Me.ddlUtente.DataValueField = "id"
        Me.ddlUtente.DataBind()
        If idu <> "-1" Then
            Me.ddlUtente.SelectedValue = idu
        Else
            Me.ddlUtente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaTipoDispositivo(Optional ByVal idtd As String = "-1")
        Dim str As String = "select tipodispositivo ,id from TipoDispositivo"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlDispositivo.DataSource = tab
        Me.ddlDispositivo.DataTextField = "tipodispositivo"
        Me.ddlDispositivo.DataValueField = "id"
        Me.ddlDispositivo.DataBind()
        If idtd <> "-1" Then
            Me.ddlDispositivo.SelectedValue = idtd
        Else
            Me.ddlDispositivo.SelectedValue = "-1"
        End If
    End Sub



    Private Sub CaricaMarchio(Optional ByVal idma As String = "-1", Optional ByVal idtd As String = "-1")
        Dim str As String = "select marchio ,Marchio.id from Marchio"
        'If idtd <> "-1" Then
        str = str & " inner join TipoDispositivo on Marchio.idtipodispositivo=TipoDispositivo.id where TipoDispositivo.id=" & idtd
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlMarchio.DataSource = tab
        Me.ddlMarchio.DataTextField = "marchio"
        Me.ddlMarchio.DataValueField = "id"
        Me.ddlMarchio.DataBind()
        If idma <> "-1" Then
            Me.ddlMarchio.SelectedValue = idma
        Else
            Me.ddlMarchio.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaModello(Optional ByVal idmo As String = "-1", Optional ByVal idma As String = "-1")
        Dim str As String = "select modello ,Modello.id from Modello"
        'If idma <> "-1" Then
        str = str & " inner join Marchio on Modello.idmarchio=Marchio.id where Marchio.id=" & idma
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlModello.DataSource = tab
        Me.ddlModello.DataTextField = "modello"
        Me.ddlModello.DataValueField = "id"
        Me.ddlModello.DataBind()
        If idmo <> "-1" Then
            Me.ddlModello.SelectedValue = idmo
        Else
            Me.ddlModello.SelectedValue = "-1"
        End If
    End Sub



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
        PanelModificaInventario.Visible = False
        PanelCredenziali.Visible = False
        Panel1.Visible = True
        PanelRicercaInventario.Visible = True
        Me.CaricaInventari(txtCodiceRicerca.Text, txtDescrizioneRicerca.Text, txtSerialeRicerca.Text, txtTipoRicerca.Text)

    End Sub

    Protected Sub btnRicercaInventario_Click(sender As Object, e As EventArgs) Handles btnRicercaInventario.Click
        Me.CaricaInventari(txtCodiceRicerca.Text, txtDescrizioneRicerca.Text, txtSerialeRicerca.Text, txtTipoRicerca.Text)
        Panel1.Visible = True
        PanelModificaInventario.Visible = False
        PanelCredenziali.Visible = False
    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If IsValid Then
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))

            Me.MyInventario = New Inventari
            Me.MyInventario.Cliente = New Cliente
            Me.MyInventario.SubCliente = New SubCliente
            Me.MyInventario.TipoDispositivo = New TipoDispositivo
            Me.MyInventario.Marchio = New Marchio
            Me.MyInventario.Modello = New Modello
            Me.MyInventario.Utente = New Utente
            Me.MyInventario.Azienda = New Azienda
            Me.MyInventario.SubAzienda = New SubAzienda
            Me.MyInventario.FornitoreCli = New Fornitore
            Me.MyInventario.FornitoreOrg = New Fornitore
            Me.MyInventario.Azienda.ID = MyUtente.Azienda.ID
            Me.MyInventario.SubAzienda.ID = MyUtente.SubAzienda.ID
            Me.MyInventario.Load(lblIdInventario.Text)
            Me.MyInventario.Cliente.ID = ddlCliente.SelectedValue
            If ddlSubCliente.SelectedValue <> "" Then
                Me.MyInventario.SubCliente.ID = ddlSubCliente.SelectedValue
            Else
                Me.MyInventario.SubCliente.ID = "-1"
            End If
            Me.MyInventario.TipoDispositivo.ID = ddlDispositivo.SelectedValue
            If ddlMarchio.SelectedValue <> "" Then
                Me.MyInventario.Marchio.ID = ddlMarchio.SelectedValue
            Else
                Me.MyInventario.Marchio.ID = "-1"
            End If

            If ddlModello.SelectedValue <> "" Then
                Me.MyInventario.Modello.ID = ddlModello.SelectedValue
            Else
                Me.MyInventario.Modello.ID = "-1"
            End If
            If txtCodice.Text = "" Then
                Me.CreaCodice()
            End If
            Me.MyInventario.Codice = txtCodice.Text
            If txtDataSca.Text <> "" Then
                Me.MyInventario.DataScad = CType(txtDataSca.Text, DateTime)
            Else
                Me.MyInventario.DataScad = "01/01/9999"
            End If

            Me.MyInventario.Descrizione = txtDescrizione.Text
            Me.MyInventario.Gateway = txtGateway.Text
            Me.MyInventario.IP = txtIp.Text
            Me.MyInventario.Note = txtNote.Text
            Me.MyInventario.Seriale = txtSeriale.Text
            Me.MyInventario.Subnet = txtSubNet.Text
            Me.MyInventario.Ubicazione = txtUbicazione.Text
            Me.MyInventario.FornitoreCli.ID = ddlFornitoreCli.SelectedValue
            Me.MyInventario.FornitoreOrg.ID = ddlFornitoreOrg.SelectedValue
            If cbxMobile.Checked Then
                Me.MyInventario.Mobile = 1
            Else
                Me.MyInventario.Mobile = 0
            End If
            Me.MyInventario.Utente.ID = ddlUtente.SelectedValue
            Me.MyInventario.SalvaData()

            lblIdInventario.Text = Me.MyInventario.ID
            'lblConferma.Visible = True
            'lblConferma.Text = "SubOrganizzazione Creata"
            '

            'Threading.Thread.Sleep(5000)

            Panel1.Visible = True
            PanelRicercaInventario.Visible = True
            PanelModificaInventario.Visible = False
            PanelCredenziali.Visible = False
            PanelBarra.Visible = False
            Me.CaricaInventari()
            Dim message2 As String = "Inventario creato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)

        Else

            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        End If
    End Sub

    Protected Sub ddlDispositivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDispositivo.SelectedIndexChanged
        Me.CaricaMarchio("-1", ddlDispositivo.SelectedValue)
        'Me.CaricaComune()
    End Sub

    Protected Sub ddlMarchio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarchio.SelectedIndexChanged
        Me.CaricaModello("-1", ddlMarchio.SelectedValue)
    End Sub



    Private Sub SvuotaCampi()
        lblIdInventario.Text = "-1"
        txtCodice.Text = ""
        'Me.CaricaOrganizzazione(Me.MyInventario.Azienda.ID)
        'Me.CaricaSubOrganizzazione(Me.MyInventario.SubAzienda.ID)
        Me.CaricaCliente()
        Me.CaricaSubCliente(, ddlCliente.SelectedValue)
        txtSeriale.Text = ""
        Me.CaricaTipoDispositivo()
        Me.CaricaMarchio(-1, ddlDispositivo.SelectedValue)
        Me.CaricaModello(-1, ddlMarchio.SelectedValue)
        txtDescrizione.Text = ""
        Me.CaricaUtente()

        Me.CaricaFornitoreCli()
        Me.CaricaFornitoreOrg()
        txtUbicazione.Text = ""
        txtIp.Text = ""
        txtSubNet.Text = ""
        txtGateway.Text = ""
        txtNote.Text = ""
        txtDataSca.Text = ""
        cbxMobile.Checked = False
    End Sub

    Private Sub InserisciCodice()
        'Dim idmax
        'Dim sl As String = "select * from Azienda where id=" & ddlAzienda.SelectedValue
        'Dim tb As DataTable = Me.MyGest.GetTab(sl)
        'Dim sql As String = "select Max(SubAzienda.id),SubAzienda.codice,Azienda.codice as cod from SubAzienda inner join Azienda on Azienda.id=SubAzienda.idAzienda where Azienda.id=" & ddlAzienda.SelectedValue & " group by SubAzienda.id,SubAzienda.codice,Azienda.codice "
        'Dim tab As DataTable = Me.MyGest.GetTab(sql)
        'If tab.Rows.Count > 0 Then
        '    idmax = CInt(tab.Rows(0).Item("codice").ToString.Split(".")(1).Substring(2))
        'Else
        '    idmax = -1
        'End If
        'If tb.Rows.Count > 0 Then
        '    idmax = tb.Rows(0).Item("codice") & ".SO" & idmax + 1
        'End If
        'txtCodice.Text = idmax
    End Sub

    Private Sub AbilitaCampi()
        'txtCodice.ReadOnly = False
        txtDataSca.ReadOnly = False
        txtDescrizione.ReadOnly = False
        txtGateway.ReadOnly = False
        txtIp.ReadOnly = False
        txtNote.ReadOnly = False
        txtSeriale.ReadOnly = False
        txtSubNet.ReadOnly = False
        txtUbicazione.ReadOnly = False
        cbxMobile.Enabled = True
        ddlFornitoreCli.Enabled = True
        ddlFornitoreOrg.Enabled = True
        ddlCliente.Enabled = True
        ddlDispositivo.Enabled = True
        ddlMarchio.Enabled = True
        ddlModello.Enabled = True
        ddlSubCliente.Enabled = True
        ddlUtente.Enabled = True
        'txtRicercaI.ReadOnly = False
        btnModifica.Visible = False

        btnSalva.Visible = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCodice.ReadOnly = True
        txtDataSca.ReadOnly = True
        txtDescrizione.ReadOnly = True
        txtGateway.ReadOnly = True
        txtIp.ReadOnly = True
        txtNote.ReadOnly = True
        txtSeriale.ReadOnly = True
        txtSubNet.ReadOnly = True
        txtUbicazione.ReadOnly = True

        cbxMobile.Enabled = False
        ddlFornitoreCli.Enabled = False
        ddlFornitoreOrg.Enabled = False
        ddlCliente.Enabled = False
        ddlDispositivo.Enabled = False
        ddlMarchio.Enabled = False
        ddlModello.Enabled = False
        ddlSubCliente.Enabled = False
        ddlUtente.Enabled = False
        'txtRicercaI.ReadOnly = False
        btnModifica.Visible = True

        btnSalva.Visible = False
    End Sub

    'Protected Sub btnCarica_Click(sender As Object, e As EventArgs) Handles btnCarica.Click
    '    Me.MySubAzienda = New SubAzienda
    '    Me.MySubAzienda.Load(lblIdAzienda.Text)
    '    If (FileUpload1.HasFile) Then
    '        Dim thefilename As String = ""
    '        Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
    '        If Directory.Exists(Server.MapPath("\logo\" & nome & "\")) = False Then
    '            '    ' ...Creo la cartella al percorso specificato
    '            Directory.CreateDirectory(Server.MapPath("\logo\" & nome & "\"))
    '        End If
    '        If Not File.Exists(Server.MapPath("\logo\" & nome & "\" & FileUpload1.FileName)) Then
    '            thefilename = FileUpload1.FileName
    '        Else
    '            thefilename = Now.Date.ToString("dd/MM/yyyy").Replace("/", "") & FileUpload1.FileName
    '        End If
    '        Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & nome & "\" & thefilename))
    '        'Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & FileUpload1.FileName))
    '        imgLogo.ImageUrl = "\logo\" & nome & "\" & thefilename
    '        'imgLogo.ImageUrl = "\logo\" & FileUpload1.FileName
    '        txtLogo.Text = thefilename
    '        Me.MySubAzienda.Logo = txtLogo.Text
    '        Me.MySubAzienda.SalvaData()
    '    End If
    '    FileUpload1 = New FileUpload
    'End Sub



#Region "Rubrica"



    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
        Me.MyCredenziali = New Credenziali
        Me.MyCredenziali.Inventario = New Inventari
        Try
            If txtDescrizioneCre.Text <> "" And txtUtenteCre.Text <> "" And txtPassCre.Text <> "" Then
                Me.MyCredenziali.ID = lblIdCredenziali.Text
                Me.MyCredenziali.Descrizione = txtDescrizioneCre.Text
                Me.MyCredenziali.Utente = txtUtenteCre.Text
                Me.MyCredenziali.Pass = txtPassCre.Text
                Me.MyCredenziali.Inventario.ID = lblIdInventario.Text
                Me.MyCredenziali.SalvaData()
                Dim message As String = "Credenziali create"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            Else
                Dim message As String = "Compilare i campi descrizione, utente e password"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

            End If

        Catch
        End Try
        Me.CaricaCredenziali()
        Me.SvuotaCampiRecapito()
    End Sub
    Private Sub SvuotaCampiRecapito()
        lblIdCredenziali.Text = "-1"
        txtDescrizioneCre.Text = ""
        txtUtenteCre.Text = ""
        txtPassCre.Text = ""
        btnElimina.Visible = False
        lblSicuro.Visible = False
    End Sub

    Private Sub CaricaCredenziali()
        Dim tab As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM Credenziali inner join Inventario on Inventario.id=Credenziali.idinventario " & _
                     "where Credenziali.idinventario = " & lblIdInventario.Text & " "

        tab = MyGest.GetTab(sqlstr)

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

        Dim btn2 As ImageButton = e.Item.FindControl("imgVedi")
        AddHandler btn2.Click, AddressOf VediPassword

    End Sub

    Private Sub VediPassword()

    End Sub

    Private Sub CancellaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyCredenziali = New Credenziali
        lblIdCredenziali.Text = bt.AlternateText
        Me.MyCredenziali.Load(lblIdCredenziali.Text)
        txtDescrizioneCre.Text = Me.MyCredenziali.Descrizione
        txtUtenteCre.Text = Me.MyCredenziali.Utente
        txtPassCre.Text = Me.MyCredenziali.Pass

        btnMemorizza.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True

    End Sub

    Private Sub ModificaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyCredenziali = New Credenziali
        lblIdCredenziali.Text = bt.AlternateText
        Me.MyCredenziali.Load(lblIdCredenziali.Text)
        txtDescrizioneCre.Text = Me.MyCredenziali.Descrizione
        txtUtenteCre.Text = Me.MyCredenziali.Utente
        txtPassCre.Text = Me.MyCredenziali.Pass

        btnMemorizza.Visible = True
        btnElimina.Visible = False
        lblSicuro.Visible = False
    End Sub

#End Region





    Protected Sub btnAnnulla1_Click(sender As Object, e As EventArgs) Handles btnAnnulla1.Click
        Me.SvuotaCampiRecapito()
        btnMemorizza.Visible = True
    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
        Me.MyCredenziali = New Credenziali
        Me.MyCredenziali.Delete(lblIdCredenziali.Text)
        Me.SvuotaCampiRecapito()
        Me.CaricaCredenziali()
        Dim message As String = "Contatto eliminato correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        btnElimina.Visible = False
        lblSicuro.Visible = False
        btnMemorizza.Visible = True
    End Sub



    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaInv.Visible = False
        PanelRicercaInventario.Visible = True
        lblIdInventario.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyInventario = New Inventari
        If Me.MyInventario.Delete(lblIdInventario.Text) Then
            Me.CaricaInventari()
            lblIdInventario.Text = "-1"
            PanelEliminaInv.Visible = False
            PanelRicercaInventario.Visible = True
            Dim message As String = "SubOrganizzazione eliminata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Private Sub CaricaFornitoreOrg(Optional ByVal idfo As String = "-1")
        Dim sql As String = "select *,Fornitore.ragsoc as ragione from Fornitore " & _
                            "inner join Fornitore_Esterno on Fornitore.id=Fornitore_Esterno.idFornitore "
        Select Session("tipoutente")
            Case "SuperAdmin"
            Case "Operatore"
                Dim azienda
                Me.MyUtente = New Utente
                Me.MyUtente.Load(Session("id"))
                If Me.MyUtente.SubAzienda.ID = "-1" Then
                    azienda = Me.MyUtente.Azienda.ID
                    sql = sql & "and Fornitore_Esterno.tipo='Organizzazioni' and Fornitore_Esterno.idesterno='" & azienda & "' "
                Else
                    azienda = Me.MyUtente.SubAzienda.ID
                    sql = sql & "and Fornitore_Esterno.tipo='SubOrganizzazioni' and Fornitore_Esterno.idesterno='" & azienda & "' "
                End If
        End Select
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row("ragione") = "..."
        row("id") = "-1"
        tab.Rows.Add(row)

        Me.ddlFornitoreOrg.DataSource = tab
        Me.ddlFornitoreOrg.DataTextField = "ragione"
        Me.ddlFornitoreOrg.DataValueField = "id"
        Me.ddlFornitoreOrg.DataBind()
        If idfo <> "-1" Then
            Me.ddlFornitoreOrg.SelectedValue = idfo
        Else
            Me.ddlFornitoreOrg.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaFornitoreCli(Optional ByVal idfc As String = "-1")
        Dim sql As String = "select *,Fornitore.ragsoc as ragione from Fornitore " & _
                            "inner join Fornitore_Esterno on Fornitore.id=Fornitore_Esterno.idFornitore "
        Select Session("tipoutente")
            Case "SuperAdmin"
            Case "Operatore"
                Dim azienda
                Me.MyUtente = New Utente
                Me.MyUtente.Load(Session("id"))
                If Me.MyUtente.SubAzienda.ID = "-1" Then
                    azienda = Me.MyUtente.Azienda.ID
                    sql = sql & "inner join Azienda on Azienda.id=Fornitore_Esterno.idesterno " & _
                                "where Fornitore_Esterno.tipo='Clienti' and Fornitore_Esterno.idesterno='" & azienda & "' "
                Else
                    azienda = Me.MyUtente.SubAzienda.ID
                    sql = sql & "inner join SubAzienda on SuAzienda.id=Fornitore_Esterno.idesterno " & _
                                "where Fornitore_Esterno.tipo='Clienti' and Fornitore_Esterno.idesterno='" & azienda & "' "
                End If
        End Select
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row("ragione") = "..."
        row("id") = "-1"
        tab.Rows.Add(row)

        Me.ddlFornitoreCli.DataSource = tab
        Me.ddlFornitoreCli.DataTextField = "ragione"
        Me.ddlFornitoreCli.DataValueField = "id"
        Me.ddlFornitoreCli.DataBind()
        If idfc <> "-1" Then
            Me.ddlFornitoreCli.SelectedValue = idfc
        Else
            Me.ddlFornitoreCli.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        PanelModificaInventario.Visible = True
        PanelCredenziali.Visible = False
        PanelBarra.Visible = False
        Panel1.Visible = False
        PanelRicercaInventario.Visible = False
        Me.AbilitaCampi()
        'Me.CaricaRegione()
        'Me.CaricaOrganizzazioni()
        Me.SvuotaCampi()
        'Me.CreaCodice()
        Me.CaricaFornitoreOrg()
        Me.CaricaFornitoreCli()
    End Sub


    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        Me.CaricaSubCliente(, ddlCliente.SelectedValue)
        Me.CaricaUtente(, , ddlCliente.SelectedValue)

    End Sub

    Private Sub CreaCodice()
        Me.MyUtente = New Utente
        Me.MyUtente.Load(Session("id"))

        If Me.MyUtente.Azienda.ID <> "-1" Then
            Me.MyCliente = New Cliente
            Me.MyCliente.Load(ddlCliente.SelectedValue)
            Dim idmax
            Dim sql As String = "select * from Inventario where Inventario.idCliente='" & ddlCliente.SelectedValue & "' "
            Dim tab As DataTable = Me.MyGest.GetTab(sql)
            If tab.Rows.Count > 0 Then
                idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Split(".")(1).Substring(0))
            Else
                idmax = 0

            End If
            idmax = Me.MyCliente.Codice & "." & idmax + 1



            txtCodice.Text = idmax
        Else
            txtCodice.Text = ""
        End If
    End Sub

    Private Sub CaricaSubCliente(Optional ByVal idsc As String = "-1", Optional ByVal idc As String = "-1")
        Dim str As String = "select SubCliente.ragsoc ,SubCliente.id from SubCliente"
        'If idc <> "-1" Then
        str = str & " inner join Cliente on SubCliente.idcliente=Cliente.id where Cliente.id=" & idc
        'End If
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & " and SubCliente.idazienda=" & MyUtente.Azienda.ID
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlSubCliente.DataSource = tab
        Me.ddlSubCliente.DataTextField = "ragsoc"
        Me.ddlSubCliente.DataValueField = "id"
        Me.ddlSubCliente.DataBind()
        If idsc <> "-1" Then
            Me.ddlSubCliente.SelectedValue = idsc
        Else
            Me.ddlSubCliente.SelectedValue = "-1"
        End If
    End Sub

  
    Protected Sub ddlSubCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubCliente.SelectedIndexChanged
        Me.CaricaUtente(, ddlSubCliente.SelectedValue, )
    End Sub

    Protected Sub lbCredenziali_Click(sender As Object, e As EventArgs) Handles lbCredenziali.Click
        If PanelCredenziali.Visible = True Then
            PanelCredenziali.Visible = False
        Else
            PanelCredenziali.Visible = True
            Me.CaricaCredenziali()
        End If
    End Sub

    Protected Sub cbxMobile_CheckedChanged(sender As Object, e As EventArgs) Handles cbxMobile.CheckedChanged
        Me.CaricaSubCliente(-1)
        If cbxMobile.Checked Then
            lblSubCliente.Visible = False
            ddlSubCliente.Visible = False
        Else
            lblSubCliente.Visible = True
            ddlSubCliente.Visible = True
        End If
    End Sub
End Class