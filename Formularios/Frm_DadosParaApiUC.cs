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
    }
}
