using System.Device.Gpio;
using System.Text.Json.Serialization;

namespace Proculite.GpioRest.Models
{
    public class PinValueModel(int pinNumber, double value)
    {
        public int PinNumber { get; set; } = pinNumber;

        public double PinValue { get; set; } = value;
    }
}
