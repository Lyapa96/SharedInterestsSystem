using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.Deviation
{
    public class DeviationAlgorithm : IChoiceTransportAlgorithm
    {
        public TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue)
        {
            var neighborsSameTransport = neighbors.Where(x => x.TransportType == currentTransportType).ToArray();
            var averageSatisfactionSameTransport = !neighborsSameTransport.Any() ? 0 : neighborsSameTransport.Average(x => x.Satisfaction);
            if (averageSatisfactionSameTransport - currentSatisfaction > deviationValue)
                return TransportType.Bus == currentTransportType ? TransportType.Car : TransportType.Bus;

            var neighborsOtherTransport = neighbors.Where(x => x.TransportType != currentTransportType).ToArray();
            var averageSatisfactionOtherTransport = !neighborsOtherTransport.Any() ? 0 : neighborsOtherTransport.Average(x => x.Satisfaction);
            if (averageSatisfactionOtherTransport - currentSatisfaction > deviationValue)
                return TransportType.Bus == currentTransportType ? TransportType.Car : TransportType.Bus;

            return currentTransportType;
        }
    }
}