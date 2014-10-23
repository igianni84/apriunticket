Imports System.Reflection

Public Class FitMail
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _utente As Utente
    Private _ticket As Tickets
    Private _evento As Evento
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
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

    Public Property Utente As Utente
        Get
            Return _utente
        End Get
        Set(ByVal value As Utente)
            _utente = value
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

    Public Property Evento As Evento
        Get
            Return _evento
        End Get
        Set(ByVal value As Evento)
            _evento = value
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
#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM FitMail WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Utente = New Utente
            Me.Utente.Load(DTR("idutente"))
            Me.Ticket = New Tickets
            Me.Ticket.Load(DTR("idticket"))
            Me.Evento = New Evento
            Me.Evento.Load(DTR("idevento"))
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
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
        pr.Parameters.AddWithValue("@idutente", Me.Utente.ID)
        pr.Parameters.AddWithValue("@idticket", Me.Ticket.ID)
        pr.Parameters.AddWithValue("@idevento", Me.Evento.ID)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        pr.Parameters.AddWithValue("@idsubazienda", Me.SubAzienda.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IFitMail", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal ticket As Integer, ByVal evento As Integer, ByVal azienda As Integer, ByVal subazienda As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM FitMail WHERE idticket=" & ticket & " and idevento=" & evento & " and idazienda=" & azienda & " and idsubazienda=" & subazienda
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
