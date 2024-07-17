using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IO;

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
            // caso o id do banco seja 1, busca no banco Lojamix
            if (dbNumber == 1)
            {
                string conexao = "Server=192.168.0.254;Database=LojamixNovo;User Id=Lojamix;Password=l0j4m1x;";
                _connection = new SqlConnection(conexao);
            }
            // caso o id do banco seja 2, busca no banco RelacaoOSComCRM
            if (dbNumber == 2)
            {
                string conexao = "Server=192.168.0.254;Database=RelacaoOScomCRM;User Id=RelOSComCRM;Password=C@sa2005;";
                _connection = new SqlConnection(conexao);
            }
        }

        public static ConexaoDB DesSerializedClassUnit(string vJson)
        {
            return JsonConvert.DeserializeObject<ConexaoDB>(vJson);
        }
        public static string SerializedClassUnit(ConexaoDB conexao)
        {
            return JsonConvert.SerializeObject(conexao);
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
                throw new FileNotFoundException("O arquivo de configuração não foi encontrado.");
            }

            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<ConexaoDB>(jsonData);
        }


    }
}
