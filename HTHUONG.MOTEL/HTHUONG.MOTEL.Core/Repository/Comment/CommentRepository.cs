using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.User
{
    public class CommentRepository : DapperRepository<Entities.Comment>, ICommentRepository
    {
        public CommentRepository(IDatabaseContextFactory factory) : base(factory)
        {
        }
    }
}
