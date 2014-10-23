Imports System.Reflection

Public Class ImportaMail
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.UID = -1
    End Sub

#Region "declare"
    Private _id As Integer = -1
    Private _uid As Integer
    Private _mailfrom As String
    Private _mailto As String
    Private _mailcc As String
    Private _oggetto As String
    Private _corpo As String
    Private _data As DateTime
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
    Private _ticket As Tickets
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

    Public Property UID As Integer
        Get
            Return _uid
        End Get
        Set(ByVal value As Integer)
            _uid = value
        End Set
    End Property

    Public Property MailFrom As String
        Get
            Return _mailfrom
        End Get
        Set(ByVal value As String)
            _mailfrom = value
        End Set
    End Property

    Public Property MailTo As String
        Get
            Return _mailto
        End Get
        Set(ByVal value As String)
            _mailto = value
        End Set
    End Property

    Public Property MailCC As String
        Get
            Return _mailcc
        End Get
        Set(ByVal value As String)
            _mailcc = value
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

    Public Property Corpo As String
        Get
            Return _corpo
        End Get
        Set(ByVal value As String)
            _corpo = value
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

    Public Property Ticket As Tickets
        Get
            Return _ticket
        End Get
        Set(ByVal value As Tickets)
            _ticket = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM ImportaMail WHERE UID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("id")
            Me.UID = DTR("uid")
            Me.MailFrom = DTR("mailfrom")
            Me.MailTo = DTR("mailto")
            Me.MailCC = DTR("mailcc")
            Me.Oggetto = DTR("oggetto")
            Me.Corpo = DTR("corpo")
            Me.Data = DTR("data")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
            Me.Ticket = New Tickets
            Me.Ticket.Load(DTR("idticket"))
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
        pr.Parameters.AddWithValue("@uid", Me.UID)
        pr.Parameters.AddWithValue("@mailfrom", Me.MailFrom)
        pr.Parameters.AddWithValue("@mailto", Me.MailTo)
        pr.Parameters.AddWithValue("@mailcc", Me.MailCC)
        pr.Parameters.AddWithValue("@oggetto", Me.Oggetto)
        pr.Parameters.AddWithValue("@corpo", Me.Corpo)
        pr.Parameters.AddWithValue("@data", Me.Data)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        pr.Parameters.AddWithValue("@idsubazienda", Me.SubAzienda.ID)
        pr.Parameters.AddWithValue("@idticket", Me.Ticket.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IImportaMail", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM ImportaMail WHERE ID=" & criterio
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
