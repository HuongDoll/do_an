using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Repository.Bill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL.Bill
{
    public class BillService: IBillService
    {
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public async Task<long> CountBillAsync(GetListRequest getListRequest)
        {
            return await _billRepository.CountAsync(getListRequest, false);

        }

        public async  Task<Core.Entities.Bill> GetBillByIDAsync(string billID)
        {
            return await _billRepository.GetByIdAsync(billID, false);
        }

        public async  Task<IEnumerable<Core.Entities.Bill>> GetBillsAsync(GetListRequest getListRequest, long limit, long offset)
        {
            return await _billRepository.ListAsync(getListRequest, limit, offset, false);
        }

        public async  Task<Guid> InsertBillAsync(Core.Entities.Bill bill, string userFullName)
        {
            var billNew = new Core.Entities.Bill();
            billNew = bill;
            billNew.BillID = Guid.NewGuid();
            billNew.CreatedDate = DateTime.UtcNow;

            await _billRepository.AddAsync(billNew);
            return billNew.BillID;
        }

        public async  Task<bool> UpdateBillByIDAsync(Core.Entities.Bill bill, Core.Entities.Bill oldBill, string userFullName)
        {
            bill.ModifiedDate = DateTime.Now;
            bill.ModifiedBy = userFullName;

            return await _billRepository.UpdateAsync(bill, oldBill.BillID.ToString());
        }
    }
}
