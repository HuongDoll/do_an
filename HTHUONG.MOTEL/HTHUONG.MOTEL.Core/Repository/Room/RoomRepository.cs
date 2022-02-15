using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.Room
{
    public class RoomRepository : DapperRepository<Entities.Room>, IRoomRepository
    {
        public RoomRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }
    }
}
