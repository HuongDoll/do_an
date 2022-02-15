using HTHUONG.MOTEL.App.BL.Room;
using HTHUONG.MOTEL.Core.Constants;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.DTOs.AjaxResult;
using HTHUONG.MOTEL.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly IRoomBL _roomBL;
        public RoomController(IRoomBL roomBL)
        {
            _roomBL = roomBL;
        }

        [HttpGet]
        [Route("{roomID}")]
        public async Task<IActionResult> GetRoomAsync(string roomID)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var room = await _roomBL.GetRoomByIDAsync(roomID);
                if (room == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }


        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetRoomsAsync([FromBody] GetListRequest getListRequest, long limit = 10, long offset = 0)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var data = await _roomBL.GetRoomsAsync(getListRequest, limit, offset);
                var total = await _roomBL.CountRoomsAsync(getListRequest);
                return Ok(new 
                {
                    Data = data.ToList(),
                    TotalRecords = total
                });
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertRoomAsync([FromBody] Core.Entities.Room room)
        {
            try
            {
                var fullname = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(fullname))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                if (!ModelState.IsValid)
                {
                    var errorResponse = new ErrorResult
                    {
                        DevMsg = DevMessage.INVALID_MODEL,
                        UserMsg = UserMessage.INVALID_MODEL
                    };
                    errorResponse.ValidationFailures = CommonFunction.GetErrorMessages(ModelState);

                    return BadRequest(errorResponse);
                }

                

                var roomID = await _roomBL.InsertRoomAsync(room, fullname);
                if (string.IsNullOrEmpty(roomID.ToString()))
                {
                    return BadRequest("INSERT_FAILED");
                }
                return StatusCode((int)HttpStatusCode.Created, roomID);
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


        [HttpPut]
        [Route("{roomID}")]
        public async Task<IActionResult> UpdateEmailByIDAsync(string roomID, [FromBody] Core.Entities.Room room)
        {
            try
            {
                var fullname = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(fullname))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                if (!ModelState.IsValid)
                {
                    var errorResponse = new ErrorResult
                    {
                        DevMsg = DevMessage.INVALID_MODEL,
                        UserMsg = UserMessage.INVALID_MODEL
                    };
                    errorResponse.ValidationFailures = CommonFunction.GetErrorMessages(ModelState);
                    return BadRequest(errorResponse);
                }


                var oldRoom = await _roomBL.GetRoomByIDAsync(roomID);
                if (oldRoom == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                if (oldRoom.Status == Enumeration.RoomStatus.Full)
                {
                    return BadRequest(new ErrorResult
                    {
                        DevMsg = "Room busy",
                        UserMsg = "Đã có người thuê"
                    });
                }


                var isUpdate = await _roomBL.UpdateRoomByIDAsync(room, oldRoom, fullname);

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


        [HttpDelete]
        [Route("{roomID}")]
        public async Task<IActionResult> SoftDeleteEmailAsync(string roomID)
        {
            try
            {
                var currentDateTime = DateTime.UtcNow;
                var userFullName = Request.Headers[HeaderKey.FULL_NAME].ToString();

                var roomDelete = await _roomBL.GetRoomByIDAsync(roomID);
                if (roomDelete == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                if(roomDelete.Status != Enumeration.RoomStatus.Empty)
                {
                    return BadRequest(new ErrorResult
                    {
                        DevMsg = "Room busy",
                        UserMsg = "Đã có người thuê"
                    });
                }

                var isDelete = await _roomBL.SoftDeleteRoomByIDAsync(roomDelete.RoomID.ToString(), userFullName);
                return isDelete? Ok() : BadRequest(new ErrorResult
                {
                    DevMsg = DevMessage.DELETE_FAILED,
                    UserMsg = UserMessage.DELETE_EXCEPTION
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResult
                {
                    DevMsg = DevMessage.EXCEPTION,
                    UserMsg = UserMessage.DELETE_EXCEPTION
                });
            }
        }

    }
}
