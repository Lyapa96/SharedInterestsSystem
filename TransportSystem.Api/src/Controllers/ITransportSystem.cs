using TransportSystem.Api.Models;

namespace TransportSystem.Api.Controllers
{
    public interface ITransportSystem
    {
        void MakeIteration(Passenger[] passengers);
    }
}