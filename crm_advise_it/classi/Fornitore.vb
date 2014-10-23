Imports System.Reflection

Public Class Fornitore
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _codice As String
    Private _ragsoc As String
    Private _pariva As String
    Private _tipo As String
    Private _codest As String
    Private _cap As String
    Private _idregione As Int16
    Private _idprovincia As Int16
    Private _idcomune As String
    Private _indirizzo As String
    Private _note As String

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

    Public Property RagSoc As String
        Get
            Return _ragsoc
        End Get
        Set(ByVal value As String)
            _ragsoc = value
        End Set
    End Property

    Public Property Pariva As String
        Get
            Return _pariva
        End Get
        Set(ByVal value As String)
            _pariva = value
        End Set
    End Property

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property

    Public Property Codest As String
        Get
            Return _codest
        End Get
        Set(ByVal value As String)
            _codest = value
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

    Public Property Note As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Fornitore WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Codice = DTR("codice")
            Me.RagSoc = DTR("ragsoc")
            Me.Pariva = DTR("pariva")
            Me.Tipo = DTR("tipo")
            Me.Codest = DTR("codiceest")
            Me.Cap = DTR("cap")
            Me.IDRegione = DTR("idregione")
            Me.IDProvincia = DTR("idprovincia")
            Me.IDComune = DTR("idcomune")
            Me.Indirizzo = DTR("indirizzo")
            Me.Note = DTR("note")
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
        pr.Parameters.AddWithValue("@ragsoc", Me.RagSoc)
        pr.Parameters.AddWithValue("@pariva", Me.Pariva)
        pr.Parameters.AddWithValue("@tipo", Me.Tipo)
        pr.Parameters.AddWithValue("@codiceest", Me.Codest)
        pr.Parameters.AddWithValue("@cap", Me.Cap)
        pr.Parameters.AddWithValue("@idregione", Me.IDRegione)
        pr.Parameters.AddWithValue("@idprovincia", Me.IDProvincia)
        pr.Parameters.AddWithValue("@idcomune", Me.IDComune)
        pr.Parameters.AddWithValue("@indirizzo", Me.Indirizzo)
        pr.Parameters.AddWithValue("@note", Me.Note)


        Dim tb As DataTable = Me.Mygest.GetTab("IFornitore", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Fornitore WHERE ID=" & criterio
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

