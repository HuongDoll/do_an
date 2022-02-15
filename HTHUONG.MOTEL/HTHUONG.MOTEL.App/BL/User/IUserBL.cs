using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public interface IUserBL
    {
        Task<object> Login(LoginDTO loginDTO);

        Task<string> AddAsync(Core.Entities.UserApp user);

        Task<Core.Entities.UserApp> GetUserByIDAsync(string userID);

        Task<bool> UpdateUserByIDAsync(Core.Entities.UserApp user, Core.Entities.UserApp oldUser, string userFullName);

        bool IsMapPassword(string password, string passwordOld);

        string ChangePassword(string newPassword);
    }
}
