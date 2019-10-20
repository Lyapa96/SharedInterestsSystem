﻿using System;
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

        public TransportType GetBestNextTransport(string currentAgentState)
        {
            var qFuncInfo = db.QFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
            if (qFuncInfo == null)
            {
                qFuncInfo = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                db.QFuncInfos.Add(qFuncInfo);
                db.SaveChanges();
            }

            return QLearningAlgorithm.GetBestNextTransportWithEpsilonMush(qFuncInfo);
        }

        public void SaveStateReward(
            string previousAgentState,
            string currentAgentState,
            double reward,
            TransportType previousAction)
        {
            var previousQFunc = db.QFuncInfos.FirstOrDefault(x => x.State == previousAgentState);
            if (previousQFunc == null)
                db.Add(StorageHelpers.CreateRandomQFuncInfo(previousAgentState));
            else
            {
                var currenQFunc = db.QFuncInfos.FirstOrDefault(x => x.State == currentAgentState);
                if (currenQFunc == null)
                {
                    currenQFunc = StorageHelpers.CreateRandomQFuncInfo(currentAgentState);
                    db.Add(currenQFunc);
                }

                var maxNextReward = currenQFunc.GetBestReward();

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