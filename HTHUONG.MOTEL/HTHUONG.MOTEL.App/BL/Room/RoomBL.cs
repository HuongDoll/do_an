using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Repository.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL.Room
{
    public class RoomBL : IRoomBL
    {
        private readonly IRoomRepository _roomRepository;
        public RoomBL(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<long> CountRoomsAsync(GetListRequest getListRequest)
        {
            return await _roomRepository.CountAsync(getListRequest, false);

        }

        public async Task<Core.Entities.Room> GetRoomByIDAsync(string emailID)
        {
            return await _roomRepository.GetByIdAsync(emailID, false);
        }

        public async Task<IEnumerable<Core.Entities.Room>> GetRoomsAsync(GetListRequest getListRequest, long limit, long offset)
        {
            return await _roomRepository.ListAsync(getListRequest, limit, offset, false);
        }

        public async Task<Guid> InsertRoomAsync(Core.Entities.Room room, string userFullName)
        {
            var now = DateTime.UtcNow;
            var roomNew = new Core.Entities.Room();
            roomNew = room;
            roomNew.RoomID = Guid.NewGuid();
            roomNew.CreatedDate = DateTime.Now;
            roomNew.PublishDate = DateTime.Now;

            await _roomRepository.AddAsync(roomNew);
            return roomNew.RoomID;
        }

        public async Task<bool> SoftDeleteRoomByIDAsync(string id, string modifiedBy)
        {
            return await _roomRepository.SoftDeleteOneAsync(id, modifiedBy);
        }

        public async Task<bool> UpdateRoomByIDAsync(Core.Entities.Room room, Core.Entities.Room oldRoom, string userFullName)
        {
            room.ModifiedDate = DateTime.Now;
            room.ModifiedBy = userFullName;

            return await _roomRepository.UpdateAsync(room, oldRoom.RoomID.ToString());
        }
    }
}
