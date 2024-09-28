using System.Device.Pwm.Drivers;

namespace Proculite.GpioRest.Wrappers
{
    public class PwmWrapper
    {
        private readonly SoftwarePwmChannel _softwarePwmChannel;
        private readonly ILogger? _logger;
        public int PinNumber { get; }
        public double Value
        {
            get => _softwarePwmChannel.DutyCycle;
            set { _softwarePwmChannel.DutyCycle = value; }
        }

        public int Frequency
        {
            get => _softwarePwmChannel.Frequency;
            set => _softwarePwmChannel.Frequency = value;
        }

        public PwmWrapper(int pinNumber, ILogger? logger = null)
        {
            PinNumber = pinNumber;
            _softwarePwmChannel = new SoftwarePwmChannel(pinNumber, usePrecisionTimer: true);
            _logger = logger;
            _softwarePwmChannel.DutyCycle = 0;
            _softwarePwmChannel.Start();
        }

        public void SetValue(double value)
        {
            LogValue(value);
            if (value < 0 || value > 1)
            {
                LogValueOutOfBounds();
                return;
            }

            _softwarePwmChannel.DutyCycle = value;
        }

        public void SetHigh()
        {
            SetValue(1);
        }

        public void SetLow()
        {
            SetValue(0);
        }

        private void LogValue(double value)
        {
            if (_logger is null)
                return;
            _logger.LogDebug("New value for pin {PinNumber}: {Value}", PinNumber, value);
        }

        private void LogValueOutOfBounds()
        {
            if (_logger is null)
                return;
            _logger.LogWarning(
                "Attempting to set value of pin {PinNumber}, but it's not between 0 and 1.",
                PinNumber
            );
        }
    }
}
