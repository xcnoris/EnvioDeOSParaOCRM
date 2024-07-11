using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.Modelos
{
    internal class OrdemServiçoRequest
    {
        public string codigoApi { get; set; }
        public string codigoOportunidade { get; set; }
        public string origemOportunidade { get; set; }
        public string nomeLead { get; set; }
        public Lead lead { get; set; }
        public Contato contato { get; set; }
        public List<Followup> followups { get; set; }
    }
}
