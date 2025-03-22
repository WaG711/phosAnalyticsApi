namespace phosAnalyticsApi.Models
{
    public class ChartData
    {
        public Guid ChartDataId { get; set; }
        public string Title { get; set; }
        public Guid CategoryId { get; set; }

        public List<ChartPoint> Points { get; set; } = [];
    }
}
