using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.Modelos
{
    internal class OportunidadeResponse
    {
        public string CodigoOportunidade { get; set; }
        public OrdemServiçoRequest OS { get; set; }
    }
}
