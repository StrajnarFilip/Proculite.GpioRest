using System.Device.Gpio;
using Proculite.GpioRest.Models;
using Proculite.GpioRest.Wrappers;

namespace Proculite.GpioRest.Services
{
    public class GpioService
    {
        public readonly ILogger<GpioService> _logger;
        private readonly int[] _pins;
        private readonly Dictionary<int, PwmWrapper> _pwmPins = new();

        public GpioService(ILogger<GpioService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _pins = configuration
                .GetSection("Pins")
                .GetChildren()
                .Select(pin => pin.Get<int>())
                .ToArray();

            SetupPins();
        }

        private void SetupPins()
        {
            _logger.LogInformation("Setting up {PinCount} pins.", _pins.Length);

            foreach (var pin in _pins)
            {
                _pwmPins[pin] = new PwmWrapper(pin);
            }
        }

        public PinValueModel CurrentPinValue(int pinNumber)
        {
            return new PinValueModel(pinNumber, _pwmPins[pinNumber].Value);
        }

        public PinValueModel PinValueModelOfPin(int pinNumber)
        {
            return new PinValueModel(pinNumber, _pwmPins[pinNumber].Value);
        }

        public PinValueModel[] StateOfAllPins()
        {
            return _pins.Select(PinValueModelOfPin).ToArray();
        }

        public void SetPinHigh(int pinNumber)
        {
            _pwmPins[pinNumber].SetHigh();
        }

        public void SetPinLow(int pinNumber)
        {
            _pwmPins[pinNumber].SetLow();
        }

        public void TogglePin(int pinNumber)
        {
            double currentValue = _pwmPins[pinNumber].Value;
            _pwmPins[pinNumber].Value = currentValue > 0 ? 0 : 1;
        }

        public void SetPin(int pinNumber, double pinValue)
        {
            _pwmPins[pinNumber].Value = pinValue;
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

        public PinValueModel SetPinReturning(int pinNumber, double pinValue)
        {
            SetPin(pinNumber, pinValue);
            return PinValueModelOfPin(pinNumber);
        }
    }
}
