using System.Collections.Generic;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Models.SatisfactionDetermination
{
    public class SatisfactionDeterminationAlgorithmFactory
    {
        private readonly Dictionary<ChoiceTransportAlgorithmType, ISatisfactionDeterminationAlgorithm> typeToAlgorithm;

        public SatisfactionDeterminationAlgorithmFactory(IAgentStateStorage storage)
        {
            typeToAlgorithm = new Dictionary<ChoiceTransportAlgorithmType, ISatisfactionDeterminationAlgorithm>
            {
                {ChoiceTransportAlgorithmType.Average, new LastFiveTripsAlgorithm()},
                {ChoiceTransportAlgorithmType.Deviation, new LastFiveTripsAlgorithm()},
                {ChoiceTransportAlgorithmType.QLearning, new QLearningQualityCoefficientAlgorithm(storage)}
            };
        }

        public ISatisfactionDeterminationAlgorithm GetAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            return typeToAlgorithm[choiceTransportAlgorithmType];
        }
    }
}