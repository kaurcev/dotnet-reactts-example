using backend.Models;

namespace backend.Interfaces
{
    public interface IDateTimeRepository
    {
        Task<IEnumerable<DateTimeInfo>> GetAllAsync();
        Task<DateTimeInfo?> GetByIdAsync(int id);
        Task<DateTimeInfo> CreateAsync(DateTimeInfo dateTimeInfo);
        Task<DateTimeInfo?> UpdateAsync(int id, DateTimeInfo dateTimeInfo);
        Task<bool> DeleteAsync(int id);
        Task<DateTimeInfo> GetCurrentDateTimeAsync();
        Task<DateTimeInfo> GetUtcDateTimeAsync();
    }
}
