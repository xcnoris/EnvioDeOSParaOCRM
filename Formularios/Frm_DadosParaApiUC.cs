using EnvioDeOSParaOCRM.DataBase;
using EnvioDeOSParaOCRM.Metodos;
using EnvioDeOSParaOCRM.Modelos;
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

namespace EnvioDeOSParaOCRM.Formularios
{
    public partial class Frm_DadosParaApiUC : UserControl
    {
        internal string Token
        {
            get
            {
                return Txt_Token.Text;
            } 
        }


        public Frm_DadosParaApiUC()
        {
            InitializeComponent();
        }

        private void Frm_DadosParaApiUC_Load(object sender, EventArgs e)
        {
            try
            {
                DadosParaAPI DadosApi = new DadosParaAPI();
                DadosApi.BuscarTokenInDB();
                if (DadosApi.Token != null)
                {
                    Txt_Token.Text = DadosApi.Token;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados de conexão: " + ex.Message);
            }
        }
    }
}
