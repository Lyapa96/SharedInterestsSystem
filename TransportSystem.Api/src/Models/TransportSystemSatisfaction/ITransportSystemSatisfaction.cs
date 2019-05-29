using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.TransportSystemSatisfaction
{
    public interface ITransportSystemSatisfaction
    {
        double Evaluate(PassengerDto[] passengers);
    }
}