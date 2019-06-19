using System;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Models.SatisfactionDetermination
{
    public class QLearningQualityCoefficientAlgorithm : ISatisfactionDeterminationAlgorithm
    {
        private const double RewardWorth = 0.5;
        private readonly IAgentStateStorage stateStorage;

        public QLearningQualityCoefficientAlgorithm(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public double GetSatisfaction(Passenger passenger)
        {
            var currentQualityCoefficient = passenger.QualityCoefficient;
            var reward = 0.0;
            if (passenger.AllQualityCoefficients.Count != 0)
            {
                var previousQualityCoefficient = passenger.AllQualityCoefficients.Last();
                reward = GetReward(previousQualityCoefficient, currentQualityCoefficient);
            }

            var currentState = new AgentState(passenger.Neighbors, passenger.Satisfaction, passenger.TransportType).GetStringFormat();
            stateStorage.SaveStateReward(passenger.PreviousState, currentState, reward, passenger.TransportType);

            var allQualityCoefficients = passenger.AllQualityCoefficients;
            var averageQuality = allQualityCoefficients.Count > 0
                ? allQualityCoefficients.Skip(Math.Max(0, allQualityCoefficients.Count - 5)).Average()
                : 0;

            return passenger.TransportType == TransportType.Bus
                ? (currentQualityCoefficient - averageQuality + 0)/2 + passenger.QualityCoefficient
                : (currentQualityCoefficient - averageQuality + 1)/2 + passenger.QualityCoefficient;
        }

        private double GetReward(double previousQualityCoefficient, double currentQualityCoefficient)
        {
            return previousQualityCoefficient > currentQualityCoefficient ? -RewardWorth : RewardWorth;
        }
    }
}