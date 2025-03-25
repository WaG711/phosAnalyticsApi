using Microsoft.AspNetCore.Mvc;
using phosAnalyticsApi.DTOs;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Models;

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
            var chartDatas = await _rpstr.GetChartDatas();

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
            var chartData = await _rpstr.GetChartDataByCategoryIdAndPeriod(categoryId, period);

            var chartDataDTO = new ChartDataDTO
            {
                CategoryId = chartData.CategoryId.ToString(),
                Title = chartData.Title,
                Points = chartData.Points.Select(point => new ChartPointDTO
                {
                    Date = point.Date.ToString("yyyy-MM-dd"),
                    Value = point.Value
                }).ToList()
            };

            return Ok(chartData);
        }
    }
}
