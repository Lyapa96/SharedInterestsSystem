using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.Neighbours
{
    public interface INeighboursManager
    {
        void SetAllNeighbours(SmoPassengerInfo[] allPassengers, int columns, int neighboursCount);
    }
}