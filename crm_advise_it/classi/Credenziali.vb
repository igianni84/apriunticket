Imports System.Reflection

Public Class Credenziali
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _descrizione As String
    Private _utente As String
    Private _pass As String
    Private _inventario As Inventari
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

    Public Property Descrizione As String
        Get
            Return _descrizione
        End Get
        Set(ByVal value As String)
            _descrizione = value
        End Set
    End Property

    Public Property Utente As String
        Get
            Return _utente
        End Get
        Set(ByVal value As String)
            _utente = value
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

    Public Property Inventario As Inventari
        Get
            Return _inventario
        End Get
        Set(ByVal value As Inventari)
            _inventario = value
        End Set
    End Property
#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Credenziali WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Descrizione = DTR("descrizione")
            Me.Utente = DTR("utente")
            Me.Pass = DTR("pass")
            Me.Inventario = New Inventari
            Me.Inventario.Load(DTR("idinventario"))
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
        pr.Parameters.AddWithValue("@descrizione", Me.Descrizione)
        pr.Parameters.AddWithValue("@utente", Me.Utente)
        pr.Parameters.AddWithValue("@pass", Me.Pass)
        pr.Parameters.AddWithValue("@idinventario", Me.Inventario.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("ICredenziali", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Credenziali WHERE ID=" & criterio
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
