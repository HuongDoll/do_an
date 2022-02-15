using HTHUONG.MOTEL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.File
{
    public class FileRepository : DapperRepository<Entities.File>, IFileRepository
    {
        public FileRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }
    }
}
