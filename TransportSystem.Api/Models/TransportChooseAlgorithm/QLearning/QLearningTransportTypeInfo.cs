using System.Collections.Generic;
using System.Linq;

namespace TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning
{
    public class QLearningTransportTypeInfo
    {
        public QLearningTransportTypeInfo(TransportType transportType, double reward)
        {
            TransportType = transportType;
            Rewards = new List<double> {reward};
        }

        public QLearningTransportTypeInfo()
        {
            Rewards = new List<double>();
        }

        public TransportType TransportType { get; set; }
        public List<double> Rewards { get; set; }
        public double GetAverageValue => Rewards.Average();
    }
}