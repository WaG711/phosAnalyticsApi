using Microsoft.AspNetCore.Mvc;
using phosAnalyticsApi.DTOs;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Models;
using phosAnalyticsApi.Services;
using System.Globalization;

namespace phosAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartDatasController : ControllerBase
    {
        private readonly IChartDataRpstr _rpstr;
        private readonly ChartDataForecastService _forecastService;

        public ChartDatasController(IChartDataRpstr rpstr, ChartDataForecastService forecastService)
        {
            _rpstr = rpstr;
            _forecastService = forecastService;
        }

        [HttpGet("bars")]
        public async Task<ActionResult<IEnumerable<ChartDataDTO>>> GetChartData()
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

        [HttpGet("pies")]
        public async Task<ActionResult<IEnumerable<PieChartDTO>>> GetPieCharts()
        {
            DateTime requestDate = DateTime.UtcNow.Date;
            var firstDayOfPreviousMonth = new DateTime(requestDate.Year, requestDate.Month, 1).AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);

            var categoryIds = new List<Guid>
            {
                Guid.Parse("894f82d3-9bb5-43d8-b502-52181c52ea4e"),
                Guid.Parse("294dec00-eb26-4943-80b7-9763eb163686"),
                Guid.Parse("030db36a-5614-4040-9191-2be35c70eef1"),
                Guid.Parse("3ed80b09-c9a0-4a21-88b5-63fa521fb832")
            };

            var pieChartDTO = await _rpstr.GetPieChartDatas(categoryIds, firstDayOfPreviousMonth, lastDayOfPreviousMonth);

            return Ok(pieChartDTO);
        }

        [HttpGet("forecast/{categoryId}")]
        public async Task<ActionResult<List<ChartDataDTO>>> GetChartDataForecast(Guid categoryId)
        {
            DateTime requestDate = DateTime.UtcNow.Date;
            var forecastData = await _rpstr.GetChartDataByCategoryId(categoryId, requestDate);
            var forecastPoints = _forecastService.SimpleForecast(forecastData);

            var endDate = requestDate.AddDays(30);

            var groupedForecast = AggregateDataByDynamicGroups(forecastPoints, requestDate, endDate);

            var chartDataDTO = new ChartDataDTO
            {
                CategoryId = forecastData.CategoryId.ToString(),
                Title = forecastData.Title,
                Description = forecastData.Description,
                Points = groupedForecast
            };

            return Ok(chartDataDTO);
        }

        [HttpGet("{categoryId}&{period}")]
        public async Task<ActionResult<ChartDataDTO>> GetChartData(Guid categoryId, string period)
        {
            if (!TryParsePeriod(period, out DateTime startDate, out DateTime endDate, out var errorResult))
            {
                return errorResult;
            }

            var data = await _rpstr.GetChartDataByCategoryIdAndDateRange(categoryId, startDate, endDate);
            if (data == null) return NotFound("Данные не найдены");

            var aggregatedPoints = AggregateDataByDynamicGroups(data.Points, startDate, endDate);

            var chartDataDTO = new ChartDataDTO
            {
                CategoryId = data.CategoryId.ToString(),
                Title = data.Title,
                Description = data.Description,
                Points = aggregatedPoints
            };

            return Ok(chartDataDTO);
        }

        private bool TryParsePeriod(string period, out DateTime startDate, out DateTime endDate, out ActionResult errorResult)
        {
            var dates = period.Split('-');
            if (dates.Length != 2 ||
                !DateTime.TryParseExact(dates[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) ||
                !DateTime.TryParseExact(dates[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                errorResult = BadRequest("Используйте формат 'dd.MM.yyyy-dd.MM.yyyy'");
                startDate = default;
                endDate = default;
                return false;
            }

            errorResult = null;
            return true;
        }

        private List<ChartPointDTO> AggregateDataByDynamicGroups(List<ChartPoint> points, DateTime startDate, DateTime endDate)
        {
            var totalDays = (endDate - startDate).TotalDays;
            var aggregatedPoints = new List<ChartPointDTO>();

            int groupSize = CalculateGroupSize(totalDays);

            for (var date = startDate; date <= endDate; date = date.AddDays(groupSize))
            {
                var chunkEnd = date.AddDays(groupSize - 1) > endDate ? endDate : date.AddDays(groupSize - 1);
                var groupPoints = GetPointsInRange(points, date, chunkEnd);

                aggregatedPoints.Add(CreateAggregatedPoint(groupPoints, date, chunkEnd, groupSize));
            }

            return aggregatedPoints;
        }

        private int CalculateGroupSize(double totalDays)
        {
            return totalDays switch
            {
                <= 14 => 1,
                <= 30 => 2,
                <= 60 => 7,
                <= 90 => 14,
                <= 180 => 14,
                _ => 30
            };
        }

        private List<ChartPoint> GetPointsInRange(List<ChartPoint> points, DateTime start, DateTime end)
        {
            return points.Where(p => p.Date >= start && p.Date <= end).ToList();
        }

        private ChartPointDTO CreateAggregatedPoint(List<ChartPoint> groupPoints, DateTime start, DateTime end, int groupSize)
        {
            double avg = groupPoints.Any() ? groupPoints.Average(p => p.Value) : 0;
            string label = groupSize > 1
                ? $"{start:dd.MM}\n{end:dd.MM}"
                : $"{start:dd.MM}";

            return new ChartPointDTO
            {
                Date = label,
                Value = avg
            };
        }
    }
}
