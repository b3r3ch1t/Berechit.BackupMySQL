using Berechit.BackupMySQL.Interfaces;
using System;

namespace Berechit.BackupMySQL.Export
{
    public class ExportViewToFile : IExportViewToFile
    {
        private ExportToFile _exportToFile;

        public void Dispose()
        {
            _exportToFile?.Dispose();
        }

        public void GerarArquivoDumpView(string view, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine(string.Format("DROP TABLE IF EXISTS `{0}`;", view));
                    _exportToFile.ExportWriteLine(string.Format("DROP VIEW IF EXISTS `{0}`;", view));

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições da View  '{0}'", view));
                    _exportToFile.ExportWriteComment("");


                    var sqlCreateView = FactorySql.SqlCreateView(view);
                    _exportToFile.ExportWriteLine(sqlCreateView);

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
