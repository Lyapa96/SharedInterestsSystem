using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Models
{
    public interface IPassengerBehaviour
    {
        ITransmissionFunc GetTransmissionFunc(TransmissionType transmissionType);
        ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(TransmissionType transmissionType);
    }
}