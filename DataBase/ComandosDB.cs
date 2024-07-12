using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace EnvioDeOSParaOCRM.DataBase
{
    internal class ComandosDB
    {
        private ConexaoDB _conexaoDB;
        public string Mensagem;

        public ComandosDB(ConexaoDB conexao)
        {
            _conexaoDB = conexao;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parametros = null)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = _conexaoDB.GetConnection();
                _conexaoDB.OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                _conexaoDB.CloseConnection();
            }
            Mensagem = "Consulta executada com sucesso!";
            return dt;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parametros = null)
        {
            int affectedRows = 0;
            try
            {
                SqlConnection connection = _conexaoDB.GetConnection();
                _conexaoDB.OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                _conexaoDB.CloseConnection();
            }
            return affectedRows;
        }

        public object ExecuteScalar(string query, SqlParameter[] parametros = null)
        {
            object result = null;
            try
            {
                SqlConnection connection = _conexaoDB.GetConnection();
                _conexaoDB.OpenConnection();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }
                result = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Mensagem = "Erro ao executar a consulta: " + ex.Message;
            }
            finally
            {
                _conexaoDB.CloseConnection();
            }
            return result;
        }

        // Metodo para transformar senha em hash
        public static string GetMD5Hasg(string senha)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(senha));
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
