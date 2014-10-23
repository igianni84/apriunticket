Imports System.Net.Mail
Imports System.Net
Imports System.Net.Sockets
Imports Limilabs.Client.IMAP
Imports Limilabs.Mail
Imports System.IO
Imports System.Net.Security

Public Class login1
    Inherits System.Web.UI.Page


#Region "Variabili"
    Public MyGest As MNGestione
    Private MyUtente As Utente
    Private MyVersione As Versione
    Private MyImportaMail As ImportaMail
    Private MyTicket As Tickets
    Private MyCliente As Cliente
    Private MysubCliente As SubCliente
    Protected versione As String

    Private Const _server As String = "imap.apriunticket.it"
    Private Const _user As String = "assistenza@apriunticket.it"
    Private Const _password As String = "Redial_07"
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.MyGest = New MNGestione(CGlobal.cs)
        Me.MyGest.Connetti()

        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(txtPswd.Text.Trim()) Then
                txtPswd.Attributes.Add("value", txtPswd.Text)

            End If
            Session.Clear()
            Session.Abandon()
            Session("id") = Nothing
            Session("tipoutente") = Nothing
            Session("isadmin") = Nothing
            Me.SalvaVersione()


            If (Request.Cookies("User") IsNot Nothing) Then

                If (Request.Cookies("User")("mail") IsNot Nothing) Then
                    txtEmailI.Text = Request.Cookies("User")("mail")
                End If
                If (Request.Cookies("User")("password") IsNot Nothing) Then
                    txtPswd.Text = Request.Cookies("User")("password")
                End If
            End If


            
            'ImportaMail
        End If
    End Sub

    'Private Sub ImportaMail()

    '    Using imap As New Imap
    '        imap.Connect(_server)                           ' Use overloads or ConnectSSL if you need to specify different port or SSL.
    '        imap.Login(_user, _password)                    ' You can also use: LoginPLAIN, LoginCRAM, LoginDIGEST, LoginOAUTH methods,
    '        ' or use UseBestLogin method if you want Mail.dll to choose for you.

    '        imap.SelectInbox()

    '        ' All search methods return messages' unique ids.

    '        Dim unseen As List(Of Long) = imap.Search(Flag.Unseen)          ' Simple 'by flag' search.

    '        Dim query As New SimpleImapQuery                                    ' Simple 'by query object' search.
    '        ' query.Subject = "report"
    '        query.Unseen = True
    '        Dim unseenReports As List(Of Long) = imap.Search(query)
    '        Dim status As FolderStatus = imap.Examine("INBOX")
    '        Dim unseenReportsNotFromAccounting As List(Of Long) = imap.Search( _
    '                Expression.And( _
    '                    Expression.To(_user) _
    '            ))                                                              ' Most advanced search using ExpressionAPI.

    '        For Each uid As Long In unseenReportsNotFromAccounting              ' Download emails from the last result.
    '            Dim email As IMail = New MailBuilder() _
    '                .CreateFromEml(imap.GetMessageByUID(uid))
    '            Me.MyImportaMail = New ImportaMail
    '            Me.MyImportaMail.Azienda = New Azienda
    '            Me.MyImportaMail.SubAzienda = New SubAzienda
    '            Me.MyImportaMail.Ticket = New Tickets
    '            Me.MyImportaMail.Load(uid)
    '            If Me.MyImportaMail.ID = "-1" Then
    '                Me.MyImportaMail.UID = uid
    '                Me.MyImportaMail.MailFrom = email.From(0).Address
    '                Me.MyImportaMail.MailTo = _user
    '                Try
    '                    Me.MyImportaMail.MailCC = email.Cc(0).Name
    '                Catch
    '                    Me.MyImportaMail.MailCC = "-1"
    '                End Try
    '                Me.MyImportaMail.Oggetto = email.Subject
    '                Me.MyImportaMail.Corpo = email.Text
    '                Me.MyImportaMail.Data = email.Date
    '                Me.MyImportaMail.Azienda.ID = Me.RestituisciAzienda(email.From(0).Address)
    '                Me.MyImportaMail.SubAzienda.ID = Me.RestituisciSubAzienda(email.From(0).Address)
    '                If email.Subject.Contains("[") Then
    '                    Me.MyImportaMail.Ticket.ID = email.Subject.Split("[")(1).Split("]")(0)
    '                Else
    '                    Me.MyImportaMail.Ticket.ID = "-1"
    '                End If
    '                If Me.MyImportaMail.Azienda.ID <> "-1" Or Me.MyImportaMail.SubAzienda.ID <> "-1" Then
    '                    Me.MyImportaMail.SalvaData()
    '                    imap.DeleteMessageByUID(uid)
    '                    Me.ApriTicket(Me.MyImportaMail.UID)
    '                End If
    '            End If
    '        Next

    '        imap.Close()
    '    End Using
    'End Sub

    Private Sub ApriTicket(ByVal uidmail)
        Me.MyCliente = New Cliente
        Me.MysubCliente = New SubCliente
        Me.MyCliente.Listino = New Listino
        Me.MysubCliente.Listino = New Listino
        Me.MyTicket = New Tickets
        Me.MyTicket.Azienda = New Azienda
        Me.MyTicket.SubAzienda = New SubAzienda
        Me.MyTicket.Soglia = New SogliaContratto
        Me.MyTicket.Contratto = New Contratto
        Me.MyTicket.Listino = New Listino
        Me.MyImportaMail = New ImportaMail
        Me.MyImportaMail.Ticket = New Tickets
        Me.MyImportaMail.Azienda = New Azienda
        Me.MyImportaMail.SubAzienda = New SubAzienda
        Me.MyTicket.Utente = New Utente
        Me.MyTicket.PerContoDi = New Utente
        Me.MyTicket.Operatore = New Utente
        Me.MyTicket.Cliente = New Cliente
        Me.MyTicket.SubCliente = New SubCliente
        Me.MyTicket.Inventario = New Inventari
        Me.MyTicket.AltroInventario = New Inventari
        Me.MyImportaMail.Load(uidmail)
        If MyImportaMail.ticket.id = "-1" Then
            'apriticket
            Me.MyTicket.Azienda.ID = MyImportaMail.Azienda.ID
            Me.MyTicket.SubAzienda.ID = MyImportaMail.SubAzienda.ID
            Me.MyTicket.ID = MyImportaMail.Ticket.ID
            Me.MyTicket.DataApertura = MyImportaMail.Data
            Me.MyTicket.Descrizione = MyImportaMail.Corpo
            Me.MyTicket.Oggetto = MyImportaMail.Oggetto
            Dim sql As String = "select * from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where userid='" & Me.MyImportaMail.MailFrom & "'"
            Dim tab As DataTable = Me.MyGest.GetTab(sql)
            If tab.Rows.Count > 0 Then
                Select Case tab.Rows(0)("tipoutente")
                    Case "Utente"
                        Me.MyTicket.Utente.ID = tab.Rows(0)("id")
                        If tab.Rows(0)("idsubcliente") <> "-1" Then
                            Me.MyTicket.SubCliente.ID = tab.Rows(0)("idsubcliente")
                            Me.MyTicket.Cliente.ID = tab.Rows(0)("idcliente")
                            Dim s As String = "Select *,Contratto.id as idcontr from Contratto inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id where tipocliente='subcliente' and idcliente=" & Me.MyTicket.SubCliente.ID & "and datascadenza>'" & DateTime.Now & "'"
                            Dim t As DataTable = Me.MyGest.GetTab(s)
                            If t.Rows.Count > 0 Then
                                Me.MyTicket.Contratto.ID = t.Rows(0)("idcontr")
                            Else
                                Me.MyTicket.Contratto.ID = "-1"
                            End If
                            If Me.MyTicket.Contratto.ID = "-1" Then
                                Me.MyTicket.Listino.ID = Me.MyCliente.Listino.ID
                            Else
                                Me.MyTicket.Listino.ID = "-1"
                            End If
                        Else
                            Me.MyTicket.Cliente.ID = tab.Rows(0)("idcliente")
                            Dim s As String = "Select *,Contratto.id as idcontr from Contratto inner join Contratto_Cliente on Contratto_Cliente.idcontratto=Contratto.id where tipocliente='cliente' and idcliente=" & Me.MyTicket.Cliente.ID & "and datascadenza>'" & DateTime.Now & "'"
                            Dim t As DataTable = Me.MyGest.GetTab(s)
                            If t.Rows.Count > 0 Then
                                Me.MyTicket.Contratto.ID = t.Rows(0)("idcontr")
                            Else
                                Me.MyTicket.Contratto.ID = "-1"
                            End If
                            If Me.MyTicket.Contratto.ID = "-1" Then

                                Me.MyTicket.Listino.ID = Me.MysubCliente.Listino.ID
                            Else
                                Me.MyTicket.Listino.ID = "-1"
                            End If
                        End If
                    Case "Operatore"
                        Me.MyTicket.Operatore.ID = tab.Rows(0)("id")
                End Select
                Dim sl As String = "select * from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where userid='" & Me.MyImportaMail.MailCC & "'"
                Dim tb As DataTable = Me.MyGest.GetTab(sl)
                If tb.Rows.Count > 0 Then
                    Me.MyTicket.PerContoDi.ID = tb.Rows(0)("id")
                End If
            End If

            Me.MyTicket.DataChiusura = "01/01/9999"
            Me.MyTicket.DataUltimo = "01/01/9999"
            Me.MyTicket.Soglia.ID = -1
            Me.MyTicket.DataScadenza = CType(MyImportaMail.Data, DateTime).AddDays(3)


           
            
            Me.MyTicket.Inventario.ID = "-1"
            Me.MyTicket.AltroInventario.ID = "-1"
            Me.MyTicket.Bloccante = 1
            Me.MyTicket.Guasto = 1
            Me.MyTicket.UrlImmagine = ""
            Me.MyTicket.UrlVideo = ""
            Me.MyTicket.idStato = 1
            Me.MyTicket.Stealth = 0
            Me.MyTicket.SalvaData()
        Else

        End If
    End Sub

    Private Function RestituisciAzienda(ByVal email As String)
        Dim sql As String = "select * from Utente where userid='" & email & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim ret = "-1"
        If tab.Rows.Count > 0 Then
            ret = tab.Rows(0)("idazienda")
            If ret = "-1" Then
                Dim s As String = "Select * from Cliente where id=" & tab.Rows(0)("idcliente")
                Dim t As DataTable = Me.MyGest.GetTab(s)
                If t.Rows.Count > 0 Then
                    ret = t.Rows(0)("idazienda")
                End If
            End If
        End If
        Return ret
    End Function

    Private Function RestituisciSubAzienda(ByVal email As String)
        Dim sql As String = "select * from Utente where userid='" & email & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim ret = "-1"
        If tab.Rows.Count > 0 Then
            ret = tab.Rows(0)("idsubazienda")
            If ret = "-1" Then
                Dim s As String = "Select * from Cliente where id=" & tab.Rows(0)("idcliente")
                Dim t As DataTable = Me.MyGest.GetTab(s)
                If t.Rows.Count > 0 Then
                    ret = t.Rows(0)("idsubazienda")
                End If
            End If
        End If
        Return ret
    End Function

    Private Sub SalvaVersione()
        Me.MyVersione = New Versione
        Dim sql As String = "Select * from Versione"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)

        If tab.Rows.Count > 0 Then
            versione = tab.Rows(0)("release").ToString.Split(".")(2)
            versione = versione + 1
            Session("release") = tab.Rows(0)("release").ToString.Split(".")(0) & "." & tab.Rows(0)("release").ToString.Split(".")(1) & "." & versione
            Me.MyVersione.ID = tab.Rows(0)("id")
            Me.MyVersione.Release = Session("release")
            Me.MyVersione.SalvaData()
        End If
    End Sub

    Private Sub Login_Click(sender As Object, e As System.EventArgs) Handles Login.Click
        If IsValid Then

            Dim tab As New DataTable
            Dim miaPasswordCriptata As String = VSTripleDES.EncryptData(txtPswd.Text)

            Dim sqlStr = "SELECT * FROM UTENTE inner join tipoUtente on Utente.idtipo=tipoUtente.id WHERE userid='" & txtEmailI.Text & "' AND psw='" & miaPasswordCriptata & "' and abilitato=1"
            tab = MyGest.GetTab(sqlStr)

            lblE.Visible = False
            If tab.Rows.Count > 0 Then
                'If tab.Rows(0)("Idtipo") = 1 Then
                Session.Add("id", tab.Rows(0)("id"))
                Session.Add("tipoutente", tab.Rows(0)("tipoutente"))
                Session.Add("isadmin", tab.Rows(0)("isadmin"))
                Session.Add("utente", tab.Rows(0)("nome") & " " & tab.Rows(0)("cognome"))

                If cbxRicorda.Checked Then
                    Response.Cookies("User")("mail") = ""
                    Response.Cookies("User")("password") = ""
                    Response.Cookies("UserSettings").Expires = DateTime.Now.AddDays(30)

                    Dim myCookie As HttpCookie = New HttpCookie("User")
                    myCookie("mail") = txtEmailI.Text
                    myCookie("password") = txtPswd.Text
                    myCookie.Expires = Now.AddDays(30)
                    Response.Cookies.Add(myCookie)
                End If
                Response.Redirect("~/tickets.aspx")

            Else
                txtEmailI.BackColor = Drawing.Color.Red
                txtPswd.BackColor = Drawing.Color.Red
            End If
            lblE.Visible = True
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Panel2.Visible = False
        Panel7.Visible = True
    End Sub

    Protected Sub btnInvia_Click(sender As Object, e As EventArgs) Handles btnInvia.Click
        Dim sql As String = "Select * From Utente where userid='" & txtEmailRec.Text & "'"
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim miaPasswordDeCriptata As String
        If tab.Rows.Count > 0 Then
            miaPasswordDeCriptata = VSTripleDES.DecryptData(tab.Rows(0)("psw"))

            Me.SendRecupero(txtEmailRec.Text, tab.Rows(0)("nome") & " " & tab.Rows(0)("cognome"), txtEmailRec.Text, miaPasswordDeCriptata)
            Me.EmailInviata()
            Panel6.Visible = True
            Panel7.Visible = False
            txtEmailRec.Text = ""
        Else
            'lblE.Text = "Email non corretta!"
        End If
    End Sub

    Private Sub SendRecupero(ByVal EMAIL As String, ByVal NOMINATIVO As String, ByVal USERNAME As String, ByVal PASSWORD As String)
        Dim SmtpClient As New Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage


        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"

        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = 25
        message.From = fromAddress
        If EMAIL.IndexOf("@") <> -1 Then
            message.To.Add(EMAIL)
        Else
            Exit Sub
        End If
        Dim testo As String

        message.Subject = "RECUPERO PASSWORD"
        testo = "<p><h2>Ciao <b>" & NOMINATIVO & "</b>:</h2></p>" & vbCrLf & _
        "Queste sono le credenziali inserite al momento della registrazione al portale C - CRM:" & vbCrLf & _
        "<br/><br/>" & vbCrLf & _
        "Email:" & USERNAME & vbCrLf & _
        "<br/>" & vbCrLf & _
        "Password:" & PASSWORD & vbCrLf & _
        "<br/><br/>" & vbCrLf & _
        "<b><br/><br/></b>" & vbCrLf & _
        "<b>DISTINTI SALUTI</b>" & vbCrLf






        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            

            SmtpClient.Send(message)

        Catch ex As Exception

            Response.Write(ex.ToString)
        End Try

    End Sub
    Private Sub EmailInviata()

        'Panel2.Visible = False
        'Panel6.Visible = True
    End Sub

    Private Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        Panel2.Visible = True
        Panel6.Visible = False
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        Panel2.Visible = True
        Panel7.Visible = False
    End Sub
End Class