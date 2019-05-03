using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithm;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace TransportSystem.Api.Models
{
    public class PassengerBehaviour : IPassengerBehaviour
    {
        private readonly SatisfactionDeterminationAlgorithmFactory satisfactionDeterminationAlgorithmFactory;
        private readonly TransmissionFuncFactory transmissionFuncFactory;

        public PassengerBehaviour(IAgentStateStorage stateStorage)
        {
            transmissionFuncFactory = new TransmissionFuncFactory(stateStorage);
            satisfactionDeterminationAlgorithmFactory = new SatisfactionDeterminationAlgorithmFactory(stateStorage);
        }

        public ITransmissionFunc GetTransmissionFunc(TransmissionType transmissionType)
        {
            return transmissionFuncFactory.GetTransmissionFunc(transmissionType);
        }

        public ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(TransmissionType transmissionType)
        {
            return satisfactionDeterminationAlgorithmFactory.GetAlgoritm(transmissionType);
        }
    }
}