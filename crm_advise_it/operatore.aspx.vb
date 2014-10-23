Imports System.Net.Mail

Public Class operatore
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyCGlobal As CGlobal
    Private MyOperatore As Utente
    Private MyOrganizzazione As Azienda
    Private MySubOrganizzazione As SubAzienda
    Private MyCliente As Cliente
    Private MySubCliente As SubCliente
    Private MyRecapito As Recapito
    Private MyUtente As Utente
    
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin") Then
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
                    Panel1.Visible = True
                    PanelRicercaOperatore.Visible = True
                    PanelRicercaOperatore.Visible = True


                    Select Case Session("tipoutente")
                        Case "SuperAdmin"
                            btnAnnulla.Visible = True
                            btNuovo.Visible = True
                            PanelRicercaOperatore.Visible = True
                            ddlTipo.Enabled = True
                            Me.CaricaOperatore()
                            Me.CaricaOrganizzazione()
                            'Me.CaricaSubOrganizzazione()
                            Me.CaricaAbilitazione()
                            Me.CaricaTipoUtente()
                            'Me.CaricaTipo()
                        Case "Operatore"
                            If Session("isadmin") = 1 Then
                                Me.CaricaOperatore()
                                Me.CaricaOrganizzazione()
                                'Me.CaricaSubOrganizzazione()
                                Me.CaricaAbilitazione()
                                Me.CaricaTipoUtente()
                                'Me.CaricaTipo()
                            Else
                                btnAnnulla.Visible = False
                                btNuovo.Visible = False
                                PanelRicercaOperatore.Visible = False
                                lblIdOperatore.Text = Session("id")
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

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaOperatore(txtRicercaOperatore.Text)
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaContatti()
    End Sub

    Private Function CaricaTipoDaTipoUtente()
        Dim sql As String = "Select * from TipoUtente where tipoutente like'%" & Session("tipoutente") & "%'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Return tab.Rows(0)("id")
    End Function

    Private Sub CaricaTipoUtente(Optional idtu As String = "-1")
        Dim str As String = "select tipoutente ,id from TipoUtente where tipoUtente<>'Utente' "
        Select Case Session("tipoutente")
            Case "Utente"
                str = str & "and tipoUtente='-1'"
            Case "SupeAdmin"
                str = str & "and tipoUtente<>'Utente'"
            Case "Operatore"
                str = str & "and tipoUtente='Operatore' "
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
        If Me.MyCGlobal.InviaMailAggiornamento(bt.AlternateText, 4) Then
            Dim message2 As String = "Credenziali inviate correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        Else
            Dim message2 As String = "Errore nell'invio delle credenziali"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        End If
    End Sub

    Private Sub ModificaUtente(ByVal sender As Object, ByVal e As System.EventArgs)
        'ViewState("modifica") = True
        PanelRecapiti.Visible = True
        lbModifica.Visible = True
        Me.MyOperatore = New Utente
        Me.MyOperatore.Azienda = New Azienda
        Me.MyOperatore.SubAzienda = New SubAzienda
        Me.MyOperatore.Cliente = New Cliente
        Me.MyOperatore.SubCliente = New SubCliente
        If Session("tipoutente") <> "Operatore" Or Session("isadmin") = 1 Then
            Me.SvuotaCampi()
            Dim bt As ImageButton = CType(sender, ImageButton)
            lblIdOperatore.Text = bt.AlternateText
        End If
        Me.MyOperatore.Load(lblIdOperatore.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo
        CaricaOrganizzazione(Me.MyOperatore.Azienda.ID)
        CaricaSubOrganizzazione(Me.MyOperatore.SubAzienda.ID)
        Me.CaricaCliente(Me.MyOperatore.Cliente.ID)
        Me.CaricaSubCliente(Me.MyOperatore.SubCliente.ID)

        Me.CaricaRegione(Me.MyOperatore.IDRegione)
        Me.CaricaProvincia(Me.MyOperatore.IDProvincia)
        'Me.CaricaComune(Me.MyOperatore.IDComune)
        ddlComune.Text = Me.MyOperatore.IDComune
        Me.CaricaTipoUtente(Me.MyOperatore.IDTipo)
        Me.CaricaReparto(Me.MyOperatore.IDReparto)
        ddlAbilitato.SelectedIndex = Me.MyOperatore.Abilitato

        txtUserid.Text = Me.MyOperatore.Userid
        txtCap.Text = Me.MyOperatore.Cap
        txtIndirizzo.Text = Me.MyOperatore.Indirizzo
        txtPassword.Text = Me.MyOperatore.Psw
        'Me.CaricaTipo(Me.MyUtente.Tipo)

        txtNome.Text = Me.MyOperatore.Nome
        txtCognome.Text = Me.MyOperatore.Cognome
        txtCap.Text = Me.MyOperatore.Cap
        txtIndirizzo.Text = Me.MyOperatore.Indirizzo

        txtPassword.Visible = False
        txtConfermaPassword.Visible = False
        lblPassword.Visible = False
        lblConfermaPassword.Visible = False
        RequiredFieldValidator2.Visible = False
        RequiredFieldValidator1.Visible = False
        If Me.MyOperatore.IsAdmin = 1 Then
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
        lblIdOperatore.Text = bt.AlternateText
        PanelEliminaOper.Visible = True
        PanelRicercaOperatore.Visible = False
        Me.MyOperatore = New Utente
        Me.MyOperatore.Load(bt.AlternateText)
        lblUseridElimina.Text = Me.MyOperatore.Userid
        lblNomeElimina.Text = Me.MyOperatore.Nome
        lblCognomeElimina.Text = Me.MyOperatore.Cognome
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

        If Session("tipoutente") = "Operatore" Then
            If Session("isadmin") = 1 Then
                ddlCliente.Enabled = False
                ddlSubCliente.Enabled = False
                ddlTipo.Enabled = False
                ddlAbilitato.Enabled = False
                cbxAmministratore.Enabled = False

                ddlOrganizzazione.Visible = True
                ddlSubOrganizzazione.Visible = True
                ddlCliente.Visible = False
                ddlSubCliente.Visible = False
                cbxAmministratore.Visible = False

                lblOrganizzazione.Visible = True
                lblSubOrganizzazione.Visible = True
                lblCliente.Visible = False
                lblSubCliente.Visible = False
            Else
                ddlTipo.Enabled = False
                ddlOrganizzazione.Enabled = False
                ddlSubOrganizzazione.Enabled = False
                ddlAbilitato.Enabled = False
                cbxAmministratore.Enabled = False
            End If
        ElseIf Session("tipoutente") = "SuperAdmin" Then
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
        If Session("tipoutente") <> "Operatore" Then
            Me.CaricaTipologiaUtente()
        End If
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        PanelModificaUtente.Visible = False
        Panel1.Visible = True
    End Sub

    Protected Sub btnRicercaCliente_Click(sender As Object, e As EventArgs) Handles btnRicercaOperatore.Click
        Me.CaricaOperatore(txtRicercaOperatore.Text)
        Panel1.Visible = True
        PanelModificaUtente.Visible = False
    End Sub

    Private Sub CaricaOperatore(Optional ByVal cognome As String = "")
        Me.MyOperatore = New Utente
        Me.MyCliente = New Cliente
        Me.MyCliente.Azienda = New Azienda
        Me.MyCliente.SubAzienda = New SubAzienda
        Dim tab As New DataTable
        Dim sql As String
        Dim sqlStr = "SELECT * " & _
                     "FROM Utente " & _
                     "left outer join Reparto on Reparto.id=Utente.idreparto  "
        'If Session("tipoutente") = "Utente" And Session("isadmin") = 1 Then
        '    Me.MyOperatore.Load(Session("id"))
        '    If Me.MyOperatore.Cliente.ID <> "-1" Then
        '        sql = "inner join Cliente on Cliente.id=Utente.idCliente where idcliente=" & Me.MyOperatore.Cliente.ID
        '    End If
        '    If Me.MyOperatore.SubCliente.ID <> "-1" Then
        '        sql = "inner join SubCliente on SubCliente.id=Utente.idSubCliente where idsubcliente=" & Me.MyOperatore.SubCliente.ID
        '    End If
        '    sqlStr = sqlStr & sql
        '    If Session("tipoutente") <> "SuperAdmin" Then
        '        sqlStr = sqlStr & " and issuperadmin<>1 "
        '    End If

        If Session("tipoutente") = "Operatore" Then
            Me.MyOperatore.Load(Session("id"))
            If Me.MyOperatore.Azienda.ID <> "-1" Then
                Me.MyCliente.Azienda.Load(Me.MyOperatore.Azienda.ID)
                sql = "inner join Azienda on Azienda.id=Utente.idAzienda where Utente.idazienda=" & Me.MyCliente.Azienda.ID
            End If
            If Me.MyOperatore.SubAzienda.ID <> "-1" Then
                Me.MyCliente.SubAzienda.Load(Me.MyOperatore.SubAzienda.ID)
                sql = "inner join subAzienda on subAzienda.id=Utente.idsubAzienda where idsubcliente=" & Me.MyOperatore.SubAzienda.ID
            End If
            sqlStr = sqlStr & sql
            If Session("tipoutente") <> "SuperAdmin" Then
                sqlStr = sqlStr & " and issuperadmin<>1 "
            End If
        ElseIf Session("tipoutente") = "SuperAdmin" Then
            sqlStr = sqlStr & " inner join TipoUtente on TipoUtente.id=Utente.idtipo left outer join Azienda on Azienda.id=Utente.idAzienda where TipoUtente.tipoutente='Operatore' "
        End If
        If cognome <> "" Then
            sqlStr = sqlStr & "and nome like '%" & cognome & "%' or cognome like '%" & cognome & "%'"
        End If
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


        If tab.Rows.Count > 10 Then
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

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If Controlla() Then
            lblErrore.Text = ""
            If Me.IsValid Then
                Me.MyOperatore = New Utente
                Me.MyOperatore.Cliente = New Cliente
                Me.MyOperatore.SubCliente = New SubCliente
                Me.MyOperatore.Azienda = New Azienda
                Me.MyOperatore.SubAzienda = New SubAzienda

                Me.MyOperatore.Load(lblIdOperatore.Text)
                Me.MyOperatore.Userid = txtUserid.Text
                If txtPassword.Text <> "" And txtConfermaPassword.Text <> "" Then
                    Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtPassword.Text)
                    Me.MyOperatore.Psw = miaPasswordCriptata
                End If
                Me.MyOperatore.Abilitato = ddlAbilitato.SelectedIndex
                Me.MyOperatore.IDRegione = ddlRegione.SelectedValue
                Me.MyOperatore.IDProvincia = ddlProvincia.SelectedValue
                Me.MyOperatore.IDComune = ddlComune.Text
                Me.MyOperatore.Cliente.ID = ddlCliente.SelectedValue
                If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
                    Me.MyOperatore.SubCliente.ID = ddlSubCliente.SelectedValue
                Else
                    Me.MyOperatore.SubCliente.ID = -1
                End If
                Me.MyOperatore.Azienda.ID = ddlOrganizzazione.SelectedValue
                If ddlSubOrganizzazione.SelectedValue <> "" Then
                    Me.MyOperatore.SubAzienda.ID = ddlSubOrganizzazione.SelectedValue
                Else
                    Me.MyOperatore.SubAzienda.ID = -1
                End If
                Me.MyOperatore.Nome = txtNome.Text
                Me.MyOperatore.Cognome = txtCognome.Text
                Me.MyOperatore.Cap = txtCap.Text
                Me.MyOperatore.Indirizzo = txtIndirizzo.Text
                If ddlReparto.SelectedValue <> "" Then
                    Me.MyOperatore.IDReparto = ddlReparto.SelectedValue
                Else
                    Me.MyOperatore.IDReparto = "-1"
                End If
                Me.MyOperatore.IDTipo = ddlTipo.SelectedValue

                If cbxAmministratore.Checked Then
                    Me.MyOperatore.IsAdmin = 1
                Else
                    Me.MyOperatore.IsAdmin = 0
                End If


                Me.MyOperatore.SalvaData()
                Me.MyCGlobal = New CGlobal
                If lblIdOperatore.Text = "-1" Then
                    MyCGlobal.InviaMailAttivazione(Me.MyOperatore.ID, 1)
                Else
                    MyCGlobal.InviaMailAggiornamento(Me.MyOperatore.ID, 4)
                End If
                lblIdOperatore.Text = Me.MyOperatore.ID
                Me.CaricaOperatore()
                Panel1.Visible = True
                PanelRicercaOperatore.Visible = True
                PanelModificaUtente.Visible = False
            Else
                Dim message2 As String = "Inserire tutti i campi Obbligatori"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)

            End If

        Else
            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        End If

    End Sub

    

    Protected Sub lbModifica_Click(sender As Object, e As EventArgs) Handles lbModifica.Click
        PanelModificaPwd.Visible = True
        btnConfermo.Visible = False
        'Label14.Visible = True
        txtVecchiaPsw.Visible = True
        btnVerifica.Visible = True
        lblError.Visible = False
        Me.DisabilitaCampi()
        btnModifica.Visible = False
        btnSalva.Visible = False
        btnAnnulla.Visible = False
        lbModifica.Visible = False
    End Sub

    Protected Sub btnVerifica_Click(sender As Object, e As EventArgs) Handles btnVerifica.Click
        Me.MyOperatore = New Utente
        Me.MyOperatore.Load(Session("id"))
        Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtVecchiaPsw.Text)

        If MyOperatore.Psw.Equals(miaPasswordCriptata) Then
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
        PanelModificaPwd.Visible = False
        txtVecchiaPsw.Visible = False
        btnVerifica.Visible = False
        lblError.Visible = False
        txtNuovaPassword.Visible = False
        txtConfirmPassword.Visible = False
        btnConfermo.Visible = False
        lblerror2.Visible = False
        Panel3.Visible = False
        lblIdOperatore.Text = "-1"
        txtUserid.Text = ""
        txtCap.Text = ""
        txtIndirizzo.Text = ""
        txtNome.Text = ""
        txtCognome.Text = ""
        lblConferma.Text = ""
        lblErrore.Visible = False
        ddlCliente.SelectedValue = "-1"
        ddlOrganizzazione.SelectedValue = "-1"
        ddlComune.Text = ""
        ddlProvincia.SelectedValue = "-1"
        ddlRegione.SelectedValue = "-1"
        ddlReparto.SelectedValue = "-1"
        ddlTipo.SelectedValue = "-1"
        cbxAmministratore.Checked = False
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
        Dim str As String = "select descrizione ,id from Azienda "
        If Session("tipoutente") = "Operatore" And Session("isadmin") = 1 Then
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
        Me.CaricaLocalita()
    End Sub

    Private Sub CaricaLocalita()
        If ddlSubOrganizzazione.SelectedValue <> "-1" Then
            Me.MySubOrganizzazione = New SubAzienda
            Me.MySubOrganizzazione.Load(ddlSubOrganizzazione.SelectedValue)
            Me.CaricaRegione(Me.MySubOrganizzazione.IDRegione)
            Me.CaricaProvincia(Me.MySubOrganizzazione.IDProvincia, Me.MySubOrganizzazione.IDRegione)
            'Me.CaricaComune(Me.MySubOrganizzazione.IDComune, Me.MySubOrganizzazione.IDProvincia)
            ddlComune.Text = Me.MySubOrganizzazione.IDComune
        Else
            Me.MyOrganizzazione = New Azienda
            Me.MyOrganizzazione.Load(ddlOrganizzazione.SelectedValue)
            Me.CaricaRegione(Me.MyOrganizzazione.IDRegione)
            Me.CaricaProvincia(Me.MyOrganizzazione.IDProvincia, Me.MyOrganizzazione.IDRegione)
            'Me.CaricaComune(Me.MyOrganizzazione.IDComune, Me.MyOrganizzazione.IDProvincia)
            ddlComune.Text = Me.MyOrganizzazione.IDComune
        End If

    End Sub



    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        Me.CaricaSubCliente(ddlCliente.SelectedValue)
    End Sub

    Protected Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        Me.CaricaTipologiaUtente()
    End Sub

    Private Sub CaricaTipologiaUtente()
        Me.MyOperatore = New Utente
        Dim sql As String = "select * from TipoUtente where id=" & ddlTipo.SelectedValue
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            Select Case tab.Rows(0).Item("tipoutente")
                Case "Operatore"

                    ddlOrganizzazione.Visible = True
                    ddlSubOrganizzazione.Visible = True
                    ddlCliente.Visible = False
                    ddlSubCliente.Visible = False
                    cbxAmministratore.Visible = True

                    lblOrganizzazione.Visible = True
                    lblSubOrganizzazione.Visible = True
                    lblCliente.Visible = False
                    lblSubCliente.Visible = False


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
            Me.MyOperatore = New Utente
            MyOperatore.Load(lblIdOperatore.Text)
            If txtNuovaPassword.Visible And txtConfirmPassword.Visible And txtNuovaPassword.Text <> "" And txtConfirmPassword.Text <> "" Then
                Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtNuovaPassword.Text)
                Me.MyOperatore.Psw = miaPasswordCriptata
                Me.MyOperatore.SalvaData()
                Me.MyCGlobal = New CGlobal
                Me.MyCGlobal.InviaMailAggiornamento(Me.MyOperatore.ID, 4)
                Panel3.Visible = True
            End If
        End If
        btnConfermo.Visible = False
        txtVecchiaPsw.Visible = False
        btnVerifica.Visible = False
        lblError.Visible = False
        txtNuovaPassword.Visible = False
        txtConfirmPassword.Visible = False
        lbModifica.Visible = True
        btnModifica.Visible = True
        btnAnnulla.Visible = True
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
                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdOperatore.Text & "'," & Me.MyRecapito.ID & ",'Utente')"
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
                     "where Recapito_Utente.idUtente = " & lblIdOperatore.Text & " and Recapito_Utente.tipoAssociazione like 'Utente' "

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
        lblIdRecapito.Text = "-1"
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
        PanelEliminaOper.Visible = False
        PanelRicercaOperatore.Visible = True
        lblIdOperatore.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyOperatore = New Utente
        If Me.MyOperatore.Delete(lblIdOperatore.Text) Then
            Me.CaricaOperatore()
            lblIdOperatore.Text = "-1"
            PanelEliminaOper.Visible = False
            PanelRicercaOperatore.Visible = True
            Dim message As String = "Operatore eliminato correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        PanelRecapiti.Visible = False
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
        lbModifica.Visible = False
    End Sub

    Protected Sub ddlSubOrganizzazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubOrganizzazione.SelectedIndexChanged
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
        If PanelRecapiti.Visible = True Then
            PanelRecapiti.Visible = False
        Else
            PanelRecapiti.Visible = True
            Me.CaricaContatti()
        End If
    End Sub
End Class