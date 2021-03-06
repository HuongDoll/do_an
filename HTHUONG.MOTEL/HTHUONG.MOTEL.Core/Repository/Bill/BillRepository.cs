using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.Bill
{
    public class BillRepository : DapperRepository<Entities.Bill>, IBillRepository
    {
        public BillRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }
    }
}
