using Berechit.BackupMySQL.MySQLObjects;
using Berechit.BackupMySQL.Repository;
using System;
using System.IO;
using System.Text;

namespace Berechit.BackupMySQL.Export
{
    internal class ExportToFile : IDisposable
    {
        private readonly TextWriter _textWriter;

        public void Dispose()
        {
            try
            {
                _textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _textWriter?.Dispose();


            GC.SuppressFinalize(this);
        }

        internal ExportToFile(string fileName)
        {
            var utf8WithoutBom = new UTF8Encoding(false);
            _textWriter = new StreamWriter(fileName, false, utf8WithoutBom);

        }
        internal void ExportWriteComment(string text)
        {
            _textWriter.WriteLine("-- {0}", text);
        }
        internal void ExportWriteLine(string text)
        {
            _textWriter.WriteLine(text);
        }
        internal void ExportWriteLine()
        {
            _textWriter.WriteLine();
        }

        internal void ExportWrite(string text)
        {
            _textWriter.Write(text);
        }

        internal void ExportHeaderDatabase()
        {
            var iqueryexpress = new QueryExpress();
            var mySqlServer = new MySqlServer(iqueryexpress);


            ExportWriteComment(string.Format("Dump Begin : {0}", DateTime.Now.ToString(VariaveisGeral.FormatoData)));

            ExportWriteComment("--------------------------------------");

            ExportWriteComment(string.Format(" Backup Gerado por : {0}", VariaveisGeral.NomeAssembly));
            ExportWriteComment(string.Format(" Database:  {0}", VariaveisGeral.Database));


            ExportWriteComment(string.Format(" VersionNumber:  {0}", mySqlServer.VersionNumber));
            ExportWriteComment(string.Format(" Edition:  {0}", mySqlServer.Edition));
            ExportWriteComment(string.Format(" MajorVersionNumber:  {0}", mySqlServer.MajorVersionNumber));
            //  ExportWriteComment(string.Format(" CurrentUser:  {0}", mySqlServer.CurrentUser));


            ExportWriteComment("--------------------------------------");

            ExportWriteLine();
        }

        internal void Export_EndInfo()
        {
            ExportWriteLine();
            ExportWriteLine();

            ExportWriteComment(string.Format("Dump End : {0}", DateTime.Now.ToString(VariaveisGeral.FormatoData)));

            ExportWriteComment("--------------------------------------");

            ExportWriteLine();
        }

        internal void ExportLinhaTracejada()
        {
            ExportWriteLine();
            ExportWriteComment("--------------------------------------");
            ExportWriteLine();
        }

        public void ExportWriteCommit()
        {
            ExportWriteLine();
            ExportWriteLine("COMMIT;");
            ExportWriteLine();
        }
    }
}
