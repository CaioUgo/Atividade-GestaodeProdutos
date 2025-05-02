using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Guna.UI2.WinForms;
using System.Runtime.InteropServices;

namespace Atividade_GestaodeProdutos
{
    public partial class Redefinir : Form
    {
        public Redefinir()
        {
            InitializeComponent();
        }

        private void btnRedefinirSenha_Click(object sender, EventArgs e)
        {
            try
            {
                string email = TxtEmailRedefinir.Text.Trim();
                string novaSenha = txtSenhaNova.Text;
                string confirmarSenha = txtConfirmarSenha.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(novaSenha) || string.IsNullOrEmpty(confirmarSenha))
                {
                    MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (novaSenha != confirmarSenha)
                {
                    MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Usuarios usuario = new Usuarios();

                if (usuario.RedefinirSenha(email, novaSenha))
                {
                    MessageBox.Show("Senha redefinida com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();

                    LoginPrincipal FormLogin = new LoginPrincipal();

                    FormLogin.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erro ao redefinir senha. Verifique o e-mail.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível redefinir senha: " + ex.Message, "Erro - Redefinir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimparCampos()
        {
            TxtEmailRedefinir.Clear();
            txtSenhaNova.Clear();
            txtConfirmarSenha.Clear();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            LoginPrincipal FormLogin = new LoginPrincipal();

            FormLogin.Show();

            this.Close();
        }

        private void Redefinir_Load(object sender, EventArgs e)
        {

        }
    }
}

