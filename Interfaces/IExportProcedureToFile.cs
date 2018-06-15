using System;

namespace Berechit.BackupMySQL.Interfaces
{
    interface IExportProcedureToFile : IDisposable
    {
        void GerarArquivoDumpProcedure(string procedure, string fileName);
    }
}
