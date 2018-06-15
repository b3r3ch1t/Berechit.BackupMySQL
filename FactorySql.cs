using Berechit.BackupMySQL.Interfaces;
using System;
using System.Text;

namespace Berechit.BackupMySQL
{
    public static class FactorySql
    {
        public const string SqlShowVariablesServer = "SHOW variables";
        public const string SqlGetAllTriggers = "SHOW TRIGGERS;";

        public static string SqlGetAllTables()
        {
            var s = new StringBuilder();

            s.Append(" SHOW  FULL TABLES  ");
            s.Append(" WHERE");
            s.Append("  table_type ='BASE TABLE';");

            return s.ToString();
        }
        public static string SqlGetAllViews()
        {
            var s = new StringBuilder();

            s.Append(" SHOW  FULL TABLES  ");
            s.Append(" WHERE");
            s.Append("  table_type ='VIEW';");

            return s.ToString();
        }
        public static string SqlCreateTable(string tableNome)
        {
            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var sql = ShowCreateTable(tableNome);

            try
            {
                var result = queryExpress.ExecuteScalarString(sql, 1) + ";";

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        private static string ShowCreateTable(string tableNome)
        {
            var s = new StringBuilder();

            s.Append(" SHOW CREATE TABLE  ");

            s.Append(tableNome);

            return s.ToString();
        }
        public static string SqlGetAllIndexFromTable(string tableNome)
        {
            var s = new StringBuilder();
            s.Append(" SHOW INDEX FROM ");
            s.Append(tableNome);

            return s.ToString();
        }
        public static string SqlGetConstraints()
        {
            var sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("     constraint_name,");
            sb.AppendLine("     table_name,");
            sb.AppendLine("     constraint_type");
            sb.AppendLine(" FROM ");
            sb.AppendLine("     information_schema.table_constraints");
            sb.AppendLine(" WHERE ");
            sb.AppendLine(string.Format("table_schema = '{0}'; ", VariaveisGeral.Database));

            return sb.ToString();
        }
        public static string SqlGetAllForeignKey()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("     tc.constraint_name,");
            sb.AppendLine("     kcu.column_name,");
            sb.AppendLine("     kcu.referenced_table_name,");
            sb.AppendLine("     kcu.referenced_column_name,");
            sb.AppendLine("     rc.update_rule,");
            sb.AppendLine("     rc.delete_rule,");
            sb.AppendLine("     tc.table_name");
            sb.AppendLine(" FROM ");
            sb.AppendLine("     information_schema.table_constraints tc");
            sb.AppendLine();

            sb.AppendLine("     INNER JOIN information_schema.key_column_usage kcu");
            sb.AppendLine("     ON tc.constraint_catalog = kcu.constraint_catalog");
            sb.AppendLine("     AND tc.constraint_schema = kcu.constraint_schema");
            sb.AppendLine("     AND tc.constraint_name = kcu.constraint_name");
            sb.AppendLine("     AND tc.table_name = kcu.table_name");
            sb.AppendLine();

            sb.AppendLine("     LEFT JOIN information_schema.referential_constraints rc");
            sb.AppendLine("     ON tc.constraint_catalog = rc.constraint_catalog");
            sb.AppendLine("     AND tc.constraint_schema = rc.constraint_schema");
            sb.AppendLine("     AND tc.constraint_name = rc.constraint_name");
            sb.AppendLine("     AND tc.table_name = rc.table_name");
            sb.AppendLine(" WHERE ");
            sb.AppendLine(string.Format("    tc.constraint_schema = '{0}'", VariaveisGeral.Database));
            sb.Append("	        AND kcu.referenced_table_name IS NOT NULL;");

            return sb.ToString();
        }
        public static string SqlCreateDatabase()
        {
            var s = new StringBuilder();
            s.Append("SHOW CREATE DATABASE IF NOT EXISTS ");
            s.Append(VariaveisGeral.Database);

            return s.ToString();

        }

        public static string SqlCreateView(string view)
        {
            var sql = string.Format("SHOW CREATE VIEW `{0}`;", view);

            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var dtView = queryExpress.GetTable(sql);

            var createViewSql = dtView.Rows[0]["Create View"] + ";";

            createViewSql = createViewSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            createViewSql = createViewSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            createViewSql = createViewSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            createViewSql = createViewSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            var result = EraseDefiner(createViewSql);

            return result;
        }
        public static string EraseDefiner(string input)
        {
            var sb = new StringBuilder();
            const string definer = " DEFINER=";
            var dIndex = input.IndexOf(definer, StringComparison.Ordinal);

            sb.AppendFormat(definer);

            var pointAliasReached = false;
            var point3RdQuoteReached = false;

            for (var i = dIndex + definer.Length; i < input.Length; i++)
            {
                if (!pointAliasReached)
                {
                    if (input[i] == '@')
                        pointAliasReached = true;

                    sb.Append(input[i]);
                    continue;
                }

                if (!point3RdQuoteReached)
                {
                    if (input[i] == '`')
                        point3RdQuoteReached = true;

                    sb.Append(input[i]);
                    continue;
                }

                if (input[i] != '`')
                {
                    sb.Append(input[i]);
                }
                else
                {
                    sb.Append(input[i]);
                    break;
                }
            }

            return input.Replace(sb.ToString(), string.Empty);
        }

        public static string SqlCreateTrigger(string trigger, string definer)
        {
            var sql = string.Format("SHOW CREATE TRIGGER `{0}`;", trigger);
            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var createTriggerSql = queryExpress.ExecuteScalarString(sql, 2);

            createTriggerSql = createTriggerSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            createTriggerSql = createTriggerSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            createTriggerSql = createTriggerSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            createTriggerSql = createTriggerSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            var sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            var result = createTriggerSql.Replace(definer, string.Empty);

            return result;
        }

        public static string SqlGetAllProcedures()
        {
            var result = string.Format("SHOW PROCEDURE STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", VariaveisGeral.Database);

            return result;

        }

        public static string SqlCreateProcedure(string procedure, string definer)
        {
            var sql = string.Format("SHOW CREATE PROCEDURE `{0}`;", procedure);
            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var createProcedureSql = queryExpress.ExecuteScalarString(sql, 2);

            createProcedureSql = createProcedureSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            var sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            var result = createProcedureSql.Replace(definer, string.Empty);

            return result;
        }

        public static string SqlGetAllFunctions()
        {
            var result = string.Format("SHOW FUNCTION STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", VariaveisGeral.Database);

            return result;

        }

        public static string SqlCreateFunction(string function, string definer)
        {
            var sql = string.Format("SHOW CREATE FUNCTION `{0}`;", function);
            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var createProcedureSql = queryExpress.ExecuteScalarString(sql, 2);

            createProcedureSql = createProcedureSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            var sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            var result = createProcedureSql.Replace(definer, string.Empty);

            return result;
        }

        public static string SqlGetAllEvents()
        {
            var result = string.Format("SHOW EVENTS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", VariaveisGeral.Database);

            return result;
        }

        public static string SqlCreateEvent(string eventName, string definer)
        {
            var sql = string.Format("SHOW CREATE Event `{0}`;", eventName);
            var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

            var createProcedureSql = queryExpress.ExecuteScalarString(sql, "Create Event");

            createProcedureSql = createProcedureSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            createProcedureSql = createProcedureSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            var sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            var result = createProcedureSql.Replace(definer, string.Empty);

            return result;
        }
    }
}
