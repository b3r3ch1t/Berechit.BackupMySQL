using MySql.Data.MySqlClient;
using System;

namespace Berechit.BackupMySQL.Export
{
    internal interface IExportTableToFile : IDisposable
    {
        bool GerarArquivoCriacaoTable(string table, string fileName);
        string SqlDropTableIfExiste(string tableName);
        string HeaderTable();
        bool GerarArquivoDumpTable(string table, string fileName);
        void ExportarLinhasTable(ExportToFile exportToFile, string table);
        void ExportarLinhasTableDados(ExportToFile exportToFile, string table);
        void ExcluirIndex(string table);
        void IncluirIndex(string table);
        string GetValueString(MySqlDataReader rdr, string tableName);
    }
}