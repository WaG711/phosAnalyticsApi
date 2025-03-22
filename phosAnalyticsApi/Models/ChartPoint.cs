namespace phosAnalyticsApi.Models
{
    public class ChartPoint
    {
        public Guid ChartPointId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
