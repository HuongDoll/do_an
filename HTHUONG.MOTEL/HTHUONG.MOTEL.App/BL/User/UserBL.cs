using HTHUONG.MOTEL.Core.Authentication;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Repository.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public class UserBL : IUserBL
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationConfig _authenticationConfig;
        public UserBL(IUserRepository userRepository, AuthenticationConfig authenticationConfig)
        {
            _userRepository = userRepository;
            _authenticationConfig = authenticationConfig;
        }

        public async Task<string> AddAsync(Core.Entities.UserApp user)
        {
            user.UserID = Guid.NewGuid();
            user.CreatedDate = DateTime.Now;
            user.Password = HashPassWord("this is a secret text", user.Password);

            await _userRepository.AddAsync(user);
            return user.UserID.ToString();
        }

        public string ChangePassword(string newPassword)
        {
            return HashPassWord("this is a secret text", newPassword);
        }

        public async Task<Core.Entities.UserApp> GetUserByIDAsync(string userID)
        {
            var user = await _userRepository.GetByIdAsync(userID);
            return user;
        }

        public bool IsMapPassword(string password, string passwordOld)
        {
            var passwordCheck = HashPassWord("this is a secret text", password);
            return passwordCheck == passwordOld;
        }

        public async Task<object> Login(LoginDTO loginDTO)
        {
            loginDTO.Password = HashPassWord("this is a secret text", loginDTO.Password);
            var user = await _userRepository.GetUser(loginDTO.UserName, loginDTO.Password);

            if (user == null) return null;

            var acessToken = _authenticationConfig.GenerateAccessToken(user.FullName, user.Email);

            var result = new
            {
                access = acessToken,
                email = user.Email,
                fullName = user.FullName,
                userID = user.UserID,
                userType = user.UserType
            };
            return result;
        }

        public async Task<bool> UpdateUserByIDAsync(Core.Entities.UserApp user, Core.Entities.UserApp oldUser, string userFullName)
        {
            user.ModifiedDate = DateTime.Now;
            user.ModifiedBy = userFullName;
            user.IsDeleted = false;

            return await _userRepository.UpdateAsync(user, oldUser.UserID.ToString());
        }


        // Mã hóa mật khẩu
        private string HashPassWord(string saltDB, string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(saltDB);

            //byte[] salt = System.Convert.FromBase64String(saltDB);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");
            return hashed;
        }
    }
}
