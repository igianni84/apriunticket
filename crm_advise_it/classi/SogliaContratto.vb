Imports System.Reflection

Public Class SogliaContratto
    Public Sub New()
        Me.Mygest = New MNGestione(CGlobal.cs)
        Me.Mygest.Connetti()
        Me.ID = -1
    End Sub

#Region "declare"

    Private _id As Integer = -1
    Private _contratto As Contratto
    Private _tiposoglia As TipoSoglia
    Private _tipotariffazione As Tariffa
    Private _soglia As String
    Private _avviso As String
    Private _idfuori As Integer
    Private _costofisso As Double
    Private _costovar As Double
    Private _listino As Listino
    Private _sla As String

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

    Public Property Contratto As Contratto
        Get
            Return _contratto
        End Get
        Set(ByVal value As Contratto)
            _contratto = value
        End Set
    End Property

    Public Property TipoSoglia As TipoSoglia
        Get
            Return _tiposoglia
        End Get
        Set(ByVal value As TipoSoglia)
            _tiposoglia = value
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

    Public Property Soglia As String
        Get
            Return _soglia
        End Get
        Set(ByVal value As String)
            _soglia = value
        End Set
    End Property

    Public Property Avviso As String
        Get
            Return _avviso
        End Get
        Set(ByVal value As String)
            _avviso = value
        End Set
    End Property

    Public Property IDFuori As Integer
        Get
            Return _idfuori
        End Get
        Set(ByVal value As Integer)
            _idfuori = value
        End Set
    End Property

    Public Property CostoFisso As Double
        Get
            Return _costofisso
        End Get
        Set(ByVal value As Double)
            _costofisso = value
        End Set
    End Property

    Public Property CostoVar As Double
        Get
            Return _costovar
        End Get
        Set(ByVal value As Double)
            _costovar = value
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

    Public Property SLA As String
        Get
            Return _sla
        End Get
        Set(ByVal value As String)
            _sla = value
        End Set
    End Property

#End Region

#Region "Private Method"

    Private Sub _load(ByVal _criterio As Integer)
        Dim SQL As String = "SELECT * FROM SogliaContratto WHERE ID=" & _criterio

        Dim DTR As SqlClient.SqlDataReader = Me.Mygest.GetReader(SQL)

        If DTR.Read Then
            Me.ID = DTR("ID")
            Me.Contratto = New Contratto
            Me.Contratto.Load(DTR("idcontratto"))
            Me.TipoSoglia = New TipoSoglia
            Me.TipoSoglia.Load(DTR("idtiposoglia"))
            Me.TipoTariffazione = New Tariffa
            Me.TipoTariffazione.Load(DTR("idtipotariffazione"))
            Me.Soglia = DTR("soglia")
            Me.Avviso = DTR("avviso")
            Me.IDFuori = DTR("idfuori")
            Me.CostoFisso = DTR("costofisso")
            Me.CostoVar = DTR("costovar")
            Me.Listino = New Listino
            Me.Listino.Load(DTR("idlistino"))
            Me.SLA = DTR("sla")

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
        pr.Parameters.AddWithValue("@idcontratto", Me.Contratto.ID)
        pr.Parameters.AddWithValue("@idtiposoglia", Me.TipoSoglia.ID)
        pr.Parameters.AddWithValue("@idtipotariffazione", Me.TipoTariffazione.ID)
        pr.Parameters.AddWithValue("@soglia", Me.Soglia)
        pr.Parameters.AddWithValue("@avviso", Me.Avviso)
        pr.Parameters.AddWithValue("@idfuori", Me.IDFuori)
        pr.Parameters.AddWithValue("@costofisso", Me.CostoFisso)
        pr.Parameters.AddWithValue("@costovar", Me.CostoVar)
        pr.Parameters.AddWithValue("@idlistino", Me.Listino.ID)
        pr.Parameters.AddWithValue("@sla", Me.SLA)

        Dim tb As DataTable = Me.Mygest.GetTab("ISogliaContratto", pr.Parameters)

        Try
            Me.ID = tb.Rows(0)("id")
            ris = True
        Catch ex As Exception

        End Try

        Return ris

    End Function

    Public Function Delete(ByVal criterio As Integer) As Boolean
        Dim ris As Boolean = False
        Dim SQL As String = "DELETE FROM SogliaContratto WHERE ID=" & criterio
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
