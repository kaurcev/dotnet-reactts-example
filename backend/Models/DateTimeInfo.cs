namespace backend.Models
{
    public class DateTimeInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; } = "UTC";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string FormattedDate => DateTime.ToString("yyyy-MM-dd");
        public string FormattedTime => DateTime.ToString("HH:mm:ss");
        public string FormattedDateTime => DateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
