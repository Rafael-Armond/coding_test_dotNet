using Microsoft.AspNetCore.Mvc;

namespace CreditosConstituidos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        public HealthController ()
        { }

        [HttpGet("self")]
        public IActionResult Self() => Ok(new {status = "Ok"});
    }
}
