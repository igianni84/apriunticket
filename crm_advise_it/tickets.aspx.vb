Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Data.OleDb

Public Class tickets1
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyAzienda As Azienda
    Private MyUtente As Utente
    Private MyTicket As Tickets
    Private MyCGlobal As CGlobal
    Private FILE_EXCELWR As String = CGlobal.fileExcelOutWr
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
                    Me.MyCGlobal = New CGlobal
                    If app.Length > 1 Then
                        If MyCGlobal.ControlloDelete("Evento", "idtickets", app(1).Split("=")(1)) Then
                            Try
                                Me.MyTicket = New Tickets
                                Me.MyTicket.Delete(app(1).Split("=")(1))
                                Dim message As String = "Ticket " & app(1).Split("=")(1) & " eliminato correttamente"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                            Catch
                                Dim message As String = "Errore nell'eliminazione del ticket"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                            End Try
                        Else
                            Dim message As String = "Il ticket non può essere cancellato. Ci sono eventi associati ad esso"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                        End If
                    End If
                    Me.CaricaClientiListView2()
                    'Me.CaricaClienti()
                    Me.CaricaTicket()
                    ContaTickets()
                    Me.VerificaSelezione()
                    Try
                        lblClienteFil.Text = Session("nomecliente")
                    Catch
                    End Try

                    'Me.CreaMenu()

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub VerificaSelezione()
        If Session("aperto") Then
            Label1.BackColor = Color.Yellow
        End If
        If Session("carico") Then
            Label2.BackColor = Color.Yellow
        End If
        If Session("risoperatore") Then
            Label3.BackColor = Color.Yellow
        End If
        If Session("riscliente") Then
            Label4.BackColor = Color.Yellow
        End If
        If Session("chiuso") Then
            Label5.BackColor = Color.Yellow
        End If
    End Sub

    'Private Sub CaricaClienti(Optional ByVal idc As String = "-1")
    '    Dim str As String = "select ragsoc ,id from Cliente "
    '    If Session("tipoutente") = "Operatore" Then
    '        Me.MyUtente = New Utente
    '        Me.MyUtente.Azienda = New Azienda
    '        Me.MyUtente.Load(Session("id"))
    '        str = str & "where Cliente.idazienda=" & MyUtente.Azienda.ID
    '        'ddlCliente.Enabled = True
    '    ElseIf Session("tipoutente") = "Utente" Then
    '        Me.MyUtente = New Utente
    '        Me.MyUtente.Cliente = New Cliente
    '        Me.MyUtente.Load(Session("id"))
    '        str = str & "where Cliente.id=" & MyUtente.Cliente.ID
    '        'ddlCliente.Enabled = False
    '    Else
    '        ddlCliente.Enabled = True
    '    End If
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


    Private Sub ContaTickets(Optional idstato As String = "-1", Optional tipo As String = "-1", Optional id As String = "-1")
        Dim sql As String = "select count(idstato) as cont,colore,idstato from Tickets " & _
                            "inner join Stato on Stato.id=Tickets.idStato " & _
                            "inner join Cliente on Cliente.id=Tickets.idcliente " & _
                            "left outer join Utente on Utente.id=Tickets.idutente " & _
                            "left outer join SubCliente on SubCliente.id=Tickets.idSubCliente " & _
                            "left outer join Inventario on Inventario.id=Tickets.idinventario " & _
                            "left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo "

        sql = sql & "where Cliente.id<>-1"


        If Session("aperto") Or Session("carico") Or Session("risoperatore") Or Session("riscliente") Or Session("chiuso") Then
            sql = sql & " and ("

            If Session("aperto") Then
                sql = sql & " idstato=1 or "
            End If

            If Session("carico") Then
                sql = sql & " idstato=2 or "
            End If

            If Session("risoperatore") Then
                sql = sql & " idstato=3 or "
            End If

            If Session("riscliente") Then
                sql = sql & " idstato=4 or "
            End If

            If Session("chiuso") Then
                sql = sql & " idstato=5 or "
            End If

            sql = sql.Remove(sql.LastIndexOf("or"), 2) & " )"
            'Else
            '    sql = sql & "and idstato<>5"
        End If
        'If idstato <> "-1" Then
        '    sql = sql & " and  idstato=" & idstato
        'End If
        'If id <> "-1" Then
        '    If tipo = "Cliente" Then
        '        sql = sql & " and Cliente.id=" & id
        '    Else
        '        sql = sql & " and SubCliente.id=" & id
        '    End If
        'End If
        If id <> "-1" Then
            If tipo = "Cliente" Or Session("idcodicecliente").ToString.Split("-")(1).StartsWith("C") Then
                sql = sql & " and Cliente.id=" & id
            ElseIf tipo = "SubCliente" Or Session("idcodicecliente").ToString.Split("-")(1).StartsWith("SC") Then
                sql = sql & " and SubCliente.id=" & id
            End If
        ElseIf Session("idcodicecliente") <> "" And Session("idcodicecliente") <> Nothing And Not IsDBNull(Session("idcodicecliente")) Then
            If Session("idcodicecliente").ToString.Split("-")(1).StartsWith("C") Then
                sql = sql & " and Cliente.id=" & Session("idcodicecliente").ToString.Split("-")(0)
            ElseIf Session("idcodicecliente").ToString.Split("-")(1).StartsWith("SC") Then
                sql = sql & " and SubCliente.id=" & Session("idcodicecliente").ToString.Split("-")(0)
            End If
        End If
        If btnGiorno.BackColor = Color.YellowGreen Or Session("componente") = "giorno" Then
            sql = sql & " and dataapertura like '" & Date.Now & "'"
        ElseIf btnSettimana.BackColor = Color.YellowGreen Or Session("componente") = "settimana" Then
            sql = sql & " and dataapertura between '" & Date.Now.AddDays(-7).ToString("yyyy-MM-dd") & "' and '" & Date.Now.ToString("yyyy-MM-dd") & "'"
        ElseIf btnMese.BackColor = Color.YellowGreen Or Session("componente") = "mese" Then
            sql = sql & " and dataapertura between '" & Date.Now.AddDays(-31).ToString("yyyy-MM-dd") & "' and '" & Date.Now.ToString("yyyy-MM-dd") & "'"
        Else
            If txtDataDa.Text <> "" Then
                sql = sql & " and dataapertura>='" & txtDataDa.Text & "'"
            End If
            If txtDataA.Text <> "" Then
                sql = sql & " and dataapertura<='" & txtDataA.Text & "'"
            End If
        End If

        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sql = sql & " and Cliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" And Session("isadmin") = 1 Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.SubCliente.ID <> "-1" Then
                Dim s As String = "Select * from Utente where idsubcliente=" & MyUtente.SubCliente.ID
                Dim t As DataTable = Me.MyGest.GetTab(s)

                sql = sql & " and SubCliente.id=" & MyUtente.SubCliente.ID
                sql = sql & " or( Tickets.idutente=" & MyUtente.ID & "or Tickets.idpercontodi=" & MyUtente.ID & ")"
                For i As Integer = 0 To t.Rows.Count - 1
                    sql = sql & "or (Tickets.idpercontodi =" & t.Rows(i)("id") & "or Tickets.idpercontodi =" & t.Rows(i)("id") & ")"
                Next
            Else
                sql = sql & " and stealth=0 and Cliente.id=" & MyUtente.Cliente.ID
            End If
        ElseIf Session("tipoutente") = "Utente" And Session("isadmin") = 0 Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            sql = sql & " and stealth=0 and Tickets.idutente=" & MyUtente.ID & "or Tickets.idpercontodi=" & MyUtente.ID
        End If



        sql = sql & "group by idstato,colore"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        lblAperti.Text = 0
        lblInCarico.Text = 0
        lblRisCliente.Text = 0
        lblRisOperatore.Text = 0
        lblChiuso.Text = 0
        For i As Integer = 0 To tab.Rows.Count - 1
            Select Case tab.Rows(i)("idstato")
                Case 1
                    lblAperti.Text = tab.Rows(i)("cont")
                    lblAperti.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                    Label1.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                Case 2
                    lblInCarico.Text = tab.Rows(i)("cont")
                    lblInCarico.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                    Label2.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                Case 4
                    lblRisCliente.Text = tab.Rows(i)("cont")
                    lblRisCliente.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                    Label3.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                Case 3
                    lblRisOperatore.Text = tab.Rows(i)("cont")
                    lblRisOperatore.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                    Label4.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                Case 5
                    lblChiuso.Text = tab.Rows(i)("cont")
                    lblChiuso.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))
                    Label5.ForeColor = Drawing.Color.FromName(tab.Rows(i)("colore"))


            End Select


        Next
        lblTutti.Text = CInt(lblAperti.Text) + CInt(lblInCarico.Text) + CInt(lblRisCliente.Text) + CInt(lblRisOperatore.Text) '+ CInt(lblChiuso.Text)
        'CategorieChart.Series.Add("Pass percentage")
        'CategorieChart.Titles.Add("Statistiche ")
        'If tab.Rows.Count > 0 Then
        '    Dim j As Int16 = 0
        '    For i As Integer = 0 To tab.Rows.Count - 1
        '        CategorieChart.Series(0).Points.AddXY(i + 10, i + 15)
        '        CategorieChart.Series(0).Points.Item(j).AxisLabel = tab.Rows(i)("cont")
        '        j = j + 1
        '    Next
        'End If

        'Dim SerieCategorie As System.Web.UI.DataVisualization.Charting.Series = CategorieChart.Series("CategorieSerie")
        'If lblAperti.Text <> "0" Then
        '    SerieCategorie.Points.AddXY("Aperti", lblAperti.Text)
        'End If
        'If lblInCarico.Text <> "0" Then
        '    SerieCategorie.Points.AddXY("In Carico", lblInCarico.Text)
        'End If
        'If lblRisOperatore.Text <> "0" Then
        '    SerieCategorie.Points.AddXY("Ris Operatore", lblRisOperatore.Text)
        'End If
        'If lblRisCliente.Text <> "0" Then
        '    SerieCategorie.Points.AddXY("Ris Cliente", lblRisCliente.Text)
        'End If
        'If lblChiuso.Text <> "0" Then
        '    SerieCategorie.Points.AddXY("Chiusi", lblChiuso.Text)
        'End If

    End Sub

    Private Sub CaricaTicket(Optional idstato As String = "-1", Optional tipo As String = "-1", Optional id As String = "-1")
        Dim sql As String = "select u.nome + ' ' +u.cognome as nomecognomeut,o.nome + ' ' +o.cognome as nomecognomeop,SubCliente.ragsoc as ragsocsub,* from Tickets " & _
                            "inner join Stato on Stato.id=Tickets.idStato " & _
                            "inner join Cliente on Cliente.id=Tickets.idcliente " & _
                            "left outer join Utente as u on u.id=Tickets.idutente " & _
                            "left outer join Utente as o on o.id=Tickets.idoperatore " & _
                            "left outer join SubCliente on SubCliente.id=Tickets.idSubCliente " & _
                            "left outer join Inventario on Inventario.id=Tickets.idinventario " & _
                            "left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
                            "where Tickets.id<>-1"
        'If ddlCliente.SelectedValue = "-1" Then
        '    If idstato = "5" Then
        '        sql = sql & " and Stato.id=5 "
        '    Else
        '        sql = sql & " and Stato.id<>5 "
        '    End If
        'End If

        If Session("aperto") Or Session("carico") Or Session("risoperatore") Or Session("riscliente") Or Session("chiuso") Then
            sql = sql & " and ("

            If Session("aperto") Then
                sql = sql & " idstato=1 or "
            End If

            If Session("carico") Then
                sql = sql & " idstato=2 or "
            End If

            If Session("risoperatore") Then
                sql = sql & " idstato=3 or "
            End If

            If Session("riscliente") Then
                sql = sql & " idstato=4 or "
            End If

            If Session("chiuso") Then
                sql = sql & " idstato=5 or "
            End If

            sql = sql.Remove(sql.LastIndexOf("or"), 2) & " )"
        Else
            sql = sql & "and idstato<>5"
        End If

        If id <> "-1" Then
            If tipo = "Cliente" Or Session("idcodicecliente").ToString.Split("-")(1).StartsWith("C") Then
                sql = sql & " and Cliente.id=" & id
            ElseIf tipo = "SubCliente" Or Session("idcodicecliente").ToString.Split("-")(1).StartsWith("SC") Then
                sql = sql & " and SubCliente.id=" & id
            End If
        ElseIf Session("idcodicecliente") <> "" And Session("idcodicecliente") <> Nothing And Not IsDBNull(Session("idcodicecliente")) Then
            If Session("idcodicecliente").ToString.Split("-")(1).StartsWith("C") Then
                sql = sql & " and Cliente.id=" & Session("idcodicecliente").ToString.Split("-")(0)
            ElseIf Session("idcodicecliente").ToString.Split("-")(1).StartsWith("SC") Then
                sql = sql & " and SubCliente.id=" & Session("idcodicecliente").ToString.Split("-")(0)
            End If
        End If
        If btnGiorno.BackColor = Color.YellowGreen Or Session("componente") = "giorno" Then
            sql = sql & " and dataapertura like '" & Today.Date & "'"
        ElseIf btnSettimana.BackColor = Color.YellowGreen Or Session("componente") = "settimana" Then
            sql = sql & " and dataapertura between '" & Today.Date.AddDays(-7).ToString("yyyy-MM-dd") & "' and '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        ElseIf btnMese.BackColor = Color.YellowGreen Or Session("componente") = "mese" Then
            sql = sql & " and dataapertura between '" & Today.Date.AddDays(-31).ToString("yyyy-MM-dd") & "' and '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        Else
            If txtDataDa.Text <> "" Then
                sql = sql & " and dataapertura>='" & txtDataDa.Text & "'"
            End If
            If txtDataA.Text <> "" Then
                sql = sql & " and dataapertura<='" & txtDataA.Text & "'"
            End If

        End If
        'If txtRicerca.Text <> "" Then
        '    sql = sql & " and (Cliente.ragsoc like '" & txtRicerca.Text & "' or SubCliente.ragsoc like '" & txtRicerca.Text & "' or Tickets.oggetto like '" & txtRicerca.Text & "' or TipoDispositivo.tipodispositivo like '" & txtRicerca.Text & "')"
        'End If
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            sql = sql & " and Cliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" And Session("isadmin") = 1 Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.SubCliente = New SubCliente
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.SubCliente.ID <> "-1" Then
                Dim s As String = "Select * from Utente where idsubcliente=" & MyUtente.SubCliente.ID
                Dim t As DataTable = Me.MyGest.GetTab(s)

                sql = sql & " and SubCliente.id=" & MyUtente.SubCliente.ID
                sql = sql & " or( Tickets.idutente=" & MyUtente.ID & "or Tickets.idpercontodi=" & MyUtente.ID & ")"
                For i As Integer = 0 To t.Rows.Count - 1
                    sql = sql & "or (Tickets.idpercontodi =" & t.Rows(i)("id") & "or Tickets.idpercontodi =" & t.Rows(i)("id") & ")"
                Next
            Else
                sql = sql & " and stealth=0 and Cliente.id=" & MyUtente.Cliente.ID
            End If
        ElseIf Session("tipoutente") = "Utente" And Session("isadmin") = 0 Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            sql = sql & " and stealth=0 and (Tickets.idutente=" & MyUtente.ID & "or Tickets.idpercontodi=" & MyUtente.ID & ") "
        End If
        sql = sql & "order by datascadenza"




        Dim tab As DataTable = Me.MyGest.GetTab(sql)


        'lblNTicket.Text = tab.Rows.Count






        Dim tstruct As New DataTable
        Dim c1 As New DataColumn("id", GetType(String), "")
        tstruct.Columns.Add(c1)
        Dim c2 As New DataColumn("stato", GetType(String), "")
        tstruct.Columns.Add(c2)
        Dim c3 As New DataColumn("dataapertura", GetType(String), "")
        tstruct.Columns.Add(c3)
        Dim c4 As New DataColumn("datachiusura", GetType(String), "")
        tstruct.Columns.Add(c4)
        Dim c5 As New DataColumn("datascadenza", GetType(String), "")
        tstruct.Columns.Add(c5)
        Dim c6 As New DataColumn("dataultimo", GetType(String), "")
        tstruct.Columns.Add(c6)
        Dim c7 As New DataColumn("ragsoc", GetType(String), "")
        tstruct.Columns.Add(c7)
        Dim c8 As New DataColumn("nomecognomeop", GetType(String), "")
        tstruct.Columns.Add(c8)
        Dim c9 As New DataColumn("ragsocsub", GetType(String), "")
        tstruct.Columns.Add(c9)
        Dim c10 As New DataColumn("nomecognomeut", GetType(String), "")
        tstruct.Columns.Add(c10)
        Dim c11 As New DataColumn("oggetto", GetType(String), "")
        tstruct.Columns.Add(c11)
        Dim c12 As New DataColumn("tipodispositivo", GetType(String), "")
        tstruct.Columns.Add(c12)
        Dim c13 As New DataColumn("colore", GetType(String), "")
        tstruct.Columns.Add(c13)
        For i As Integer = 0 To tab.Rows.Count - 1
            tstruct.Rows.Add(i)
            tstruct.Rows(i)("colore") = tab.Rows(i)("colore")
            tstruct.Rows(i)("id") = tab.Rows(i)("id")
            tstruct.Rows(i)("stato") = tab.Rows(i)("stato")
            tstruct.Rows(i)("dataapertura") = tab.Rows(i)("dataapertura").ToString.Split(":")(0) & ":" & tab.Rows(i)("dataapertura").ToString.Split(":")(1)
            If tab.Rows(i)("datachiusura") = "01/01/9999" Then
                tstruct.Rows(i)("datachiusura") = ""
            Else
                tstruct.Rows(i)("datachiusura") = tab.Rows(i)("datachiusura").ToString.Split(":")(0) & ":" & tab.Rows(i)("datachiusura").ToString.Split(":")(1)
            End If
            tstruct.Rows(i)("datascadenza") = tab.Rows(i)("datascadenza").ToString.Split(":")(0) & ":" & tab.Rows(i)("datascadenza").ToString.Split(":")(1)
            If tab.Rows(i)("dataultimo") = "01/01/9999" Then
                tstruct.Rows(i)("dataultimo") = ""
            Else
                tstruct.Rows(i)("dataultimo") = tab.Rows(i)("dataultimo").ToString.Split(":")(0) & ":" & tab.Rows(i)("dataultimo").ToString.Split(":")(1)
            End If

            tstruct.Rows(i)("ragsoc") = tab.Rows(i)("ragsoc")
            tstruct.Rows(i)("nomecognomeop") = tab.Rows(i)("nomecognomeop")

            tstruct.Rows(i)("ragsocsub") = tab.Rows(i)("ragsocsub")
            tstruct.Rows(i)("nomecognomeut") = tab.Rows(i)("nomecognomeut")
            tstruct.Rows(i)("oggetto") = tab.Rows(i)("oggetto")
            tstruct.Rows(i)("tipodispositivo") = tab.Rows(i)("tipodispositivo")


        Next
        ListView1.DataSource = tstruct
        ListView1.DataBind()


        'ListView4.DataSource = tstruct
        'ListView4.DataBind()
        If tstruct.Rows.Count > 0 Then
            Panel2.Visible = False
        Else
            Panel2.Visible = True
        End If
    End Sub

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As Button = e.Item.FindControl("btnGestisci")
        AddHandler btn.Click, AddressOf GestisciTicket

        'Dim btn1 As Button = e.Item.FindControl("btnElimina")
        'AddHandler btn1.Click, AddressOf EliminaTicket
    End Sub

    Private Sub EliminaTicket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As Button = CType(sender, Button)
        Dim message As String = bt.ToolTip
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopupCancel('" + message + "');", True)

    End Sub

    Private Sub GestisciTicket(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As Button = CType(sender, Button)
        lblIdTicket.Text = bt.ToolTip
        Me.ModificaTicket()

    End Sub

    Private Sub ModificaTicket()
        Response.Redirect("gestticket.aspx?id=" & lblIdTicket.Text)
    End Sub

    'Private Sub CreaMenu()

    '    Dim mydt As New DataTable
    '    Me.MyAzienda = New Azienda
    '    Select Case Session("tipoutente")
    '        Case "Operatore"
    '            Me.MyUtente = New Utente
    '            Me.MyUtente.Azienda = New Azienda
    '            Me.MyUtente.Cliente = New Cliente
    '            Me.MyUtente.Load(Session("id"))
    '            mydt = Me.MyGest.GetTab("SELECT distinct (ragsoc),Cliente.id FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id inner join Stato on Stato.id=Tickets.idstato where idstato<>5 and Cliente.idazienda=" & Me.MyUtente.Azienda.ID)
    '        Case "SuperAdmin"
    '            mydt = Me.MyGest.GetTab("SELECT distinct (ragsoc),Cliente.id FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id")
    '        Case "Utente"
    '            mydt = Me.MyGest.GetTab("SELECT distinct (ragsoc),Cliente.id FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id inner join Stato on Stato.id=Tickets.idstato where idstato<>5 and Cliente.id=" & Me.MyUtente.Cliente.ID)
    '    End Select

    '    Dim nodo As TreeNode
    '    Dim r As Integer
    '    Do While r <= mydt.Rows.Count - 1
    '        If mydt.Rows(r)("ragsoc").ToString.Length > 20 Then
    '            nodo = New TreeNode(mydt.Rows(r)("ragsoc").ToString.Substring(0, 20) & "...", "Cliente/" & mydt.Rows(r)("id"))
    '        Else
    '            nodo = New TreeNode(mydt.Rows(r)("ragsoc"), "Cliente/" & mydt.Rows(r)("id"))
    '        End If
    '        Me.TreeView1.Nodes.Add(nodo)

    '        Me.CreaSottoMenu(mydt.Rows(r)("id"), Me.TreeView1.Nodes(r))

    '        r = r + 1
    '    Loop
    '    mydt.Dispose()

    'End Sub

    'Private Sub CreaSottoMenu(ByVal idm As Integer, ByVal nodes As TreeNode)
    '    Dim mydt As New DataTable
    '    mydt = Me.MyGest.GetTab("SELECT distinct(ragsoc),SubCliente.id FROM SubCliente inner join Tickets on Tickets.idsubcliente=SubCliente.id WHERE SubCliente.IDCliente=" & idm & "group by ragsoc,SubCliente.id")

    '    For Each r As DataRow In mydt.Rows
    '        Dim valore As Boolean
    '        'If Session("id") <> Nothing Then
    '        '    'valore = Me.Abilita(idm)
    '        'Else
    '        valore = True
    '        'End If

    '        If valore = True Then
    '            Dim nodo As TreeNode
    '            If r("ragsoc").ToString.Length > 20 Then
    '                nodo = New TreeNode(r("ragsoc").ToString.Substring(0, 20) & "...", "SubCliente/" & r("id"))
    '            Else
    '                nodo = New TreeNode(r("ragsoc"), "SubCliente/" & r("id"))
    '            End If
    '            nodo.ToolTip = r("ragsoc")
    '            nodes.ChildNodes.Add(nodo)

    '        End If


    '    Next
    '    mydt.Dispose()

    'End Sub

    'Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged
    '    ddlCliente.SelectedValue = "-1"
    '    txtDataA.Text = ""
    '    txtDataDa.Text = ""
    '    Dim val = Me.TreeView1.SelectedNode.Value.Split("/")
    '    If IsDBNull(ViewState("stato")) Or ViewState("stato") = Nothing Then
    '        Me.FiltraTickets("-1", val(0), val(1))
    '    Else
    '        Me.FiltraTickets(ViewState("stato"), val(0), val(1))
    '    End If

    '    'Response.Redirect("/ticket.aspx/?id=" & val(1))
    'End Sub

    'Protected Sub btnApri_Click(sender As Object, e As EventArgs) Handles btnApri.Click

    '    If Session("tipoutente") = "Utente" Then
    '        Me.MyCGlobal = New CGlobal
    '        Me.MyUtente = New Utente
    '        Me.MyUtente.Load(Session("id"))
    '        If Me.MyUtente.SubCliente.ID <> "-1" Then
    '            If Me.MyCGlobal.VerificaBloccoCliente(MyUtente.SubCliente.ID, "SubCliente") Then
    '                Response.Redirect("apriticket.aspx")
    '            Else
    '                Dim message As String = "SubCliente bloccato. Contatti la sua organizzazione per ulteriori informmazioni"
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
    '            End If
    '        Else
    '            If Me.MyCGlobal.VerificaBloccoCliente(MyUtente.Cliente.ID, "Cliente") Then
    '                Response.Redirect("apriticket.aspx")
    '            Else
    '                Dim message As String = "Cliente bloccato. Contatti la sua organizzazione per ulteriori informmazioni"
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
    '            End If
    '        End If
    '    Else
    '        Response.Redirect("apriticket.aspx")
    '    End If

    'End Sub

    Protected Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        If Session("aperto") Then
            Label1.BackColor = Drawing.Color.White
            'Label1.BackColor = Color.White
            'Label1.ForeColor = Color.Red
            Session("aperto") = False
        Else
            Label1.BackColor = Drawing.Color.Yellow
            'Label1.ForeColor = Color.White
            'Label1.BackColor = Color.Red

            Session("aperto") = True
        End If

        Label6.BackColor = Drawing.Color.White
        Me.FiltraTickets() '"1")
        'Label1.BackColor = Drawing.Color.Yellow
        'Label2.BackColor = Drawing.Color.White
        'Label3.BackColor = Drawing.Color.White
        'Label4.BackColor = Drawing.Color.White
        'Label5.BackColor = Drawing.Color.White
        'Label6.BackColor = Drawing.Color.White
    End Sub

    Private Sub FiltraTickets(Optional ByVal stato As String = "-1", Optional ByVal tipo As String = "-1", Optional id As String = "-1")
        Session("stato") = stato

        Me.CaricaTicket(stato, tipo, id)
        ContaTickets(stato, tipo, id)
        Me.CaricaClientiListView2()
    End Sub

    'Private Function RestituisciIdStato(ByVal stato As String)
    '    Dim sql As String = "select * from Stato where stato='" & stato & "'"
    '    Dim tab As DataTable = Me.MyGest.GetTab(sql)
    '    Dim res = "-1"
    '    If tab.Rows.Count > 0 Then
    '        res = tab.Rows(0)("idstato")
    '    End If
    '    Return res
    'End Function

    Protected Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        If Session("carico") Then
            Label2.BackColor = Drawing.Color.White
            Session("carico") = False
        Else
            Label2.BackColor = Drawing.Color.Yellow
            Session("carico") = True
        End If

        Label6.BackColor = Drawing.Color.White
        Me.FiltraTickets() '"2")
        'Label2.BackColor = Drawing.Color.Yellow
        'Label1.BackColor = Drawing.Color.White
        'Label3.BackColor = Drawing.Color.White
        'Label4.BackColor = Drawing.Color.White
        'Label5.BackColor = Drawing.Color.White
        'Label6.BackColor = Drawing.Color.White
    End Sub

    Protected Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        If Session("riscliente") Then
            Label3.BackColor = Drawing.Color.White
            Session("riscliente") = False
        Else
            Label3.BackColor = Drawing.Color.Yellow
            Session("riscliente") = True
        End If

        Label6.BackColor = Drawing.Color.White
        Me.FiltraTickets() '"4")
        'Label3.BackColor = Drawing.Color.Yellow
        'Label2.BackColor = Drawing.Color.White
        'Label4.BackColor = Drawing.Color.White
        'Label1.BackColor = Drawing.Color.White
        'Label5.BackColor = Drawing.Color.White
        'Label6.BackColor = Drawing.Color.White
    End Sub

    Protected Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        If Session("risoperatore") Then
            Label4.BackColor = Drawing.Color.White
            Session("risoperatore") = False
        Else
            Label4.BackColor = Drawing.Color.Yellow
            Session("risoperatore") = True
        End If

        Label6.BackColor = Drawing.Color.White
        Me.FiltraTickets() '"3")
        'Label4.BackColor = Drawing.Color.Yellow
        'Label2.BackColor = Drawing.Color.White
        'Label1.BackColor = Drawing.Color.White
        'Label3.BackColor = Drawing.Color.White
        'Label5.BackColor = Drawing.Color.White
        'Label6.BackColor = Drawing.Color.White
    End Sub

    Protected Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        If Session("chiuso") Then
            Label5.BackColor = Drawing.Color.White
            Session("chiuso") = False
        Else
            Label5.BackColor = Drawing.Color.Yellow
            Session("chiuso") = True
        End If
        Label6.BackColor = Drawing.Color.White
        Me.FiltraTickets() '"5")
        'Label5.BackColor = Drawing.Color.Yellow
        'Label2.BackColor = Drawing.Color.White
        'Label3.BackColor = Drawing.Color.White
        'Label4.BackColor = Drawing.Color.White
        'Label1.BackColor = Drawing.Color.White
        'Label6.BackColor = Drawing.Color.White
    End Sub

    Protected Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Session("stato") = "-1"
        Session("chiuso") = False
        Session("risoperatore") = False
        Session("riscliente") = False
        Session("carico") = False
        Session("aperto") = False
        Session("idcodicecliente") = ""
        Session("nomecliente") = ""
        lblClienteFil.Text = ""
        Me.CaricaTicket()
        ContaTickets()
        Me.CaricaClientiListView2()
        Label5.BackColor = Drawing.Color.White
        Label2.BackColor = Drawing.Color.White
        Label3.BackColor = Drawing.Color.White
        Label4.BackColor = Drawing.Color.White
        Label1.BackColor = Drawing.Color.White
        Label6.BackColor = Drawing.Color.Yellow

    End Sub

    'Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
    '    If Session("stato") <> "" And Session("stato") <> Nothing And IsDBNull(Session("stato")) Then
    '        Me.FiltraTickets(Session("stato"), "Cliente", -1) 'ddlCliente.SelectedValue)
    '    Else
    '        Me.FiltraTickets(, "Cliente", -1) 'ddlCliente.SelectedValue)
    '    End If
    'End Sub

    Protected Sub txtDataDa_TextChanged(sender As Object, e As EventArgs) Handles txtDataDa.TextChanged
        Session("componente") = ""
        btnSettimana.BackColor = Color.Coral
        btnMese.BackColor = Color.Coral
        btnGiorno.BackColor = Color.Coral
        If Session("stato") <> "" And Session("stato") <> Nothing And IsDBNull(Session("stato")) Then
            Me.FiltraTickets(Session("stato"), , -1) 'ddlCliente.SelectedValue)
        Else
            Me.FiltraTickets(, "Cliente", -1) 'ddlCliente.SelectedValue)
        End If
    End Sub

    Protected Sub txtDataA_TextChanged(sender As Object, e As EventArgs) Handles txtDataA.TextChanged
        Session("componente") = ""
        btnSettimana.BackColor = Color.Coral
        btnMese.BackColor = Color.Coral
        btnGiorno.BackColor = Color.Coral
        If Session("stato") <> "" And Session("stato") <> Nothing And IsDBNull(Session("stato")) Then
            Me.FiltraTickets(Session("stato"), , -1) 'ddlCliente.SelectedValue)
        Else
            Me.FiltraTickets(, "Cliente", -1) 'ddlCliente.SelectedValue)
        End If
    End Sub


    Private Sub CaricaClientiListView2()
        Dim sql As String
        Me.MyAzienda = New Azienda
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Cliente = New Cliente
        Me.MyUtente.Load(Session("id"))
        Select Case Session("tipoutente")
            
            Case "Operatore"
                sql = "SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,Cliente.id,Cliente.codice FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id inner join Stato on Stato.id=Tickets.idstato where Tickets.idsubcliente=-1 and Cliente.idazienda=" & Me.MyUtente.Azienda.ID
            Case "SuperAdmin"
                sql = "SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,Cliente.id,Cliente.codice FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id where Tickets.idsubcliente=-1"
            Case "Utente"
                'Me.MyUtente.Cliente.Load(Session("id"))
                sql = "SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,Cliente.id,Cliente.codice FROM Cliente inner join Tickets on Tickets.idcliente=Cliente.id inner join Stato on Stato.id=Tickets.idstato where Tickets.idsubcliente=-1 and Cliente.id=" & Me.MyUtente.Cliente.ID

        End Select

        If Session("aperto") Or Session("carico") Or Session("risoperatore") Or Session("riscliente") Or Session("chiuso") Then
            sql = sql & " and ("

            If Session("aperto") Then
                sql = sql & " idstato=1 or "
            End If

            If Session("carico") Then
                sql = sql & " idstato=2 or "
            End If

            If Session("risoperatore") Then
                sql = sql & " idstato=3 or "
            End If

            If Session("riscliente") Then
                sql = sql & " idstato=4 or "
            End If

            If Session("chiuso") Then
                sql = sql & " idstato=5 or "
            End If


            sql = sql.Remove(sql.LastIndexOf("or"), 2) & " )"
        Else
            sql = sql & "and idstato<>5"
        End If

        Select Case Session("tipoutente")
            Case "Operatore"
                Me.MyUtente = New Utente
                Me.MyUtente.Azienda = New Azienda
                Me.MyUtente.Cliente = New Cliente
                Me.MyUtente.Load(Session("id"))
                sql = sql & " union SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,SubCliente.id,SubCliente.codice FROM SubCliente inner join Tickets on Tickets.idsubcliente=SubCliente.id inner join Stato on Stato.id=Tickets.idstato where Tickets.idcliente<>-1 and SubCliente.idazienda=" & Me.MyUtente.Azienda.ID
            Case "SuperAdmin"
                sql = sql & " union SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,SubCliente.id,SubCliente.codice FROM SubCliente inner join Tickets on Tickets.idsubcliente=SubCliente.id where Tickets.idcliente<>-1"

            Case "Utente"
                sql = sql & " union SELECT distinct (SUBSTRING(ragsoc, 1, 20)) as ragsoc1,ragsoc,SubCliente.id,SubCliente.codice FROM SubCliente inner join Tickets on Tickets.idsubcliente=SubCliente.id inner join Stato on Stato.id=Tickets.idstato where Tickets.idcliente<>-1 and SubCliente.id=" & Me.MyUtente.SubCliente.ID

        End Select

        If Session("aperto") Or Session("carico") Or Session("risoperatore") Or Session("riscliente") Or Session("chiuso") Then
            sql = sql & " and ("

            If Session("aperto") Then
                sql = sql & " idstato=1 or "
            End If

            If Session("carico") Then
                sql = sql & " idstato=2 or "
            End If

            If Session("risoperatore") Then
                sql = sql & " idstato=3 or "
            End If

            If Session("riscliente") Then
                sql = sql & " idstato=4 or "
            End If

            If Session("chiuso") Then
                sql = sql & " idstato=5 or "
            End If


            sql = sql.Remove(sql.LastIndexOf("or"), 2) & " )"
        Else
            sql = sql & "and idstato<>5"
        End If

        Dim tab As DataTable = Me.MyGest.GetTab(sql)


        'Dim tstruct As New DataTable
        'Dim c1i As New DataColumn("id1", GetType(String), "")
        'tstruct.Columns.Add(c1i)
        'Dim c1r As New DataColumn("ragsoc1", GetType(String), "")
        'tstruct.Columns.Add(c1r)
        'Dim c2i As New DataColumn("id2", GetType(String), "")
        'tstruct.Columns.Add(c2i)
        'Dim c2r As New DataColumn("ragsoc2", GetType(String), "")
        'tstruct.Columns.Add(c2r)
        'Dim c3i As New DataColumn("id3", GetType(String), "")
        'tstruct.Columns.Add(c3i)
        'Dim c3r As New DataColumn("ragsoc3", GetType(String), "")
        'tstruct.Columns.Add(c3r)
        'Dim c4i As New DataColumn("id4", GetType(String), "")
        'tstruct.Columns.Add(c4i)
        'Dim c4r As New DataColumn("ragsoc4", GetType(String), "")
        'tstruct.Columns.Add(c4r)
        'Dim c5i As New DataColumn("id5", GetType(String), "")
        'tstruct.Columns.Add(c5i)
        'Dim c5r As New DataColumn("ragsoc5", GetType(String), "")
        'tstruct.Columns.Add(c5r)
        'Dim c6i As New DataColumn("id6", GetType(String), "")
        'tstruct.Columns.Add(c6i)
        'Dim c6r As New DataColumn("ragsoc6", GetType(String), "")
        'tstruct.Columns.Add(c6r)
        'Dim c7i As New DataColumn("id7", GetType(String), "")
        'tstruct.Columns.Add(c7i)
        'Dim c7r As New DataColumn("ragsoc7", GetType(String), "")
        'tstruct.Columns.Add(c7r)
        'Dim c8i As New DataColumn("id8", GetType(String), "")
        'tstruct.Columns.Add(c8i)
        'Dim c8r As New DataColumn("ragsoc8", GetType(String), "")
        'tstruct.Columns.Add(c8r)
        'Dim j = 0
        'For i As Integer = 0 To tab.Rows.Count - 1 Step 8
        '    Try

        '        tstruct.Rows.Add(j)
        '        tstruct.Rows(j)("id1") = tab.Rows(i)("id")
        '        tstruct.Rows(j)("ragsoc1") = tab.Rows(i)("ragsoc")
        '        ViewState("ragsoc1") = tstruct.Rows(j)("ragsoc1")
        '        'tstruct = CaricaSubClientiListView2(tab.Rows(i)("id"), tstruct)
        '        tstruct.Rows(j)("id2") = tab.Rows(i + 1)("id")
        '        tstruct.Rows(j)("ragsoc2") = tab.Rows(i + 1)("ragsoc")
        '        ViewState("ragsoc2") = tstruct.Rows(j)("ragsoc2")
        '        'tstruct = CaricaSubClientiListView2(tab.Rows(i + 1)("id"), tstruct)
        '        tstruct.Rows(j)("id3") = tab.Rows(i + 2)("id")
        '        tstruct.Rows(j)("ragsoc3") = tab.Rows(i + 2)("ragsoc")
        '        ViewState("ragsoc3") = tstruct.Rows(j)("ragsoc3")
        '        'tstruct = CaricaSubClientiListView2(tab.Rows(i + 2)("id"), tstruct)
        '        tstruct.Rows(j)("id4") = tab.Rows(i + 3)("id")
        '        tstruct.Rows(j)("ragsoc4") = tab.Rows(i + 3)("ragsoc")
        '        ViewState("ragsoc4") = tstruct.Rows(j)("ragsoc4")
        '        'tstruct = CaricaSubClientiListView2(tab.Rows(i + 3)("id"), tstruct)
        '        tstruct.Rows(j)("id5") = tab.Rows(i + 4)("id")
        '        tstruct.Rows(j)("ragsoc5") = tab.Rows(i + 4)("ragsoc")
        '        ViewState("ragsoc5") = tstruct.Rows(j)("ragsoc5")
        '        'tstruct = CaricaSubClientiListView2(tab.Rows(i + 4)("id"), tstruct)
        '        tstruct.Rows(j)("id6") = tab.Rows(i + 5)("id")
        '        tstruct.Rows(j)("ragsoc6") = tab.Rows(i + 5)("ragsoc")
        '        ViewState("ragsoc6") = tstruct.Rows(j)("ragsoc6")
        '        tstruct.Rows(j)("id7") = tab.Rows(i + 6)("id")
        '        tstruct.Rows(j)("ragsoc7") = tab.Rows(i + 6)("ragsoc")
        '        ViewState("ragsoc7") = tstruct.Rows(j)("ragsoc7")
        '        tstruct.Rows(j)("id8") = tab.Rows(i + 7)("id")
        '        tstruct.Rows(j)("ragsoc8") = tab.Rows(i + 7)("ragsoc")
        '        ViewState("ragsoc8") = tstruct.Rows(j)("ragsoc8")
        '        j = j + 1
        '    Catch
        '    End Try
        'Next

        ListView2.DataSource = tab
        ListView2.DataBind()




        'ListView2.DataSource = tab
        'ListView2.DataBind()
        'Dim nodo As TreeNode
        'Dim r As Integer
        'Do While r <= mydt.Rows.Count - 1
        '    If mydt.Rows(r)("ragsoc").ToString.Length > 20 Then
        '        nodo = New TreeNode(mydt.Rows(r)("ragsoc").ToString.Substring(0, 20) & "...", "Cliente/" & mydt.Rows(r)("id"))
        '    Else
        '        nodo = New TreeNode(mydt.Rows(r)("ragsoc"), "Cliente/" & mydt.Rows(r)("id"))
        '    End If
        '    Me.TreeView1.Nodes.Add(nodo)

        '    Me.CreaSottoMenu(mydt.Rows(r)("id"), Me.TreeView1.Nodes(r))

        '    r = r + 1
        'Loop
        tab.Dispose()
    End Sub

    'Private Function CaricaSubClientiListView2(ByVal idm, ByVal tstruct)
    '    Dim mydt As New DataTable
    '    mydt = Me.MyGest.GetTab("SELECT distinct(ragsoc),SubCliente.id FROM SubCliente inner join Tickets on Tickets.idsubcliente=SubCliente.id WHERE SubCliente.IDCliente=" & idm & "group by ragsoc,SubCliente.id")


    '    Dim c1i As New DataColumn("idsub11", GetType(String), "")
    '    tstruct.Columns.Add(c1i)
    '    Dim c1r As New DataColumn("ragsocsub11", GetType(String), "")
    '    tstruct.Columns.Add(c1r)
    '    Dim c2i As New DataColumn("idsub12", GetType(String), "")
    '    tstruct.Columns.Add(c2i)
    '    Dim c2r As New DataColumn("ragsocsub12", GetType(String), "")
    '    tstruct.Columns.Add(c2r)
    '    Dim c3i As New DataColumn("idsub13", GetType(String), "")
    '    tstruct.Columns.Add(c3i)
    '    Dim c3r As New DataColumn("ragsocsub13", GetType(String), "")
    '    tstruct.Columns.Add(c3r)
    '    Dim c4i As New DataColumn("idsub14", GetType(String), "")
    '    tstruct.Columns.Add(c4i)
    '    Dim c4r As New DataColumn("ragsocsub14", GetType(String), "")
    '    tstruct.Columns.Add(c4r)
    '    Dim c5i As New DataColumn("idsub15", GetType(String), "")
    '    tstruct.Columns.Add(c5i)
    '    Dim c5r As New DataColumn("ragsocsub15", GetType(String), "")
    '    tstruct.Columns.Add(c5r)
    '    Dim c6i As New DataColumn("idsub16", GetType(String), "")
    '    tstruct.Columns.Add(c6i)
    '    Dim c6r As New DataColumn("ragsocsub16", GetType(String), "")
    '    tstruct.Columns.Add(c6r)
    '    If mydt.Rows.Count > 0 Then
    '        For i As Integer = 0 To 5 Step 6
    '            Try
    '                tstruct.Rows.Add(i)
    '                tstruct.Rows(i)("idsub11") = mydt.Rows(i)("id")
    '                tstruct.Rows(i)("ragsocsub11") = mydt.Rows(i)("ragsoc")
    '                tstruct.Rows(i)("idsub12") = mydt.Rows(i + 1)("id")
    '                tstruct.Rows(i)("ragsocsub12") = mydt.Rows(i + 1)("ragsoc")
    '                tstruct.Rows(i)("idsub13") = mydt.Rows(i + 2)("id")
    '                tstruct.Rows(i)("ragsocsub13") = mydt.Rows(i + 2)("ragsoc")
    '                tstruct.Rows(i)("idsub14") = mydt.Rows(i + 3)("id")
    '                tstruct.Rows(i)("ragsocsu14") = mydt.Rows(i + 3)("ragsoc")
    '                tstruct.Rows(i)("idsub15") = mydt.Rows(i + 4)("id")
    '                tstruct.Rows(i)("ragsocsub15") = mydt.Rows(i + 4)("ragsoc")
    '                tstruct.Rows(i)("idsub16") = mydt.Rows(i + 5)("id")
    '                tstruct.Rows(i)("ragsocsub16") = mydt.Rows(i + 5)("ragsoc")
    '            Catch
    '            End Try
    '        Next
    '    End If
    '    Return tstruct
    '    'ListView2.DataSource = tstruct
    '    'ListView2.DataBind()
    '    mydt.Dispose()
    'End Function

    Private Sub ListView2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView2.ItemCreated
        'Dim btn1 As Button = e.Item.FindControl("Button1")
        'AddHandler btn1.Click, AddressOf Gestione

        'Dim btn2 As Button = e.Item.FindControl("Button2")
        'AddHandler btn2.Click, AddressOf Gestione

        Dim btn3 As Button = e.Item.FindControl("Button3")

        AddHandler btn3.Click, AddressOf Gestione

        'Dim btn4 As Button = e.Item.FindControl("Button4")
        'AddHandler btn4.Click, AddressOf Gestione

        'Dim btn5 As Button = e.Item.FindControl("Button5")
        'AddHandler btn5.Click, AddressOf Gestione

        'Dim btn6 As Button = e.Item.FindControl("Button6")
        'AddHandler btn6.Click, AddressOf Gestione

        'Dim btn7 As Button = e.Item.FindControl("Button7")
        'AddHandler btn7.Click, AddressOf Gestione

        'Dim btn8 As Button = e.Item.FindControl("Button8")
        'AddHandler btn8.Click, AddressOf Gestione

    End Sub




    Private Sub Gestione(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim bt As Button = CType(sender, Button)
        lblClienteFil.Text = bt.Text
        Session("nomecliente") = lblClienteFil.Text
        Session("idcodicecliente") = bt.ToolTip
        Dim tipo
        If bt.ToolTip.Split("-")(1).StartsWith("C") Then
            tipo = "Cliente"
        Else
            tipo = "SubCliente"
        End If
        'bt.BackColor = Color.YellowGreen
        If IsDBNull(Session("stato")) Or Session("stato") = Nothing Then
            Me.FiltraTickets("-1", tipo, bt.ToolTip.Split("-")(0))
        Else
            Me.FiltraTickets(Session("stato"), tipo, bt.ToolTip.Split("-")(0))
        End If

    End Sub



    Protected Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        Dim b = ListView2.Items.Item(0)
        Dim a = ListView2.SelectedIndex
        MsgBox(a)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        MsgBox("Prova")
    End Sub



    Protected Sub btnGiorno_Click(sender As Object, e As EventArgs) Handles btnGiorno.Click
        If btnGiorno.BackColor = Color.YellowGreen Then
            btnSettimana.BackColor = Color.Coral
            btnMese.BackColor = Color.Coral
            btnGiorno.BackColor = Color.Coral
            Session("componente") = ""
        Else
            btnSettimana.BackColor = Color.Coral
            btnMese.BackColor = Color.Coral
            btnGiorno.BackColor = Color.YellowGreen
            txtDataA.Text = ""
            txtDataDa.Text = ""
            Session("componente") = "giorno"
        End If
        If IsDBNull(Session("stato")) Or Session("stato") = Nothing Then
            Me.FiltraTickets("-1", "Cliente")
        Else
            Me.FiltraTickets(Session("stato"), "Cliente")
        End If
    End Sub

    Protected Sub btnSettimana_Click(sender As Object, e As EventArgs) Handles btnSettimana.Click
        If btnSettimana.BackColor = Color.YellowGreen Then
            btnSettimana.BackColor = Color.Coral
            btnMese.BackColor = Color.Coral
            btnGiorno.BackColor = Color.Coral
            Session("componente") = ""
        Else
            btnSettimana.BackColor = Color.YellowGreen
            btnMese.BackColor = Color.Coral
            btnGiorno.BackColor = Color.Coral
            txtDataA.Text = ""
            txtDataDa.Text = ""
            Session("componente") = "settimana"
        End If
        If IsDBNull(Session("stato")) Or Session("stato") = Nothing Then
            Me.FiltraTickets("-1", "Cliente")
        Else
            Me.FiltraTickets(Session("stato"), "Cliente")
        End If
    End Sub

    Protected Sub btnMese_Click(sender As Object, e As EventArgs) Handles btnMese.Click
        If btnMese.BackColor = Color.YellowGreen Then
            btnSettimana.BackColor = Color.Coral
            btnMese.BackColor = Color.Coral
            btnGiorno.BackColor = Color.Coral
            Session("componente") = ""
        Else
            btnSettimana.BackColor = Color.Coral
            btnMese.BackColor = Color.YellowGreen
            btnGiorno.BackColor = Color.Coral
            txtDataA.Text = ""
            txtDataDa.Text = ""
            Session("componente") = "mese"
        End If
        If IsDBNull(Session("stato")) Or Session("stato") = Nothing Then
            Me.FiltraTickets("-1", "Cliente")
        Else
            Me.FiltraTickets(Session("stato"), "Cliente")
        End If
    End Sub

    'Private Sub btnRicerca_Click(sender As Object, e As System.EventArgs) Handles btnRicerca.Click
    '    If IsDBNull(ViewState("stato")) Or ViewState("stato") = Nothing Then
    '        Me.FiltraTickets("-1", "Cliente")
    '    Else
    '        Me.FiltraTickets(ViewState("stato"), "Cliente")
    '    End If
    'End Sub

    Private Sub lbExcel_Click(sender As Object, e As System.EventArgs) Handles lbExcel.Click


        'Dim xlWorkBook As Excel.Workbook
        'Dim xlWorkSheet As Excel.Worksheet

        'xlWorkBook = New Excel.Application().Workbooks.Add(Missing.Value)
        'xlWorkBook.Application.Visible = True
        'xlWorkSheet = xlWorkBook.ActiveSheet

        ''riempiamo un dataset con i dati del file Excel
        '' Dim dsData As DataSet = getData()
        'Dim j As Integer = 5


        'xlWorkSheet.Cells(1, 1) = "Data"
        'xlWorkSheet.Cells(2, 1) = "Cliente"
        'xlWorkSheet.Cells(3, 1) = "Stato"

        ''costruiamo la riga di intestazione
        'xlWorkSheet.Cells(4, 1) = "Nr.Ticket"
        'xlWorkSheet.Cells(4, 2) = "Stato"
        'xlWorkSheet.Cells(4, 3) = "Data Apertura"
        'xlWorkSheet.Cells(4, 4) = "Data Chiusura"
        'xlWorkSheet.Cells(4, 5) = "Data Scadenza"
        'xlWorkSheet.Cells(4, 6) = "Data Ultimo Stato"
        'xlWorkSheet.Cells(4, 7) = "Cliente"
        'xlWorkSheet.Cells(4, 8) = "Operatore"
        'xlWorkSheet.Cells(4, 9) = "SubCliente"
        'xlWorkSheet.Cells(4, 10) = "Utente"
        'xlWorkSheet.Cells(4, 11) = "Oggetto"
        'xlWorkSheet.Cells(4, 12) = "Inventario"


        ''mettiamo l'intestazione in grassetto
        'xlWorkSheet.Range("$A4:$N4").Font.ColorIndex = Excel.Constants.xlColor3
        'xlWorkSheet.Range("$A4:$N4").Font.Bold = True

        'Dim sql As String
        'Dim tab As DataTable

        'Dim b As Button
        'For Each lv As ListViewItem In ListView1.Items
        '    b = lv.FindControl("btnGestisci")
        '    sql = "select u.nome + ' ' +u.cognome as nomecognomeut,o.nome + ' ' +o.cognome as nomecognomeop,SubCliente.ragsoc as ragsocsub,Cliente.ragsoc as ragsoccli,Tickets.id as idt,Inventario.descrizione as descr,* from Tickets " & _
        '                    "inner join Stato on Stato.id=Tickets.idStato " & _
        '                    "inner join Cliente on Cliente.id=Tickets.idcliente " & _
        '                    "left outer join Utente as u on u.id=Tickets.idutente " & _
        '                    "left outer join Utente as o on o.id=Tickets.idoperatore " & _
        '                    "left outer join SubCliente on SubCliente.id=Tickets.idSubCliente " & _
        '                    "left outer join Inventario on Inventario.id=Tickets.idinventario " & _
        '                    "left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
        '                    "where Tickets.id=" & b.Text
        '    tab = Me.MyGest.GetTab(sql)

        '    For i As Integer = 0 To tab.Rows.Count - 1
        '        xlWorkSheet.Cells(j, 1) = tab.Rows(i)("idt")
        '        xlWorkSheet.Cells(j, 2) = tab.Rows(i)("stato")
        '        xlWorkSheet.Cells(j, 3) = tab.Rows(i)("dataapertura")
        '        xlWorkSheet.Cells(j, 4) = tab.Rows(i)("datachiusura")
        '        xlWorkSheet.Cells(j, 5) = tab.Rows(i)("datascadenza")
        '        xlWorkSheet.Cells(j, 6) = tab.Rows(i)("dataultimo")
        '        xlWorkSheet.Cells(j, 7) = tab.Rows(i)("ragsoccli")
        '        xlWorkSheet.Cells(j, 8) = tab.Rows(i)("nomecognomeop")
        '        xlWorkSheet.Cells(j, 9) = tab.Rows(i)("ragsocsub")
        '        xlWorkSheet.Cells(j, 10) = tab.Rows(i)("nomecognomeut")
        '        xlWorkSheet.Cells(j, 11) = tab.Rows(i)("oggetto")
        '        xlWorkSheet.Cells(j, 12) = tab.Rows(i)("descr")

        '        '   creiamo una formula per effettuare la somma dei vari punteggi
        '        'xlWorkSheet.Cells(i, 7).Formula = "=SOMMA($C{0}:$F{0})".Replace("{0}",
        '        '                                                                i.ToString())

        '        j = j + 1
        '    Next

        '    '   diamo la giusta larghezza alle colonne
        '    xlWorkSheet.Columns.AutoFit()
        'Next




        'Dim intestazioni = "Data;" & vbCrLf & "Cliente;" & vbCrLf & "Stato;" & vbCrLf
        'Dim intestazioni2 = "Nr.Ticket;Stato;Data Apertura;Data Chiusura;Data Scadenza;Data Ultimo Stato;Cliente;Operatore;SubCliente;Utente;Oggetto;Inventario;"
        Dim tab As DataTable
        Dim tab2 As DataTable
        Dim strSQL As String
        Dim b As Button
        Dim stato As String = ""
        Dim nomefile As String = "report\excel\Report_" & Date.Today.ToString("dd/MM/yyyy").Replace("/", "") & ".xls"
        'Crea Table file Excel
        'Dim strExcelConn As String = System.Configuration.ConfigurationManager.AppSettings.Item("ExcelConnection2")
        Using sw As StreamWriter = File.CreateText(Server.MapPath(nomefile))
            sw.WriteLine("Filtri Attivi:" & vbTab)
            sw.WriteLine("Periodo" & vbTab & txtDataDa.Text & " - " & txtDataA.Text)
            If Session("aperto") Then
                stato = "aperto"
            End If
            If Session("carico") Then
                stato = "carico"
            End If
            If Session("risoperatore") Then
                stato = "risoperatore"
            End If
            If Session("riscliente") Then
                stato = "riscliente"
            End If
            If Session("chiuso") Then
                stato = "chiuso"
            End If
            sw.WriteLine("Stato" & vbTab & stato & vbCrLf)
            For Each lv As ListViewItem In ListView1.Items
                b = lv.FindControl("btnGestisci")
                strSQL = "select u.nome + ' ' +u.cognome as nomecognomeut,o.nome + ' ' +o.cognome as nomecognomeop,SubCliente.ragsoc as ragsocsub,Cliente.ragsoc as ragsoccli,Tickets.id as idt,Tickets.descrizione as descr,* from Tickets " & _
                                "inner join Stato on Stato.id=Tickets.idStato " & _
                                "inner join Cliente on Cliente.id=Tickets.idcliente " & _
                                "left outer join Utente as u on u.id=Tickets.idutente " & _
                                "left outer join Utente as o on o.id=Tickets.idoperatore " & _
                                "left outer join SubCliente on SubCliente.id=Tickets.idSubCliente " & _
                                "left outer join Inventario on Inventario.id=Tickets.idinventario " & _
                                "left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
                                "where stealth=0 and Tickets.id=" & b.Text
                tab = Me.MyGest.GetTab(strSQL)
                sw.WriteLine("Data" & vbTab & Now.Date)
                sw.WriteLine("Cliente" & vbTab & tab.Rows(0)("ragsoccli"))
                sw.WriteLine()
                sw.WriteLine("Nr.Ticket" & vbTab & "Stato" & vbTab & "Data Apertura" & vbTab & "Data Chiusura" & vbTab & "Data Scadenza" & vbTab & "Data Ultimo Stato" & vbTab & "Cliente" & vbTab & "Operatore" & vbTab & "SubCliente" & vbTab & "Utente" & vbTab & "Oggetto" & vbTab & "Descrizione")
                For i As Integer = 0 To tab.Rows.Count - 1
                    Dim f = tab.Rows(i)("descr").ToString.Split("<").Length - 1
                    Dim descr = tab.Rows(i)("descr")
                    For l As Integer = 0 To f - 1
                        descr = descr.ToString.Remove(descr.ToString.IndexOf("<"), descr.ToString.IndexOf(">") + 1 - descr.ToString.IndexOf("<"))
                    Next
                    Try
                        sw.WriteLine(tab.Rows(i)("idt") & vbTab & "Aperto" & vbTab & tab.Rows(i)("dataapertura") & vbTab & tab.Rows(i)("datachiusura") & vbTab & tab.Rows(i)("datascadenza") & vbTab & tab.Rows(i)("dataultimo") & vbTab & tab.Rows(i)("ragsoccli") & vbTab & tab.Rows(i)("nomecognomeop") & vbTab & tab.Rows(i)("ragsocsub") & vbTab & tab.Rows(i)("nomecognomeut") & vbTab & tab.Rows(i)("oggetto") & vbTab & descr.Replace("&nbsp;", " ").Replace("&egrave;", "è").Replace("&agrave;", "à").Replace("&ugrave;", "ù").Replace("Ã¨", "è").Replace(vbCrLf, "").Replace("piÃ¹", "più").Replace("&hellip;¹", "..."))
                    Catch
                        sw.WriteLine(tab.Rows(i)("idt") & vbTab & "Aperto" & vbTab & tab.Rows(i)("dataapertura") & vbTab & tab.Rows(i)("datachiusura") & vbTab & tab.Rows(i)("datascadenza") & vbTab & tab.Rows(i)("dataultimo") & vbTab & tab.Rows(i)("ragsoccli") & vbTab & tab.Rows(i)("nomecognomeop") & vbTab & tab.Rows(i)("ragsocsub") & vbTab & tab.Rows(i)("nomecognomeut") & vbTab & tab.Rows(i)("oggetto") & vbTab & "")

                    End Try

                    strSQL = "Select * from Evento " & _
                            "inner join Stato on Stato.id=Evento.idstato " & _
                            "left outer join Utente on Utente.id=Evento.idoperatore " & _
                            "left outer join Tariffazione on Tariffazione.id=Evento.idtipotariffazione " & _
                            "where stealth=0 and idtickets = " & b.Text
                    tab2 = Me.MyGest.GetTab(strSQL)
                    For j As Integer = 0 To tab2.Rows.Count - 1
                        Dim k = tab2.Rows(j)("descrizione").ToString.Split("<").Length - 1
                        Dim descrizione = tab2.Rows(j)("descrizione")
                        For l As Integer = 0 To k - 1
                            descrizione = descrizione.ToString.Remove(descrizione.ToString.IndexOf("<"), descrizione.ToString.IndexOf(">") + 1 - descrizione.ToString.IndexOf("<"))
                        Next
                        sw.WriteLine("" & vbTab & tab2.Rows(j)("stato") & vbTab & tab2.Rows(j)("data") & vbTab & "" & vbTab & "" & vbTab & "" & vbTab & "" & vbTab & tab2.Rows(j)("cognome") & vbTab & "" & vbTab & "" & vbTab & "" & vbTab & descrizione.ToString.Replace("&nbsp;", " ").Replace("&egrave;", "è").Replace("&agrave;", "à").Replace("&ugrave;", "ù").Replace("Ã¨", "è").Replace(vbCrLf, "").Replace("piÃ¹", "più").Replace("&hellip;", "..."))
                    Next
                Next
                sw.WriteLine()
            Next



            sw.Close()
        End Using
        Me.ScaricaFile(nomefile)
        'LinkButton1.PostBackUrl = "tickets.aspx?file=report/excel/prova.xls"
        'LinkButton1_Click(sender, e)
        'Dim strRequest As String = "www.apriunticket.it/report/excel/prova.xls" 'Request.QueryString("file")
        ''-- if something was passed to the file querystring
        'If strRequest <> "" Then
        '    'get absolute path of the file
        '    Dim path As String = strRequest 'Server.MapPath(strRequest)
        '    'get file object as FileInfo
        '    Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        '    '-- if the file exists on the server
        '    If file.Exists Then
        '        'set appropriate headers
        '        Response.Clear()
        '        Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
        '        Response.AddHeader("Content-Length", file.Length.ToString())
        '        Response.ContentType = "application/octet-stream"
        '        Response.WriteFile(file.FullName)
        '        Response.End()
        '        'if file does not exist
        '    Else
        '        Response.Write("This file does not exist.")
        '    End If
        '    'nothing in the URL as HTTP GET
        'Else
        '    Response.Write("Please provide a file to download.")
        'End If

        'Dim dbConn As New OleDbConnection(strExcelConn)
        'Dim strSQL As String

        'strSQL = "Create Table Report (Nr.Ticket number,Stato char(50),Data Apertura char(50),Data Chiusura char(50),Data Scadenza char(50),Data Ultimo Stato char(50),Cliente char(50),Operatore char(50),SubCliente char(50),Utente char(50),Oggetto char(50),Inventario char(50)"
        'Dim cmd As New OleDbCommand(strSQL, dbConn)
        'dbConn.Open()

        'cmd.ExecuteNonQuery()



        ''Insert Dati in file Excel

        'Dim tab As DataTable

        'Dim b As Button

        'For Each lv As ListViewItem In ListView1.Items
        '    b = lv.FindControl("btnGestisci")
        '    strSQL = "select u.nome + ' ' +u.cognome as nomecognomeut,o.nome + ' ' +o.cognome as nomecognomeop,SubCliente.ragsoc as ragsocsub,Cliente.ragsoc as ragsoccli,Tickets.id as idt,Inventario.descrizione as descr,* from Tickets " & _
        '                    "inner join Stato on Stato.id=Tickets.idStato " & _
        '                    "inner join Cliente on Cliente.id=Tickets.idcliente " & _
        '                    "left outer join Utente as u on u.id=Tickets.idutente " & _
        '                    "left outer join Utente as o on o.id=Tickets.idoperatore " & _
        '                    "left outer join SubCliente on SubCliente.id=Tickets.idSubCliente " & _
        '                    "left outer join Inventario on Inventario.id=Tickets.idinventario " & _
        '                    "left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
        '                    "where Tickets.id=" & b.Text
        '    tab = Me.MyGest.GetTab(strSQL)
        '    For i As Integer = 0 To tab.Rows.Count - 1
        '        strSQL = "INSERT INTO [Report$](Nr.Ticket,Stato,Data Apertura,Data Chiusura,Data Scadenza,Data Ultimo Stato,Cliente,Operatore,SubCliente,Utente,Oggetto,Inventario) VALUES(" & tab.Rows(i)("idt") & "," & tab.Rows(i)("stato") & "," & tab.Rows(i)("dataapertura") & "," & tab.Rows(i)("datachiusura") & "," & tab.Rows(i)("datascadenza") & "," & tab.Rows(i)("dataultimo") & "," & tab.Rows(i)("ragsoccli") & "," & tab.Rows(i)("nomecognomeop") & "," & tab.Rows(i)("ragsocsub") & "," & tab.Rows(i)("nomecognomeut") & "," & tab.Rows(i)("oggetto") & "," & tab.Rows(i)("descr") & ")"
        '        Dim cmd2 As New OleDbCommand(strSQL, dbConn)
        '        dbConn.Open()
        '        cmd2.ExecuteNonQuery()
        '    Next
        'Next



        'Dim intestazioni = "Data;" & vbCrLf & "Cliente;" & vbCrLf & "Stato;" & vbCrLf
        'Dim intestazioni2 = "Nr.Ticket;Stato;Data Apertura;Data Chiusura;Data Scadenza;Data Ultimo Stato;Cliente;Operatore;SubCliente;Utente;Oggetto;Inventario;"
        ''Dim bolla = app.ToString.Split(vbCrLf)(1)
        ''Dim finale1 = app.ToString.Split(vbCrLf)(app.ToString.Split(vbCrLf).Count - 3)
        ''Dim finale2 = app.ToString.Split(vbCrLf)(app.ToString.Split(vbCrLf).Count - 2)
        ''Dim PoC = app.ToString.Split(vbCrLf)(app.ToString.Split(vbCrLf).Count - 4)
        'Using sw As StreamWriter = File.CreateText(FILE_EXCELWR & "prova.xls")
        '    sw.Write(intestazioni & vbCrLf & intestazioni2) '& bolla)

        '    'While rdrDati.Read()
        '    '    sw.Write(rdrDati.Item("Cod") & ";" & rdrDati.Item("Descrizione") & ";" & rdrDati.Item("Quantita") & ";" & rdrDati.Item("QtaRilevata") & ";" & rdrDati.Item("Ordine") & ";" & rdrDati.Item("Nome") & ";" & rdrDati.Item("Collo").ToString.Replace("%", "") & vbCrLf)
        '    'End While
        '    'sw.WriteLine(PoC.Substring(1) & finale1 & finale2)
        '    sw.Close()
        'End Using
    End Sub

    'Private Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
    Private Sub ScaricaFile(ByVal nomefile As String)
        Dim strRequest As String = nomefile '"report/excel/prova.xls" 'Request.QueryString("file")
        '-- if something was passed to the file querystring
        If strRequest <> "" Then
            'get absolute path of the file
            Dim path As String = Server.MapPath(strRequest)
            'get file object as FileInfo
            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
            '-- if the file exists on the server
            If file.Exists Then
                'set appropriate headers
                Response.Clear()
                Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                Response.AddHeader("Content-Length", file.Length.ToString())
                Response.ContentType = "application/octet-stream"
                Response.WriteFile(file.FullName)
                Response.End()
                'if file does not exist
            Else
                Response.Write("This file does not exist.")
            End If
            'nothing in the URL as HTTP GET
        Else
            Response.Write("Please provide a file to download.")
        End If
    End Sub
End Class
