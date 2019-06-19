﻿using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage
{
    public class MemoryStorage : IAgentStateStorage
    {
        private readonly List<QFuncInfo> qFuncInfos = new List<QFuncInfo>();

        public TransportType GetBestNextTransport(string currentAgentState)
        {
            var qFuncInfo = qFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
            if (qFuncInfo == null)
            {
                qFuncInfo = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                qFuncInfos.Add(qFuncInfo);
            }

            return QLearningAlgorithm.GetBestNextTransportWithEpsilonMush(qFuncInfo);
        }

        public void SaveStateReward(
            string previousAgentState,
            string currentAgentState,
            double reward,
            TransportType previousAction)
        {
            var previousQFunc = qFuncInfos.FirstOrDefault(x => x.State == previousAgentState);
            if (previousQFunc == null)
                qFuncInfos.Add(StorageHelpers.CreateRandomQFuncInfo(previousAgentState));
            else
            {
                var currenQFunc = qFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
                if (currenQFunc == null)
                {
                    currenQFunc = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                    qFuncInfos.Add(currenQFunc);
                }

                var maxNextReward = currenQFunc.GetBestReward();

                if (previousAction == TransportType.Bus)
                    previousQFunc.BusReward = QLearningAlgorithm.GetUpdateReward(previousQFunc.BusReward, maxNextReward, reward);
                else
                    previousQFunc.CarReward = QLearningAlgorithm.GetUpdateReward(previousQFunc.CarReward, maxNextReward, reward);
            }
        }
    }
}