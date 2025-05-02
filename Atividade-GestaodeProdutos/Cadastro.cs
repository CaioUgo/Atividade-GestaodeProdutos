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
    public partial class Cadastro : Form
    {
        public Cadastro()
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
                    usuario.Cpf = TxtCpfCadastro.Text.Trim();
                    usuario.Email = TxtEmailUsuario.Text;
                    usuario.Usuario = TxtUserUsuario.Text;
                    usuario.DefinirSenha(txtSenhaUsuario.Text);

                    if (!usuario.ValidarCPF(usuario.Cpf))
                    {
                        MessageBox.Show("Esse CPF não é válido!", "Aviso - Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
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

                        LoginPrincipal FormLogin = new LoginPrincipal();

                        FormLogin.Show();

                        this.Close();
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

        private void btnFecharCad_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizarCad_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            LoginPrincipal FormLogin = new LoginPrincipal();

            FormLogin.Show();

            this.Close();
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
    }
}
