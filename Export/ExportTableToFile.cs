using Berechit.BackupMySQL.Interfaces;
using Berechit.BackupMySQL.MySQLObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Berechit.BackupMySQL.Export
{
    internal class ExportTableToFile : IExportTableToFile
    {
        private ExportToFile _exportToFile;

        public bool GerarArquivoCriacaoTable(string table, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine();

                    _exportToFile.ExportLinhaTracejada();

                    _exportToFile.ExportWrite(string.Format("USE {0};", VariaveisGeral.Database));

                    _exportToFile.ExportLinhaTracejada();

                    var headerTable = HeaderTable();

                    _exportToFile.ExportLinhaTracejada();

                    _exportToFile.ExportWrite(headerTable);

                    _exportToFile.ExportLinhaTracejada();

                    var sqlDropIfExiste = SqlDropTableIfExiste(table);

                    _exportToFile.ExportWrite(sqlDropIfExiste);

                    _exportToFile.ExportLinhaTracejada();

                    var sqlCreate = FactorySql.SqlCreateTable(table);

                    _exportToFile.ExportWrite(sqlCreate);

                    _exportToFile.ExportWriteLine();

                    _exportToFile.ExportLinhaTracejada();

                    _exportToFile.Export_EndInfo();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }


            return true;
        }

        public string SqlDropTableIfExiste(string tableName)
        {
            var s = new StringBuilder();

            s.AppendLine(string.Format("DROP TABLE IF EXISTS `{0}`;", tableName));

            s.AppendLine("/*!40101 SET @saved_cs_client     = @@character_set_client */;");

            s.AppendLine("/*!40101 SET character_set_client = utf8 */;");

            s.AppendLine("");

            return s.ToString();
        }

        public string HeaderTable()
        {
            var s = new StringBuilder();



            s.AppendLine("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
            s.AppendLine("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
            s.AppendLine("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
            s.AppendLine("/*!40101 SET NAMES utf8 */;");
            s.AppendLine("/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;");
            s.AppendLine("/*!40103 SET TIME_ZONE=\'+00:00\' */;");
            s.AppendLine("/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;");
            s.AppendLine("/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;");
            s.AppendLine("/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;");
            s.AppendLine("/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;");


            return s.ToString();

        }

        public bool GerarArquivoDumpTable(string table, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições da table  '{0}'", table));
                    _exportToFile.ExportWriteComment("");

                    _exportToFile.ExportWriteLine();

                    _exportToFile.ExportWriteLine();

                    _exportToFile.ExportLinhaTracejada();

                    _exportToFile.ExportWrite(string.Format("USE {0};", VariaveisGeral.Database));

                    _exportToFile.ExportLinhaTracejada();

                    var headerTable = HeaderTable();

                    _exportToFile.ExportLinhaTracejada();

                    _exportToFile.ExportWrite(headerTable);

                    _exportToFile.ExportLinhaTracejada();

                    var sqlDropIfExiste = SqlDropTableIfExiste(table);

                    _exportToFile.ExportWrite(sqlDropIfExiste);

                    _exportToFile.ExportWriteLine();

                    var sqlCreate = FactorySql.SqlCreateTable(table);

                    _exportToFile.ExportWrite(sqlCreate);

                    _exportToFile.ExportWriteLine();



                    _exportToFile.ExportWriteLine("/*!40101 SET character_set_client = @saved_cs_client */;");

                    _exportToFile.ExportWriteLine();

                    _exportToFile.ExportLinhaTracejada();

                    ExportarLinhasTable(_exportToFile, table);

                    ExportEndDump();

                    _exportToFile.Export_EndInfo();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }


            return true;
        }

        public void ExportEndDump()
        {
            var sb = new StringBuilder();

            sb.AppendLine("/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;");
            sb.AppendLine("/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;");
            sb.AppendLine("/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;");
            sb.AppendLine("/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;");
            sb.AppendLine("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
            sb.AppendLine("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
            sb.AppendLine("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
            sb.AppendLine("/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;");

            _exportToFile.ExportWriteLine(sb.ToString());
        }

        public void ExportarLinhasTable(ExportToFile exportToFile, string table)
        {
            try
            {

                exportToFile.ExportWriteComment("");
                exportToFile.ExportWriteComment(string.Format("Dumping data for table {0}", table));
                exportToFile.ExportWriteComment("");
                ExportarLinhasTableDados(exportToFile, table);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ExportarLinhasTableDados(ExportToFile exportToFile, string table)
        {
            var mySqlConnection = BackupMySqlConnection.CriarConexao();
            var sql = string.Format("SELECT * FROM `{0}`;", table);
            try
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }

                using (var command = new MySqlCommand(sql, mySqlConnection))
                {
                    var rdr = command.ExecuteReader();
                    var totalBytes = default(int);

                    if (!rdr.HasRows) return;

                    _exportToFile.ExportWriteLine();
                    _exportToFile.ExportWriteLine(string.Format("LOCK TABLES `{0}` WRITE;", table));
                    _exportToFile.ExportWriteLine();


                    exportToFile.ExportWriteLine();
                    exportToFile.ExportWriteLine(string.Format("/*!40000 ALTER TABLE `{0}` DISABLE KEYS */;", table));
                    exportToFile.ExportWriteLine();


                    exportToFile.ExportWriteLine();
                    ExcluirIndex(table);
                    exportToFile.ExportWriteLine();


                    while (rdr.Read())
                    {
                        try
                        {
                            var sqlDataRow = GetValueString(rdr, table);

                            exportToFile.ExportWriteLine(sqlDataRow);

                            totalBytes += sqlDataRow.Length;

                            if (totalBytes < VariaveisGeral.LimiteCommit) continue;

                            exportToFile.ExportWriteCommit();
                            totalBytes = 0;
                        }
                        catch (Exception e)
                        {
                            Console.Write("tabela ==> " + table);
                            Console.WriteLine(e);
                        }

                    }

                    exportToFile.ExportWriteLine();
                    exportToFile.ExportWriteLine(string.Format("/*!40000 ALTER TABLE `{0}` ENABLE KEYS */;", table));
                    exportToFile.ExportWriteLine();


                    IncluirIndex(table);

                    _exportToFile.ExportWriteLine();
                    _exportToFile.ExportWriteLine("UNLOCK TABLES;");
                    _exportToFile.ExportWriteLine();


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ExcluirIndex(string table)
        {
            var mySqlDatabaseRepository = VariaveisGeral.Container.GetInstance<IMySqlDatabaseRepository>();

            var listIndexGeral = mySqlDatabaseRepository.RelacaoIndexTable(table);

            var listIndex = listIndexGeral.Where(x => !x.PrimaryKey && !x.NonUnique).ToList();

            var listConstraints = mySqlDatabaseRepository.RelacaoConstraintsTable(table);

            var listForeignKeys = mySqlDatabaseRepository.RelacaoForeignKey(table).ToList();

            var listIndexExcluir =
                (from i in listIndex
                 join con in listConstraints on i.KeyName equals con.ConstraintName into All
                 from al in All.DefaultIfEmpty()
                 select new { i.KeyName }).ToList();

            var listIndexDrop = new List<string>();


            foreach (var item in listIndexExcluir)
            {
                if (listForeignKeys.Any(x => x.IndexColummNameLocal.Contains(item.KeyName))) continue;

                var dropIndex = string.Format("DROP INDEX `{0}` ON `{1}` ;", item.KeyName, table);

                if (listIndexDrop.Contains(dropIndex)) continue;

                listIndexDrop.Add(dropIndex);


            }

            foreach (var dropIndex in listIndexDrop)
            {
                _exportToFile.ExportWriteLine(dropIndex);

            }

        }

        public void IncluirIndex(string table)
        {
            var mySqlDatabaseRepository = VariaveisGeral.Container.GetInstance<IMySqlDatabaseRepository>();

            var listIndexGeral = mySqlDatabaseRepository.RelacaoIndexTable(table);

            var listIndex = listIndexGeral.Where(x => !x.PrimaryKey && !x.NonUnique).ToList();

            var listConstraints = mySqlDatabaseRepository.RelacaoConstraintsTable(table);

            var listIndexIncluir =
                (from i in listIndex
                 join con in listConstraints on i.KeyName equals con.ConstraintName into All
                 from al in All.DefaultIfEmpty()
                 select new { i.KeyName }).ToList();

            var listForeignKeys = mySqlDatabaseRepository.RelacaoForeignKey(table).ToList();

            var sqlCreate = FactorySql.SqlCreateTable(table);


            var listIndexDdl = sqlCreate.Split('\n').Where(x => x.Contains("KEY") && !x.Contains("UNIQUE KEY") && !x.Contains("PRIMARY KEY")).ToList();

            var addKey = string.Empty;

            var listAddIndex = new List<string>();


            foreach (var item in listIndexIncluir)
            {
                if (listForeignKeys.Any(x => x.IndexColummNameLocal.Contains(item.KeyName))) continue;


                addKey = listIndexDdl.FirstOrDefault(x => x.Contains(item.KeyName));

                if (addKey != null && addKey.EndsWith("ENGINE"))
                {
                    var addKeyRemove = addKey.Split('\n').Last();
                    addKey = addKey.Replace(addKeyRemove, "");
                }


                if (addKey != null && addKey.EndsWith(","))
                {
                    addKey = addKey.Substring(0, addKey.Length - 1);
                }

                if (addKey == null) continue;

                var stringAddKey = string.Format("ALTER TABLE `{0}` ADD {1} ;", table, addKey);

                if (listAddIndex.Contains(stringAddKey)) continue;

                listAddIndex.Add(stringAddKey);

            }


            foreach (var dropIndex in listAddIndex)
            {
                _exportToFile.ExportWriteLine(dropIndex);
            }


        }

        public string GetValueString(MySqlDataReader rdr, string tableName)
        {
            var sb = new StringBuilder();
            var columnName = String.Empty;
            var valor = new object();
            try
            {
                sb.Append(string.Format("INSERT INTO `{0}` ", tableName));

                for (var i = 0; i < rdr.FieldCount; i++)
                {
                    columnName = rdr.GetName(i);

                    sb.AppendFormat(i == 0 ? "(" : ",");

                    sb.Append(string.Format("`{0}`", columnName));
                }

                sb.Append(") VALUES (");

                for (var i = 0; i < rdr.FieldCount; i++)
                {
                    valor = rdr[i];

                    var valorFormatado = Utils.ConvertToSqlFormat(valor);


                    sb.Append(valorFormatado);
                    sb.Append(i == rdr.FieldCount - 1 ? ");" : ",");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("tabela ==> " + tableName);

                Console.WriteLine("Coluna  ==> " + columnName);
                Console.WriteLine("Valor   ==> " + valor);
                Console.WriteLine(e);
            }


            return sb.ToString();
        }

        public static string SqlSelecionarTodosDadosTabela(MySqlTable table)
        {
            return string.Format("SELECT * FROM `{0}`;", table.Nome);
        }

        public void Dispose()
        {
            _exportToFile?.Dispose();
        }
    }
}
