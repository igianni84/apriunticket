Imports System.IO

Public Class suborganizzazioni
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MySubAzienda As SubAzienda
    Private MyRecapito As Recapito
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        Me.Form.Enctype = "multipart/form-data"
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaSubOrganizzazioni.Visible = True
                    PanelRicercaSubOrganizzazioni.Visible = True
                    Me.CaricaSubOrganizzazioni()
                    Me.CaricaOrganizzazioni()

                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

   

    Private Sub CaricaOrganizzazioni(Optional ida As String = "-1")
        Dim str As String = "select descrizione ,id from Azienda "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where Azienda.id=" & MyUtente.Azienda.ID
            If Session("isadmin") = 0 Then

            End If
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlAzienda.DataSource = tab
        Me.ddlAzienda.DataTextField = "descrizione"
        Me.ddlAzienda.DataValueField = "id"
        Me.ddlAzienda.DataBind()
        If ida <> "-1" Then
            Me.ddlAzienda.SelectedValue = ida
        Else
            Me.ddlAzienda.SelectedValue = "-1"
        End If
    End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaSubOrganizzazioni(txtRicercaSubOrganizzazioni.Text)
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaContatti()
    End Sub


    Private Sub CaricaSubOrganizzazioni(Optional ByVal descrizione As String = "")
        lblSicuro.Visible = False
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,SubAzienda.descrizione as nomeAzienda,Regioni.nome as regione,Province.nome as provincia " & _
                     "FROM SubAzienda inner join Azienda on Azienda.id=SubAzienda.idazienda " & _
                     "inner join Regioni on Regioni.idRegione=SubAzienda.idregione " & _
                     "inner join Province on Province.idProvincia=SubAzienda.idProvincia " & _
                     "where subAzienda.id<>-1"
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.SubAzienda = New SubAzienda
            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.SubAzienda.ID <> "-1" Then
                sqlStr = sqlStr & "and subAzienda.id=" & MyUtente.SubAzienda.ID
                If Session("isadmin") = 0 Then

                End If
            Else
                sqlStr = sqlStr & "and Azienda.id=" & MyUtente.Azienda.ID
            End If
        End If
        If descrizione <> "" Then
            sqlStr = sqlStr & "and SubAzienda.descrizione like '%" & descrizione & "%'"
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

    Private Sub ListView1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView1.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaSubOrganizzazione

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaSubOrganizzazione
    End Sub

    Private Sub ModificaSubOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        PanelRecapiti.Visible = True
        lbRecapiti.Visible = True
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MySubAzienda = New SubAzienda
        lblIdSubAzienda.Text = bt.AlternateText
        Me.MySubAzienda.Load(lblIdSubAzienda.Text)
        'Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
        'imgLogo.ImageUrl = "\logo\" & nome & "\" & MySubAzienda.Logo

        txtCodice.Text = Me.MySubAzienda.Codice
        txtCap.Text = Me.MySubAzienda.Cap
        txtIndirizzo.Text = Me.MySubAzienda.Indirizzo
        txtPiva.Text = Me.MySubAzienda.Pariva
        txtIndirizzo.Text = Me.MySubAzienda.Indirizzo
        Me.CaricaRegione(Me.MySubAzienda.IDRegione)
        Me.CaricaProvincia(Me.MySubAzienda.IDProvincia)
        'Me.CaricaComune(Me.MySubAzienda.IDComune)
        ddlComune.Text = Me.MySubAzienda.IDComune
        Me.CaricaOrganizzazioni(Me.MySubAzienda.Azienda.ID)
        txtRagSociale.Text = Me.MySubAzienda.Descrizione
        Panel1.Visible = False
        PanelModificaOrganizzazione.Visible = True
        PanelRicercaSubOrganizzazioni.Visible = False
        Me.DisabilitaCampi()
        Me.CaricaTipologiaContatto()
        Me.CaricaContatti()
    End Sub

    Private Sub CancellaSubOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdSubAzienda.Text = bt.AlternateText
        PanelEliminaOrg.Visible = True
        PanelRicercaSubOrganizzazioni.Visible = False
        Me.MySubAzienda = New SubAzienda
        Me.MySubAzienda.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MySubAzienda.Codice
        lblSubOrganizzazioneElimina.Text = Me.MySubAzienda.Descrizione
        lblPartitaIvaElimina.Text = Me.MySubAzienda.Pariva
        'Dim message As String = "Cancellazione avvenuta correttamente"
        'ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowPopup('" + message + "');", True)
    End Sub

    Private Sub CaricaRegione(Optional ByVal idr As String = "-1")
        Dim str As String = "select nome ,idRegione from Regioni"

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlRegione.DataSource = tab
        Me.ddlRegione.DataTextField = "nome"
        Me.ddlRegione.DataValueField = "idRegione"
        Me.ddlRegione.DataBind()
        If idr <> "-1" Then
            Me.ddlRegione.SelectedValue = idr
        Else
            Me.ddlRegione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaProvincia(Optional ByVal idp As String = "-1", Optional ByVal idr As String = "-1")
        Dim str As String = "select Province.nome ,idProvincia from Province"
        If idr <> "-1" Then
            str = str & " inner join Regioni on Province.idRegione=Regioni.idRegione where Regioni.idRegione=" & idr
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlProvincia.DataSource = tab
        Me.ddlProvincia.DataTextField = "nome"
        Me.ddlProvincia.DataValueField = "idProvincia"
        Me.ddlProvincia.DataBind()
        If idp <> "-1" Then
            Me.ddlProvincia.SelectedValue = idp
        Else
            Me.ddlProvincia.SelectedValue = "-1"
        End If
    End Sub

    'Private Sub CaricaComune(Optional ByVal idc As String = "-1", Optional ByVal idp As String = "-1")
    '    Dim str As String = "select Comuni.nome+' - '+Comuni.CAP as nom ,idComune from Comuni"
    '    If idp <> "-1" Then
    '        str = str & " inner join Province on Comuni.idProvincia=Province.idProvincia where Province.idProvincia=" & idp
    '    End If
    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(str)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "..."
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlComune.DataSource = tab
    '    Me.ddlComune.DataTextField = "nom"
    '    Me.ddlComune.DataValueField = "idComune"
    '    Me.ddlComune.DataBind()
    '    If idc <> "-1" Then
    '        Me.ddlComune.SelectedValue = idc
    '    Else
    '        Me.ddlComune.SelectedValue = "-1"
    '    End If
    'End Sub

    Protected Sub btnModifica_Click(sender As Object, e As EventArgs) Handles btnModifica.Click
        'txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        'ddlComune.Enabled = True
        'txtIndirizzo.ReadOnly = False
        'txtPiva.ReadOnly = False
        'ddlProvincia.Enabled = True
        'txtRagSociale.ReadOnly = False
        'ddlRegione.Enabled = True
        'txtRicercaOrganizzazioni.ReadOnly = False
        'btnModifica.Visible = False
        'btnSalva.Visible = True
        Me.AbilitaCampi()
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        Panel1.Visible = True
        PanelModificaOrganizzazione.Visible = False
        PanelRicercaSubOrganizzazioni.Visible = True
        Me.CaricaSubOrganizzazioni()
    End Sub

    Protected Sub btnRicercaOrganizzazioni_Click(sender As Object, e As EventArgs) Handles btnRicercaSubOrganizzazioni.Click
        Me.CaricaSubOrganizzazioni(txtRicercaSubOrganizzazioni.Text)
        Panel1.Visible = True
        PanelModificaOrganizzazione.Visible = False
       
    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If IsValid Then

            Me.MySubAzienda = New SubAzienda
            Me.MySubAzienda.Azienda = New Azienda
            Me.MySubAzienda.Load(lblIdSubAzienda.Text)
            Me.MySubAzienda.IDRegione = ddlRegione.SelectedValue
            Me.MySubAzienda.IDProvincia = ddlProvincia.SelectedValue
            Me.MySubAzienda.IDComune = ddlComune.Text
            Me.MySubAzienda.Cap = txtCap.Text
            Me.MySubAzienda.Codice = txtCodice.Text
            Me.MySubAzienda.Indirizzo = txtIndirizzo.Text
            Me.MySubAzienda.Pariva = txtPiva.Text
            Me.MySubAzienda.Azienda.ID = ddlAzienda.SelectedValue
            Me.MySubAzienda.Descrizione = txtRagSociale.Text
            'Me.MySubAzienda.Logo = txtLogo.Text
            Me.MySubAzienda.SalvaData()
            
            lblIdSubAzienda.Text = Me.MySubAzienda.ID
            lblConferma.Visible = True
            'lblConferma.Text = "SubOrganizzazione Creata"
            '
           
            Dim message2 As String = "SubOrganizzazione creata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            'Threading.Thread.Sleep(5000)
            Me.CaricaSubOrganizzazioni()
            Panel1.Visible = True
            PanelRicercaSubOrganizzazioni.Visible = True
            PanelModificaOrganizzazione.Visible = False
        Else
            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub ddlRegione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegione.SelectedIndexChanged
        Me.CaricaProvincia("-1", ddlRegione.SelectedValue)
        'Me.CaricaComune()
    End Sub

    'Protected Sub ddlProvincia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvincia.SelectedIndexChanged
    '    Me.CaricaComune("-1", ddlProvincia.SelectedValue)
    'End Sub

    

    Private Sub SvuotaCampi()
        lblIdSubAzienda.Text = "-1"
        txtCodice.Text = ""
        txtCap.Text = ""
        txtIndirizzo.Text = ""
        txtPiva.Text = ""
        txtIndirizzo.Text = ""
        Me.CaricaRegione()
        Me.CaricaProvincia()
        'Me.CaricaComune()
        ddlComune.Text = ""
        Me.CaricaOrganizzazioni()
        lblConferma.Text = ""
        txtRagSociale.Text = ""
    End Sub

    Private Sub InserisciCodice()
        Dim idmax
        Dim sl As String = "select * from Azienda where id=" & ddlAzienda.SelectedValue
        Dim tb As DataTable = Me.MyGest.GetTab(sl)
        Dim sql As String = "select Max(SubAzienda.id),SubAzienda.codice,Azienda.codice as cod from SubAzienda inner join Azienda on Azienda.id=SubAzienda.idAzienda where Azienda.id=" & ddlAzienda.SelectedValue & " group by SubAzienda.id,SubAzienda.codice,Azienda.codice "
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            idmax = CInt(tab.Rows(0).Item("codice").ToString.Split(".")(1).Substring(2))
        Else
            idmax = 0
        End If
        If tb.Rows.Count > 0 Then
            idmax = tb.Rows(0).Item("codice") & ".SO" & idmax + 1
        End If
        txtCodice.Text = idmax
    End Sub

    Private Sub AbilitaCampi()
        txtCap.ReadOnly = False
        'txtCodice.ReadOnly = False
        ddlComune.Enabled = True
        txtIndirizzo.ReadOnly = False
        txtPiva.ReadOnly = False
        ddlProvincia.Enabled = True
        txtRagSociale.ReadOnly = False
        ddlRegione.Enabled = True
        ddlAzienda.Enabled = True
        txtRicercaSubOrganizzazioni.ReadOnly = False
        btnModifica.Visible = False
        'btnCarica.Visible = True
        'FileUpload1.Visible = True
        btnSalva.Visible = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCap.ReadOnly = True
        'txtCodice.ReadOnly = True
        ddlComune.Enabled = False
        txtIndirizzo.ReadOnly = True
        txtPiva.ReadOnly = True
        ddlProvincia.Enabled = False
        txtRagSociale.ReadOnly = True
        ddlRegione.Enabled = False
        ddlAzienda.Enabled = False
        txtRicercaSubOrganizzazioni.ReadOnly = True
        btnModifica.Visible = True
        'btnCarica.Visible = False
        'FileUpload1.Visible = False
        btnSalva.Visible = False
    End Sub

    'Protected Sub btnCarica_Click(sender As Object, e As EventArgs) Handles btnCarica.Click
    '    Me.MySubAzienda = New SubAzienda
    '    Me.MySubAzienda.Load(lblIdAzienda.Text)
    '    If (FileUpload1.HasFile) Then
    '        Dim thefilename As String = ""
    '        Dim nome As String = MySubAzienda.Descrizione.Replace(".", "").Replace("'", "")
    '        If Directory.Exists(Server.MapPath("\logo\" & nome & "\")) = False Then
    '            '    ' ...Creo la cartella al percorso specificato
    '            Directory.CreateDirectory(Server.MapPath("\logo\" & nome & "\"))
    '        End If
    '        If Not File.Exists(Server.MapPath("\logo\" & nome & "\" & FileUpload1.FileName)) Then
    '            thefilename = FileUpload1.FileName
    '        Else
    '            thefilename = Now.Date.ToString("dd/MM/yyyy").Replace("/", "") & FileUpload1.FileName
    '        End If
    '        Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & nome & "\" & thefilename))
    '        'Me.FileUpload1.SaveAs(Server.MapPath("\logo\" & FileUpload1.FileName))
    '        imgLogo.ImageUrl = "\logo\" & nome & "\" & thefilename
    '        'imgLogo.ImageUrl = "\logo\" & FileUpload1.FileName
    '        txtLogo.Text = thefilename
    '        Me.MySubAzienda.Logo = txtLogo.Text
    '        Me.MySubAzienda.SalvaData()
    '    End If
    '    FileUpload1 = New FileUpload
    'End Sub

    Protected Sub ddlAzienda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAzienda.SelectedIndexChanged
        Me.InserisciCodice()
    End Sub

#Region "Rubrica"
    Private Sub CaricaTipologiaContatto(Optional ByVal idtc As String = "-1")
        Dim sql As String = "Select descrizione,id from TipoContatto"

        If idtc <> "-1" Then
            sql = sql & " where TipoContatto.id=" & idtc
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTipologiaContatto.DataSource = tab
        Me.ddlTipologiaContatto.DataTextField = "descrizione"
        Me.ddlTipologiaContatto.DataValueField = "id"
        Me.ddlTipologiaContatto.DataBind()
        If idtc <> "-1" Then
            Me.ddlTipologiaContatto.SelectedValue = idtc
        Else
            Me.ddlTipologiaContatto.SelectedValue = "-1"
        End If
    End Sub

    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
        Me.MyRecapito = New Recapito

        Try
            Me.MyRecapito.ID = lblIdRecapito.Text
            Me.MyRecapito.Contatto = txtContatto.Text
            Me.MyRecapito.Descrizione = txtDescrizioneContatto.Text
            Me.MyRecapito.IDTipo = ddlTipologiaContatto.SelectedValue
            Me.MyRecapito.SalvaData()
            If lblIdRecapito.Text = "-1" Then
                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdSubAzienda.Text & "'," & Me.MyRecapito.ID & ",'SubOrganizzazione')"
                MyGest.GetTab(sql)
            End If

        Catch
        End Try
        Me.CaricaContatti()
        Me.SvuotaCampiRecapito()
    End Sub
    Private Sub SvuotaCampiRecapito()
        lblIdRecapito.Text = "-1"
        txtContatto.Text = ""
        txtDescrizioneContatto.Text = ""
        btnElimina.Visible = False
        lblSicuro.Visible = False
        CaricaTipologiaContatto()
    End Sub

    Private Sub CaricaContatti()
        Dim tab As New DataTable

        Dim sqlStr = "SELECT *,TipoContatto.descrizione as descr " & _
                     "FROM Recapito inner join Recapito_Utente on Recapito_Utente.idRecapito=Recapito.id " & _
                     "inner join SubAzienda on SubAzienda.id=Recapito_Utente.idUtente " & _
                     "inner join TipoContatto on TipoContatto.id=Recapito.idtipo " & _
                     "where Recapito_Utente.idUtente = " & lblIdSubAzienda.Text & " and Recapito_Utente.tipoAssociazione like 'SubOrganizzazione' "

        tab = MyGest.GetTab(sqlStr)

        ListView2.DataSource = tab
        ListView2.DataBind()

        If tab.Rows.Count > 10 Then
            DataPager2.Visible = True
        Else
            DataPager2.Visible = False
        End If
    End Sub

    Private Sub ListView2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView2.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaRecapito

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaRecapito

    End Sub

    Private Sub CancellaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyRecapito = New Recapito
        lblIdRecapito.Text = bt.AlternateText
        Me.MyRecapito.Load(lblIdRecapito.Text)
        txtContatto.Text = Me.MyRecapito.Contatto
        txtDescrizioneContatto.Text = Me.MyRecapito.Descrizione

        ddlTipologiaContatto.SelectedValue = Me.MyRecapito.IDTipo
        btnMemorizza.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True
        btnAnnulla1.Visible = False
        btnAnnullaContatto.Visible = True

    End Sub

    Private Sub ModificaRecapito(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyRecapito = New Recapito
        lblIdRecapito.Text = bt.AlternateText
        Me.MyRecapito.Load(lblIdRecapito.Text)
        txtContatto.Text = Me.MyRecapito.Contatto
        txtDescrizioneContatto.Text = Me.MyRecapito.Descrizione
        ddlTipologiaContatto.SelectedValue = Me.MyRecapito.IDTipo
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        btnAnnulla.Visible = True
        lblSicuro.Visible = False
    End Sub

#End Region

    
    
    
    
    Protected Sub btnAnnulla1_Click(sender As Object, e As EventArgs) Handles btnAnnulla1.Click
        Me.SvuotaCampiRecapito()
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        lblSicuro.Visible = False
    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
        Me.MyRecapito = New Recapito
        Me.MyRecapito.Delete(lblIdRecapito.Text)
        Me.SvuotaCampiRecapito()
        Me.CaricaContatti()
        Dim message As String = "Contatto eliminato correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        btnAnnulla1.Visible = True
        btnAnnullaContatto.Visible = False
        btnElimina.Visible = False
        lblSicuro.Visible = False
        btnMemorizza.Visible = True
        lblIdRecapito.Text = "-1"
    End Sub



    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaOrg.Visible = False
        PanelRicercaSubOrganizzazioni.Visible = True
        lblIdSubAzienda.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MySubAzienda = New SubAzienda
        If Me.MySubAzienda.Delete(lblIdSubAzienda.Text) Then
            Me.CaricaSubOrganizzazioni()
            lblIdSubAzienda.Text = "-1"
            PanelEliminaOrg.Visible = False
            PanelRicercaSubOrganizzazioni.Visible = True
            Dim message As String = "SubOrganizzazione eliminata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        PanelRecapiti.Visible = False
        lbRecapiti.Visible = False
        PanelModificaOrganizzazione.Visible = True
        Panel1.Visible = False
        Me.AbilitaCampi()
        Me.CaricaRegione()
        Me.CaricaOrganizzazioni()
        Me.SvuotaCampi()
    End Sub


    Protected Sub btnAnnullaContatto_Click(sender As Object, e As EventArgs) Handles btnAnnullaContatto.Click
        Me.SvuotaCampiRecapito()
        lblSicuro.Visible = False
        btnElimina.Visible = False
        btnAnnullaContatto.Visible = False
        btnMemorizza.Visible = True
        btnAnnulla1.Visible = True
    End Sub

    Protected Sub lbRecapiti_Click(sender As Object, e As EventArgs) Handles lbRecapiti.Click
        If PanelRecapiti.Visible = True Then
            PanelRecapiti.Visible = False
        Else
            PanelRecapiti.Visible = True
            Me.CaricaContatti()
        End If
    End Sub
End Class