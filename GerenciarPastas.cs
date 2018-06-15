using Berechit.BackupMySQL.Enums;
using System;
using System.IO;

namespace Berechit.BackupMySQL
{
    internal static class GerenciarPastas
    {
        internal static StatusPasta CriarPastaInicial(string targetDirectory)
        {

            try
            {

                if (!targetDirectory.EndsWith(@"\"))
                {
                    targetDirectory += @"\";
                }


                targetDirectory += VariaveisGeral.NomeAssembly + @"\" + VariaveisGeral.Database;

                try
                {
                    ExcluirPastRecursivamente(targetDirectory + @"\" + VariaveisGeral.PastaDdl);
                }
                catch (Exception e)
                {
                    switch (e.HResult)
                    {
                        case -2147024893:
                            break;
                        default:
                            Console.WriteLine(e);
                            return StatusPasta.Desconhecido;
                    }

                }


                try
                {
                    ExcluirPastRecursivamente(targetDirectory + @"\" + VariaveisGeral.PastaDml);
                }
                catch (Exception e)
                {
                    switch (e.HResult)
                    {
                        case -2147024893:
                            break;
                        default:
                            Console.WriteLine(e);
                            return StatusPasta.Desconhecido;
                    }

                }

                try
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusPasta.Desconhecido;
                }

                try
                {
                    Directory.CreateDirectory(targetDirectory + @"\" + VariaveisGeral.PastaDdl);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusPasta.Desconhecido;
                }

                try
                {
                    Directory.CreateDirectory(targetDirectory + @"\" + VariaveisGeral.PastaDml);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return StatusPasta.Desconhecido;
                }



                VariaveisGeral.PastaDestino = targetDirectory;

                if (GerarPastaTables())
                {
                    return StatusPasta.PastaCriada;
                };

                return StatusPasta.Desconhecido;

            }
            catch (Exception e)
            {
                switch (e.HResult)
                {
                    case -2147024893:
                        return StatusPasta.PastaInvalida;


                    default:
                        return StatusPasta.Desconhecido;

                }

            }
        }

        public static void ExcluirPastRecursivamente(string path)
        {
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                File.Delete(file);
            }
            var subdirectoryEntries = Directory.GetDirectories(path);

            foreach (var item in subdirectoryEntries)
            {
                Directory.Delete(item, true);
            }


        }

        internal static bool CriarPasta(string pasta)
        {
            try
            {
                Directory.CreateDirectory(pasta);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal static bool GerarPastaTables()
        {

            VariaveisGeral.PastaDestinoDdlTable = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Tables";
            VariaveisGeral.PastaDestinoDmlTable = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDml + @"\" + "Tables";

            VariaveisGeral.PastaDestinoDdlView = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Views";
            VariaveisGeral.PastaDestinoDdlTrigger = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Triggers";
            VariaveisGeral.PastaDestinoDdlProcedure = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Procedures";
            VariaveisGeral.PastaDestinoDdlFunctions = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Funcoes";
            VariaveisGeral.PastaDestinoDdlEvents = VariaveisGeral.PastaDestino + @"\" + VariaveisGeral.PastaDdl + @"\" + "Eventos";



            var resultDdlTable = CriarPasta(VariaveisGeral.PastaDestinoDdlTable);
            var resultDmlTable = CriarPasta(VariaveisGeral.PastaDestinoDmlTable);

            var resultDdlView = CriarPasta(VariaveisGeral.PastaDestinoDdlView);
            var resultDdlTrigger = CriarPasta(VariaveisGeral.PastaDestinoDdlTrigger);
            var resultDdlProcedure = CriarPasta(VariaveisGeral.PastaDestinoDdlProcedure);
            var resultDdlFuctions = CriarPasta(VariaveisGeral.PastaDestinoDdlFunctions);
            var resultDdlEvents = CriarPasta(VariaveisGeral.PastaDestinoDdlEvents);


            return resultDdlTable && resultDmlTable && resultDdlView && resultDdlTrigger && resultDdlProcedure && resultDdlFuctions && resultDdlEvents;
        }

        public static void ExcluirArquivo(string arquivo)
        {
            try
            {
                File.Delete(arquivo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ExcluirPast(string pastaExcluir)
        {
            try
            {
                Directory.Delete(pastaExcluir, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }



        }
    }
}
