using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.DTOs
{
    public class GetListRequest
    {
        /// <summary>
        /// Các trường cần lấy
        /// </summary>
        public List<string> SelectedFields { get; set; }

        /// <summary>
        /// Bộ lọc
        /// </summary>
        public List<List<FilterRequest>> Filters { get; set; }

        /// <summary>
        /// Mảng các giá trị dùng để sắp xếp
        /// +prop: Sắp xếp tăng. 
        /// -prop: Sắp xếp giảm
        /// </summary>
        public List<string> Orders { get; set; }
    }
}
