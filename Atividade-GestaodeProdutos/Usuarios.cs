using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Atividade_GestaodeProdutos
{
    class Usuarios
    {
        private string email;
        private string senha;
        private string usuario;
        private string nome;
        private string cpf;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public string Nome
        {
            get { return nome; }
            set { nome = value ; }
        }
        public string Senha
        {
            get { return senha; }
            set { senha = value; }

             
        }
        public string Email
        {
            get { return email; }
            set
            {
                if (!verificarEmail(value))
                    throw new Exception("Email inválido.");
                email = value;
            }
        }
    
        public string Cpf
        {
            get { return cpf;  }
            set { cpf = value; }    
        }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool verificarEmail(string email)
        {
            string emailValido = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailValido);
            return regex.IsMatch(email);
        }
//----------------------------------------------------------------------------------------------------------------------------------//
        public bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            string cpfVerificado = tempCpf + digito2;
            return cpf == cpfVerificado;
        }
//----------------------------------------------------------------------------------------------------------------------------------//
        
        public bool CadastrarUsuario()
        {
            try
            {
                using(MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string inserir = "INSERT INTO usuarios (nome, cpf, email, senha, usuario) values (@nome, @cpf, @email, @senha, @usuario)";

                    MySqlCommand comando = new MySqlCommand(inserir, conexaoBanco);

                    comando.Parameters.AddWithValue("@nome", Nome);
                    comando.Parameters.AddWithValue("@cpf", Cpf);
                    comando.Parameters.AddWithValue("@email", Email);
                    comando.Parameters.AddWithValue("@senha", Senha);
                    comando.Parameters.AddWithValue("@usuario", Usuario);

                    int resultado = comando.ExecuteNonQuery();
                    
                    if(resultado > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar usuário - Método -> " + ex.Message, "Erro - Cadastrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public bool RedefinirSenha(string email, string novaSenha)
        {
            try
            {
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);

                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string update = "UPDATE usuarios SET senha = @senha WHERE email = @email";

                    MySqlCommand comando = new MySqlCommand(update, conexaoBanco);
                    comando.Parameters.AddWithValue("@senha", senhaHash);
                    comando.Parameters.AddWithValue("@email", email);

                    int resultado = comando.ExecuteNonQuery();

                    if (resultado > 0)
                    {
                        return true;  
                    }
                    else
                    {
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao redefinir a senha: " + ex.Message, "Erro - Redefinir Senha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public bool ExisteCpfeEmail()
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string select = "SELECT COUNT(*) FROM usuarios WHERE email = @email and cpf = @cpf;";
                    MySqlCommand comando = new MySqlCommand(select, conexaoBanco);
                    comando.Parameters.AddWithValue("@email", Email);
                    comando.Parameters.AddWithValue("@cpf", Cpf);

                    int count = Convert.ToInt32(comando.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar nome existente - Método -> " + ex.Message, "Erro - Verificar Nome", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------
        public static Usuarios ObterPorLoginOuEmail(string login)
        {
            using (MySqlConnection conexaoBD = new ConexaoBD().Conectar())
            {
                string sql = "SELECT nome, cpf, email, senha as SenhaHas, usuario FROM usuarios WHERE usuario = @login OR email = @login";

                using var cmd = new MySqlCommand(sql, conexaoBD);
                cmd.Parameters.AddWithValue("@login", login);

                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return null;

                return new Usuarios
                {
                    Nome = reader.GetString("nome"),
                    Cpf = reader.GetString("cpf"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("SenhaHas"),
                    Usuario = reader.GetString("usuario")
                };
            }
        }
        public void DefinirSenha(string senhaPura)
        {
            senha = BCrypt.Net.BCrypt.HashPassword(senhaPura);
        }
        public bool VerificarSenha(string senhaDigitada, string hashArmazenado)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashArmazenado);
        }
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 




