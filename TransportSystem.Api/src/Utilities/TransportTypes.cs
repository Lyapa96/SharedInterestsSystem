using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;

namespace TransportSystem.Api.Utilities
{
    public static class TransportTypes
    {
        public static TransportType[] AllTransportTypes =
            {TransportType.Bus, TransportType.Car, TransportType.Subway, TransportType.Bike, TransportType.Tram};

        public static TransportType GetRandomTransportWithoutType(TransportType transportType, IRandomizer randomizer)
        {
            var types = AllTransportTypes.Where(t => t != transportType).ToArray();
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
    }
}