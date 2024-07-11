using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDeOSParaOCRM.DataBase
{
    internal class ComandosDB
    {

        private ConexaoDB conexaoDB;
        public string Mensagem;

        public ComandosDB(ConexaoDB conexao)
        {
            conexaoDB = conexao;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = conexaoDB.GetConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                conexaoDB.CloseConnection(conexaoDB.GetConnection());
            }
            Mensagem = "Consulta executada com sucesso!";
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            int affectedRows = 0;
            try
            {
                SqlConnection connection = conexaoDB.GetConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                conexaoDB.CloseConnection(conexaoDB.GetConnection());
            }
            return affectedRows;
        }

        public object ExecuteScalar(string query)
        {
            object result = null;
            try
            {
                SqlConnection connection = conexaoDB.GetConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                result = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                conexaoDB.CloseConnection(conexaoDB.GetConnection());
            }
            return result;
        }

        // Metodo para transformar senha em hash
        public static string GetMD5Hasg(string senha)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Converte a string da senha para um array de byte e calcula o hash
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(senha));

                // Cria uma nova stringbuilder para coletar os bytes e criar a string
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

    }
}
