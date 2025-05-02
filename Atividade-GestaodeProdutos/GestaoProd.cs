using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atividade_GestaodeProdutos
{
    public partial class GestaoProd : Form
    {
        public GestaoProd(string nomeUsuario)
        {
            InitializeComponent();
            GestaoProd_Load(this, EventArgs.Empty);
            lblUsuario.Text = nomeUsuario;
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            LoginPrincipal FormLogin = new LoginPrincipal();

            FormLogin.Show();

            this.Close();
        }
        //----------------------------------------------------------------------------------------------------------------------------------//
        private void btnCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtNome.Text) || !string.IsNullOrWhiteSpace(txtCategoria.Text) || !string.IsNullOrWhiteSpace(txtPreco.Text) || !string.IsNullOrWhiteSpace(txtQuantidade.Text))
                {
                    Produtos produto = new Produtos();
                    produto.Nome = txtNome.Text;
                    produto.Quantidade = int.Parse(txtQuantidade.Text);
                    produto.Categoria = txtCategoria.Text;
                    produto.Preco = Convert.ToDecimal(txtPreco.Text);

                    if (produto.ExisteNome())
                    {
                        MessageBox.Show("Esse produto já está cadastrado!", "Aviso - Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LimparCampos();
                        return;
                    }

                    if (produto.CadastrarProduto())
                    {
                        produto.ListarProdutos(DataListaProdutos);
                        MessageBox.Show("Produto: " + txtNome.Text + " cadastrado com sucesso!", "Sucesso - cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível cadastrar", "Erro - cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LimparCampos();
                    }
                }
                else
                {
                    MessageBox.Show("Favor preencher corretamente os campos!", "Erro - campos em branco", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível realizar o cadastro: " + ex.Message, "Erro - Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimparCampos();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Informe uma linha para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Produtos produto = new Produtos();
                produto.Id = int.Parse(txtID.Text);
                produto.Nome = txtNome.Text;
                produto.Quantidade = int.Parse(txtQuantidade.Text);
                produto.Categoria = txtCategoria.Text;
                produto.Preco = Convert.ToDecimal(txtPreco.Text);

                if (produto.EditarPodutos())
                {
                    MessageBox.Show("Produto: " + txtNome.Text + " editado com sucesso!", "Sucesso - edição", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    produto.ListarProdutos(DataListaProdutos);
                    LimparCampos();
                }
                else
                {
                    MessageBox.Show("Não foi possível editar", "Erro - Editar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimparCampos();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível realizar a edição: " + ex.Message, "Erro - Editar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimparCampos();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------//

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Informe uma linha para excluir!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Produtos produto = new Produtos();
                produto.Nome = txtNome.Text;
                produto.Quantidade = int.Parse(txtQuantidade.Text);
                produto.Categoria = txtCategoria.Text;
                produto.Preco = Convert.ToDecimal(txtPreco.Text);

                if (produto.ExcluirProdutos())
                {
                    MessageBox.Show("Produto: " + txtNome.Text + " exclusão com sucesso!", "Sucesso - exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    produto.ListarProdutos(DataListaProdutos);
                    LimparCampos();
                }
                else
                {
                    MessageBox.Show("Não foi possível excluir", "Erro - exclusão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimparCampos();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível realizar a exclusão: " + ex.Message, "Erro - Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimparCampos();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------//
        public void LimparCampos()
        {
            txtCategoria.Clear();
            txtNome.Clear();
            txtPreco.Clear();
            txtQuantidade.Clear();
            txtID.Clear();
        }

        private void GestaoProd_Load(object sender, EventArgs e)
        {
            DataListaProdutos.Columns.Clear();

            DataListaProdutos.ColumnCount = 5;
            DataListaProdutos.Columns[0].Name = "ID";
            DataListaProdutos.Columns[1].Name = "Nome";
            DataListaProdutos.Columns[2].Name = "Categoria";
            DataListaProdutos.Columns[3].Name = "Preco";
            DataListaProdutos.Columns[4].Name = "Quantidade";

            Produtos produto = new Produtos();
            produto.ListarProdutos(DataListaProdutos);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DataListaProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = DataListaProdutos.Rows[e.RowIndex];
                string id = row.Cells["id"].Value.ToString();
                string nome = row.Cells["nome"].Value.ToString();
                string categoria = row.Cells["categoria"].Value.ToString();
                string preco = row.Cells["preco"].Value.ToString();
                string quantidade = row.Cells["quantidade"].Value.ToString();

                txtID.Text = id;
                txtNome.Text = nome;
                txtCategoria.Text = categoria;
                txtQuantidade.Text = quantidade;
                txtPreco.Text = preco;
            }
        }

        private void txtProcurar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string ProcurarNome = txtProcurar.Text.Trim();

                Produtos produto = new Produtos();
                produto.ListarPorNomesouCategoria(ProcurarNome, DataListaProdutos);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private void PanelCabecalho_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
