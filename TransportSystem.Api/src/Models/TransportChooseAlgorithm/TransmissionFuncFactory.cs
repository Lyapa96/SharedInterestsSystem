using System.Collections.Generic;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace TransportSystem.Api.Models.TransportChooseAlgorithm
{
    public class TransmissionFuncFactory
    {
        private const double CarAvailabilityProbability = 85;
        private readonly Dictionary<ChoiceTransportAlgorithmType, IChoiceTransportAlgorithm> typeToFunc;

        public TransmissionFuncFactory(IAgentStateStorage stateStorage)
        {
            typeToFunc = new Dictionary<ChoiceTransportAlgorithmType, IChoiceTransportAlgorithm>
            {
                {ChoiceTransportAlgorithmType.Average, new AveragingFunc(CarAvailabilityProbability)},
                {ChoiceTransportAlgorithmType.Deviation, new DeviationFunc()},
                {ChoiceTransportAlgorithmType.QLearning, new QLearningChoiceTransportAlgorithm(stateStorage)}
            };
        }

        public IChoiceTransportAlgorithm GetTransmissionFunc(ChoiceTransportAlgorithmType type)
        {
            return typeToFunc[type];
        }
    }
}