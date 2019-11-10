using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;

namespace TransportSystem.Api.Utilities
{
    public static class TransportTypes
    {
        public static TransportType GetRandomTransportWithoutType(TransportType transportType, IRandomizer randomizer, TransportType[] availableTransportTypes)
        {
            var types = availableTransportTypes.Where(t => t != transportType).ToArray();
            var randomNumber = randomizer.GetRandomNumber(0, types.Length);
            var type = types[randomNumber];

            return type;
        }

        public static TransportType GetRandomTransportTypeBetweenCarAndBus(IRandomizer randomizer)
        {
            var types = new[] {TransportType.Bus, TransportType.Car};
            var index = randomizer.GetRandomNumber(0, types.Length);
            return types[index];
        }

        public static TransportType GetRandomTransportType(IRandomizer randomizer, TransportType[] availableTransportTypes)
        {
            var index = randomizer.GetRandomNumber(0, availableTransportTypes.Length);
            return availableTransportTypes[index];
        }
    }
}