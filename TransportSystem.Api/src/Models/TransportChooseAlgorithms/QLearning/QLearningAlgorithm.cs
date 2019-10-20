using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public static class QLearningAlgorithm
    {
        private const double Lf = 1;
        private const double Df = 0.1;
        private const double Epsilon = 0.15;
        private static readonly IRandomizer randomizer = new DefaultRandomizer();

        public static double GetUpdateReward(double previousReward, double maxNextReward, double currentReward)
        {
            return previousReward + Lf * (currentReward + Df * maxNextReward + previousReward);
        }

        public static TransportType GetBestNextTransportWithEpsilonMush(QFuncInfo qFuncInfo)
        {
            var bestTransportType = qFuncInfo.GetBestTransportType();

            return randomizer.GetRandomDouble() > Epsilon
                ? bestTransportType
                : TransportTypes.GetRandomTransportWithoutType(bestTransportType, randomizer);
        }
    }
}