using EnvioDeOSParaOCRM.DataBase;
using EnvioDeOSParaOCRM.Formularios;
using EnvioDeOSParaOCRM.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM
{
    public partial class FrmTelaPrincipal : Form
    {
        private InserirOportunidade InserirOpn;
        private const string Token = "F65F9082EE9DB13A464B5DC0A9F2B8D56840CA3A1178826B0DF17DA2CE7DD621";

        private Frm_ConexaoDB_UC FrmcoenxaoUC;
        private Frm_Log FrmLog;
        

        public FrmTelaPrincipal()
        {
            InitializeComponent();


            FrmcoenxaoUC = new Frm_ConexaoDB_UC();
            FrmLog = new Frm_Log();
            InserirOpn = new InserirOportunidade();

            AdicionarUserControls();

            // Timer para executar a função periodicamente
            Timer timer = new Timer();
            timer.Interval = 10000; // 10 segundos
            timer.Tick += async (s, e) =>
            {
                try
                {
                    await InserirOpn.VerificarNovosServicos(Token);
                }
                catch (Exception ex)
                {
                    // Log de erro
                    Console.WriteLine($"[ERROR]: {ex.Message}");
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

            // Crie a segunda aba "Logs"
            TabPage TB2 = new TabPage
            {
                Name = "Logs",
                Text = "Logs"
            };
            TB2.Controls.Add(FrmLog);

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
                ConexaoDB conexao = LeituraFormulario();

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(basePath, "conexao.json");

                conexao.SaveConnectionData(filePath);
            }
            catch(Exception ex)
            {

            }
            //SalvarArquivo();
        }

        private ConexaoDB LeituraFormulario()
        {
            return new ConexaoDB
            {

                Servidor = FrmcoenxaoUC.Servidor,
                IpHost = FrmcoenxaoUC.IpHost,
                DataBase = FrmcoenxaoUC.DataBase,
                Usuario = FrmcoenxaoUC.Usuario,
                Senha = FrmcoenxaoUC.Senha
            };
        }

        private void SalvarArquivo()
        {
            ConexaoDB conexao = new ConexaoDB();
            conexao.Servidor = "192.168.0.254";
            conexao.IpHost = "192.168.0.254";
            conexao.DataBase = "LojamixNovo";
            conexao.Usuario = "Lojamix";
            conexao.Senha = "l0j4m1x";

            conexao.SaveConnectionData("caminho_para_o_arquivo.json");

        }
    }
}
