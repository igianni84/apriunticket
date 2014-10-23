Imports System.Net.Mail

Public Class CGlobal
    Private MyGest As MNGestione
    Private MyTickets As Tickets
    Private MyEvento As Evento
    Private MyUtente As Utente
    Private MyParametriMail As ParametriMail
    Private MyContenutoMail As ContenutoMail
     Public Shared fileExcelOutWr As String = "\report\excel\"
    Public Shared cs As String = System.Configuration.ConfigurationManager.AppSettings.Item("cc")

    Public Sub New()
        Me.MyGest = New MNGestione(CGlobal.cs)
        Me.MyGest.Connetti()
    End Sub

    Public Function Verifier(ByVal table As String, ByVal campoid As String, ByVal valore As String, Optional ByVal idorg As String = "-1") As Boolean
        Dim sql = "Select * from " & table & " where " & campoid & "= '" & valore & "'"
        If idorg <> "-1" Then
            sql = sql & " and idazienda=" & idorg
        End If
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim ris As Boolean = True
        Try
            Dim count = tab.Rows.Count
            If count > 0 Then
                ris = False
            End If
        Catch ex As Exception

        End Try
        Return ris
    End Function

    Public Function VerifierLegami(ByVal table1 As String, ByVal table2 As String, ByVal campoid As String) As Boolean
        Dim sql = "Select * from " & table1 & " inner join " & table2 & " on " & table1 & ".id= " & table2 & "." & campoid

        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim ris As Boolean = True
        Try
            Dim count = tab.Rows.Count
            If count > 0 Then
                ris = False
            End If
        Catch ex As Exception

        End Try
        Return ris
    End Function

    Function GestNum(ByVal num As String) As String
        If IsNumeric(num) = True Then
            num = num.Replace(".", ",")
            If Not num.Contains(",") Then
                num = num & ",00"
            End If
        Else
            num = "0"
        End If
        Return num
    End Function

    Function Replica(ByVal num As String) As String
        If IsNumeric(num) = True Then
            num = num.Replace(".", ",")
        Else
            num = ""
        End If
        Return num


    End Function

    Function VerificaBloccoCliente(ByVal idcliente As String, ByVal tipocliente As String)
        Dim MyCliente As New Cliente
        Dim MySubCliente As New SubCliente
        Dim res = False
        If tipocliente = "Cliente" Then
            MyCliente.Load(idcliente)
            If MyCliente.Blocco_Amm = 0 Then
                res = True
            End If
        Else
            MySubCliente.Load(idcliente)
            If MySubCliente.Blocco_Amm = 0 Then
                res = True
            End If
        End If
        Return res
    End Function

    Function ControlloDelete(ByVal table As String, ByVal campoid As String, ByVal valore As String, Optional ByVal idorg As String = "-1") As Boolean
        Dim sql = "Select * from " & table & " where " & campoid & "= '" & valore & "'"
        If idorg <> "-1" Then
            sql = sql & " and idazienda=" & idorg
        End If
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim ris As Boolean = True
        Try
            Dim count = tab.Rows.Count
            If count > 0 Then
                ris = False
            End If
        Catch ex As Exception

        End Try
        Return ris

    End Function

    Function InviaMailAttivazione(ByVal id As String, ByVal tipo As String)
        Dim SmtpClient As New Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyParametriMail = New ParametriMail
        Me.MyContenutoMail = New ContenutoMail
        Me.MyUtente.Load(id)
        Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

        Me.MyContenutoMail.LoadTipoMail(tipo)
        Dim param = False
        If MyParametriMail.ID <> "-1" Then
            param = True
        End If

        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"
        Dim port As String = 25
        If param Then
            from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
            user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
            passw = Me.MyParametriMail.Pass '"LelloTata2006" '"advise07"
            host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
            port = Me.MyParametriMail.Porta
        End If


        Dim miaPasswordDeCriptata As String = VSTripleDES.DecryptData(Me.MyUtente.Psw)


        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = port
        message.From = fromAddress
        If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
            message.To.Add(Me.MyUtente.Userid)
        Else
            Return False
            Exit Function
        End If
        Dim testo As String
        Dim oggetto As String = "Attivazione Utente su ApriUnTicket.it Advise srl"
        Dim corpo As String = "Ti confermiamo l'avvenuta registrazione sul portale ApriUnTicket.it"
        Dim saluti As String = "DISTINTI SALUTI"



        'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
        'Dim tabs As DataTable = Me.MyGest.GetTab(s)
        'If tabs.Rows.Count > 0 Then
        '    If tabs.Rows(0)("oggetto") <> "" Then
        '        message.Subject = tabs.Rows(0)("oggetto")
        '    End If
        '    If tabs.Rows(0)("intestazione") <> "" Then
        '        intestazione = tabs.Rows(0)("intestazione")
        '    End If
        '    If tabs.Rows(0)("saluti") <> "" Then
        '        saluti = tabs.Rows(0)("saluti")
        '    End If
        'End If
        Dim accesso As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")
        Dim tempo As String = VSTripleDES.EncryptData(accesso & "\" & Me.MyUtente.ID)

        testo = "<p><h2>Ciao <b>" & Me.MyUtente.Nome & " " & Me.MyUtente.Cognome & "</b>:</h2></p>" & vbCrLf & _
       corpo & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Queste sono le tue credenziali di accesso:" & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Email:" & Me.MyUtente.Userid & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Password:" & miaPasswordDeCriptata & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Per attivare il tuo account clicca o copia il link di seguito:" & vbCrLf & _
       "<a href=""http://www.apriunticket.it/attivazione.aspx?utente=" & tempo & """>http://www.apriunticket.it/attivazione.aspx</a>" & vbCrLf & _
       "<b><br/><br/></b>" & vbCrLf & _
       "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf

        If Me.MyContenutoMail.ID <> "-1" Then
            oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione)

            testo = Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione)
            testo = testo & vbCrLf & "<br/><br/>" & vbCrLf & _
                    "Per attivare il tuo account clicca o copia il link di seguito:" & vbCrLf & _
                    "<a href=""http://www.apriunticket.it/attivazione.aspx?utente=" & tempo & """>http://www.apriunticket.it/attivazione.aspx</a>" & vbCrLf & _
                    "<b><br/><br/></b>" & vbCrLf

            testo = testo & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione)

        End If
        message.Subject = oggetto
        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            ' SmtpClient.UseDefaultCredentials = True
            ' SmtpClient.EnableSsl = True

            SmtpClient.Send(message)
            Return True
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        End Try
    End Function

    Function InviaMailAggiornamento(ByVal id As String, ByVal tipo As String)
        Dim SmtpClient As New Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyParametriMail = New ParametriMail
        Me.MyContenutoMail = New ContenutoMail
        Me.MyUtente.Load(id)
        Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

        Me.MyContenutoMail.LoadTipoMail(tipo)
        Dim param = False
        If MyParametriMail.ID <> "-1" Then
            param = True
        End If

        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"
        Dim port As String = 25
        If param Then
            from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
            user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
            passw = Me.MyParametriMail.Pass '"LelloTata2006" '"advise07"
            host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
            port = Me.MyParametriMail.Porta
        End If


        Dim miaPasswordDeCriptata As String = VSTripleDES.DecryptData(Me.MyUtente.Psw)


        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = port
        message.From = fromAddress
        If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
            message.To.Add(Me.MyUtente.Userid)
        Else
            Return False
            Exit Function
        End If
        Dim testo As String
        Dim oggetto As String = "Aggiornamento Credenziali su ApriUnTicket.it Advise srl"
        Dim corpo As String = "Sono state aggiornate le sue credenziali sul portale ApriUnTicket.it"
        Dim saluti As String = "DISTINTI SALUTI"



        'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
        'Dim tabs As DataTable = Me.MyGest.GetTab(s)
        'If tabs.Rows.Count > 0 Then
        '    If tabs.Rows(0)("oggetto") <> "" Then
        '        message.Subject = tabs.Rows(0)("oggetto")
        '    End If
        '    If tabs.Rows(0)("intestazione") <> "" Then
        '        intestazione = tabs.Rows(0)("intestazione")
        '    End If
        '    If tabs.Rows(0)("saluti") <> "" Then
        '        saluti = tabs.Rows(0)("saluti")
        '    End If
        'End If
        Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")

        testo = "<p><h2>Ciao <b>" & Me.MyUtente.Nome & " " & Me.MyUtente.Cognome & "</b>:</h2></p>" & vbCrLf & _
       corpo & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Queste sono le tue nuove credenziali di accesso:" & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Email:" & Me.MyUtente.Userid & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Password:" & miaPasswordDeCriptata & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf

        If Me.MyContenutoMail.ID <> "-1" Then
            oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione)

            testo = Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione) & _
                vbCrLf & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Userid$", Me.MyUtente.Userid).Replace("$Password$", miaPasswordDeCriptata).Replace("$Organizzazione$", Me.MyUtente.Azienda.Descrizione)
        End If
        message.Subject = oggetto
        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            ' SmtpClient.UseDefaultCredentials = True
            ' SmtpClient.EnableSsl = True

            SmtpClient.Send(message)
            Return True
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        End Try
    End Function

    'con funzionalità mail condivisa
    'Function InviaMailTicket(ByVal idticket As String, ByVal idutente As String)
    '    Dim SmtpClient As New Net.Mail.SmtpClient
    '    Dim message As New System.Net.Mail.MailMessage
    '    Me.MyAzienda = New Azienda
    '    Me.MyTickets = New Tickets
    '    Me.MyUtente = New Utente
    '    Me.MyUtente = New Utente
    '    Me.MyUtente.Azienda = New Azienda
    '    Me.MyParametriMail = New ParametriMail
    '    Me.MyContenutoMail = New ContenutoMail
    '    Me.MyTickets.Cliente = New Cliente
    '    Me.MyTickets.SubCliente = New SubCliente
    '    Me.MyTickets.Inventario = New Inventari
    '    Me.MyTickets.Inventario.TipoDispositivo = New TipoDispositivo
    '    Me.MyTickets.Inventario.Marchio = New Marchio
    '    Me.MyTickets.Inventario.Modello = New Modello
    '    Me.MyTickets.Operatore = New Utente

    '    Me.MyTickets.Load(idticket)
    '    Me.MyUtente.Load(Me.MyTickets.Operatore.ID)
    '    Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

    '    Me.MyContenutoMail.LoadTipoMail(3)
    '    Dim param = False
    '    If MyParametriMail.ID <> "-1" Then
    '        param = True
    '    End If

    '    Dim from As String = "assistenza@apriunticket.it"
    '    Dim user As String = "assistenza@apriunticket.it"
    '    Dim passw As String = "Redial_07"
    '    Dim host As String = "smtp.apriunticket.it"
    '    Dim port As String = 25
    '    If param Then
    '        from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
    '        user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
    '        passw = Me.MyParametriMail.Pass '"LelloTata2006" '"advise07"
    '        host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
    '        port = Me.MyParametriMail.Porta
    '    End If


    '    Dim miaPasswordDeCriptata As String = VSTripleDES.DecryptData(Me.MyUtente.Psw)


    '    Dim dominio As String = ""
    '    Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


    '    SmtpClient.Host = host
    '    SmtpClient.Port = port
    '    message.From = fromAddress
    '    If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
    '        message.To.Add(Me.MyUtente.Userid)
    '        If Me.MyUtente.Userid <> Me.MyTickets.Operatore.Userid Then
    '            message.Bcc.Add(Me.MyTickets.Operatore.Userid)
    '        End If
    '        If Me.MyUtente.Userid <> Me.MyTickets.PerContoDi.Userid Then
    '            message.CC.Add(Me.MyTickets.PerContoDi.Userid)
    '        End If
    '        If Me.MyUtente.Userid <> Me.MyTickets.Utente.Userid And Me.MyTickets.PerContoDi.Userid <> Me.MyTickets.Utente.Userid Then
    '            message.CC.Add(Me.MyTickets.Utente.Userid)
    '        End If
    '        If Me.MyTickets.Cliente.SubAzienda.ID <> "-1" Then
    '            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID
    '            Dim t As DataTable = Me.MyGest.GetTab(s)
    '            For i As Integer = 0 To t.Rows.Count - 1
    '                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID Then
    '                    message.Bcc.Add(t.Rows(i)("userid"))
    '                End If
    '            Next
    '        Else
    '            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID
    '            Dim t As DataTable = Me.MyGest.GetTab(s)
    '            For i As Integer = 0 To t.Rows.Count - 1
    '                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID Then
    '                    message.Bcc.Add(t.Rows(i)("userid"))
    '                End If
    '            Next
    '        End If
    '    Else
    '        Return False
    '        Exit Function
    '    End If
    '    Dim testo As String
    '    Dim oggetto As String = "Apertura Ticket su ApriUnTicket.it Advise srl"
    '    Dim corpo As String = "E' stato aperto un ticket sul portale ApriUnTicket.it"
    '    Dim saluti As String = "DISTINTI SALUTI"



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
    '    Dim sql As String = "select * from Stato where id=" & Me.MyTickets.idStato
    '    Dim tab As DataTable = Me.MyGest.GetTab(sql)
    '    Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")
    '    Dim tipout As String = ""
    '    Select Case Me.MyUtente.IDTipo
    '        Case 3
    '            tipout = "SuperAdmin"
    '        Case 4
    '            tipout = "Utente"
    '        Case 5
    '            tipout = "Operatore"
    '        Case 6
    '            tipout = "OperatoreForn"
    '    End Select
    '    testo = "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> Dettaglio Ticket </a>" & vbCrLf
    '    If Me.MyTickets.SubCliente.ID <> "-1" Then
    '        testo = testo & "<p><h2>Spett.le <b>" & Me.MyTickets.Cliente.RagSoc & "</b>:</h2></p>" & vbCrLf
    '    Else
    '        testo = testo & "<p><h2>Spett.le <b>" & Me.MyTickets.SubCliente.RagSoc & "</b>:</h2></p>" & vbCrLf
    '    End If
    '    testo = testo & corpo & vbCrLf & _
    '       "<br/><br/>" & vbCrLf & _
    '       "Ecco il problema riscontratto:" & vbCrLf & _
    '       "<br/><br/>" & vbCrLf & _
    '       "<b>Ticket n°:" & Me.MyTickets.ID & "</b>" & vbCrLf & _
    '       "<br/>" & vbCrLf & _
    '       "Oggetto:" & Me.MyTickets.Oggetto & vbCrLf & _
    '       "<br/>" & vbCrLf & _
    '       "Descrizione:" & Me.MyTickets.Descrizione & vbCrLf & _
    '       "<br/>" & vbCrLf & _
    '       "Stato:" & tab.Rows(0)("stato") & vbCrLf & _
    '       "<br/>" & vbCrLf & _
    '       "Cliente:" & Me.MyTickets.Cliente.RagSoc & vbCrLf & _
    '       "<br/>" & vbCrLf & _
    '       "SubCliente:" & Me.MyTickets.SubCliente.RagSoc & vbCrLf & _
    '       "<br/>" & vbCrLf
    '    Try
    '        If Not IsDBNull(Me.MyTickets.Inventario.TipoDispositivo) Then
    '            testo = testo & "Inventario:" & Me.MyTickets.Inventario.TipoDispositivo.TipoDispositivo & " " & Me.MyTickets.Inventario.Marchio.Marchio & " " & Me.MyTickets.Inventario.Modello.Modello & vbCrLf & _
    '       "<br/><br/><br/>" & vbCrLf
    '        End If
    '    Catch
    '    End Try
    '    testo = testo & "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf

    '    If Me.MyContenutoMail.ID <> "-1" Then
    '        Dim tipodispositivo = ""
    '        Dim marchio = ""
    '        Dim modello = ""
    '        Dim subcliente = ""

    '        Try
    '            tipodispositivo = Me.MyTickets.Inventario.TipoDispositivo.TipoDispositivo
    '        Catch
    '        End Try
    '        Try
    '            marchio = Me.MyTickets.Inventario.Marchio.Marchio
    '        Catch
    '        End Try
    '        Try
    '            modello = Me.MyTickets.Inventario.Modello.Modello
    '        Catch
    '        End Try
    '        Try
    '            subcliente = Me.MyTickets.SubCliente.RagSoc
    '        Catch
    '        End Try


    '        oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello)


    '        testo = vbCrLf & "Accedi:<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> Dettaglio Ticket </a>"

    '        testo = testo & Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello)
    '        testo = testo & vbCrLf & vbCrLf & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello)

    '    End If
    '    message.Subject = oggetto
    '    message.Body = testo
    '    message.IsBodyHtml = True
    '    message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

    '    Try
    '        SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
    '        ' SmtpClient.UseDefaultCredentials = True
    '        ' SmtpClient.EnableSsl = True

    '        SmtpClient.Send(message)
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '        MsgBox(ex.ToString)
    '    End Try
    'End Function

    Function InviaMailTicket(ByVal idticket As String, ByVal idutente As String, ByVal temporesiduo() As TextBox)
        Dim SmtpClient As New Net.Mail.SmtpClient

        Dim message As New System.Net.Mail.MailMessage
        Me.MyTickets = New Tickets
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.SubAzienda = New SubAzienda
        Me.MyParametriMail = New ParametriMail
        Me.MyContenutoMail = New ContenutoMail
        Me.MyTickets.Cliente = New Cliente
        Me.MyTickets.SubCliente = New SubCliente
        Me.MyTickets.Inventario = New Inventari
        Me.MyTickets.Inventario.TipoDispositivo = New TipoDispositivo
        Me.MyTickets.Inventario.Marchio = New Marchio
        Me.MyTickets.Inventario.Modello = New Modello
        Me.MyUtente.Load(idutente)
        Me.MyTickets.Load(idticket)
        Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

        Me.MyContenutoMail.LoadTipoMail(3)
        Dim param = False
        If MyParametriMail.ID <> "-1" Then
            param = True
        End If

        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"
        Dim port As String = 25
        If param Then
            from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
            user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
            passw = Me.MyParametriMail.Pass
            host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
            port = Me.MyParametriMail.Porta
        End If


        Dim miaPasswordDeCriptata As String = VSTripleDES.DecryptData(Me.MyUtente.Psw)


        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = port
        message.From = fromAddress
        If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
            message.To.Add(Me.MyUtente.Userid)
        Else
            Return False
            Exit Function
        End If
        Dim testo As String
        Dim oggetto As String = "Apertura Ticket n° [" & idticket & "] su ApriUnTicket.it Advise srl"
        Dim corpo As String = "E' stato aperto un ticket sul portale ApriUnTicket.it"
        Dim saluti As String = "DISTINTI SALUTI"



        'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
        'Dim tabs As DataTable = Me.MyGest.GetTab(s)
        'If tabs.Rows.Count > 0 Then
        '    If tabs.Rows(0)("oggetto") <> "" Then
        '        message.Subject = tabs.Rows(0)("oggetto")
        '    End If
        '    If tabs.Rows(0)("intestazione") <> "" Then
        '        intestazione = tabs.Rows(0)("intestazione")
        '    End If
        '    If tabs.Rows(0)("saluti") <> "" Then
        '        saluti = tabs.Rows(0)("saluti")
        '    End If
        'End If
        Dim sql As String = "select * from Stato where id=" & Me.MyTickets.idStato
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")
        Dim tipout As String = ""
        Select Case Me.MyUtente.IDTipo
            Case 3
                tipout = "SuperAdmin"
            Case 4
                tipout = "Utente"
            Case 5
                tipout = "Operatore"
            Case 6
                tipout = "OperatoreForn"
        End Select
        testo = "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> <input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'> </a>" & vbCrLf
        Dim sopp As String = "select * from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where Utente.id=" & idutente & " and tipoutente='Operatore' and idazienda=" & Me.MyTickets.Azienda.ID & " and idsubazienda=" & Me.MyTickets.SubAzienda.ID
        Dim topp As DataTable = Me.MyGest.GetTab(sopp)
        If topp.Rows.Count > 0 Then
            testo = testo & "<a href=""http://www.apriunticket.it/prendiincarico.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> <input type='button' value='Prendi In Carico' style='border: none;margin-left:50px;background: blue; color:#fff ; width:100px;height:50px' onMouseOver='color:#0F0'></a>" & vbCrLf
        End If
        testo = testo & "<p><h2>Spett.le <b>" & Me.MyUtente.Nome & " " & Me.MyUtente.Cognome & "</b>:</h2></p>" & vbCrLf & _
       corpo & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Ecco il problema riscontrato:" & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "<b>Ticket n°:" & Me.MyTickets.ID & "</b>" & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Oggetto:" & Me.MyTickets.Oggetto & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Descrizione:" & Me.MyTickets.Descrizione & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Stato:" & tab.Rows(0)("stato") & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Cliente:" & Me.MyTickets.Cliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf & _
       "SubCliente:" & Me.MyTickets.SubCliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf
        Try
            For i As Integer = 0 To temporesiduo.Length - 1
                testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
               "<br/>" & vbCrLf
            Next
        Catch
        End Try
        Try
            If Not IsDBNull(Me.MyTickets.Inventario.TipoDispositivo) Then
                testo = testo & "Inventario:" & Me.MyTickets.Inventario.TipoDispositivo.TipoDispositivo & " " & Me.MyTickets.Inventario.Marchio.Marchio & " " & Me.MyTickets.Inventario.Modello.Modello & vbCrLf & _
           "<br/><br/><br/>" & vbCrLf
            End If
        Catch
        End Try


        testo = testo & "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf
        Dim sqlstato As String = "select * from Stato where id=" & Me.MyTickets.idStato
        Dim tabstato = Me.MyGest.GetTab(sql)

        If Me.MyContenutoMail.ID <> "-1" Then
            Dim tipodispositivo = ""
            Dim marchio = ""
            Dim modello = ""
            Dim subcliente = ""

            Try
                tipodispositivo = Me.MyTickets.Inventario.TipoDispositivo.TipoDispositivo
            Catch
            End Try
            Try
                marchio = Me.MyTickets.Inventario.Marchio.Marchio
            Catch
            End Try
            Try
                modello = Me.MyTickets.Inventario.Modello.Modello
            Catch
            End Try
            Try
                subcliente = Me.MyTickets.SubCliente.RagSoc
            Catch
            End Try

            If Me.MyContenutoMail.Oggetto.Contains("$Ticket$") Then
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Ticket$", "[" & Me.MyTickets.ID & "]").Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
            Else
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
                oggetto = oggetto & "Tickets n° [" & Me.MyTickets.ID & "]"
            End If

            testo = vbCrLf & "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> <input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'> </a>"
            Dim sop As String = "select * from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where Utente.id=" & idutente & " and tipoutente='Operatore' and idazienda=" & Me.MyTickets.Azienda.ID & " and idsubazienda=" & Me.MyTickets.SubAzienda.ID
            Dim top As DataTable = Me.MyGest.GetTab(sop)
            If top.Rows.Count > 0 Then
                testo = testo & "<a href=""http://www.apriunticket.it/prendiincarico.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """><input type='button' value='Prendi In Carico' style='border: none;margin-left:50px;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'></a>" & vbCrLf
            End If
            testo = testo & Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
            Try
                For i As Integer = 0 To temporesiduo.Count - 1
                    testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
                   "<br/>" & vbCrLf
                Next
            Catch
            End Try
            testo = testo & vbCrLf & vbCrLf & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)

        End If
        Dim s As String = "Select * from FitMail where idticket= " & Me.MyTickets.ID & " and idazienda=" & Me.MyTickets.Azienda.ID & " and idsubazienda=" & Me.MyTickets.SubAzienda.ID
        Dim t As DataTable = Me.MyGest.GetTab(s)
        testo = testo & vbCrLf & vbCrLf & "<br/><br/>In copia<br/><div style=""font-size:10px"">"
        For i As Integer = 0 To t.Rows.Count - 1
            Me.MyUtente.Load(t.Rows(i)("idutente"))
            testo = testo & Me.MyUtente.Userid & "<br/>"
        Next
        testo = testo & "</div>"
        message.Subject = oggetto
        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            ' SmtpClient.UseDefaultCredentials = True
            ' SmtpClient.EnableSsl = True

            SmtpClient.Send(message)
            Return True
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        End Try
    End Function

    Function InviaMailAggiornamentoTicket(ByVal idticket As String, ByVal idutente As String, ByVal temporesiduo() As TextBox)
        Dim SmtpClient As New Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage
        Me.MyTickets = New Tickets
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyParametriMail = New ParametriMail
        Me.MyContenutoMail = New ContenutoMail
        Me.MyTickets.Cliente = New Cliente
        Me.MyTickets.SubCliente = New SubCliente
        Me.MyTickets.Utente = New Utente
        Me.MyTickets.Inventario = New Inventari
        Me.MyTickets.Inventario.TipoDispositivo = New TipoDispositivo
        Me.MyTickets.Inventario.Marchio = New Marchio
        Me.MyTickets.Inventario.Modello = New Modello
        Me.MyUtente.Load(idutente)
        Me.MyTickets.Load(idticket)
        Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

        Me.MyContenutoMail.LoadTipoMail(3)
        Dim param = False
        If MyParametriMail.ID <> "-1" Then
            param = True
        End If

        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"
        Dim port As String = 25
        If param Then
            from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
            user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
            passw = Me.MyParametriMail.Pass '"LelloTata2006" '"advise07"
            host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
            port = Me.MyParametriMail.Porta
        End If


        Dim miaPasswordDeCriptata As String = VSTripleDES.DecryptData(Me.MyUtente.Psw)


        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = port
        message.From = fromAddress
        If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
            message.To.Add(Me.MyUtente.Userid)
        Else
            Return False
            Exit Function
        End If
        Dim testo As String
        Dim oggetto As String = "Aggiornamento Ticket n° [" & idticket & "] su ApriUnTicket.it Advise srl"
        Dim corpo As String = "E' stato aggiornato un ticket sul portale ApriUnTicket.it"
        Dim saluti As String = "DISTINTI SALUTI"



        'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
        'Dim tabs As DataTable = Me.MyGest.GetTab(s)
        'If tabs.Rows.Count > 0 Then
        '    If tabs.Rows(0)("oggetto") <> "" Then
        '        message.Subject = tabs.Rows(0)("oggetto")
        '    End If
        '    If tabs.Rows(0)("intestazione") <> "" Then
        '        intestazione = tabs.Rows(0)("intestazione")
        '    End If
        '    If tabs.Rows(0)("saluti") <> "" Then
        '        saluti = tabs.Rows(0)("saluti")
        '    End If
        'End If
        Dim sql As String = "select * from Stato where id=" & Me.MyTickets.idStato
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")
        Dim tipout As String = ""
        Select Case Me.MyUtente.IDTipo
            Case 3
                tipout = "SuperAdmin"
            Case 4
                tipout = "Utente"
            Case 5
                tipout = "Operatore"
            Case 6
                tipout = "OperatoreForn"
        End Select
        testo = "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> <input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'> </a>" & vbCrLf

        testo = testo & "<p><h2>Spett.le <b>" & Me.MyUtente.Nome & " " & Me.MyUtente.Cognome & "</b>:</h2></p>" & vbCrLf & _
       corpo & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Ecco l'aggiornamento rilevato:" & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "<b>Ticket n°:" & Me.MyTickets.ID & "</b>" & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Oggetto:" & Me.MyTickets.Oggetto & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Descrizione:" & Me.MyTickets.Descrizione & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Stato:" & tab.Rows(0)("stato") & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Cliente:" & Me.MyTickets.Cliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf & _
       "SubCliente:" & Me.MyTickets.SubCliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf
        Try
            For i As Integer = 0 To temporesiduo.Count - 1
                testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
               "<br/>" & vbCrLf
            Next
        Catch
        End Try
        Try
            If Not IsDBNull(Me.MyEvento.Tickets.Inventario.TipoDispositivo) Then
                testo = testo & "Inventario:" & Me.MyTickets.Inventario.TipoDispositivo.TipoDispositivo & " " & Me.MyTickets.Inventario.Marchio.Marchio & " " & Me.MyTickets.Inventario.Modello.Modello & vbCrLf & _
           "<br/><br/><br/>" & vbCrLf
            End If
        Catch
        End Try
        testo = testo & "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf
        Dim sqlstato As String = "select * from Stato where id=" & Me.MyTickets.idStato
        Dim tabstato = Me.MyGest.GetTab(sql)

        If Me.MyContenutoMail.ID <> "-1" Then
            Dim tipodispositivo = ""
            Dim marchio = ""
            Dim modello = ""
            Dim subcliente = ""

            Try
                tipodispositivo = Me.MyEvento.Tickets.Inventario.TipoDispositivo.TipoDispositivo
            Catch
            End Try
            Try
                marchio = Me.MyEvento.Tickets.Inventario.Marchio.Marchio
            Catch
            End Try
            Try
                modello = Me.MyEvento.Tickets.Inventario.Modello.Modello
            Catch
            End Try
            Try
                subcliente = Me.MyEvento.Tickets.SubCliente.RagSoc
            Catch
            End Try
            If Me.MyContenutoMail.Oggetto.Contains("$Ticket$") Then
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Ticket$", "[" & Me.MyTickets.ID & "]").Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
            Else
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
                oggetto = oggetto & "Tickets n° [" & Me.MyTickets.ID & "]"
            End If




            testo = vbCrLf & "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyTickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """><input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'></a>"

            testo = testo & Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)
            Try
                For i As Integer = 0 To temporesiduo.Count - 1
                    testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
                   "<br/>" & vbCrLf
                Next
            Catch
            End Try
            testo = testo & vbCrLf & vbCrLf & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyTickets.Oggetto).Replace("$Descrizione$", Me.MyTickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyTickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataInvio$", Me.MyTickets.DataApertura).Replace("$UtenteTicket$", Me.MyTickets.Utente.Cognome)

        End If
        Dim s As String = "Select * from FitMail where idticket= " & Me.MyTickets.ID & " and idazienda=" & Me.MyTickets.Azienda.ID & " and idsubazienda=" & Me.MyTickets.SubAzienda.ID
        Dim t As DataTable = Me.MyGest.GetTab(s)
        testo = testo & vbCrLf & vbCrLf & "<br/><br/>In copia<br/><div style=""font-size:10px"">"
        For i As Integer = 0 To t.Rows.Count - 1
            Me.MyUtente.Load(t.Rows(i)("idutente"))
            testo = testo & Me.MyUtente.Userid & "<br/>"
        Next
        testo = testo & "</div>"
        message.Subject = oggetto
        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            ' SmtpClient.UseDefaultCredentials = True
            ' SmtpClient.EnableSsl = True

            SmtpClient.Send(message)
            Return True
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        End Try
    End Function

    Function InviaMailEvento(ByVal idevento As String, ByVal idutente As String, ByVal temporesiduo() As TextBox)
        Dim SmtpClient As New Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage
        Me.MyEvento = New Evento
        Me.MyEvento.Tickets = New Tickets
        Me.MyEvento.Tickets.Utente = New Utente
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyParametriMail = New ParametriMail
        Me.MyContenutoMail = New ContenutoMail
        Me.MyEvento.Tickets.Cliente = New Cliente
        Me.MyEvento.Tickets.SubCliente = New SubCliente
        Me.MyEvento.Tickets.Inventario = New Inventari
        Me.MyEvento.Tickets.Inventario.TipoDispositivo = New TipoDispositivo
        Me.MyEvento.Tickets.Inventario.Marchio = New Marchio
        Me.MyEvento.Tickets.Inventario.Modello = New Modello
        Me.MyEvento.Utente = New Utente
        Me.MyUtente.Load(idutente)
        Me.MyEvento.Load(idevento)
        Me.MyParametriMail.LoadDaAzienda(Me.MyUtente.Azienda.ID)

        Me.MyContenutoMail.LoadTipoMail(3)
        Dim param = False
        If MyParametriMail.ID <> "-1" Then
            param = True
        End If

        Dim from As String = "assistenza@apriunticket.it"
        Dim user As String = "assistenza@apriunticket.it"
        Dim passw As String = "Redial_07"
        Dim host As String = "smtp.apriunticket.it"
        Dim port As String = 25
        If param Then
            from = Me.MyParametriMail.Mittente '"carlo_mail2002@yahoo.it" '"software@advise.it"
            user = Me.MyParametriMail.Account '"carlo_mail2002@yahoo.it" '"software@advise.it"
            passw = Me.MyParametriMail.Pass '"LelloTata2006" '"advise07"
            host = Me.MyParametriMail.Smtp '"smtp.mail.yahoo.it" '"authsmtp.advise.it"
            port = Me.MyParametriMail.Porta
        End If




        Dim dominio As String = ""
        Dim fromAddress As New MailAddress(from, "ApriUnTicket.it")


        SmtpClient.Host = host
        SmtpClient.Port = port
        message.From = fromAddress
        If Me.MyUtente.Userid.IndexOf("@") <> -1 Then
            message.To.Add(Me.MyUtente.Userid)
        Else
            Return False
            Exit Function
        End If
        Dim testo As String
        Dim oggetto As String = "Risposta Ticket n° [" & Me.MyEvento.Tickets.ID & "] su ApriUnTicket.it Advise srl"
        Dim corpo As String = "E' presente una risposta al ticket sul portale ApriUnTicket.it"
        Dim saluti As String = "DISTINTI SALUTI"



        'Dim s As String = "select * from ContentMail where tipo='att_ut' and idofficina=" & officina
        'Dim tabs As DataTable = Me.MyGest.GetTab(s)
        'If tabs.Rows.Count > 0 Then
        '    If tabs.Rows(0)("oggetto") <> "" Then
        '        message.Subject = tabs.Rows(0)("oggetto")
        '    End If
        '    If tabs.Rows(0)("intestazione") <> "" Then
        '        intestazione = tabs.Rows(0)("intestazione")
        '    End If
        '    If tabs.Rows(0)("saluti") <> "" Then
        '        saluti = tabs.Rows(0)("saluti")
        '    End If
        'End If
        Dim sql As String = "select * from Stato where id=" & Me.MyEvento.Tickets.idStato
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim tempo As String = System.DateTime.Now.ToString.Replace("/", "").Replace(" ", "").Replace(":", "")
        Dim tipout As String = ""
        Select Case Me.MyUtente.IDTipo
            Case 3
                tipout = "SuperAdmin"
            Case 4
                tipout = "Utente"
            Case 5
                tipout = "Operatore"
            Case 6
                tipout = "OperatoreForn"
        End Select
        testo = "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyEvento.Tickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """><input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'></a>" & vbCrLf

        testo = testo & "<p><h2>Spett.le <b>" & Me.MyUtente.Nome & " " & Me.MyUtente.Cognome & "</b>:</h2></p>" & vbCrLf & _
       corpo & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "Ecco il problema riscontrato:" & vbCrLf & _
       "<br/><br/>" & vbCrLf & _
       "<b>Ticket n°:" & Me.MyEvento.Tickets.ID & "</b>" & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Oggetto:" & Me.MyEvento.Tickets.Oggetto & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Descrizione Ticket:" & Me.MyEvento.Tickets.Descrizione & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Descrizione Evento:" & Me.MyEvento.Descrizione & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Stato:" & tab.Rows(0)("stato") & vbCrLf & _
       "<br/>" & vbCrLf & _
       "Cliente:" & Me.MyEvento.Tickets.Cliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf & _
       "SubCliente:" & Me.MyEvento.Tickets.SubCliente.RagSoc & vbCrLf & _
       "<br/>" & vbCrLf
        Try
            For i As Integer = 0 To temporesiduo.Count - 1
                testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
               "<br/>" & vbCrLf
            Next
        Catch
        End Try
        Try
            If Not IsDBNull(Me.MyEvento.Tickets.Inventario.TipoDispositivo) Or Not Me.MyEvento.Tickets.Inventario.TipoDispositivo.Equals(Nothing) Then
                testo = testo & "Inventario:" & Me.MyEvento.Tickets.Inventario.TipoDispositivo.TipoDispositivo & " " & Me.MyEvento.Tickets.Inventario.Marchio.Marchio & " " & Me.MyEvento.Tickets.Inventario.Modello.Modello & vbCrLf & _
                "<br/><br/><br/>" & vbCrLf
            End If
        Catch
        End Try
        testo = testo & "<b>" & vbCrLf & vbCrLf & saluti & "</b>" & vbCrLf
        Dim sqlstato As String = "select * from Stato where id=" & Me.MyEvento.Tickets.idStato
        Dim tabstato = Me.MyGest.GetTab(sql)
        If Me.MyContenutoMail.ID <> "-1" Then
            Dim tipodispositivo = ""
            Dim marchio = ""
            Dim modello = ""
            Dim subcliente = ""

            Try
                tipodispositivo = Me.MyEvento.Tickets.Inventario.TipoDispositivo.TipoDispositivo
            Catch
            End Try

            Try
                marchio = Me.MyEvento.Tickets.Inventario.Marchio.Marchio
            Catch
            End Try

            Try
                modello = Me.MyEvento.Tickets.Inventario.Modello.Modello
            Catch
            End Try
            Try
                subcliente = Me.MyEvento.Tickets.SubCliente.RagSoc
            Catch
            End Try
            If Me.MyContenutoMail.Oggetto.Contains("$Ticket$") Then
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyEvento.Tickets.Oggetto).Replace("$Corpo$", Me.MyEvento.Tickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyEvento.Tickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Ticket$", "[" & Me.MyEvento.Tickets.ID & "]").Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataEvento$", Me.MyEvento.Data).Replace("$DataInvio$", Me.MyEvento.Tickets.DataApertura).Replace("$UtenteEvento$", Me.MyEvento.Utente.Cognome).Replace("$UtenteTicket$", Me.MyEvento.Tickets.Utente.Cognome)
            Else
                oggetto = Me.MyContenutoMail.Oggetto.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyEvento.Tickets.Oggetto).Replace("$Corpo$", Me.MyEvento.Tickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyEvento.Tickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataEvento$", Me.MyEvento.Data).Replace("$DataInvio$", Me.MyEvento.Tickets.DataApertura).Replace("$UtenteEvento$", Me.MyEvento.Utente.Cognome).Replace("$UtenteTicket$", Me.MyEvento.Tickets.Utente.Cognome)

                oggetto = oggetto & "Tickets n° [" & Me.MyEvento.Tickets.ID & "]"
            End If

            testo = vbCrLf & "<a href=""http://www.apriunticket.it/gestticket.aspx?id=" & Me.MyEvento.Tickets.ID & "&utente=" & tempo & "\" & Me.MyUtente.ID & "&tipo=" & tipout & """> <input type='button' value='Dettaglio Ticket' style='border: none;background: blue;color: #fff;width:150px;height:50px' onMouseOver='color:#0F0'> </a>"

            testo = testo & Me.MyContenutoMail.Corpo.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyEvento.Tickets.Oggetto).Replace("$Descrizione$", Me.MyEvento.Tickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyEvento.Tickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataEvento$", Me.MyEvento.Data).Replace("$DataInvio$", Me.MyEvento.Tickets.DataApertura).Replace("$UtenteEvento$", Me.MyEvento.Utente.Cognome).Replace("$UtenteTicket$", Me.MyEvento.Tickets.Utente.Cognome)
            Try
                For i As Integer = 0 To temporesiduo.Count - 1
                    testo = testo & "Soglia:" & temporesiduo(i).Text & vbCrLf & _
                   "<br/>" & vbCrLf
                Next
            Catch
            End Try
            testo = testo & vbCrLf & vbCrLf & Me.MyContenutoMail.Firma.Replace("$Operatore$", Me.MyUtente.Cognome & " " & Me.MyUtente.Nome).Replace("$Oggetto$", Me.MyEvento.Tickets.Oggetto).Replace("$Descrizione$", Me.MyEvento.Tickets.Descrizione).Replace("$Stato$", tab.Rows(0)("stato")).Replace("$Cliente$", Me.MyEvento.Tickets.Cliente.RagSoc).Replace("$SubCliente$", subcliente).Replace("$Inventario$", tipodispositivo & " " & marchio & " " & modello).Replace("$Stato$", tabstato.Rows(0)("stato")).Replace("$DataEvento$", Me.MyEvento.Data).Replace("$DataInvio$", Me.MyEvento.Tickets.DataApertura).Replace("$UtenteEvento$", Me.MyEvento.Utente.Cognome).Replace("$UtenteTicket$", Me.MyEvento.Tickets.Utente.Cognome)

        End If
        Dim s As String = "Select * from FitMail where idticket= " & Me.MyEvento.Tickets.ID & " and idazienda=" & Me.MyEvento.Tickets.Azienda.ID & " and idsubazienda=" & Me.MyEvento.Tickets.SubAzienda.ID
        Dim t As DataTable = Me.MyGest.GetTab(s)
        testo = testo & vbCrLf & vbCrLf & "<br/><br/>In copia<br/><div style=""font-size:10px"">"
        For i As Integer = 0 To t.Rows.Count - 1
            Me.MyUtente.Load(t.Rows(i)("idutente"))
            testo = testo & Me.MyUtente.Userid & "<br/>"
        Next
        testo = testo & "</div>"
        message.Subject = oggetto
        message.Body = testo
        message.IsBodyHtml = True
        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess

        Try
            SmtpClient.Credentials = New System.Net.NetworkCredential(user, passw)
            ' SmtpClient.UseDefaultCredentials = True
            ' SmtpClient.EnableSsl = True

            SmtpClient.Send(message)
            Return True
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        End Try
    End Function
End Class
