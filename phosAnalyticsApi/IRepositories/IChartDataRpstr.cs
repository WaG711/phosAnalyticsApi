using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.IRepositories
{
    public interface IChartDataRpstr
    {
        Task<List<ChartData>> GetChartDatas();
        Task<ChartData> GetChartDataByCategoryIdAndPeriod(Guid categoryId, string dateRange);
    }
}
