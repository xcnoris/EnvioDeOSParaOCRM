using EnvioDeOSParaOCRM.Modelos;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace EnvioDeOSParaOCRM.Metodos
{
    internal class EnviarOrdemServiçoForCRM
    {
        public static async Task<OportunidadeResponse> EnviarOportunidade(OrdemServiçoRequest request, string Token)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.leadfinder.com.br/integracao/v2/inserirOportunidade";
                client.DefaultRequestHeaders.Add("Authorization", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        OportunidadeResponse resposta = JsonConvert.DeserializeObject<OportunidadeResponse>(responseBody);
                        if (resposta != null)
                        {
                            MetodosGerais.RegistrarLog("Resposta OK:");
                            MetodosGerais.RegistrarLog(responseBody);
                            return resposta;
                        }
                        else
                        {
                            MetodosGerais.RegistrarLog("Erro na resposta: resposta desserializada é nula.");
                        }
                    }
                    else
                    {
                        MetodosGerais.RegistrarLog("Erro na resposta da API:");
                        MetodosGerais.RegistrarLog($"Status Code: {response.StatusCode}");
                        MetodosGerais.RegistrarLog(responseBody);
                    }
                }
                catch (Exception ex)
                {
                    MetodosGerais.RegistrarLog("Exceção durante a chamada da API:");
                    MetodosGerais.RegistrarLog(ex.Message);
                }

                return null;
            }
        }

        public static async Task<OportunidadeResponse> AtualizarAcao(AtualizarAcaoRequest request, string Token)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.leadfinder.com.br/integracao/movimentarOportunidade";
                client.DefaultRequestHeaders.Add("Authorization", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string json = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        OportunidadeResponse resposta = JsonConvert.DeserializeObject<OportunidadeResponse>(responseBody);
                        if (resposta != null)
                        {
                            MetodosGerais.RegistrarLog("Resposta OK:");
                            MetodosGerais.RegistrarLog(responseBody);
                            return resposta;
                        }
                        else
                        {
                            MetodosGerais.RegistrarLog("Erro na resposta: resposta desserializada é nula.");
                        }
                    }
                    else
                    {
                        MetodosGerais.RegistrarLog("Erro na resposta da API:");
                        MetodosGerais.RegistrarLog($"Status Code: {response.StatusCode}");
                        MetodosGerais.RegistrarLog(responseBody);
                    }
                }
                catch (Exception ex)
                {
                    MetodosGerais.RegistrarLog("Exceção durante a chamada da API:");
                    MetodosGerais.RegistrarLog(ex.Message);
                }

                return null;
            }
        }
    }
}

