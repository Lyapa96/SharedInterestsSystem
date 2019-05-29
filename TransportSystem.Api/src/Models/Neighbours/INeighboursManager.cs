using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.Neighbours
{
    public interface INeighboursManager
    {
        void SetAllNeighbours(PassengerDto[] allPassengers, int columns, int neighboursCount);
    }
}