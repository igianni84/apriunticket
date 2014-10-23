Imports System.Reflection

Public Class Tickets
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
    Private _cliente As Cliente
    Private _subcliente As SubCliente
    Private _contratto As Contratto
    Private _soglia As SogliaContratto
    Private _listino As Listino
    Private _utente As Utente
    Private _percontodi As Utente
    Private _inventario As Inventari
    Private _altroinventario As Inventari
    Private _idstato As Integer
    Private _oggetto As String
    Private _descrizione As String
    Private _bloccante As Integer
    Private _guasto As Integer
    Private _dataapertura As DateTime
    Private _datachiusura As DateTime
    Private _datascadenza As DateTime
    Private _dataultimo As DateTime
    Private _urlimmagine As String
    Private _urlvideo As String
    Private _operatore As Utente
    Private _stealth As Int16
    Private Mygest As MNGestione

#End Region

#Region "Public Property"

    Public Property ID As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property Azienda As Azienda
        Get
            Return _azienda
        End Get
        Set(ByVal value As Azienda)
            _azienda = value
        End Set
    End Property

    Public Property SubAzienda As SubAzienda
        Get
            Return _subazienda
        End Get
        Set(ByVal value As SubAzienda)
            _subazienda = value
        End Set
    End Property

    Public Property Cliente As Cliente
        Get
            Return _cliente
        End Get
        Set(ByVal value As Cliente)
            _cliente = value
        End Set
    End Property

    Public Property SubCliente As SubCliente
        Get
            Return _subcliente
        End Get
        Set(ByVal value As SubCliente)
            _subcliente = value
        End Set
    End Property

    Public Property Contratto As Contratto
        Get
            Return _contratto
        End Get
        Set(ByVal value As Contratto)
            _contratto = value
        End Set
    End Property

    Public Property Soglia As SogliaContratto
        Get
            Return _soglia
        End Get
        Set(ByVal value As SogliaContratto)
            _soglia = value
        End Set
    End Property

    Public Property Listino As Listino
        Get
            Return _listino
        End Get
        Set(ByVal value As Listino)
            _listino = value
        End Set
    End Property

    Public Property Utente As Utente
        Get
            Return _utente
        End Get
        Set(ByVal value As Utente)
            _utente = value
        End Set
    End Property

    Public Property PerContoDi As Utente
        Get
            Return _percontodi
        End Get
        Set(ByVal value As Utente)
            _percontodi = value
        End Set
    End Property

    Public Property Inventario As Inventari
        Get
            Return _inventario
        End Get
        Set(ByVal value As Inventari)
            _inventario = value
        End Set
    End Property

    Public Property AltroInventario As Inventari
        Get
            Return _altroinventario
        End Get
        Set(ByVal value As Inventari)
            _altroinventario = value
        End Set
    End Property

    Public Property idStato As Int16
        Get
            Return _idstato
        End Get
        Set(ByVal value As Int16)
            _idstato = value
        End Set
    End Property

    Public Property Oggetto As String
        Get
            Return _oggetto
        End Get
        Set(ByVal value As String)
            _oggetto = value
        End Set
    End Property

    Public Property Descrizione As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property

    Public Property Bloccante As Int16
        Get
            Return _bloccante
        End Get
        Set(ByVal value As Int16)
            _bloccante = value
        End Set
    End Property


    Public Property Guasto As Int16
        Get
            Return _guasto
        End Get
        Set(ByVal value As Int16)
            _guasto = value
        End Set
    End Property

    Public Property DataApertura As DateTime
        Get
            Return _dataapertura
        End Get
        Set(ByVal value As DateTime)
            _dataapertura = value
        End Set
    End Property

    Public Property DataChiusura As DateTime
        Get
            Return _datachiusura
        End Get
        Set(ByVal value As DateTime)
            _datachiusura = value
        End Set
    End Property

    Public Property DataScadenza As DateTime
        Get
            Return _datascadenza
        End Get
        Set(ByVal value As DateTime)
            _datascadenza = value
        End Set
    End Property

    Public Property DataUltimo As DateTime
        Get
            Return _dataultimo
        End Get
        Set(ByVal value As DateTime)
            _dataultimo = value
        End Set
    End Property

    Public Property UrlImmagine As String
        Get
            Return _urlimmagine
        End Get
        Set(ByVal value As String)
            _urlimmagine = value
        End Set
    End Property

    Public Property UrlVideo As String
        Get
            Return _urlvideo
        End Get
        Set(ByVal value As String)
            _urlvideo = value
        End Set
    End Property

    Public Property Operatore As Utente
        Get
            Return _operatore
        End Get
        Set(ByVal value As Utente)
            _operatore = value
        End Set
    End Property

    Public Property Stealth As Int16
        Get
            Return _stealth
        End Get
        Set(ByVal value As Int16)
            _stealth = value
        End Set
    End Property
#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Tickets WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
            Me.Cliente = New Cliente
            Me.Cliente.Load(DTR("idcliente"))
            Me.SubCliente = New SubCliente
            Me.SubCliente.Load(DTR("idsubcliente"))
            Me.Contratto = New Contratto
            Me.Contratto.Load(DTR("idcontratto"))
            Me.Soglia = New SogliaContratto
            Me.Soglia.Load(DTR("idsoglia"))
            Me.Listino = New Listino
            Me.Listino.Load(DTR("idlistino"))
            Me.Utente = New Utente
            Me.Utente.Load(DTR("idutente"))
            Me.PerContoDi = New Utente
            Me.PerContoDi.Load(DTR("idpercontodi"))
            Me.Inventario = New Inventari
            Me.Inventario.Load(DTR("idinventario"))
            Me.AltroInventario = New Inventari
            Me.AltroInventario.Load(DTR("idaltroinventario"))

            Me.idStato = DTR("idstato")
            Me.Oggetto = DTR("oggetto")
            Me.Descrizione = DTR("descrizione")
            Me.Bloccante = DTR("bloccante")
            Me.Guasto = DTR("guasto")
            Me.DataApertura = DTR("dataapertura")
            Me.DataChiusura = DTR("datachiusura")
            Me.DataScadenza = DTR("datascadenza")
            Me.DataUltimo = DTR("dataultimo")
            Me.UrlImmagine = DTR("urlimmagine")
            Me.UrlVideo = DTR("urlvideo")
            Me.Operatore = New Utente
            Me.Operatore.Load(DTR("idoperatore"))
            Me.Stealth = DTR("stealth")
        End If
        DTR.Close()
    End Sub

#End Region

#Region "Public Method"

    Public Sub Load(ByVal criterio As Integer)
        Me._load(criterio)
    End Sub

    Public Function SalvaData() As Boolean
        Dim ris As Boolean = False

        Dim pr As New SqlClient.SqlCommand
        pr.Parameters.AddWithValue("@id", Me.ID)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        pr.Parameters.AddWithValue("@idsubazienda", Me.SubAzienda.ID)
        pr.Parameters.AddWithValue("@idcliente", Me.Cliente.ID)
        pr.Parameters.AddWithValue("@idsubcliente", Me.SubCliente.ID)
        pr.Parameters.AddWithValue("@idcontratto", Me.Contratto.ID)
        pr.Parameters.AddWithValue("@idsoglia", Me.Soglia.ID)
        pr.Parameters.AddWithValue("@idlistino", Me.Listino.ID)
        pr.Parameters.AddWithValue("@idutente", Me.Utente.ID)
        pr.Parameters.AddWithValue("@idpercontodi", Me.PerContoDi.ID)
        pr.Parameters.AddWithValue("@idinventario", Me.Inventario.ID)
        pr.Parameters.AddWithValue("@idaltroinventario", Me.AltroInventario.ID)
        pr.Parameters.AddWithValue("@idstato", Me.IDStato)
        pr.Parameters.AddWithValue("@oggetto", Me.Oggetto)
        pr.Parameters.AddWithValue("@descrizione", Me.Descrizione)
        pr.Parameters.AddWithValue("@bloccante", Me.Bloccante)
        pr.Parameters.AddWithValue("@Guasto", Me.Guasto)
        pr.Parameters.AddWithValue("@dataapertura", Me.DataApertura)
        pr.Parameters.AddWithValue("@datachiusura", Me.DataChiusura)
        pr.Parameters.AddWithValue("@datascadenza", Me.DataScadenza)
        pr.Parameters.AddWithValue("@dataultimo", Me.DataUltimo)
        pr.Parameters.AddWithValue("@urlimmagine", Me.UrlImmagine)
        pr.Parameters.AddWithValue("@urlvideo", Me.UrlVideo)
        pr.Parameters.AddWithValue("@idoperatore", Me.Operatore.ID)
        pr.Parameters.AddWithValue("@stealth", Me.Stealth)
        Dim tb As DataTable = Me.Mygest.GetTab("ITickets", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Tickets WHERE ID=" & criterio
        Try
            ris = Me.Mygest.Execute(SQL)
        Catch

        End Try
        Return ris

    End Function

    Public Sub Dispose()

        Dim fi() As FieldInfo
        Dim ty As Type = MyClass.GetType
        fi = ty.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.Static)

        Dim field As FieldInfo
        For Each field In fi
            If Convert.ToString(field.FieldType).IndexOf("crm_advise_it") <> -1 And Convert.ToString(field.FieldType).IndexOf("crm_advise_it.MNGestione") = -1 Then

                Dim TY_FIGLIO As Type = field.FieldType
                For Each ST As MethodInfo In TY_FIGLIO.GetMethods
                    Dim ass As Assembly = Assembly.GetExecutingAssembly

                    If ST.Name = "Dispose" Then
                        Dim VALORE As Object = ass.CreateInstance(Convert.ToString(field.FieldType))
                        ST.Invoke(VALORE, Nothing)
                    End If
                Next

            End If


        Next

        Me.Mygest.ActiveCon.Close()
    End Sub

#End Region
End Class
