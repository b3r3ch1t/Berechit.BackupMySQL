using Berechit.BackupMySQL.Export;
using Berechit.BackupMySQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Berechit.BackupMySQL.MySQLObjects
{
    public class MySqlDatabase : IDisposable
    {

        private readonly IMySqlDatabaseRepository _mySqlDatabaseRepository;

        public MySqlDatabase(IMySqlDatabaseRepository mySqlDatabaseRepository)
        {
            _mySqlDatabaseRepository = mySqlDatabaseRepository;
            PreencherDadosServidor();
            InicializarRelacaoObjetos();
        }

        #region Propriedades

        public string Server { get; private set; }
        public string DataBase { get; private set; }

        public List<string> Tables { get; private set; }
        public List<string> Views { get; private set; }
        public List<string> Triggers { get; private set; }
        public List<string> Procedures { get; private set; }
        public List<string> Functions { get; private set; }
        public List<string> Events { get; private set; }
        #endregion


        private void PreencherDadosServidor()
        {
            using (var mySqlConnection = BackupMySqlConnection.CriarConexao())
            {
                Server = mySqlConnection.DataSource;
                DataBase = mySqlConnection.Database;
            }

        }

        private void InicializarRelacaoObjetos()
        {
            try
            {
                PreencherTables();
                PreencherViews();
                PreencherTriggers();
                PreencherProcedures();
                PreencherFunctions();
                PreencherEvents();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void PreencherEvents()
        {
            var events = _mySqlDatabaseRepository.RelacaoNomeEvents().ToList();

            Events = events;
        }

        private void PreencherFunctions()
        {
            var functions = _mySqlDatabaseRepository.RelacaoNomeFunctions().ToList();

            Functions = functions;
        }

        private void PreencherProcedures()
        {
            var procedures = _mySqlDatabaseRepository.RelacaoNomeProcedures().ToList();

            Procedures = procedures;
        }

        private void PreencherTriggers()
        {
            var triggers = _mySqlDatabaseRepository.RelacaoNomeTriggers().ToList();

            Triggers = triggers;
        }

        private string GerarSqlCriarDatabase()
        {
            var result = string.Empty;

            //try
            //{
            //    var commandText = SqlCreateDatabase();

            //    result = QueryExpress.ExecuteScalarString(commandText, 1);

            //}
            //catch (Exception)
            //{
            //    result = string.Empty;
            //}


            return result + ";";
        }


        private void PreencherViews()
        {
            var view = _mySqlDatabaseRepository.RelacaoNomeViews().ToList();

            Views = view.OrderBy(x => x).ToList();
        }

        private void PreencherTables()
        {
            var tables = _mySqlDatabaseRepository.RelacaoNomeTabelas().ToList();

            Tables = tables.OrderBy(x => x).ToList();

        }

        internal bool GerarSqlCriarDatabase(string fileName)
        {

            var sql = GerarSqlCriarDatabase();
            var result = this.GerarArquivoCriacaoDatabase(fileName, sql);

            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
