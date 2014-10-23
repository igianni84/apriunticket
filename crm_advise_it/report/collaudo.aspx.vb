Public Class collaudo
    Inherits System.Web.UI.Page

    Private MYGest As MNGestione
    Private sql As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MYGest = New MnGestione(CGlobal.cs)
        MYGest.Connetti()
        If Not IsPostBack And Session("sql_app") <> "" Then
            sql = Session("sql_app")
            Dim tab As DataTable = Me.MYGest.GetTab(sql)
            Dim rd As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            rd.Load(Server.MapPath("~/Print/Report/CrEL_APP.rpt"))
            rd.SetDataSource(tab)
            cws.ReportSource = rd
            Session("sql_app") = ""
        End If

    End Sub
End Class