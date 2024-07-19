using EnvioDeOSParaOCRM.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM.Formularios
{
    public partial class Frm_ConexaoDB_UC : UserControl
    {
        public string Servidor
        {
            get { return Txt_Servidor.Text; }
        }
        public string IpHost
        {
            get { return Txt_IpHost.Text; }
        }
        public string DataBase
        {
            get { return Txt_DataBase.Text; }
        }
        public string Usuario
        {
            get { return Txt_Usuario.Text; }
        }
        public string Senha
        {
            get { return Txt_Senha.Text; }
        }
        public Frm_ConexaoDB_UC()
        {
            InitializeComponent();
        }
        private void Frm_ConexaoDB_UC_Load(object sender, EventArgs e)
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(basePath, "conexao.json");

                ConexaoDB conexao = ConexaoDB.LoadConnectionData(filePath);
                if (conexao != null)
                {
                    Txt_Servidor.Text = conexao.Servidor;
                    Txt_IpHost.Text = conexao.IpHost;
                    Txt_DataBase.Text = conexao.DataBase;
                    Txt_Usuario.Text = conexao.Usuario;
                    Txt_Senha.Text = conexao.Senha;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados de conexão: " + ex.Message);
            }
        }

        private void Btn_TestarConexao_Click(object sender, EventArgs e)
        {
            try
            {
                // Cria uma instância de ConexaoDB com os dados fornecidos no formulário
                ConexaoDB conexao = new ConexaoDB
                {
                    Servidor = Txt_Servidor.Text,
                    IpHost = Txt_IpHost.Text,
                    DataBase = Txt_DataBase.Text,
                    Usuario = Txt_Usuario.Text,
                    Senha = Txt_Senha.Text
                };

                // Monta a string de conexão com os dados fornecidos
                string connectionString = $"Server={conexao.IpHost};Database={conexao.DataBase};User Id={conexao.Usuario};Password={conexao.Senha};";

                // Tenta abrir a conexão
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    MessageBox.Show("Conexão bem-sucedida!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao testar conexão: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao testar conexão: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
