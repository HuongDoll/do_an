using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Infrastructure
{
    public interface IDatabaseContextFactory
    {
        IDapperDatabaseContext Context();
    }
}
