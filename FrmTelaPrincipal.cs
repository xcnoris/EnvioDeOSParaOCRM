using EnvioDeOSParaOCRM.DataBase;
using EnvioDeOSParaOCRM.Formularios;
using EnvioDeOSParaOCRM.Metodos;
using EnvioDeOSParaOCRM.Modelos;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM
{
    public partial class FrmTelaPrincipal : Form
    {
        private InserirOportunidade InserirOpn;
        private const string Token = "F65F9082EE9DB13A464B5DC0A9F2B8D56840CA3A1178826B0DF17DA2CE7DD621";

        private Frm_ConexaoDB_UC FrmcoenxaoUC;
        private Frm_DadosParaApiUC FrmDadosApiUC;
        private Frm_Log FrmLog;
        

        public FrmTelaPrincipal()
        {
            InitializeComponent();


            FrmcoenxaoUC = new Frm_ConexaoDB_UC();
            FrmDadosApiUC = new Frm_DadosParaApiUC();
            FrmLog = new Frm_Log();
            InserirOpn = new InserirOportunidade(FrmLog);

            AdicionarUserControls();

            // Timer para executar a função periodicamente
            Timer timer = new Timer();
            timer.Interval = 3000000; // 5 min
            timer.Tick += async (s, e) =>
            {
                try
                {
                    await InserirOpn.VerificarNovosServicos(FrmDadosApiUC);
                }
                catch (Exception ex)
                {
                    // Log de erro
                    MetodosGerais.RegistrarLog($"[ERROR]: {ex.Message}");
                }
            };
            timer.Start();
        }

        private void FrmTelaPrincipal_SizeChanged(object sender, EventArgs e)
        {
            MinimizarParaBandeja();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void AdicionarUserControls()
        {
            // Defina a posição e o tamanho dos controles de usuário para se ajustar às abas
            FrmcoenxaoUC.Dock = DockStyle.Fill;

            // Crie a primeira aba "Geral"
            TabPage TB1 = new TabPage
            {
                Name = "Geral",
                Text = "Geral"
            };
            TB1.Controls.Add(FrmcoenxaoUC);

            TabPage TB2 = new TabPage
            {
                Name = "API",
                Text = "API"
            };
            TB2.Controls.Add(FrmDadosApiUC);



            // Adicione as abas ao TabControl
            TBC_Dados.TabPages.Add(TB1);
            TBC_Dados.TabPages.Add(TB2);
        }

        private void Btn_Fechar_Click(object sender, EventArgs e)
        {
            // Minimiza a janela
            this.WindowState = FormWindowState.Minimized;
            MinimizarParaBandeja();
        }

        internal void MinimizarParaBandeja()
        {
            // Verifica se o ponteiro do mouse está na área de trabalho (não na barra de tarefas)
            bool MousePointerNotOnTaskBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if (MousePointerNotOnTaskBar)
            {
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "Seu programa está sendo minimizado para a bandeja do Windows";
                notifyIcon1.ShowBalloonTip(1000);
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void Btn_Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                ConexaoDB conexao = LeituraFormularioConexao();
                DadosParaAPI dadosAPI = LeituraFrmDadosAPI();

                dadosAPI.InserirTokenInTable();

                if (dadosAPI.Status)
                {
                    MessageBox.Show("Valores Salvos", "Envio de Ordem de Serviço", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(basePath, "conexao.json");

                conexao.SaveConnectionData(filePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            //SalvarArquivo();
        }

        private ConexaoDB LeituraFormularioConexao()
        {
            try
            {
                // Caso algum dado seja nulo ele retorna uma mensagem
                if (string.IsNullOrEmpty(FrmcoenxaoUC.Servidor) ||
                    string.IsNullOrEmpty(FrmcoenxaoUC.IpHost) ||
                    string.IsNullOrEmpty(FrmcoenxaoUC.DataBase) ||
                    string.IsNullOrEmpty(FrmcoenxaoUC.Usuario) ||
                    string.IsNullOrEmpty(FrmcoenxaoUC.Senha))
                {
                    throw new ArgumentException("Todos os campos de conexão são obrigatórios.");
                }

                return new ConexaoDB
                {
                    Servidor = FrmcoenxaoUC.Servidor,
                    IpHost = FrmcoenxaoUC.IpHost,
                    DataBase = FrmcoenxaoUC.DataBase,
                    Usuario = FrmcoenxaoUC.Usuario,
                    Senha = FrmcoenxaoUC.Senha
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Envio de OS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MetodosGerais.RegistrarLog($"Erro: {ex.Message}");
                throw;
            }
        }

        private DadosParaAPI LeituraFrmDadosAPI()
        {
            try
            {
                // Caso algum dado seja nulo ele retorna uma mensagem
                if (string.IsNullOrEmpty(FrmDadosApiUC.Token))
                {
                    throw new ArgumentException("Todos os campos são obrigatórios.");
                }

                return new DadosParaAPI
                {
                    Token = FrmDadosApiUC.Token,
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Envio de OS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MetodosGerais.RegistrarLog($"Erro: {ex.Message}");
                throw;
            }
           
        }

    }
}
