Imports System.Reflection

Public Class Inventari
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _codice As String
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
    Private _cliente As Cliente
    Private _subcliente As SubCliente
    Private _seriale As String
    Private _tipodispositivo As TipoDispositivo
    Private _marchio As Marchio
    Private _modello As Modello
    Private _descrizione As String
    Private _utente As Utente
    Private _ubicazione As String
    Private _ip As String
    Private _subnet As String
    Private _gateway As String
    Private _note As String
    Private _datascad As DateTime
    Private _fornitorecli As Fornitore
    Private _fornitoreorg As Fornitore
    Private _mobile As Int16

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

    Public Property Codice As String
        Get
            Return _codice
        End Get
        Set(ByVal value As String)
            _codice = value
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

    Public Property Seriale As String
        Get
            Return _seriale
        End Get
        Set(ByVal value As String)
            _seriale = value
        End Set
    End Property

    Public Property TipoDispositivo As TipoDispositivo
        Get
            Return _tipodispositivo
        End Get
        Set(ByVal value As TipoDispositivo)
            _tipodispositivo = value
        End Set
    End Property

    Public Property Marchio As Marchio
        Get
            Return _marchio
        End Get
        Set(ByVal value As Marchio)
            _marchio = value
        End Set
    End Property

    Public Property Modello As Modello
        Get
            Return _modello
        End Get
        Set(ByVal value As Modello)
            _modello = value
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

    Public Property Utente As Utente
        Get
            Return _utente
        End Get
        Set(ByVal value As Utente)
            _utente = value
        End Set
    End Property

    Public Property Ubicazione As String
        Get
            Return _ubicazione
        End Get
        Set(ByVal value As String)
            _ubicazione = value
        End Set
    End Property

    Public Property IP As String
        Get
            Return _ip
        End Get
        Set(ByVal value As String)
            _ip = value
        End Set
    End Property

    Public Property Subnet As String
        Get
            Return _subnet
        End Get
        Set(ByVal value As String)
            _subnet = value
        End Set
    End Property

    Public Property Gateway As String
        Get
            Return _gateway
        End Get
        Set(ByVal value As String)
            _gateway = value
        End Set
    End Property


    Public Property Note As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property

    Public Property DataScad As DateTime
        Get
            Return _datascad
        End Get
        Set(ByVal value As DateTime)
            _datascad = value
        End Set
    End Property

    Public Property FornitoreCli As Fornitore
        Get
            Return _fornitorecli
        End Get
        Set(ByVal value As Fornitore)
            _fornitorecli = value
        End Set
    End Property

    Public Property FornitoreOrg As Fornitore
        Get
            Return _fornitoreorg
        End Get
        Set(ByVal value As Fornitore)
            _fornitoreorg = value
        End Set
    End Property

    Public Property Mobile As Int16
        Get
            Return _mobile
        End Get
        Set(ByVal value As Int16)
            _mobile = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Inventario WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Codice = DTR("codice")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
            Me.Cliente = New Cliente
            Me.Cliente.Load(DTR("idcliente"))
            Me.SubCliente = New SubCliente
            Me.SubCliente.Load(DTR("idsubcliente"))
            Me.Seriale = DTR("seriale")
            Me.TipoDispositivo = New TipoDispositivo
            Me.TipoDispositivo.Load(DTR("idtipodispositivo"))
            Me.Marchio = New Marchio
            Me.Marchio.Load(DTR("idmarchio"))
            Me.Modello = New Modello
            Me.Modello.Load(DTR("idmodello"))
            Me.Descrizione = DTR("descrizione")
            Me.Utente = New Utente
            Me.Utente.Load(DTR("idutente"))
            Me.Ubicazione = DTR("ubicazione")
            Me.IP = DTR("ip")
            Me.Subnet = DTR("subnet")
            Me.Gateway = DTR("gateway")
            Me.Note = DTR("note")
            Me.DataScad = DTR("datascad")
            Me.FornitoreCli = New Fornitore
            Me.FornitoreCli.Load(DTR("idfornitorecli"))
            Me.FornitoreOrg = New Fornitore
            Me.FornitoreOrg.Load(DTR("idfornitoreorg"))
            Me.Mobile = DTR("mobile")
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
        pr.Parameters.AddWithValue("@codice", Me.Codice)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        pr.Parameters.AddWithValue("@idsubazienda", Me.SubAzienda.ID)
        pr.Parameters.AddWithValue("@idcliente", Me.Cliente.ID)
        pr.Parameters.AddWithValue("@idsubcliente", Me.SubCliente.ID)
        pr.Parameters.AddWithValue("@seriale", Me.Seriale)
        pr.Parameters.AddWithValue("@idtipodispositivo", Me.TipoDispositivo.ID)
        pr.Parameters.AddWithValue("@idmarchio", Me.Marchio.ID)
        pr.Parameters.AddWithValue("@idmodello", Me.Modello.ID)
        pr.Parameters.AddWithValue("@descrizione", Me.Descrizione)
        pr.Parameters.AddWithValue("@idutente", Me.Utente.ID)
        pr.Parameters.AddWithValue("@ubicazione", Me.Ubicazione)
        pr.Parameters.AddWithValue("@ip", Me.IP)
        pr.Parameters.AddWithValue("@subnet", Me.Subnet)
        pr.Parameters.AddWithValue("@gateway", Me.Gateway)
        pr.Parameters.AddWithValue("@note", Me.Note)
        pr.Parameters.AddWithValue("@datascad", Me.DataScad)
        pr.Parameters.AddWithValue("@idfornitorecli", Me.FornitoreCli.ID)
        pr.Parameters.AddWithValue("@idfornitoreorg", Me.FornitoreOrg.ID)
        pr.Parameters.AddWithValue("@mobile", Me.Mobile)
        Dim tb As DataTable = Me.Mygest.GetTab("IInventario", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Inventario WHERE ID=" & criterio
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
