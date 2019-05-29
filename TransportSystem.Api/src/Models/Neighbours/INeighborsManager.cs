using System.Collections.Generic;
using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.Neighbours
{
    public interface INeighborsManager
    {
        void SetGeometricNeighbors(PassengerDto[] allPassengers, int columns);

        Dictionary<string, List<string>> SetEachPassengerNeighbors(int neighborsCount, int columns, PassengerDto[] allPassengers);
    }
}