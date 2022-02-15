using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.User
{
    public class HistoryRepository : DapperRepository<Entities.History>, IHistoryRepository
    {
        public HistoryRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }
    }
}
