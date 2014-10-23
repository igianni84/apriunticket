
<%@ Page Language="vb" Debug="True" %> 

<%@ import Namespace="System.Net" %> 
<%@ import Namespace="System.Net.Sockets" %> 
<script runat="server"> 

'E' utile leggere l'RFC 1939 che spiega il Post Office Protocol (POP3) 
'ftp://ftp.rfc-editor.org/in-notes/rfc1939.txt 

Dim tcpC as New TcpClient() 


    
    Sub Page_load()
        If IsPostBack() Then
            lblMessaggi.Text = ""
            ReadMail(host.Text, utente.Text, pwd.Text)
        End If
    End Sub

' Manda il comando e restituisce la risposta 
Function SendCommand(byRef NetStream as NetworkStream, byVal sToSend as String) 
Dim bData() as Byte = Encoding.ASCII.GetBytes(sToSend.ToCharArray) 
NetStream.Write(bData,0,bData.Length()) 
Return GetResponse(NetStream) 
End Function 

' Controlla se c'è una risposta e la restituisce 
Function GetResponse(byRef NetStream as NetworkStream) 
Dim bytes(tcpC.ReceiveBufferSize) As Byte 
NetStream.Read(bytes, 0, bytes.length) 

'Restituisce i dati ricevuti 
Dim ReturnData As String = Encoding.ASCII.GetString(bytes) 
Return ReturnData 
End Function 

Function ReadMail(host as string, user as string, pass as string) 
Dim NetStream as NetworkStream, MyResponse as string 

' apre una connessione con il server di posta sulla porta 110 
try 
tcpC.Connect(host,110) 
catch MyEx as Exception 
'in caso di errore restituisce un messaggio 
lblMessaggi.Text += "Errore nella connessione all'host: " & host & " (porta 110)<br>" & _ 
"L'errore riportato è: " & MyEx.message & "<br>Controlla e riprova<br>" 
end try 

' Recupera la risposta 
try 
NetStream = tcpC.GetStream() 
MyResponse = GetResponse(netstream) 
catch MyEx as Exception 
lblMessaggi.Text += "Si è verificato un errore!" 
end try 

'Invia il nome dell'utente (account sul server) 
MyResponse = SendCommand(netstream,"user " & user & vbCrLF) 

'Invia la password 
MyResponse = SendCommand(netstream,"pass " & pass & vbCrLf) 

'Controlla se il collegamento è andato a buon fine 
if left(MyResponse,4)="-ERR" then 
lblMessaggi.Text += "Errore nel collegamento dell'utente; controlla i dati e riprova<BR>" 
lblMessaggi.Text += MyResponse & "<br>" 
MyResponse=SendCommand(netstream,"QUIT" & vbCrLF) 
tcpC.close 
else 
'Indica che il collegamento ha avuto successo 
lblMessaggi.Text += "Utente correttamente collegato<BR><BR>" 

'Richiede le statistiche dell'intera casella 
MyResponse=SendCommand(netstream,"stat" & vbCrLf) 

dim tmpArray() as string 
'nel primo elemento (indice 0) c'è '+OK' nel secondo il numero dei messaggi, nel terzo la dimensione dei messaggi in bytes 
tmpArray = split(MyResponse," ") 

dim thisMess as integer 
'quindi qui trovo il numero dei messaggi 
dim NumMess as string = tmpArray(1) 

if cint(NumMess) > 0 then 'controllo se ci sono messaggi 
'Scrivo il numero dei messaggi 
lblMessaggi.Text += "Ci sono " & NumMess & " messaggi per un totale di " & tmpArray(2) & " bytes<br>" 
'per ogni messaggio della casella recupero la dimensione 

for thisMess = 1 to cint(numMess) 
MyResponse = SendCommand(netstream,"list " & thisMess & vbCrLf) 
tmpArray = split(MyResponse," ") 

MyResponse = "<b>Il messaggio Numero: " & thisMess & " occupa " & tmpArray(2) & " bytes</b>" 
lblMessaggi.Text += MyResponse & "<br>" 
next 


for thisMess = 1 to cint(numMess) 



lblMessaggi.Text +="<b>Messaggio: " & thisMess &"</b><br>" 
'MyResponse=SendCommand(netstream,"TOP " & thisMess & " 10" & vbCrLf) 
MyResponse=SendCommand(netstream,"RETR " & thisMess & vbCrLf) 



lblMessaggi.Text += "<font color=#ff6600>"&Server.HtmlEncode(MyResponse).Replace(vbCrLf, "<br>")&"</font>" 




next 



else 
lblMessaggi.Text += "La casella è vuota" 
end if 
end if 

' chiudo la connessione con il server 
MyResponse=SendCommand(netstream,"QUIT" & vbCrLF) 

' chiudo la connessione TCP 
tcpC.close 
End Function 

</script> 
<html> 
<head> 
<title>Statistiche casella mail</title> 
</head> 
<body> 
<form id="calc" method="post" runat="server"> 
<p> 
Server di posta (POP3): 
<asp:TextBox id="host" runat="server" Width="200px"></asp:TextBox> 
</p> 
<p> 
Utente per collegamento: 
<asp:TextBox id="utente" runat="server" Width="200px"></asp:TextBox> 
</p> 
<p> 
Password della casella: 
<asp:TextBox id="pwd" runat="server" Width="200px" TextMode="Password"></asp:TextBox> 
</p> 
<p> 
<asp:Button id="Button1" runat="server" Text="Controlla Casella"></asp:Button> 
</p> 
<p> 
<asp:Label id="lblMessaggi" runat="server"></asp:Label> 
</p> 
</form> 
</body> 
</html>