using System;

namespace Berechit.BackupMySQL.Interfaces
{
    public interface IExportTriggerToFile : IDisposable
    {
        void GerarArquivoDumpTrigger(string trigger, string fileName);
    }
}