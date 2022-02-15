using Dapper.Contrib.Extensions;
using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HTHUONG.MOTEL.Core.Constants.Enumeration;

namespace HTHUONG.MOTEL.Core.Entities
{
    [Table("userapp")]
    public class UserApp : BaseEntity
    {
        public Guid UserID { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public UserType UserType { get; set; }


        /// <summary>
        /// Đã bị xóa hay chưa (Xóa mềm): 0: chưa bị xóa; 1: đã bị xóa
        /// </summary>
        public bool IsDeleted { get; set; } = false;

       
    }
}
