using EnvioDeOSParaOCRM.Formularios;
using EnvioDeOSParaOCRM.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        public FrmTelaPrincipal()
        {
            InitializeComponent();

            AdiconarUserControls();

            InserirOpn = new InserirOportunidade();

            // Timer para executar a função periodicamente
            Timer timer = new Timer();
            timer.Interval = 10000; // 10 segundos
            timer.Tick += async (s, e) => await InserirOpn.VerificarNovosServicos(Token);
            timer.Start();
        }

        private void FrmTelaPrincipal_SizeChanged(object sender, EventArgs e)
        {
            bool MousePointerNotOnTaskBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if (this.WindowState == FormWindowState.Minimized && MousePointerNotOnTaskBar)
            {
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "Seu programa esta sendo minimizado para a bandeja do windows";
                notifyIcon1.ShowBalloonTip(1000);
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }

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

        private void AdiconarUserControls()
        {
            Frm_Log FrmLog = new Frm_Log(); 

            TabPage TB = new TabPage();
            TB.Name = "Geral";
            TB.Text = "Geral";
            TB.Controls.Add(FrmLog);
            

            TBC_Dados.TabPages.Add(TB);
        }
    }
}
