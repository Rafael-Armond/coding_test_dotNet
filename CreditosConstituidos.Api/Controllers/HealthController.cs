using CreditosConstituidos.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CreditosConstituidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly CreditoDbContext _context;

        public HealthController(CreditoDbContext context)
        {
            _context = context;
        }

        [HttpGet("self")]
        public IActionResult Self() => Ok(new {status = "Ok"});

        [HttpGet("/ready")]
        public async Task<IActionResult> Ready(CancellationToken ct)
        {
            var canConnect = await _context.Database.CanConnectAsync(ct);

            if (!canConnect)
                return StatusCode(503, new { status = "unavailable" });

            return Ok(new { status = "ready" });
        }
    }
}
