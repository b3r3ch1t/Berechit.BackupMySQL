using Berechit.BackupMySQL.Enums;

namespace Berechit.BackupMySQL.MySQLObjects
{
    public class MySqlForeignKey
    {
        #region propriedades
        internal string IndexName { get; set; }
        internal string IndexColummNameLocal { get; set; }

        internal string TableLocalName { get; set; }
        internal string TableReferencedName { get; set; }

        internal string IndexColummReferenced { get; set; }

        internal ReferentialActions OnDelete { get; set; }

        internal ReferentialActions OnUpdate { get; set; }

        #endregion

        public ReferentialActions SelecionarReferentialAction(string valor)
        {
            valor = valor.ToUpper();

            switch (valor)
            {
                case "RESTRICT":
                    return ReferentialActions.Restrict;
                case "CASCADE":
                    return ReferentialActions.Cascade;
                case "SET NULL":
                    return ReferentialActions.SetNull;
                case "NO ACTION":
                    return ReferentialActions.NoAction;
                default:
                    return ReferentialActions.SetDefault;
            }
        }

    }
}
