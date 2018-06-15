using Berechit.BackupMySQL.Enums;

namespace Berechit.BackupMySQL.MySQLObjects
{
    public class MySqlConstraint
    {

        public string ConstraintName { get; set; }
        public string TableName { get; set; }

        public ConstraintType ConstraintType { get; set; }


    }
}
