using TransportSystem.Api.Models;

namespace TransportSystem.Api.Controllers
{
    public class TransportSystem : ITransportSystem
    {
        public void MakeIteration(Passenger[] passengers)
        {
            foreach (var passenger in passengers)
                passenger.ChooseNextTransportType();

            QualityTransportManager.ChangeQuality(passengers);

            foreach (var passenger in passengers)
                passenger.UpdateSatisfaction();
        }
    }
}