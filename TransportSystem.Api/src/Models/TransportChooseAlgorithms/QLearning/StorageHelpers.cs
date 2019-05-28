using System;
using System.Collections.Generic;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public static class StorageHelpers
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof (T), value, true);
        }

        public static List<QLearningTransportTypeInfo> CreateFirstInfo(TransportType transportType, double reward)
        {
            var busInfo = new QLearningTransportTypeInfo(TransportType.Bus, 0.5);
            var carInfo = new QLearningTransportTypeInfo(TransportType.Car, 0.5);
            if (transportType == TransportType.Bus)
                busInfo.Rewards.Add(reward);
            else
                carInfo.Rewards.Add(reward);
            return new List<QLearningTransportTypeInfo>
            {
                busInfo,
                carInfo
            };
        }

        public static List<AgentAction> CreateFirstAgentActions(TransportType transportType, double reward)
        {
            const double startReward = 0.5;
            var busInfo = new AgentAction(TransportType.Bus, 1, startReward, startReward);
            var carInfo = new AgentAction(TransportType.Car, 1, startReward, startReward);
            if (transportType == TransportType.Bus)
                busInfo.AddReward(reward);
            else
                carInfo.AddReward(reward);
            return new List<AgentAction>
            {
                busInfo,
                carInfo
            };
        }

        public static QFuncInfo CreateRandomQFuncInfo(string state)
        {
            var rnd = new Random();
            return new QFuncInfo
            {
                State = state,
                BusReward = rnd.NextDouble(),
                CarReward = rnd.NextDouble()
            };
        }
    }
}