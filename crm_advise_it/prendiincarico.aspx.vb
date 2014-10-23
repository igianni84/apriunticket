Public Class prendiincarico
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyCGlobal As CGlobal
    Private MyTickets As Tickets
    Private MyUtente As Utente
    Private MyEvento As Evento
    Private MyFitMail As FitMail
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
                    Me.PrendiInCaricoTicket()

                Else
                    Response.Redirect("~/login.aspx")
                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub PrendiInCaricoTicket()
        Me.MyTickets = New Tickets
        Me.MyTickets.Load(lblIdTickets.Text)
        Me.MyTickets.idStato = 2
        Me.MyTickets.SalvaData()

        Me.MyEvento = New Evento
        Me.MyEvento.Operatore = New Utente
        Me.MyEvento.Tickets = New Tickets
        Me.MyEvento.TipoTariffazione = New Tariffa
        Me.MyEvento.Utente = New Utente

        Me.MyEvento.Data = DateTime.Now
        Me.MyEvento.Descrizione = ""
        Me.MyEvento.ID = "-1"
        Me.MyEvento.Operatore.ID = Session("id")
        Me.MyEvento.Stealth = 0
        Me.MyEvento.Tempo = "00:00"
        Me.MyEvento.Tickets.ID = Me.MyTickets.ID
        Me.MyEvento.TipoTariffazione.ID = -1
        Me.MyEvento.IDStato = 2
        Me.MyEvento.UrlImmagine = ""
        Me.MyEvento.UrlVideo = ""
        Me.MyEvento.Utente.ID = -1
        Me.MyEvento.SalvaData()
        Me.InviaMailEvento(Me.MyEvento.ID)
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
        If Me.MyEvento.Utente.ID <> "-1" Then
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
                    azienda = True
                End If
            Next

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID & "and idcliente=" & Me.MyTickets.Cliente.ID & "and idsubcliente=" & Me.MyTickets.SubCliente.ID
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

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID & "and idcliente=" & Me.MyTickets.Cliente.ID & "and idsubcliente=" & Me.MyTickets.SubCliente.ID
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
        Dim tipo(10) As TextBox
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
                    Me.MyCGlobal.InviaMailEvento(idevento, tad.Rows(i)("idu"), Tipo)
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

End Class