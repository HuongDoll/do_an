using Microsoft.AspNetCore.Http;
using Minio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public class MinIOService : IMinIOService
    {
        private MinioClient _minioClient;
        private static int check = 0;
        private readonly string BucketName = "motel";
        private readonly int _widthMedium = 256;
        private readonly int _heightMedium = 171;

        public MinIOService()
        {
            _minioClient = new MinioClient("127.0.0.1:9000",
                                          accessKey: "minioadmin",
                                          secretKey: "minioadmin"
                                          );
        }
        private async Task<bool> CheckAndCreateBucketNameAsync()
        {
            try
            {
                if (check == 0)
                {
                    if (!await _minioClient.BucketExistsAsync(BucketName))
                    {
                        await _minioClient.MakeBucketAsync(BucketName);
                    }
                    check = 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} Err CheckAndCreateBucketName: {ex}");
                return false;
            }
        }

        private async Task<byte[]> GetByteArrayFromImageAsync(IFormFile file, int width = 0, int height = 0, string typeFile = "")
        {
            if (typeFile == "image" && width != 0 && height != 0)
            {
                var inStream = file.OpenReadStream();
                var outStream = new MemoryStream();
                using (var image = Image.Load(inStream, out IImageFormat format))
                {
                    double w = (double)image.Width;
                    double h = (double)image.Height;
                    double scale = w > h ? w / width : h / height;
                    double hh, ww;
                    if (w > h)
                    {
                        hh = h / scale;
                        height = (int)hh;
                    }
                    else
                    {
                        ww = w / scale;
                        width = (int)ww;
                    }
                    image.Mutate(i => i.Resize(width, height));
                    image.Save(outStream, format);
                }
                return outStream.ToArray();
            }
            else
            {
                using var target = new MemoryStream();
                await file.CopyToAsync(target);
                return target.ToArray();
            }

        }

        /// <summary>
        /// Upload ảnh lên MinIo
        /// </summary>
        /// <param name="file">file upload</param>
        /// <param name="hostID">ID công ty</param>
        /// <returns>Đường dẫn trên MinIO</returns>
        public async Task<List<string>> UploadImageToMinIOAsync(IFormFile file, string hostID)
        {
            var isTrue = await CheckAndCreateBucketNameAsync();
            if (!isTrue) return null;
            var url = new List<string>();
            String extend = Path.GetExtension(Regex.Replace(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName, "\\s+", "-").Trim('"'));
            var mimeType = MimeMapping.MimeUtility.GetMimeMapping(file.FileName);
            var id = Guid.NewGuid();
            //var myBytes = await _minIO.GetByteArrayFromImageAsync(file, _widthSmall, _heightSmall, "image");
            //String fileKey = $"img_{id}_{_widthSmall}x{_heightSmall}{extend}";
            //await _minIO.PutObject(_minIOBucketName, fileKey, myBytes, companyID);

            var myBytes = await GetByteArrayFromImageAsync(file, _widthMedium, _heightMedium, "image");
            var fileKey = $"{hostID}/img_{id}_medium{extend}";
            using (MemoryStream filestream = new MemoryStream(myBytes))
            {
                await _minioClient.PutObjectAsync(BucketName, fileKey, filestream, filestream.Length, mimeType);
            }
            url.Add(fileKey);

            myBytes = await GetByteArrayFromImageAsync(file, 0, 0, "image");
            fileKey = $"{hostID}/img_{id}{extend}";
            using (MemoryStream filestream = new MemoryStream(myBytes))
            {
                await _minioClient.PutObjectAsync(BucketName, fileKey, filestream, filestream.Length, mimeType);
            }
            url.Add(fileKey);

            return url;
        }

        public async Task<byte[]> GetFileToMinIOAsync(string hostID, string key)
        {
            try
            {
                var byteData = new MemoryStream();
                await _minioClient.GetObjectAsync(BucketName, $"{hostID}/{key}", (stream) =>
                {
                    stream.CopyTo(byteData);
                });
                return byteData.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                return null;
            }

        }
    }
}
