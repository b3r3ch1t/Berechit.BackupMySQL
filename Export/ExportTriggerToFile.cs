using Berechit.BackupMySQL.Interfaces;
using System;

namespace Berechit.BackupMySQL.Export
{
    public class ExportTriggerToFile : IExportTriggerToFile
    {
        private ExportToFile _exportToFile;

        public void Dispose()
        {
            _exportToFile?.Dispose();
        }

        public void GerarArquivoDumpTrigger(string trigger, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

                    var sqlDefiner = FactorySql.SqlGetAllTriggers;

                    var definer = queryExpress.ExecuteScalarString(sqlDefiner, "Definer");

                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine(string.Format("DROP TRIGGER  `{0}`;", trigger));

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições da Trigger  '{0}'", trigger));
                    _exportToFile.ExportWriteComment("");

                    var sqlCreateTrigger = FactorySql.SqlCreateTrigger(trigger: trigger, definer: definer) + ";";
                    _exportToFile.ExportWriteLine(sqlCreateTrigger);

                    _exportToFile.Export_EndInfo();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

    }
}
