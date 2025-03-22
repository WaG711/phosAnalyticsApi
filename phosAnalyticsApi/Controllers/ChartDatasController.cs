using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartDatasController : ControllerBase
    {
        private readonly phosAnalyticsApiContext _context;

        public ChartDatasController(phosAnalyticsApiContext context)
        {
            _context = context;
        }

        // GET: api/ChartDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChartData>>> GetChartData()
        {
            return await _context.ChartData.ToListAsync();
        }

        // GET: api/ChartDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChartData>> GetChartData(Guid id)
        {
            var chartData = await _context.ChartData.FindAsync(id);

            if (chartData == null)
            {
                return NotFound();
            }

            return chartData;
        }

        // PUT: api/ChartDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChartData(Guid id, ChartData chartData)
        {
            if (id != chartData.ChartDataId)
            {
                return BadRequest();
            }

            _context.Entry(chartData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChartDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChartDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChartData>> PostChartData(ChartData chartData)
        {
            _context.ChartData.Add(chartData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChartData", new { id = chartData.ChartDataId }, chartData);
        }

        // DELETE: api/ChartDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChartData(Guid id)
        {
            var chartData = await _context.ChartData.FindAsync(id);
            if (chartData == null)
            {
                return NotFound();
            }

            _context.ChartData.Remove(chartData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChartDataExists(Guid id)
        {
            return _context.ChartData.Any(e => e.ChartDataId == id);
        }
    }
}
