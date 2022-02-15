using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Constants
{
    public class Enumeration
    {
        /// <summary>
        /// Kiểu lọc
        /// </summary>
        public enum FilterType
        {
            Equal = 0,
            Contain = 1,
            NotEqual = 2,
            GreaterThan = 3,
            LessThan = 4,
            GreaterThanEqual = 5,
            LessThanEqual = 6,
            IsNotEmpty = 7,
            NotContain = 8,
            HasFilledOutForm = 9,
            HasNotFilledOutForm = 10,
            Between = 11,
            IsAnyOf = 12,
            IsNoneOf = 13,
            IsEmpty = 14,
            EqualToAnyOf = 15,
            NotEqualToAnyOf = 16,
            ContainAnyOf = 17,
            NotContainAnyOf = 18,
            StartWith = 19,
            EndWith = 20,
            EqualIn = 34,   //Mới chỉ cho Number
            HasFilterFamily = 35,
            HasNotFilterFamily = 36
        }

        public enum UserType
        {
            User = 0,
            Host = 1,
        }

        public enum RoomStatus
        {
            Empty = 0, // Mới tạo
            Process = 1, // CÓ Yêu Cầu
            Full = 2, // Đã cho thuê
            Save = 3, // Đã có bài đăng,
            Publish = 4 // Đã đăng bài
        }

        public enum BillStatus
        {
            None = 0,
            Success = 1
        }
    }
}
