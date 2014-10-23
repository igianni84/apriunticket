Imports System.Reflection

Public Class GiorLavorativa
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _giorno As String
    Private _oraap As String
    Private _orach As String
    Private _pausain As String
    Private _pausafi As String
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

    Public Property Giorno As String
        Get
            Return _giorno
        End Get
        Set(ByVal value As String)
            _giorno = value
        End Set
    End Property

    Public Property OraAp As String
        Get
            Return _oraap
        End Get
        Set(ByVal value As String)
            _oraap = value
        End Set
    End Property

    Public Property OraCh As String
        Get
            Return _orach
        End Get
        Set(ByVal value As String)
            _orach = value
        End Set
    End Property

    Public Property PausaIn As String
        Get
            Return _pausain
        End Get
        Set(ByVal value As String)
            _pausain = value
        End Set
    End Property

    Public Property PausaFi As String
        Get
            Return _pausafi
        End Get
        Set(ByVal value As String)
            _pausafi = value
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
        Dim SQL As String = "SELECT * FROM GiorLavorativa WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Giorno = DTR("giorno")
            Me.OraAp = DTR("oraap")
            Me.OraCh = DTR("orach")
            Me.PausaIn = DTR("pausain")
            Me.PausaFi = DTR("pausafi")
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

    Public Function SalvaData() As Boolean
        Dim ris As Boolean = False

        Dim pr As New SqlClient.SqlCommand
        pr.Parameters.AddWithValue("@id", Me.ID)
        pr.Parameters.AddWithValue("@giorno", Me.Giorno)
        pr.Parameters.AddWithValue("@oraap", Me.OraAp)
        pr.Parameters.AddWithValue("@orach", Me.OraCh)
        pr.Parameters.AddWithValue("@pausain", Me.PausaIn)
        pr.Parameters.AddWithValue("@pausafi", Me.PausaFi)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IGiorLavorativa", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM GiorLavorativa WHERE IDazienda=" & criterio
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
