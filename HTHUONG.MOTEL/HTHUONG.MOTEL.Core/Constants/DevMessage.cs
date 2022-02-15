using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Constants
{
    public static class DevMessage
    {
        /// <summary>
        /// Thiếu trường x-full-name trong request header
        /// </summary>
        public const string LACK_OF_FULL_NAME = "Lack of x-full-name in request header.";

        /// <summary>
        /// Lỗi kết nối tới database
        /// </summary>
        public const string CONNECT_DATABASE_FAILED = "Connect to database failed.";

        /// <summary>
        /// Xóa bản ghi trong DB thất bại
        /// </summary>
        public const string DELETE_FAILED = "Could not delete the record(s) in database.";

        /// <summary>
        /// Bắt được một exception
        /// </summary>
        public const string EXCEPTION = "Catched an exception.";

        /// <summary>
        /// Dữ liệu đẩy lên trong request body không hợp lệ
        /// </summary>
        public const string INVALID_MODEL = "Model state values are invalid.";

    }
}
