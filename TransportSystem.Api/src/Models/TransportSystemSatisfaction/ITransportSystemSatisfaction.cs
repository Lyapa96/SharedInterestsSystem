using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportSystemSatisfaction
{
    public interface ITransportSystemSatisfaction
    {
        double Evaluate(PassengerDto[] passengers);
    }
}