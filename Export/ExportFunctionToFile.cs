using Berechit.BackupMySQL.Interfaces;
using System;

namespace Berechit.BackupMySQL.Export
{
    public class ExportFunctionToFile : IExportFunctionToFile
    {
        private ExportToFile _exportToFile;

        public void Dispose()
        {
            _exportToFile?.Dispose();
        }

        public void GerarArquivoDumpFunction(string function, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

                    var sqlDefiner = FactorySql.SqlGetAllFunctions();
                    var definer = queryExpress.ExecuteScalarString(sqlDefiner, "Definer");

                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine(string.Format("DROP FUNCTION IF EXISTS `{0}`;", function));

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições da Function  '{0}'", function));
                    _exportToFile.ExportWriteComment("");

                    var sqlCreateProcedure = FactorySql.SqlCreateFunction(function: function, definer: definer) + ";";


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
