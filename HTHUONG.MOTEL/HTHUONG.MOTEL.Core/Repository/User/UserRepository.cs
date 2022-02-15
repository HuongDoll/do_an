using Dapper;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Infrastructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.User
{
    public class UserRepository : DapperRepository<Entities.UserApp>, IUserRepository
    {
        public UserRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }

        public async Task<Entities.UserApp> GetUser(string userName, string password)
        {
            var sql = $"select * from {_tableName} where UserName='{userName}' and Password='{password}';";

            using (var _dapperDatabaseContext = (DapperDatabaseContext)_factory.Context())
            {
                _dapperDatabaseContext._sqlCommand.CommandType = CommandType.Text;
                _dapperDatabaseContext._sqlCommand.CommandText = sql.ToString();
                await _dapperDatabaseContext._sqlCommand.PrepareAsync();
                var reader = await _dapperDatabaseContext._sqlCommand.ExecuteReaderAsync();

                //Đọc data từ reader
                var data = ReadData(reader);
                var company = data.AsList();
                return company.Count > 0 ? company[0] : null;
            }
        }
    }
}
