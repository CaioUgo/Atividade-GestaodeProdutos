    using Guna.UI2.WinForms;
    using System.Runtime.InteropServices;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    namespace Atividade_GestaodeProdutos
    {
    public partial class LoginPrincipal : Form
    {
        public LoginPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnFechar.PressedState.ImageSize = new Size(10, 10);
            btnMinimizar.PressedState.ImageSize = new Size(12, 12);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private void panelCabecalho_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        } 
        private void linkCadastrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cadastro FormCadastro = new Cadastro();

            FormCadastro.Show();

            this.Hide();
        }

        private void linkRedefinir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Redefinir FormRedefinir = new Redefinir();

            FormRedefinir.Show();

            this.Hide();
        }
        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnLoginUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string login = TxtEmailUsuario.Text;
                string senha = txtSenhaUsuario.Text;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
                {
                    MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                var usuario = Usuarios.ObterPorLoginOuEmail(login);
                if (usuario != null && usuario.VerificarSenha(senha, usuario.Senha))
                {
                    MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    GestaoProd FormProd = new GestaoProd(usuario.Usuario);

                    FormProd.Show();

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Não foi possível realizar o login: " + ex.Message, "Erro - Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimparCampos();
            }
        }

        public void LimparCampos()
        {
            TxtEmailUsuario.Clear();
            txtSenhaUsuario.Clear();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////



    }
}
