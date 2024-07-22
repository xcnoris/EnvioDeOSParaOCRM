using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using EnvioDeOSParaOCRM.DataBase;

namespace EnvioDeOSParaOCRM.Modelos
{
    internal class DadosParaAPI
    {
        public string Message;
        public bool Status = false;

        private ConexaoDB _conexaoDBRelOSCRM;
        private ComandosDB _comandosDB;

        public string Token { get; set; }

        public DadosParaAPI()
        {
            _conexaoDBRelOSCRM = new ConexaoDB(2);
            _comandosDB = new ComandosDB(_conexaoDBRelOSCRM);
        }

        public void InserirTokenInTable()
        {
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Tokens WHERE Id = 1";
                string insertQuery = @"
                    INSERT INTO Tokens (Id, Token)
                    VALUES (1, @Token)";
                string updateQuery = @"
                    UPDATE Tokens 
                    SET Token = @Token
                    WHERE Id = 1";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, _conexaoDBRelOSCRM.GetConnection()))
                {
                    _conexaoDBRelOSCRM.OpenConnection();
                    int count = (int)checkCmd.ExecuteScalar();

                    using (SqlCommand cmd = new SqlCommand(count > 0 ? updateQuery : insertQuery, _conexaoDBRelOSCRM.GetConnection()))
                    {
                        cmd.Parameters.AddWithValue("@Token", this.Token);
                        cmd.ExecuteNonQuery();
                    }

                    Message = count > 0 ? "Token atualizado com sucesso!" : "Token inserido com sucesso!";
                    Status = true;
                }
            }
            catch (SqlException ex)
            {
                Message = "Erro ao inserir/atualizar Token no banco de dados: " + ex.Message;
                Status = false;
            }
            catch (Exception ex)
            {
                Message = "Erro ao inserir/atualizar Token no banco de dados: " + ex.Message;
                Status = false;
            }
            finally
            {
                _conexaoDBRelOSCRM.CloseConnection();
            }
        }


        public void BuscarTokenInDB()
        {
            

            try
            {
                string query = @"SELECT Token FROM Tokens WHERE Id = 1";

                using (SqlCommand cmd = new SqlCommand(query, _conexaoDBRelOSCRM.GetConnection()))
                {
                    _conexaoDBRelOSCRM.OpenConnection();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        this.Token = reader["Token"].ToString();
                        this.Message = "Token consultado com sucesso!";
                        this.Status = true;
                    }
                    else
                    {
                        this.Message = "Token não encontrado.";
                        this.Status = false;
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                this.Message = "Erro ao consultar Token no banco de dados: " + ex.Message;
                this.Status = false;
            }
            catch (Exception ex)
            {
                this.Message = "Erro ao consultar Token no banco de dados: " + ex.Message;
                this.Status = false;
            }
            finally
            {
                _conexaoDBRelOSCRM.CloseConnection();
            }
        }


    }
}
