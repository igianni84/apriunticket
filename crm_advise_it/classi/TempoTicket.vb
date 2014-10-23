Imports System.Reflection

Public Class TempoTicket
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _ticket As Tickets
    Private _tipotariffazione As Tariffa
    Private _tempotot As String
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

    Public Property Ticket As Tickets
        Get
            Return _ticket
        End Get
        Set(ByVal value As Tickets)
            _ticket = value
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

    Public Property TempoTot As String
        Get
            Return _tempotot
        End Get
        Set(ByVal value As String)
            _tempotot = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM TempoTicket WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Ticket = New Tickets
            Me.Ticket.Load(DTR("idticket"))
            Me.TipoTariffazione = New Tariffa
            Me.TipoTariffazione.Load(DTR("idtipotariffazione"))
            Me.TempoTot = DTR("tempotot")

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
        pr.Parameters.AddWithValue("@idticket", Me.Ticket.ID)
        pr.Parameters.AddWithValue("@idtipotariffazione", Me.TipoTariffazione.ID)
        pr.Parameters.AddWithValue("@tempotot", Me.TempoTot)
        Dim tb As DataTable = Me.Mygest.GetTab("ITempoTicket", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM TempoTicket WHERE ID=" & criterio
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
