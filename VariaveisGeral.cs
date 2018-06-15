using SimpleInjector;

namespace Berechit.BackupMySQL
{
    internal static class VariaveisGeral
    {
        internal const string FormatoData = "dd-MM-yyyy HH:mm:ss";
        internal static string NomeAssembly = string.Empty;
        internal const string PastaDdl = "DDL";
        internal const string PastaDml = "DML";
        internal const int LimiteCommit = 5242880;

        internal static string PastaDestino { get; set; }
        internal static string PastaDestinoDdlTable { get; set; }
        internal static string PastaDestinoDmlTable { get; set; }

        internal static string PastaDestinoDdlView { get; set; }


        internal static string Database { get; set; }

        public static Container Container { get; set; }
        public static string PastaDestinoDdlTrigger { get; set; }
        public static string PastaDestinoDdlProcedure { get; set; }
        public static string PastaDestinoDdlFunctions { get; set; }
        public static string PastaDestinoDdlEvents { get; set; }
    }
}
