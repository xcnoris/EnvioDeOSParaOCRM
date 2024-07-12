using EnvioDeOSParaOCRM.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.Modelos
{
    internal class RelacaoOScomCRM
    {
        public string Message;
        public bool Status;

        private ConexaoDB _conexaoDBRelOSCRM;
        private ComandosDB _comandosDB;

        public RelacaoOScomCRM()
        {
            _conexaoDBRelOSCRM = new ConexaoDB(2);
            _comandosDB = new ComandosDB(_conexaoDBRelOSCRM);
        }



        public int Id { get; set; }
        public int Id_Ordem_Servico { get; set; }
        public int Cod_Oportunidade { get; set; }
        public int Id_Categoria_OrdemServico { get; set; }


    }
}
