using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public class AgentState
    {
        private readonly TransportType currentTransoprt;

        private readonly List<(TransportType transportType, int value)> neighborsTransportRelation = new List<(TransportType transportType, int value)>();

        public AgentState(HashSet<Passenger> neighbors, double currentSatisfaction, TransportType currentTransoprt)
        {
            this.currentTransoprt = currentTransoprt;
            foreach (var neighbor in neighbors)
            {
                var difference = neighbor.Satisfaction - currentSatisfaction;
                if (difference > 0)
                    neighborsTransportRelation.Add((neighbor.TransportType, -1));
                else if (difference < 0)
                    neighborsTransportRelation.Add((neighbor.TransportType, 1));
                else neighborsTransportRelation.Add((neighbor.TransportType, 0));
            }
        }

        public string GetStringFormat()
        {
            var stringBuilder = new StringBuilder();
            foreach (var pair in neighborsTransportRelation.OrderBy(x => x.transportType))
            {
                stringBuilder.Append($"[{pair.transportType} : {pair.value}]");
            }

            return currentTransoprt + stringBuilder.ToString();
        }
    }
}