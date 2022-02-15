using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Entities
{
    public class File : BaseEntity
    {
        public Guid FileID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid RoomID { get; set; } 

        [Required]
        public Guid HostID { get; set; }

        public string UrlMedium { get; set; } = "";

        public string UrlRaw { get; set; } = "";

        public bool IsDeleted { get; set; } = false;

    }
}
