using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql
{
    public class QFuncInfo
    {
        public QFuncInfo(string state, Random rnd)
        {
            State = state;
            TransportTypesToRewards = TransportTypes
                .AllTransportTypes
                .Select(type => (type, rnd.NextDouble()))
                .ToDictionary(x => x.type, x => x.Item2);
        }


        [Key] public string State { get; set; }

        public readonly Dictionary<TransportType, double> TransportTypesToRewards;

        public void SetReward(TransportType transportType, double reward)
        {
            TransportTypesToRewards[transportType] = reward;
        }

        public double GetReward(TransportType transportType)
        {
            return TransportTypesToRewards[transportType];
        }

        public TransportType GetBestTransportType()
        {
            return TransportTypesToRewards.Aggregate((first, second) => first.Value > second.Value ? first : second)
                .Key;
        }

        public double GetBestReward()
        {
            return TransportTypesToRewards.Values.Max();
        }
    }
}