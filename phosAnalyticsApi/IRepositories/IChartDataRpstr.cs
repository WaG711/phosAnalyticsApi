using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.IRepositories
{
    public interface IChartDataRpstr
    {
        Task<List<ChartData>> GetChartDatas(DateTime date);
        Task<ChartData> GetChartDataByCategoryIdAndDateRange(Guid categoryId, DateTime startDate, DateTime endDate);
    }
}
