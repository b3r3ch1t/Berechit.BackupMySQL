using System;
using System.Collections.Generic;
using System.Data;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IQueryExpress : IDisposable
    {
        string ExecuteScalarString(string sql, int columnIndex);
        DataTable GetTable(string sql);
        string ExecuteScalarString(string sql);
        IEnumerable<string> ExecuteReader(string sql, int columnIndex);
        Dictionary<string, string> InformacoesMySqlServer();
        string ExecuteScalarString(string sql, string columnName);
        IEnumerable<string> ExecuteReader(string sql, string columnName);
    }
}