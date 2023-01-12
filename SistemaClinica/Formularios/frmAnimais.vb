Imports MySql.Data.MySqlClient

Public Class frmAnimais
    Private Sub cbCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCategoria.SelectedIndexChanged, cbCategoriaBuscar.SelectedIndexChanged
        Buscar()
    End Sub

    Private Sub frmAnimais_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CarregarCategorias()
        CarregarCategoriasBuscar()
        Listar()
    End Sub

    Private Sub CarregarCategorias()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select * from categorias order by id_categoria desc"

            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                cbCategoria.ValueMember = "id_categoria"
                cbCategoria.DisplayMember = "descricao"
                cbCategoria.DataSource = dt


            Else
                'MsgBox("É preciso cadastrar antes as categorias")'
            End If

        Catch ex As Exception
            MsgBox("Erro ao listar os dados" + ex.Message)
        End Try
    End Sub

    Private Sub CarregarCategoriasBuscar()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select * from categorias order by id_categoria desc"

            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                cbCategoriaBuscar.ValueMember = "id_categoria"
                cbCategoriaBuscar.DisplayMember = "descricao"
                cbCategoriaBuscar.DataSource = dt
            Else
                'MsgBox("É preciso cadastrar antes as categorias")'
            End If

        Catch ex As Exception
            MsgBox("Erro ao listar os dados" + ex.Message)
        End Try
    End Sub


    Sub Limpar()
        txtBuscar.Text = ""
        txtCodigo.Text = ""
        txtIdade.Text = ""
        txtNome.Text = ""
        txtRaca.Text = ""
    End Sub

    Sub HabilitarCampos()
        txtIdade.Enabled = True
        txtNome.Enabled = True
        txtRaca.Enabled = True
        cbCategoria.Enabled = True
    End Sub

    Sub DesabilitarCampos()
        txtIdade.Enabled = False
        txtNome.Enabled = False
        txtRaca.Enabled = False
        cbCategoria.Enabled = False
    End Sub

    Private Sub Listar()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select a.id_animal, c.descricao, a.nome, a.raca, a.idade, a.data_cadastro, a.id_categoria 
            From animais as a INNER JOIN categorias as c on a.id_categoria = c.id_categoria order by nome asc"


            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)
            dg.DataSource = dt

            FormatarDG()
            ContarLinhas()

        Catch ex As Exception
            MsgBox("Erro ao listar os dados" + ex.Message)
        End Try
    End Sub

    Sub ContarLinhas()
        Dim total As Integer = dg.Rows.Count
        lblRegistros.Text = total
    End Sub

    Private Sub FormatarDG()

        dg.Columns(0).Visible = False
        dg.Columns(6).Visible = False

        dg.Columns(0).HeaderText = "Id do Animal"
        dg.Columns(1).HeaderText = "Categoria"
        dg.Columns(2).HeaderText = "Nome"
        dg.Columns(3).HeaderText = "Raca"
        dg.Columns(4).HeaderText = "Idade"
        dg.Columns(5).HeaderText = "Data de cadastro"

        'dg.Columns(0).Width = 140
        'dg.Columns(1).Width = 140

    End Sub


    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click

        If cbCategoria.Text <> "" Then
            Limpar()
            HabilitarCampos()
            btnSalvar.Enabled = True
        Else
            MsgBox("É preciso cadastrar antes a categoria")
        End If


    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click

        'Salvar datas no MYSQL
        Dim data As String
        data = Now.ToString("yyyy-MM-dd")
        'MsgBox(data)

        If txtNome.Text <> "" Or txtRaca.Text <> "" Or txtIdade.Text <> "" Then

            Try
                abrir()
                Dim cmd As MySqlCommand
                Dim sql As String
                sql = "INSERT INTO animais (id_categoria, nome, raca, idade, data_cadastro) 
                VALUES ('" & cbCategoria.SelectedValue & "','" & txtNome.Text & "', 
                '" & txtRaca.Text & "', '" & txtIdade.Text & "', '" & data & "')"
                cmd = New MySqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("Dados salvos com sucesso")
            Catch ex As Exception
                MsgBox("Erro ao salvar" + ex.Message)
            End Try

            btnSalvar.Enabled = False
            Limpar()
            Listar()


        Else

            MsgBox("Preencha os campos")
        End If
    End Sub

    Private Sub dg_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellClick
        btnEditar.Enabled = True
        btnExcluir.Enabled = True
        HabilitarCampos()
        txtCodigo.Text = dg.CurrentRow.Cells(0).Value
        cbCategoria.Text = dg.CurrentRow.Cells(1).Value
        txtNome.Text = dg.CurrentRow.Cells(2).Value
        txtRaca.Text = dg.CurrentRow.Cells(3).Value
        txtIdade.Text = dg.CurrentRow.Cells(4).Value
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If txtNome.Text <> "" Or txtRaca.Text <> "" Or txtIdade.Text <> "" Then

            'Dim descricao As String
            'descricao = txtNome.Text


            Try
                abrir()
                Dim cmd As MySqlCommand
                Dim sql As String
                sql = "UPDATE animais SET id_categoria = '" & cbCategoria.SelectedValue & "',  
                nome = '" & txtNome.Text & "', raca = '" & txtRaca.Text & "', idade = '" & txtIdade.Text & "'
                where id_animal = '" & txtCodigo.Text & "' "
                cmd = New MySqlCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("Dados Editados com sucesso")
            Catch ex As Exception
                MsgBox("Erro ao editar" + ex.Message)
            End Try

            btnEditar.Enabled = False
            btnExcluir.Enabled = False
            DesabilitarCampos()
            Limpar()
            Listar()

        Else
            MsgBox("Preencha os campos")
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click

        If txtNome.Text <> "" Or txtRaca.Text <> "" Or txtIdade.Text <> "" Then

            Dim result As DialogResult =
        MessageBox.Show("Confirmar Exclusão?", "Excluir Registro",
        MessageBoxButtons.YesNo)
            If result = vbYes Then
                Try
                    abrir()
                    Dim cmd As MySqlCommand
                    Dim sql As String
                    sql = "DELETE from animais where id_animal = '" & txtCodigo.Text & " ' "
                    cmd = New MySqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    MsgBox("Dado Excluído com sucesso")
                Catch ex As Exception
                    MsgBox("Erro ao excluir" + ex.Message)
                End Try
                btnEditar.Enabled = False
                btnExcluir.Enabled = False
                DesabilitarCampos()
                Limpar()
                Listar()

            End If

        Else
            MsgBox("Selecione um registro para excluir")
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Buscar()
    End Sub

    Private Sub Buscar()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select a.id_animal, c.descricao, a.nome, a.raca, a.idade, a.data_cadastro, a.id_categoria 
            From animais as a INNER JOIN categorias as c on a.id_categoria = c.id_categoria where 
            nome LIKE '" & txtBuscar.Text & "%' and a.id_categoria = '" & cbCategoriaBuscar.SelectedValue & "' order by nome asc"

            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)
            dg.DataSource = dt

            FormatarDG()
            ContarLinhas()

        Catch ex As Exception
            MsgBox("Erro ao listar os dados" + ex.Message)
        End Try
    End Sub

    Private Sub dg_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class