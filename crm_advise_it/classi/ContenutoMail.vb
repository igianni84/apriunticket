Imports System.Reflection

Public Class ContenutoMail
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _oggetto As String
    Private _corpo As String
    Private _firma As String
    Private _tipomail As String
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

    Public Property Firma As String
        Get
            Return _firma
        End Get
        Set(ByVal value As String)
            _firma = value
        End Set
    End Property

    Public Property TipoMail As String
        Get
            Return _tipomail
        End Get
        Set(ByVal value As String)
            _tipomail = value
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
        Dim SQL As String = "SELECT * FROM ContenutoMail WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Oggetto = DTR("oggetto")
            Me.Corpo = DTR("corpo")
            Me.Firma = DTR("firma")
            Me.TipoMail = DTR("idtipomail")
            Me.Azienda = New Azienda
            Me.Azienda.Load(DTR("idazienda"))
        End If
        DTR.Close()
    End Sub


    Private Sub _loadtipomail(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM ContenutoMail WHERE IDtipomail=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Oggetto = DTR("oggetto")
            Me.Corpo = DTR("corpo")
            Me.Firma = DTR("firma")
            Me.TipoMail = DTR("idtipomail")
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

    Public Sub LoadTipoMail(ByVal criterio As Integer)
        Me._loadtipomail(criterio)
    End Sub

    Public Function SalvaData() As Boolean
        Dim ris As Boolean = False

        Dim pr As New SqlClient.SqlCommand
        pr.Parameters.AddWithValue("@id", Me.ID)
        pr.Parameters.AddWithValue("@oggetto", Me.Oggetto)
        pr.Parameters.AddWithValue("@corpo", Me.Corpo)
        pr.Parameters.AddWithValue("@firma", Me.Firma)
        pr.Parameters.AddWithValue("@idtipomail", Me.TipoMail)
        pr.Parameters.AddWithValue("@idazienda", Me.Azienda.ID)
        Dim tb As DataTable = Me.Mygest.GetTab("IContenutoMail", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM ContenutoMail WHERE ID=" & criterio
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
