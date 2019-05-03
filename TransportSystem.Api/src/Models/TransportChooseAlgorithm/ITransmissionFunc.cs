using System.Collections.Generic;

namespace TransportSystem.Api.Models.TransportChooseAlgorithm
{
    public interface ITransmissionFunc
    {
        TransportType ChooseNextTransportType(HashSet<Passenger> neighbors, TransportType currentTransportType, double currentSatisfaction, double deviationValue);
    }
}