using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models
{
    public interface INeighboursManager
    {
        void SetAllNeighbours(SmoPassengerInfo[] allPassengers, int columns, int neighboursCount);
    }
}