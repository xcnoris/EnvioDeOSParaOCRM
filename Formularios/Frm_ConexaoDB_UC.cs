using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM.Formularios
{
    public partial class Frm_ConexaoDB_UC : UserControl
    {
        public string Servidor
        {
            get { return Txt_Servidor.Text; }
        }
        public string IpHost
        {
            get { return Txt_IpHost.Text; }
        }
        public string DataBase
        {
            get { return Txt_DataBase.Text; }
        }
        public string Usuario
        {
            get { return Txt_Usuario.Text; }
        }
        public string Senha
        {
            get { return Txt_Senha.Text; }
        }
        public Frm_ConexaoDB_UC()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
