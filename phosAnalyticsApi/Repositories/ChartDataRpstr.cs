using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.DTOs;
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
                .OrderBy(cD => cD.Title)
                .Select(cD => new ChartData
                {
                    ChartDataId = cD.ChartDataId,
                    Title = cD.Title,
                    CategoryId = cD.CategoryId,
                    Points = cD.Points
                        .Where(p => p.Date.Date >= startDate && p.Date.Date <= endDate)
                        .OrderBy(p => p.Date)
                        .ToList(),
                    Description = cD.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<PieChartDTO>> GetPieChartDatas(List<Guid> categoryIds, DateTime firstDayOfPreviousMonth, DateTime lastDayOfPreviousMonth)
        {
            return await _context.ChartData
                .Include(c => c.Points)
                .Where(c => categoryIds.Contains(c.CategoryId))
                .Select(c => new PieChartDTO
                {
                    Title = c.Title,
                    Value = c.Points
                        .Where(p => p.Date.Date >= firstDayOfPreviousMonth && p.Date.Date <= lastDayOfPreviousMonth)
                        .Sum(p => p.Value)
                })
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<List<ChartData>> GetChartDatas(DateTime date)
        {
            DateTime endDate = date.AddDays(-1).Date;
            DateTime startDate = endDate.AddDays(-6);

            return await _context.ChartData
                .Include(cD => cD.Points)
                .Where(cD => cD.Points.Any(p => p.Date.Date >= startDate && p.Date.Date <= endDate))
                .OrderBy(cD => cD.Title)
                .Select(cD => new ChartData
                {
                    ChartDataId = cD.ChartDataId,
                    Title = cD.Title,
                    CategoryId = cD.CategoryId,
                    Points = cD.Points
                        .Where(p => p.Date.Date >= startDate && p.Date.Date <= endDate)
                        .OrderBy(p => p.Date)
                        .ToList(),
                    Description = cD.Description
                })
                .ToListAsync();
        }
    }
}
