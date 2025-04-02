using Microsoft.AspNetCore.Mvc;
using phosAnalyticsApi.DTOs;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Models;
using System.Globalization;

namespace phosAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartDatasController : ControllerBase
    {
        private readonly IChartDataRpstr _rpstr;

        public ChartDatasController(IChartDataRpstr rpstr)
        {
            _rpstr = rpstr;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChartData>>> GetChartData()
        {
            DateTime requestDate = DateTime.UtcNow.Date;

            var chartDatas = await _rpstr.GetChartDatas(requestDate);

            var chartDatasDTO = chartDatas.Select(chartData => new ChartDataDTO
            {
                CategoryId = chartData.CategoryId.ToString(),
                Title = chartData.Title,
                Points = chartData.Points.Select(point => new ChartPointDTO
                {
                    Date = point.Date.ToString("dd"),
                    Value = point.Value
                }).ToList()
            }).ToList();

            return Ok(chartDatasDTO);
        }

        [HttpGet("{categoryId}&{period}")]
        public async Task<ActionResult<ChartData>> GetChartData(Guid categoryId, string period)
        {
            var dates = period.Split('-');
            if (dates.Length != 2 ||
                !DateTime.TryParseExact(dates[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate) ||
                !DateTime.TryParseExact(dates[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                return BadRequest("Используйте 'dd.MM.yyyy-dd.MM.yyyy'");
            }

            var chartData = await _rpstr.GetChartDataByCategoryIdAndDateRange(categoryId, startDate, endDate);
            if (chartData == null)
            {
                return NotFound("Данные не найдены");
            }

            var chartDataDTO = new ChartDataDTO
            {
                CategoryId = chartData.CategoryId.ToString(),
                Title = chartData.Title,
                Points = chartData.Points.Select(point => new ChartPointDTO
                {
                    Date = point.Date.ToString("yyyy-MM-dd"),
                    Value = point.Value
                }).ToList(),
                Description = chartData.Description,
            };

            return Ok(chartData);
        }
    }
}
