using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.Neighbours
{
    public class NeighboursManager : INeighboursManager
    {
        private readonly int columns;
        private readonly int neighboursCount;
        private SmoPassengerInfo[] smoPassengers;
        private Dictionary<string, List<string>> idToNeighbours;
        private readonly Random random;

        public NeighboursManager()
        {
            random = new Random();
        }

        public void SetAllNeighbours(SmoPassengerInfo[] allPassengers, int columns, int neighboursCount)
        {
            idToNeighbours = allPassengers.ToDictionary(x => x.Id, x => new List<string>());

            for (var i = 0; i < allPassengers.Length; i++)
            {
                var passenger = allPassengers[i];
                passenger.Neighbours = GetNeighbours(passenger.Id, i, columns, allPassengers);
            }
        }

        public string[] GetNeighboursFor(string agentId, int agentPosition)
        {
            var freeNeighbours = idToNeighbours
                .Where(pair => pair.Value.Count <= neighboursCount)
                .ToList();

            var allCurrentNeighbours = idToNeighbours[agentId];

            SetTopologyNeigbours(agentId, agentPosition, allCurrentNeighbours);

            freeNeighbours = freeNeighbours
                .Where(pair => pair.Value.Count <= neighboursCount)
                .Where(pair => !allCurrentNeighbours.Contains(pair.Key))
                .ToList();

            var randomNeighbours = new List<string>();

            for (var i = 0; i < neighboursCount - allCurrentNeighbours.Count; i++)
            {
                if (freeNeighbours.Count == 0)
                    continue;
                var number = random.Next(0, freeNeighbours.Count - 1);
                var otherNeighbour = smoPassengers[number].Id;
                randomNeighbours.Add(otherNeighbour);
                idToNeighbours[otherNeighbour].Add(agentId);
                freeNeighbours.RemoveAt(number);
            }

            return allCurrentNeighbours.Concat(randomNeighbours).ToArray();
        }

        private string[] GetNeighbours(string passengerId, int agentPosition, int columns, SmoPassengerInfo[] passengers)
        {
            var neighbours = new List<string>();
            if (agentPosition - 1 > 0)
                neighbours.Add(passengers[agentPosition - 1].Id);

            if (agentPosition + 1 < passengers.Length)
                neighbours.Add(passengers[agentPosition + 1].Id);

            if (agentPosition - columns > 0)
                neighbours.Add(passengers[agentPosition - columns].Id);

            if (agentPosition + columns < passengers.Length)
                neighbours.Add(passengers[agentPosition + columns].Id);

            return neighbours.ToArray();
        }

        private void SetTopologyNeigbours(string agentId, int agentPosition, List<string> allCurrentNeighbours)
        {
            if (agentPosition - 1 > 0)
            {
                var leftNeighbourId = smoPassengers[agentPosition - 1].Id;
                TryAddNeighbour(agentId, allCurrentNeighbours, leftNeighbourId);
            }

            if (agentPosition + 1 < smoPassengers.Length)
            {
                var rigthNeigbour = smoPassengers[agentPosition + 1].Id;
                TryAddNeighbour(agentId, allCurrentNeighbours, rigthNeigbour);
            }

            if (agentPosition - columns > 0)
            {
                var upNeighbour = smoPassengers[agentPosition - columns].Id;
                TryAddNeighbour(agentId, allCurrentNeighbours, upNeighbour);
            }

            if (agentPosition + columns < smoPassengers.Length)
            {
                var downNeighbour = smoPassengers[agentPosition + columns].Id;
                TryAddNeighbour(agentId, allCurrentNeighbours, downNeighbour);
            }
        }

        private void TryAddNeighbour(string agentId, List<string> neighbours, string leftNeighbourId)
        {
            if (!neighbours.Contains(leftNeighbourId))
            {
                neighbours.Add(leftNeighbourId);
                idToNeighbours[leftNeighbourId].Add(agentId);
            }
        }
    }
}