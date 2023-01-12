Imports MySql.Data.MySqlClient

Public Class frmFuncionarios

    Private Sub frmFuncionarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Listar()
    End Sub

    Sub Limpar()
        txtBuscar.Text = ""
        txtCodigo.Text = ""
        txtNome.Text = ""
        txtEndereco.Text = ""
        txtCPF.Text = ""
        txtTelefone.Text = ""
        txtSenha.Text = ""
        txtUsuario.Text = ""
        txtEmail.Text = ""
    End Sub

    Sub HabilitarCampos()
        txtNome.Enabled = True
        txtCPF.Enabled = True
        txtEndereco.Enabled = True
        txtTelefone.Enabled = True
        txtSenha.Enabled = True
        cbCargo.Enabled = True
        txtUsuario.Enabled = True
        txtEmail.Enabled = True
    End Sub

    Sub DesabilitarCampos()
        txtNome.Enabled = False
        txtCPF.Enabled = False
        txtEndereco.Enabled = False
        txtTelefone.Enabled = False
        txtSenha.Enabled = False
        cbCargo.Enabled = False
        txtUsuario.Enabled = False
        txtEmail.Enabled = False
    End Sub

    Private Sub Listar()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select * from funcionarios order by nome asc"


            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)
            dg.DataSource = dt

            'FormatarDG()
            'ContarLinhas()

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

        dg.Columns(0).HeaderText = "Id do Funcionario"
        dg.Columns(1).HeaderText = "Nome"
        dg.Columns(2).HeaderText = "Cargo"
        dg.Columns(3).HeaderText = "Endereço"
        dg.Columns(4).HeaderText = "Telefone"
        dg.Columns(5).HeaderText = "CPF"
        dg.Columns(6).HeaderText = "Senha"
        dg.Columns(7).HeaderText = "Usuário"
        dg.Columns(8).HeaderText = "Email"

        dg.Columns(0).Width = 80
        dg.Columns(1).Width = 80

    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Limpar()
        HabilitarCampos()
        btnSalvar.Enabled = True
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        If txtNome.Text <> "" Or txtCPF.Text <> "" Or txtSenha.Text <> "" Then

            Try
                'VERIFICAR SE USUÁRIO JÁ EXITE
                abrir()
                Dim cmdUser As MySqlCommand
                Dim reader As MySqlDataReader
                Dim sql2 As String
                sql2 = "select * from funcionarios where usuario = '" & txtUsuario.Text & "' "
                cmdUser = New MySqlCommand(sql2, con)
                reader = cmdUser.ExecuteReader

                If reader.Read = True Then
                    MsgBox("Este nome de usuário já existe no banco de dados")
                    fechar()
                Else
                    fechar()
                    abrir()
                    Dim cmd As MySqlCommand
                    Dim sql As String
                    sql = "INSERT INTO funcionarios (nome, cargo, endereco, telefone, cpf, senha, usuario, email) 
                VALUES ('" & txtNome.Text & "','" & cbCargo.Text & "', 
                '" & txtEndereco.Text & "', '" & txtTelefone.Text & "', '" & txtCPF.Text & "', 
                '" & txtSenha.Text & "', '" & txtUsuario.Text & "', '" & txtEmail.Text & "' )"
                    cmd = New MySqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    MsgBox("Dados salvos com sucesso")
                    btnSalvar.Enabled = False
                    Limpar()
                    Listar()
                End If



            Catch ex As Exception
                MsgBox("Erro ao salvar" + ex.Message)
            End Try


        Else

            MsgBox("Preencha os campos")
        End If

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If txtNome.Text <> "" Or txtCPF.Text <> "" Or txtSenha.Text <> "" Then

            'Dim descricao As String
            'descricao = txtNome.Text


            Try
                abrir()
                Dim cmd As MySqlCommand
                Dim sql As String
                sql = "UPDATE funcionarios SET nome = '" & txtNome.Text & "',  
                cargo = '" & cbCargo.Text & "', endereco = '" & txtEndereco.Text & "'
                , telefone = '" & txtTelefone.Text & "' , cpf = '" & txtCPF.Text & "', 
                senha = '" & txtSenha.Text & "', usuario = '" & txtUsuario.Text & "', email = '" & txtEmail.Text & "'
                where id_funcionario = '" & txtCodigo.Text & "' "
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


    Private Sub dg_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellClick
        btnEditar.Enabled = True
        btnExcluir.Enabled = True
        HabilitarCampos()
        txtCodigo.Text = dg.CurrentRow.Cells(0).Value
        txtNome.Text = dg.CurrentRow.Cells(1).Value
        cbCargo.Text = dg.CurrentRow.Cells(2).Value
        txtEndereco.Text = dg.CurrentRow.Cells(3).Value
        txtTelefone.Text = dg.CurrentRow.Cells(4).Value
        txtCPF.Text = dg.CurrentRow.Cells(5).Value
        txtSenha.Text = dg.CurrentRow.Cells(6).Value
        txtUsuario.Text = dg.CurrentRow.Cells(7).Value
        txtEmail.Text = dg.CurrentRow.Cells(8).Value
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If txtCodigo.Text <> "" Then

            Dim result As DialogResult =
        MessageBox.Show("Confirmar Exclusão?", "Excluir Registro",
        MessageBoxButtons.YesNo)
            If result = vbYes Then
                Try
                    abrir()
                    Dim cmd As MySqlCommand
                    Dim sql As String
                    sql = "DELETE from funcionarios where id_funcionario = '" & txtCodigo.Text & " ' "
                    cmd = New MySqlCommand(sql, con)
                    cmd.ExecuteNonQuery()
                    MsgBox("Dados Excluído com sucesso")
                Catch ex As Exception
                    MsgBox("Erro ao excluir" + ex.Message)
                End Try
                btnEditar.Enabled = False
                btnExcluir.Enabled = False
                txtNome.Text = ""
                Listar()

            End If


        Else
            MsgBox("Selecione um registro para excluir")
        End If
    End Sub

    Private Sub Buscar()
        Try
            abrir()
            Dim dt As New DataTable
            Dim da As MySqlDataAdapter
            Dim sql As String
            sql = "select * from funcionarios where nome LIKE '" & txtBuscar.Text & "%'
            and cargo = '" & cbCargoBuscar.Text & "'
            order by nome asc"

            da = New MySqlDataAdapter(sql, con)
            da.Fill(dt)
            dg.DataSource = dt

            FormatarDG()
            ContarLinhas()

        Catch ex As Exception
            MsgBox("Erro ao listar os dados" + ex.Message)
        End Try
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Buscar()
    End Sub

    Private Sub cbCargoBuscar_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbCargoBuscar.SelectedValueChanged
        Buscar()
    End Sub

    Private Sub cbCargoBuscar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCargoBuscar.SelectedIndexChanged

    End Sub
End Class