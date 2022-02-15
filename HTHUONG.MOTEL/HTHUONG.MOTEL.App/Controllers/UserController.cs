using HTHUONG.MOTEL.App.BL;
using HTHUONG.MOTEL.Core.Constants;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.DTOs.AjaxResult;
using HTHUONG.MOTEL.Core.Repository.User;
using HTHUONG.MOTEL.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserBL _userBL;
        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpGet]
        [Route("{userID}")]
        public async Task<IActionResult> GetEmailAsync(string userID)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var user = await _userBL.GetUserByIDAsync(userID);
                if (user == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var result = await _userBL.Login(loginDTO);
                if (result == null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] Core.Entities.UserApp user)
        {
            try
            {
                var createCompanyID = await _userBL.AddAsync(user);
                return Ok(createCompanyID);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateCreateExceptionErrorResult(HttpContext));
            }
        }

        [HttpPut]
        [Route("{userID}")]
        public async Task<IActionResult> UpdateEmailByIDAsync(string userID, [FromBody] UserDTO user, [FromQuery] bool isChangePassword = false)
        {
            try
            {
                var fullname = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(fullname))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));



                var oldUser = await _userBL.GetUserByIDAsync(userID);
                if (oldUser == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                var userNew = new Core.Entities.UserApp();

                if (isChangePassword)
                {
                    var isMapPassword = _userBL.IsMapPassword(user.Password, oldUser.Password);
                    if (!isMapPassword)
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, new ErrorResult
                        {
                            DevMsg = DevMessage.EXCEPTION,
                            UserMsg = "Password không trùng"
                        });
                    }
                    oldUser.Password = _userBL.ChangePassword(user.NewPassword);
                    userNew = oldUser;
                }
                else
                {
                    userNew.UserID = oldUser.UserID;
                    userNew.FullName = user.FullName;
                    userNew.UserName = user.UserName;
                    userNew.Password = oldUser.Password;
                    userNew.Email = user.Email;
                    userNew.Phone = user.Phone;
                    userNew.UserType = oldUser.UserType;
                    userNew.CreatedDate = oldUser.CreatedDate;
                    userNew.CreatedBy = oldUser.CreatedBy;
                }


                var isUpdate = await _userBL.UpdateUserByIDAsync(userNew, oldUser, fullname);

                return Ok(isUpdate);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult
                {
                    DevMsg = DevMessage.EXCEPTION,
                    UserMsg = UserMessage.CREATE_EXCEPTION
                });
            }
        }

    }
}
