using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public static class QLearningAlgorithm
    {
        private const double Lf = 1;
        private const double Df = 0.1;
        private const double Epsilon = 0.15;
        private static readonly Random rnd = new Random();

        public static double GetUpdateReward(double previousReward, double maxNextReward, double currentReward)
        {
            return previousReward + Lf*(currentReward + Df*maxNextReward + previousReward);
        }

        public static TransportType GetBestNextTransportWithEpsilonMush(QFuncInfo qFuncInfo)
        {
            var bestTransportType = qFuncInfo.BusReward > qFuncInfo.CarReward ? TransportType.Bus : TransportType.Car;
            return rnd.NextDouble() > Epsilon
                ? bestTransportType
                : GetOtherRandomTransportType(bestTransportType);
        }

        public static TransportType GetOtherRandomTransportType(TransportType transportType)
        {
            TransportType[] allTransportTypes = {TransportType.Car, TransportType.Bus};
            var appropriateTransport = allTransportTypes
                .Where(x => x != transportType)
                .ToArray();
            return GetRandomTransportType(appropriateTransport, rnd);
        }

        private static TransportType GetRandomTransportType(IReadOnlyList<TransportType> appropriateTransport, Random rnd)
        {
            if (appropriateTransport.Count == 1)
                return appropriateTransport.First();

            return rnd.NextDouble() < 0.5 ? TransportType.Bus : TransportType.Car;
        }
    }
}