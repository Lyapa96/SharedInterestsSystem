using System.Collections.Generic;
using TransportSystem.Api.Models.TransportChooseAlgorithms.Average;
using TransportSystem.Api.Models.TransportChooseAlgorithms.Deviation;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms
{
    public class ChoiceTransportAlgorithmFactory
    {
        private const double CarAvailabilityProbability = 85;
        private readonly Dictionary<ChoiceTransportAlgorithmType, IChoiceTransportAlgorithm> typeToFunc;

        public ChoiceTransportAlgorithmFactory(IAgentStateStorage stateStorage)
        {
            typeToFunc = new Dictionary<ChoiceTransportAlgorithmType, IChoiceTransportAlgorithm>
            {
                {ChoiceTransportAlgorithmType.Average, new AveragingAlgorithm(CarAvailabilityProbability)},
                {ChoiceTransportAlgorithmType.Deviation, new DeviationAlgorithm()},
                {ChoiceTransportAlgorithmType.QLearning, new QLearningChoiceTransportAlgorithm(stateStorage)}
            };
        }

        public IChoiceTransportAlgorithm GetTransmissionFunc(ChoiceTransportAlgorithmType type)
        {
            return typeToFunc[type];
        }
    }
}