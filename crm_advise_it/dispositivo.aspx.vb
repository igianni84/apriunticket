Public Class dispositivo
    Inherits System.Web.UI.Page


#Region "Declare"
    Private MyGest As MNGestione
    Private MyTipoDispositivo As TipoDispositivo
    Private MyMarchio As Marchio
    Private MyModello As Modello
    Private MyCGlobal As CGlobal
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyCGlobal = New CGlobal
        Mygest.Connetti()

        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then

                    Select Case Session("tipoutente")
                        Case "SuperAdmin"
                            Me.CaricaDispositivo()
                            Me.FiltraDispositivo()
                        Case "Operatore"
                            Me.CaricaDispositivo(, Me.RestituisciOrganizzazione())
                            Me.FiltraDispositivo(Me.RestituisciOrganizzazione())
                        Case Else
                            Response.Redirect("~/login.aspx")
                    End Select

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.FiltraDispositivo()
    End Sub

    Private Sub FiltraDispositivo(Optional ByVal idorg As String = "-1")
        Dim tab As New DataTable

        Dim sqlStr = "SELECT * FROM TipoDispositivo " & _
                     "left outer join Marchio on TipoDispositivo.id=Marchio.idtipodispositivo " & _
                     "left outer join Modello on Marchio.id=Modello.idmarchio " & _
                     "where TipoDispositivo.id<>-1 "
        If ddlTipoDispositivo.SelectedValue <> "-1" Then
            sqlStr = sqlStr & "and TipoDispositivo.id=" & ddlTipoDispositivo.SelectedValue
            If ddlMarchio.SelectedValue <> "-1" Then
                sqlStr = sqlStr & " and Marchio.id=" & ddlMarchio.SelectedValue
                If ddlModello.SelectedValue <> "-1" Then
                    sqlStr = sqlStr & " and Modello.id=" & ddlModello.SelectedValue
                End If
            End If
        End If
        If idorg <> "-1" Then
            sqlStr = sqlStr & " and TipoDispositivo.idazienda=" & idorg
        End If
        tab = MyGest.GetTab(sqlStr)

        ListView1.DataSource = tab
        ListView1.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager1.Visible = True
        Else
            DataPager1.Visible = False
        End If
    End Sub

    Private Function RestituisciOrganizzazione() As Integer
        Dim sql As String = "select idazienda from utente where id=" & Session("id")
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        Dim res As Integer = -1
        If tab.Rows.Count > 0 Then
            res = tab.Rows(0)("idazienda")
        End If
        Return res
    End Function

    Private Sub CaricaDispositivo(Optional ByVal idd As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tipodispositivo,id,idazienda from TipoDispositivo"
        If idorg <> "-1" Then
            str = str & " where idazienda=" & idorg
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipoDispositivo.DataSource = tab
        Me.ddlTipoDispositivo.DataTextField = "tipodispositivo"
        Me.ddlTipoDispositivo.DataValueField = "id"
        Me.ddlTipoDispositivo.DataBind()
        If idd <> "-1" Then
            Me.ddlTipoDispositivo.SelectedValue = idd
        Else
            Me.ddlTipoDispositivo.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaMarchio(Optional ByVal idma As String = "-1", Optional ByVal idd As String = "-1")
        Dim str As String = "select marchio ,Marchio.id as idmar from Marchio"
        If idd <> "-1" Then
            str = str & " inner join TipoDispositivo on Marchio.idtipodispositivo=TipoDispositivo.id where TipoDispositivo.id=" & idd
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlMarchio.DataSource = tab
        Me.ddlMarchio.DataTextField = "marchio"
        Me.ddlMarchio.DataValueField = "idmar"
        Me.ddlMarchio.DataBind()
        If idma <> "-1" Then
            Me.ddlMarchio.SelectedValue = idma
        Else
            Me.ddlMarchio.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaModello(Optional ByVal idmo As String = "-1", Optional ByVal idma As String = "-1")
        Dim str As String = "select Modello.modello ,Modello.id as idmod from Modello"
        'If idma <> "-1" Then
        str = str & " inner join Marchio on Modello.idmarchio=Marchio.id where Marchio.id=" & idma
        'End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlModello.DataSource = tab
        Me.ddlModello.DataTextField = "modello"
        Me.ddlModello.DataValueField = "idmod"
        Me.ddlModello.DataBind()
        If idmo <> "-1" Then
            Me.ddlModello.SelectedValue = idmo
        Else
            Me.ddlModello.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub imgTipoDispositivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgTipoDispositivo.Click
        Me.MyTipoDispositivo = New TipoDispositivo
        Me.MyTipoDispositivo.Azienda = New Azienda
        Me.MyCGlobal = New CGlobal
        If txtTipoDispositivo.Text <> "" And MyCGlobal.Verifier("TipoDispositivo", "tipodispositivo", txtTipoDispositivo.Text, Me.RestituisciOrganizzazione()) Then
            Me.MyTipoDispositivo.TipoDispositivo = txtTipoDispositivo.Text
            Me.MyTipoDispositivo.Azienda.ID = Me.RestituisciOrganizzazione
            Me.MyTipoDispositivo.SalvaData()
            txtTipoDispositivo.Text = ""
            Me.CaricaDispositivo()
            Dim message As String = "Dispositivo Inserito"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Else
            Dim message As String = "Dispositivo già Presente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If

    End Sub

    Protected Sub ddlTipoDispositivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoDispositivo.SelectedIndexChanged
        Me.CaricaMarchio(, ddlTipoDispositivo.SelectedValue)
        Me.CaricaModello(-1, -1)
        Select Case Session("tipoutente")
            Case "SuperAdmin"
                Me.FiltraDispositivo()
            Case "Operatore"
                Me.FiltraDispositivo(Me.RestituisciOrganizzazione())
        End Select
    End Sub

    Protected Sub ddlMarchio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarchio.SelectedIndexChanged
        Me.CaricaModello(, ddlMarchio.SelectedValue)
        Select Case Session("tipoutente")
            Case "SuperAdmin"
                Me.FiltraDispositivo()
            Case "Operatore"
                Me.FiltraDispositivo(Me.RestituisciOrganizzazione())
        End Select
    End Sub


    Protected Sub imgMarchio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgMarchio.Click
        Me.MyMarchio = New Marchio
        Me.MyMarchio.TipoDispositivo = New TipoDispositivo
        Me.MyCGlobal = New CGlobal
        If txtMarchio.Text <> "" And MyCGlobal.Verifier("Marchio", "marchio", txtMarchio.Text) And ddlTipoDispositivo.SelectedValue <> "-1" Then

            Me.MyMarchio.Marchio = txtMarchio.Text
            Me.MyMarchio.TipoDispositivo.ID = ddlTipoDispositivo.SelectedValue
            Me.MyMarchio.SalvaData()
            txtMarchio.Text = ""
            Me.CaricaMarchio()
            Dim message As String = "Marchio Inserito"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Else
            Dim message As String = "Seleziona Dispositivo o Marchio già Presente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub imgModello_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgModello.Click
        Me.MyModello = New Modello
        Me.MyModello.Marchio = New Marchio
        Me.MyCGlobal = New CGlobal
        If txtModello.Text <> "" And MyCGlobal.Verifier("Modello", "modello", txtModello.Text) And ddlMarchio.SelectedValue <> "-1" Then
            Me.MyModello.Modello = txtModello.Text
            Me.MyModello.Marchio.ID = ddlMarchio.SelectedValue
            Me.MyModello.SalvaData()
            txtModello.Text = ""
            Me.CaricaModello()
            Dim message As String = "Modello Inserito"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        Else
            Dim message As String = "Seleziona Marchio o Modello già Presente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub imgElTipoDispositivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgElTipoDispositivo.Click
        If MyCGlobal.VerifierLegami("TipoDispositivo", "Inventario", "idtipodispositivo") Then
            If MyCGlobal.VerifierLegami("TipoDispositivo", "Marchio", "idtipodispositivo") Then
                If ddlTipoDispositivo.SelectedValue <> "-1" Then
                    Me.MyTipoDispositivo = New TipoDispositivo
                    Me.MyTipoDispositivo.Delete(ddlTipoDispositivo.SelectedValue)
                    Me.CaricaDispositivo()
                End If
            Else
                Dim message As String = "Attenzione! Ci sono dei legami tra questo dispositivo e i marchi"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If
        Else
            Dim message As String = "Attenzione! Ci sono dei legami con questo dispositivo e gli inventari"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub imgElMarchio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgElMarchio.Click
        If MyCGlobal.VerifierLegami("Marchio", "Inventario", "idmarchio") Then
            If MyCGlobal.VerifierLegami("Marchio", "Modello", "idmarchio") Then
                If ddlMarchio.SelectedValue <> "-1" Then
                    Me.MyMarchio = New Marchio
                    Me.MyMarchio.Delete(ddlMarchio.SelectedValue)
                    Me.CaricaMarchio()
                End If
            Else
                Dim message As String = "Attenzione! Ci sono dei legami tra questo marchio e i modelli"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
            End If
        Else
            Dim message As String = "Attenzione! Ci sono dei legami con questo marchio e  gli inventari"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub imgElModello_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgElModello.Click
        If ddlModello.SelectedValue <> "-1" Then
            Me.MyModello = New Modello
            Me.MyModello.Delete(ddlModello.SelectedValue)
            Me.CaricaModello()
        End If
    End Sub
End Class