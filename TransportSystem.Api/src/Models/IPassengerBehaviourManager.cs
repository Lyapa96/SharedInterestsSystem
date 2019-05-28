using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Models
{
    public interface IPassengerBehaviourManager
    {
        IChoiceTransportAlgorithm GetTransmissionFunc(ChoiceTransportAlgorithmType choiceTransportAlgorithmType);
        ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType);
    }
}