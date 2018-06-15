using Berechit.BackupMySQL.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using static System.UInt32;

namespace Berechit.BackupMySQL
{


    internal class BackupMySqlConnection
    {
        public static RespostaConexaoString StringConexao;

        internal static void ValidarConexao(string connectionString)
        {

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectionString);

                conn.Open();
                StringConexao = new RespostaConexaoString(connectionString: connectionString, statusConexao: StatusConexao.ConexaoValida);
            }
            catch (ArgumentException)
            {
                /*
                Console.WriteLine("Check the Connection String.");
                Console.WriteLine(a_ex.Message);
                Console.WriteLine(a_ex.ToString());
                */
            }
            catch (MySqlException ex)
            {
                /*string sqlErrorMessage = "Message: " + ex.Message + "\n" +
                "Source: " + ex.Source + "\n" +
                "Number: " + ex.Number;
                Console.WriteLine(sqlErrorMessage);
                */
                StringConexao = new RespostaConexaoString(connectionString: string.Empty, statusConexao: StatusConexao.Desconhechido);


                switch (ex.Number)
                {
                    //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                    case 1042: // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
                        StringConexao = new RespostaConexaoString(connectionString: string.Empty, statusConexao: StatusConexao.ImpossivelConectarAoServidor);

                        break;
                    case 0: // Access denied (Check DB name,username,password)
                        StringConexao = new RespostaConexaoString(connectionString: string.Empty, statusConexao: StatusConexao.AcessoNegado);

                        break;
                }
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }


        }

        #region Singleton

        internal static readonly MySqlConnection Instance = null;

        internal static MySqlConnection CriarConexao(string stringConexao)
        {

            ValidarConexao(stringConexao);

            if (StringConexao.StatusConexao != StatusConexao.ConexaoValida)
            {
                return null;
            }

            return Instance ?? new MySqlConnection(stringConexao);
        }

        internal static MySqlConnection CriarConexao()
        {

            if (StringConexao.StatusConexao != StatusConexao.ConexaoValida)
            {
                return null;
            }
            return Instance ?? new MySqlConnection(StringConexao.ConnectionString);
        }

        #endregion Singleton


        public static RespostaConexaoString CriarStringConexao(string server, uint port, string user, string password, string databaseName)
        {
            var stringConexao = new MySqlConnectionStringBuilder
            {
                Database = databaseName,
                Password = password,
                UserID = user,
                Port = port,
                Server = server,
                SslMode = MySqlSslMode.None,
                ConnectionTimeout = MaxValue,
                UseCompression = true
            };

            var result = new RespostaConexaoString(connectionString: stringConexao.ToString(), statusConexao: StatusConexao.Desconhechido);

            StringConexao = result;

            ValidarConexao(result.ConnectionString);

            return StringConexao;
        }
    }

}
