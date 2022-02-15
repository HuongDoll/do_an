using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTHUONG.MOTEL.Core.Constants.Enumeration;

namespace HTHUONG.MOTEL.Core.DTOs
{
    public class FilterRequest
    {
        /// <summary>
        /// Tên trường cần lọc
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Giá trị lọc
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Mảng các giá trị cho các toán tử đa ngôi
        /// </summary>
        public List<object> ValueArray { get; set; }

        /// <summary>
        /// Toán tử lọc
        /// </summary>
        public FilterType Operator { get; set; }
    }
}
