using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Infrastructure
{
    public interface IDapperDatabaseContext
    {
        // <summary>
        /// Lấy toàn bộ tham số đầu vào được khai báo trong store
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        //public MySqlParameterCollection GetParamFromStore(string commandText);

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// ExecuteReaderAsync
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Task<IDataReader> ExecuteReaderAsync(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int Execute(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public Task<int> ExecuteAsync(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        public object ExecuteScalar(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        public Task<object> ExecuteScalarAsync(string commandText, object param = null, int? commandTimeout = null, CommandType? commandType = null);

        public void Dispose();
    }
}
