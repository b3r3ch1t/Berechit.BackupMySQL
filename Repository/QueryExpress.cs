using Berechit.BackupMySQL.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Berechit.BackupMySQL.Repository
{
    public class QueryExpress : IQueryExpress
    {
        public string ExecuteScalarString(string sql, int columnIndex)
        {
            var dt = GetTable(sql);

            if (dt.Rows[0][columnIndex] is byte[] bytes)
                return Encoding.UTF8.GetString(bytes);


            return dt.Rows[0][columnIndex] + "";
        }

        public DataTable GetTable(string sql)
        {
            var mySqlConnection = BackupMySqlConnection.CriarConexao();
            var dt = new DataTable();
            try
            {
                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }

                using (var command = new MySqlCommand(sql, mySqlConnection))
                {
                    command.CommandText = sql;
                    using (var da = new MySqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }

                    return dt;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataTable();
            }
            finally
            {
                mySqlConnection.Close();
            }

        }

        public string ExecuteScalarString(string sql)
        {
            var mySqlConnection = BackupMySqlConnection.CriarConexao();
            try
            {

                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }


                using (var command = new MySqlCommand(sql, mySqlConnection))
                {
                    command.CommandText = sql;
                    var ob = command.ExecuteScalar();
                    if (ob is byte[] bytes)
                        return Encoding.UTF8.GetString(bytes);

                    return ob + "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
            finally
            {
                mySqlConnection.Close();
            }

        }

        public IEnumerable<string> ExecuteReader(string sql, int columnIndex)
        {
            var result = new List<string>();
            var mySqlConnection = BackupMySqlConnection.CriarConexao();

            try
            {

                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }


                using (var command = new MySqlCommand(sql, mySqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = reader[columnIndex].ToString();
                            result.Add(r);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
            finally
            {
                mySqlConnection.Close();
            }

            return result;
        }

        public Dictionary<string, string> InformacoesMySqlServer()
        {
            const string sql = FactorySql.SqlShowVariablesServer;

            var tableIndex = GetTable(sql);

            var result = new Dictionary<string, string>();

            for (var i = 0; i < tableIndex.Rows.Count; i++)
            {
                var variable = tableIndex.Rows[i][0].ToString();
                var value = tableIndex.Rows[i][1].ToString();



                result.Add(variable, value);
            }

            return result;

        }

        public string ExecuteScalarString(string sql, string field)
        {
            var dt = GetTable(sql);

            if (dt.Rows[0][field] is byte[] bytes)
                return Encoding.UTF8.GetString(bytes);


            return dt.Rows[0][field] + "";
        }

        public IEnumerable<string> ExecuteReader(string sql, string columnName)
        {
            var result = new List<string>();
            var mySqlConnection = BackupMySqlConnection.CriarConexao();

            try
            {

                if (mySqlConnection.State != ConnectionState.Open)
                {
                    mySqlConnection.Open();
                }


                using (var command = new MySqlCommand(sql, mySqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = reader[columnName].ToString();
                            result.Add(r);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<string>();
            }
            finally
            {
                mySqlConnection.Close();
            }

            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
