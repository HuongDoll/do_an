using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository.User
{
    public interface IUserRepository : IRepository<Entities.UserApp>
    {
        Task<Entities.UserApp> GetUser(string email, string password);
    }
}
