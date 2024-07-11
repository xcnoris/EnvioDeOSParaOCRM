using System;
using System.Data.SqlClient;

namespace EnvioDeOSParaOCRM.DataBase
{
    internal class ConexaoDB
    {
        private readonly string _connectionString;

        public ConexaoDB()
        {
            _connectionString = "Server=192.168.0.254;Database=LojamixNovo;User Id=Lojamix;Password=l0j4m1x;";
        }

        public SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("Connection opened successfully.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening connection: {ex.Message}");
                throw;
            }
        }

        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Connection closed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing connection: {ex.Message}");
                throw;
            }
        }
    }
}
