using EnvioDeOSParaOCRM.DataBase;
using System;
using System.Data;

namespace EnvioDeOSParaOCRM.Metodos
{
    internal class BuscarOrdemDeServicoInDB
    {
        private ConexaoDB _conexaoDB;
        private ComandosDB _comandosDB;

        public BuscarOrdemDeServicoInDB()
        {
            _conexaoDB = new ConexaoDB(1);
            _comandosDB = new ComandosDB(_conexaoDB);
        }

        internal DataTable BuscarOrdemDeServiçoInDB()
        {
            try
            {

                // Buscar serviços no banco de dados a partir de uma data ou parâmetro definido
                string query = "SELECT id_ordem_servico, nome_cliente, fone_ddd_cliente + fone_numero_cliente AS telefone, email_cliente, id_categoria_ordem_servico FROM ordem_servico WHERE data_hora_cadastro >= '01/07/2024' AND id_ordem_servico = 7648";

                // Converte o resultado do select em DataTable
                DataTable servicosTable = _comandosDB.ExecuteQuery(query);

                Console.WriteLine($"\nForam encontradas {servicosTable.Rows.Count} ordem de serviço no banco de dados\n");
                return servicosTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR]: {ex.Message} - {_comandosDB.Mensagem}");
                return null;
            }
        }
    }
}
