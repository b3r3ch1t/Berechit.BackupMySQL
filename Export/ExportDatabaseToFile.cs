using Berechit.BackupMySQL.MySQLObjects;
using System;

namespace Berechit.BackupMySQL.Export
{
    internal static class ExportDatabaseToFile

    {
        private static ExportToFile _exportToFile;
        internal static bool GerarArquivoCriacaoDatabase(this MySqlDatabase database, string fileName, string sql)
        {
            try
            {

                using (_exportToFile = new ExportToFile(fileName))
                {
                    _exportToFile.ExportHeaderDatabase();
                    _exportToFile.ExportWrite(sql);

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
    }
}
