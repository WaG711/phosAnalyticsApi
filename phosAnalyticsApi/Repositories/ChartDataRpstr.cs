using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.Repositories
{
    public class ChartDataRpstr : IChartDataRpstr
    {
        private readonly phosAnalyticsApiContext _context;

        public ChartDataRpstr(phosAnalyticsApiContext context)
        {
            _context = context;
        }

        public async Task<ChartData> GetChartDataByCategoryIdAndPeriod(Guid categoryId, string dateRange)
        {
            return await _context.ChartData.FirstOrDefaultAsync(cD => cD.CategoryId == categoryId);
        }

        public async Task<List<ChartData>> GetChartDatas()
        {
            return await _context.ChartData.Include(cD => cD.Points).ToListAsync();
        }
    }
}
