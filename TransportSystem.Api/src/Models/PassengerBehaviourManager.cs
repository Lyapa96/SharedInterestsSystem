using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithm;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace TransportSystem.Api.Models
{
    public class PassengerBehaviourManager : IPassengerBehaviourManager
    {
        private readonly SatisfactionDeterminationAlgorithmFactory satisfactionDeterminationAlgorithmFactory;
        private readonly TransmissionFuncFactory transmissionFuncFactory;

        public PassengerBehaviourManager(IAgentStateStorage stateStorage)
        {
            transmissionFuncFactory = new TransmissionFuncFactory(stateStorage);
            satisfactionDeterminationAlgorithmFactory = new SatisfactionDeterminationAlgorithmFactory(stateStorage);
        }

        public IChoiceTransportAlgorithm GetTransmissionFunc(ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            return transmissionFuncFactory.GetTransmissionFunc(choiceTransportAlgorithmType);
        }

        public ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            return satisfactionDeterminationAlgorithmFactory.GetAlgoritm(choiceTransportAlgorithmType);
        }
    }
}