using System.Device.Gpio;

namespace Proculite.GpioRest.Services
{
    public class GpioService
    {
        public readonly ILogger<GpioService> _logger;
        private readonly int[] _pins;
        private readonly GpioController _gpioController;

        public GpioService(ILogger<GpioService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _pins = configuration
                .GetSection("Pins")
                .GetChildren()
                .Select(pin => pin.Get<int>())
                .ToArray();

            _gpioController = new GpioController();
            SetupPins();
        }

        private void SetupPins()
        {
            _logger.LogInformation("Setting up {PinCount} pins", _pins.Length);
            PinMode pinMode = PinMode.Output;

            foreach (var pin in _pins)
            {
                if (_gpioController.IsPinOpen(pin))
                {
                    _logger.LogWarning("Pin number {Pin} is already open.", pin);
                    continue;
                }

                if (!_gpioController.IsPinModeSupported(pin, pinMode))
                {
                    _logger.LogWarning("Pin number {Pin} does not support {PinMode}", pin, pinMode);
                }

                _gpioController.OpenPin(pin, pinMode, PinValue.Low);
            }
        }

        public PinValue CurrentPinValue(int pinNumber)
        {
            return _gpioController.Read(pinNumber);
        }
    }
}
