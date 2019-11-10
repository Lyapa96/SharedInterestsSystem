using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.System
{
    public class TransportSystem : ITransportSystem
    {
        public void MakeIteration(Passenger[] passengers, TransportType[] availableTransportTypes)
        {
            foreach (var passenger in passengers)
            {
                passenger.ChooseNextTransportType(availableTransportTypes);
            }

            QualityTransportManager.ChangeQuality(passengers);

            foreach (var passenger in passengers)
            {
                passenger.UpdateSatisfaction();
            }
        }
    }
}