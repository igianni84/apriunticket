Public Class apriticket
    Inherits System.Web.UI.Page
#Region "Declare"
    Private MyGest As MNGestione
    Private MyUtente As Utente
    Private MyCliente As Cliente
    Private MySubCliente As SubCliente
    Private MyTickets As Tickets
    Private MyContratto As Contratto
    Private MyInventario As Inventari
    Private MyDispositivo As TipoDispositivo
    Private MyMarchio As Marchio
    Private MyModello As Modello
    Private MyFitMail As FitMail
    Private MyCGlobal As CGlobal
    Shared tipo(10) As TextBox
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyGest = New MNGestione(CGlobal.cs)
        MyGest.Connetti()
        If Not IsNothing(Session("id")) And (Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Or Session("tipoutente") = "SuperAdmin") Then
            If Session("id") = -1 Then

                Response.Redirect("~/login.aspx")
            Else
                'btnApri.Attributes.Add("onclick", "UpdateTextArea()")
                Me.AbilitaAggiungiRefresh()

                If Not IsPostBack Then
                    Me.CaricaClienti()
                    'txtDataAp.Text = Date.Now.Date.ToString("yyyy-MM-dd") '"1984-12-10" '
                    'txtOraAp.Text = DateTime.Now.ToString("HH:mm") '& ":" & Date.Now.Minute
                    'lblDataAp.Text = txtDataAp.Text '"1984-12-10" '
                    'lblOraAp.Text = txtOraAp.Text
                    'lblDataAp2.Text = txtDataAp.Text '"1984-12-10" '
                    'lblOraAp2.Text = txtOraAp.Text

                    Dim app = Request.Url.ToString.Split("?")
                    Dim miaStringaCriptata As String
                    If app.Length > 1 Then
                        miaStringaCriptata = VSTripleDES.DecryptData(Request.Url.ToString.Split("?")(1))
                        Dim id = miaStringaCriptata.Split("&")(0).ToString.Split("=")(1)
                        Dim tipo = miaStringaCriptata.Split("&")(1).ToString.Split("=")(1)
                        Select Case tipo
                            Case "cliente"
                                Me.CaricaClienti(id)
                                ddlCliente_SelectedIndexChanged(sender, e)
                            Case "subcliente"
                                Me.CaricaClienti(Me.RestituisciCliente(id))
                                ddlCliente_SelectedIndexChanged(sender, e)
                                Me.CaricaSubClienti(Me.RestituisciCliente(id), id)
                                ddlSubCliente_SelectedIndexChanged(sender, e)
                            Case "inventario"
                                Me.CaricaClienti(Me.RestituisciClienteDaInv(id))
                                ddlCliente_SelectedIndexChanged(sender, e)
                                Me.CaricaSubClienti(Me.RestituisciClienteDaInv(id), Me.RestituisciSubClienteDaInv(id))
                                ddlSubCliente_SelectedIndexChanged(sender, e)
                                Me.MyUtente = New Utente
                                Me.MyUtente.SubCliente = New SubCliente
                                Me.MyUtente.Load(Session("id"))
                                If Me.VerificaMobile(id) Then
                                    ddlSubCliente.SelectedValue = Me.MyUtente.SubCliente.ID
                                End If
                                Me.CaricaInventario(id)
                                ddlInventario_SelectedIndexChanged(sender, e)
                                Me.CaricaPerContoDi(Me.RestituisciClienteDaInv(id), Me.RestituisciSubClienteDaInv(id), Me.RestituisciPerContoDiDaInv(id))
                        End Select
                    ElseIf Session("tipoutente") = "Utente" Then
                        Dim id = Session("id")
                        Dim tipo = ""
                        Me.MyUtente = New Utente
                        Me.MyUtente.SubCliente = New SubCliente
                        Me.MyUtente.Cliente = New Cliente
                        Me.MyUtente.Load(id)
                        If Me.MyUtente.SubCliente.ID <> "-1" Then
                            tipo = "subcliente"
                        Else
                            tipo = "cliente"
                        End If

                        Select Case tipo
                            Case "cliente"
                                Me.CaricaClienti(Me.MyUtente.Cliente.ID)
                                ddlCliente_SelectedIndexChanged(sender, e)
                                'Me.CaricaUtentiCliente(Me.MyUtente.Cliente.ID, , Session("id"))
                                ddlUtente.SelectedValue = Session("id")
                            Case "subcliente"
                                Me.CaricaClienti(Me.RestituisciCliente(Me.MyUtente.SubCliente.ID))
                                ddlCliente_SelectedIndexChanged(sender, e)
                                Me.CaricaSubClienti(Me.RestituisciCliente(Me.MyUtente.SubCliente.ID), Me.MyUtente.SubCliente.ID)
                                ddlSubCliente_SelectedIndexChanged(sender, e)
                                Me.CaricaInventario()
                                'Me.CaricaUtentiCliente(Me.MyUtente.Cliente.ID, Me.MyUtente.SubCliente.ID, Session("id"))
                                'ddlUtente.SelectedValue = Session("id")
                        End Select

                    End If
                End If
                End If
        Else
                Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Function VerificaMobile(ByVal id As String)
        Me.MyInventario = New Inventari
        Me.MyInventario.Load(id)
        If Me.MyInventario.Mobile = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function RestituisciPerContoDiDaInv(ByVal idinventario As String)
        Me.MyInventario = New Inventari
        Me.MyInventario.Utente = New Utente
        Me.MyInventario.Load(idinventario)
        Return Me.MyInventario.Utente.ID
    End Function

    Private Function RestituisciClienteDaInv(ByVal idinventario As String)
        Me.MyInventario = New Inventari
        Me.MyInventario.Cliente = New Cliente
        Me.MyInventario.Load(idinventario)
        Return Me.MyInventario.Cliente.ID
    End Function

    Private Function RestituisciSubClienteDaInv(ByVal idinventario As String)
        Me.MyInventario = New Inventari
        Me.MyInventario.SubCliente = New SubCliente
        Me.MyInventario.Load(idinventario)
        Return Me.MyInventario.SubCliente.ID
    End Function

    Private Function RestituisciCliente(ByVal idsubcliente As String)
        Me.MySubCliente = New SubCliente
        Me.MySubCliente.Cliente = New Cliente
        Me.MySubCliente.Load(idsubcliente)
        Return Me.MySubCliente.Cliente.ID
    End Function

    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        If VerificaValiditaContratto() = True Then
            Me.CaricaSubClienti(ddlCliente.SelectedValue)
            Me.CaricaUtentiCliente()
            Me.CaricaContrattiListini()
            Me.CaricaPerContoDi()
            'txtCliente21.Text = ddlCliente.SelectedItem.Text
            'txtCliente31.Text = ddlCliente.SelectedItem.Text
            txtScadenza.Text = ""
            'txtCliente41.Text = ddlCliente.SelectedItem.Text
            Me.CaricaInventario()
        End If
    End Sub

    Function VerificaValiditaContratto() As Boolean
        Dim ret As Boolean = True
        If Session("tipoutente") = "Operatore" Then
            Me.MyCGlobal = New CGlobal
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
                If Me.MyCGlobal.VerificaBloccoCliente(ddlSubCliente.SelectedValue, "SubCliente") Then
                    ret = True
                Else
                    ret = False
                    Dim message As String = "SubCliente bloccato. Contratto scaduto o esaurite le ore a disposizione "
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopupRedirect('" + message + "');", True)
                End If
            Else
                If Me.MyCGlobal.VerificaBloccoCliente(ddlCliente.SelectedValue, "Cliente") Then
                Else
                    ret = False
                    Dim message As String = "Cliente bloccato.  Contratto scaduto o esaurite le ore a disposizione "
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopupRedirect('" + message + "');", True)
                End If
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyCGlobal = New CGlobal
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
                If Me.MyCGlobal.VerificaBloccoCliente(ddlSubCliente.SelectedValue, "SubCliente") Then
                    ret = True
                Else
                    ret = False
                    Dim message As String = "SubCliente bloccato. Contratto scaduto o esaurite le ore a disposizione "
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopupRedirect('" + message + "');", True)
                End If
            End If
            End If
            Return ret
    End Function

    Protected Sub ddlSubCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubCliente.SelectedIndexChanged
        If VerificaValiditaContratto() = True Then
            Me.CaricaUtentiCliente()
            Me.CaricaContrattiListini()
            Me.CaricaPerContoDi()
            Me.CaricaInventario()
            'txtSubCliente21.Text = ddlSubCliente.SelectedItem.Text
            'txtSubCliente31.Text = ddlSubCliente.SelectedItem.Text
            'txtSubCliente41.Text = ddlSubCliente.SelectedItem.Text
        End If
    End Sub

    Private Sub CaricaClienti(Optional ByVal idc As String = "-1")
        Dim str As String = "select ragsoc ,id from Cliente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where Cliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            str = str & "where Cliente.id=" & MyUtente.Cliente.ID
        End If
        str = str & " order by ragsoc"
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlCliente.DataSource = tab
        Me.ddlCliente.DataTextField = "ragsoc"
        Me.ddlCliente.DataValueField = "id"
        Me.ddlCliente.DataBind()
        If idc <> "-1" Then
            Me.ddlCliente.SelectedValue = idc
        Else
            Me.ddlCliente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaSubClienti(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1")
        Dim str As String = "select ragsoc ,id from SubCliente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.Load(Session("id"))
            str = str & "where SubCliente.idazienda=" & MyUtente.Azienda.ID
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.SubCliente = New SubCliente
            Me.MyUtente.Load(Session("id"))
            str = str & "where SubCliente.id<>-1" '=" & MyUtente.SubCliente.ID
        End If
        str = str & " and SubCliente.idcliente=" & idc
        str = str & " order by ragsoc"
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlSubCliente.DataSource = tab
        Me.ddlSubCliente.DataTextField = "ragsoc"
        Me.ddlSubCliente.DataValueField = "id"
        Me.ddlSubCliente.DataBind()
        If idc <> "-1" Then
            Me.ddlSubCliente.SelectedValue = idsc
        Else
            Me.ddlSubCliente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaUtentiCliente(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1", Optional ByVal idu As String = "-1")
        Dim str As String = "select nome + ' ' + cognome AS nomecompleto ,id from Utente "
        If Session("tipoutente") = "Operatore" Then
            Me.MyUtente = New Utente
            Me.MyUtente.SubCliente = New SubCliente
            Me.MyUtente.Cliente = New Cliente
            Me.MyUtente.Load(Session("id"))
            If ddlSubCliente.SelectedValue <> "-1" Then
                str = str & "where Utente.idsubcliente=" & ddlSubCliente.SelectedValue
            Else
                str = str & "where Utente.idcliente=" & ddlCliente.SelectedValue
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            Me.MyUtente = New Utente
            Me.MyUtente.Load(Session("id"))
            str = str & "where Utente.id=" & MyUtente.ID
        End If
        str = str & " order by nomecompleto"
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlUtente.DataSource = tab
        Me.ddlUtente.DataTextField = "nomecompleto"
        Me.ddlUtente.DataValueField = "id"
        Me.ddlUtente.DataBind()
        If idu <> "-1" Then
            Me.ddlUtente.SelectedValue = idu
        Else
            Me.ddlUtente.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaPerContoDi(Optional ByVal idc As String = "-1", Optional ByVal idsc As String = "-1", Optional ByVal idu As String = "-1")
        Dim str As String = "select nome + ' ' + cognome AS nomecompleto ,id from Utente "

        Me.MyUtente = New Utente
        Me.MyUtente.SubCliente = New SubCliente
        Me.MyUtente.Cliente = New Cliente
        Me.MyUtente.Load(Session("id"))
        'If ddlSubCliente.SelectedValue <> "-1" Then
        '    str = str & "where Utente.idsubcliente=" & ddlSubCliente.SelectedValue
        'Else
        str = str & "where Utente.idcliente=" & ddlCliente.SelectedValue
        'End If
        str = str & " order by nomecompleto"
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlPerContoDi.DataSource = tab
        Me.ddlPerContoDi.DataTextField = "nomecompleto"
        Me.ddlPerContoDi.DataValueField = "id"
        Me.ddlPerContoDi.DataBind()
        If idu <> "-1" Then
            Me.ddlPerContoDi.SelectedValue = idu
        Else
            Me.ddlPerContoDi.SelectedValue = "-1"
        End If
    End Sub

    Private Sub CaricaContrattiListini()
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Load(Session("id"))
        Dim str As String = "select codice AS nomecontratto ,Contratto.id from Contratto " & _
                                    "inner join Contratto_Cliente on Contratto.id=Contratto_Cliente.idcontratto " & _
                                    "where Contratto.adata>'" & Date.Today.ToString("yyyy/MM/dd") & "' and Contratto.idazienda=" & MyUtente.Azienda.ID
        If Session("tipoutente") = "Operatore" Then
            ddlContratto.Visible = True
            imgpiucontratto.visible = True
            imgRefreshContratto.Visible = True
            txtScadenza.Visible = True
            'ddlSoglia.Visible = True
            table.Visible = True
            ddlListino.Visible = False
            imgPiuListino.Visible = False
            imgrefreshListino.visible = False

            lblContratto.Visible = True
            lblSoglia.Visible = True
            lblScadenza.Visible = True
            lblListino.Visible = False

            'txtContratto21.Visible = True
            'txtScadenza21.Visible = True
            'txtSoglia21.Visible = True
            'txtListino21.Visible = False
            'lblContratto2.Visible = True
            'lblSoglia2.Visible = True
            'lblScadenza2.Visible = True
            'lblListino2.Visible = False

            'txtContratto31.Visible = True
            'txtScadenza31.Visible = True
            'txtSoglia31.Visible = True
            'txtListino31.Visible = False
            'lblContratto3.Visible = True
            'lblSoglia3.Visible = True
            'lblScadenza3.Visible = True
            'lblListino3.Visible = False

           
            If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
                str = str & "and Contratto_Cliente.tipocliente='subcliente' and Contratto_Cliente.idcliente=" & ddlSubCliente.SelectedValue
            Else
                str = str & "and Contratto_Cliente.tipocliente='cliente' and Contratto_Cliente.idcliente=" & ddlCliente.SelectedValue
            End If
        ElseIf Session("tipoutente") = "Utente" Then
            'str = str & "and Utente.id=-1"
            ddlContratto.Visible = False
            imgpiucontratto.visible = False
            imgRefreshContratto.Visible = False
            txtScadenza.Visible = False
            'ddlSoglia.Visible = False
            table.Visible = False
            ddlListino.Visible = False
            imgPiuListino.Visible = False
            imgrefreshListino.visible = False
            lblContratto.Visible = False
            lblSoglia.Visible = False
            lblScadenza.Visible = False
            lblListino.Visible = False


            'txtContratto21.Visible = False
            'txtScadenza21.Visible = False
            'txtSoglia21.Visible = False
            'txtListino21.Visible = False
            'lblContratto2.Visible = False
            'lblSoglia2.Visible = False
            'lblScadenza2.Visible = False
            'lblListino2.Visible = False

            'txtContratto31.Visible = False
            'txtScadenza31.Visible = False
            'txtSoglia31.Visible = False
            'txtListino31.Visible = False
            'lblContratto3.Visible = False
            'lblSoglia3.Visible = False
            'lblScadenza3.Visible = False
            'lblListino3.Visible = False

            'txtContratto41.Visible = False
            'txtListino41.Visible = False
            'lblContratto4.Visible = False
            'lblListino4.Visible = False
        End If
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)
        If tab.Rows.Count > 1 And Session("tipoutente") <> "Utente" Then
            Me.ddlContratto.DataSource = tab
            Me.ddlContratto.DataTextField = "nomecontratto"
            Me.ddlContratto.DataValueField = "id"
            Me.ddlContratto.DataBind()
            Me.ddlContratto.SelectedValue = "-1"
        Else
            Me.CaricaListino()
        End If
    End Sub

    Private Sub CaricaListino()
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyCliente = New Cliente
        Me.MySubCliente = New SubCliente
        Me.MyCliente.Listino = New Listino
        Me.MySubCliente.Listino = New Listino
        Me.MyUtente.Load(Session("id"))
        Dim str As String = "select descrizione ,id from Listino " & _
                                    "where Listino.idazienda=" & MyUtente.Azienda.ID
        If Session("tipoutente") = "Operatore" Then
            ddlContratto.Visible = False
            imgpiucontratto.visible = False
            imgRefreshContratto.Visible = False
            txtScadenza.Visible = False
            'ddlSoglia.Visible = False
            table.Visible = False
            ddlListino.Visible = True
            imgPiuListino.Visible = True
            imgrefreshListino.visible = True
            lblContratto.Visible = False
            lblSoglia.Visible = False
            lblScadenza.Visible = False
            lblListino.Visible = True

            'txtContratto21.Visible = False
            'txtScadenza21.Visible = False
            'txtSoglia21.Visible = False
            'txtListino21.Visible = True
            'lblContratto2.Visible = False
            'lblSoglia2.Visible = False
            'lblScadenza2.Visible = False
            'lblListino2.Visible = True

            'txtContratto31.Visible = False
            'txtScadenza31.Visible = False
            'txtSoglia31.Visible = False
            'txtListino31.Visible = True
            'lblContratto3.Visible = False
            'lblSoglia3.Visible = False
            'lblScadenza3.Visible = False
            'lblListino3.Visible = True

            
            Dim tab As DataTable
            tab = Me.MyGest.GetTab(str)
            Dim row As DataRow = tab.NewRow
            row(0) = "..."
            row(1) = "-1"
            tab.Rows.Add(row)
            Me.ddlListino.DataSource = tab
            Me.ddlListino.DataTextField = "descrizione"
            Me.ddlListino.DataValueField = "id"
            Me.ddlListino.DataBind()
            Me.MyCliente.Load(ddlCliente.SelectedValue)
            If ddlSubCliente.SelectedValue <> "-1" Then
                If Me.MySubCliente.Listino.ID <> -1 Then
                    Me.ddlListino.SelectedValue = Me.MySubCliente.Listino.ID
                Else
                    Me.ddlListino.SelectedValue = "-1"
                End If
            Else
                If Me.MyCliente.Listino.ID <> -1 Then
                    Me.ddlListino.SelectedValue = Me.MyCliente.Listino.ID
                Else
                    Me.ddlListino.SelectedValue = "-1"
                End If
            End If
            'txtListino21.Text = ddlListino.SelectedItem.Text
            'txtListino31.Text = ddlListino.SelectedItem.Text

        ElseIf Session("tipoutente") = "Utente" Then
            ddlContratto.Visible = False
            imgpiucontratto.visible = False
            imgRefreshContratto.Visible = False
            txtScadenza.Visible = False
            'ddlSoglia.Visible = False
            table.Visible = False
            ddlListino.Visible = False
            imgPiuListino.Visible = False
            imgrefreshListino.visible = False
            lblContratto.Visible = False
            lblSoglia.Visible = False
            lblScadenza.Visible = False
            lblListino.Visible = False

            'txtContratto21.Visible = False
            'txtScadenza21.Visible = False
            'txtSoglia21.Visible = False
            'txtListino21.Visible = False
            'lblContratto2.Visible = False
            'lblSoglia2.Visible = False
            'lblScadenza2.Visible = False
            'lblListino2.Visible = False

            'txtContratto31.Visible = False
            'txtScadenza31.Visible = False
            'txtSoglia31.Visible = False
            'txtListino31.Visible = False
            'lblContratto3.Visible = False
            'lblSoglia3.Visible = False
            'lblScadenza3.Visible = False
            'lblListino3.Visible = False

            
        End If

    End Sub

    Private Sub CaricaInventario(Optional id As String = "-1")
        Me.MyUtente = New Utente
        Me.MyUtente.Azienda = New Azienda
        Me.MyUtente.Load(Session("id"))
        '"left outer join TipoDispositivo on TipoDispositivo.id=Inventario.idtipodispositivo " & _
        '"left outer join Marchio on Marchio.idtipodispositivo=TipoDispositivo.id " & _
        '"left outer join Modello on Marchio.id=Modello.idmarchio " & _
        Dim str As String = "select codice AS nomecontratto ,Inventario.id from Inventario " & _
                            "where codice<>'-1' "

        If ddlSubCliente.SelectedValue <> "-1" And ddlSubCliente.SelectedValue <> "" Then
            str = str & "and Inventario.idsubcliente=" & ddlSubCliente.SelectedValue & " or (Inventario.idcliente=" & ddlCliente.SelectedValue & " and Inventario.mobile=1) "
        Else
            str = str & "and Inventario.idcliente=" & ddlCliente.SelectedValue
        End If

        Dim tab As DataTable
        tab = Me.MyGest.GetTab(str)
        Dim row As DataRow = tab.NewRow
        row(0) = "..."
        row(1) = "-1"
        tab.Rows.Add(row)

        Me.ddlInventario.DataSource = tab
        Me.ddlInventario.DataTextField = "nomecontratto"
        Me.ddlInventario.DataValueField = "id"
        Me.ddlInventario.DataBind()
        Me.ddlInventario.SelectedValue = id



        Me.ddlInventario1.DataSource = tab
        Me.ddlInventario1.DataTextField = "nomecontratto"
        Me.ddlInventario1.DataValueField = "id"
        Me.ddlInventario1.DataBind()
        Me.ddlInventario1.SelectedValue = "-1"
        If ddlUtente.SelectedValue <> "-1" Then
            str = str & " and idutente=" & ddlUtente.SelectedValue
            Dim tab2 As DataTable = Me.MyGest.GetTab(str)
            If tab2.Rows.Count = 1 Then
                Me.ddlInventario.SelectedValue = tab2.Rows(0)("id")
            End If
            'If tab.Rows.Count > 0 Then
            '    Me.myinventario = New Inventario
            '    Me.myinventario.load(tab.Rows(0)("id"))
            'End If
        End If
    End Sub

    Protected Sub txtDataAp_TextChanged(sender As Object, e As EventArgs) Handles txtDataAp.TextChanged
        'lblDataAp.Text = txtDataAp.Text
        'lblDataAp2.Text = txtDataAp.Text

    End Sub

    Protected Sub txtOraAp_TextChanged(sender As Object, e As EventArgs) Handles txtOraAp.TextChanged
        'lblOraAp.Text = txtOraAp.Text
        'lblOraAp2.Text = txtOraAp.Text

    End Sub

    Protected Sub ddlContratto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlContratto.SelectedIndexChanged
        'txtContratto21.Text = ddlContratto.SelectedItem.Text
        'txtContratto31.Text = ddlContratto.SelectedItem.Text

        Me.CaricaScadenza()
        Me.CaricaSoglia()
    End Sub

    Private Function ValutaMinutaggio(ByVal ora, ByVal minuto) As String
        Dim frat
        If minuto > 59 Then
            frat = minuto \ 60
            ora = ora + frat
            minuto = minuto - (frat * 60)
        End If
        If minuto.ToString.Length = 1 Then
            minuto = "0" & minuto
        End If
        If ora.ToString.Length = 1 Then
            ora = "0" & ora
        End If
        Return ora & ":" & minuto
    End Function

    Private Sub CaricaSoglia()
        Dim rg As New HtmlTableRow
        Dim cl As New HtmlTableCell


        
        Dim sql As String = " select soglia,tariffazione,tiposoglia from SogliaContratto " & _
                            " inner join Tariffazione on Tariffazione.id=SogliaContratto.idtipotariffazione " & _
                            " inner join TipoSoglia on TipoSoglia.id=SogliaContratto.idtiposoglia " & _
                            " where idcontratto = " & ddlContratto.SelectedValue
        Dim tab As DataTable
        tab = Me.MyGest.GetTab(sql)


        'calcolo residuo soglie
        Dim str As String = "select sum(cast(substring(tempotot,1,2)as integer))as tempotot,sum(cast(substring(tempotot,4,2)as integer))as mintot,TempoTicket .idtipotariffazione   from TempoTicket " & _
                            "inner join Tickets on Tickets.id=TempoTicket.idTicket " & _
                            "inner join Contratto on Contratto.id=Tickets.idcontratto " & _
                            "where Contratto.id = " & ddlContratto.SelectedValue & _
                            "group by TempoTicket .idtipotariffazione "


        Dim tb As DataTable = Me.MyGest.GetTab(str)





        Dim tabresiduo As New DataTable

        Dim sqlStr = "SELECT * " & _
                     "FROM SogliaContratto inner join TipoFuoriSoglia on TipoFuoriSoglia.id=SogliaContratto.idfuori " & _
                     "inner join TipoSoglia on TipoSoglia.id=SogliaContratto.idtiposoglia " & _
                     "inner join Tariffazione on Tariffazione.id=SogliaContratto.idtipotariffazione " & _
                     "inner join Contratto on Contratto.id=SogliaContratto.idcontratto " & _
                     "left outer join Listino on Listino.id=SogliaContratto.idlistino " & _
                     "where Contratto.id = " & ddlContratto.SelectedValue & " "

        tabresiduo = MyGest.GetTab(sqlStr)
        Dim temporesiduo(10)
        Dim sogliaora
        Dim sogliamin
        For i As Integer = 0 To tabresiduo.Rows.Count - 1
            temporesiduo(i) = tabresiduo.Rows(i)("soglia")
            For j As Integer = 0 To tb.Rows.Count - 1
                If tabresiduo.Rows(i)("idtipotariffazione") = tb.Rows(j)("idtipotariffazione") Then
                    Try
                        sogliaora = tabresiduo.Rows(i)("soglia").ToString.Split(":")(0)
                    Catch
                        sogliaora = tabresiduo.Rows(i)("soglia")
                    End Try
                    Try
                        sogliamin = tb.Rows(i)("soglia").ToString.Split(":")(1)
                    Catch
                        sogliamin = "00"
                    End Try
                    
                    Dim differenza = sogliaora - Me.ValutaMinutaggio(tb.Rows(j)("tempotot"), tb.Rows(j)("mintot")).ToString.Split(":")(0) & ":" & sogliamin - Me.ValutaMinutaggio(tb.Rows(j)("tempotot"), tb.Rows(j)("mintot")).ToString.Split(":")(1)
                    If CInt(differenza.ToString.Split(":")(1)) < 0 Then
                        temporesiduo(i) = CInt(differenza.ToString.Split(":")(0)) - 1 & ":" & 60 + CInt(differenza.ToString.Split(":")(1))
                    Else
                        temporesiduo(i) = differenza
                    End If

                    'Dim differenza As TimeSpan = tab.Rows(i)("soglia").Subtract(CDate(tb.Rows(i)("tempotot")))

                End If
            Next
            Try
                If temporesiduo(i).ToString.Split(":")(1).Length = 1 Then
                    temporesiduo(i) = temporesiduo(i).ToString.Split(":")(0) & ":0" & temporesiduo(i).ToString.Split(":")(1)
                End If
            Catch
            End Try
        Next


        'fine calcolo residuo



        'campo.TextMode = TextBoxMode.MultiLine
        If tab.Rows.Count > 0 Then
            For i As Integer = 0 To tab.Rows.Count - 1
                tipo(i) = New TextBox
                tipo(i).CssClass = "input-text"
                tipo(i).Width = 400
                tipo(i).Enabled = False

                If temporesiduo(i).ToString.Split(":")(0) >= 0 Then
                    tipo(i).Text = tab.Rows(i)("tariffazione") & " - " & tab.Rows(i)("soglia") & " " & tab.Rows(i)("tiposoglia") & " - " & temporesiduo(i) & " " & tab.Rows(i)("tiposoglia") & " rimanenti"
                Else
                    tipo(i).Text = tab.Rows(i)("tariffazione") & " - " & tab.Rows(i)("soglia") & " " & tab.Rows(i)("tiposoglia") & " - Soglia superata di " & temporesiduo(i).ToString.Replace("-", "") & " " & tab.Rows(i)("tiposoglia")
                End If
               

                tipo(i).Visible = True

                cl.Controls.Add(tipo(i))

                rg.Cells.Add(cl)
                Me.table.Rows.Add(rg)

            Next
            lblSoglia.Visible = True
        Else
            lblSoglia.Visible = False
        End If
        'campo.Text = descr




        'campo.Width = 700
        'campo.Height = 80
        'campo.ReadOnly = True

    End Sub

    'Private Sub CaricaSoglia2()
    '    Dim sql As String = "select soglia,id from SogliaContratto where idcontratto=" & ddlContratto.SelectedValue
    '    Dim tab As DataTable
    '    tab = Me.MyGest.GetTab(sql)
    '    Dim row As DataRow = tab.NewRow
    '    row(0) = "0"
    '    row(1) = "-1"
    '    tab.Rows.Add(row)

    '    Me.ddlSoglia.DataSource = tab
    '    Me.ddlSoglia.DataTextField = "soglia"
    '    Me.ddlSoglia.DataValueField = "id"
    '    Me.ddlSoglia.DataBind()
    '    Me.ddlSoglia.SelectedValue = "-1"

    '    'Me.ddlSoglia21.DataSource = tab
    '    'Me.ddlSoglia21.DataTextField = "soglia"
    '    'Me.ddlSoglia21.DataValueField = "id"
    '    'Me.ddlSoglia21.DataBind()
    '    'Me.ddlSoglia21.SelectedValue = "-1"

    '    'Me.ddlSoglia31.DataSource = tab
    '    'Me.ddlSoglia31.DataTextField = "soglia"
    '    'Me.ddlSoglia31.DataValueField = "id"
    '    'Me.ddlSoglia31.DataBind()
    '    'Me.ddlSoglia31.SelectedValue = "-1"

    'End Sub

    Private Sub CaricaScadenza()
        Me.MyContratto = New Contratto
        If ddlContratto.SelectedValue <> "-1" Then
            Me.MyContratto.Load(ddlContratto.SelectedValue)

            If Me.MyContratto.AData < Date.Today Then
                Dim message As String = "Attenzione! Contratto scaduto. "
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
                Me.CaricaContrattiListini()
            Else
                txtScadenza.Text = Me.MyContratto.AData
                'txtScadenza21.Text = txtScadenza.Text
                'txtScadenza31.Text = txtScadenza.Text
            End If
        Else
            txtScadenza.Text = ""
        End If

    End Sub

    Protected Sub ddlListino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlListino.SelectedIndexChanged
        'txtListino21.Text = ddlListino.SelectedItem.Text
        'txtListino31.Text = ddlListino.SelectedItem.Text

    End Sub

    Protected Sub ddlUtente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUtente.SelectedIndexChanged
        'txtUtente21.Text = ddlUtente.SelectedItem.Text
        'txtUtente31.Text = ddlUtente.SelectedItem.Text

        Me.CaricaInventario()
    End Sub

    Protected Sub ddlPerContoDi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPerContoDi.SelectedIndexChanged
        'txtPerContoDi21.Text = ddlPerContoDi.SelectedItem.Text
        'txtPerContoDi31.Text = ddlPerContoDi.SelectedItem.Text

    End Sub


    Protected Sub btnApri_Click(sender As Object, e As EventArgs) Handles btnApri.Click


        If txtDescrizione.Text <> "" And txtOggetto.Text <> "" And ddlCliente.SelectedValue <> "" Then
            Me.MyTickets = New Tickets
            Me.MyTickets.Azienda = New Azienda
            Me.MyTickets.SubAzienda = New SubAzienda
            Me.MyTickets.Cliente = New Cliente
            Me.MyTickets.SubCliente = New SubCliente
            Me.MyTickets.Utente = New Utente
            Me.MyTickets.PerContoDi = New Utente
            Me.MyTickets.Soglia = New SogliaContratto
            Me.MyTickets.Contratto = New Contratto
            Me.MyTickets.Listino = New Listino
            Me.MyTickets.Inventario = New Inventari
            Me.MyTickets.AltroInventario = New Inventari
            Me.MyUtente = New Utente
            Me.MyUtente.Azienda = New Azienda
            Me.MyUtente.SubAzienda = New SubAzienda
            Me.MyTickets.Operatore = New Utente
            Me.MyUtente.Load(Session("id"))
            If Session("tipoutente") = "Operatore" Or Session("tipoutente") = "Utente" Then
                If MyUtente.SubAzienda.ID <> "-1" Then
                    Me.MyTickets.Azienda.ID = MyUtente.Azienda.ID
                    Me.MyTickets.SubAzienda.ID = MyUtente.SubAzienda.ID
                Else
                    Me.MyTickets.Azienda.ID = MyUtente.Azienda.ID
                    Me.MyTickets.SubAzienda.ID = -1
                End If
                Me.MyTickets.Cliente.ID = ddlCliente.SelectedValue
                Me.MyTickets.SubCliente.ID = ddlSubCliente.SelectedValue
                Me.MyTickets.DataApertura = CType(txtDataAp.Text & " " & txtOraAp.Text, DateTime)

                Me.MyTickets.DataChiusura = "01/01/9999"
                Me.MyTickets.DataUltimo = "01/01/9999"
                If ddlUtente.SelectedValue <> "" Then
                    Me.MyTickets.Utente.ID = ddlUtente.SelectedValue
                    Me.MyTickets.PerContoDi.ID = ddlPerContoDi.SelectedValue
                Else
                    Me.MyTickets.Utente.ID = "-1"
                    Me.MyTickets.PerContoDi.ID = "-1"
                End If

                If ddlContratto.SelectedValue <> "" Then
                    Me.MyTickets.Contratto.ID = ddlContratto.SelectedValue
                    If ddlSoglia.SelectedValue <> "" Then
                        Me.MyTickets.Soglia.ID = ddlSoglia.SelectedValue
                    Else
                        Me.MyTickets.Soglia.ID = -1
                    End If
                Else
                    Me.MyTickets.Contratto.ID = "-1"
                End If


                'If Me.MyTickets.Contratto.ID <> "-1" Then
                '    Dim sql As String = "Select * from SogliaContratto where idcontratto=" & Me.MyTickets.Contratto.ID
                '    Dim tab As DataTable = Me.MyGest.GetTab(sql)
                'End If
                If ddlSoglia.SelectedValue <> "-1" And ddlSoglia.SelectedValue <> "" Then
                    Me.MyTickets.Soglia.Load(ddlSoglia.SelectedValue)
                    Me.MyTickets.DataScadenza = CType(txtDataAp.Text & " " & txtOraAp.Text, DateTime).AddDays(MyTickets.Soglia.SLA)
                Else
                    Me.MyTickets.DataScadenza = CType(txtDataAp.Text & " " & txtOraAp.Text, DateTime).AddDays(3)
                End If
                    If ddlListino.SelectedValue <> "" Then
                        Me.MyTickets.Listino.ID = ddlListino.SelectedValue
                    Else
                        Me.MyTickets.Listino.ID = "-1"
                    End If
                    If ddlInventario.SelectedValue <> "" Then
                        Me.MyTickets.Inventario.ID = ddlInventario.SelectedValue
                        Me.MyTickets.AltroInventario.ID = ddlInventario1.SelectedValue
                    Else
                        Me.MyTickets.Inventario.ID = "-1"
                        Me.MyTickets.AltroInventario.ID = "-1"
                    End If
                    If rbTutti.Checked Then
                        Me.MyTickets.Bloccante = 2
                    Else
                        Me.MyTickets.Bloccante = 1
                    End If
                    If rbFisso.Checked Then
                        Me.MyTickets.Guasto = 2
                    Else
                        Me.MyTickets.Guasto = 1

                    End If
                    Me.MyTickets.Oggetto = txtOggetto.Text
                    Me.MyTickets.Descrizione = txtDescrizione.Text
                    Me.MyTickets.UrlImmagine = ""
                    Me.MyTickets.UrlVideo = ""
                    Me.MyTickets.idStato = 1
                    If Session("tipoutente") = "Operatore" Then
                        Me.MyTickets.Operatore.ID = Session("id")
                    Else
                        Me.MyTickets.Operatore.ID = -1
                    End If
                    If cbxStealth.Checked Then
                        Me.MyTickets.Stealth = 1
                    Else
                        Me.MyTickets.Stealth = 0
                    End If
                Me.MyTickets.SalvaData()
                Me.InviaMailTicket()
                Me.SvuotaCampi()
                    If Session("tipoutente") = "Operatore" Then
                        Dim message2 As String = "Ticket creato correttamente"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message2 + "');", True)
                    Else
                        Dim message As String = "Grazie! hai aperto il ticket numero " & MyTickets.ID & ".Sarai contattato il prima possibile "
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopupRedirect('" + message + "');", True)
                    End If
                    Response.Redirect("tickets.aspx")
                End If
        Else
            Dim message As String = "Inserire tutti i campi Obbligatori"
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Popup", "ShowPopup('" + message + "');", True)
        End If
    End Sub

    Private Sub InviaMailTicket()
        Me.MyCGlobal = New CGlobal
        Me.MyFitMail = New FitMail
        Me.MyFitMail.Ticket = New Tickets
        Me.MyFitMail.Evento = New Evento
        Me.MyFitMail.Utente = New Utente
        Me.MyFitMail.Azienda = New Azienda
        Me.MyFitMail.SubAzienda = New SubAzienda

        Dim utente As Boolean = False
        Dim percontodi As Boolean = False
        Dim sess As Boolean = False
        Dim azienda As Boolean = False
        Dim subazienda As Boolean = False

        If Not cbxStealth.Checked Then
            If ddlUtente.SelectedValue <> "-1" Then
                'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, ddlUtente.SelectedValue)
                Me.MyFitMail.ID = -1
                Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                Me.MyFitMail.Evento.ID = -1
                Me.MyFitMail.Utente.ID = ddlUtente.SelectedValue
                Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                Me.MyFitMail.SalvaData()
                utente = True
            End If
            If ddlPerContoDi.SelectedValue <> "-1" And ddlPerContoDi.SelectedValue <> ddlUtente.SelectedValue Then
                'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, ddlPerContoDi.SelectedValue)
                Me.MyFitMail.ID = -1
                Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                Me.MyFitMail.Evento.ID = -1
                Me.MyFitMail.Utente.ID = ddlPerContoDi.SelectedValue
                Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                Me.MyFitMail.SalvaData()
                percontodi = True
            End If
        End If
        If Session("id") <> ddlUtente.SelectedValue And Session("id") <> ddlPerContoDi.SelectedValue Then
            'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, Session("id"))
            Me.MyFitMail.ID = -1
            Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
            Me.MyFitMail.Evento.ID = -1
            Me.MyFitMail.Utente.ID = Session("id")
            Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
            Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
            Me.MyFitMail.SalvaData()
            sess = True
        End If
        Me.MyTickets.Cliente = New Cliente
        Me.MyTickets.SubCliente = New SubCliente
        Me.MyTickets.Cliente.SubAzienda = New SubAzienda
        Dim t As DataTable
        Dim tad As DataTable
        If Me.MyTickets.Cliente.SubAzienda.ID <> "-1" Then
            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID
            t = Me.MyGest.GetTab(s)
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = t.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                    azienda = True
                End If
            Next

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idazienda=" & Me.MyTickets.Cliente.Azienda.ID & "and idcliente=" & Me.MyTickets.Cliente.ID & "and idsubcliente=" & Me.MyTickets.SubCliente.ID
            tad = Me.MyGest.GetTab(s)
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = tad.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                End If
            Next
        Else
            Dim s As String = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Operatore' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID
            t = Me.MyGest.GetTab(s)
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = t.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                    subazienda = True
                End If
            Next

            s = "select *,Utente.id as idu from Utente inner join TipoUtente on TipoUtente.id=Utente.idtipo where tipoutente='Utente' and isadmin='1' and idsubazienda=" & Me.MyTickets.Cliente.SubAzienda.ID & "and idcliente=" & Me.MyTickets.Cliente.ID & "and idsubcliente=" & Me.MyTickets.SubCliente.ID
            tad = Me.MyGest.GetTab(s)
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    'Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"))
                    Me.MyFitMail.ID = -1
                    Me.MyFitMail.Ticket.ID = Me.MyTickets.ID
                    Me.MyFitMail.Evento.ID = -1
                    Me.MyFitMail.Utente.ID = tad.Rows(i)("idu")
                    Me.MyFitMail.Azienda.ID = Me.MyTickets.Azienda.ID
                    Me.MyFitMail.SubAzienda.ID = Me.MyTickets.SubAzienda.ID
                    Me.MyFitMail.SalvaData()
                End If
            Next
        End If

        If utente Then
            Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, ddlUtente.SelectedValue, tipo)
        End If
        If percontodi Then
            Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, ddlPerContoDi.SelectedValue, tipo)
        End If
        If sess Then
            Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, Session("id"), tipo)
        End If
        If azienda Then
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"), tipo)
                End If
            Next
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, tad.Rows(i)("idu"), tipo)
                End If
            Next
        ElseIf subazienda Then
            For i As Integer = 0 To t.Rows.Count - 1
                If t.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And t.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And t.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, t.Rows(i)("idu"), tipo)
                End If
            Next
            For i As Integer = 0 To tad.Rows.Count - 1
                If tad.Rows(i)("idu") <> Me.MyTickets.Operatore.ID And tad.Rows(i)("idu") <> Me.MyTickets.PerContoDi.ID And tad.Rows(i)("idu") <> Me.MyTickets.Utente.ID Then
                    Me.MyCGlobal.InviaMailTicket(Me.MyTickets.ID, tad.Rows(i)("idu"), tipo)
                End If
            Next
        End If
        utente = False
        percontodi = False
        sess = False
        azienda = False
        subazienda = False
        Me.MyFitMail.Delete(Me.MyTickets.ID, -1, Me.MyTickets.Azienda.ID, Me.MyTickets.SubAzienda.ID)

    End Sub

    Private Sub SvuotaCampi()
        txtDescrizione.Text = ""
        txtOggetto.Text = ""
        cbxStealth.Checked = False
        ddlCliente.SelectedValue = -1
        ddlSubCliente.SelectedValue = -1
        ddlContratto.SelectedValue = -1
        ddlListino.SelectedValue = -1
        ddlSoglia.SelectedValue = -1
        ddlUtente.SelectedValue = -1
        ddlPerContoDi.SelectedValue = -1
        txtScadenza.Text = ""
        txtDataAp.Text = ""
        txtOraAp.Text = ""

        'txtCliente21.Text = ""
        'txtContratto21.Text = ""
        'txtListino21.Text = ""
        'txtUtente21.Text = ""
        'txtSubCliente21.Text = ""
        'txtScadenza21.Text = ""
        'txtSoglia21.Text = ""
        'txtPerContoDi21.Text = ""

        ddlInventario.SelectedValue = -1
        ddlInventario1.SelectedValue = -1

        'txtCliente31.Text = ""
        'txtContratto31.Text = ""
        'txtListino31.Text = ""
        'txtUtente31.Text = ""
        'txtSubCliente31.Text = ""
        'txtScadenza31.Text = ""
        'txtSoglia31.Text = ""
        'txtPerContoDi31.Text = ""
    End Sub

    Protected Sub ddlSoglia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSoglia.SelectedIndexChanged
        'txtSoglia21.Text = ddlSoglia.Text
        'txtSoglia31.Text = ddlSoglia.Text
    End Sub

    Protected Sub ddlInventario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlInventario.SelectedIndexChanged
        Me.MyInventario = New Inventari
        Me.MyDispositivo = New TipoDispositivo
        Me.MyMarchio = New Marchio
        Me.MyModello = New Modello
        Me.MyInventario.TipoDispositivo = New TipoDispositivo
        Me.MyInventario.Marchio = New Marchio
        Me.MyInventario.Modello = New Modello
        Me.MyInventario.Utente = New Utente
        Me.MyInventario.FornitoreOrg = New Fornitore
        Me.MyInventario.FornitoreCli = New Fornitore
        Me.MyInventario.Load(ddlInventario.SelectedValue)
        Me.MyDispositivo.Load(Me.MyInventario.TipoDispositivo.ID)
        lblDispositivo.Text = Me.MyDispositivo.TipoDispositivo
        Me.MyMarchio.Load(Me.MyInventario.Marchio.ID)
        lblMarchio.Text = Me.MyMarchio.Marchio
        Me.MyModello.Load(Me.MyInventario.Modello.ID)
        lblModello.Text = Me.MyModello.Modello
        lblSeriale.Text = Me.MyInventario.Seriale
        lblUbicazione.Text = Me.MyInventario.Ubicazione
        lblUtenteInv.Text = Me.MyInventario.Utente.Cognome + " " + Me.MyInventario.Utente.Nome
        lblFornOrg.Text = Me.MyInventario.FornitoreOrg.RagSoc
        lblFornCli.Text = Me.MyInventario.FornitoreCli.RagSoc
        'txtInventario31.Text = ddlInventario.SelectedItem.Text
        If ddlInventario.SelectedValue <> "-1" Then
            lblDispositivo.Visible = True
            lblMarchio.Visible = True
            lblModello.Visible = True
            lblSeriale.Visible = True
            lblUbicazione.Visible = True
            lblUtenteInv.Visible = True
            lblFornOrg.Visible = True
            lblFornCli.Visible = True

            Label9.Visible = True
            Label10.Visible = True
            Label11.Visible = True
            Label12.Visible = True
            Label13.Visible = True
            Label14.Visible = True
            Label15.Visible = True
            Label16.Visible = True
        Else
            lblDispositivo.Visible = False
            lblMarchio.Visible = False
            lblModello.Visible = False
            lblSeriale.Visible = False
            lblUbicazione.Visible = False
            lblUtenteInv.Visible = False
            lblFornOrg.Visible = False
            lblFornCli.Visible = False

            Label9.Visible = False
            Label10.Visible = False
            Label11.Visible = False
            Label12.Visible = False
            Label13.Visible = False
            Label14.Visible = False
            Label15.Visible = False
            Label16.Visible = False
        End If
    End Sub

    Protected Sub ddlInventario1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlInventario1.SelectedIndexChanged
        Me.MyInventario = New Inventari
        Me.MyDispositivo = New TipoDispositivo
        Me.MyMarchio = New Marchio
        Me.MyModello = New Modello
        Me.MyInventario.TipoDispositivo = New TipoDispositivo
        Me.MyInventario.Marchio = New Marchio
        Me.MyInventario.Modello = New Modello
        Me.MyInventario.Utente = New Utente
        Me.MyInventario.FornitoreOrg = New Fornitore
        Me.MyInventario.FornitoreCli = New Fornitore
        Me.MyInventario.Load(ddlInventario1.SelectedValue)
        Me.MyDispositivo.Load(Me.MyInventario.TipoDispositivo.ID)
        lblDispositivo1.Text = Me.MyDispositivo.TipoDispositivo
        Me.MyMarchio.Load(Me.MyInventario.Marchio.ID)
        lblMarchio1.Text = Me.MyMarchio.Marchio
        Me.MyModello.Load(Me.MyInventario.Modello.ID)
        lblModello1.Text = Me.MyModello.Modello
        lblSeriale1.Text = Me.MyInventario.Seriale
        lblUbicazione1.Text = Me.MyInventario.Ubicazione
        lblUtenteInv1.Text = Me.MyInventario.Utente.Cognome + " " + Me.MyInventario.Utente.Nome
        lblFornOrg1.Text = Me.MyInventario.FornitoreOrg.RagSoc
        lblFornCli1.Text = Me.MyInventario.FornitoreCli.RagSoc
        'txtAltroInventario31.Text = ddlInventario1.SelectedItem.Text
        If ddlInventario1.SelectedValue <> "-1" Then
            lblDispositivo1.Visible = True
            lblMarchio1.Visible = True
            lblModello1.Visible = True
            lblSeriale1.Visible = True
            lblUbicazione1.Visible = True
            lblUtenteInv1.Visible = True
            lblFornOrg1.Visible = True
            lblFornCli1.Visible = True

            Label1.Visible = True
            Label2.Visible = True
            Label3.Visible = True
            Label4.Visible = True
            Label5.Visible = True
            Label6.Visible = True
            Label7.Visible = True
            Label8.Visible = True
        Else
            lblDispositivo1.Visible = False
            lblMarchio1.Visible = False
            lblModello1.Visible = False
            lblSeriale1.Visible = False
            lblUbicazione1.Visible = False
            lblUtenteInv1.Visible = False
            lblFornOrg1.Visible = False
            lblFornCli1.Visible = False

            Label1.Visible = False
            Label2.Visible = False
            Label3.Visible = False
            Label4.Visible = False
            Label5.Visible = False
            Label6.Visible = False
            Label7.Visible = False
            Label8.Visible = False
        End If
    End Sub
   


    Protected Sub imgRefreshCliente_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshCliente.Click
        Me.CaricaClienti()
    End Sub

    Protected Sub imgRefreshContratto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshContratto.Click
        txtScadenza.Text = ""
        Me.CaricaContrattiListini()

    End Sub

    Protected Sub imgRefreshListino_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshListino.Click
        Me.CaricaListino()
    End Sub

    Protected Sub imgRefreshSubCliente_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshSubCliente.Click
        Me.CaricaSubClienti(ddlCliente.SelectedValue)
    End Sub

    Protected Sub imgRefreshUtente_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshUtente.Click
        Me.CaricaUtentiCliente(ddlCliente.SelectedValue, ddlSubCliente.SelectedValue)
        Me.CaricaPerContoDi(ddlCliente.SelectedValue, ddlSubCliente.SelectedValue)
    End Sub

    Protected Sub imgRefreshInvetario_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgRefreshInventario.Click
        Me.CaricaInventario(ddlInventario.SelectedValue)
        ddlInventario_SelectedIndexChanged(sender, e)
        ddlInventario1_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub AbilitaAggiungiRefresh()
        If Session("tipoutente") = "Utente" And Session("isadmin") = 1 Then
            imgPiuCliente.Visible = False
            imgRefreshCliente.Visible = False
            imgPiuSubCliente.Visible = False
            imgRefreshSubCliente.Visible = False
            imgAppPiuSubCliente.Visible = True
            imgAppRefreshSubCliente.Visible = True
            imgPiuContratto.Visible = False
            imgRefreshContratto.Visible = False
            imgPiuListino.Visible = False
            imgRefreshListino.Visible = False
            imgPiuUtente.Visible = True
            imgRefreshUtente.Visible = True
            imgAppPiuUtente.Visible = False
            imgAppRefreshUtente.Visible = False
            imgPiuInventario.Visible = False
            imgRefreshInventario.Visible = False
        ElseIf Session("tipoutente") = "Utente" Then
            imgPiuCliente.Visible = False
            imgRefreshCliente.Visible = False
            imgPiuSubCliente.Visible = False
            imgRefreshSubCliente.Visible = False
            imgPiuContratto.Visible = False
            imgRefreshContratto.Visible = False
            imgPiuListino.Visible = False
            imgRefreshListino.Visible = False
            imgPiuUtente.Visible = False
            imgRefreshUtente.Visible = False
            imgPiuPerContoDi.Visible = False
            imgRefreshPerContoDi.Visible = False
            imgPiuInventario.Visible = False
            imgRefreshInventario.Visible = False
        End If
    End Sub
End Class