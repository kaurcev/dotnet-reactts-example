using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class DateTimeService
    {
        private readonly IDateTimeRepository _dateTimeRepository;

        public DateTimeService(IDateTimeRepository dateTimeRepository)
        {
            _dateTimeRepository = dateTimeRepository;
        }

        public async Task<IEnumerable<DateTimeInfo>> GetAllDateTimeEntriesAsync()
        {
            return await _dateTimeRepository.GetAllAsync();
        }

        public async Task<DateTimeInfo?> GetDateTimeEntryByIdAsync(int id)
        {
            return await _dateTimeRepository.GetByIdAsync(id);
        }

        public async Task<DateTimeInfo> CreateDateTimeEntryAsync(DateTimeInfo dateTimeInfo)
        {
            return await _dateTimeRepository.CreateAsync(dateTimeInfo);
        }

        public async Task<DateTimeInfo?> UpdateDateTimeEntryAsync(int id, DateTimeInfo dateTimeInfo)
        {
            return await _dateTimeRepository.UpdateAsync(id, dateTimeInfo);
        }

        public async Task<bool> DeleteDateTimeEntryAsync(int id)
        {
            return await _dateTimeRepository.DeleteAsync(id);
        }

        public async Task<DateTimeInfo> GetCurrentDateTimeAsync()
        {
            return await _dateTimeRepository.GetCurrentDateTimeAsync();
        }

        public async Task<DateTimeInfo> GetUtcDateTimeAsync()
        {
            return await _dateTimeRepository.GetUtcDateTimeAsync();
        }
    }
}
