using System;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IExportEventToFile : IDisposable
    {
        void GerarArquivoDumpEvent(string eventName, string fileName);
    }
}