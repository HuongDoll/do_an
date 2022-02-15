using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Infrastructure
{
    public class DapperDatabaseContext : IDapperDatabaseContext, IDisposable
    {
        public IDbConnection _dbConnection { get; }

        public MySqlCommand _sqlCommand;

        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }

        public DapperDatabaseContext()
        {
            _dbConnection = new MySqlConnection(ConnectionString);
            _dbConnection.Open();
            _sqlCommand = (MySqlCommand)_dbConnection.CreateCommand();

        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var reader = _dbConnection.ExecuteReader(commandText, param, transaction, commandTimeout, commandType);
            return reader;
        }

        /// <summary>
        /// ExecuteReaderAsync
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _dbConnection.ExecuteReaderAsync(commandText, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int Execute(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var transac = _dbConnection.BeginTransaction())
            {
                var result = _dbConnection.Execute(commandText, param, transac, commandTimeout, commandType);
                transac.Commit();
                return result;
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var transac = _dbConnection.BeginTransaction())
            {
                var result = await _dbConnection.ExecuteAsync(commandText, param, transac, commandTimeout, commandType);
                transac.Commit();
                return result;
            }
        }

        public object ExecuteScalar(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var transac = _dbConnection.BeginTransaction())
            {
                var result = _dbConnection.ExecuteScalar(commandText, param, transac, commandTimeout, commandType);
                transac.Commit();
                return result;
            }
        }

        public async Task<object> ExecuteScalarAsync(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var transac = _dbConnection.BeginTransaction())
            {
                var result = await _dbConnection.ExecuteScalarAsync(commandText, param, transac, commandTimeout, commandType);
                transac.Commit();
                return result;
            }
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}
