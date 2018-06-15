using System;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IExportFunctionToFile : IDisposable
    {
        void GerarArquivoDumpFunction(string function, string fileName);
    }
}