using Berechit.BackupMySQL.Enums;

namespace Berechit.BackupMySQL
{
    public class RespostaConexaoString
    {
        public string ConnectionString { get; private set; }

        public StatusConexao StatusConexao { get; private set; }

        public RespostaConexaoString(string connectionString, StatusConexao statusConexao)
        {
            ConnectionString = connectionString;
            StatusConexao = statusConexao;
        }

        public void AlterarStatusConexaoString(StatusConexao statusConexao)
        {
            StatusConexao = statusConexao;
        }
    }
}
