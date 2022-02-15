using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.DTOs.AjaxResult
{
    public class ErrorResult
    {
        /// <summary>
        /// Thông báo lỗi cho Developer. Mô tả chung về lỗi, cho phép Developer có thể nắm được vấn đề đang gặp phải.
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Thông báo lỗi cho người dùng cuối. Tùy yêu cầu của dự án tích hợp mà thông báo này có thể được hiển thị lên hay không.
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Mã lỗi trả về
        /// </summary>
        public string? ErrCode { get; set; }

        /// <summary>
        /// Danh sách các lỗi validate
        /// </summary>
        public List<ValidationFailure> ValidationFailures { get; set; }
    }
}
