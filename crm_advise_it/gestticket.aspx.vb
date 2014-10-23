Public Class gestticket
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyCGlobal As CGlobal
    Private MyTickets As Tickets
    Private MyUtente As Utente
    Private MyEvento As Evento
    Private MyTempoTicket As TempoTicket
    Private MyCliente As Cliente
    Private MySubCliente As SubCliente
    Private MyFitMail As FitMail
    Private Shared tipo(10) As TextBox
    Private Shared pres As Boolean
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()

        'inserire gestione ticket da link mail con id ticket e id utente(ps:aggiungere nella mail anche il tipo utente)
        Try
            Session("id") = Request.QueryString("utente").Split("\")(1)
            lblIdTickets.Text = Request.QueryString("id")
            Session("tipoutente") = Request.QueryString("tipo")
        Catch
        End Try
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then

                    lblIdTickets.Text = Request.QueryString("id")
                    Me.CaricaTicket()
                    Me.CaricaEventi()
                    Me.CaricaTipoTariffazione()
                    txtData.Text = Date.Now.Date.ToString("yyyy-MM-dd") '"1984-12-10" '
                    txtOra.Text = DateTime.Now.ToString("HH:mm")
                    Select Case Session("tipoutente")
                        Case "SuperAdmin"
                            btRispondi.Visible = True
                            btnCarico.Visible = True
                            txtData.Enabled = True
                            txtOra.Enabled = True
                            lblTipoTariffazione.Visible = True
                            lblTempo.Visible = True
                            ddlTipoTariffazione.Visible = True
                            txtTempo.Visible = True
                            ddlOperatore2.Enabled = True
                            ddlOperatore.Enabled = True
                            txtDescrizione.Enabled = True
                            cbxStealth.Visible = True
                            btnAggiorna.Visible = True
                            btnOnSite.Visible = True
                            btnTelefonata.Visible = True
                        Case "Operatore"
                            btRispondi.Visible = True
                            btnCarico.Visible = True
                            txtData.Enabled = True
                            txtOra.Enabled = True
                            lblTipoTariffazione.Visible = True
                            lblTempo.Visible = True
                            ddlTipoTariffazione.Visible = True
                            txtTempo.Visible = True
                            ddlOperatore2.Enabled = True
                            ddlOperatore.Enabled = True
                            txtDescrizione.Enabled = True
                            cbxStealth.Visible = True
                            btnAggiorna.Visible = True
                            btnOnSite.Visible = True
                            btnTelefonata.Visible = True
                        Case "Utente"
                            btRispondi.Visible = False
                            btnCarico.Visible = False
                            txtData.Enabled = False
                            txtOra.Enabled = False
                            lblTipoTariffazione.Visible = False
                            lblTempo.Visible = False
                            ddlTipoTariffazione.Visible = False
                            txtTempo.Visible = False
                            ddlOperatore2.Enabled = False
                            ddlOperatore.Enabled = False
                            txtDescrizione.Enabled = False
                            cbxStealth.Visible = False
                            btnAggiorna.Visible = False
                            btnOnSite.Visible = False
                            btnTelefonata.Visible = False
                        Case Else
                            Response.Redirect("~/login.aspx")
                    End Select

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CaricaTicket()
        Try
            Me.MyTickets = New Tickets

            Me.MyTickets.Load(lblIdTickets.Text)
            txtDataAp.Text = CDate(Me.MyTickets.DataApertura.ToString.Split(" ")(0)).ToString("yyyy-MM-dd")
            txtOraAp.Text = Me.MyTickets.DataApertura.ToString.Split(" ")(1)
            txtTicket.Text = Me.MyTickets.ID
            txtDataScadenza.Text = CDate(Me.MyTickets.DataScadenza.ToString.Split(" ")(0)).ToString("yyyy-MM-dd")
            Me.CaricaClienti(Me.MyTickets.Cliente.ID)
            Me.CaricaSubClienti(Me.MyTickets.Cliente.ID, Me.MyTickets.SubCliente.ID)
            Me.CaricaContratti(Me.MyTickets.Contratto.ID)
            Me.CaricaUtentiCliente(Me.MyTickets.Cliente.ID, Me.MyTickets.SubCliente.ID, Me.MyTickets.Utente.ID)
            Me.CaricaInventario(Me.MyTickets.Inventario.ID)
            Me.CaricaSoglia(Me.MyTickets.Contratto.ID, Me.MyTickets.Soglia.ID)
            Me.CaricaPerContoDi(Me.MyTickets.Cliente.ID, Me.MyTickets.SubCliente.ID, Me.MyTickets.PerContoDi.ID)
            txtOggetto.Text = Me.MyTickets.Oggetto
            txtDescrizione.Text = Me.MyTickets.Descrizione
            Me.CaricaOperatori(Me.MyTickets.Operatore.ID)
            Dim sql As String = "select* from Evento where Evento.idtickets=" & lblIdTickets.Text
            Dim tab As DataTable = Me.MyGest.GetTab(sql)
            If tab.Rows.Count > 0 Then
                Me.CaricaOperatoreEvento(tab.Rows(tab.Rows.Count - 1)("idoperatore"))
            Else
                Me.CaricaOperatoreEvento()
            End If
        Catch

        End Try

    End Sub

    Private Sub CaricaSoglia(Optional ByVal idc As String = "-1", Optional ByVal ids As String = "-1")
        Dim sql As String = "select soglia,id from SogliaContratto where idcontratto=" & idc
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "0"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTariffa.DataSource = tab
        Me.ddlTariffa.DataTextField = "soglia"
        Me.ddlTariffa.DataValueField = "id"
        Me.ddlTariffa.DataBind()
        Me.ddlTariffa.SelectedValue = ids


    End Sub
    Private Sub CaricaContratti(Optional ByVal idc As String = "-1")
        Me.MyTickets = New Tickets
        Me.MyTickets.Azienda = New Azienda
        Me.MyTickets.Load(lblIdTickets.Text)
        Dim str As String = "select codice AS nomecontratto ,Contratto.id from Contratto " & _
                                    "inner join Contratto_Cliente on Contratto.id=Contratto_Cliente.idcontratto " & _
                                    "where Contratto.idazienda=" & MyTickets.Azienda.ID
        If Session("tipoutente") = "Operatore" Then

            If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
                str = str & "and Contratto_Cliente.tipocliente='subcliente' and Contratto_Cliente.idcliente=" & ddlSubCliente.SelectedValue
            Else
                str = str & "and Contratto_Cliente.tipocliente='cliente' and Contratto_Cliente.idcliente=" & ddlCliente.SelectedValue
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            str = str & "and Contratto.id=-1"

        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)
        If tab.Rows.Count > 1 And Session("tipoutente") <> "Utente" Then
            Me.ddlContratto.DataSource = tab
            Me.ddlContratto.DataTextField = "nomecontratto"
            Me.ddlContratto.DataValueField = "id"
            Me.ddlContratto.DataBind()
            Me.ddlContratto.SelectedValue = idc
        End If
    End Sub

    Private Sub CaricaClienti(Optional ByVal idc As String = "-1")
        Dim str As String = "select ragsoc ,id from Cliente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where Cliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            str = str & "where Cliente.id=" & MyUtente.Cliente.ID
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

    Private Sub CaricaSubClienti(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1")
        Dim str As String = "select ragsoc ,id from SubCliente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where SubCliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            str = str & "where SubCliente.id=" & MyUtente.Cliente.ID
        End If
        str = str & " and SubCliente.idcliente=" & idc
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
        If idc <> "-1" Then
            Me.ddlSubCliente.SelectedValue = idsc
        Else
            Me.ddlSubCliente.SelectedValue = "-1"
        End If
    End Sub

    Private Function RestituisciOrganizzazione() As Integer

        Dim sql As String
            sql = "select idazienda from utente where id=" & Session("id")

        Dim tab As DataTable = Me.MyGest.GetTab(Sql)
        Dim res As Integer = -1
        If tab.Rows.Count > 0 Then
            res = tab.Rows(0)("idazienda")
        End If
        Return res
    End Function

    Private Sub CaricaUtentiCliente(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1", Optional ByVal idu As String = "-1")
        Dim str As String = "select nome + ' ' + cognome AS nomecompleto ,id from Utente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.SubCliente = New SubCliente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            If ddlSubCliente.SelectedValue <> "-1" Then
                str = str & "where Utente.idsubcliente=" & ddlSubCliente.SelectedValue
            Else
                str = str & "where Utente.idcliente=" & ddlCliente.SelectedValue
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            str = str & "where Utente.id=" & MyUtente.ID
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlUtente.DataSource = tab
        Me.ddlUtente.DataTextField = "nomecompleto"
        Me.ddlUtente.DataValueField = "id"
        Me.ddlUtente.DataBind()

        Me.ddlUtente1.DataSource = tab
        Me.ddlUtente1.DataTextField = "nomecompleto"
        Me.ddlUtente1.DataValueField = "id"
        Me.ddlUtente1.DataBind()
        If idu <> "-1" Then
            Me.ddlUtente.SelectedValue = idu
            Me.ddlUtente1.SelectedValue = idu
        Else
            Me.ddlUtente.SelectedValue = "-1"
            Me.ddlUtente1.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaUtentiRis(Optional ByVal idu As String = "-1")
        Dim str As String = "select nome + ' ' + cognome AS nomecompleto ,id from Utente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.SubCliente = New SubCliente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            If ddlSubCliente.SelectedValue <> "-1" Then
                str = str & "where Utente.idsubcliente=" & ddlSubCliente.SelectedValue
            Else
                str = str & "where Utente.idcliente=" & ddlCliente.SelectedValue
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            str = str & "where Utente.id=" & MyUtente.ID
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)



        Me.ddlUtente1.DataSource = tab
        Me.ddlUtente1.DataTextField = "nomecompleto"
        Me.ddlUtente1.DataValueField = "id"
        Me.ddlUtente1.DataBind()

        Me.ddlUtente1.SelectedValue = idu

    End Sub

    Private Sub CaricaOperatori(Optional ByVal idu As String = "-1")
        Me.MyTickets = New Tickets
        Me.MyTickets.Azienda = New Azienda
        Me.MyTickets.Load(lblIdTickets.Text)
        Dim sql As String = "select nome + ' ' + cognome AS nomecompleto,Utente.id from Utente " & _
                            "inner join TipoUtente on TipoUtente.id=Utente.idtipo " & _
                            "where tipoutente='Operatore' and Utente.idazienda=" & MyTickets.Azienda.ID
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Me.ddlOperatore.DataSource = tab
        Me.ddlOperatore.DataTextField = "nomecompleto"
        Me.ddlOperatore.DataValueField = "id"
        Me.ddlOperatore.DataBind()

        If idu <> "-1" Then
            Me.ddlOperatore.SelectedValue = idu
        Else
            Me.ddlOperatore.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaOperatoreEvento(Optional ByVal idue As String = "-1")
        Me.MyTickets = New Tickets
        Me.MyTickets.Azienda = New Azienda
        Me.MyTickets.Load(lblIdTickets.Text)
        Dim sql As String = "select nome + ' ' + cognome AS nomecompleto,Utente.id from Utente " & _
                            "inner join TipoUtente on TipoUtente.id=Utente.idtipo " & _
                            "where tipoutente='Operatore' and Utente.idazienda=" & MyTickets.Azienda.ID
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)
        Me.ddlOperatore2.DataSource = tab
        Me.ddlOperatore2.DataTextField = "nomecompleto"
        Me.ddlOperatore2.DataValueField = "id"
        Me.ddlOperatore2.DataBind()

        If idue <> "-1" Then
            Me.ddlOperatore2.SelectedValue = idue
        Else
            Me.ddlOperatore2.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaPerContoDi(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1", Optional ByVal idu As String = "-1")
        Dim str As String = "select nome + ' ' + cognome AS nomecompleto ,id from Utente "

        Me.MyUtente = New Utente
        Me.MyUtente.SubCliente = New SubCliente
        Me.MyUtente.Cliente = New Cliente
        Me.MyUtente.Load(Session("id"))
        If ddlSubCliente.SelectedValue <> "-1" Then
            str = str & "where Utente.idsubcliente=" & ddlSubCliente.SelectedValue
        Else
            str = str & "where Utente.idcliente=" & ddlCliente.SelectedValue
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlPerContoDi.DataSource = tab
        Me.ddlPerContoDi.DataTextField = "nomecompleto"
        Me.ddlPerContoDi.DataValueField = "id"
        Me.ddlPerContoDi.DataBind()
        If idu <> "-1" Then
            Me.ddlPerContoDi.SelectedValue = idu
        Else
            Me.ddlPerContoDi.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaInventario(Optional ByVal id As String = "-1")
        Me.MyTickets = New Tickets
        Me.MyTickets.Azienda = New Azienda
        Me.MyTickets.Load(lblIdTickets.Text)
        
        Dim str As String = "select codice AS nomecontratto ,Inventario.id from Inventario " & _
                            "inner join Tickets on Tickets.idinventario=Inventario.id " & _
                            "where Tickets.idazienda=" & MyTickets.Azienda.ID
        If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
            str = str & " and Tickets.idsubcliente=" & ddlSubCliente.SelectedValue
        Else
            str = str & " and Inventario.idcliente=" & ddlCliente.SelectedValue
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlInventario.DataSource = tab
        Me.ddlInventario.DataTextField = "nomecontratto"
        Me.ddlInventario.DataValueField = "id"
        Me.ddlInventario.DataBind()

        Me.ddlInventario.SelectedValue = id


        'If ddlUtente.SelectedValue <> "-1" Then
        '    str = str & " and idutente=" & ddlUtente.SelectedValue
        '    Dim tab2 As DataTable = Me.MyGest.GetTab(str)
        '    If tab2.Rows.Count = 1 Then
        '        Me.ddlInventario.SelectedValue = tab2.Rows(0)("id")
        '    End If
        '    'If tab.Rows.Count > 0 Then
        '    '    Me.myinventario = New Inventario
        '    '    Me.myinventario.load(tab.Rows(0)("id"))
        '    'End If
        'End If
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

    Private Sub CaricaEventi()
        Dim sql As String = "Select * from Evento " & _
                            "inner join Stato on Stato.id=Evento.idstato " & _
                            "left outer join Utente on Utente.id=Evento.idoperatore " & _
                            "left outer join Tariffazione on Tariffazione.id=Evento.idtipotariffazione " & _
                            "where idtickets = " & lblIdTickets.Text
        Select Case Session("tipoutente")
            Case "Utente"
                sql = sql & " and stealth=0"
        End Select
        sql = sql & " order by data"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        ListView1.DataSource = tab
        ListView1.DataBind()
        Dim i As Integer = 0
        For Each lvi As ListViewItem In ListView1.Items
            Dim checksi As CheckBox = lvi.FindControl("rd1")
            'Dim descrizione As TextBox '= lvi.FindControl("lblDescrizione")
            Dim label As Label = lvi.FindControl("lblDescrizione")
            label.Font.Size = FontUnit.XXSmall
            'descrizione.Text = label.Text
            'If descrizione.Text = "" Then

            '    descrizione.Visible = False
            'End If
            If tab.Rows(i)("stealth") = 1 Then
                checksi.Checked = True
            End If
            i = i + 1
        Next


        'Dim tstruct As New DataTable
        'Dim c1 As New DataColumn("id", GetType(String), "")
        'tstruct.Columns.Add(c1)
        'Dim c2 As New DataColumn("stato", GetType(String), "")
        'tstruct.Columns.Add(c2)
        'Dim c3 As New DataColumn("dataapertura", GetType(String), "")
        'tstruct.Columns.Add(c3)
        'Dim c4 As New DataColumn("datachiusura", GetType(String), "")
        'tstruct.Columns.Add(c4)
        'Dim c5 As New DataColumn("datascadenza", GetType(String), "")
        'tstruct.Columns.Add(c5)
        'Dim c6 As New DataColumn("dataultimo", GetType(String), "")
        'tstruct.Columns.Add(c6)
        'Dim c7 As New DataColumn("ragsoc", GetType(String), "")
        'tstruct.Columns.Add(c7)
        'Dim c8 As New DataColumn("nomecognomeop", GetType(String), "")
        'tstruct.Columns.Add(c8)
        'Dim c9 As New DataColumn("ragsocsub", GetType(String), "")
        'tstruct.Columns.Add(c9)
        'Dim c10 As New DataColumn("nomecognomeut", GetType(String), "")
        'tstruct.Columns.Add(c10)
        'Dim c11 As New DataColumn("oggetto", GetType(String), "")
        'tstruct.Columns.Add(c11)
        'Dim c12 As New DataColumn("tipodispositivo", GetType(String), "")
        'tstruct.Columns.Add(c12)
        'Dim c13 As New DataColumn("colore", GetType(String), "")
        'tstruct.Columns.Add(c13)
        'For i As Integer = 0 To tab.Rows.Count - 1
        '    tstruct.Rows.Add(i)
        '    tstruct.Rows(i)("colore") = tab.Rows(i)("colore")
        '    tstruct.Rows(i)("id") = tab.Rows(i)("id")
        '    tstruct.Rows(i)("stato") = tab.Rows(i)("stato")
        '    tstruct.Rows(i)("dataapertura") = tab.Rows(i)("dataapertura").ToString.Split(":")(0) & ":" & tab.Rows(i)("dataapertura").ToString.Split(":")(1)
        '    If tab.Rows(i)("datachiusura") = "01/01/9999" Then
        '        tstruct.Rows(i)("datachiusura") = ""
        '    Else
        '        tstruct.Rows(i)("datachiusura") = tab.Rows(i)("datachiusura").ToString.Split(":")(0) & ":" & tab.Rows(i)("datachiusura").ToString.Split(":")(1)
        '    End If
        '    tstruct.Rows(i)("datascadenza") = tab.Rows(i)("datascadenza").ToString.Split(":")(0) & ":" & tab.Rows(i)("datascadenza").ToString.Split(":")(1)
        '    If tab.Rows(i)("dataultimo") = "01/01/9999" Then
        '        tstruct.Rows(i)("dataultimo") = ""
        '    Else
        '        tstruct.Rows(i)("dataultimo") = tab.Rows(i)("dataultimo").ToString.Split(":")(0) & ":" & tab.Rows(i)("dataultimo").ToString.Split(":")(1)
        '    End If

        '    tstruct.Rows(i)("ragsoc") = tab.Rows(i)("ragsoc")
        '    tstruct.Rows(i)("nomecognomeop") = tab.Rows(i)("nomecognomeop")

        '    tstruct.Rows(i)("ragsocsub") = tab.Rows(i)("ragsocsub")
        '    tstruct.Rows(i)("nomecognomeut") = tab.Rows(i)("nomecognomeut")
        '    tstruct.Rows(i)("oggetto") = tab.Rows(i)("oggetto")
        '    tstruct.Rows(i)("tipodispositivo") = tab.Rows(i)("tipodispositivo")
        'Next
        'ListView1.DataSource = tstruct
        'ListView1.DataBind()


    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim rdy As CheckBox = e.Item.FindControl("rd1")
        AddHandler rdy.CheckedChanged, AddressOf Controlla
    End Sub

    Private Sub Controlla(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rd As CheckBox = CType(sender, CheckBox)
        Dim sql As String
        Dim id_doc As Integer = rd.ToolTip
        If rd.Checked Then
            sql = "update Evento set stealth=1 where id=" & id_doc

            'Me.CaricaRigheContratto()
        Else
            sql = "update Evento set stealth=0 where id=" & id_doc
        End If
        MyGest.Execute(sql)
    End Sub


    Protected Sub btnNascondi_Click(sender As Object, e As EventArgs) Handles btnNascondi.Click
        If PanelTicket.Visible = True Then
            PanelTicket.Visible = False
            btnNascondi.Text = "Mostra"
        Else
            PanelTicket.Visible = True
            btnNascondi.Text = "Nascondi"
        End If
    End Sub

    Protected Sub btRispondi_Click(sender As Object, e As EventArgs) Handles btRispondi.Click
        Me.SalvaEvento(3)
    End Sub

    Protected Sub btnCarico_Click(sender As Object, e As EventArgs) Handles btnCarico.Click
        If ddlOperatore2.SelectedValue <> "-1" And ddlOperatore2.SelectedValue <> "" Then
            Me.SalvaEventoCarico(2)
        Else
            ddlOperatore2.BorderColor = Drawing.Color.Red
            Dim message As String = "Inserire operatore a cui assegnare ticket"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        End If

        'gestire mail
    End Sub

    Private Sub SalvaEventoCarico(ByVal idstato As String)
        Me.MyTickets = New Tickets
        Me.MyTickets.Load(lblIdTickets.Text)
        Me.MyTickets.idStato = idstato
        Me.MyTickets.DataUltimo = DateTime.Now
        Me.MyTickets.DataScadenza = txtDataScadenza.Text & " " & TimeOfDay
        Me.MyTickets.SalvaData()

        Me.MyEvento = New Evento
        Me.MyEvento.Operatore = New Utente
        Me.MyEvento.Utente = New Utente
        Me.MyEvento.Tickets = New Tickets
        Me.MyEvento.TipoTariffazione = New Tariffa

        Me.MyEvento.Tempo = txtTempo.Text

        Me.MyEvento.Tickets.ID = lblIdTickets.Text
        Me.MyEvento.Data = txtData.Text & " " & txtOra.Text
        Me.MyEvento.Descrizione = txtDescrizione1.Text
        Me.MyEvento.IDStato = idstato
        'If Session("tipoutente") = "Operatore" Then
        Me.MyEvento.Operatore.ID = ddlOperatore2.SelectedValue 'Session("id")
        'Else
        'Me.MyEvento.Operatore.ID = -1
        'End If
        If cbxStealth.Checked = 1 Then
            Me.MyEvento.Stealth = 1
        Else
            Me.MyEvento.Stealth = 0
        End If
        If ddlUtente1.SelectedValue <> "" Then
            Me.MyEvento.Utente.ID = ddlUtente1.SelectedValue
        Else
            Me.MyEvento.Utente.ID = -1
        End If
        Me.MyEvento.UrlImmagine = ""
        Me.MyEvento.UrlVideo = ""
        If idstato = 2 Or idstato = 4 Then
            Me.MyEvento.Tempo = "00:00"
        Else
            Me.MyEvento.Tempo = txtTempo.Text
        End If
        Me.MyEvento.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
        Me.MyEvento.SalvaData()
        Me.InviaMailEvento(Me.MyEvento.ID)


        txtDescrizione1.Text = ""
        txtData.Text = ""
        txtOra.Text = ""
        cbxStealth.Checked = False
        Me.CaricaUtentiRis(Me.MyTickets.Utente.ID)
        Dim message As String = "Stato ticket aggiornato"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Me.CaricaEventi()
        Response.Redirect("tickets.aspx")
    End Sub

    Private Sub InviaMailEvento(ByVal idevento As String)
        Me.MyCGlobal = New CGlobal
        Me.MyEvento = New Evento
        Me.MyEvento.Tickets = New Tickets

        Me.MyEvento.Utente = New Utente
        Me.MyEvento.Operatore = New Utente
        Me.MyFitMail = New FitMail
        Me.MyFitMail.Ticket = New Tickets
        Me.MyFitMail.Evento = New Evento
        Me.MyFitMail.Utente = New Utente
        Me.MyFitMail.Azienda = New Azienda
        Me.MyFitMail.SubAzienda = New SubAzienda

        Dim utente As Boolean = False
        Dim operatore As Boolean = False
        Dim sess As Boolean = False
        Dim azienda As Boolean = False
        Dim subazienda As Boolean = False
        Dim tab As DataTable
        Dim t As DataTable
        Dim tad As DataTable
        Me.MyEvento.Load(idevento)
            If Me.MyEvento.Utente.ID <> "-1" And cbxStealth.Checked = False Then
            'Me.MyCGlobal.InviaMailEvento(idevento, Me.MyEvento.Utente.ID)
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
            Me.MyFitMail.Evento.ID = Me.MyEvento.ID
            Me.MyFitMail.Utente.ID = Me.MyEvento.Utente.ID
            Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            utente = True
            End If
        If Me.MyEvento.Operatore.ID <> "-1" Then
            'Dim sql As String = "select * from evento where idstato=2 and id<>'" & idevento & "' and idtickets=" & Me.MyEvento.Tickets.ID
            'Dim tab As DataTable = Me.MyGest.GetTab(sql)
            'If tab.Rows.Count > 0 Then
            '    If pres = False Then
            '        Me.MyFitMail.ID = -1
            '        Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
            '        Me.MyFitMail.Evento.ID = Me.MyEvento.ID
            '        Me.MyFitMail.Utente.ID = Me.MyEvento.Tickets.Operatore.ID
            '        Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
            '        Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
            '        Me.MyFitMail.SalvaData()
            '        pres = True
            '    End If
            'Else
            'Me.MyCGlobal.InviaMailEvento(idevento, Me.MyEvento.Operatore.ID)
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
            Me.MyFitMail.Evento.ID = Me.MyEvento.ID
            Me.MyFitMail.Utente.ID = Me.MyEvento.Operatore.ID
            Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            'End If
            operatore = True
        End If
        If Session("id") <> Me.MyEvento.Utente.ID And Session("id") <> Me.MyEvento.Operatore.ID Then
            'Me.MyCGlobal.InviaMailEvento(idevento, Session("id"))
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
            Me.MyFitMail.Evento.ID = Me.MyEvento.ID
            Me.MyFitMail.Utente.ID = Session("id")
            Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            sess = True
        End If

        If Me.MyTickets.Cliente.SubAzienda.ID <> "-1" Then
            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID
            t = Me.MyGest.GetTab(s)
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Dim sql As String = "select * from evento where idstato=2 and id<>" & idevento & " and idtickets=" & Me.MyEvento.Tickets.ID
                    Dim tabc As DataTable = Me.MyGest.GetTab(sql)
                    If tabc.Rows.Count > 0 Then
                        If pres = False Then
                            Me.MyFitMail.ID = -1
                            Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
                            Me.MyFitMail.Evento.ID = Me.MyEvento.ID
                            Me.MyFitMail.Utente.ID = tabc.Rows(0)("idoperatore") 'Me.MyEvento.Tickets.Operatore.ID
                            Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
                            Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
                            Me.MyFitMail.SalvaData()
                            pres = True
                        End If
                    Else
                        Me.MyFitMail.ID = -1
                        Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                        Me.MyFitMail.Evento.ID = -1
                        Me.MyFitMail.Utente.ID = t.Rows(i)("idu")
                        Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                        Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                        Me.MyFitMail.SalvaData()
                    End If
                    azienda = True
                End If
            Next

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID & " and idcliente=" & Me.MyTickets.Cliente.ID & " and idsubcliente=" & Me.MyTickets.SubCliente.ID
            tad = Me.MyGest.GetTab(s)
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = tad.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                End If
            Next
        Else
            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID
            t = Me.MyGest.GetTab(s)
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Dim sql As String = "select * from evento where idstato=2 and id<>'" & idevento & "' and idtickets=" & Me.MyEvento.Tickets.ID
                    Dim tabc As DataTable = Me.MyGest.GetTab(sql)
                    If tabc.Rows.Count > 0 Then
                        If pres = False Then
                            Me.MyFitMail.ID = -1
                            Me.MyFitMail.Ticket.ID = Me.MyEvento.Tickets.ID
                            Me.MyFitMail.Evento.ID = Me.MyEvento.ID
                            Me.MyFitMail.Utente.ID = tabc.Rows(0)("idoperatore") 'Me.MyEvento.Tickets.Operatore.ID
                            Me.MyFitMail.Azienda.ID = Me.MyEvento.Tickets.Azienda.ID
                            Me.MyFitMail.SubAzienda.ID = Me.MyEvento.Tickets.SubAzienda.ID
                            Me.MyFitMail.SalvaData()
                            pres = True
                        End If
                    Else
                        Me.MyFitMail.ID = -1
                        Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                        Me.MyFitMail.Evento.ID = -1
                        Me.MyFitMail.Utente.ID = t.Rows(i)("idu")
                        Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                        Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                        Me.MyFitMail.SalvaData()
                    End If
                    subazienda = True
                End If
            Next

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID & " and idcliente=" & Me.MyTickets.Cliente.ID & " and idsubcliente=" & Me.MyTickets.SubCliente.ID
            tad = Me.MyGest.GetTab(s)
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = tad.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                End If
            Next
        End If
        Me.ResiduoSoglia()
        If utente Then
            Me.MyCGlobal.InviaMailEvento(idevento, Me.MyEvento.Utente.ID, tipo)
        End If
        If operatore Then
            Me.MyCGlobal.InviaMailEvento(idevento, Me.MyEvento.Operatore.ID, tipo)
        End If
        If sess Then
            Me.MyCGlobal.InviaMailEvento(idevento, Session("id"), tipo)
        End If
        If azienda Then
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailEvento(idevento, t.Rows(i)("idu"), tipo)
                End If
            Next
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailEvento(idevento, tad.Rows(i)("idu"), tipo)
                End If
            Next
        ElseIf subazienda Then
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailEvento(idevento, t.Rows(i)("idu"), tipo)
                End If
            Next
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyEvento.Tickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailEvento(idevento, tad.Rows(i)("idu"), tipo)
                End If
            Next
        End If
        utente = False
        operatore = False
        sess = False
        azienda = False
        subazienda = False

        Me.MyFitMail.Delete(Me.MyEvento.Tickets.ID, Me.MyEvento.ID, Me.MyTickets.Azienda.ID, Me.MyTickets.SubAzienda.ID)
        pres = False
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


    Private Sub SalvaEvento(ByVal idstato As String)
        Me.MyTempoTicket = New TempoTicket
        Me.MyTickets = New Tickets
        Me.MyTickets.Contratto = New Contratto
        Me.MyTickets.Cliente = New Cliente
        Me.MyTickets.SubCliente = New SubCliente
        Me.MyTickets.Load(lblIdTickets.Text)
        Dim s As String = " select * from Tickets " & _
                          " inner join Contratto on Contratto.id=Tickets.idcontratto " & _
                          " where idcontratto = " & Me.MyTickets.Contratto.ID
        Dim t As DataTable = Me.MyGest.GetTab(s)
        If t.Rows.Count > 0 Then
            If t.Rows(0)("adata") < Date.Today Then
                If Me.MyTickets.SubCliente.ID <> "-1" Then
                    Me.MySubCliente = New SubCliente
                    Me.MySubCliente.Load(Me.MyTickets.SubCliente.ID)
                    Me.MySubCliente.Blocco_Amm = 1
                    Me.MySubCliente.SalvaData()
                    Dim message As String = "Contratto scaduto. SubCliente bloccato"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                    Exit Sub
                Else
                    Me.MyCliente = New Cliente
                    Me.MyCliente.Load(Me.MyTickets.Cliente.ID)
                    Me.MyCliente.Blocco_Amm = 1
                    Me.MyCliente.SalvaData()
                    Dim message As String = "Contratto scaduto. Cliente bloccato"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                    Exit Sub
                End If
            End If
        End If

        Dim sql As String = "select tempo from evento " & _
                            " inner join Tickets on Tickets.id=Evento.idtickets " & _
                            " where idtipotariffazione = " & ddlTipoTariffazione.SelectedValue & " And idcontratto = " & Me.MyTickets.Contratto.ID & " and Tickets.id=" & lblIdTickets.Text
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim tempo As String
        Dim ora As Integer = 0
        Dim minuto As Integer = 0
        If txtTempo.Text <> "" Then
            ora = CInt(txtTempo.Text.Split(":")(0))
            minuto = CInt(txtTempo.Text.Split(":")(1))
        Else
            ora = 0
            minuto = 0
        End If
        If tab.Rows.Count > 0 And Me.MyTickets.Contratto.ID <> "-1" Then
            For i As Integer = 0 To tab.Rows.Count - 1
                ora = ora + CInt(tab.Rows(i)("tempo").ToString.Split(":")(0))
                minuto = minuto + CInt(tab.Rows(i)("tempo").ToString.Split(":")(1))
            Next
        End If

        Dim sql2 As String = "select tempo from evento " & _
                            " inner join Tickets on Tickets.id=Evento.idtickets " & _
                            " where idtipotariffazione = " & ddlTipoTariffazione.SelectedValue & " And idcontratto = " & Me.MyTickets.Contratto.ID
        Dim tab2 As DataTable = Me.MyGest.GetTab(sql2)
        Dim tempo2 As String
        Dim ora2 As Integer = 0
        Dim minuto2 As Integer = 0
        If txtTempo.Text <> "" Then
            ora2 = CInt(txtTempo.Text.Split(":")(0))
            minuto2 = CInt(txtTempo.Text.Split(":")(1))
        Else
            ora2 = 0
            minuto2 = 0
        End If
        If tab2.Rows.Count > 0 And Me.MyTickets.Contratto.ID <> "-1" Then
            For i As Integer = 0 To tab2.Rows.Count - 1
                ora2 = ora2 + CInt(tab2.Rows(i)("tempo").ToString.Split(":")(0))
                minuto2 = minuto2 + CInt(tab2.Rows(i)("tempo").ToString.Split(":")(1))
            Next
        End If
        tempo2 = Me.ValutaMinutaggio(ora2, minuto2)


        tempo = Me.ValutaMinutaggio(ora, minuto) 'ora & ":" & minuti
        Dim str As String = "select tempotot,soglia,fuorisoglia,avviso from TempoTicket " & _
                            " inner join Tickets on Tickets.id=TempoTicket.idTicket " & _
                            " inner join Contratto on Contratto.id=Tickets.idcontratto " & _
                            " inner join SogliaContratto on SogliaContratto.idcontratto=Contratto.id " & _
                            " inner join TipoFuoriSoglia on TipoFuoriSoglia.id=SogliaContratto.idfuori " & _
                            " where TempoTicket.idtipotariffazione = " & ddlTipoTariffazione.SelectedValue & " And Tickets.idcontratto = " & Me.MyTickets.Contratto.ID

        Dim tb As DataTable = Me.MyGest.GetTab(str)
        If tb.Rows.Count > 0 Then
            If tb.Rows(0)("soglia").ToString.Split(":")(0) > ora2 Or (tb.Rows(0)("fuorisoglia") = "Avvisa" Or tb.Rows(0)("fuorisoglia") = "Listino") Then
                If tb.Rows(0)("avviso").ToString.Split(":")(0) < ora2 Then
                    Me.InviaMailAvviso(CInt(tb.Rows(0)("soglia").ToString.Split(":")(0)) - ora2)
                End If
                If tb.Rows(0)("fuorisoglia") = "Avvisa" Or tb.Rows(0)("fuorisoglia") = "Listino" Then
                    Me.ApportaConseguenza(tb)
                End If
                If txtTempo.Text <> "" And ddlTipoTariffazione.SelectedValue <> "-1" Then
                    If lblDescrizione1.Text <> "" Then
                        Me.MyTickets = New Tickets
                        Me.MyTickets.Load(lblIdTickets.Text)
                        Me.MyTickets.idStato = idstato
                        If idstato = 5 Then
                            Me.MyTickets.DataChiusura = DateTime.Now
                        End If
                        Me.MyTickets.DataUltimo = DateTime.Now

                        Me.MyTickets.DataScadenza = txtDataScadenza.Text & " " & TimeOfDay
                        'Me.MyTickets.Cliente
                        Me.MyTickets.SalvaData()

                        Me.MyEvento = New Evento
                        Me.MyEvento.Operatore = New Utente
                        Me.MyEvento.Utente = New Utente
                        Me.MyEvento.Tickets = New Tickets
                        Me.MyEvento.TipoTariffazione = New Tariffa



                        Me.MyEvento.Tickets.ID = lblIdTickets.Text
                        Me.MyEvento.Data = txtData.Text & " " & txtOra.Text
                        Me.MyEvento.Descrizione = txtDescrizione1.Text
                        Me.MyEvento.IDStato = idstato
                        'If Session("tipoutente") = "Operatore" Then
                        Me.MyEvento.Operatore.ID = ddlOperatore2.SelectedValue 'Session("id")
                        'Else
                        '    Me.MyEvento.Operatore.ID = -1
                        'End If
                        If cbxStealth.Checked = 1 Then
                            Me.MyEvento.Stealth = 1
                        Else
                            Me.MyEvento.Stealth = 0
                        End If
                        Me.MyEvento.Utente.ID = ddlUtente1.SelectedValue
                        Me.MyEvento.UrlImmagine = ""
                        Me.MyEvento.UrlVideo = ""
                        Me.MyEvento.Tempo = txtTempo.Text
                        Me.MyEvento.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
                        Me.MyEvento.SalvaData()
                        Me.InviaMailEvento(Me.MyEvento.ID)


                        Me.MyTempoTicket = New TempoTicket
                        Me.MyTempoTicket.Ticket = New Tickets
                        Me.MyTempoTicket.TipoTariffazione = New Tariffa
                        Dim id = VerificaTempoTicket()
                        If id <> "-1" Then
                            Me.MyTempoTicket.ID = id
                            Me.MyTempoTicket.TempoTot = tempo
                        Else
                            Me.MyTempoTicket.TempoTot = txtTempo.Text
                        End If

                        Me.MyTempoTicket.Ticket.ID = lblIdTickets.Text
                        Me.MyTempoTicket.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
                        Me.MyTempoTicket.SalvaData()

                        txtDescrizione1.Text = ""
                        txtData.Text = ""
                        txtOra.Text = ""
                        cbxStealth.Checked = False
                        Me.CaricaUtentiRis()
                        Dim message As String = "Stato ticket aggiornato"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                        Me.CaricaEventi()
                        Response.Redirect("tickets.aspx")
                    Else
                        Dim message As String = "Evento non inserito"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

                    End If
                Else
                    txtTempo.BorderColor = Drawing.Color.Red
                    ddlTipoTariffazione.BorderColor = Drawing.Color.Red
                    Dim message As String = "Inserire tipologia assistenza e tempo di utilizzo"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                End If
            Else
                ApportaConseguenza(tb)
            End If
        Else
            If txtTempo.Text <> "" And ddlTipoTariffazione.SelectedValue <> "-1" Then
                If lblDescrizione1.Text <> "" Then
                    Me.MyTickets = New Tickets
                    Me.MyTickets.Load(lblIdTickets.Text)
                    Me.MyTickets.idStato = idstato
                    If idstato = 5 Then
                        Me.MyTickets.DataChiusura = DateTime.Now
                    End If
                    Me.MyTickets.DataUltimo = DateTime.Now
                    Me.MyTickets.DataScadenza = txtDataScadenza.Text & " " & TimeOfDay
                    Me.MyTickets.SalvaData()

                    Me.MyEvento = New Evento
                    Me.MyEvento.Operatore = New Utente
                    Me.MyEvento.Utente = New Utente
                    Me.MyEvento.Tickets = New Tickets
                    Me.MyEvento.TipoTariffazione = New Tariffa



                    Me.MyEvento.Tickets.ID = lblIdTickets.Text
                    Me.MyEvento.Data = txtData.Text & " " & txtOra.Text
                    Me.MyEvento.Descrizione = txtDescrizione1.Text
                    Me.MyEvento.IDStato = idstato
                    'If Session("tipoutente") = "Operatore" Then
                    Me.MyEvento.Operatore.ID = ddlOperatore2.SelectedValue 'Session("id")
                    'Else
                    '    Me.MyEvento.Operatore.ID = -1
                    'End If
                    If cbxStealth.Checked = 1 Then
                        Me.MyEvento.Stealth = 1
                    Else
                        Me.MyEvento.Stealth = 0
                    End If
                    Me.MyEvento.Utente.ID = ddlUtente1.SelectedValue
                    Me.MyEvento.UrlImmagine = ""
                    Me.MyEvento.UrlVideo = ""
                    Me.MyEvento.Tempo = txtTempo.Text
                    Me.MyEvento.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
                    Me.MyEvento.SalvaData()
                    Me.InviaMailEvento(Me.MyEvento.ID)


                    Me.MyTempoTicket = New TempoTicket
                    Me.MyTempoTicket.Ticket = New Tickets
                    Me.MyTempoTicket.TipoTariffazione = New Tariffa
                    Dim id = VerificaTempoTicket()
                    If id <> "-1" Then
                        Me.MyTempoTicket.ID = id
                        Me.MyTempoTicket.TempoTot = tempo
                    Else
                        Me.MyTempoTicket.TempoTot = txtTempo.Text
                    End If

                    Me.MyTempoTicket.Ticket.ID = lblIdTickets.Text
                    Me.MyTempoTicket.TipoTariffazione.ID = ddlTipoTariffazione.SelectedValue
                    Me.MyTempoTicket.SalvaData()

                    txtDescrizione1.Text = ""
                    txtData.Text = ""
                    txtOra.Text = ""
                    cbxStealth.Checked = False
                    Me.CaricaUtentiRis()
                    Dim message As String = "Stato ticket aggiornato"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                    Me.CaricaEventi()
                    Response.Redirect("tickets.aspx")
                Else
                    Dim message As String = "Evento non inserito"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

                End If
            Else
                txtTempo.BorderColor = Drawing.Color.Red
                ddlTipoTariffazione.BorderColor = Drawing.Color.Red
                Dim message As String = "Inserire tipologia assistenza e tempo di utilizzo"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If

        End If

    End Sub

    Private Sub ApportaConseguenza(ByVal tab As DataTable)
        Select Case tab.Rows(0)("fuorisoglia")
            Case "Blocca"
                Dim message As String = "Attenzione! Superata la soglia contratto. Si provvedrà in automatico al blocco dell'apertura ticket"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

                Me.MyTickets = New Tickets
                Me.MyTickets.Contratto = New Contratto
                Me.MyCliente = New Cliente
                Me.MyTickets.Cliente = New Cliente
                Me.MySubCliente = New SubCliente
                Me.MyTickets.SubCliente = New SubCliente
                Me.MyTickets.Load(lblIdTickets.Text)

                Dim tempo = 0
                Dim str As String = "select distinct(tariffazione),tempotot,soglia,fuorisoglia,avviso from TempoTicket " & _
                            " inner join Tariffazione on Tariffazione.id=TempoTicket.idtipotariffazione  " & _
                            " inner join SogliaContratto on SogliaContratto.idtipotariffazione =Tariffazione.id " & _
                            " inner join Contratto on Contratto.id=SogliaContratto .idcontratto " & _
                            " inner join TipoFuoriSoglia on TipoFuoriSoglia.id=SogliaContratto.idfuori " & _
                            " where SogliaContratto.idcontratto = " & Me.MyTickets.Contratto.ID


                Dim tb As DataTable = Me.MyGest.GetTab(str)

                If tb.Rows.Count > 0 Then
                    For i As Integer = 0 To tb.Rows.Count - 1

                        tempo = tempo + tb.Rows(i)("tempotot")
                    Next
                    If tempo > tb.Rows(0)("soglia") Then
                        If Me.MyTickets.SubCliente.ID <> "-1" Then
                            Me.MySubCliente.Load(Me.MyTickets.SubCliente.ID)
                            Me.MySubCliente.Blocco_Amm = 1
                            Me.MySubCliente.SalvaData()
                        Else
                            Me.MyCliente.Load(Me.MyTickets.Cliente.ID)
                            Me.MyCliente.Blocco_Amm = 1
                            Me.MyCliente.SalvaData()
                        End If
                    End If
                End If
            Case "Listino"
                Dim message As String = "Attenzione! Superata la soglia contratto. Si provvedrà in automatico tariffare l'assistenza secondo listino"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            Case "Avvisa"
                Dim message As String = "Attenzione! Superata la soglia contratto. Si provvedrà in automatico ad inviare un'email al cliente per il rinnovo"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                Me.InviaMailAvviso()
        End Select
    End Sub

    Private Sub InviaMailAvviso(Optional temporesiduo As Integer = 0)

    End Sub

    Private Function VerificaTempoTicket() As String
        Dim sql As String = "select * from TempoTicket where idtipotariffazione=" & ddlTipoTariffazione.SelectedValue & " and idticket=" & lblIdTickets.Text
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim idtempoticket = "-1"
        If tab.Rows.Count > 0 Then
            idtempoticket = tab.Rows(0)("id")
        End If
        Return idtempoticket
    End Function

    Protected Sub btnRispostaCli_Click(sender As Object, e As EventArgs) Handles btnRispostaCli.Click
        Me.SalvaEventoCarico(4)
    End Sub

    Protected Sub btnChiudi_Click(sender As Object, e As EventArgs) Handles btnChiudi.Click
        Me.SalvaEvento(5)
    End Sub

    Protected Sub btnOnSite_Click(sender As Object, e As EventArgs) Handles btnOnSite.Click


    End Sub

    Protected Sub btnAggiorna_Click(sender As Object, e As EventArgs) Handles btnAggiorna.Click
        Me.MyTickets = New Tickets
        Me.MyTickets.Azienda = New Azienda
        Me.MyTickets.SubAzienda = New SubAzienda
        Me.MyTickets.Cliente = New Cliente
        Me.MyTickets.SubCliente = New SubCliente
        Me.MyTickets.Utente = New Utente
        Me.MyTickets.PerContoDi = New Utente
        Me.MyTickets.Soglia = New SogliaContratto
        Me.MyTickets.Contratto = New Contratto
        Me.MyTickets.Listino = New Listino
        Me.MyTickets.Inventario = New Inventari
        Me.MyTickets.AltroInventario = New Inventari
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.SubAzienda = New SubAzienda
        Me.MyTickets.Operatore = New Utente

        Me.MyTickets.Load(lblIdTickets.Text)
        'Me.MyTickets.DataScadenza = CDate(txtDataScadenza.Text)
        'Me.MyTickets.Cliente.ID = ddlCliente.SelectedValue
        'If ddlContratto.SelectedValue = "" Then
        '    Me.MyTickets.Contratto.ID = -1
        'Else
        '    Me.MyTickets.Contratto.ID = ddlContratto.SelectedValue
        'End If
        'Me.MyTickets.Utente.ID = ddlUtente.SelectedValue
        'Me.MyTickets.Inventario.ID = ddlInventario.SelectedValue
        'Me.MyTickets.SubCliente.ID = ddlSubCliente.SelectedValue
        'Me.MyTickets.PerContoDi.ID = ddlPerContoDi.SelectedValue
        Me.MyTickets.Descrizione = txtDescrizione.Text
        Me.MyTickets.Operatore.ID = ddlOperatore.SelectedValue
        Me.MyTickets.SalvaData()
        Me.InviaMailAggiornamentoTicket(Me.MyTickets.ID)
        Dim message2 As String = "Descrizione e/o Operatore aggiornati correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)

    End Sub

    Private Sub InviaMailAggiornamentoTicket(ByVal idticket As String)
        Me.MyCGlobal = New CGlobal
        Me.MyTickets = New Tickets
        Me.MyTickets.Utente = New Utente
        Me.MyTickets.PerContoDi = New Utente

        Me.MyFitMail = New FitMail
        Me.MyFitMail.Ticket = New Tickets
        Me.MyFitMail.Evento = New Evento
        Me.MyFitMail.Utente = New Utente
        Me.MyFitMail.Azienda = New Azienda
        Me.MyFitMail.SubAzienda = New SubAzienda

        Dim utente As Boolean = False
        Dim percontodi As Boolean = False
        Dim sess As Boolean = False



        Me.MyTickets.Load(idticket)
        If Me.MyTickets.Utente.ID <> "-1" Then
            'Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Me.MyTickets.Utente.ID)
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
            Me.MyFitMail.Evento.ID = -1
            Me.MyFitMail.Utente.ID = Me.MyTickets.Utente.ID
            Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            utente = True
        End If
        If Me.MyTickets.PerContoDi.ID <> "-1" Then
            'Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Me.MyTickets.PerContoDi.ID)
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
            Me.MyFitMail.Evento.ID = -1
            Me.MyFitMail.Utente.ID = Me.MyTickets.PerContoDi.ID
            Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            percontodi = True
        End If
        If Session("id") <> Me.MyTickets.Utente.ID And Session("id") <> Me.MyTickets.PerContoDi.ID Then
            'Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Session("id"))
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
            Me.MyFitMail.Evento.ID = -1
            Me.MyFitMail.Utente.ID = Session("id")
            Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            sess = True
        End If
        Me.ResiduoSoglia()
        If utente Then
            Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Me.MyTickets.Utente.ID, tipo)
        End If
        If percontodi Then
            Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Me.MyTickets.PerContoDi.ID, tipo)
        End If
        If sess Then
            Me.MyCGlobal.InviaMailAggiornamentoTicket(idticket, Session("id"), tipo)
        End If
        
        utente = False
        percontodi = False
        sess = False
       
        Me.MyFitMail.Delete(Me.MyTickets.ID, -1, Me.MyTickets.Azienda.ID, Me.MyTickets.SubAzienda.ID)

    End Sub

    Protected Sub btnTelefonata_Click(sender As Object, e As EventArgs) Handles btnTelefonata.Click

    End Sub

    Private Sub ResiduoSoglia()
        Dim contratto = -1
        If ddlContratto.SelectedValue <> "" Then
            contratto = ddlContratto.SelectedValue
        End If

        Dim sql As String = " select soglia,tariffazione,tiposoglia from SogliaContratto " & _
                            " inner join Tariffazione on Tariffazione.id=SogliaContratto.idtipotariffazione " & _
                            " inner join TipoSoglia on TipoSoglia.id=SogliaContratto.idtiposoglia " & _
                            " where idcontratto = " & contratto
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)


        'calcolo residuo soglie
        Dim str As String = "select sum(cast(substring(tempotot,1,2)as integer))as tempotot,sum(cast(substring(tempotot,4,2)as integer))as mintot,TempoTicket .idtipotariffazione   from TempoTicket " & _
                            "inner join Tickets on Tickets.id=TempoTicket.idTicket " & _
                            "inner join Contratto on Contratto.id=Tickets.idcontratto " & _
                            "where Contratto.id = " & contratto & _
                            "group by TempoTicket .idtipotariffazione "

        Dim tb As DataTable = Me.MyGest.GetTab(str)

        Dim tabresiduo As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM SogliaContratto inner join TipoFuoriSoglia on TipoFuoriSoglia.id=SogliaContratto.idfuori " & _
                     "inner join TipoSoglia on TipoSoglia.id=SogliaContratto.idtiposoglia " & _
                     "inner join Tariffazione on Tariffazione.id=SogliaContratto.idtipotariffazione " & _
                     "inner join Contratto on Contratto.id=SogliaContratto.idcontratto " & _
                     "left outer join Listino on Listino.id=SogliaContratto.idlistino " & _
                     "where Contratto.id = " & contratto & " "

        tabresiduo = MyGest.GetTab(sqlStr)
        Dim temporesiduo(10)
        Dim sogliaora
        Dim sogliamin
        For i As Integer = 0 To tabresiduo.Rows.Count - 1
            temporesiduo(i) = tabresiduo.Rows(i)("soglia")
            For j As Integer = 0 To tb.Rows.Count - 1
                If tabresiduo.Rows(i)("idtipotariffazione") = tb.Rows(j)("idtipotariffazione") Then
                    Try
                        sogliaora = tabresiduo.Rows(i)("soglia").ToString.Split(":")(0)
                    Catch
                        sogliaora = tabresiduo.Rows(i)("soglia")
                    End Try
                    Try
                        sogliamin = tb.Rows(i)("soglia").ToString.Split(":")(1)
                    Catch
                        sogliamin = "00"
                    End Try

                    Dim differenza = sogliaora - Me.ValutaMinutaggio(tb.Rows(j)("tempotot"), tb.Rows(j)("mintot")).ToString.Split(":")(0) & ":" & sogliamin - Me.ValutaMinutaggio(tb.Rows(j)("tempotot"), tb.Rows(j)("mintot")).ToString.Split(":")(1)
                    If CInt(differenza.ToString.Split(":")(1)) < 0 Then
                        temporesiduo(i) = CInt(differenza.ToString.Split(":")(0)) - 1 & ":" & 60 + CInt(differenza.ToString.Split(":")(1))
                    Else
                        temporesiduo(i) = differenza
                    End If

                End If
            Next
            Try
                If temporesiduo(i).ToString.Split(":")(1).Length = 1 Then
                    temporesiduo(i) = temporesiduo(i).ToString.Split(":")(0) & ":0" & temporesiduo(i).ToString.Split(":")(1)
                End If
            Catch
            End Try
        Next

        'fine calcolo residuo


        If tab.Rows.Count > 0 Then
            For i As Integer = 0 To tab.Rows.Count - 1
                tipo(i) = New TextBox

                If temporesiduo(i).ToString.Split(":")(0) >= 0 Then
                    tipo(i).Text = tab.Rows(i)("tariffazione") & " - " & tab.Rows(i)("soglia") & " " & tab.Rows(i)("tiposoglia") & " - " & temporesiduo(i) & " " & tab.Rows(i)("tiposoglia") & " rimanenti"
                Else
                    tipo(i).Text = tab.Rows(i)("tariffazione") & " - " & tab.Rows(i)("soglia") & " " & tab.Rows(i)("tiposoglia") & " - Soglia superata di " & temporesiduo(i).ToString.Replace("-", "") & " " & tab.Rows(i)("tiposoglia")
                End If

            Next

        End If
    End Sub
End Class