using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public interface IFileService
    {
        Task<Guid> InsertFileAsync(Core.Entities.File file);

        Task<long> CountFilesAsync(GetListRequest getListRequest);

        Task<IEnumerable<Core.Entities.File>> GetFilesAsync(GetListRequest getListRequest, long limit, long offset);
    }
}
