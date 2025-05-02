using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atividade_GestaodeProdutos
{
    public partial class UcCadastro : UserControl
    {
        public delegate void CadastroConcluidoEventHandler(object sender, EventArgs e);
        public event CadastroConcluidoEventHandler CadastroConcluido;

        public UcCadastro()
        {
            InitializeComponent();
        }

        private void btnCadastrarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TxtUserUsuario.Text) || !string.IsNullOrWhiteSpace(txtSenhaUsuario.Text) || !string.IsNullOrWhiteSpace(TxtCpfCadastro.Text) || !string.IsNullOrWhiteSpace(TxtEmailUsuario.Text) || !string.IsNullOrWhiteSpace(TxtNomeUsuario.Text))
                {
                    Usuarios usuario = new Usuarios();
                    usuario.Nome = TxtNomeUsuario.Text;
                    usuario.Cpf = TxtCpfCadastro.Text;
                    usuario.Email = TxtEmailUsuario.Text;
                    usuario.Usuario = TxtUserUsuario.Text;
                    usuario.Senha = txtSenhaUsuario.Text;

                    if (usuario.ExisteCpfeEmail())
                    {
                        MessageBox.Show("Esse usuário já está cadastrado!", "Aviso - Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LimparCampos();
                        return;
                    }

                    if (usuario.CadastrarUsuario())
                    {
                        MessageBox.Show("Cadastro feito com sucesso!" + TxtUserUsuario + " cadastrado com sucesso!", "Sucesso - cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();

                        CadastroConcluido?.Invoke(this, EventArgs.Empty);
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
            }
        }
        public void LimparCampos()
        {
            txtSenhaUsuario.Clear();
            TxtCpfCadastro.Clear();
            TxtEmailUsuario.Clear();
            TxtNomeUsuario.Clear();
            TxtUserUsuario.Clear();
        }

        private void UcCadastro_Load(object sender, EventArgs e)
        {

        }
    }
}
