using Berechit.BackupMySQL.Enums;
using Berechit.BackupMySQL.Interfaces;
using Berechit.BackupMySQL.MySQLObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Berechit.BackupMySQL.Repository
{
    public class MySqlDatabaseRepository : IMySqlDatabaseRepository
    {
        private readonly IQueryExpress _queryExpress;

        public MySqlDatabaseRepository(IQueryExpress queryExpress)
        {
            _queryExpress = queryExpress;
        }

        public void Dispose()
        {
            _queryExpress.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<string> RelacaoNomeTabelas()
        {
            try
            {
                var commandText = FactorySql.SqlGetAllTables();

                try
                {
                    var listTables = _queryExpress.ExecuteReader(commandText, 0).ToList().ToList();

                    return listTables.OrderBy(x => x).ToList();
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<string> RelacaoNomeViews()
        {
            try
            {
                var commandText = FactorySql.SqlGetAllViews();

                try
                {
                    var listQueries = _queryExpress.ExecuteReader(commandText, 0).ToList();

                    return listQueries.OrderBy(x => x).ToList();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<string> RelacaoNomeTriggers()
        {
            try
            {
                var sql = FactorySql.SqlGetAllTriggers;

                try
                {
                    var listTriggers = _queryExpress.ExecuteReader(sql, 0).ToList();

                    return listTriggers.OrderBy(x => x).ToList();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<string> RelacaoNomeProcedures()
        {
            try
            {
                var sql = FactorySql.SqlGetAllProcedures();

                try
                {
                    var listProcedures = _queryExpress.ExecuteReader(sql, "name").ToList();

                    return listProcedures.OrderBy(x => x).ToList();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<string> RelacaoNomeFunctions()
        {
            try
            {
                var sql = FactorySql.SqlGetAllFunctions();

                try
                {
                    var listFunctions = _queryExpress.ExecuteReader(sql, "name").ToList();

                    return listFunctions.OrderBy(x => x).ToList();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<string> RelacaoNomeEvents()
        {
            try
            {
                var sql = FactorySql.SqlGetAllEvents();

                try
                {
                    var listFunctions = _queryExpress.ExecuteReader(sql, "name").ToList();

                    return listFunctions.OrderBy(x => x).ToList();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
        }

        public IEnumerable<MySqlIndex> RelacaoIndexTable(string table)
        {
            var result = new List<MySqlIndex>();

            try
            {
                var commandText = FactorySql.SqlGetAllIndexFromTable(table);

                var tableIndex = _queryExpress.GetTable(commandText);

                for (var i = 0; i < tableIndex.Rows.Count; i++)
                {
                    var keyName = tableIndex.Rows[i][2].ToString();
                    var seqInIndex = Convert.ToInt16(tableIndex.Rows[i][3].ToString());
                    var columnName = tableIndex.Rows[i][4].ToString();
                    var comment = tableIndex.Rows[i][11].ToString();
                    var indexComment = tableIndex.Rows[i][12].ToString();
                    var nonUnique = tableIndex.Rows[i][1].ToString() == "0";
                    var tipoIndexSort = tableIndex.Rows[i][5].ToString();

                    var nullAble = tableIndex.Rows[i][8].ToString() != string.Empty;

                    var indexType = tableIndex.Rows[i][10].ToString();

                    var idx = new MySqlIndex
                    {
                        KeyName = keyName,
                        SeqInIndex = seqInIndex,
                        ColumnName = columnName,
                        Comment = comment,
                        IndexComment = indexComment,
                        NonUnique = nonUnique,
                    };

                    idx.PrimaryKey = string.Equals(idx.KeyName.ToUpper(), "PRIMARY".ToUpper(),
                        StringComparison.OrdinalIgnoreCase);


                    switch (tipoIndexSort)
                    {
                        case "A":
                            idx.Collation = TipoIndexSort.Ascending;
                            break;
                        case "D":
                            idx.Collation = TipoIndexSort.Descending;
                            break;
                        default:
                            idx.Collation = TipoIndexSort.NotSorted;
                            break;
                    }

                    idx.NullAble = nullAble;

                    switch (indexType)
                    {
                        case "BTREE":
                            idx.IndexType = TipoIndex.Btree;
                            break;
                        case "FULLTEXT":
                            idx.IndexType = TipoIndex.Fulltext;
                            break;
                        case "HASH":
                            idx.IndexType = TipoIndex.Hash;
                            break;
                        case "RTREE":
                            idx.IndexType = TipoIndex.Rtree;
                            break;
                        default:
                            idx.IndexType = TipoIndex.Desconhecido;
                            break;
                    }

                    result.Add(idx);
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public IEnumerable<MySqlConstraint> RelacaoConstraintsTable(string table)
        {
            var sql = FactorySql.SqlGetConstraints();

            var result = new List<MySqlConstraint>();

            //var tableIndex = QueryExpress.GetTable(sql);
            var tableIndex = new DataTable();

            for (var i = 0; i < tableIndex.Rows.Count; i++)
            {
                var constraintName = tableIndex.Rows[i][0].ToString();
                var tableName = tableIndex.Rows[i][1].ToString();
                var constraintType = tableIndex.Rows[i][2].ToString().ToUpper();


                var constraint =
                    new MySqlConstraint() { ConstraintName = constraintName, TableName = tableName };
                switch (constraintType)
                {
                    case "PRIMARY KEY":
                        constraint.ConstraintType = ConstraintType.PrimaryKey;
                        break;
                    case "UNIQUE":
                        constraint.ConstraintType = ConstraintType.Unique;
                        break;
                    case "FOREIGN KEY":
                        constraint.ConstraintType = ConstraintType.ForeignKey;
                        break;
                    default:
                        constraint.ConstraintType = ConstraintType.Desconhecido;
                        break;
                }


                result.Add(constraint);

            }


            return result;
        }

        public IEnumerable<MySqlForeignKey> RelacaoForeignKey(string table)
        {
            var result = new List<MySqlForeignKey>();


            try
            {
                var commandText = FactorySql.SqlGetAllForeignKey();

                var tableIndex = _queryExpress.GetTable(commandText);



                for (var i = 0; i < tableIndex.Rows.Count; i++)
                {
                    var indexName = tableIndex.Rows[i][0].ToString();
                    var indexColummNameLocal = tableIndex.Rows[i][1].ToString();
                    var tableReferencedName = tableIndex.Rows[i][2].ToString();
                    var indexColummReferenced = tableIndex.Rows[i][3].ToString();


                    var tableLocalName = tableIndex.Rows[i][6].ToString();


                    var idx = new MySqlForeignKey
                    {
                        IndexName = indexName,
                        IndexColummNameLocal = indexColummNameLocal,
                        TableReferencedName = tableReferencedName,
                        IndexColummReferenced = indexColummReferenced,
                        TableLocalName = tableLocalName
                    };


                    idx.OnDelete = idx.SelecionarReferentialAction(tableIndex.Rows[i][4].ToString());
                    idx.OnUpdate = idx.SelecionarReferentialAction(tableIndex.Rows[i][5].ToString());
                    result.Add(idx);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }
    }
}