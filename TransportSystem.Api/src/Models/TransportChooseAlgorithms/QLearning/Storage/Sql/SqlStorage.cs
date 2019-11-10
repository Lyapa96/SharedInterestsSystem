using System;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql
{
    public class SqlStorage : IAgentStateStorage, IDisposable
    {
        private readonly QFuncContext db;

        public SqlStorage()
        {
            db = new QFuncContext();
        }
            
        public TransportType GetBestNextTransport(string currentAgentState, TransportType[] availableTransportTypes)
        {
            var qFuncInfo = db.QFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
            if (qFuncInfo == null)
            {
                qFuncInfo = StorageHelpers.CreateRandomQFuncInfo(currentAgentState, availableTransportTypes);
                db.QFuncInfos.Add(qFuncInfo);
                db.SaveChanges();
            }

            return QLearningAlgorithm.GetBestNextTransportWithEpsilonMush(qFuncInfo, availableTransportTypes);
        }

        public void SaveStateReward(
            string previousAgentState,
            string currentAgentState,
            double reward,
            TransportType previousAction,
            TransportType[] availableTransportTypes)
        {
            var previousQFunc = db.QFuncInfos.FirstOrDefault(x => x.State == previousAgentState);
            if (previousQFunc == null)
                db.Add(StorageHelpers.CreateRandomQFuncInfo(previousAgentState, availableTransportTypes));
            else
            {
                var currentQFunc = db.QFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
                if (currentQFunc == null)
                {
                    currentQFunc = StorageHelpers.CreateRandomQFuncInfo(currentAgentState, availableTransportTypes);
                    db.Add(currentQFunc);
                }

                var maxNextReward = currentQFunc.GetBestReward();

                var previousReward = previousQFunc.GetReward(previousAction);
                var newReward = QLearningAlgorithm.GetUpdateReward(previousReward, maxNextReward, reward);
                previousQFunc.SetReward(previousAction, newReward);
            }

            db.SaveChanges();
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}