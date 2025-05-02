using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Cms;
using BCrypt.Net;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Atividade_GestaodeProdutos
{
    class Produtos
    {
        private string nome;
        private string categoria;
        private int quantidade;
        private decimal preco;
        private int id;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public string Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }
        public decimal Preco
        {
            get { return preco; }
            set { preco = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool CadastrarProduto()
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string inserir = "INSERT INTO produtos (nome, preco, categoria, quantidade) values (@nome, @preco, @categoria, @quantidade)";

                    MySqlCommand comando = new MySqlCommand(inserir, conexaoBanco);

                    comando.Parameters.AddWithValue("@nome", Nome);
                    comando.Parameters.AddWithValue("@preco", Preco);
                    comando.Parameters.AddWithValue("@categoria", Categoria);
                    comando.Parameters.AddWithValue("@quantidade", Quantidade);

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
                MessageBox.Show("Erro ao cadastrar produto - Método -> " + ex.Message, "Erro - Cadastrar produto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public bool ExcluirProdutos()
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string delete = "DELETE FROM produtos WHERE nome = @nome and preco = @preco and quantidade = @quantidade and categoria = @categoria ;";
                    MySqlCommand deletarSql = new MySqlCommand(delete, conexaoBanco);
                    deletarSql.Parameters.AddWithValue("@nome", Nome);
                    deletarSql.Parameters.AddWithValue("@preco", Preco);
                    deletarSql.Parameters.AddWithValue("@quantidade", Quantidade);
                    deletarSql.Parameters.AddWithValue("@categoria", Categoria);

                    int resultado = deletarSql.ExecuteNonQuery();
                    return resultado > 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir produto - Método -> " + ex.Message, "Erro - Excluir produto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public bool EditarPodutos()
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string editar = "Update produtos set nome = @nome, preco = @preco, quantidade = @quantidade, categoria = @categoria where id = @id;";
                    MySqlCommand editarSql = new MySqlCommand(editar, conexaoBanco);
                    editarSql.Parameters.AddWithValue("@id", Id);
                    editarSql.Parameters.AddWithValue("@nome", Nome);
                    editarSql.Parameters.AddWithValue("@categoria", Categoria);
                    editarSql.Parameters.AddWithValue("@quantidade", Quantidade);
                    editarSql.Parameters.AddWithValue("@preco", Preco);

                    int resultado = editarSql.ExecuteNonQuery();
                    return resultado > 0;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar produto - Método -> " + ex.Message, "Erro - Editar produto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public void ListarPorNomesouCategoria(string nomeBusca, DataGridView grid)
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string select = " SELECT * FROM produtos WHERE nome LIKE @nome OR categoria LIKE @categoria;";

                    MySqlCommand selectSql = new MySqlCommand(select, conexaoBanco);
                    selectSql.Parameters.AddWithValue("@nome", "%" + nomeBusca + "%");
                    selectSql.Parameters.AddWithValue("@categoria", "%" + nomeBusca + "%");

                    MySqlDataReader readerSelect = selectSql.ExecuteReader();
                    grid.Rows.Clear();

                    while (readerSelect.Read())
                    {
                        string id = readerSelect["id"].ToString();
                        string nome = readerSelect["nome"].ToString();
                        string categoria = readerSelect["categoria"].ToString();
                        string preco = readerSelect["preco"].ToString();
                        string quantidade = readerSelect["quantidade"].ToString();
                        grid.Rows.Add(id, nome, categoria, preco, quantidade);
                    }

                    readerSelect.Close();

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro ao procurar produto - Método -> " + ex.Message, "Erro - Procurar Produto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public void ListarProdutos(DataGridView grid)
        {
            using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
            {
                string select = "select * from produtos;";

                MySqlCommand comando = new MySqlCommand(select, conexaoBanco);
                MySqlDataReader readerSelect = comando.ExecuteReader();

                grid.Rows.Clear();

                while (readerSelect.Read())
                {
                    string id = readerSelect["id"].ToString();
                    string nome = readerSelect["nome"].ToString();
                    string categoria = readerSelect["categoria"].ToString();
                    string preco = readerSelect["preco"].ToString();
                    string quantidade = readerSelect["quantidade"].ToString();
                   
                    grid.Rows.Add(id, nome, categoria, preco, quantidade);
                }

                readerSelect.Close();

            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        public bool ExisteNome()
        {
            try
            {
                using (MySqlConnection conexaoBanco = new ConexaoBD().Conectar())
                {
                    string select = "SELECT COUNT(*) FROM produtos WHERE nome = @nome;";
                    MySqlCommand comando = new MySqlCommand(select, conexaoBanco);
                    comando.Parameters.AddWithValue("@nome", Nome);

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
        //----------------------------------------------------------------------------------------------------------------------------------//

    }
}
