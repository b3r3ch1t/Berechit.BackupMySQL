using Berechit.BackupMySQL.Interfaces;
using System;

namespace Berechit.BackupMySQL.Export
{
    public class ExportEventToFile : IExportEventToFile
    {
        private ExportToFile _exportToFile;
        public void Dispose()
        {
            _exportToFile?.Dispose();
        }

        public void GerarArquivoDumpEvent(string eventName, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

                    var sqlDefiner = FactorySql.SqlGetAllEvents();
                    var definer = queryExpress.ExecuteScalarString(sqlDefiner, "Definer");

                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine(string.Format("DROP EVENT IF EXISTS `{0}`;", eventName));

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições do Event  '{0}'", eventName));
                    _exportToFile.ExportWriteComment("");

                    var sqlCreateProcedure = FactorySql.SqlCreateEvent(eventName: eventName, definer: definer) + ";";


                    _exportToFile.ExportWriteLine(sqlCreateProcedure);

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
