Public Class contratto1
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyCliente As Cliente
    Private MySubCliente As SubCliente
    Private MyContratto As Contratto
    Private MySoglia As SogliaContratto
    Private MyRigheContratto As RigheContratto
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        
        Me.Form.Enctype = "multipart/form-data"
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else

                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaContratto.Visible = True
                    Me.CaricaContratto()
                    Dim app = Request.Url.ToString.Split("?")
                    Dim miaStringaCriptata As String
                    If app.Length > 1 Then

                        miaStringaCriptata = VSTripleDES.DecryptData(Request.Url.ToString.Split("?")(1))
                        ViewState("idcliente") = miaStringaCriptata.Split("&")(0).ToString.Split("=")(1)
                        ViewState("tipocliente") = miaStringaCriptata.Split("&")(1).ToString.Split("=")(1)
                        ViewState("operazione") = miaStringaCriptata.Split("&")(2).ToString.Split("=")(1)
                        If ViewState("operazione") = "modifica" Then
                            ViewState("idcontratto") = miaStringaCriptata.Split("&")(3).ToString.Split("=")(1)
                        End If
                    End If
                    If ViewState("idcliente") <> Nothing Then
                        PanelModificaContratto.Visible = True
                        PanelRicercaContratto.Visible = False
                        Panel1.Visible = False
                        Me.AbilitaCampi()
                        If ViewState("operazione") = "aggiungi" Then
                            Me.InserisciCodice()
                            Me.CaricaTipoContratto(, RestituisciOrganizzazione)
                            Me.CaricaTipoScadenza(, RestituisciOrganizzazione)
                            Me.CaricaTipoFatturazione(, RestituisciOrganizzazione)
                            Me.CaricaListBoxSubCliente(ViewState("idcliente"))
                            PanelSoglie.Visible = False
                            PanelRighe.Visible = False
                            lbSoglie.Visible = False
                            lbRighe.Visible = False

                        Else
                            Me.ModificaContratto(sender, e, ViewState("idcontratto"))
                        End If
                    Else
                        Me.CaricaListBoxSubCliente()
                    End If

                Else

                End If
                'Me.DisabilitaCampi()
                'Me.CaricaRegione()
                'Me.CaricaProvincia()
                ' Me.CaricaComune()
               

            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub



    Private Sub CaricaContratto(Optional ByVal descrizione As String = "")
        Dim tab As New DataTable
        
        Dim sqlStr = "SELECT distinct(Contratto.id),Contratto.codice,convert(VARCHAR(10),Contratto.dadata,103)as dadata ,convert(VARCHAR(10),Contratto.adata,103)as adata ,Contratto.importo,Cliente.ragsoc  " & _
                     "FROM Contratto inner join Azienda on Azienda.id=Contratto.idazienda " & _
                     "inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id " & _
                     "inner join Cliente on Cliente.id=Contratto_Cliente.idcliente " & _
                     "inner join TipoScadenza on TipoScadenza.id=Contratto.idtiposcadenza " & _
                     "inner join TipoContratto on TipoContratto.id=Contratto.idtipocontratto " & _
                     "where Contratto.id<>-1 "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sqlStr = sqlStr & "and Azienda.id=" & MyUtente.Azienda.ID
            If Session("isadmin") = 1 Then
                btnModifica.Visible = True
            Else
                btnModifica.Visible = False
            End If
            '    PanelRicercaContratto.Visible = False
            'Else
            '    PanelRicercaContratto.Visible = True
        ElseIf Session("tipoutente") = "Utente" Then
            btnModifica.Visible = False
        End If

        Select Case descrizione
            Case "validi"
                sqlStr = sqlStr & " and convert(date,'" & DateTime.Now & "',103) between dadata and adata"
            Case "scaduti"
                sqlStr = sqlStr & " and  convert(date,'" & DateTime.Now & "',103) > adata "
        End Select




        tab = MyGest.GetTab(sqlStr)

        ListView1.DataSource = tab
        ListView1.DataBind()

        If tab.Rows.Count > 100 Then
            DataPager1.Visible = True
        Else
            DataPager1.Visible = False
        End If
        
    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaContratto

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaContratto

        Dim btn2 As ImageButton = e.Item.FindControl("imgMostra")
        AddHandler btn2.Click, AddressOf ModificaContratto

        Dim btn3 As ImageButton = e.Item.FindControl("imgDuplica")
        AddHandler btn3.Click, AddressOf DuplicaContratto

    End Sub

    Private Function GeneraCodice(ByVal codice As String)
        Dim idmax
        Dim cliente As String = codice.Split(".")(0)
        idmax = codice.Split(".")(1)
        idmax = cliente & "." & idmax + 1
        Return idmax
       
    End Function


    Private Sub DuplicaContratto(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.MyContratto = New Contratto
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdContratto.Text = bt.AlternateText
        Me.MyContratto.Load(lblIdContratto.Text)
        Dim time As TimeSpan = Me.MyContratto.AData.Subtract(Me.MyContratto.DaData)
        Dim adata = Me.MyContratto.AData.AddDays(time.Days)
        Me.MyContratto.DaData = Me.MyContratto.AData.AddDays(1)
        Me.MyContratto.AData = adata.AddDays(1)
        Me.MyContratto.Codice = Me.GeneraCodice(Me.MyContratto.Codice)
        Me.MyContratto.ID = -1
        Me.MyContratto.SalvaData()
        Me.MySoglia = New SogliaContratto
        Me.MySoglia.Contratto = New Contratto
        Dim sql As String = "select id from SogliaContratto where idcontratto=" & lblIdContratto.Text
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            For i As Integer = 0 To tab.Rows.Count - 1
                Me.MySoglia.Load(tab.Rows(i)("id"))
                Me.MySoglia.Contratto.ID = Me.MyContratto.ID
                Me.MySoglia.ID = -1
                Me.MySoglia.SalvaData()
            Next
        End If

        
        Dim sq As String = "select * from Contratto_Cliente where idcontratto=" & lblIdContratto.Text
        Dim tb As DataTable = Me.MyGest.GetTab(sq)
        If tb.Rows.Count > 0 Then
            For i As Integer = 0 To tb.Rows.Count - 1
                Dim s As String = "Insert Into Contratto_Cliente values('" & Me.MyContratto.ID & "','" & tb.Rows(i)("idcliente") & "','" & tb.Rows(i)("tipocliente") & "')"
                MyGest.GetTab(s)
            Next
        End If
        Me.CaricaContratto()
    End Sub

    Private Sub CaricaListBoxSubCliente(Optional ByVal IdCliente As String = "-1")
        Dim sql As String = "Select ragsoc,id from SubCliente where id<>-1 "
        If ViewState("tipocliente") = "cliente" And IdCliente <> "-1" And Not IsDBNull(IdCliente) And IdCliente <> Nothing Then
            sql = sql & " and SubCliente.idcliente=" & IdCliente
        Else
            sql = sql & " and SubCliente.id=-1"
        End If
        'If IdCliente <> "-1" And Not IsDBNull(IdCliente) And IdCliente <> Nothing Then
        '    sql = sql & " and SubCliente.idcliente=" & IdCliente
        'End If

        Dim tab As DataTable = Me.MyGest.GetTab(sql)

        Me.cblSubCliente.DataSource = tab
        Me.cblSubCliente.DataTextField = "ragsoc"
        Me.cblSubCliente.DataValueField = "id"
        Me.cblSubCliente.DataBind()
    End Sub


    Private Sub ModificaContratto(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal idcontratto As String = "-1")
        Me.SvuotaCampiSoglia()
        PanelSoglie.Visible = True
        PanelRighe.Visible = True
        lbSoglie.Visible = True
        lbRighe.Visible = True
        Me.MyContratto = New Contratto
        If idcontratto = "-1" Then
            Dim bt As ImageButton = CType(sender, ImageButton)
            lblIdContratto.Text = bt.AlternateText
        Else
            lblIdContratto.Text = idcontratto
        End If
        Me.MyContratto.Load(lblIdContratto.Text)
        txtCodice.Text = Me.MyContratto.Codice
        txtCodEsterno.Text = Me.MyContratto.CodEsterno
        txtDaData.Text = Me.MyContratto.DaData
        txtAData.Text = Me.MyContratto.AData
        Me.CaricaTipoContratto(Me.MyContratto.IDTipoContratto, Me.RestituisciOrganizzazione)
        Me.CaricaTipoScadenza(Me.MyContratto.IDTipoScadenza, Me.RestituisciOrganizzazione)
        Me.CaricaTipoFatturazione(Me.MyContratto.IDTipoFatturazione, Me.RestituisciOrganizzazione)

        txtImporto.Text = Me.MyContratto.Importo

        Panel1.Visible = False
        PanelModificaContratto.Visible = True
        PanelRicercaContratto.Visible = False
        Me.DisabilitaCampi()
        'Me.CaricaTipologiaContatto()
        Me.CaricaSoglia()
        Me.CaricaTipoSoglia()
        Me.CaricaListino()
        Me.CaricaFuoriSoglia()
        Me.CaricaTipoTariffazione()

        Me.CaricaListBoxSubCliente()
        Me.CaricaSubClienti()
        Select Case Session("tipoutente")
            Case "Operatore"
                btnModifica.Visible = False
                If Session("isadmin") = 1 Then
                    btnModifica.Visible = True
                End If
            Case "Utente"
                If Session("isadmin") = 0 Then
                    txtImporto.Text = ""
                End If
            Case Else
                btnModifica.Visible = True
        End Select

    End Sub

    Private Function RestituisciOrganizzazione() As Integer
        Dim app = Request.Url.ToString.Split("?")
        Dim sql As String
        If app.Length > 1 Then
            sql = "select idazienda from cliente where id=" & ViewState("idcliente")
        Else
            sql = "select idazienda from utente where id=" & Session("id")
        End If
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim res As Integer = -1
        If tab.Rows.Count > 0 Then
            res = tab.Rows(0)("idazienda")
        End If
        Return res
    End Function

    Private Function RestituisciSubOrganizzazione() As Integer
        Dim app = Request.Url.ToString.Split("?")
        Dim sql As String
        If app.Length > 1 Then
            sql = "select idsubazienda from subcliente where id=" & ViewState("idcliente")
        Else
            sql = "select idsubazienda from utente where id=" & Session("id")
        End If
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim res As Integer = -1
        If tab.Rows.Count > 0 Then
            res = tab.Rows(0)("idsubazienda")
        End If
        Return res
    End Function

    Private Sub CancellaContratto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)

        lblIdContratto.Text = bt.AlternateText
        PanelEliminaContratto.Visible = True
        PanelRicercaContratto.Visible = False
        Me.MyContratto = New Contratto
        Me.MyContratto.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyContratto.Codice
        lblCodEsternoElimina.Text = Me.MyContratto.CodEsterno

    End Sub


    Private Sub CaricaTipoContratto(Optional ByVal idtc As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tipocontratto,id from TipoContratto"
        'If idr <> "-1" Then
        str = str & " where TipoContratto.idazienda=" & idorg
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoContratto.DataSource = tab
        Me.ddlTipoContratto.DataTextField = "tipocontratto"
        Me.ddlTipoContratto.DataValueField = "id"
        Me.ddlTipoContratto.DataBind()
        If idtc <> "-1" Then
            Me.ddlTipoContratto.SelectedValue = idtc
        Else
            Me.ddlTipoContratto.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaTipoScadenza(Optional ByVal idts As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tiposcadenza,id from TipoScadenza"
        'If idr <> "-1" Then
        str = str & " where TipoScadenza.idazienda=" & idorg
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoScadenza.DataSource = tab
        Me.ddlTipoScadenza.DataTextField = "tiposcadenza"
        Me.ddlTipoScadenza.DataValueField = "id"
        Me.ddlTipoScadenza.DataBind()
        If idts <> "-1" Then
            Me.ddlTipoScadenza.SelectedValue = idts
        Else
            Me.ddlTipoScadenza.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaTipoFatturazione(Optional ByVal idtf As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tipofatturazione,id from TipoFatturazione"
        'If idr <> "-1" Then
        str = str & " where TipoFatturazione.idazienda=" & idorg
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoFatturazione.DataSource = tab
        Me.ddlTipoFatturazione.DataTextField = "tipofatturazione"
        Me.ddlTipoFatturazione.DataValueField = "id"
        Me.ddlTipoFatturazione.DataBind()
        If idtf <> "-1" Then
            Me.ddlTipoFatturazione.SelectedValue = idtf
        Else
            Me.ddlTipoFatturazione.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub btnModifica_Click(sender As Object, e As EventArgs) Handles btnModifica.Click

        Me.AbilitaCampi()
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        Dim app = Request.Url.ToString.Split("?")
        Dim miaStringadeCriptata As String
        If app.Length > 1 Then

            miaStringadeCriptata = VSTripleDES.DecryptData(Request.Url.ToString.Split("?")(1))
            ViewState("idcliente") = miaStringadeCriptata.Split("&")(0).ToString.Split("=")(1)
            ViewState("tipocliente") = miaStringadeCriptata.Split("&")(1).ToString.Split("=")(1)
            If ViewState("tipocliente") = "cliente" Then
                Dim str = "idcliente=" & ViewState("idcliente")
                Dim miaStringaCriptata As String
                miaStringaCriptata = VSTripleDES.EncryptData(str)
                Response.Redirect("cliente.aspx?" & miaStringaCriptata)
            Else
                Dim str = "idcliente=" & ViewState("idcliente")
                Dim miaStringaCriptata As String
                miaStringaCriptata = VSTripleDES.EncryptData(str)
                Response.Redirect("subcliente.aspx?" & miaStringaCriptata)
            End If

        Else


            PanelModificaContratto.Visible = False
            Panel1.Visible = True
            PanelRicercaContratto.Visible = True
        End If
    End Sub

    'Protected Sub btnRicercaOrganizzazioni_Click(sender As Object, e As EventArgs) Handles btnRicercaOrganizzazioni.Click
    '    Me.CaricaContratto(txtRicercaOrganizzazioni.Text)
    '    Panel1.Visible = True
    '    PanelModificaOrganizzazione.Visible = False
    'End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        ' Me.SalvaSubCliente()
        Dim sql As String = "select * from TipoScadenza " & _
                            "where idazienda=" & RestituisciOrganizzazione() & " and id='" & ddlTipoScadenza.SelectedValue & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)

        Dim sql2 As String = "select * from TipoFatturazione " & _
                            "where idazienda=" & RestituisciOrganizzazione() & " and id='" & ddlTipoFatturazione.SelectedValue & "'"
        Dim tab2 As DataTable = Me.MyGest.GetTab(sql2)
        Dim differenza As TimeSpan = (CDate(txtAData.Text).Subtract(txtDaData.Text))
        Dim giorni As String = differenza.ToString.Split(".")(0)
        If (giorni < (tab.Rows(0)("giorni")) + 10) And (giorni > (tab.Rows(0)("giorni") - 10)) Then
            If tab.Rows(0)("giorni") >= tab2.Rows(0)("giorni") Then
                If IsValid Then
                    Me.MyContratto = New Contratto
                    Me.MyContratto.Azienda = New Azienda
                    Me.MyContratto.SubAzienda = New SubAzienda

                    Me.MyContratto.Load(lblIdContratto.Text)
                    Me.MyContratto.Azienda.ID = Me.RestituisciOrganizzazione
                    Me.MyContratto.SubAzienda.ID = Me.RestituisciSubOrganizzazione

                    Me.MyContratto.Codice = txtCodice.Text
                    Me.MyContratto.CodEsterno = txtCodEsterno.Text
                    Me.MyContratto.DaData = txtDaData.Text
                    Me.MyContratto.AData = txtAData.Text
                    Me.MyContratto.Importo = txtImporto.Text
                    Me.MyContratto.IDTipoFatturazione = ddlTipoFatturazione.SelectedValue

                    Me.MyContratto.IDTipoScadenza = ddlTipoScadenza.SelectedValue
                    Me.MyContratto.IDTipoContratto = ddlTipoContratto.SelectedValue
                    Me.MyContratto.SalvaData()

                    Me.SalvaRigheContratto(Me.MyContratto.ID)

                    Dim tipo As String = ""
                    Dim cliente As String = ""
                    Me.MyUtente = New Utente
                    Me.MyUtente.Load(Session("id"))
                    If IsDBNull(ViewState("idcliente")) Or ViewState("idcliente") = Nothing Then
                        Dim sqlcli As String = "select * from Contratto_Cliente where idcontratto=" & lblIdContratto.Text
                        Dim tabcli As DataTable = Me.MyGest.GetTab(sqlcli)
                        If tabcli.Rows.Count > 0 Then
                            cliente = tabcli.Rows(0)("idcliente")
                            tipo = tabcli.Rows(0)("tipocliente")
                        End If
                    ElseIf ViewState("tipocliente") = "cliente" Then
                        cliente = ViewState("idcliente")
                        tipo = "cliente"
                    Else
                        cliente = ViewState("idcliente")
                        tipo = "subcliente"
                    End If



                    lblIdContratto.Text = Me.MyContratto.ID
                    Me.SalvaContratto_Cliente(cliente, tipo)
                    Me.SalvaSubCliente()
                    lblConferma.Visible = True
                    lblConferma.Text = "Contratto Creato"
                    Me.CaricaContratto()

                    PanelModificaContratto.Visible = False
                    Panel1.Visible = True
                    PanelRicercaContratto.Visible = True
                    Dim message2 As String = "Contratto creato correttamente.Si prega di inserire le soglie"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
                Else

                    Dim message As String = "Inserire tutti i campi obbligatori"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                End If
            Else
                Dim message As String = "Attenzione! Il periodo di fatturazione è maggiore di quello di scadenza"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If
        Else
            Dim message As String = "Attenzione! Il periodo di scadenza è diverso di quello contrattuale"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Private Sub SalvaRigheContratto(ByVal id As Integer)
        Dim sql As String = "select * from TipoScadenza " & _
                            "where idazienda=" & RestituisciOrganizzazione() & " and id='" & ddlTipoScadenza.SelectedValue & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)

        Dim sql2 As String = "select * from TipoFatturazione " & _
                            "where idazienda=" & RestituisciOrganizzazione() & " and id='" & ddlTipoFatturazione.SelectedValue & "'"
        Dim tab2 As DataTable = Me.MyGest.GetTab(sql2)

        If tab.Rows.Count > 0 And tab2.Rows.Count > 0 Then
            Me.MyRigheContratto = New RigheContratto
            Me.MyRigheContratto.Contratto = New Contratto
            Me.MyRigheContratto.DeleteDaContratto(id)
            Dim data = Me.VerificaUltimaRiga(id)
            Dim div = tab.Rows(0)("giorni") / tab2.Rows(0)("giorni")
            Dim da As DateTime
            If data = "" Then
                da = txtDaData.Text
            Else
                da = CDate(data).AddDays(1)
                Dim differenza As TimeSpan = (CDate(txtAData.Text).Subtract(data))
                Dim giorni As String = differenza.ToString.Split(".")(0)
                div = Int(giorni / tab2.Rows(0)("giorni"))

            End If


            Dim a As DateTime = txtAData.Text
            For i As Integer = 0 To div
                Me.MyRigheContratto.ID = -1
                Me.MyRigheContratto.Contratto.ID = id
                Dim differenza As TimeSpan = (CDate(txtAData.Text).Subtract(da))
                If differenza.TotalDays >= tab2.Rows(0)("giorni") Then
                    'If da < a Then
                    Me.MyRigheContratto.Periodo = da & "  -  " & da.AddMonths(tab2.Rows(0)("mesi")).AddDays(-1)
                Else
                    Me.MyRigheContratto.Periodo = da & "  -  " & a
                End If
                da = da.AddMonths(tab2.Rows(0)("mesi"))
                Me.MyRigheContratto.Data = "01/01/9999"
                Me.MyRigheContratto.Fatturato = 0
                Me.MyRigheContratto.SalvaData()
            Next
        End If

    End Sub

    Private Function VerificaUltimaRiga(ByVal id As Integer) As String
        Dim sql As String = "select periodo from RigheContratto where idcontratto=" & id
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Return (tab.Rows(tab.Rows.Count - 1)("periodo").ToString.Split("-  ")(1))
        Else
            Return ""
        End If
    End Function

    Private Function ContaRiga(ByVal id As Integer) As Integer
        Dim sql As String = "select count(*) as num from RigheContratto where idcontratto=" & id
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Return tab.Rows(0)("num")
        Else
            Return 0
        End If
    End Function

    Private Sub SalvaContratto_Cliente(ByVal cliente As String, ByVal tipo As String)
        Dim sql2 As String = "select * from Contratto_Cliente where idcontratto=" & lblIdContratto.Text & " and idcliente=" & cliente & " and tipocliente='" & tipo & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql2)

        If tab.Rows.Count <= 0 Then
            Dim sql As String = "Insert Into Contratto_Cliente values('" & lblIdContratto.Text & "','" & cliente & "','" & tipo & "')"
            MyGest.GetTab(sql)
        End If
    End Sub

    Private Sub DeleteContratto_Cliente(ByVal cliente As String, ByVal tipo As String)
        Dim sql2 As String = "select * from Contratto_Cliente where idcontratto=" & lblIdContratto.Text & " and idcliente=" & cliente & " and tipocliente='" & tipo & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql2)

        If tab.Rows.Count > 0 Then
            Dim sql As String = "Delete From Contratto_Cliente where idcontratto='" & lblIdContratto.Text & "' and idcliente='" & cliente & "' and tipocliente='" & tipo & "'"
            MyGest.GetTab(sql)
        End If
    End Sub

    Private Sub SalvaSubCliente()
        'If cblSubCliente.SelectedValue <> Nothing Then
        'Dim Entry As Object
        For x = 0 To cblSubCliente.Items.Count - 1

            If cblSubCliente.Items.Item(x).Selected = True Then
                Me.SalvaContratto_Cliente(cblSubCliente.Items.Item(x).Value.ToString, "subcliente")
            Else
                Me.DeleteContratto_Cliente(cblSubCliente.Items.Item(x).Value.ToString, "subcliente")
            End If
        Next
        'For Each Entry In cblSubCliente.SelectedIndex(0)
        '    Me.SalvaContratto_Cliente(Entry.ToString, "subcliente")
        'Next
        'End If
    End Sub

    Private Sub CaricaSubClienti()
        Dim sql2 As String = "select * from Contratto_Cliente inner join SubCliente on SubCliente.id=Contratto_Cliente.idcliente where tipocliente='subcliente' and idcontratto=" & lblIdContratto.Text
        Dim tab As DataTable = Me.MyGest.GetTab(sql2)
        Dim Entry As Object
        If tab.Rows.Count > 0 Then
            For Each Entry In cblSubCliente.Items
                For j As Integer = 0 To tab.Rows.Count - 1
                    If Entry.ToString = tab.Rows(j)("ragsoc") Then
                        cblSubCliente.Items(j).Selected = True
                    End If
                Next
            Next

        End If



        'Dim sql As String = "Select ragsoc,id from SubCliente where id<>-1 "
        'If ViewState("tipocliente") = "subcliente" Then
        '    sql = sql & " and SubCliente.id<>" & IdCliente
        'End If
        'If IdCliente <> "-1" And Not IsDBNull(IdCliente) And IdCliente <> Nothing Then
        '    sql = sql & " and SubCliente.idcliente=" & IdCliente
        'End If

        'Dim tab As DataTable = Me.MyGest.GetTab(sql)

        'Me.cblSubCliente.DataSource = tab
        'Me.cblSubCliente.DataTextField = "ragsoc"
        'Me.cblSubCliente.DataValueField = "id"
        'Me.cblSubCliente.DataBind()
    End Sub

    Private Sub ListView3_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView3.ItemCreated
        Dim rdy As CheckBox = e.Item.FindControl("rd1")
        AddHandler rdy.CheckedChanged, AddressOf Controlla
    End Sub

    Private Sub Controlla(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rd As CheckBox = CType(sender, CheckBox)
        Dim sql As String
        Dim id_doc As Integer = rd.ToolTip
        If rd.Checked Then
            sql = "update RigheContratto set fatturato=1 where id=" & id_doc

            'Me.CaricaRigheContratto()
        Else
            sql = "update RigheContratto set fatturato=0 where id=" & id_doc
        End If
        MyGest.Execute(sql)
    End Sub

    Private Sub SvuotaCampi()
        lblIdContratto.Text = "-1"
        txtCodice.Text = ""
        txtCodEsterno.Text = ""
        txtDaData.Text = ""
        txtAData.Text = ""
        txtImporto.Text = ""
        Me.CaricaTipoContratto(, Me.RestituisciOrganizzazione)
        Me.CaricaTipoFatturazione(, Me.RestituisciOrganizzazione)
        Me.CaricaTipoScadenza(, Me.RestituisciOrganizzazione)
        lblConferma.Text = ""
    End Sub

    Private Sub InserisciCodice()
        Me.MyUtente = New Utente
        Me.MyUtente.Load(Session("id"))

        If Me.MyUtente.Azienda.ID <> "-1" Then
            Dim idmax
            Dim sql As String
            Dim tab As DataTable
            Dim cliente As String = ""
            If ViewState("tipocliente") = "cliente" Then
                Me.MyCliente = New Cliente
                Me.MyCliente.Load(ViewState("idcliente"))

                sql = "select * from Contratto inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id " & _
                                    " where Contratto_Cliente.idCliente='" & Me.MyCliente.ID & "' "
                tab = Me.MyGest.GetTab(sql)
                cliente = Me.MyCliente.Codice
            Else
                Me.MySubCliente = New SubCliente
                Me.MySubCliente.Load(ViewState("idcliente"))

                sql = "select * from Contratto inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id " & _
                                    " where Contratto_Cliente.idCliente='" & Me.MySubCliente.ID & "' "
                tab = Me.MyGest.GetTab(sql)
                cliente = Me.MySubCliente.Codice
            End If
            If tab.Rows.Count > 0 Then
                idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Split(".")(1).Substring(0))
            Else
                idmax = 0

            End If
            idmax = cliente & "." & idmax + 1



            txtCodice.Text = idmax
        Else
            txtCodice.Text = ""
        End If
    End Sub


    Private Sub AbilitaCampi()
        'txtCodice.ReadOnly = False
        txtCodEsterno.ReadOnly = False
        txtDaData.ReadOnly = False
        txtAData.ReadOnly = False
        txtImporto.ReadOnly = False

        cblSubCliente.Enabled = True

        CalendarExtender.Enabled = True
        CalendarExtender1.Enabled = True

        ddlTipoScadenza.Enabled = True
        ddlTipoContratto.Enabled = True
        ddlTipoFatturazione.Enabled = True

        btnModifica.Visible = False
        btnSalva.Visible = True

    End Sub

    Private Sub DisabilitaCampi()
        'txtCodice.ReadOnly = True
        txtCodEsterno.ReadOnly = True
        txtDaData.ReadOnly = True
        txtAData.ReadOnly = True
        txtImporto.ReadOnly = True

        cblSubCliente.Enabled = False

        CalendarExtender.Enabled = False
        CalendarExtender1.Enabled = False

        ddlTipoScadenza.Enabled = False
        ddlTipoContratto.Enabled = False
        ddlTipoFatturazione.Enabled = False

        btnModifica.Visible = True
        btnSalva.Visible = False
    End Sub



    '#Region "Soglia"
    Private Sub CaricaTipoSoglia(Optional ByVal idts As String = "-1")
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Load(Session("id"))

        Dim sql As String = "Select tiposoglia,id from TipoSoglia where TipoSoglia.idazienda=" & Me.MyUtente.Azienda.ID

        If idts <> "-1" Then
            sql = sql & " and TipoSoglia.id=" & idts
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "scegli il tipo soglia"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoSoglia.DataSource = tab
        Me.ddlTipoSoglia.DataTextField = "tiposoglia"
        Me.ddlTipoSoglia.DataValueField = "id"
        Me.ddlTipoSoglia.DataBind()
        If idts <> "-1" Then
            Me.ddlTipoSoglia.SelectedValue = idts
        Else
            Me.ddlTipoSoglia.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaListino(Optional ByVal idl As String = "-1")
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Load(Session("id"))

        Dim sql As String = "Select descrizione,id from Listino where Listino.idazienda=" & Me.MyUtente.Azienda.ID

        If idl <> "-1" Then
            sql = sql & " and Listino.id=" & idl
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "scegli il listino"
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

    Private Sub CaricaFuoriSoglia(Optional ByVal idfs As String = "-1")

        Dim sql As String = "Select fuorisoglia,id from TipoFuoriSoglia "

        If idfs <> "-1" Then
            sql = sql & " and TipoFuoriSoglia.id=" & idfs
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "scegli il fuori soglia"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlFuori.DataSource = tab
        Me.ddlFuori.DataTextField = "fuorisoglia"
        Me.ddlFuori.DataValueField = "id"
        Me.ddlFuori.DataBind()
        If idfs <> "-1" Then
            Me.ddlFuori.SelectedValue = idfs
        Else
            Me.ddlFuori.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaTipoTariffazione(Optional ByVal idtt As String = "-1")

        Dim sql As String = "Select tariffazione,id from Tariffazione where Tariffazione.idazienda=" & Me.RestituisciOrganizzazione

        If idtt <> "-1" Then
            sql = sql & " and Tariffazione.id=" & idtt
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "scegli la tariffazione"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoTariffazione.DataSource = tab
        Me.ddlTipoTariffazione.DataTextField = "tariffazione"
        Me.ddlTipoTariffazione.DataValueField = "id"
        Me.ddlTipoTariffazione.DataBind()
        If idtt <> "-1" Then
            Me.ddlTipoTariffazione.SelectedValue = idtt
        Else
            Me.ddlTipoTariffazione.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
        Dim sql As String = "select * from SogliaContratto where idcontratto=" & lblIdContratto.Text & " and idtipotariffazione=" & ddlTipoTariffazione.SelectedValue
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count <= 0 Or lblIdSoglia.Text <> "-1" Then
            If ddlFuori.SelectedValue <> "-1" And ddlTipoTariffazione.SelectedValue <> "-1" And ddlTipoSoglia.SelectedValue <> "-1" Then
                Me.MySoglia = New SogliaContratto
                Me.MySoglia.TipoSoglia = New TipoSoglia
                Me.MySoglia.Listino = New Listino
                Me.MySoglia.Contratto = New Contratto
                Me.MySoglia.TipoTariffazione = New Tariffa
                Try
                    Me.MySoglia.ID = lblIdSoglia.Text
                    Me.MySoglia.Contratto.ID = lblIdContratto.Text
                    Me.MySoglia.TipoSoglia.ID = ddlTipoSoglia.SelectedValue
                    Me.MySoglia.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
                    If txtSoglia.Text <> "" Then
                        Me.MySoglia.Soglia = txtSoglia.Text
                    Else
                        Me.MySoglia.Soglia = 0
                    End If
                    If txtAvviso.Text <> "" Then
                        Me.MySoglia.Avviso = txtAvviso.Text
                    Else
                        Me.MySoglia.Avviso = 0
                    End If
                    Me.MySoglia.IDFuori = ddlFuori.SelectedValue
                    If txtCostoFisso.Text <> "" Then
                        Me.MySoglia.CostoFisso = txtCostoFisso.Text
                    Else
                        Me.MySoglia.CostoFisso = 0
                    End If
                    If txtCostoVar.Text <> "" Then
                        Me.MySoglia.CostoVar = txtCostoVar.Text
                    Else
                        Me.MySoglia.CostoVar = 0
                    End If
                    Me.MySoglia.Listino.ID = ddlListino.SelectedValue
                    If txtSLA.Text <> "" Then
                        Me.MySoglia.SLA = txtSLA.Text
                    Else
                        Me.MySoglia.SLA = 0
                    End If
                    Me.MySoglia.SalvaData()

                Catch
                End Try
                Me.CaricaSoglia()
                Me.SvuotaCampiSoglia()
                Dim message2 As String = "Soglia Contratto creata correttamente"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            Else
                If ddlTipoSoglia.SelectedValue <> "-1" Then
                    Label1.Visible = False
                Else
                    Label1.Visible = True
                End If
                If ddlTipoTariffazione.SelectedValue <> "-1" Then
                    Label2.Visible = False
                Else
                    Label2.Visible = True
                End If
                If ddlFuori.SelectedValue <> "-1" Then
                    Label3.Visible = False
                Else
                    Label3.Visible = True
                End If
                Dim message As String = "Inserire tutti i campi obbligatori"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If
        Else
            Dim message As String = "Tipo tariffazione già presente per questo contratto"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub
    Private Sub SvuotaCampiSoglia()
        lblIdSoglia.Text = "-1"

        Me.CaricaTipoSoglia()
        Me.CaricaFuoriSoglia()
        Me.CaricaListino()
        Me.CaricaTipoTariffazione()

        txtSoglia.Text = ""
        txtAvviso.Text = ""
        txtCostoFisso.Text = ""
        txtCostoVar.Text = ""
        txtSLA.Text = ""

    End Sub

    Private Function ValutaMinutaggio(ByVal ora, ByVal minuto) As String
        Dim frat
        If minuto > 59 Then
            frat = minuto \ 60
            ora = ora + frat
            minuto = minuto - (frat * 60)
        End If
        If minuto.ToString.Length = 1 Then
            minuto = "0" & minuto
        End If
        If ora.ToString.Length = 1 Then
            ora = "0" & ora
        End If
        Return ora & ":" & minuto
    End Function


    Private Sub CaricaSoglia()
        Dim str As String = "select sum(cast(substring(tempotot,1,2)as integer))as tempotot,sum(cast(substring(tempotot,4,2)as integer))as mintot,TempoTicket .idtipotariffazione   from TempoTicket " & _
                            "inner join Tickets on Tickets.id=TempoTicket.idTicket " & _
                            "inner join Contratto on Contratto.id=Tickets.idcontratto " & _
                            "where Contratto.id = " & lblIdContratto.Text & _
                            "group by TempoTicket .idtipotariffazione "


        Dim tb As DataTable = Me.MyGest.GetTab(str)





        Dim tab As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM SogliaContratto inner join TipoFuoriSoglia on TipoFuoriSoglia.id=SogliaContratto.idfuori " & _
                     "inner join TipoSoglia on TipoSoglia.id=SogliaContratto.idtiposoglia " & _
                     "inner join Tariffazione on Tariffazione.id=SogliaContratto.idtipotariffazione " & _
                     "inner join Contratto on Contratto.id=SogliaContratto.idcontratto " & _
                     "left outer join Listino on Listino.id=SogliaContratto.idlistino " & _
                     "where Contratto.id = " & lblIdContratto.Text & " "

        tab = MyGest.GetTab(sqlStr)




        Dim tstruct As New DataTable
        Dim c1 As New DataColumn("id", GetType(String), "")
        tstruct.Columns.Add(c1)
        Dim c2 As New DataColumn("tiposoglia", GetType(String), "")
        tstruct.Columns.Add(c2)
        Dim c3 As New DataColumn("tariffazione", GetType(String), "")
        tstruct.Columns.Add(c3)
        Dim c4 As New DataColumn("soglia", GetType(String), "")
        tstruct.Columns.Add(c4)
        Dim c5 As New DataColumn("tempores", GetType(String), "")
        tstruct.Columns.Add(c5)
        Dim c6 As New DataColumn("avviso", GetType(String), "")
        tstruct.Columns.Add(c6)
        Dim c7 As New DataColumn("fuorisoglia", GetType(String), "")
        tstruct.Columns.Add(c7)
        Dim c8 As New DataColumn("costofisso", GetType(String), "")
        tstruct.Columns.Add(c8)
        Dim c9 As New DataColumn("costovar", GetType(String), "")
        tstruct.Columns.Add(c9)
        Dim c10 As New DataColumn("sla", GetType(String), "")
        tstruct.Columns.Add(c10)
        Dim sogliaora
        Dim sogliamin
        For i As Integer = 0 To tab.Rows.Count - 1
            tstruct.Rows.Add(i)
            tstruct.Rows(i)("tempores") = tab.Rows(i)("soglia")
            For j As Integer = 0 To tb.Rows.Count - 1
                If tab.Rows(i)("idtipotariffazione") = tb.Rows(j)("idtipotariffazione") Then
                    Try
                        sogliaora = tab.Rows(i)("soglia").ToString.Split(":")(0)
                    Catch
                        sogliaora = tab.Rows(i)("soglia")
                    End Try
                    Try
                        sogliamin = tb.Rows(i)("soglia").ToString.Split(":")(1)
                    Catch
                        sogliamin = "00"
                    End Try
                    Dim differenza = sogliaora - Me.ValutaMinutaggio(tb.Rows(i)("tempotot"), tb.Rows(i)("mintot")).ToString.Split(":")(0) & ":" & sogliamin - Me.ValutaMinutaggio(tb.Rows(i)("tempotot"), tb.Rows(i)("mintot")).ToString.Split(":")(1) 'tab.Rows(i)("soglia") - tb.Rows(i)("tempotot")
                    If CInt(differenza.ToString.Split(":")(1)) < 0 Then
                        tstruct.Rows(i)("tempores") = CInt(differenza.ToString.Split(":")(0)) - 1 & ":" & 60 + CInt(differenza.ToString.Split(":")(1))
                    Else
                        tstruct.Rows(i)("tempores") = differenza
                    End If

                    'Dim differenza As TimeSpan = tab.Rows(i)("soglia").Subtract(CDate(tb.Rows(i)("tempotot")))

                End If
            Next
            Try
                If tstruct.Rows(i)("tempores").ToString.Split(":")(1).Length = 1 Then
                    tstruct.Rows(i)("tempores") = tstruct.Rows(i)("tempores").ToString.Split(":")(0) & ":0" & tstruct.Rows(i)("tempores").ToString.Split(":")(1)
                End If
            Catch
            End Try


            'tstruct.Rows(i)("tempores") = tstruct.Rows(i)("tempores") '& ":00"
            tstruct.Rows(i)("id") = tab.Rows(i)("id")
            tstruct.Rows(i)("tiposoglia") = tab.Rows(i)("tiposoglia")
            tstruct.Rows(i)("tariffazione") = tab.Rows(i)("tariffazione")
            Dim soglia = tab.Rows(i)("soglia")
            Try
                Dim a = tab.Rows(i)("soglia").ToString.Split(":")(1)
            Catch
                soglia = tab.Rows(i)("soglia") & ":00"
            End Try
            tstruct.Rows(i)("soglia") = soglia

            Dim avviso = tab.Rows(i)("avviso")
            Try
                Dim b = tab.Rows(i)("avviso").ToString.Split(":")(1)
            Catch
                avviso = tab.Rows(i)("avviso") & ":00"
            End Try
            tstruct.Rows(i)("avviso") = avviso
            tstruct.Rows(i)("fuorisoglia") = tab.Rows(i)("fuorisoglia")
            tstruct.Rows(i)("costofisso") = tab.Rows(i)("costofisso")
            tstruct.Rows(i)("costovar") = tab.Rows(i)("costovar")
            tstruct.Rows(i)("sla") = tab.Rows(i)("sla")

        Next


        ListView2.DataSource = tstruct
        ListView2.DataBind()

        If tstruct.Rows.Count > 100 Then
            DataPager2.Visible = True
        Else
            DataPager2.Visible = False
        End If
    End Sub

    Private Sub ListView2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView2.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaSoglia

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaSoglia

    End Sub

    Private Sub CancellaSoglia(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MySoglia = New SogliaContratto
        Me.MySoglia.TipoSoglia = New TipoSoglia
        lblIdSoglia.Text = bt.AlternateText
        Me.MySoglia.Load(lblIdSoglia.Text)
        ddlTipoSoglia.SelectedValue = Me.MySoglia.TipoSoglia.ID
        btnMemorizza.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True
        btnAnnulla1.Visible = False
        btnAnnullaSoglia.Visible = True
    End Sub

    Private Sub ModificaSoglia(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MySoglia = New SogliaContratto
        Me.MySoglia.TipoSoglia = New TipoSoglia
        Me.MySoglia.Listino = New Listino
        Me.MySoglia.TipoTariffazione = New Tariffa
        lblIdSoglia.Text = bt.AlternateText
        Me.MySoglia.Load(lblIdSoglia.Text)
        ddlTipoSoglia.SelectedValue = Me.MySoglia.TipoSoglia.ID
        txtSoglia.Text = Me.MySoglia.Soglia
        txtAvviso.Text = Me.MySoglia.Avviso
        ddlFuori.SelectedValue = Me.MySoglia.IDFuori
        txtCostoFisso.Text = Me.MySoglia.CostoFisso
        txtCostoVar.Text = Me.MySoglia.CostoVar
        ddlListino.SelectedValue = Me.MySoglia.Listino.ID
        ddlTipoTariffazione.SelectedValue = Me.MySoglia.TipoTariffazione.ID
        If ddlListino.SelectedValue <> "-1" Then
            ddlListino.Visible = True
        Else
            ddlListino.Visible = False
        End If
        txtSLA.Text = Me.MySoglia.SLA
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaSoglia.Visible = False
        btnAnnulla.Visible = True
        lblSicuro.Visible = False
    End Sub

    '#End Region

    Protected Sub btnAnnulla1_Click(sender As Object, e As EventArgs) Handles btnAnnulla1.Click
        Me.SvuotaCampiSoglia()
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaSoglia.Visible = False
        lblSicuro.Visible = False

    End Sub

    'Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
    '    Me.MyRecapito = New Recapito
    '    Me.MyRecapito.Delete(lblIdRecapito.Text)
    '    Me.SvuotaCampiRecapito()
    '    Me.CaricaContatti()
    '    Dim message As String = "Contatto eliminato correttamente"
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

    '    btnAnnulla1.Visible = True
    '    btnAnnullaContatto.Visible = False
    '    btnElimina.Visible = False
    '    lblSicuro.Visible = False
    '    btnMemorizza.Visible = True
    'End Sub

    Private Sub CaricaRigheContratto()
        Dim sql As String = "select * from RigheContratto where idcontratto=" & lblIdContratto.Text
        Dim tab As DataTable = Me.MyGest.GetTab(sql)

        For Each RW As DataRow In tab.Rows

        Next
        If tab.Rows.Count > 0 Then

            lbRighe.Visible = True
            ListView3.DataSource = tab
            ListView3.DataBind()

            ViewState("tab") = tab

            Dim i As Integer = 0
            For Each lvi As ListViewItem In ListView3.Items
                Dim checksi As CheckBox = lvi.FindControl("rd1")
                If tab.Rows(i)("fatturato") = 1 Then
                    checksi.Checked = True
                End If
                i = i + 1
            Next




        Else
            lbRighe.Visible = False
        End If
    End Sub

    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaContratto.Visible = False
        PanelRicercaContratto.Visible = True
        lblIdContratto.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyContratto = New Contratto
        If Me.MyContratto.Delete(lblIdContratto.Text) Then
            Me.CaricaContratto()
            lblIdContratto.Text = "-1"
            PanelEliminaContratto.Visible = False
            PanelRicercaContratto.Visible = True
            Dim message As String = "Contratto eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    'Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
    '    Me.SvuotaCampi()
    '    PanelModificaOrganizzazione.Visible = True
    '    Panel1.Visible = False

    '    Me.CaricaRegione()
    '    Me.AbilitaCampi()
    '    Me.InserisciCodice()
    'End Sub

    Protected Sub btnAnnullaSoglia_Click(sender As Object, e As EventArgs) Handles btnAnnullaSoglia.Click
        'Me.SvuotaCampiRecapito()
        lblSicuro.Visible = False
        btnElimina.Visible = False
        btnAnnullaSoglia.Visible = False
        btnMemorizza.Visible = True
        btnAnnulla1.Visible = True
    End Sub

    Protected Sub rbTutti_CheckedChanged(sender As Object, e As EventArgs) Handles rbTutti.CheckedChanged
        Me.CaricaContratto("tutti")
        rbScaduti.Checked = False
        rbValidi.Checked = False
    End Sub

    Protected Sub rbValidi_CheckedChanged(sender As Object, e As EventArgs) Handles rbValidi.CheckedChanged
        Me.CaricaContratto("validi")
        rbScaduti.Checked = False
        rbTutti.Checked = False
    End Sub

    Protected Sub rbScaduti_CheckedChanged(sender As Object, e As EventArgs) Handles rbScaduti.CheckedChanged
        Me.CaricaContratto("scaduti")
        rbTutti.Checked = False
        rbValidi.Checked = False
    End Sub

    Protected Sub ddlFuori_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFuori.SelectedIndexChanged
        If ddlFuori.SelectedItem.Text = "Listino" Then
            ddlListino.Visible = True
        Else
            ddlListino.Visible = False
        End If
        Me.CaricaListino()
    End Sub

    Protected Sub lbRighe_Click(sender As Object, e As EventArgs) Handles lbRighe.Click
        If PanelRighe.Visible = True Then
            PanelRighe.Visible = False
        Else
            PanelRighe.Visible = True
            Me.CaricaRigheContratto()
        End If

    End Sub

    Protected Sub lbSoglie_Click(sender As Object, e As EventArgs) Handles lbSoglie.Click
        If PanelSoglie.Visible = True Then
            PanelSoglie.Visible = False
        Else
            PanelSoglie.Visible = True
            Me.CaricaSoglia()
        End If
    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click

    End Sub
End Class