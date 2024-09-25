using System.Device.Gpio;
using Proculite.GpioRest.Models;

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
            _logger.LogInformation("Setting up {PinCount} pins.", _pins.Length);
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

        public PinValueModel PinValueModelOfPin(int pinNumber)
        {
            return new PinValueModel(pinNumber, _gpioController.Read(pinNumber));
        }

        public PinValueModel[] StateOfAllPins()
        {
            return _pins.Select(PinValueModelOfPin).ToArray();
        }

        public void SetPinHigh(int pinNumber)
        {
            _gpioController.Write(pinNumber, PinValue.High);
        }

        public void SetPinLow(int pinNumber)
        {
            _gpioController.Write(pinNumber, PinValue.Low);
        }

        public void TogglePin(int pinNumber)
        {
            PinValue currentValue = CurrentPinValue(pinNumber);
            PinValue newValue = currentValue == PinValue.High ? PinValue.Low : PinValue.High;
            _gpioController.Write(pinNumber, newValue);
        }

        public PinValueModel SetPinHighReturning(int pinNumber)
        {
            SetPinHigh(pinNumber);
            return PinValueModelOfPin(pinNumber);
        }

        public PinValueModel SetPinLowReturning(int pinNumber)
        {
            SetPinLow(pinNumber);
            return PinValueModelOfPin(pinNumber);
        }

        public PinValueModel TogglePinReturning(int pinNumber)
        {
            TogglePin(pinNumber);
            return PinValueModelOfPin(pinNumber);
        }
    }
}
