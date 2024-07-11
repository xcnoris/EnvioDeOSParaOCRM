using EnvioDeOSParaOCRM.DataBase;
using EnvioDeOSParaOCRM.Modelos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.Metodos
{
    internal class InserirOportunidade
    {
        private ConexaoDB _conexaoDB;
        private ComandosDB _comandosDB;
       

        public InserirOportunidade()
        {
            _conexaoDB = new ConexaoDB();
            _comandosDB = new ComandosDB(_conexaoDB);
        }

      


        public async Task VerificarNovosServicos(string token)
        {
            string query = "SELECT id_ordem_servico, nome_cliente, fone_ddd_cliente + fone_numero_cliente AS telefone, email_cliente FROM ordem_servico WHERE data_hora_cadastro >= '01/07/2024' AND id_ordem_servico = 7636";
            DataTable servicosTable = _comandosDB.ExecuteQuery(query);

            foreach (DataRow row in servicosTable.Rows)
            {
                OrdemServiçoRequest oportunidade = new OrdemServiçoRequest
                {
                    codigoApi = "4B29E80B1A",
                    codigoOportunidade = "",
                    origemOportunidade = "Lojamix",
                    lead = new Lead
                    {
                        nomeLead = row["nome_cliente"].ToString(),
                        telefoneLead = row["telefone"].ToString(),
                        emailLead = row["email_cliente"].ToString(),
                        cnpjLead = "07446072000106",
                        origemLead = "Teste api",
                        contatos = new List<Contato>
                {
                    new Contato
                    {
                        nomeContato = row["nome_cliente"].ToString(),
                        telefoneContato = row["telefone"].ToString(),
                        emailContato = row["email_cliente"].ToString()
                    }
                }
                    },
                    contato = new Contato
                    {
                        nomeContato = row["nome_cliente"].ToString(),
                        telefoneContato = row["telefone"].ToString(),
                        emailContato = row["email_cliente"].ToString()
                    },
                    followups = new List<Followup>
            {
                new Followup { textoFollowup = "Teste - Essa oportunidade foi criada a partir da API de integração da LeadFinder" },
                new Followup { textoFollowup = "Teste - É possível inserir followups com os históricos da oportunidade via API" }
            }
                };

                OportunidadeResponse response = await EnviarOrdemServiçoForCRM.EnviarOportunidade(oportunidade, token);

                if (response != null)
                {
                    
                    Console.WriteLine($"Oportunidade criada com código: {response.CodigoOportunidade} - {row["id_ordem_servico"].ToString()}");
                }
                else
                {
                    Console.WriteLine("Erro ao criar a oportunidade.");
                }
            }
        }


    }
}
