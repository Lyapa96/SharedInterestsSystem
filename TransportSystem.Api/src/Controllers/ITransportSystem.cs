using TransportSystem.Api.Models;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Controllers
{
    public interface ITransportSystem
    {
        void MakeIteration(Passenger[] passengers);
    }
}