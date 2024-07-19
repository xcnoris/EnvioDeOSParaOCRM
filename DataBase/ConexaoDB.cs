using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM.DataBase
{
    public class ConexaoDB
    {
        private SqlConnection _connection;

        public string Servidor { get; set; }
        public string IpHost { get; set; }
        public string DataBase { get; set; }
        public string Usuario { get; set; }
        public string  Senha { get; set; }


        public ConexaoDB()
        {

        }

        public ConexaoDB(int dbNumber)
        {
            CarregarbancoLojamix();
            // caso o id do banco seja 1, busca no banco Lojamix
            if (dbNumber == 1)
            {
                string conexao = $"Server={IpHost};Database={DataBase};User Id={Usuario};Password={Senha};";
                _connection = new SqlConnection(conexao);
            }
            // caso o id do banco seja 2, busca no banco RelacaoOSComCRM
            if (dbNumber == 2)
            {
                string conexao = "Server=localhost;Database=RelacaoOScomCRM;User Id=RelOSComCRM;Password=C@sa2005;";
                _connection = new SqlConnection(conexao);
            }
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }


        public void SaveConnectionData(string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, jsonData);
        }
        public static ConexaoDB LoadConnectionData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<ConexaoDB>(jsonData);
        }

        private void CarregarbancoLojamix()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(basePath, "conexao.json");

                ConexaoDB conexao = ConexaoDB.LoadConnectionData(filePath);
                if (conexao != null)
                {
                    Servidor = conexao.Servidor;
                    IpHost = conexao.IpHost;
                    DataBase = conexao.DataBase;
                    Usuario = conexao.Usuario;
                    Senha = conexao.Senha;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados de conexão: " + ex.Message);
            }
        }
    }
}
