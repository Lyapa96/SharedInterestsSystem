using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.Neighbors
{
    public class NeighborsManager : INeighborsManager
    {
        private readonly IRandomizer randomizer;

        public NeighborsManager(IRandomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public Dictionary<string, List<string>> GetGeometricNeighborhood(PassengerDto[] allPassengers, int columns)
        {
            return allPassengers
                .Select((passenger, index) => (id: passenger.Id, neighbors: GetNeighbors(index, columns, allPassengers).ToList()))
                .ToDictionary(x => x.id, x => x.neighbors);
        }

        public Dictionary<string, List<string>> GetEachPassengerNeighbors(int neighborsCount, int columns, PassengerDto[] allPassengers)
        {
            var neighborhoodInformation = GetGeometricNeighborhood(allPassengers, columns);

            foreach (var passengerId in allPassengers.Select(passenger => passenger.Id))
            {
                var appropriateCandidates = GetAppropriateCandidates(passengerId, neighborhoodInformation, neighborsCount).ToList();
                var needCountPassengers = neighborsCount - neighborhoodInformation[passengerId].Count;
                var randomNeighborsIds = GetRandomNeighbors(appropriateCandidates, needCountPassengers);
                foreach (var randomNeighborId in randomNeighborsIds)
                {
                    neighborhoodInformation[randomNeighborId].Add(passengerId);
                    neighborhoodInformation[passengerId].Add(randomNeighborId);
                }
            }

            return neighborhoodInformation;
        }

        private List<string> GetRandomNeighbors(List<string> appropriateCandidates, int needCountPassengers)
        {
            var randomNeighbors = new List<string>();
            for (var i = 0; i < needCountPassengers; i++)
            {
                if (appropriateCandidates.Count == 0)
                    return randomNeighbors;
                var randomPassengerIndex = randomizer.GetRandomNumber(0, appropriateCandidates.Count - 1);
                var randomPassengerId = appropriateCandidates[randomPassengerIndex];

                randomNeighbors.Add(randomPassengerId);
                appropriateCandidates.RemoveAt(randomPassengerIndex);
            }

            return randomNeighbors;
        }

        private IEnumerable<string> GetAppropriateCandidates(
            string passengerId,
            Dictionary<string, List<string>> neighborhood,
            int neighborsCount)
        {
            return neighborhood
                .Select(x => x.Key)
                .Where(otherPassengerId => otherPassengerId != passengerId)
                .Where(id => !neighborhood[passengerId].Contains(id))
                .Where(id => neighborhood[id].Count < neighborsCount);
        }

        private string[] GetNeighbors(int agentPosition, int columns, PassengerDto[] passengers)
        {
            var neighbors = new List<string>();
            if (agentPosition - 1 >= 0 && (agentPosition) % columns != 0)
                neighbors.Add(passengers[agentPosition - 1].Id);

            if (agentPosition + 1 < passengers.Length && (agentPosition + 1)%columns != 0)
                neighbors.Add(passengers[agentPosition + 1].Id);

            if (agentPosition - columns >= 0)
                neighbors.Add(passengers[agentPosition - columns].Id);

            if (agentPosition + columns < passengers.Length)
                neighbors.Add(passengers[agentPosition + columns].Id);

            return neighbors.ToArray();
        }
    }
}