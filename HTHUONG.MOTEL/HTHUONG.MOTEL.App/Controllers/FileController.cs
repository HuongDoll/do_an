using HTHUONG.MOTEL.App.BL;
using HTHUONG.MOTEL.App.BL.Room;
using HTHUONG.MOTEL.Core.Constants;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Controllers
{
    public class FileController: BaseApiController
    {
        private readonly IMinIOService _minIO;
        private readonly IFileService _fileService;
        private readonly IRoomBL _roomBL;

        public FileController(IMinIOService minIO, IFileService fileService, IRoomBL roomBL)
        {
            _minIO = minIO;
            _fileService= fileService;
            _roomBL = roomBL;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/public/files/{hostID}/{key}")]
        public async Task<IActionResult> GetFileByKeyAsync(string hostID, string key)
        {
            try
            {
                var byteContent = await _minIO.GetFileToMinIOAsync(hostID, key);
                String mimeType = MimeMapping.MimeUtility.GetMimeMapping(key);
                return File(byteContent, mimeType, key);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetFilesAsync([FromBody] GetListRequest getListRequest, long limit = 10, long offset = 0)
        {
            try
            {
                var userName = Request.Headers[HeaderKey.FULL_NAME].ToString();
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(CommonFunction.GenerateLackOfCompanyIDErrorResult(HttpContext));

                var data = await _fileService.GetFilesAsync(getListRequest, limit, offset);
                var total = await _fileService.CountFilesAsync(getListRequest);
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
        [Route("uploads")]
        public async Task<IActionResult> GetRoomsAsync([FromForm] List<IFormFile> files, [FromQuery] string hostID, [FromQuery] string roomID)
        {
            try
            {
                if (string.IsNullOrEmpty(hostID))
                {
                    return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
                }
                var links = new List<string>();
                foreach (var file in files)
                {
                    var link = await _minIO.UploadImageToMinIOAsync(file, hostID);
                    links.AddRange(link);
                    await _fileService.InsertFileAsync(new Core.Entities.File {HostID=Guid.Parse(hostID), RoomID= Guid.Parse(roomID), UrlMedium = link[0], UrlRaw = link[1] });
                }
                var room = await _roomBL.GetRoomByIDAsync(roomID);
                room.UrlThumbnail = links.ElementAtOrDefault(0);
                await _roomBL.UpdateRoomByIDAsync(room, room, room.ModifiedBy);
                return Ok(links);
            }
            catch (Exception ex)
            {
                return BadRequest(CommonFunction.GenerateGetExceptionErrorResult(HttpContext));
            }
        }
    }
}
