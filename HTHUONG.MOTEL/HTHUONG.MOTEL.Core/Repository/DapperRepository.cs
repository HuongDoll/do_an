using Dapper;
using Dapper.Contrib.Extensions;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Utils;
using HTHUONG.MOTEL.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTHUONG.MOTEL.Core.Constants.Enumeration;

namespace HTHUONG.MOTEL.Core.Repository
{
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        protected IDatabaseContextFactory _factory;

        protected string _tableName;


        protected string ConnectionString { get; set; }
        public string TableName { get => _tableName; }

        public DapperRepository(IDatabaseContextFactory factory)
        {
            _factory = factory;
            _tableName = Utility.GetEntityName<T>();
        }
        #region Method

        /// <summary>
        /// Hàm để init DbContext động theo connectionString. 
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public void InitializeDatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Hàm đọc data từ lấy ra được từ database. 
        /// </summary>
        /// <param name="sqlDataReader">IDataReader</param>
        /// <returns>Danh dách đối tượng tương ứng với lớp thực thể T</returns>
        protected IEnumerable<T> ReadData(IDataReader sqlDataReader)
        {

            var res = new List<T>();
            while (sqlDataReader.Read())
            {
                var entity = Activator.CreateInstance<T>();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    var fieldType = sqlDataReader.GetDataTypeName(i);
                    var fieldName = sqlDataReader.GetName(i);
                    var fieldValue = sqlDataReader.GetValue(i);
                    var property = entity.GetType().GetProperty(fieldName);
                    if (fieldValue != System.DBNull.Value && property != null)
                    {
                        //Nếu là bit thì chuyển thành bool
                        if (fieldType == "BIT")
                        {
                            if ((UInt64)fieldValue == 0) property.SetValue(entity, false);
                            else if ((UInt64)fieldValue == 1) property.SetValue(entity, true);
                            continue;
                        }
                        //Cast từ string sang Guid
                        if ((property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?)) && fieldValue.GetType() == typeof(String))
                        {
                            property.SetValue(entity, Guid.Parse(fieldValue.ToString()));
                        }
                        else
                        {
                            property.SetValue(entity, fieldValue);
                        }
                    }
                }
                res.Add(entity);
            }
            return res;
        }

        ///// <summary>
        ///// Lấy dữ liệu theo tên store truyền vào
        ///// </summary>
        ///// <param name="storeName">Tên store</param>
        ///// <returns>Danh dách đối tượng tương ứng với lớp thực thể T</returns>
        //public IEnumerable<T> GetEntities(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        //{
        //    IDataReader sqlDataReader = _dapperDatabaseContext.ExecuteReader(commandText, param, transaction, commandTimeout, commandType);
        //    return ReadData(sqlDataReader);
        //}

        /// <summary>
        /// Lấy dữ liệu theo tên store truyền vào (bất đồng bộ)
        /// </summary>
        /// <returns>Danh dách đối tượng tương ứng với lớp thực thể T</returns>
        public virtual async Task<IEnumerable<T>> GetEntitiesAsync(string commandText, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                IDataReader sqlDataReader = await _dapperDatabaseContext.ExecuteReaderAsync(commandText, param, transaction, commandTimeout, commandType);
                var a = sqlDataReader;
                return ReadData(a);
            }
        }

        /// <summary>
        /// Lấy bản ghi theo id.
        /// </summary>
        /// <param name="id">ID của bản ghi</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <returns>Đối tượng tương ứng với lớp thực thể T</returns>
        public async Task<T> GetByIdAsync(object id, bool? isDeleted = null)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var res = new List<T>();
                var table = _tableName;
                if (_tableName.ToLower() == "userapp")
                {
                    table = "User";
                }
                if (isDeleted != null)
                {
                    res = (await GetEntitiesAsync($"SELECT * FROM {_tableName} WHERE IsDeleted={isDeleted} AND {table}ID = '{id.ToString()}';")).AsList();
                }
                else
                {
                    res = (await GetEntitiesAsync($"SELECT * FROM {_tableName} WHERE {table}ID ='{id.ToString()}';")).AsList();
                }
                if (res.Count > 0)
                {
                    return res[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Lấy tất cả bản ghi theo công ty. 
        /// </summary>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <returns>Danh dách đối tượng tương ứng với lớp thực thể T</returns>
        public async Task<IEnumerable<T>> ListAllAsync(bool? isDeleted = null)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var sql = "";
                if (isDeleted == null)
                {
                    sql = $"SELECT * FROM {_tableName};";
                }
                else
                {
                    sql = $"SELECT * FROM {_tableName} WHERE  IsDeleted={isDeleted};";
                }
                return await GetEntitiesAsync(sql);
            }
        }

        /// <summary>
        /// Lọc các bản ghi. 
        /// </summary>
        /// <param name="getListRequest">Object chứa dữ liệu để lọc bản ghi</param>
        /// <param name="limit">Lấy bao nhiêu bản ghi</param>
        /// <param name="offset">Bỏ qua bao nhiêu bản ghi</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <param name="isDefault">Có lấy các trường mặc định hay không</param>
        /// <returns>Danh dách đối tượng tương ứng với lớp thực thể T</returns>
        public async Task<IEnumerable<T>> ListAsync(GetListRequest getListRequest, long limit, long offset, bool? isDeleted = null, bool? isDefault = false)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var instance = Activator.CreateInstance<T>();
                var props = instance.GetType().GetProperties();
                var selectedFields = new List<string>();

                var filters = new List<List<FilterRequest>>();
                var orderClause = new StringBuilder(" ORDER BY ");

                #region Build câu lệnh Select và Order By
                //Kiểm tra xem trường cần lấy có thật hay không + chống sql injection
                foreach (var prop in props)
                {
                    if (getListRequest.SelectedFields == null) return new List<T>();
                    if (getListRequest.SelectedFields.Contains(prop.Name))
                    {
                        selectedFields.Add(prop.Name);
                    }
                    //Build luôn câu order by
                    if (getListRequest.Orders != null)
                    {
                        foreach (var order in getListRequest.Orders)
                        {
                            var direction = order.StartsWith("-") ? " DESC " : " ASC ";
                            if (order.Substring(1).Equals(prop.Name))
                            {
                                orderClause.Append(prop.Name).Append(direction).Append(",");
                            }
                        }
                    }
                }
                if (getListRequest.Filters != null)
                {
                    foreach (var item in getListRequest.Filters)
                    {
                        var filterAnds = new List<FilterRequest>();
                        foreach (var filterItem in item)
                        {
                            if (CommonFunction.HasProperty(instance, filterItem.Key)) filterAnds.Add(filterItem);
                        }
                        if (filterAnds.Count > 0) filters.Add(filterAnds);
                    }
                }

                #endregion

                #region Build câu lệnh truy vấn WHERE
                var table = _tableName;
                if(table.ToLower() == "file")
                {
                    table = "files";
                }
                if (selectedFields.Count > 0)
                {
                    //Build câu truy vấn
                    var sql = new StringBuilder("SELECT ");
                    sql.Append(string.Join(',', selectedFields)).Append($" FROM {table} WHERE  ");
                    if (isDefault == true)
                    {
                        sql.Append(" (IsDefault=TRUE) ");
                    }
                    
                    //Nếu có IsDeleted thì truyền thêm IsDeleted
                    if (isDeleted != null)
                    {
                        sql.Append($" (IsDeleted={isDeleted}) ");
                    }
                    if (filters.Count > 0)
                    {
                        sql.Append("AND ");
                        for (int i = 0; i < filters.Count; i++)
                        {
                            var andClauses = filters[i];
                            sql.Append(" ( ");
                            for (int j = 0; j < andClauses.Count; j++)
                            {
                                var andClause = andClauses[j];
                                var values = andClause.ValueArray;
                                var prefix = "";
                                switch (andClause.Operator)
                                {
                                    case FilterType.Equal: //0
                                        sql.Append($"{andClause.Key} = @param{i}{j}");
                                        //sql.Append()
                                        break;
                                    case FilterType.Contain:    //1
                                        sql.Append($"{andClause.Key} LIKE CONCAT('%',@param{i}{j},'%')");
                                        break;
                                    case FilterType.GreaterThan:    //3
                                        sql.Append($"{andClause.Key} > @param{i}{j}");

                                        break;
                                    case FilterType.GreaterThanEqual:   //5
                                        sql.Append($"{andClause.Key} >= @param{i}{j}");
                                        break;
                                    case FilterType.LessThan:   //4
                                        sql.Append($"{andClause.Key} < @param{i}{j}");
                                        break;
                                    case FilterType.LessThanEqual:  //6
                                        sql.Append($"{andClause.Key} <= @param{i}{j}");
                                        break;
                                    case FilterType.NotEqual:   //2
                                        sql.Append($"{andClause.Key} != @param{i}{j}");
                                        break;
                                    case FilterType.IsEmpty:    //14
                                        sql.Append($"{andClause.Key} IS NULL");
                                        break;
                                    case FilterType.IsNotEmpty: //7
                                        sql.Append($"{andClause.Key} IS NOT NULL");
                                        break;
                                    case FilterType.ContainAnyOf: //17 - Chứa một trong những giá trị
                                        sql.Append(" ( ");
                                        for (int k = 0; k < values.Count; k++)
                                        {
                                            sql.Append($"{andClause.Key} LIKE CONCAT('%',@param{i}{j}{k}, '%') OR ");
                                            _dapperDatabaseContext._sqlCommand.Parameters.Add(new MySqlParameter($"param{i}{j}{k}", values[k]));
                                        }
                                        sql.Remove(sql.ToString().LastIndexOf(" OR "), 4).Append(")");
                                        break;
                                    case FilterType.EqualToAnyOf:  //15 - Bằng một trong những giá trị
                                        sql.Append($"{andClause.Key} IN (");
                                        prefix = "";
                                        for (int k = 0; k < values.Count; k++)
                                        {
                                            sql.Append(prefix).Append($"@param{i}{j}{k}");
                                            _dapperDatabaseContext._sqlCommand.Parameters.Add(new MySqlParameter($"param{i}{j}{k}", values[k]));
                                            prefix = ",";
                                        }
                                        sql.Append(")");
                                        break;
                                }
                                sql.Append(" AND ");
                            }
                            sql.Remove(sql.ToString().LastIndexOf(" AND "), 5).Append(" ) OR ");
                        }
                        // Nếu có Order By
                        if (!orderClause.Equals(" ORDER BY "))
                        {
                            sql.Remove(sql.ToString().LastIndexOf(" OR "), 4).Append(orderClause.Remove(orderClause.ToString().LastIndexOf(","), 1)).Append($" LIMIT {offset},{limit};");
                        }
                        else
                        {
                            sql.Remove(sql.ToString().LastIndexOf(" OR "), 4).Append($" LIMIT {offset},{limit};");
                        }
                    }
                    else
                    {
                        if (!orderClause.Equals(" ORDER BY "))
                        {
                            sql.Append(orderClause.Remove(orderClause.ToString().LastIndexOf(","), 1)).Append($" LIMIT {offset},{limit};");
                        }
                        else
                        {
                            sql.Append($" LIMIT {offset},{limit};");
                        }
                    }
                    #region Thực hiện câu lệnh truy vấn
                    _dapperDatabaseContext._sqlCommand.CommandType = CommandType.Text;
                    _dapperDatabaseContext._sqlCommand.CommandText = sql.ToString();
                    for (int i = 0; i < filters.Count; i++)
                    {
                        for (int j = 0; j < filters[i].Count; j++)
                        {
                            var param = new MySqlParameter($"param{i}{j}", filters[i][j].Value);
                            _dapperDatabaseContext._sqlCommand.Parameters.Add(param);
                        }
                    }
                    await _dapperDatabaseContext._sqlCommand.PrepareAsync();
                    var reader = await _dapperDatabaseContext._sqlCommand.ExecuteReaderAsync();
                    #endregion
                    //Đọc data từ reader
                    var data = ReadData(reader);
                    return data;
                }
                #endregion
                return new List<T>();
            }
        }

        /// <summary>
        /// Thêm một bản ghi. 
        /// </summary>
        /// <param name="entity">Đối tượng muốn thêm mới</param>
        /// <returns>
        /// Extension này của Dapper trả về id (nếu id là int).
        /// Try catch ở chỗ dùng hàm này để kiểm tra thành công/thất bại.
        /// </returns>
        public async Task<int> AddAsync(T entity)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                return await _dapperDatabaseContext._dbConnection.InsertAsync<T>(entity);
            }
        }

        /// <summary>
        /// Hàm cập nhật bản ghi.
        /// </summary>
        /// <param name="entity">Đối tượng cần cập nhật</param>
        /// <param name="id">ID của bản ghi cần cập nhật</param>
        /// <returns>
        /// true: Cập nhật thành công
        /// false: Cập nhật thất bại
        /// </returns>
        public async Task<bool> UpdateAsync(T entity, string id)
        {
            var table = _tableName;
            if (_tableName.ToLower() == "userapp")
            {
                table = "User";
            }
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                //Buil command
                var sql = new StringBuilder($"UPDATE {_tableName} SET ");
                var props = entity.GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (prop.Name == "CreatedDate" || prop.Name == "CreatedBy") continue;
                    if (prop.Name == $"{table}ID") continue;
                    sql.Append($"{prop.Name} = @{prop.Name}, ");
                }
                sql.Remove(sql.Length - 2, 2).Append($" WHERE {table}ID=@id;");
                _dapperDatabaseContext._sqlCommand.CommandType = CommandType.Text;
                _dapperDatabaseContext._sqlCommand.CommandText = sql.ToString();
                foreach (var prop in props)
                {
                    if (prop.Name == $"{table}ID" ) continue;
                    var param = new MySqlParameter(prop.Name, prop.GetValue(entity));
                    _dapperDatabaseContext._sqlCommand.Parameters.Add(param);
                }
                _dapperDatabaseContext._sqlCommand.Parameters.Add(new MySqlParameter("id", id));
                await _dapperDatabaseContext._sqlCommand.PrepareAsync();
                return await _dapperDatabaseContext._sqlCommand.ExecuteNonQueryAsync() > 0;
            }
        }

        /// <summary>
        /// Hàm xóa nhiều
        /// </summary>
        /// <param name="ids">Danh sách id của các bản ghi cần xóa</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        public async Task<bool> DeleteAsync(string[] ids)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var sql = new StringBuilder($"DELETE FROM {_tableName} WHERE {_tableName}ID in @ids;");
                return (await _dapperDatabaseContext.ExecuteAsync(sql.ToString(), new { ids })) > 0;
            }
        }

        /// <summary>
        /// Hàm đếm bản ghi
        /// </summary>
        /// <param name="getListRequest">Object lọc contact</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <param name="isDefault">Có lấy các trường mặc định hay không</param>
        /// <returns>Số bản ghi</returns>
        public async Task<long> CountAsync(GetListRequest getListRequest, bool? isDeleted = null, bool? isDefault = false)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var instance = Activator.CreateInstance<T>();
                var props = instance.GetType().GetProperties();
                var filters = new List<List<FilterRequest>>();
                //Kiểm tra xem trường cần lấy có thật hay không, chống sql injection
                if (getListRequest.Filters != null)
                {
                    foreach (var item in getListRequest.Filters)
                    {
                        var filterAnds = new List<FilterRequest>();
                        foreach (var filterItem in item)
                        {
                            if (CommonFunction.HasProperty(instance, filterItem.Key)) filterAnds.Add(filterItem);
                        }
                        if (filterAnds.Count > 0) filters.Add(filterAnds);
                    }
                }
                var table = _tableName;
                if (table.ToLower() == "file")
                {
                    table = "files";
                }
                //Build câu truy vấn
                var sql = new StringBuilder($"SELECT COUNT({_tableName}ID) FROM {table} WHERE ");
                //Nếu có IsDeleted thì truyền thêm IsDeleted
                if (isDefault == true)
                {
                    sql.Append(" (IsDefault=TRUE) ");
                }

                if (isDeleted != null)
                {
                    sql.Append($" (IsDeleted={isDeleted}) ");
                }
                if (filters.Count > 0)
                {
                    sql.Append("AND ");
                    for (int i = 0; i < filters.Count; i++)
                    {
                        var andClauses = filters[i];
                        sql.Append(" ( ");
                        for (int j = 0; j < andClauses.Count; j++)
                        {
                            var andClause = andClauses[j];
                            var values = andClause.ValueArray;
                            var prefix = "";
                            switch (andClause.Operator)
                            {
                                case FilterType.Equal:
                                    sql.Append($"{andClause.Key} = @param{i}{j}");
                                    //sql.Append()
                                    break;
                                case FilterType.Contain:
                                    sql.Append($"{andClause.Key} LIKE CONCAT('%',@param{i}{j},'%')");
                                    break;
                                case FilterType.GreaterThan:
                                    sql.Append($"{andClause.Key} > @param{i}{j}");
                                    break;
                                case FilterType.GreaterThanEqual:
                                    sql.Append($"{andClause.Key} >= @param{i}{j}");
                                    break;
                                case FilterType.LessThan:
                                    sql.Append($"{andClause.Key} < @param{i}{j}");
                                    break;
                                case FilterType.LessThanEqual:
                                    sql.Append($"{andClause.Key} <= @param{i}{j}");
                                    break;
                                case FilterType.NotEqual:
                                    sql.Append($"{andClause.Key} != @param{i}{j}");
                                    break;
                                case FilterType.IsEmpty:
                                    sql.Append($"{andClause.Key} IS NULL");
                                    break;
                                case FilterType.IsNotEmpty:
                                    sql.Append($"{andClause.Key} IS NOT NULL");
                                    break;
                                case FilterType.ContainAnyOf: //17 - Chứa một trong những giá trị
                                    sql.Append(" ( ");
                                    for (int k = 0; k < values.Count; k++)
                                    {
                                        sql.Append($"{andClause.Key} LIKE CONCAT('%',@param{i}{j}{k}, '%') OR ");
                                        _dapperDatabaseContext._sqlCommand.Parameters.Add(new MySqlParameter($"param{i}{j}{k}", values[k]));
                                    }
                                    sql.Remove(sql.ToString().LastIndexOf(" OR "), 4).Append(")");
                                    break;
                                case FilterType.EqualToAnyOf:  //15 - Bằng một trong những giá trị
                                    sql.Append($"{andClause.Key} IN (");
                                    prefix = "";
                                    for (int k = 0; k < values.Count; k++)
                                    {
                                        sql.Append(prefix).Append($"@param{i}{j}{k}");
                                        _dapperDatabaseContext._sqlCommand.Parameters.Add(new MySqlParameter($"param{i}{j}{k}", values[k]));
                                        prefix = ",";
                                    }
                                    sql.Append(")");
                                    break;
                            }
                            sql.Append(" AND ");
                        }
                        sql.Remove(sql.ToString().LastIndexOf(" AND "), 5).Append(" ) OR ");
                    }
                    sql.Remove(sql.ToString().LastIndexOf(" OR "), 4);
                }
                _dapperDatabaseContext._sqlCommand.CommandType = CommandType.Text;
                _dapperDatabaseContext._sqlCommand.CommandText = sql.ToString();
                for (int i = 0; i < filters.Count; i++)
                {
                    for (int j = 0; j < filters[i].Count; j++)
                    {
                        var param = new MySqlParameter($"param{i}{j}", filters[i][j].Value);
                        _dapperDatabaseContext._sqlCommand.Parameters.Add(param);
                    }
                }
                await _dapperDatabaseContext._sqlCommand.PrepareAsync();
                var reader = await _dapperDatabaseContext._sqlCommand.ExecuteReaderAsync();
                reader.Read();
                return (long)reader.GetValue(0);
            }
        }

        /// <summary>
        /// Hàm xóa một bản ghi. 
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        public async Task<bool> DeleteOneAsync(string id)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var sql = new StringBuilder($"DELETE FROM {_tableName} WHERE {_tableName}ID = @id;");
                return (await _dapperDatabaseContext.ExecuteAsync(sql.ToString(), new { id })) > 0;
            }
        }

        /// <summary>
        /// Hàm xóa mềm nhiều bản ghi. 
        /// </summary>
        /// <param name="ids">Danh sách id của các bản ghi cần xóa</param>
        /// <param name="modifiedBy">Người chỉnh sửa gần nhất</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        public async Task<bool> SoftDeleteAsync(string[] ids, string modifiedBy)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var currentDateTime = DateTime.UtcNow;
                var sql = $"UPDATE {_tableName} " +
                    $"SET IsDeleted = b'1', ModifiedBy = @modifiedBy, ModifiedDate = @currentDateTime " +
                    $"WHERE {_tableName}ID in @ids;";
                return (await _dapperDatabaseContext.ExecuteAsync(sql, new { modifiedBy, currentDateTime, ids })) > 0;
            }
        }

        /// <summary>
        /// Hàm xóa mềm một bản ghi.
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <param name="modifiedBy">Người chỉnh sửa gần nhất</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        public async Task<bool> SoftDeleteOneAsync(string id, string modifiedBy)
        {
            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                var modifiedDate = DateTime.UtcNow;
                var sql = $"UPDATE {_tableName} " +
                    $"SET IsDeleted = b'1', ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate " +
                    $"WHERE {_tableName}ID = @id;";
                return (await _dapperDatabaseContext.ExecuteAsync(sql, new { modifiedBy, modifiedDate, id })) > 0;
            }
        }
        #endregion
    }
}
