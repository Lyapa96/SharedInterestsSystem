using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.System
{
    public interface ITransportSystem
    {
        void MakeIteration(Passenger[] passengers);
    }
}