using System.Collections.Generic;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.Neighbours
{
    public interface INeighborsManager
    {
        Dictionary<string, List<string>> GetGeometricNeighborhood(PassengerDto[] allPassengers, int columns);
        Dictionary<string, List<string>> GetEachPassengerNeighbors(int neighborsCount, int columns, PassengerDto[] allPassengers);
    }
}