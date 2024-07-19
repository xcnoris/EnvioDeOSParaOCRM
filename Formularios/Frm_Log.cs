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
    public partial class Frm_Log : UserControl
    {
        //public string TextoBoxLog 
        //{
        //    get
        //}
        public Frm_Log()
        {
            InitializeComponent();
        }

        internal void AdiconarMenssageLog(string mensagem)
        {
            Txt_Logs.Text += $"{mensagem}\n";
        }
    }
}
