namespace phosAnalyticsApi.DTOs
{
    public class ChartDataDTO
    {
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public List<ChartPointDTO> Points { get; set; } = [];
    }
}
