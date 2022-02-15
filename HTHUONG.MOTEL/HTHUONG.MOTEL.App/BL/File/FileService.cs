using HTHUONG.MOTEL.Core.DTOs;
using HTHUONG.MOTEL.Core.Entities;
using HTHUONG.MOTEL.Core.Repository.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.BL
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<long> CountFilesAsync(GetListRequest getListRequest)
        {
            return await _fileRepository.CountAsync(getListRequest, false);
        }

        public async Task<IEnumerable<File>> GetFilesAsync(GetListRequest getListRequest, long limit, long offset)
        {
            return await _fileRepository.ListAsync(getListRequest, limit, offset, false);
        }

        public async Task<Guid> InsertFileAsync(File file)
        {
            var now = DateTime.UtcNow;
            var fileNew = new Core.Entities.File();
            fileNew = file;
            fileNew.FileID = Guid.NewGuid();
            fileNew.CreatedDate = DateTime.UtcNow;

            await _fileRepository.AddAsync(fileNew);
            return fileNew.RoomID;
        }
    }
}
