using TransportSystem.Api.Models;
using TransportSystem.Api.Models.Data;

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