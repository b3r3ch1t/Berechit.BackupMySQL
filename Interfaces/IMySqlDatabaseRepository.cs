using Berechit.BackupMySQL.MySQLObjects;
using System;
using System.Collections.Generic;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IMySqlDatabaseRepository : IDisposable
    {
        IEnumerable<string> RelacaoNomeTabelas();
        IEnumerable<string> RelacaoNomeViews();
        IEnumerable<string> RelacaoNomeTriggers();
        IEnumerable<string> RelacaoNomeProcedures();
        IEnumerable<string> RelacaoNomeFunctions();
        IEnumerable<string> RelacaoNomeEvents();
        IEnumerable<MySqlIndex> RelacaoIndexTable(string table);
        IEnumerable<MySqlConstraint> RelacaoConstraintsTable(string table);
        IEnumerable<MySqlForeignKey> RelacaoForeignKey(string table);
    }

}
