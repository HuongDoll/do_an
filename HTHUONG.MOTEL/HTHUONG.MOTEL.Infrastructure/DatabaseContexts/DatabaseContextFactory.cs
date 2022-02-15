using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Infrastructure
{
    public class DatabaseContextFactory : IDatabaseContextFactory
    {

        /// <summary>
        /// Khởi tạo mới 1 Db Context nếu chưa có
        /// </summary>
        /// <returns>dataContext</returns>
        public IDapperDatabaseContext Context()
        {
            return new DapperDatabaseContext();
        }
    }
}
