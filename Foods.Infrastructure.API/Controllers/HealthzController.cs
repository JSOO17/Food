using Microsoft.AspNetCore.Mvc;

namespace Foods.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthzController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
