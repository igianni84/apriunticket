Imports System.Reflection

Public Class Evento
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _data As DateTime
    Private _descrizione As String
    Private _operatore As Utente
    Private _utente As Utente
    Private _idstato As Integer
    Private _stealth As String
    Private _urlimmagine As String
    Private _urlvideo As String
    Private _tickets As Tickets
    Private _tempo As String
    Private _tipotariffazione As Tariffa
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

    Public Property Data As DateTime
        Get
            Return _data
        End Get
        Set(ByVal value As DateTime)
            _data = value
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

    Public Property Operatore As Utente
        Get
            Return _operatore
        End Get
        Set(ByVal value As Utente)
            _operatore = value
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

    Public Property IDStato As Int16
        Get
            Return _idstato
        End Get
        Set(ByVal value As Int16)
            _idstato = value
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

    Public Property Tickets As Tickets
        Get
            Return _tickets
        End Get
        Set(ByVal value As Tickets)
            _tickets = value
        End Set
    End Property

    Public Property Tempo As String
        Get
            Return _tempo
        End Get
        Set(ByVal value As String)
            _tempo = value
        End Set
    End Property

    Public Property TipoTariffazione As Tariffa
        Get
            Return _tipotariffazione
        End Get
        Set(ByVal value As Tariffa)
            _tipotariffazione = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Evento WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Data = DTR("data")
            Me.Descrizione = DTR("descrizione")
            Me.Operatore = New Utente
            Me.Operatore.Load(DTR("idoperatore"))
            Me.Utente = New Utente
            Me.Utente.Load(DTR("idutente"))
            Me.IDStato = DTR("idstato")
            Me.Stealth = DTR("stealth")
            Me.UrlImmagine = DTR("urlimmagine")
            Me.UrlVideo = DTR("urlvideo")
            Me.Tickets = New Tickets
            Me.Tickets.Load(DTR("idtickets"))
            Me.Tempo = DTR("tempo")
            Me.TipoTariffazione = New Tariffa
            Me.TipoTariffazione.Load(DTR("idtipotariffazione"))
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
        pr.Parameters.AddWithValue("@data", Me.Data)
        pr.Parameters.AddWithValue("@descrizione", Me.Descrizione)
        pr.Parameters.AddWithValue("@idoperatore", Me.Operatore.ID)
        pr.Parameters.AddWithValue("@idutente", Me.Utente.ID)
        pr.Parameters.AddWithValue("@idstato", Me.IDStato)
        pr.Parameters.AddWithValue("@stealth", Me.Stealth)
        pr.Parameters.AddWithValue("@urlimmagine", Me.UrlImmagine)
        pr.Parameters.AddWithValue("@urlvideo", Me.UrlVideo)
        pr.Parameters.AddWithValue("@idtickets", Me.Tickets.ID)
        pr.Parameters.AddWithValue("@tempo", Me.Tempo)
        pr.Parameters.AddWithValue("@idtipotariffazione", Me.TipoTariffazione.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IEvento", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Evento WHERE ID=" & criterio
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
