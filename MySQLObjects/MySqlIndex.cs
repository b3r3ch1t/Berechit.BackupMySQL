using Berechit.BackupMySQL.Enums;

namespace Berechit.BackupMySQL.MySQLObjects
{
    public class MySqlIndex
    {

        internal bool PrimaryKey { get; set; }
        internal bool NonUnique { get; set; }
        internal string KeyName { get; set; }
        internal int SeqInIndex { get; set; }
        internal string ColumnName { get; set; }
        internal TipoIndexSort Collation { get; set; }
        internal bool NullAble { get; set; }
        internal TipoIndex IndexType { get; set; }
        internal string Comment { get; set; }
        internal string IndexComment { get; set; }

    }
}

