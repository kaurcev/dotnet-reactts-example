using backend.Interfaces;
using backend.Models;

namespace backend.Repositories
{
    public class DateTimeRepository : IDateTimeRepository
    {
        private readonly List<DateTimeInfo> _dateTimeEntries;

        public DateTimeRepository()
        {
            _dateTimeEntries = new List<DateTimeInfo>
            {
                new DateTimeInfo
                {
                    Id = 1,
                    Title = "Текущее время UTC",
                    Description = "Время в формате UTC",
                    DateTime = DateTime.UtcNow,
                    TimeZone = "UTC",
                },
            };
        }

        public async Task<IEnumerable<DateTimeInfo>> GetAllAsync()
        {
            return await Task.FromResult(_dateTimeEntries.AsReadOnly());
        }

        public async Task<DateTimeInfo?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dateTimeEntries.FirstOrDefault(d => d.Id == id));
        }

        public async Task<DateTimeInfo> CreateAsync(DateTimeInfo dateTimeInfo)
        {
            dateTimeInfo.Id = _dateTimeEntries.Any() ? _dateTimeEntries.Max(d => d.Id) + 1 : 1;
            dateTimeInfo.CreatedAt = DateTime.UtcNow;
            dateTimeInfo.UpdatedAt = DateTime.UtcNow;

            _dateTimeEntries.Add(dateTimeInfo);
            return await Task.FromResult(dateTimeInfo);
        }

        public async Task<DateTimeInfo?> UpdateAsync(int id, DateTimeInfo dateTimeInfo)
        {
            var existingEntry = _dateTimeEntries.FirstOrDefault(d => d.Id == id);
            if (existingEntry == null)
                return null;

            existingEntry.Title = dateTimeInfo.Title;
            existingEntry.Description = dateTimeInfo.Description;
            existingEntry.DateTime = dateTimeInfo.DateTime;
            existingEntry.TimeZone = dateTimeInfo.TimeZone;
            existingEntry.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existingEntry);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = _dateTimeEntries.FirstOrDefault(d => d.Id == id);
            if (entry == null)
                return false;

            _dateTimeEntries.Remove(entry);
            return await Task.FromResult(true);
        }

        public async Task<DateTimeInfo> GetCurrentDateTimeAsync()
        {
            return await Task.FromResult(
                new DateTimeInfo
                {
                    Id = 0,
                    Title = "Текущее время",
                    Description = "Актуальное время на момент запроса",
                    DateTime = DateTime.Now,
                    TimeZone = TimeZoneInfo.Local.DisplayName,
                }
            );
        }

        public async Task<DateTimeInfo> GetUtcDateTimeAsync()
        {
            return await Task.FromResult(
                new DateTimeInfo
                {
                    Id = 0,
                    Title = "UTC время",
                    Description = "Координированное всемирное время",
                    DateTime = DateTime.UtcNow,
                    TimeZone = "UTC",
                }
            );
        }
    }
}
