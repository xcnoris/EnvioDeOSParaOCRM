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
                            Console.WriteLine("Resposta OK:");
                            Console.WriteLine(responseBody);
                            return resposta;
                        }
                        else
                        {
                            Console.WriteLine("Erro na resposta: resposta desserializada é nula.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erro na resposta da API:");
                        Console.WriteLine($"Status Code: {response.StatusCode}");
                        Console.WriteLine(responseBody);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exceção durante a chamada da API:");
                    Console.WriteLine(ex.Message);
                }

                return null;
            }
        }
    }
}
