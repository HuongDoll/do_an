using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTHUONG.MOTEL.Core.Constants.Enumeration;

namespace HTHUONG.MOTEL.Core.Entities
{
    [Table("bill")]
    public class Bill : BaseEntity
    {
        public Guid BillID { get; set; }
        public BillStatus Status { get; set; }
        public Guid UserID { get; set; }
        public Guid HostID { get; set; }
        public int PriceRoom { get; set; }
        public int PriceService { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Đã bị xóa hay chưa (Xóa mềm): 0: chưa bị xóa; 1: đã bị xóa
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
