using System.Device.Gpio;
using System.Text.Json.Serialization;

namespace Proculite.GpioRest.Models
{
    public class PinModel(int number, double value)
    {
        public int Number { get; set; } = number;

        public double Value { get; set; } = value;
    }
}
