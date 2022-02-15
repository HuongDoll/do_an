using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL.Bill
{
    public interface IBillService
    {
        Task<long> CountBillAsync(GetListRequest getListRequest);

        Task<Core.Entities.Bill> GetBillByIDAsync(string billID);

        Task<IEnumerable<Core.Entities.Bill>> GetBillsAsync(GetListRequest getListRequest, long limit, long offset);

        Task<Guid> InsertBillAsync(Core.Entities.Bill bill, string userFullName);

        Task<bool> UpdateBillByIDAsync(Core.Entities.Bill bill, Core.Entities.Bill oldBill, string userFullName);
    }
}
