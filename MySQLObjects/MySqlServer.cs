using Berechit.BackupMySQL.Interfaces;
using System;
using System.Collections.Generic;

namespace Berechit.BackupMySQL.MySQLObjects
{
    public class MySqlServer : IDisposable
    {
        internal readonly Dictionary<string, string> Propriedades;

        #region Propriedades

        public string VersionNumber => Propriedades["version"];
        public string Edition => Propriedades["version_comment"];
        public string MajorVersionNumber => GetMajorVersionNumber();
        public string CharacterSetServer => Propriedades["character_set_server"];
        public string CharacterSetSystem => Propriedades["character_set_system"];
        public string CharacterSetConnection => Propriedades["character_set_connection"];
        public string CharacterSetDatabase => Propriedades["character_set_database"];


        #endregion

        public MySqlServer(IQueryExpress queryExpress)
        {
            Propriedades = queryExpress.InformacoesMySqlServer();
        }

        private string GetMajorVersionNumber()
        {
            var versionNumber = Propriedades["version"];
            var vsa = versionNumber.Split('.');
            string v;
            if (vsa.Length > 1)
                v = vsa[0] + "." + vsa[1];
            else
                v = vsa[0];

            return v;
        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}
