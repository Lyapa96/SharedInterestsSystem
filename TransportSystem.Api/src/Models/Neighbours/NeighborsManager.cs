using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.Neighbours
{
    public class NeighborsManager : INeighborsManager
    {
        private readonly Random random;

        public NeighborsManager()
        {
            random = new Random();
        }

        public void SetGeometricNeighbors(PassengerDto[] allPassengers, int columns)
        {
            for (var index = 0; index < allPassengers.Length; index++)
            {
                allPassengers[index].Neighbours = GetNeighbors(index, columns, allPassengers);
            }
        }

        public Dictionary<string, List<string>> SetGeometricNeighbors2(PassengerDto[] allPassengers, int columns)
        {
            return allPassengers
                .Select((passenger, index) => (id: passenger.Id, neighbors: GetNeighbors(index, columns, allPassengers).ToList()))
                .ToDictionary(x => x.id, x => x.neighbors);
        }

        public Dictionary<string, List<string>> SetEachPassengerNeighbors(int neighborsCount, int columns, PassengerDto[] allPassengers)
        {
            SetGeometricNeighbors(allPassengers, columns);

            var neighborhoodInformation = allPassengers.ToDictionary(x => x.Id, x => x.Neighbours.ToList());

            foreach (var passenger in allPassengers)
            {
                var appropriateCandidates = GetAppropriateCandidates(passenger, allPassengers).ToList();
                var needCountPassengers = neighborsCount - neighborhoodInformation[passenger.Id].Count;
                var randomNeighbors = GetRandomNeighbors(appropriateCandidates, needCountPassengers);
                foreach (var randomNeighbor in randomNeighbors)
                {
                    neighborhoodInformation[randomNeighbor].Add(passenger.Id);
                    neighborhoodInformation[passenger.Id].Add(randomNeighbor);
                }
            }

            return neighborhoodInformation;
        }

        private List<string> GetRandomNeighbors(List<PassengerDto> appropriateCandidates, int needCountPassengers)
        {
            var randomNeighbors = new List<string>();
            for (var i = 0; i < needCountPassengers; i++)
            {
                if (appropriateCandidates.Count == 0)
                    return randomNeighbors;
                var randomPassengerIndex = random.Next(0, appropriateCandidates.Count - 1);
                var randomPassenger = appropriateCandidates[randomPassengerIndex];

                randomNeighbors.Add(randomPassenger.Id);
                appropriateCandidates.RemoveAt(randomPassengerIndex);
            }

            return randomNeighbors;
        }

        private IEnumerable<PassengerDto> GetAppropriateCandidates(PassengerDto passenger, PassengerDto[] allPassenger)
        {
            return allPassenger.Where(x => x.Id != passenger.Id && !x.Neighbours.Contains(x.Id));
        }

        private string[] GetNeighbors(int agentPosition, int columns, PassengerDto[] passengers)
        {
            var neighbors = new List<string>();
            if (agentPosition - 1 > 0)
                neighbors.Add(passengers[agentPosition - 1].Id);

            if (agentPosition + 1 < passengers.Length)
                neighbors.Add(passengers[agentPosition + 1].Id);

            if (agentPosition - columns > 0)
                neighbors.Add(passengers[agentPosition - columns].Id);

            if (agentPosition + columns < passengers.Length)
                neighbors.Add(passengers[agentPosition + columns].Id);

            return neighbors.ToArray();
        }
    }
}