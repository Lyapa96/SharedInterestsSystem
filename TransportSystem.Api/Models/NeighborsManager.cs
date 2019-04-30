using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models
{
    public class NeighborsManager
    {
        private readonly SmoPassengerInfo[] smoPassengers;
        private readonly int columns;
        private readonly int neighboursCount;
        private Dictionary<string, List<string>> idToNeighbours;
        private Random random;

        public NeighborsManager(SmoPassengerInfo[] smoPassengers, int columns, int neighboursCount)
        {
            random = new Random();
            this.smoPassengers = smoPassengers;
            this.columns = columns;
            this.neighboursCount = neighboursCount;
            idToNeighbours = smoPassengers.ToDictionary(x => x.Id, x => new List<string>());
        }

        public string[] GetNeighboursFor(string agentId, int agentPosition)
        {
            var freeNeighbours = idToNeighbours
                .Where(pair => pair.Value.Count <= neighboursCount)
                .ToList();

            var allCurrentNeighbours = idToNeighbours[agentId];

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

            freeNeighbours = freeNeighbours
                .Where(pair => pair.Value.Count <= neighboursCount)
                .Where(pair => !allCurrentNeighbours.Contains(pair.Key))
                .ToList();

            var randomNeighbours = new List<string>();
            
            for (var i = 0; i < neighboursCount - allCurrentNeighbours.Count; i++)
            {
                if (freeNeighbours.Count == 0)
                    continue;
                var number = random.Next(0, freeNeighbours.Count -1);
                var otherNeighbour = smoPassengers[number].Id;
                randomNeighbours.Add(otherNeighbour);
                idToNeighbours[otherNeighbour].Add(agentId);
                freeNeighbours.RemoveAt(number);
            }

            return allCurrentNeighbours.Concat(randomNeighbours).ToArray();
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