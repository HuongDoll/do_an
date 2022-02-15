using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.DTOs
{
    public class ValidationFailure
    {
        /// <summary>
        /// Tên thuộc tính
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Lý do validate lỗi
        /// </summary>
        public string FailureReason { get; set; }
    }
}
