Imports MySql.Data.MySqlClient

Module Conexao

    Public con As New MySqlConnection("server=localhost;userid=root;password=;database=sistema_clinica;")


    Sub abrir()  
       if con.State = 0 then
            con.Open()

        End if
            

    End Sub
    
    Sub fechar()  
       if con.State = 1 then
            con.Close()

        End if
            

    End Sub

End Module
