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

        public PinModel PinModelOfPin(int pinNumber)
        {
            PwmWrapper pwmWrapper = _pwmPins[pinNumber];
            return new PinModel(pinNumber, pwmWrapper.Value, pwmWrapper.Frequency);
        }

        public PinModel[] StateOfAllPins()
        {
            return _pins.Select(PinModelOfPin).ToArray();
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

        public void SetPinFrequency(int pinNumber, int pinFrequency)
        {
            _pwmPins[pinNumber].Frequency = pinFrequency;
        }

        public PinModel SetPinHighReturning(int pinNumber)
        {
            SetPinHigh(pinNumber);
            return PinModelOfPin(pinNumber);
        }

        public PinModel SetPinLowReturning(int pinNumber)
        {
            SetPinLow(pinNumber);
            return PinModelOfPin(pinNumber);
        }

        public PinModel TogglePinReturning(int pinNumber)
        {
            TogglePin(pinNumber);
            return PinModelOfPin(pinNumber);
        }

        public PinModel SetPinReturning(int pinNumber, double pinValue)
        {
            SetPin(pinNumber, pinValue);
            return PinModelOfPin(pinNumber);
        }

        public PinModel SetPinFrequencyReturning(int pinNumber, int pinFrequency)
        {
            _pwmPins[pinNumber].Frequency = pinFrequency;
            return PinModelOfPin(pinNumber);
        }
    }
}
