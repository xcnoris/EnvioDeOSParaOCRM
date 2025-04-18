﻿using EnvioDeOSParaOCRM.DataBase;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM.Metodos
{
    internal class VerificarOSinTableRelacao
    {
        public string Message;
        public bool Status = false;

        private ConexaoDB _conexaoDBRelOSCRM;
        private ComandosDB _comandosDB;

        public VerificarOSinTableRelacao()
        {
            _conexaoDBRelOSCRM = new ConexaoDB(2);
            _comandosDB = new ComandosDB(_conexaoDBRelOSCRM);
        }

        public DataTable BuscaOSInDBRelacao(string id_ordemservico)
        {
            try
            {

                string query = @"
                SELECT 
                    *
                FROM 
                    Relacao_OrdemServico_CRM
                WHERE 
                    id_ordemservico = @id_ordemservico
               ";

                SqlParameter[] parametros = new SqlParameter[]
                {
                new SqlParameter("@id_ordemservico", id_ordemservico),

                };
                DataTable TB = _comandosDB.ExecuteQuery(query, parametros);

                Message = $"Busca efetuada com sucesso. OS existe na tabela de relação.";
                Status = true;
                return TB;
            }
            catch(SqlException ex) 
            {
                Message = $"[ERROR]: {ex.Message} - {_comandosDB.Mensagem}";
                Status = false;
                throw ex;
            }
            catch (Exception ex)
            {
                Message = $"[ERROR]: {ex.Message} - {_comandosDB.Mensagem}";
                Status = false;
                throw ex;
            }
            finally
            {
                _conexaoDBRelOSCRM.CloseConnection();
            }

        }

        public void InserirRelacaoInTable(int idOrdemServico, string codOportunidade, int IdCategoria)
        {
            try
            {
                string query = @"
                    INSERT INTO Relacao_OrdemServico_CRM (id_ordemservico, cod_oportunidade, id_categoria_ordem_servico, data_criacao)
                    VALUES (@id_ordemservico, @cod_oportunidade, @id_categoria_ordem_servico, @data_criacao)";

                using (SqlCommand cmd = new SqlCommand(query, _conexaoDBRelOSCRM.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@id_ordemservico", idOrdemServico);
                    cmd.Parameters.AddWithValue("@cod_oportunidade", codOportunidade);
                    cmd.Parameters.AddWithValue("@id_categoria_ordem_servico", IdCategoria);
                    cmd.Parameters.AddWithValue("@data_criacao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@data_alteracao", DateTime.Now);

                    _conexaoDBRelOSCRM.OpenConnection();
                    cmd.ExecuteNonQuery();
                

                    Message = "Ordem de serviço inserida com sucesso!";
                    Status = true;
                }
            }
            catch (SqlException ex)
            {
                Message = "Erro ao inserir ordem de serviço no banco de dados: " + ex.Message;
                Status = false;
            }
            catch (Exception ex)
            {
                Message = "Erro ao inserir ordem de serviço no banco de dados: " + ex.Message;
                Status = false;
            }
            finally
            {
                _conexaoDBRelOSCRM.CloseConnection();
            }
        }

        internal void AlterarCategoriaInTableRelacao(int idOrdemServico, int idCategoria)
        {
            try
            {
                string query = @"UPDATE Relacao_OrdemServico_CRM
                                 SET
                                    id_categoria_ordem_servico = @idCategoria
                                 WHERE 
                                    id_ordemservico = @idOrdemServico;
                                    ";


                using (SqlCommand cmd = new SqlCommand(query, _conexaoDBRelOSCRM.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@idCategoria", idCategoria);
                    cmd.Parameters.AddWithValue("@idOrdemServico", idOrdemServico);

                    _conexaoDBRelOSCRM.OpenConnection();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Message = "Categoria da ordem de serviço atualizada com sucesso!";
                        Status = true;
                    }
                    else
                    {
                        Message = "Nenhuma ordem de serviço encontrada com o ID fornecido.";
                        Status = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Message = "Erro ao inserir ordem de serviço no banco de dados: " + ex.Message;
                Status = false;
            }
            catch (Exception ex)
            {
                Message = "Erro ao inserir ordem de serviço no banco de dados: " + ex.Message;
                Status = false;
            }
            finally
            {
                _conexaoDBRelOSCRM.CloseConnection();
            }
        }
    }
}
