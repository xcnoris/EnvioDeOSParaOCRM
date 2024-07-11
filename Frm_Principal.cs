using EnvioDeOSParaOCRM.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM
{
    public partial class Frm_Principal : Form
    {
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private InserirOportunidade InserirOpn;
        private const string Token = "F65F9082EE9DB13A464B5DC0A9F2B8D56840CA3A1178826B0DF17DA2CE7DD621";

        public Frm_Principal()
        {
            InitializeComponent();

            InserirOpn = new InserirOportunidade();
            // Timer para executar a função periodicamente
            Timer timer = new Timer();
            timer.Interval = 10000; // 60 segundos
            timer.Tick += async (s, e) => await InserirOpn.VerificarNovosServicos(Token);
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}