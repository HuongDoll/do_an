using HTHUONG.MOTEL.Core.Constants;
using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.DTOs.AjaxResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Utils
{
    public static class SysConfig
    {
        public static IConfiguration configuration;
    }

    public class CommonFunction
    {
        private static IConfiguration _configuration;

        static CommonFunction()
        {
            _configuration = SysConfig.configuration;
        }

        /// <summary>
        /// Lấy giá trị connection string theo tên từ file appsettings
        /// </summary>
        /// <param name="name">Tên connection string</param>
        /// <returns>Connection string</returns>
        public static string GetConnectString(string name)
        {
            return _configuration.GetConnectionString(name);
        }

        /// <summary>
        /// Lấy Value trong file appsettings theo Key (1 cấp)
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value tương ứng với Key</returns>
        public static string GetAppSettings(string key)
        {
            try
            {
                return _configuration.GetSection(key).Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy Value trong file appsettings theo Key (2 cấp)
        /// </summary>
        /// <param name="key">Key cấp 1</param>
        /// <param name="subKey">Key cấp 2</param>
        /// <returns>Value tương ứng với Key</returns>
        public static string GetAppSettings(string key, string subKey)
        {
            try
            {
                return _configuration.GetSection(key).GetSection(subKey).Value ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy Value trong file appsettings theo Key (3 cấp)
        /// </summary>
        /// <param name="key">Key cấp 1</param>
        /// <param name="subKey">Key cấp 2</param>
        /// <param name="subKey2">Key cấp 3</param>
        /// <returns>Value tương ứng với Key</returns>
        public static string GetAppSettings(string key, string subKey, string subKey2)
        {
            try
            {
                return _configuration.GetSection(key).GetSection(subKey).GetSection(subKey2).Value ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static IConfigurationSection GetAppSettingConfig(string key, string subKey = null)
        {
            try
            {
                if (subKey == null)
                {
                    return _configuration.GetSection(key);
                }
                else
                {
                    return _configuration.GetSection(key).GetSection(subKey);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                return null;
            }
        }


        /// <summary>
        /// Lấy thông tin ErrorMessages từ ModelState
        /// </summary>
        /// <param name="modelStateDictionary">Đối tượng ModelStateDictionary</param>
        /// <returns>Danh sách mã message lỗi</returns>
        public static List<ValidationFailure> GetErrorMessages(ModelStateDictionary modelStateDictionary)
        {
            var validationFailures = new List<ValidationFailure>();

            foreach (var modelStateKey in modelStateDictionary.Keys)
            {
                var modelStateVal = modelStateDictionary[modelStateKey];

                foreach (var error in modelStateVal.Errors)
                {
                    validationFailures.Add(new ValidationFailure
                    {
                        Property = modelStateKey,
                        FailureReason = error.ErrorMessage
                    });
                }
            }

            return validationFailures;
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải Json hay không
        /// </summary>
        /// <param name="inputString">Chuỗi muốn kiểm tra</param>
        /// <returns>
        /// true: Đúng là JSON
        /// false: Sai định dạng JSON
        /// </returns>
        public static bool IsValidJsonFormat(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return false;
            }
            inputString = inputString.Trim();
            if ((inputString.StartsWith("{") && inputString.EndsWith("}")) || //For object
                (inputString.StartsWith("[") && inputString.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(inputString);
                    return true;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra PropertyName có phải của Object hay không
        /// </summary>
        /// <param name="obj">Object cần kiểm tra: Nếu dùng <T> - Activator.CreateInstance<T>()</T></param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <returns>
        /// True: Nếu đúng
        /// False: Nếu sai
        /// </returns>
        public static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Trả về thông tin lỗi khi kết nối database thất bại
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateConnectDatabaseFailedErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.CONNECT_DATABASE_FAILED,
                UserMsg = UserMessage.CONNECT_DATABASE_FAILED
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi thiếu trường x-company-id trong header
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateLackOfCompanyIDErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.LACK_OF_FULL_NAME,
                UserMsg = UserMessage.LACK_OF_HEADERS
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi thiếu trường x-full-name trong header
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateLackOfFullNameErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.LACK_OF_FULL_NAME,
                UserMsg = UserMessage.LACK_OF_HEADERS
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi thêm mới dữ liệu gặp exception
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateCreateExceptionErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.EXCEPTION,
                UserMsg = UserMessage.CREATE_EXCEPTION
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi cập nhật dữ liệu gặp exception
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateUpdateExceptionErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.EXCEPTION,
                UserMsg = UserMessage.UPDATE_EXCEPTION
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi lấy dữ liệu gặp exception
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateGetExceptionErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.EXCEPTION,
                UserMsg = UserMessage.GET_EXCEPTION
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi xóa dữ liệu gặp exception
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateDeleteExceptionErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.EXCEPTION,
                UserMsg = UserMessage.DELETE_EXCEPTION
            };
        }

        /// <summary>
        /// Trả về thông tin lỗi khi đối tượng truyền lên không hợp lệ
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>Đối tượng ErrorResult</returns>
        public static ErrorResult GenerateInvalidModelErrorResult(HttpContext httpContext)
        {
            return new ErrorResult
            {
                DevMsg = DevMessage.INVALID_MODEL,
                UserMsg = UserMessage.INVALID_MODEL
            };
        }
    }
}
