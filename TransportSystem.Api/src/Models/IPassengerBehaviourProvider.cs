using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Models
{
    public interface IPassengerBehaviourProvider
    {
        IChoiceTransportAlgorithm GetChoiceTransportAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType);
        ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(ChoiceTransportAlgorithmType choiceTransportAlgorithmType);
    }
}