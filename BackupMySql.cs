using Berechit.BackupMySQL.Enums;
using Berechit.BackupMySQL.Export;
using Berechit.BackupMySQL.Interfaces;
using Berechit.BackupMySQL.MySQLObjects;
using SimpleInjector;
using System;
using System.IO.Compression;

namespace Berechit.BackupMySQL
{
    public class BackupMySql : IDisposable
    {
        public MySqlDatabase MySqlDatabase;
        public MySqlServer MySqlServer;

        internal string ConexaoString;
        internal StatusConexao StatusConexao;
        internal StatusPasta StatusPasta;

        public BackupMySql(string server, int port, string user, string password, string database, string pasta)
        {
            VariaveisGeral.NomeAssembly = GetType().Name;

            //1) Primeiro a ser inicializado devido do IoC
            InicializarBootstrapper();

            //2) Segundo a ser inicializado devido aos dados do Banco de dados 
            InicializarConnectionString(server, port, user, password, database);

            InicializarPastas(pasta);

            MySqlDatabase = VariaveisGeral.Container.GetInstance<MySqlDatabase>();

            MySqlServer = VariaveisGeral.Container.GetInstance<MySqlServer>();

        }

        private void InicializarPastas(string pasta)
        {
            if (string.IsNullOrWhiteSpace(pasta))
            {
                StatusPasta = StatusPasta.PastaInvalida;
            }

            StatusPasta = GerenciarPastas.CriarPastaInicial(pasta);

            if (StatusPasta != StatusPasta.PastaCriada)
            {
                throw new Exception(string.Format("Erro ao criar a pasta de saída --> {0}", StatusPasta.ToString()));

            }
        }

        private void InicializarConnectionString(string server, int port, string user, string password, string database)
        {
            var stringConexao = BackupMySqlConnection.CriarStringConexao(server, Convert.ToUInt32(port), user, password, database);

            if (stringConexao.StatusConexao != StatusConexao.ConexaoValida ||
                stringConexao.ConnectionString == string.Empty)
                throw new Exception(string.Format("Conexão Inválida --> {0}", stringConexao.StatusConexao.ToString()));


            StatusConexao = stringConexao.StatusConexao;
            ConexaoString = stringConexao.ConnectionString;

            VariaveisGeral.Database = database;
        }

        private void InicializarBootstrapper()
        {
            VariaveisGeral.Container = new Container();
            SimpleInjectorBootStrapper.InitializeContainer(VariaveisGeral.Container);
        }

        internal void GerarSqlTables()
        {
            try
            {
                var listTables = MySqlDatabase.Tables;

                var exportTableToFile = VariaveisGeral.Container.GetInstance<IExportTableToFile>();

                foreach (var function in listTables)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlTable + @"\table_" + function + ".sql";
                    exportTableToFile.GerarArquivoCriacaoTable(table: function, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Tables " + e.Message);
            }
        }

        public string GerarBackupDdl()
        {

            GerarSqlViews();
            GerarSqlTriggers();
            GerarSqlProcedures();
            GerarSqlFunctions();
            GerarSqlTables();
            GerarSqlEvents();

            return "Backup gerado com sucesso !!!!!";
        }

        private void GerarSqlEvents()
        {
            try
            {
                var listEvents = MySqlDatabase.Events;

                var exportEventsToFile = VariaveisGeral.Container.GetInstance<IExportEventToFile>();

                foreach (var itemEvent in listEvents)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlEvents + @"\Events_" + itemEvent + ".sql";
                    exportEventsToFile.GerarArquivoDumpEvent(eventName: itemEvent, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Functions " + e.Message);
            }
        }

        private void GerarSqlFunctions()
        {
            try
            {
                var listFunctions = MySqlDatabase.Functions;

                var exportFunctionToFile = VariaveisGeral.Container.GetInstance<IExportFunctionToFile>();

                foreach (var function in listFunctions)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlFunctions + @"\function_" + function + ".sql";
                    exportFunctionToFile.GerarArquivoDumpFunction(function: function, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Functions " + e.Message);
            }
        }

        private void GerarSqlProcedures()
        {
            try
            {
                var listProcedures = MySqlDatabase.Procedures;

                var exportProcedureToFile = VariaveisGeral.Container.GetInstance<IExportProcedureToFile>();

                foreach (var procedure in listProcedures)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlProcedure + @"\procedure_" + procedure + ".sql";
                    exportProcedureToFile.GerarArquivoDumpProcedure(procedure: procedure, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Procedures " + e.Message);
            }
        }

        private void GerarSqlTriggers()
        {
            try
            {
                var listTriggers = MySqlDatabase.Triggers;

                var exportTriggerToFile = VariaveisGeral.Container.GetInstance<IExportTriggerToFile>();

                foreach (var trigger in listTriggers)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlTrigger + @"\trigger_" + trigger + ".sql";
                    exportTriggerToFile.GerarArquivoDumpTrigger(trigger: trigger, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Triggers " + e.Message);
            }
        }

        public string GerarBackupDml()
        {
            GerarBackupDdl();

            GerarSqlDumpTables();


            return "Backup Finalizado com sucesso !!!!";
        }

        private void GerarSqlViews()
        {
            try
            {
                var listViews = MySqlDatabase.Views;

                var exportViewToFile = VariaveisGeral.Container.GetInstance<IExportViewToFile>();

                foreach (var view in listViews)
                {
                    var fileName = VariaveisGeral.PastaDestinoDdlView + @"\view_" + view + ".sql";
                    exportViewToFile.GerarArquivoDumpView(view: view, fileName: fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Erro ao gerar o backup das Views " + e.Message);
            }
        }

        private void GerarSqlDumpTables()
        {
            try
            {
                var listTables = MySqlDatabase.Tables;

                var exportTableToFile = VariaveisGeral.Container.GetInstance<IExportTableToFile>();

                foreach (var table in listTables)
                {
                    var fileName = VariaveisGeral.PastaDestinoDmlTable + @"\table_" + table + ".sql";
                    exportTableToFile.GerarArquivoDumpTable(table: table, fileName: fileName);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(string.Format("Erro ao exportar as tabelas.{0}", e.Message));
            }



        }

        public void Dispose()
        {
            MySqlDatabase?.Dispose();
            MySqlServer?.Dispose();
            ConexaoString = string.Empty;
        }

        public string GerarBackupDmlArquivoCompactado()
        {
            try
            {
                GerarBackupDml();

                var diretorio = VariaveisGeral.PastaDestino;//Caminho do diretório
                var arquivo = diretorio + DateTime.Now.ToString("_ddMMyyyyHHmmss") + ".zip";//Caminho do arquivo zip a ser criado

                GerenciarPastas.ExcluirArquivo(arquivo);


                ZipFile.CreateFromDirectory(diretorio, arquivo);

                GerenciarPastas.ExcluirPast(VariaveisGeral.PastaDestino);

                return "Backup gerado com sucesso !!!!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Erro ao criar o Backup " + e.Message;

            }


        }
    }
}
