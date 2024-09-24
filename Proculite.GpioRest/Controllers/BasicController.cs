using Microsoft.AspNetCore.Mvc;

namespace Proculite.GpioRest.Controllers
{
    public class BasicController : Controller
    {
        [HttpGet("/ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}
