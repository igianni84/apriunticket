Public Class listino1
    Inherits System.Web.UI.Page

#Region "Declare"
    Private MyGest As MNGestione
    Private MyCGlob As CGlobal
    Private MyUtente As Utente
    Private MyListino As Listino
    Private MyTariffa As TariffaLis

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        Me.Form.Enctype = "multipart/form-data"
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                If Not IsPostBack Then
                    Panel1.Visible = True
                    PanelRicercaListino.Visible = True
                    Me.CaricaListino()


                End If
            End If
        Else
            Response.Redirect("~/login.aspx")
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

    Private Sub CaricaTariffazione(Optional ByVal idt As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select tariffazione,id,idazienda from Tariffazione"
        If idorg <> "-1" Then
            str = str & " where idazienda=" & idorg
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "tipo tariffazione"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlTariffazione.DataSource = tab
        Me.ddlTariffazione.DataTextField = "tariffazione"
        Me.ddlTariffazione.DataValueField = "id"
        Me.ddlTariffazione.DataBind()
        If idt <> "-1" Then
            Me.ddlTariffazione.SelectedValue = idt
        Else
            Me.ddlTariffazione.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaMisura(Optional ByVal idm As String = "-1", Optional ByVal idorg As String = "-1")
        Dim str As String = "select misura,id,idazienda from Misura"
        If idorg <> "-1" Then
            str = str & " where idazienda=" & idorg
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "tipo misura"
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlMisura.DataSource = tab
        Me.ddlMisura.DataTextField = "misura"
        Me.ddlMisura.DataValueField = "id"
        Me.ddlMisura.DataBind()
        If idm <> "-1" Then
            Me.ddlMisura.SelectedValue = idm
        Else
            Me.ddlMisura.SelectedValue = "-1"
        End If
    End Sub

    Private Sub ListView1_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView1.PagePropertiesChanging
        Me.DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaListino(txtRicercaListino.Text)
    End Sub

    Private Sub ListView2_PagePropertiesChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.PagePropertiesChangingEventArgs) Handles ListView2.PagePropertiesChanging
        Me.DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        Me.CaricaTariffe()
    End Sub


    Private Sub CaricaListino(Optional ByVal descrizione As String = "")
        lblSicuro.Visible = False
        Dim tab As New DataTable
        Dim sqlStr As String
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.SubAzienda = New SubAzienda
        Me.MyUtente.Load(Session("id"))
        If Me.MyUtente.SubAzienda.ID = "-1" Then
            sqlStr = "SELECT * " & _
                     "FROM Listino where Listino.idazienda= " & Me.MyUtente.Azienda.ID
        Else
            sqlStr = "SELECT * " & _
                     "FROM Listino where Listino.idazienda= " & Me.MyUtente.SubAzienda.ID
        End If
        If descrizione <> "" Then
            sqlStr = sqlStr & "and Listino.descrizione like '%" & descrizione & "%'"
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

        Dim btn2 As ImageButton = e.Item.FindControl("imgMostra")
        AddHandler btn2.Click, AddressOf ModificaSubOrganizzazione
    End Sub

    Private Sub ModificaSubOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyListino = New Listino
        lblIdListino.Text = bt.AlternateText
        Me.MyListino.Load(lblIdListino.Text)
        txtCodice.Text = Me.MyListino.Codice
        txtDescrizione.Text = Me.MyListino.Descrizione
        txtCodEsterno.Text = Me.MyListino.CodEsterno
        
        Panel1.Visible = False
        PanelModificaListino.Visible = True
        PanelRicercaListino.Visible = False
        Me.DisabilitaCampi()
        Me.CaricaTariffe()
        Me.CaricaTariffazione(, Me.RestituisciOrganizzazione())
        Me.CaricaMisura(, Me.RestituisciOrganizzazione())
        PanelTariffe.Visible = True
        'Me.CaricaTipologiaContatto()
        'Me.CaricaContatti()
    End Sub

    Private Sub CaricaTariffe()
        Me.MyCGlob = New CGlobal
        Dim tab As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM Tariffa inner join Tariffazione on Tariffazione.id=Tariffa.idtariffazione " & _
                     "inner join Misura on Misura.id=Tariffa.idmisura " & _
                     "where Tariffa.idlistino = " & lblIdListino.Text

        tab = MyGest.GetTab(sqlStr)




        Dim tstruct As New DataTable
        Dim c1 As New DataColumn("id", GetType(String), "")
        tstruct.Columns.Add(c1)
        Dim c2 As New DataColumn("tariffazione", GetType(String), "")
        tstruct.Columns.Add(c2)
        Dim c3 As New DataColumn("prezzounitario", GetType(String), "")
        tstruct.Columns.Add(c3)
        Dim c4 As New DataColumn("dirittochiamata", GetType(String), "")
        tstruct.Columns.Add(c4)
        Dim c5 As New DataColumn("prezzoextra", GetType(String), "")
        tstruct.Columns.Add(c5)
        Dim c6 As New DataColumn("percextra", GetType(String), "")
        tstruct.Columns.Add(c6)
        Dim c7 As New DataColumn("misura", GetType(String), "")
        tstruct.Columns.Add(c7)
        Dim c8 As New DataColumn("costo", GetType(String), "")
        tstruct.Columns.Add(c8)
        
        For i As Integer = 0 To tab.Rows.Count - 1
            tstruct.Rows.Add(i)
            tstruct.Rows(i)("tariffazione") = tab.Rows(i)("tariffazione")
            tstruct.Rows(i)("id") = tab.Rows(i)("id")
            tstruct.Rows(i)("prezzounitario") = Me.MyCGlob.GestNum(tab.Rows(i)("prezzounitario"))
            tstruct.Rows(i)("dirittochiamata") = Me.MyCGlob.GestNum(tab.Rows(i)("dirittochiamata"))
            If tab.Rows(i)("percextra") = "" Then
                tstruct.Rows(i)("percextra") = "0"
            Else
                tstruct.Rows(i)("percextra") = tab.Rows(i)("percextra")
            End If
            tstruct.Rows(i)("prezzoextra") = Me.MyCGlob.GestNum(tab.Rows(i)("prezzoextra"))
            

            tstruct.Rows(i)("misura") = tab.Rows(i)("misura")
            tstruct.Rows(i)("costo") = Me.MyCGlob.GestNum(tab.Rows(i)("costo"))

            
        Next





        ListView2.DataSource = tstruct
        ListView2.DataBind()

        If tab.Rows.Count > 5 Then
            DataPager2.Visible = True
        Else
            DataPager2.Visible = False
        End If
    End Sub

    Private Sub CancellaSubOrganizzazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        lblIdListino.Text = bt.AlternateText
        PanelEliminaLis.Visible = True
        PanelRicercaListino.Visible = False
        Me.MyListino = New Listino
        Me.MyListino.Load(bt.AlternateText)
        lblCodiceElimina.Text = Me.MyListino.Codice
        lblListinoElimina.Text = Me.MyListino.Descrizione
        lblCodEsternoElimina.Text = Me.MyListino.CodEsterno
    End Sub

    Protected Sub btnModifica_Click(sender As Object, e As EventArgs) Handles btnModifica.Click
        Me.AbilitaCampi()
    End Sub

    Protected Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        Panel1.Visible = True
        PanelModificaListino.Visible = False
        PanelRicercaListino.Visible = True
        PanelTariffe.Visible = False
        Me.CaricaListino()

    End Sub

    Protected Sub btnRicercaListino_Click(sender As Object, e As EventArgs) Handles btnRicercaListino.Click
        Me.CaricaListino(txtRicercaListino.Text)
        Panel1.Visible = True
        PanelModificaListino.Visible = False

    End Sub

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If IsValid Then

            Me.MyListino = New Listino
            Me.MyListino.Azienda = New Azienda
            Me.MyListino.SubAzienda = New SubAzienda
            Me.MyListino.Load(lblIdListino.Text)
            Me.MyListino.Codice = txtCodice.Text
            Me.MyListino.CodEsterno = txtCodEsterno.Text
            Me.MyListino.Descrizione = txtDescrizione.Text

            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.SubAzienda = New SubAzienda

            Me.MyUtente.Load(Session("id"))
            If Me.MyUtente.SubAzienda.ID = "-1" Then
                Me.MyListino.Azienda.ID = Me.MyUtente.Azienda.ID
                Me.MyListino.SubAzienda.ID = "-1"
            Else
                Me.MyListino.Azienda.ID = Me.MyUtente.Azienda.ID
                Me.MyListino.SubAzienda.ID = Me.MyUtente.SubAzienda.ID
            End If
            Me.MyListino.SalvaData()

            lblIdListino.Text = Me.MyListino.ID
            lblConferma.Visible = True
            'lblConferma.Text = "SubOrganizzazione Creata"
            '

            Dim message2 As String = "Listino creato correttamente. Ora puoi inserire le tariffe"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            'Threading.Thread.Sleep(5000)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
            Me.CaricaListino()
            Panel1.Visible = False
            PanelRicercaListino.Visible = False
            PanelModificaListino.Visible = True
            PanelTariffe.Visible = True

            Me.CaricaTariffe()
        Else
            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        End If
    End Sub

   



    Private Sub SvuotaCampi()
        lblIdListino.Text = "-1"
        txtCodice.Text = ""
        txtCodEsterno.Text = ""
        txtDescrizione.Text = ""
        lblConferma.Text = ""

    End Sub

    Private Sub InserisciCodice()
        Dim idmax
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.SubAzienda = New SubAzienda

        Me.MyUtente.Load(Session("id"))
        

        Dim sql As String = "select Listino.id,Listino.codice from Listino "
        If Me.MyUtente.SubAzienda.ID = "-1" Then
            sql = sql & "where Listino.idazienda=" & Me.MyUtente.Azienda.ID & " group by Listino.id,Listino.codice "
        Else
            sql = sql & "where Listino.idsubazienda=" & Me.MyUtente.SubAzienda.ID & " group by Listino.id,Listino.codice "
        End If
        Dim tab As DataTable = Me.MyGest.GetTab(sql)
        If tab.Rows.Count > 0 Then
            idmax = CInt(tab.Rows(tab.Rows.Count - 1).Item("codice").ToString.Substring(1))
        Else
            idmax = 0
        End If

        idmax = "L" & idmax + 1

        txtCodice.Text = idmax
    End Sub

    Private Sub AbilitaCampi()
        txtDescrizione.ReadOnly = False
        txtCodEsterno.ReadOnly = False
        
        txtRicercaListino.ReadOnly = False
        btnModifica.Visible = False
        'btnCarica.Visible = True
        'FileUpload1.Visible = True
        btnSalva.Visible = True
    End Sub

    Private Sub DisabilitaCampi()
        txtCodice.ReadOnly = True

        txtDescrizione.ReadOnly = True
        txtCodEsterno.ReadOnly = True
        txtRicercaListino.ReadOnly = True
        btnModifica.Visible = True
       
        btnSalva.Visible = False
    End Sub

   
   

    '#Region "Rubrica"
    '    Private Sub CaricaTipologiaContatto(Optional ByVal idtc As String = "-1")
    '        Dim sql As String = "Select descrizione,id from TipoContatto"

    '        If idtc <> "-1" Then
    '            sql = sql & " where TipoContatto.id=" & idtc
    '        End If
    '        Dim tab As DataTable
    '        tab = Me.MyGest.GetTab(sql)
    '        Dim row As DataRow = tab.NewRow
    '        row(0) = "..."
    '        row(1) = "-1"
    '        tab.Rows.Add(row)

    '        Me.ddlTipologiaContatto.DataSource = tab
    '        Me.ddlTipologiaContatto.DataTextField = "descrizione"
    '        Me.ddlTipologiaContatto.DataValueField = "id"
    '        Me.ddlTipologiaContatto.DataBind()
    '        If idtc <> "-1" Then
    '            Me.ddlTipologiaContatto.SelectedValue = idtc
    '        Else
    '            Me.ddlTipologiaContatto.SelectedValue = "-1"
    '        End If
    '    End Sub

    '    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
    '        Me.MyRecapito = New Recapito

    '        Try
    '            Me.MyRecapito.ID = lblIdRecapito.Text
    '            Me.MyRecapito.Contatto = txtContatto.Text
    '            Me.MyRecapito.IDTipo = ddlTipologiaContatto.SelectedValue
    '            Me.MyRecapito.SalvaData()
    '            If lblIdRecapito.Text = "-1" Then
    '                Dim sql As String = "Insert Into Recapito_Utente values('" & lblIdSubAzienda.Text & "'," & Me.MyRecapito.ID & ",'SubOrganizzazione')"
    '                MyGest.GetTab(sql)
    '            End If

    '        Catch
    '        End Try
    '        Me.CaricaContatti()
    '        Me.SvuotaCampiRecapito()
    '    End Sub
    '    Private Sub SvuotaCampiRecapito()
    '        lblIdRecapito.Text = "-1"
    '        txtContatto.Text = ""
    '        btnElimina.Visible = False
    '        lblSicuro.Visible = False
    '        CaricaTipologiaContatto()
    '    End Sub

    '    Private Sub CaricaContatti()
    '        Dim tab As New DataTable

    '        Dim sqlStr = "SELECT *,TipoContatto.descrizione as descr " & _
    '                     "FROM Recapito inner join Recapito_Utente on Recapito_Utente.idRecapito=Recapito.id " & _
    '                     "inner join SubAzienda on SubAzienda.id=Recapito_Utente.idUtente " & _
    '                     "inner join TipoContatto on TipoContatto.id=Recapito.idtipo " & _
    '                     "where Recapito_Utente.idUtente = " & lblIdSubAzienda.Text & " and Recapito_Utente.tipoAssociazione like 'SubOrganizzazione' "

    '        tab = MyGest.GetTab(sqlStr)

    '        ListView2.DataSource = tab
    '        ListView2.DataBind()

    '        If tab.Rows.Count > 10 Then
    '            DataPager2.Visible = True
    '        Else
    '            DataPager2.Visible = False
    '        End If
    '    End Sub

    Private Sub ListView2_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListView2.ItemCreated
        Dim btn As ImageButton = e.Item.FindControl("imgCancella")
        AddHandler btn.Click, AddressOf CancellaTariffa

        Dim btn1 As ImageButton = e.Item.FindControl("imgModifica")
        AddHandler btn1.Click, AddressOf ModificaTariffa

    End Sub

    Private Sub CancellaTariffa(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyTariffa = New TariffaLis
        Me.MyTariffa.Misura = New Misura
        Me.MyTariffa.Tariffazione = New Tariffa
        lblIdTariffa.Text = bt.AlternateText
        Me.MyTariffa.Load(lblIdtariffa.Text)
        txtPrezzoUnitario.Text = Me.MyTariffa.PrezzoUnitario
        txtDirittoChiamata.Text = Me.MyTariffa.DirittoChiamata
        txtPrezzoExtra.Text = Me.MyTariffa.PrezzoExtra
        txtPercExtra.Text = Me.MyTariffa.PercExtra
        txtCosto.Text = Me.MyTariffa.Costo
        Me.CaricaTariffazione(Me.MyTariffa.Tariffazione.ID, Me.RestituisciOrganizzazione)
        Me.CaricaMisura(Me.MyTariffa.Misura.ID, Me.RestituisciOrganizzazione)
        btnMemorizza.Visible = False
        lblSicuro.Visible = True
        btnElimina.Visible = True
        btnAnnulla1.Visible = False
        btnAnnullaTariffa.Visible = True

    End Sub

    Private Sub ModificaTariffa(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim bt As ImageButton = CType(sender, ImageButton)
        Me.MyTariffa = New TariffaLis
        Me.MyTariffa.Misura = New Misura
        Me.MyTariffa.Tariffazione = New Tariffa
        lblIdTariffa.Text = bt.AlternateText
        Me.MyTariffa.Load(lblIdTariffa.Text)
        txtPrezzoUnitario.Text = Me.MyTariffa.PrezzoUnitario
        txtDirittoChiamata.Text = Me.MyTariffa.DirittoChiamata
        txtPrezzoExtra.Text = Me.MyTariffa.PrezzoExtra
        txtPercExtra.Text = Me.MyTariffa.PercExtra.Replace("%", "")
        txtCosto.Text = Me.MyTariffa.Costo
        Me.CaricaTariffazione(Me.MyTariffa.Tariffazione.ID, Me.RestituisciOrganizzazione)
        Me.CaricaMisura(Me.MyTariffa.Misura.ID, Me.RestituisciOrganizzazione)
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaTariffa.Visible = False
        btnAnnulla.Visible = True
        lblSicuro.Visible = False
    End Sub

    '#End Region





    Protected Sub btnAnnulla1_Click(sender As Object, e As EventArgs) Handles btnAnnulla1.Click
        'Me.SvuotaCampiRecapito()
        Me.PulisciCampiTariffe()
        btnMemorizza.Visible = True
        btnElimina.Visible = False
        btnAnnullaTariffa.Visible = False
        lblSicuro.Visible = False
    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
        Me.MyTariffa = New TariffaLis
        Me.MyTariffa.Delete(lblIdTariffa.Text)
        Me.PulisciCampiTariffe()
        'Me.SvuotaCampiRecapito()
        ' Me.CaricaContatti()
        Dim message As String = "Tariffa eliminata correttamente"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)

        btnAnnulla1.Visible = True
        btnAnnullaTariffa.Visible = False
        btnElimina.Visible = False
        lblSicuro.Visible = False
        btnMemorizza.Visible = True
    End Sub

    Protected Sub btnAnnullaCanc_Click(sender As Object, e As EventArgs) Handles btnAnnullaCanc.Click
        PanelEliminaLis.Visible = False
        PanelRicercaListino.Visible = True
        lblIdListino.Text = "-1"
    End Sub

    Protected Sub btnConferma_Click(sender As Object, e As EventArgs) Handles btnConferma.Click
        Me.MyListino = New Listino
        If Me.MyListino.Delete(lblIdListino.Text) Then
            Me.CaricaListino()
            lblIdListino.Text = "-1"
            PanelEliminaLis.Visible = False
            PanelRicercaListino.Visible = True
            Dim message As String = "Listino eliminata correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Protected Sub btNuovo_Click(sender As Object, e As EventArgs) Handles btNuovo.Click
        PanelModificaListino.Visible = True
        Panel1.Visible = False
        Me.AbilitaCampi()
        Me.SvuotaCampi()
        Me.InserisciCodice()
        PanelTariffe.Visible = False
    End Sub

    Protected Sub btnMemorizza_Click(sender As Object, e As EventArgs) Handles btnMemorizza.Click
        If Me.IsValid Then
            Me.MyTariffa = New TariffaLis
            Me.MyTariffa.Listino = New Listino
            Me.MyTariffa.Misura = New Misura
            Me.MyTariffa.Tariffazione = New Tariffa
            If (Not IsNumeric(txtPercExtra.Text) And txtPercExtra.Text <> "") Or (Not IsNumeric(txtCosto.Text) And txtCosto.Text <> "") Or (Not IsNumeric(txtDirittoChiamata.Text) And txtDirittoChiamata.Text <> "") Or (Not IsNumeric(txtPrezzoUnitario.Text) And txtPrezzoUnitario.Text <> "") Or (Not IsNumeric(txtPrezzoExtra.Text) And txtPrezzoExtra.Text <> "") Then
                Dim message As String = "Inserire un valore numerico"
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                Exit Sub
            End If
            Me.AdattaCampiEuro()
            Me.MyTariffa.ID = lblIdTariffa.Text
            Me.MyTariffa.Costo = txtCosto.Text
            Me.MyTariffa.DirittoChiamata = txtDirittoChiamata.Text
            Me.MyTariffa.Listino.ID = lblIdListino.Text
            Me.MyTariffa.Misura.ID = ddlMisura.SelectedValue
            Me.MyTariffa.PercExtra = txtPercExtra.Text
            Me.MyTariffa.PrezzoUnitario = txtPrezzoUnitario.Text
            Me.MyTariffa.PrezzoExtra = txtPrezzoExtra.Text
            Me.MyTariffa.Tariffazione.ID = ddlTariffazione.SelectedValue
            Me.MyTariffa.SalvaData()
            Me.CaricaTariffe()
            Me.PulisciCampiTariffe()
            Dim message2 As String = "Tariffa inserita correttamente"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
        Else
            Dim message As String = "Inserire i campi obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If

    End Sub

    Private Sub AdattaCampiEuro()
        Me.MyCGlob = New CGlobal
        txtCosto.Text = Me.MyCGlob.GestNum(txtCosto.Text)
        txtDirittoChiamata.Text = Me.MyCGlob.GestNum(txtDirittoChiamata.Text)
        If Not txtPercExtra.Text.Contains("%") Then
            txtPercExtra.Text = txtPercExtra.Text & "%"
        End If
        txtPrezzoExtra.Text = Me.MyCGlob.GestNum(txtPrezzoExtra.Text)
        txtPrezzoUnitario.Text = Me.MyCGlob.GestNum(txtPrezzoUnitario.Text)
    End Sub

    Private Sub PulisciCampiTariffe()
        Me.CaricaTariffazione(, Me.RestituisciOrganizzazione)
        txtPrezzoUnitario.Text = ""
        txtDirittoChiamata.Text = ""
        txtPrezzoExtra.Text = ""
        txtPercExtra.Text = ""
        Me.CaricaMisura(, Me.RestituisciOrganizzazione)
        txtCosto.Text = ""
        lblIdTariffa.Text = "-1"
    End Sub

    Protected Sub btnAnnullaTariffa_Click(sender As Object, e As EventArgs) Handles btnAnnullaTariffa.Click
        Me.PulisciCampiTariffe()
        lblSicuro.Visible = False
        btnElimina.Visible = False
        btnAnnullaTariffa.Visible = False
        btnMemorizza.Visible = True
        btnAnnulla1.Visible = True
    End Sub
End Class