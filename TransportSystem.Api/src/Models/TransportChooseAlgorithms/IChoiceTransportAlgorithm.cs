using System.Collections.Generic;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms
{
    public interface IChoiceTransportAlgorithm
    {
        TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue);
    }
}