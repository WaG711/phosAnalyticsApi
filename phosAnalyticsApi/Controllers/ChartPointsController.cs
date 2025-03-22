using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phosAnalyticsApi.Models;

namespace phosAnalyticsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartPointsController : ControllerBase
    {
        private readonly phosAnalyticsApiContext _context;

        public ChartPointsController(phosAnalyticsApiContext context)
        {
            _context = context;
        }

        // GET: api/ChartPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChartPoint>>> GetChartPoint()
        {
            return await _context.ChartPoint.ToListAsync();
        }

        // GET: api/ChartPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChartPoint>> GetChartPoint(Guid id)
        {
            var chartPoint = await _context.ChartPoint.FindAsync(id);

            if (chartPoint == null)
            {
                return NotFound();
            }

            return chartPoint;
        }

        // PUT: api/ChartPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChartPoint(Guid id, ChartPoint chartPoint)
        {
            if (id != chartPoint.ChartPointId)
            {
                return BadRequest();
            }

            _context.Entry(chartPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChartPointExists(id))
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

        // POST: api/ChartPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChartPoint>> PostChartPoint(ChartPoint chartPoint)
        {
            _context.ChartPoint.Add(chartPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChartPoint", new { id = chartPoint.ChartPointId }, chartPoint);
        }

        // DELETE: api/ChartPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChartPoint(Guid id)
        {
            var chartPoint = await _context.ChartPoint.FindAsync(id);
            if (chartPoint == null)
            {
                return NotFound();
            }

            _context.ChartPoint.Remove(chartPoint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChartPointExists(Guid id)
        {
            return _context.ChartPoint.Any(e => e.ChartPointId == id);
        }
    }
}
