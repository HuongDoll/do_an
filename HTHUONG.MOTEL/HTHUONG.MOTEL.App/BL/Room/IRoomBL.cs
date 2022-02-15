using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL.Room
{
    public interface IRoomBL
    {
        Task<long> CountRoomsAsync(GetListRequest getListRequest);

        Task<Core.Entities.Room> GetRoomByIDAsync(string roomID);

        Task<IEnumerable<Core.Entities.Room>> GetRoomsAsync(GetListRequest getListRequest, long limit, long offset);

        Task<Guid> InsertRoomAsync(Core.Entities.Room room, string userFullName);

        Task<bool> UpdateRoomByIDAsync(Core.Entities.Room room, Core.Entities.Room oldRoom, string userFullName);

        Task<bool> SoftDeleteRoomByIDAsync(string id, string modifiedBy);

    }
}
