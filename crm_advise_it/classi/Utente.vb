Imports System.Reflection

Public Class Utente
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
    Private _userid As String
    Private _psw As String
    Private _isadmin As Int16
    Private _cliente As Cliente
    Private _subcliente As SubCliente
    Private _nome As String
    Private _cognome As String
    Private _cap As String
    Private _idregione As Int16
    Private _idprovincia As Int16
    Private _idcomune As String
    Private _indirizzo As String
    Private _idreparto As Int16
    Private _idtipo As Int16
    Private _issuperadmin As Int16
    Private _abilitato As Int16

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

    Public Property Userid As String
        Get
            Return _userid
        End Get
        Set(ByVal value As String)
            _userid = value
        End Set
    End Property

    Public Property Psw As String
        Get
            Return _psw
        End Get
        Set(ByVal value As String)
            _psw = value
        End Set
    End Property

    Public Property IsAdmin As Int16
        Get
            Return _isadmin
        End Get
        Set(ByVal value As Int16)
            _isadmin = value
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

    Public Property Nome As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property Cognome As String
        Get
            Return _cognome
        End Get
        Set(ByVal value As String)
            _cognome = value
        End Set
    End Property

    Public Property Cap As String
        Get
            Return _cap
        End Get
        Set(ByVal value As String)
            _cap = value
        End Set
    End Property

    Public Property Indirizzo As String
        Get
            Return _indirizzo
        End Get
        Set(ByVal value As String)
            _indirizzo = value
        End Set
    End Property

    Public Property IDRegione As Int16
        Get
            Return _idregione
        End Get
        Set(ByVal value As Int16)
            _idregione = value
        End Set
    End Property

    Public Property IDProvincia As Int16
        Get
            Return _idprovincia
        End Get
        Set(ByVal value As Int16)
            _idprovincia = value
        End Set
    End Property

    Public Property IDComune As String
        Get
            Return _idcomune
        End Get
        Set(ByVal value As String)
            _idcomune = value
        End Set
    End Property

    Public Property IDReparto As Int16
        Get
            Return _idreparto
        End Get
        Set(ByVal value As Int16)
            _idreparto = value
        End Set
    End Property

    Public Property IDTipo As Int16
        Get
            Return _idtipo
        End Get
        Set(ByVal value As Int16)
            _idtipo = value
        End Set
    End Property

    Public Property IsSuperAdmin As Int16
        Get
            Return _issuperadmin
        End Get
        Set(ByVal value As Int16)
            _issuperadmin = value
        End Set
    End Property

    Public Property Abilitato As Int16
        Get
            Return _abilitato
        End Get
        Set(ByVal value As Int16)
            _abilitato = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Utente WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
            Me.Userid = DTR("userid")
            Me.Psw = DTR("psw")
            Me.IsAdmin = DTR("isadmin")
            Me.Cliente = New Cliente
            Me.Cliente.Load(DTR("idcliente"))
            Me.SubCliente = New SubCliente
            Me.SubCliente.Load(DTR("idsubcliente"))
            Me.Nome = DTR("nome")
            Me.Cognome = DTR("cognome")
            Me.Cap = DTR("cap")
            Me.IDRegione = DTR("idregione")
            Me.IDProvincia = DTR("idprovincia")
            Me.IDComune = DTR("idcomune")
            Me.Indirizzo = DTR("indirizzo")
            Me.IDReparto = DTR("idreparto")
            Me.IDTipo = DTR("idtipo")
            Me.IsSuperAdmin = DTR("issuperadmin")
            Me.Abilitato = DTR("abilitato")

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
        pr.Parameters.AddWithValue("@userid", Me.Userid)
        pr.Parameters.AddWithValue("@psw", Me.Psw)
        pr.Parameters.AddWithValue("@isadmin", Me.IsAdmin)
        pr.Parameters.AddWithValue("@idcliente", Me.Cliente.ID)
        pr.Parameters.AddWithValue("@idsubcliente", Me.SubCliente.ID)
        pr.Parameters.AddWithValue("@nome", Me.Nome)
        pr.Parameters.AddWithValue("@cognome", Me.Cognome)
        pr.Parameters.AddWithValue("@cap", Me.Cap)
        pr.Parameters.AddWithValue("@idregione", Me.IDRegione)
        pr.Parameters.AddWithValue("@idprovincia", Me.IDProvincia)
        pr.Parameters.AddWithValue("@idcomune", Me.IDComune)
        pr.Parameters.AddWithValue("@indirizzo", Me.Indirizzo)
        pr.Parameters.AddWithValue("@idreparto", Me.IDReparto)
        pr.Parameters.AddWithValue("@idtipo", Me.IDTipo)
        pr.Parameters.AddWithValue("@issuperadmin", Me.IsSuperAdmin)
        pr.Parameters.AddWithValue("@abilitato", Me.Abilitato)

        Dim tb As DataTable = Me.Mygest.GetTab("IUtente", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Utente WHERE ID=" & criterio
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
