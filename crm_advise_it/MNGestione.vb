Imports System.Data
Imports System.Data.SqlClient
Public Class MNGestione

    Public Sub New(ByVal _cnn As String)
        Me.ActiveCon = New SqlClient.SqlConnection
        Me.CNN = _cnn
    End Sub

#Region "Declare"

    Private cn As String
    Private con As System.Data.SqlClient.SqlConnection
    Private _str As String


#End Region

#Region "Public Property"

    Public Property CNN() As String
        Get
            Return cn
        End Get
        Set(ByVal value As String)
            cn = value
        End Set
    End Property

    Public Property ActiveCon() As System.Data.SqlClient.SqlConnection
        Get
            Return con
        End Get
        Set(ByVal value As System.Data.SqlClient.SqlConnection)
            con = value
        End Set
    End Property


#End Region

#Region "Public Method"

    Public Sub Connetti()

        Me.ActiveCon.ConnectionString = Me.CNN

        If Me.ActiveCon.State = ConnectionState.Closed Then
            SqlConnection.ClearPool(Me.ActiveCon)
            Me.ActiveCon.Open()
        End If

        
    End Sub

    Public Function GetTab(ByVal str As String) As DataTable

        Dim tab As New DataTable
        Dim adp As New SqlClient.SqlDataAdapter(str, Me.ActiveCon)
        adp.Fill(tab)

        Return tab

    End Function

    Public Function GetSet(ByVal str As String) As DataSet

        Dim dts As New DataSet
        Dim adp As New SqlClient.SqlDataAdapter(str, Me.ActiveCon)
        adp.Fill(dts)


        Return dts

    End Function

    Public Function GetReader(str As String) As SqlClient.SqlDataReader
        Dim dt As SqlClient.SqlDataReader
        Dim CMD As New SqlClient.SqlCommand(str, Me.ActiveCon)
        dt = CMD.ExecuteReader
        Return dt
    End Function

    Public Function Execute(str As String) As Boolean
        Dim CMD As New SqlClient.SqlCommand(str, Me.ActiveCon)
        Dim ris As Boolean = False
        Try
            CMD.ExecuteNonQuery()
            ris = True
        Catch ex As Exception

        End Try
        Return ris
    End Function

    Public Sub Dispose()

    End Sub

    Public Function GetTab(ByVal name As String, ByVal _par As SqlParameterCollection) As DataTable

        Dim tab As New DataTable
        Dim sql As New SqlCommand(name, Me.ActiveCon)
        sql.CommandType = CommandType.StoredProcedure

        For Each p As SqlParameter In _par
            sql.Parameters.AddWithValue(p.ParameterName, p.Value)
        Next

        Dim adp As New SqlClient.SqlDataAdapter(sql)
        adp.Fill(tab)

        Return tab

    End Function


#End Region

End Class
