using System.Device.Gpio;
using System.Text.Json.Serialization;

namespace Proculite.GpioRest.Models
{
    public class PinValueModel(int pinNumber, PinValue pinValue)
    {
        public int PinNumber { get; set; } = pinNumber;

        [JsonIgnore]
        public PinValue PinValue { get; set; } = pinValue;
        public int Value => PinValue == PinValue.High ? 1 : 0;
    }
}
