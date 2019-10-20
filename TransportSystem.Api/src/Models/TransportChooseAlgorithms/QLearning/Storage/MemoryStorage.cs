using System.Collections.Generic;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage
{
    public class MemoryStorage : IAgentStateStorage
    {
        private readonly Dictionary<string, QFuncInfo> statesToQFuncInfos = new Dictionary<string, QFuncInfo>();

        public TransportType GetBestNextTransport(string currentAgentState)
        {
            if (!statesToQFuncInfos.ContainsKey(currentAgentState))
            {
                var qFuncInfo = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                statesToQFuncInfos.Add(currentAgentState, qFuncInfo);
            }

            return QLearningAlgorithm.GetBestNextTransportWithEpsilonMush(statesToQFuncInfos[currentAgentState]);
        }

        public void SaveStateReward(
            string previousAgentState,
            string currentAgentState,
            double reward,
            TransportType previousAction)
        {
            if (!statesToQFuncInfos.ContainsKey(previousAgentState))
            {
                statesToQFuncInfos.Add(previousAgentState, StorageHelpers.CreateRandomQFuncInfo(previousAgentState));
            }
            else
            {
                var previousQFunc = statesToQFuncInfos[previousAgentState];
                if (!statesToQFuncInfos.ContainsKey(currentAgentState))
                {
                    var qFuncInfo = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                    statesToQFuncInfos.Add(currentAgentState, qFuncInfo);
                }

                var currentQFunc = statesToQFuncInfos[currentAgentState];

                var maxNextReward = currentQFunc.GetBestReward();

                var previousReward = previousQFunc.GetReward(previousAction);
                var newReward = QLearningAlgorithm.GetUpdateReward(previousReward, maxNextReward, reward);
                previousQFunc.SetReward(previousAction, newReward);
            }

        }
    }
}