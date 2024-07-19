using EnvioDeOSParaOCRM.Formularios;
using System;
using System.IO;
using System.Windows.Forms;

namespace EnvioDeOSParaOCRM
{
    internal static class MetodosGerais
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        public static void RegistrarLog(string mensagem)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LogFilePath, true))
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}";
                    sw.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções relacionadas ao log
                MessageBox.Show($"Erro ao registrar log: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
