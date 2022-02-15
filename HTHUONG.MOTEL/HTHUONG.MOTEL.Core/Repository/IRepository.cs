using HTHUONG.MOTEL.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.Core.Repository
{
    /// <summary>
    /// Base interface repository.
    /// Gồm các hàm sử dụng để thao tác với database
    /// </summary>
    /// <typeparam name="T">Tên lớp thực thể. Ví dụ: CTA, Form</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Khởi tạo database context
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        void InitializeDatabaseContext(string connectionString);

        /// <summary>
        /// Lấy một bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi muốn lấy</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <returns>Đối tượng tương ứng với lớp thực thể T</returns>
        Task<T> GetByIdAsync(object id, bool? isDeleted = null);

        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <returns>Danh sách tất cả bản ghi có kiểu dữ liệu tương ứng với lớp thực thể T</returns>
        Task<IEnumerable<T>> ListAllAsync(bool? isDeleted = null);

        /// <summary>
        /// Lấy danh sách bản ghi (có lọc và phân trang)
        /// </summary>
        /// <param name="getListRequest">Object chứa dữ liệu để lọc bản ghi</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="offset">Số bản ghi bị bỏ qua</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <param name="isDefault">Có lấy các trường mặc định hay không</param>
        /// <returns>Danh sách bản ghi có kiểu dữ liệu tương ứng với lớp thực thể T</returns>
        Task<IEnumerable<T>> ListAsync(GetListRequest getListRequest, long limit, long offset, bool? isDeleted = null, bool? isDefault = false);

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng muốn thêm mới có kiểu dữ liệu tương ứng với lớp thực thể T</param>
        /// <returns>
        /// Trả về id nếu id là int. 
        /// Trả về 0 nếu id không phải int. 
        /// </returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// Cập nhật bản ghi theo ID
        /// </summary>
        /// <param name="entity">Đối tượng muốn cập nhật có kiểu dữ liệu tương ứng với lớp thực thể T</param>
        /// <param name="id">ID của bản ghi muốn cập nhật</param>
        /// <returns>
        /// true: Cập nhật thành công
        /// false: Cập nhật thất bại
        /// </returns>
        Task<bool> UpdateAsync(T entity, string id);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="ids">Danh sách ID của các bản ghi muốn xóa. ID là string dạng uuid</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        Task<bool> DeleteAsync(string[] ids);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi muốn xóa, ID là string dạng uuid</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        Task<bool> DeleteOneAsync(string id);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="ids">Danh sách ID của các bản ghi muốn xóa. ID là string dạng uuid</param>
        /// <param name="modifiedBy">Người chỉnh sửa gần nhất</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        Task<bool> SoftDeleteAsync(string[] ids, string modifiedBy);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi muốn xóa, ID là string dạng uuid</param>
        /// <param name="modifiedBy">Người chỉnh sửa gần nhất</param>
        /// <returns>
        /// true: Xóa thành công
        /// false: Xóa thất bại
        /// </returns>
        Task<bool> SoftDeleteOneAsync(string id, string modifiedBy);

        /// <summary>
        /// Đếm tổng số bản ghi
        /// </summary>
        /// <param name="getListRequest">Object chứa dữ liệu để lọc bản ghi</param>
        /// <param name="isDeleted">Trạng thái xóa mềm (null: cả bản ghi đã xóa mềm và chưa xóa, true: các bản ghi đã xóa mềm, false: các bản ghi chưa xóa)</param>
        /// <param name="isDefault">Có lấy các trường mặc định hay không</param>
        /// <returns>Số lượng bản ghi</returns>
        Task<long> CountAsync(GetListRequest getListRequest, bool? isDelete = null, bool? isDefault = false);
    }
}
