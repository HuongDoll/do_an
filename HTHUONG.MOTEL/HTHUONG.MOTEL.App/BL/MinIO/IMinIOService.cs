using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public interface IMinIOService
    {

        /// <summary>
        /// Upload ảnh lên MinIo
        /// </summary>
        /// <param name="file">file upload</param>
        /// <param name="hostID">id công ty</param>
        /// <returns>Đường dẫn trên MinIO</returns>
        Task<List<string>> UploadImageToMinIOAsync(IFormFile file, string hostID);

        Task<byte[]> GetFileToMinIOAsync(string hostID, string key);
    }
}
