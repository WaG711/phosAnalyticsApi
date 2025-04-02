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

        public async Task<ChartData> GetChartDataByCategoryIdAndDateRange(Guid categoryId, DateTime startDate, DateTime endDate)
        {
            return await _context.ChartData
                .Include(cD => cD.Points)
                .Where(cD => cD.CategoryId == categoryId)
                .Select(cD => new ChartData
                {
                    ChartDataId = cD.ChartDataId,
                    Title = cD.Title,
                    CategoryId = cD.CategoryId,
                    Points = cD.Points
                        .Where(p => p.Date.Date >= startDate && p.Date.Date <= endDate)
                        .ToList(),
                    Description = cD.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChartData>> GetChartDatas(DateTime date)
        {
            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = date.Date;

            return await _context.ChartData
                .Include(cD => cD.Points)
                .Where(cD => cD.Points.Any(p => p.Date.Date >= startOfWeek && p.Date.Date <= endOfWeek))
                .Select(cD => new ChartData
                {
                    ChartDataId = cD.ChartDataId,
                    Title = cD.Title,
                    CategoryId = cD.CategoryId,
                    Points = cD.Points
                        .Where(p => p.Date.Date >= startOfWeek && p.Date.Date <= endOfWeek)
                        .ToList()
                })
                .ToListAsync();
                }
    }
}
