using Berechit.BackupMySQL.Interfaces;
using System;

namespace Berechit.BackupMySQL.Export
{
    public class ExportProcedureToFile : IExportProcedureToFile
    {
        private ExportToFile _exportToFile;

        public void Dispose()
        {
            _exportToFile?.Dispose();
        }


        public void GerarArquivoDumpProcedure(string procedure, string fileName)
        {
            try
            {
                using (_exportToFile = new ExportToFile(fileName))
                {
                    var queryExpress = VariaveisGeral.Container.GetInstance<IQueryExpress>();

                    var sqlDefiner = FactorySql.SqlGetAllProcedures();

                    var definer = queryExpress.ExecuteScalarString(sqlDefiner, "Definer");

                    _exportToFile.ExportHeaderDatabase();

                    _exportToFile.ExportWriteLine(string.Format("DROP PROCEDURE IF EXISTS `{0}`;", procedure));

                    _exportToFile.ExportWriteComment("");
                    _exportToFile.ExportWriteComment(string.Format("Definições da Procedure  '{0}'", procedure));
                    _exportToFile.ExportWriteComment("");

                    var sqlCreateProcedure = FactorySql.SqlCreateProcedure(procedure: procedure, definer: definer) + ";";
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
