using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTHUONG.MOTEL.Core.Constants.Enumeration;

namespace HTHUONG.MOTEL.Core.Entities
{
    [Table("room")]
    public class Room : BaseEntity
    {
        public Guid RoomID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = Guid.Empty;

        [Required]
        public Guid HostID { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public int Price { get; set; }
        public RoomStatus Status { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string UrlThumbnail { get; set; } = "";


        /// <summary>
        /// Đã bị xóa hay chưa (Xóa mềm): 0: chưa bị xóa; 1: đã bị xóa
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
