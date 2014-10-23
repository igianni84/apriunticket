Imports System.Reflection

Public Class TariffaLis
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _tariffazione As Tariffa
    Private _misura As Misura
    Private _listino As Listino
    Private _prezzounitario As Double
    Private _dirittochiamata As Double
    Private _prezzoextra As Double
    Private _percextra As String
    Private _costo As Double
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

    Public Property Tariffazione As Tariffa
        Get
            Return _tariffazione
        End Get
        Set(ByVal value As Tariffa)
            _tariffazione = value
        End Set
    End Property

    Public Property Misura As Misura
        Get
            Return _misura
        End Get
        Set(ByVal value As Misura)
            _misura = value
        End Set
    End Property

    Public Property Listino As Listino
        Get
            Return _listino
        End Get
        Set(ByVal value As Listino)
            _listino = value
        End Set
    End Property

    Public Property PrezzoUnitario As Double
        Get
            Return _prezzounitario
        End Get
        Set(ByVal value As Double)
            _prezzounitario = value
        End Set
    End Property

    Public Property DirittoChiamata As Double
        Get
            Return _dirittochiamata
        End Get
        Set(ByVal value As Double)
            _dirittochiamata = value
        End Set
    End Property

    Public Property PrezzoExtra As Double
        Get
            Return _prezzoextra
        End Get
        Set(ByVal value As Double)
            _prezzoextra = value
        End Set
    End Property

    Public Property PercExtra As String
        Get
            Return _percextra
        End Get
        Set(ByVal value As String)
            _percextra = value
        End Set
    End Property

    Public Property Costo As Double
        Get
            Return _costo
        End Get
        Set(ByVal value As Double)
            _costo = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM Tariffa WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Tariffazione = New Tariffa
            Me.Tariffazione.ID = DTR("idtariffazione")
            Me.Misura = New Misura
            Me.Misura.Load(DTR("idmisura"))
            Me.Listino = New Listino
            Me.Listino.Load(DTR("idlistino"))
            Me.Prezzounitario = DTR("prezzounitario")
            Me.DirittoChiamata = DTR("dirittochiamata")
            Me.PrezzoExtra = DTR("prezzoextra")
            Me.PercExtra = DTR("percextra")
            Me.Costo = DTR("costo")
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
        pr.Parameters.AddWithValue("@idtariffazione", Me.Tariffazione.ID)
        pr.Parameters.AddWithValue("@idmisura", Me.Misura.ID)
        pr.Parameters.AddWithValue("@idlistino", Me.Listino.ID)
        pr.Parameters.AddWithValue("@prezzounitario", Me.Prezzounitario)
        pr.Parameters.AddWithValue("@dirittochiamata", Me.DirittoChiamata)
        pr.Parameters.AddWithValue("@prezzoextra", Me.PrezzoExtra)
        pr.Parameters.AddWithValue("@percextra", Me.PercExtra)
        pr.Parameters.AddWithValue("@costo", Me.Costo)

        Dim tb As DataTable = Me.Mygest.GetTab("ITariffa", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM Tariffa WHERE ID=" & criterio
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
