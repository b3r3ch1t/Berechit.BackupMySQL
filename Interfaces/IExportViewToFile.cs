using System;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IExportViewToFile : IDisposable
    {
        void GerarArquivoDumpView(string view, string fileName);
    }
}