namespace Proculite.GpioRest.Services
{
    public class GpioService
    {
        public readonly ILogger<GpioService> _logger;
        public GpioService(ILogger<GpioService> logger)
        {
            _logger = logger;
        }
    }
}