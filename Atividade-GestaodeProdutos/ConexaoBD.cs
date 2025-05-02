using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Atividade_GestaodeProdutos
{
    class ConexaoBD
    {
        private string conexaoBanco = "Server=localhost; Database=GestaoProdutos; Uid=root; Pwd='';";

        public MySqlConnection Conectar()
        {
            MySqlConnection conexao = new MySqlConnection(conexaoBanco);

            conexao.Open();

            return conexao;
        }

    }
}
