using phosAnalyticsApi.DTOs;
using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.IRepositories
{
    public interface IChartDataRpstr
    {
        Task<List<ChartData>> GetChartDatas(DateTime date);
        Task<ChartData> GetChartDataByCategoryIdAndDateRange(Guid categoryId, DateTime startDate, DateTime endDate);
        Task<List<PieChartDTO>> GetPieChartDatas(List<Guid> categoryIds, DateTime firstDayOfPreviousMonth, DateTime lastDayOfPreviousMonth);
    }
}
