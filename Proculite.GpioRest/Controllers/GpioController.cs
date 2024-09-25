using Microsoft.AspNetCore.Mvc;
using Proculite.GpioRest.Services;

namespace Proculite.GpioRest.Controllers
{
    [Route("/gpio")]
    public class GpioController : Controller
    {
        private readonly GpioService _gpioService;

        public GpioController(GpioService gpioService)
        {
            _gpioService = gpioService;
        }

        [HttpGet("pin-value/{pinNumber}")]
        public IActionResult PinValue(int pinNumber)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_gpioService.PinValueModelOfPin(pinNumber));
        }

        [HttpGet("pin-value")]
        public IActionResult AllPinValues()
        {
            return Ok(_gpioService.StateOfAllPins());
        }
    }
}
