Imports System.Net.Mail

Public Class utente1
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyCGlobal As CGlobal
    Private MyUtente As Utente
    Private MyOrganizzazione As Azienda
    Private MySubOrganizzazione As SubAzienda
    Private MyCliente As Cliente
    Private MySubCliente As SubCliente
    Private MyRecapito As Recapito
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Utente" Or Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not String.IsNullOrEmpty(txtPassword.Text.Trim()) Then
                    txtPassword.Attributes.Add("value", txtPassword.Text)
                End If
                If Not String.IsNullOrEmpty(txtConfermaPassword.Text.Trim()) Then
                    txtConfermaPassword.Attributes.Add("value", txtConfermaPassword.Text)
                End If
                If Not IsPostBack Then
                    txtPassword.Text = ""
                    txtUserid.Text = ""
                    Panel1.Visible = True
                    PanelRicercaUtente.Visible = True
                    PanelRicercaUtente.Visible = True

                    Select Case Session("tipoutente")
                        Case "SuperAdmin"
                            btnAnnulla.Visible = True
                            btNuovo.Visible = True
                            PanelRicercaUtente.Visible = True
                            ddlTipo.Enabled = True
                            Me.CaricaUtente()
                            Me.CaricaCliente()
                            Me.CaricaSubCliente()
                            Me.CaricaAbilitazione()
                            Me.CaricaTipoUtente()
                            'Me.CaricaTipo()
                        Case "Operatore"
                            Me.CaricaUtente()
                            Me.CaricaCliente()
                            Me.CaricaSubCliente()
                            Me.CaricaAbilitazione()
                            Me.CaricaTipoUtente()
                            'Me.CaricaTipo()
                        Case "Utente"
                            If Session("isadmin") = 1 Then
                                Me.CaricaUtente()
                                Me.CaricaCliente()
                                'Me.CaricaSubCliente()
                                Me.CaricaAbilitazione()
                                Me.CaricaTipoUtente()
                                'Me.CaricaTipo()
                            Else
                                btnAnnulla.Visible = False
                                btNuovo.Visible = False
                                PanelRicercaUtente.Visible = False
                                lblIdUtente.Text = Session("id")
                                Me.CaricaTipoUtente(Me.CaricaTipoDaTipoUtente)
                                ddlTipo.Enabled = False
                                ModificaUtente(sender, e)
                            End If
                    End Select


                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Function CaricaTipoDaTipoUtente()
        Dim sql As String = "Select * from TipoUtente where tipoutente like'%" & Session("tipoutente") & "%'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Return tab.Rows(0)("id")
    End Function

    Private Sub CaricaTipoUtente(Optional idtu As String = "-1")
        Dim str As String = "select tipoutente ,id from TipoUtente "
        Select Case Session("tipoutente")
            Case "Utente"
                str = str & "where tipoUtente='Utente'"
            Case "SuperAdmin"
                str = str & "where tipoUtente<>'Operatore'"
            Case "Operatore"
                str = str & "where tipoUtente<>'Operatore' and tipoutente<>'SuperAdmin'"
        End Select
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipo.DataSource = tab
        Me.ddlTipo.DataTextField = "tipoutente"
        Me.ddlTipo.DataValueField = "id"
        Me.ddlTipo.DataBind()
        If idtu <> "-1" Then
            Me.ddlTipo.SelectedValue = idtu
        Else
            Me.ddlTipo.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaCliente(Optional ByVal idc As String = "-1")
        Dim str As String = "select ragsoc ,id from Cliente"
        If Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            str = str & " where Cliente.id=" & Me.MyUtente.Cliente.ID
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

    Private Sub CaricaSubCliente(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1")
        Dim str As String = "select SubCliente.ragsoc as nomeSubCliente ,SubCliente.id as idSubCliente from SubCliente"
        If idc <> "-1" Then
            str = str & " inner join Cliente on SubCliente.idCliente=Cliente.id where Cliente.id=" & idc
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlSubCliente.DataSource = tab
        Me.ddlSubCliente.DataTextField = "nomeSubCliente"
        Me.ddlSubCliente.DataValueField = "idSubCliente"
        Me.ddlSubCliente.DataBind()
        If idsc <> "-1" Then
            Me.ddlSubCliente.SelectedValue = idsc
        Else
            Me.ddlSubCliente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        Me.CaricaUtente(txtRicercaUtente.Text)
    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaUtente

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaUtente

        Dim btn2 As ImageButton = e.Item.FindControl("imgRinvia")
        AddHandler btn2.Click, AddressOf RinviaCredenziali
    End Sub

    Private Sub RinviaCredenziali(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyCGlobal = New CGlobal
        If Me.MyCGlobal.InviaMailAggiornamento(bt.AlternateText, 5) Then
            Dim message2 As String = "Credenziali inviate correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        Else
            Dim message2 As String = "Errore nell'invio delle credenziali"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        End If
    End Sub

    Private Sub ModificaUtente(ByVal sender As Object, ByVal e As System.EventArgs)
        lbModifica.Visible = True
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.SubAzienda = New SubAzienda
        Me.MyUtente.Cliente = New Cliente
        Me.MyUtente.SubCliente = New SubCliente
        If Session("tipoutente") <> "Utente" Or Session("isadmin") = 1 Then
            Me.SvuotaCampi()
            Dim bt As ImageButton = CType(sender, ImageButton)
            lblIdUtente.Text = bt.AlternateText
        End If
        Me.MyUtente.Load(lblIdUtente.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo
        CaricaOrganizzazione(Me.MyUtente.Azienda.ID)
        CaricaSubOrganizzazione(Me.MyUtente.SubAzienda.ID)
        Me.CaricaCliente(Me.MyUtente.Cliente.ID)
        Me.CaricaSubCliente(Me.MyUtente.SubCliente.ID)

        Me.CaricaRegione(Me.MyUtente.IDRegione)
        Me.CaricaProvincia(Me.MyUtente.IDProvincia)
        'Me.CaricaComune(Me.MyUtente.IDComune)
        Me.CaricaTipoUtente(Me.MyUtente.IDTipo)
        Me.CaricaReparto(Me.MyUtente.IDReparto)
        ddlAbilitato.SelectedIndex = Me.MyUtente.Abilitato

        txtUserid.Text = Me.MyUtente.Userid
        txtCap.Text = Me.MyUtente.Cap
        txtIndirizzo.Text = Me.MyUtente.Indirizzo

        'Me.CaricaTipo(Me.MyUtente.Tipo)

        txtNome.Text = Me.MyUtente.Nome
        txtCognome.Text = Me.MyUtente.Cognome
        txtCap.Text = Me.MyUtente.Cap
        txtIndirizzo.Text = Me.MyUtente.Indirizzo

        txtPassword.Visible = False
        txtConfermaPassword.Visible = False
        lblPassword.Visible = False
        lblConfermaPassword.Visible = False
        RequiredFieldValidator2.Visible = False
        RequiredFieldValidator1.Visible = False
        If Me.MyUtente.IsAdmin = 1 Then
            cbxAmministratore.Checked = True
        Else
            cbxAmministratore.Checked = False
        End If


        Panel1.Visible = False
        PanelModificaUtente.Visible = True
        Me.DisabilitaCampi()
        Me.CaricaTipologiaUtente()
        Me.CaricaTipologiaContatto()
        Me.CaricaContatti()
    End Sub

    Private Sub CaricaAbilitazione()
        ddlAbilitato.Items.Add("no")
        ddlAbilitato.Items.Add("si")
    End Sub



    Private Sub CancellaUtente(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdUtente.Text = bt.AlternateText
        PanelEliminaUtente.Visible = True
        PanelRicercaUtente.Visible = False
        Me.MyUtente = New Utente
        Me.MyUtente.Load(bt.AlternateText)
        lblUseridElimina.Text = Me.MyUtente.Userid
        lblNomeElimina.Text = Me.MyUtente.Nome
        lblCognomeElimina.Text = Me.MyUtente.Cognome
    End Sub

    Private Sub CaricaReparto(Optional ByVal idr As String = "-1")
        Dim str As String = "select reparto ,id from Reparto"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlReparto.DataSource = tab
        Me.ddlReparto.DataTextField = "reparto"
        Me.ddlReparto.DataValueField = "id"
        Me.ddlReparto.DataBind()
        If idr <> "-1" Then
            Me.ddlReparto.SelectedValue = idr
        Else
            Me.ddlReparto.SelectedValue = "-1"
        End If
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

    'Private Sub CaricaTipo(Optional ByVal idt As String = "-1")
    '    Dim str As String = "select tipoutente ,id from TipoUtente"

    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlTipo.DataSource = tab
    '    Me.ddlTipo.DataTextField = "tipoutente"
    '    Me.ddlTipo.DataValueField = "id"
    '    Me.ddlTipo.DataBind()
    '    If idt <> "-1" Then
    '        Me.ddlTipo.SelectedValue = idt
    '    Else
    '        Me.ddlTipo.SelectedValue = "-1"
    '    End If
    'End Sub

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
        Me.AbilitaCampi()

        If Session("tipoutente") = "Utente" Then
            If Session("isadmin") = 1 Then
                ddlCliente.Enabled = False
                ddlSubCliente.Enabled = False
                ddlTipo.Enabled = False
                ddlAbilitato.Enabled = True
                cbxAmministratore.Enabled = True

                ddlOrganizzazione.Visible = False
                ddlSubOrganizzazione.Visible = False
                ddlCliente.Visible = True
                ddlSubCliente.Visible = True
                cbxAmministratore.Visible = True

                lblOrganizzazione.Visible = False
                lblSubOrganizzazione.Visible = False
                lblCliente.Visible = True
                lblSubCliente.Visible = True
            Else
                ddlTipo.Enabled = False
                ddlCliente.Enabled = False
                ddlSubCliente.Enabled = False
                ddlAbilitato.Enabled = False
                cbxAmministratore.Enabled = False
            End If
        ElseIf Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin" Then
            ddlCliente.Enabled = True
            ddlSubCliente.Enabled = True
            ddlTipo.Enabled = True
            ddlAbilitato.Enabled = True
            cbxAmministratore.Enabled = True

            ddlOrganizzazione.Visible = False
            ddlSubOrganizzazione.Visible = False
            ddlCliente.Visible = True
            ddlSubCliente.Visible = True
            cbxAmministratore.Visible = True

            lblOrganizzazione.Visible = False
            lblSubOrganizzazione.Visible = False
            lblCliente.Visible = True
            lblSubCliente.Visible = True

        End If
        If Session("tipoutente") <> "Utente" Then
            Me.CaricaTipologiaUtente()
        End If
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        PanelModificaUtente.Visible = False
        Panel1.Visible = True
    End Sub

    Protected Sub btnRicercaCliente_Click(sender As Object, e As EventArgs) Handles btnRicercaUtente.Click
        Me.CaricaUtente(txtRicercaUtente.Text)
        Panel1.Visible = True
        PanelModificaUtente.Visible = False
    End Sub

    Private Sub CaricaUtente(Optional ByVal cognome As String = "")
        Me.MyUtente = New Utente
        Me.MyCliente = New Cliente
        Me.MyCliente.Azienda = New Azienda
        Me.MyCliente.SubAzienda = New SubAzienda
        Dim tab As New DataTable
        Dim sql As String
        Dim sqlStr = "SELECT * " & _
                     "FROM Utente " & _
                     "left outer join Reparto on Reparto.id=Utente.idreparto  " & _
                     "inner join TipoUtente on TipoUtente.id=Utente.idtipo "
        If Session("tipoutente") = "Utente" And Session("isadmin") = 1 Then
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.Cliente.ID <> "-1" Then
                sql = "inner join Cliente on Cliente.id=Utente.idCliente where idcliente=" & Me.MyUtente.Cliente.ID
            End If
            If Me.MyUtente.SubCliente.ID <> "-1" Then
                sql = "inner join SubCliente on SubCliente.id=Utente.idSubCliente where idsubcliente=" & Me.MyUtente.SubCliente.ID
            End If
            sqlStr = sqlStr & sql
            If Session("tipoutente") <> "SuperAdmin" Then
                sqlStr = sqlStr & " and issuperadmin<>1 "
            End If

        ElseIf Session("tipoutente") = "Operatore" Then
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.Azienda.ID <> "-1" Then
                Me.MyCliente.Azienda.Load(Me.MyUtente.Azienda.ID)
                sql = "inner join Cliente on Cliente.id=Utente.idCliente inner join Azienda on Azienda.id=Cliente.idAzienda where Cliente.idazienda=" & Me.MyCliente.Azienda.ID
            End If
            If Me.MyUtente.SubAzienda.ID <> "-1" Then
                Me.MyCliente.SubAzienda.Load(Me.MyUtente.SubAzienda.ID)
                sql = "inner join Cliente on Cliente.id=Utente.idCliente inner join subAzienda on subAzienda.id=Cliente.idsubAzienda where idsubcliente=" & Me.MyUtente.SubAzienda.ID
            End If
            sqlStr = sqlStr & sql
            If Session("tipoutente") <> "SuperAdmin" Then
                sqlStr = sqlStr & " and issuperadmin<>1 "
            End If
        ElseIf Session("tipoutente") = "SuperAdmin" Then



            sql = "left outer join Cliente on Cliente.id=Utente.idCliente where TipoUtente.tipoutente<>'Operatore'"
            sqlStr = sqlStr & sql

        End If
        If cognome <> "" Then
            sqlStr = sqlStr & "and nome like '%" & cognome & "%' or cognome like '%" & cognome & "%'"
        End If
        sqlStr = sqlStr & " and TipoUtente.tipoutente='Utente'"
        tab = MyGest.GetTab(sqlStr)
        ListView1.DataSource = tab
        ListView1.DataBind()


        'Dim tstruct As New DataTable
        'Dim c1 As New DataColumn("userd", GetType(String), "")
        'tstruct.Columns.Add(c1)
        'Dim c2 As New DataColumn("nome", GetType(String), "")
        'tstruct.Columns.Add(c2)
        'Dim c3 As New DataColumn("cognome", GetType(String), "")
        'tstruct.Columns.Add(c3)
        'Dim c4 As New DataColumn("reparto", GetType(String), "")
        'tstruct.Columns.Add(c4)
        'Dim c5 As New DataColumn("cliente", GetType(String), "")
        'tstruct.Columns.Add(c5)
        'Dim c6 As New DataColumn("id", GetType(String), "")
        'tstruct.Columns.Add(c6)
        'For i As Integer = 0 To tab.Rows.Count - 1
        '    tstruct.Rows.Add(i)

        '    tstruct.Rows(i)("userid") = tab.Rows(i)("userid")
        '    tstruct.Rows(i)("nome") = tab.Rows(i)("nome")
        '    tstruct.Rows(i)("cognome") = tab.Rows(i)("cognome")
        '    tstruct.Rows(i)("reparto") = tab.Rows(i)("reparto")
        '    If tab.Rows(i)("idsubcliente") <> -1 Then
        '        tstruct.Rows(i)("cliente") = tab.Rows(i)("ragsoc")
        '    Else
        '        tstruct.Rows(i)("cliente") = tab.Rows(i)("ragsoc")
        '    End If
        'Next


        If tab.Rows.Count > 100 Then
            DataPager1.Visible = True
        Else
            DataPager1.Visible = False
        End If

    End Sub

    Private Function Controlla()
        Dim result As Boolean = False
        Dim emailregexp As Regex = New Regex("(?<user>[^@]+)@(?<host>.+)")
        Dim mail As String = txtUserid.Text
        Dim controllo As Match = emailregexp.Match(mail)
        If (controllo.Success) Then
            result = True
        Else
        End If
        Return result
    End Function

    Private Function ControllaPresMail()
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Load(Session("id"))
        Dim ret As Boolean = False
        Dim sql As String = "select * from Utente " & _
                            "inner join TipoUtente on TipoUtente.id=Utente.idtipo " & _
                            "inner join Cliente on Cliente.id=Utente.idcliente " & _
                            "inner join Azienda on Azienda.id=Cliente.idazienda " & _
                            "where tipoutente='Utente' and userid='" & txtUserid.Text & "' and Azienda.id=" & MyUtente.Azienda.ID
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            ret = True
        End If

        Return ret
    End Function

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click

        If Controlla() Then
            lblErrore.Text = ""
            If Me.IsValid Then
                If lblIdUtente.Text = "-1" Then
                    If ControllaPresMail() Then
                        Dim message2 As String = "Mail già presente"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
                        Exit Sub
                    End If
                End If


                Me.MyUtente = New Utente
                Me.MyUtente.Cliente = New Cliente
                Me.MyUtente.SubCliente = New SubCliente
                Me.MyUtente.Azienda = New Azienda
                Me.MyUtente.SubAzienda = New SubAzienda

                Me.MyUtente.Load(lblIdUtente.Text)
                Me.MyUtente.Userid = txtUserid.Text

                If txtPassword.Text <> "" And txtConfermaPassword.Text <> "" Then
                    Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtPassword.Text)
                    Me.MyUtente.Psw = miaPasswordCriptata
                End If

                Me.MyUtente.Abilitato = ddlAbilitato.SelectedIndex
                Me.MyUtente.IDRegione = ddlRegione.SelectedValue
                Me.MyUtente.IDProvincia = ddlProvincia.SelectedValue
                Me.MyUtente.IDComune = ddlComune.Text
                Me.MyUtente.Cliente.ID = ddlCliente.SelectedValue
                If ddlSubCliente.SelectedValue <> "-1" Or ddlSubCliente.SelectedValue <> "" Then
                    Me.MyUtente.SubCliente.ID = ddlSubCliente.SelectedValue
                Else
                    Me.MyUtente.SubCliente.ID = -1
                End If
                Me.MyUtente.Azienda.ID = ddlOrganizzazione.SelectedValue
                If ddlSubOrganizzazione.SelectedValue <> "" Then
                    Me.MyUtente.SubAzienda.ID = ddlSubOrganizzazione.SelectedValue
                Else
                    Me.MyUtente.SubAzienda.ID = -1
                End If
                Me.MyUtente.Nome = txtNome.Text
                Me.MyUtente.Cognome = txtCognome.Text
                Me.MyUtente.Cap = txtCap.Text
                Me.MyUtente.Indirizzo = txtIndirizzo.Text
                If ddlReparto.SelectedValue <> "" Then
                    Me.MyUtente.IDReparto = ddlReparto.SelectedValue
                Else
                    Me.MyUtente.IDReparto = "-1"
                End If
                Me.MyUtente.IDTipo = ddlTipo.SelectedValue

                If cbxAmministratore.Checked Then
                    Me.MyUtente.IsAdmin = 1
                Else
                    Me.MyUtente.IsAdmin = 0
                End If


                Me.MyUtente.SalvaData()
                
                'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "showLoading();", True)
                Me.MyCGlobal = New CGlobal
                If lblIdUtente.Text = "-1" Then
                    MyCGlobal.InviaMailAttivazione(Me.MyUtente.ID, 2)
                Else
                    MyCGlobal.InviaMailAggiornamento(Me.MyUtente.ID, 5)
                End If
                Dim message As String = "Utente creato correttamente"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

                lblIdUtente.Text = Me.MyUtente.ID
                lblConferma.Visible = True
                lblConferma.ForeColor = Drawing.Color.Green
                lblConferma.Text = "Utente Creato"
                Me.CaricaUtente()
                Panel1.Visible = True
                PanelRicercaUtente.Visible = True
                PanelModificaUtente.Visible = False


            Else
                Dim message As String = "Inserire tutti i campi obbligatori"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If
        Else
            Dim message As String = "Formato mail non corretto"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        End If

    End Sub

    'Private Sub SendAttivazione(ByVal EMAIL As String, ByVal NOMINATIVO As String, ByVal USERNAME As String, ByVal PASSWORD As String)
    '    Dim SmtpClient As New Net.Mail.SmtpClient
    '    Dim message As New System.Net.Mail.MailMessage


    '    Dim from As String = "carlo_mail2002@yahoo.it" '"software@advise.it"
    '    Dim user As String = "carlo_mail2002@yahoo.it" '"software@advise.it"
    '    Dim passw As String = "LelloTata2006" '"advise07"
    '    Dim host As String = "smtp.mail.yahoo.it" '"authsmtp.advise.it"

    '    Dim dominio As String = ""
    '    Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


    '    SmtpClient.Host = host
    '    SmtpClient.Port = 25
    '    message.From = fromAddress
    '    If EMAIL.IndexOf("@") <> -1 Then
    '        message.To.Add(EMAIL)
    '    Else
    '        Exit Sub
    '    End If
    '    Dim testo As String

    '    Dim intestazione As String = "Ti confermiamo l'avvenuta registrazione sul portale ApriUnTicket.it"
    '    Dim saluti As String = "DISTINTI SALUTI"
    '    message.Subject = "Attivazione Utente su ApriUnTicket.it Advise srl"


    '    'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
    '    'Dim tabs As DataTable = Me.MyGest.GetTab(s)
    '    'If tabs.Rows.Count > 0 Then
    '    '    If tabs.Rows(0)("oggetto") <> "" Then
    '    '        message.Subject = tabs.Rows(0)("oggetto")
    '    '    End If
    '    '    If tabs.Rows(0)("intestazione") <> "" Then
    '    '        intestazione = tabs.Rows(0)("intestazione")
    '    '    End If
    '    '    If tabs.Rows(0)("saluti") <> "" Then
    '    '        saluti = tabs.Rows(0)("saluti")
    '    '    End If
    '    'End If
    '    Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")

    '    testo = "<p><h2>Ciao <b>" & NOMINATIVO & "</b>:</h2></p>" & vbCrLf & _
    '   intestazione & vbCrLf & _
    '   "<br/><br/>" & vbCrLf & _
    '   "Queste sono le tue credenziali di accesso:" & vbCrLf & _
    '   "<br/><br/>" & vbCrLf & _
    '   "Email:" & USERNAME & vbCrLf & _
    '   "<br/>" & vbCrLf & _
    '   "Password:" & PASSWORD & vbCrLf & _
    '   "<br/><br/>" & vbCrLf & _
    '   "Per attivare il tuo account clicca o copia il link di seguito:" & vbCrLf & _
    '   "<a href=""http://www.advise.it/crm_advise/attivazione.aspx?utente=" & tempo & "\" & lblIdUtente.Text & """>http://www.advise.it/crm_advise/attivazione.aspx</a>" & vbCrLf & _
    '   "<b><br/><br/></b>" & vbCrLf & _
    '   "<b>" & saluti & "</b>" & vbCrLf


    '    message.Body = testo
    '    message.IsBodyHtml = True
    '    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

    '    Try
    '        SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
    '        ' SmtpClient.UseDefaultCredentials = True
    '        ' SmtpClient.EnableSsl = True

    '        SmtpClient.Send(message)

    '    Catch ex As Exception
    '        ' MsgBox(ex.ToString)
    '        Response.Write(ex.ToString)
    '    End Try

    'End Sub


    Protected Sub lbModifica_Click(sender As Object, e As EventArgs) Handles lbModifica.Click
        PanelModPass.Visible = True
        btnConfermo.Visible = False
        'Label14.Visible = True
        txtVecchiaPsw.Visible = True
        btnVerifica.Visible = True
        lblError.Visible = False
    End Sub

    Protected Sub btnVerifica_Click(sender As Object, e As EventArgs) Handles btnVerifica.Click
        Me.MyUtente = New Utente
        Me.MyUtente.Load(Session("id"))
        Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtVecchiaPsw.Text)

        If MyUtente.Psw.Equals(miaPasswordCriptata) Then
            'Label14.Visible = False
            txtVecchiaPsw.Visible = False
            'lblPassword.Visible = True
            txtNuovaPassword.Visible = True
            'lblConferma.Visible = True
            txtConfirmPassword.Visible = True
            btnConfermo.Visible = True
            btnVerifica.Visible = False
            lblError.Visible = False
        Else
            'Label14.Visible = False
            'txtVecchiaPsw.Visible = False
            'btnConfermo.Visible = True
            lblError.Visible = True
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
        txtVecchiaPsw.Visible = False
        btnVerifica.Visible = False
        lblError.Visible = False
        txtNuovaPassword.Visible = False
        txtConfirmPassword.Visible = False
        btnConfermo.Visible = False
        lblerror2.Visible = False
        Panel3.Visible = False
        lblIdUtente.Text = "-1"
        txtUserid.Text = ""
        txtPassword.Text = ""
        txtCap.Text = ""
        txtIndirizzo.Text = ""
        txtNome.Text = ""
        txtCognome.Text = ""
        lblConferma.Text = ""
        lblErrore.Visible = False
        ddlCliente.SelectedValue = "-1"
        ddlOrganizzazione.SelectedValue = "-1"
        'ddlComune.SelectedValue = "-1"
        ddlComune.Text = ""
        ddlProvincia.SelectedValue = "-1"
        ddlRegione.SelectedValue = "-1"
        ddlReparto.SelectedValue = "-1"
        ddlTipo.SelectedValue = "-1"
        cbxAmministratore.Checked = False
        lblIdRecapito.Text = "-1"
    End Sub

    

    'Private Sub InserisciCodice()
    '    Dim idmax
    '    Dim sql As String = "select Max(id),codice from SubCliente group by id,codice"
    '    Dim tab As DataTable = Me.MyGest.GetTab(sql)
    '    If tab.Rows.Count > 0 Then
    '        idmax = CInt(tab.Rows(0).Item("codice").ToString.Substring(2))
    '    Else
    '        idmax = -1
    '    End If
    '    idmax = "SC" & idmax + 1
    '    txtCodice.Text = idmax
    'End Sub

    Private Sub AbilitaCampi()
        txtUserid.ReadOnly = False
        txtCap.ReadOnly = False
        txtIndirizzo.ReadOnly = False
        txtNome.ReadOnly = False
        txtCognome.ReadOnly = False


        btnModifica.Visible = False
        btnSalva.Visible = True
        ddlAbilitato.Enabled = True
        ddlCliente.Enabled = True
        ddlComune.Enabled = True
        ddlOrganizzazione.Enabled = True
        ddlProvincia.Enabled = True
        ddlRegione.Enabled = True
        ddlReparto.Enabled = True
        ddlSubCliente.Enabled = True
        ddlSubOrganizzazione.Enabled = True
        ddlTipo.Enabled = True
        cbxAmministratore.Enabled = True
    End Sub

    Private Sub DisabilitaCampi()
        txtUserid.ReadOnly = True
        txtCap.ReadOnly = True
        txtIndirizzo.ReadOnly = True
        txtNome.ReadOnly = True
        txtCognome.ReadOnly = True

        btnModifica.Visible = True
        btnSalva.Visible = False

        ddlAbilitato.Enabled = False
        ddlCliente.Enabled = False
        ddlComune.Enabled = False
        ddlOrganizzazione.Enabled = False
        ddlProvincia.Enabled = False
        ddlRegione.Enabled = False
        ddlReparto.Enabled = False
        ddlSubCliente.Enabled = False
        ddlSubOrganizzazione.Enabled = False
        ddlTipo.Enabled = False
        cbxAmministratore.Enabled = False
    End Sub

    Private Sub CaricaOrganizzazione(Optional ByVal ido As String = "-1")
        Dim str As String = "select descrizione ,id from Azienda"

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
        Else
            Me.ddlOrganizzazione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaSubOrganizzazione(Optional ByVal ido As String = "-1", Optional ByVal idso As String = "-1")
        Dim str As String = "select SubAzienda.descrizione as nomeSubAzienda ,SubAzienda.id as idSubAzienda from SubAzienda"
        If ido <> "-1" Then
            str = str & " inner join Azienda on SubAzienda.idAzienda=Azienda.id where Azienda.id=" & ido
        End If
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
        Else
            Me.ddlSubOrganizzazione.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub ddlOrganizzazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOrganizzazione.SelectedIndexChanged
        Me.CaricaSubOrganizzazione(ddlOrganizzazione.SelectedValue)
    End Sub

    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        Me.CaricaSubCliente(ddlCliente.SelectedValue)
        Me.CaricaLocalita()
    End Sub

    Private Sub CaricaLocalita()
        If ddlSubCliente.SelectedValue <> "-1" Then
            Me.MySubCliente = New SubCliente
            Me.MySubCliente.Load(ddlSubCliente.SelectedValue)
            Me.CaricaRegione(Me.MySubCliente.IDRegione)
            Me.CaricaProvincia(Me.MySubCliente.IDProvincia, Me.MySubCliente.IDRegione)
            'Me.CaricaComune(Me.MySubCliente.IDComune, Me.MySubCliente.IDProvincia)
            ddlComune.Text = Me.MySubCliente.IDComune
        Else
            Me.MyCliente = New Cliente
            Me.MyCliente.Load(ddlCliente.SelectedValue)
            Me.CaricaRegione(Me.MyCliente.IDRegione)
            Me.CaricaProvincia(Me.MyCliente.IDProvincia, Me.MyCliente.IDRegione)
            'Me.CaricaComune(Me.MyCliente.IDComune, Me.MyCliente.IDProvincia)
            ddlComune.Text = Me.MyCliente.IDComune
        End If

    End Sub

    Protected Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        Me.CaricaTipologiaUtente()
    End Sub

    Private Sub CaricaTipologiaUtente()
        Me.MyUtente = New Utente
        Dim sql As String = "select * from TipoUtente where id=" & ddlTipo.SelectedValue
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Select Case tab.Rows(0).Item("tipoutente")
                Case "Utente"

                    ddlOrganizzazione.Visible = False
                    ddlSubOrganizzazione.Visible = False
                    ddlCliente.Visible = True
                    ddlSubCliente.Visible = True
                    cbxAmministratore.Visible = True

                    lblOrganizzazione.Visible = False
                    lblSubOrganizzazione.Visible = False
                    lblCliente.Visible = True
                    lblSubCliente.Visible = True

                Case "SuperAdmin"

                    ddlOrganizzazione.Visible = False
                    ddlSubOrganizzazione.Visible = False
                    ddlCliente.Visible = False
                    ddlSubCliente.Visible = False
                    cbxAmministratore.Visible = False

                    lblOrganizzazione.Visible = False
                    lblSubOrganizzazione.Visible = False
                    lblCliente.Visible = False
                    lblSubCliente.Visible = False

            End Select
        End If
    End Sub

    Protected Sub btnConfermo_Click(sender As Object, e As EventArgs) Handles btnConfermo.Click
        If txtNuovaPassword.Text.Equals(txtConfirmPassword.Text) Then
            Me.MyUtente = New Utente
            MyUtente.Load(lblIdUtente.Text)
            If txtNuovaPassword.Visible And txtConfirmPassword.Visible And txtNuovaPassword.Text <> "" And txtConfirmPassword.Text <> "" Then
                Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtNuovaPassword.Text)
                Me.MyUtente.Psw = miaPasswordCriptata
                Me.MyUtente.SalvaData()
                Me.MyCGlobal = New CGlobal
                Me.MyCGlobal.InviaMailAggiornamento(Me.MyUtente.ID, 4)
                Panel3.Visible = True
            End If
        End If
        btnConfermo.Visible = False
        txtVecchiaPsw.Visible = False
        btnVerifica.Visible = False
        lblError.Visible = False
        txtNuovaPassword.Visible = False
        txtConfirmPassword.Visible = False
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
                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdUtente.Text & "'," & Me.MyRecapito.ID & ",'Utente')"
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
                     "inner join Utente on Utente.id=Recapito_Utente.idUtente " & _
                     "inner join TipoContatto on TipoContatto.id=Recapito.idtipo " & _
                     "where Recapito_Utente.idUtente = " & lblIdUtente.Text & " and Recapito_Utente.tipoAssociazione like 'Utente' "

        tab = MyGest.GetTab(sqlStr)

        ListView2.DataSource = tab
        ListView2.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager2.Visible = True
        Else
            DataPager2.Visible = False
        End If
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaContatti()
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
        lblIdRecapito.Text = "-1"
    End Sub



    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaUtente.Visible = False
        PanelRicercaUtente.Visible = True
        lblIdUtente.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyUtente = New Utente
        If Me.MyUtente.Delete(lblIdUtente.Text) Then
            Me.CaricaUtente()
            lblIdUtente.Text = "-1"
            PanelEliminaUtente.Visible = False
            PanelRicercaUtente.Visible = True
            Dim message As String = "Utente eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        Me.SvuotaCampi()
        txtPassword.Visible = True
        txtConfermaPassword.Visible = True
        lblPassword.Visible = True
        lblConfermaPassword.Visible = True
        RequiredFieldValidator2.Visible = True
        RequiredFieldValidator1.Visible = True

        PanelModificaUtente.Visible = True
        Panel1.Visible = False
        Me.AbilitaCampi()
        Me.CaricaRegione()
        Me.CaricaOrganizzazione()
        Me.CaricaCliente()
        Me.CaricaContatti()
        lbModifica.Visible = False
    End Sub

    Protected Sub ddlSubCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubCliente.SelectedIndexChanged
        Me.CaricaLocalita()
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
        If Panel2.Visible = True Then
            Panel2.Visible = False
        Else
            Panel2.Visible = True
            Me.CaricaContatti()
        End If
    End Sub
End Class