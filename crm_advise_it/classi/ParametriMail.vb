Imports System.Reflection

Public Class ParametriMail
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _mittente As String
    Private _account As String
    Private _pass As String
    Private _smtp As String
    Private _porta As String
    Private _azienda As Azienda
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

    Public Property Mittente As String
        Get
            Return _mittente
        End Get
        Set(ByVal value As String)
            _mittente = value
        End Set
    End Property

    Public Property Account As String
        Get
            Return _account
        End Get
        Set(ByVal value As String)
            _account = value
        End Set
    End Property

    Public Property Pass As String
        Get
            Return _pass
        End Get
        Set(ByVal value As String)
            _pass = value
        End Set
    End Property

    Public Property Smtp As String
        Get
            Return _smtp
        End Get
        Set(ByVal value As String)
            _smtp = value
        End Set
    End Property

    Public Property Porta As String
        Get
            Return _porta
        End Get
        Set(ByVal value As String)
            _porta = value
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

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM ParametriMail WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Mittente = DTR("mittente")
            Me.Account = DTR("account")
            Me.Pass = DTR("pass")
            Me.Smtp = DTR("smtp")
            Me.Porta = DTR("porta")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
        End If
        DTR.Close()
    End Sub

    Private Sub _loaddaazienda(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM ParametriMail WHERE IDazienda=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Mittente = DTR("mittente")
            Me.Account = DTR("account")
            Me.Pass = DTR("pass")
            Me.Smtp = DTR("smtp")
            Me.Porta = DTR("porta")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
        End If
        DTR.Close()
    End Sub
#End Region

#Region "Public Method"

    Public Sub Load(ByVal criterio As Integer)
        Me._load(criterio)
    End Sub

    Public Sub LoadDaAzienda(ByVal criterio As Integer)
        Me._loaddaazienda(criterio)
    End Sub

    Public Function SalvaData() As Boolean
        Dim ris As Boolean = False

        Dim pr As New SqlClient.SqlCommand
        pr.Parameters.AddWithValue("@id", Me.ID)
        pr.Parameters.AddWithValue("@mittente", Me.Mittente)
        pr.Parameters.AddWithValue("@account", Me.Account)
        pr.Parameters.AddWithValue("@pass", Me.Pass)
        pr.Parameters.AddWithValue("@smtp", Me.Smtp)
        pr.Parameters.AddWithValue("@porta", Me.Porta)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IParametriMail", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM ParametriMail WHERE ID=" & criterio
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
