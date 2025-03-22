using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.IRepositories;
using phosAnalyticsApi.Models;
using phosAnalyticsApi.Repositories;

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
            return Ok(chartDatas);
        }

        [HttpGet("{categoryId}&{period}")]
        public async Task<ActionResult<ChartData>> GetChartData(Guid categoryId, string period)
        {
            var chratData = await _rpstr.GetChartDataByCategoryIdAndPeriod(categoryId, period);
            return Ok(chratData);
        }
    }
}
