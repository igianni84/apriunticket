Imports System.Reflection

Public Class Contratto
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _azienda As Azienda
    Private _subazienda As SubAzienda
    Private _codice As String
    Private _codesterno As String
    Private _dadata As DateTime
    Private _adata As DateTime
    Private _idtiposcadenza As Integer
    Private _importo As Double
    Private _idtipofatturazione As String
    Private _idtipocontratto As Integer

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

    Public Property Codice As String
        Get
            Return _codice
        End Get
        Set(ByVal value As String)
            _codice = value
        End Set
    End Property

    Public Property CodEsterno As String
        Get
            Return _codesterno
        End Get
        Set(ByVal value As String)
            _codesterno = value
        End Set
    End Property

    Public Property DaData As DateTime
        Get
            Return _dadata
        End Get
        Set(ByVal value As DateTime)
            _dadata = value
        End Set
    End Property

    Public Property AData As DateTime
        Get
            Return _adata
        End Get
        Set(ByVal value As DateTime)
            _adata = value
        End Set
    End Property

    Public Property IDTipoScadenza As Integer
        Get
            Return _idtiposcadenza
        End Get
        Set(ByVal value As Integer)
            _idtiposcadenza = value
        End Set
    End Property

    Public Property IDTipoContratto As Integer
        Get
            Return _idtipocontratto
        End Get
        Set(ByVal value As Integer)
            _idtipocontratto = value
        End Set
    End Property

    Public Property Importo As Double
        Get
            Return _importo
        End Get
        Set(ByVal value As Double)
            _importo = value
        End Set
    End Property

    Public Property IDTipoFatturazione As String
        Get
            Return _idtipofatturazione
        End Get
        Set(ByVal value As String)
            _idtipofatturazione = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Contratto WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
            Me.SubAzienda = New SubAzienda
            Me.SubAzienda.Load(DTR("idsubazienda"))
            Me.Codice = DTR("codice")
            Me.CodEsterno = DTR("codesterno")
            Me.DaData = DTR("dadata")
            Me.AData = DTR("adata")
            Me.IDTipoScadenza = DTR("idtiposcadenza")
            Me.IDTipoContratto = DTR("idtipocontratto")
            Me.Importo = DTR("importo")
            Me.idtipoFatturazione = DTR("idtipofatturazione")

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
        pr.Parameters.AddWithValue("@codice", Me.Codice)
        pr.Parameters.AddWithValue("@codesterno", Me.CodEsterno)
        pr.Parameters.AddWithValue("@dadata", Me.DaData)
        pr.Parameters.AddWithValue("@adata", Me.AData)
        pr.Parameters.AddWithValue("@idtiposcadenza", Me.IDTipoScadenza)
        pr.Parameters.AddWithValue("@idtipocontratto", Me.IDTipoContratto)
        pr.Parameters.AddWithValue("@importo", Me.Importo)
        pr.Parameters.AddWithValue("@idtipofatturazione", Me.IDTipoFatturazione)

        Dim tb As DataTable = Me.Mygest.GetTab("IContratto", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Contratto WHERE ID=" & criterio
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
