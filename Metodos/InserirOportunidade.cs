using EnvioDeOSParaOCRM.DataBase;
using EnvioDeOSParaOCRM.Formularios;
using EnvioDeOSParaOCRM.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.Metodos
{
    internal class InserirOportunidade
    {
        public string Message;
        public bool Status = false;

        private ConexaoDB _conexaoDBLojamix;
        private ConexaoDB _conexaoDBRelOSCRM;

        private ComandosDB _comandosDBLojamix;
        private ComandosDB _comandosDBRelOSCR;
        private BuscarOrdemDeServicoInDB BuscarOS;

        private Frm_Log frm_log;
        public InserirOportunidade(Frm_Log frmLog)
        {
            frm_log = frmLog;
            _conexaoDBLojamix = new ConexaoDB(1);
            _conexaoDBRelOSCRM = new ConexaoDB(2);

            _comandosDBLojamix = new ComandosDB(_conexaoDBLojamix);
            _comandosDBRelOSCR = new ComandosDB(_conexaoDBRelOSCRM);

            BuscarOS = new BuscarOrdemDeServicoInDB();

        }



        public async Task VerificarNovosServicos(Frm_DadosParaApiUC DadosAPI)
        {
            try
            {
                MetodosGerais.RegistrarInicioLog();
                // Busca serviços no DB
                DataTable servicosTable = BuscarOS.BuscarOrdemDeServiçoInDB();

                // Passa por cada OS que retornar no select
                foreach (DataRow row in servicosTable.Rows)
                {
                    string id_ordemServico = row["id_ordem_servico"].ToString();
                    string id_Categoria = row["id_categoria_ordem_servico"].ToString();
                    string nomecliente = row["nome_cliente"].ToString();
                    string idosenome = $"{id_ordemServico} - {nomecliente}";
                    string codigoJornada = "C8DA5BD4D7";

                    // Verifica se a OS já esta na tabela de relação, caso ela este, significa que já existe um cady/oportunidade criada no CRM
                    VerificarOSinTableRelacao ServicoinTableRelacao = new VerificarOSinTableRelacao();
                    DataTable Tb = ServicoinTableRelacao.BuscaOSInDBRelacao(id_ordemServico);


                    // Log para verificação
                    MetodosGerais.RegistrarLog($"Verificando OS {id_ordemServico}...");

                    if (Tb.Rows.Count == 0)
                    {
                        // Log para verificação
                        MetodosGerais.RegistrarLog($"OS {id_ordemServico} não encontrada na tabela de relação.");

                        // Instancia a classe para a Ordem de Serviço que não foi encontrada na tabela Relacao_OrdemServico_CRM
                        OrdemServiçoRequest oportunidade = new OrdemServiçoRequest
                        {
                            codigoApi = "4B29E80B1A",
                            codigoOportunidade = "",
                            origemOportunidade = "Lojamix - Consumo API",
                            lead = new Lead
                            {
                                nomeLead = idosenome,
                                telefoneLead = row["telefone"].ToString(),
                                emailLead = row["email_cliente"].ToString(),
                                cnpjLead = "",
                                origemLead = "Serviço de consumo de API",
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
                        new Followup { textoFollowup = "Essa oportunidade foi criada a partir da API de integração da LeadFinder" },
                        new Followup { textoFollowup = "É possível inserir followups com os históricos da oportunidade via API" }
                    }
                        };

                        // tenta criar a oportunidade no CRM
                        OportunidadeResponse response = await EnviarOrdemServiçoForCRM.EnviarOportunidade(oportunidade, DadosAPI.Token);
                        ServicoinTableRelacao.InserirRelacaoInTable(Convert.ToInt32(id_ordemServico), response.CodigoOportunidade.ToString(), Convert.ToInt32(id_Categoria));

                        // Verifica se a Ordem de Serviço esta entrando com aguardando avaliação, caso nao esteja altera no CRM
                        if (!(Convert.ToInt32(id_Categoria) == 1))
                        {
                            string cod_oportunidade = response.CodigoOportunidade.ToString();

                            string codAcao = SelecionarCodAcao(id_Categoria);
                            string textoFolloup = SelecionarMensagemAtualizacao(id_Categoria);
                            AtualizarAcaoRequest AtualizarAcao = new AtualizarAcaoRequest
                            {
                                codigoOportunidade = cod_oportunidade,
                                codigoAcao = codAcao,
                                codigoJornada = codigoJornada,
                                textoFollowup = textoFolloup
                            };

                            // Atualize a categoria na tabela de relação se necessário

                            EnviarOrdemServiçoForCRM.AtualizarAcao(AtualizarAcao, DadosAPI.Token);

                            // Log para verificação
                            MetodosGerais.RegistrarLog($"Categoria atualizada para {id_Categoria} na tabela de relação para a OS {id_ordemServico}.");
                         
                        }

                        // Caso der certo a criação entra no if
                        if (ServicoinTableRelacao.Status)
                        {
                            try
                            {
                                if (ServicoinTableRelacao.Status)
                                {
                                    MetodosGerais.RegistrarLog($"Oportunidade criada com código: {ServicoinTableRelacao.Message} - {id_ordemServico}");

                                    MetodosGerais.RegistrarLog(ServicoinTableRelacao.Message);
                                    Message = ServicoinTableRelacao.Message;
                                    Status = true;
                                }
                                else
                                {
                                    MetodosGerais.RegistrarLog($"[ERROR]: {ServicoinTableRelacao.Message}");
                                    Message = $"[ERROR]: {ServicoinTableRelacao.Message}";
                                    Status = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                MetodosGerais.RegistrarLog($"[ERROR]: {ex.Message}");
                                Message = $"[ERROR]: {ex.Message}";
                                Status = false;
                            }

                        }
                        else
                        {
                            MetodosGerais.RegistrarLog("Erro ao criar a oportunidade.");
                        }
                    }
                    else
                    {
                        // Caso a OS esteja na tabela de relação. Deve ser verificado se o ID da categoria mudou. 
                        // Método para ver se a categoria mudou

                        string cod_oportunidade = Tb.Rows[0]["cod_oportunidade"].ToString();
                        string categoriaExistente = Tb.Rows[0]["id_categoria_ordem_servico"].ToString();
             

                        if (id_Categoria != categoriaExistente)
                        {


                            MetodosGerais.RegistrarLog($"A categoria da ordem de serviço {id_ordemServico} mudou de {categoriaExistente} para {id_Categoria}.");

                            ServicoinTableRelacao.AlterarCategoriaInTableRelacao(Convert.ToInt32(id_ordemServico), Convert.ToInt32(id_Categoria));
                            if (ServicoinTableRelacao.Status)
                            {

                                string codAcao = SelecionarCodAcao(id_Categoria);
                                string textoFolloup = SelecionarMensagemAtualizacao(id_Categoria);

                                AtualizarAcaoRequest AtualizarAcao = new AtualizarAcaoRequest
                                {
                                    codigoOportunidade = cod_oportunidade,
                                    codigoAcao = codAcao,
                                    codigoJornada = codigoJornada,
                                    textoFollowup = textoFolloup
                                };

                                // Atualize a categoria na tabela de relação se necessário

                                EnviarOrdemServiçoForCRM.AtualizarAcao(AtualizarAcao, DadosAPI.Token);

                                // Log para verificação
                                MetodosGerais.RegistrarLog($"Categoria atualizada para {id_Categoria} na tabela de relação para a OS {id_ordemServico}.");
                            }
                        }
                        else
                        {
                            MetodosGerais.RegistrarLog($"Ordem de serviço {id_ordemServico} já existe na tabela com a mesma categoria.");
                        }
                    }

        
                }
                MetodosGerais.RegistrarFinalLog();
            }
            catch (Exception ex)
            {
                MetodosGerais.RegistrarInicioLog();
                MetodosGerais.RegistrarLog($"[ERROR]: {ex.Message}");
                Message = $"[ERROR]: {ex.Message}";
                Status = false;
                MetodosGerais.RegistrarFinalLog();
            }
        }

        private string SelecionarCodAcao(string idCategoria)
        {
            switch (idCategoria)
            {
                case "1":  // AGUARDANDO ANALISE
                    return "610728D401E283FA07FB";

                case "2":   // EM ANALISE
                    return "488A0F6E2D44B5516917";

                case "3":    // COTAÇÃO DE PEÇAS
                    return "99EA5272D06B15DABEC1";

                case "4":   // AGUARDANDO APROVAÇÃO DO CLIENTE
                    return "75B00E055610E6122A8F";

                case "5": // APROVADO PELO CLIENTE
                    return "8C9E3423809C14E52C0B";

                case "6":  //REJEITADO PELO  CLIENTE
                    return "4B9C182C13E7D5FD58FA";

                case "7":  // AGUARDANDO PEÇAS
                    return "FA5DF972949B8A7CFAB3";

                case "8": //AGUARDANDO SUBSTITUIÇÃO DE PEÇA
                    return "DA62CFF0FB010DE83E78";

                case "9": //AGUARDANDO SUBSTITUIÇÃO DE PEÇA
                    return "6133AF81ECA6CBB37D18";

                case "10":  // APROVADO PARA DESCARTE
                    return "88F9B12D58D6950B3C72";

                case "11": // SEM CONSERTO
                    return "75B00E055610E6122A8F";

                case "12":// EM SERVIÇO EXTERNO
                    return "88F9B12D58D6950B3C72";

                case "13": //BUSCAR SERVIÇO EXTERNO
                    return "88F9B12D58D6950B3C72";

                case "14": // ENCERRADA
                    return "88F9B12D58D6950B3C72";

                case "15":  // PRONTA
                    return "B76F652A3AFE943D944E";

                case "16": // DESCARTADA PELO CLIENTE
                    return "88F9B12D58D6950B3C72";

                case "17": // ENVIADO PARA DESCARTE
                    return "88F9B12D58D6950B3C72";

                default:
                    MetodosGerais.RegistrarLog("Número inválido. Por favor, escolha um número de 1 a 5.");
                    return null;
            }
        }

        internal string SelecionarMensagemAtualizacao(string idCategoria)
        {
            switch (idCategoria)
            {
                case "1":  // AGUARDANDO ANALISE
                    return "OS ENTROU NA ETAPA AGUARDANDO ANALISE";

                case "2":   // EM ANALISE
                    return "OS ENTROU NA ETAPA EM ANALISE";

                case "3":    // COTAÇÃO DE PEÇAS
                    return "OS ENTROU NA ETAPA COTAÇÃO DE PEÇAS";

                case "4":   // AGUARDANDO APROVAÇÃO DO CLIENTE
                    return "OS ENTROU NA ETAPA AGUARDANDO APROVAÇÃO DO CLIENTE";

                case "5": // APROVADO PELO CLIENTE
                    return "OS ENTROU NA ETAPA APROVADO PELO CLIENTE";

                case "6":  //REJEITADO PELO  CLIENTE
                    return "OS ENTROU NA ETAPA REJEITADO PELO  CLIENTE";

                case "7":  // AGUARDANDO PEÇAS
                    return "OS ENTROU NA ETAPA AGUARDANDO PEÇAS";

                case "8": //AGUARDANDO SUBSTITUIÇÃO DE PEÇA
                    return "OS ENTROU NA ETAPA AGUARDANDO SUBSTITUIÇÃO DE PEÇA";

                case "10":  // APROVADO PARA DESCARTE
                    return "OS ENTROU NA ETAPA APROVADA PARA DESCARTE";

                case "11": // SEM CONSERTO
                    return "OS ENTROU NA ETAPA SEM CONSERTO";

                case "12":// EM SERVIÇO EXTERNO
                    return "OS ENTROU NA ETAPA EM SERVIÇO EXTERNO";

                case "13": //BUSCAR SERVIÇO EXTERNO
                    return "OS ENTROU NA ETAPA BUSCAR SERVIÇO EXTERNO";

                case "14": // ENCERRADA
                    return "OS ENTROU NA ETAPA ENCERRADA";

                case "15":  // PRONTA
                    return "OS ENTROU NA ETAPA PRONTA";

                case "16": // DESCARTADA PELO CLIENTE
                    return "OS ENTROU NA ETAPA DESCARTADA PELO CLIENTE";

                case "17": // ENVIADO PARA DESCARTE
                    return "OS ENTROU NA ETAPA ENVIADO PARA DESCARTE";

                default:
                    MetodosGerais.RegistrarLog("Número inválido. Por favor, escolha um número de 1 a 5.");
                    return null;
            }
        }
    }
}
