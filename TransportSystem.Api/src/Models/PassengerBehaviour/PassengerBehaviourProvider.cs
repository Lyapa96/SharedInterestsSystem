using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Models.PassengerBehaviour
{
    public class PassengerBehaviourProvider : IPassengerBehaviourProvider
    {
        private readonly SatisfactionDeterminationAlgorithmFactory satisfactionDeterminationAlgorithmFactory;
        private readonly ChoiceTransportAlgorithmFactory choiceTransportAlgorithmFactory;

        public PassengerBehaviourProvider(IAgentStateStorage stateStorage)
        {
            choiceTransportAlgorithmFactory = new ChoiceTransportAlgorithmFactory(stateStorage);
            satisfactionDeterminationAlgorithmFactory = new SatisfactionDeterminationAlgorithmFactory(stateStorage);
        }

        public IChoiceTransportAlgorithm GetChoiceTransportAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            return choiceTransportAlgorithmFactory.GetTransmissionFunc(choiceTransportAlgorithmType);
        }

        public ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            return satisfactionDeterminationAlgorithmFactory.GetAlgorithm(choiceTransportAlgorithmType);
        }
    }
}