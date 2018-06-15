using Berechit.BackupMySQL.Export;
using Berechit.BackupMySQL.Interfaces;
using Berechit.BackupMySQL.Repository;
using SimpleInjector;

namespace Berechit.BackupMySQL
{
    internal static class SimpleInjectorBootStrapper
    {
        public static void InitializeContainer(Container container)
        {
            //container.Register<IUserRepository>(() => new SqlUserRepository("some constr"),
            //    Lifestyle.Singleton);

            container.Register<IQueryExpress, QueryExpress>(Lifestyle.Singleton);

            container.Register<IMySqlDatabaseRepository, MySqlDatabaseRepository>(Lifestyle.Singleton);

            container.Register<IExportTableToFile, ExportTableToFile>(Lifestyle.Singleton);
            container.Register<IExportViewToFile, ExportViewToFile>(Lifestyle.Singleton);
            container.Register<IExportTriggerToFile, ExportTriggerToFile>(Lifestyle.Singleton);
            container.Register<IExportProcedureToFile, ExportProcedureToFile>(Lifestyle.Singleton);
            container.Register<IExportFunctionToFile, ExportFunctionToFile>(Lifestyle.Singleton);
            container.Register<IExportEventToFile, ExportEventToFile>(Lifestyle.Singleton);



            container.Verify();

        }
    }
}