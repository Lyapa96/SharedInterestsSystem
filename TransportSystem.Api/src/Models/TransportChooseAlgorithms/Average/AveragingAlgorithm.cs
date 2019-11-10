using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.Average
{
    public class AveragingAlgorithm : IChoiceTransportAlgorithm
    {
        private readonly IRandomizer randomizer;
        private readonly double carAvailabilityProbability;

        public AveragingAlgorithm(IRandomizer randomizer, double carAvailabilityProbability)
        {
            this.randomizer = randomizer;
            this.carAvailabilityProbability = carAvailabilityProbability;
        }

        public TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors, 
            TransportType currentTransportType,
            double currentSatisfaction, 
            double deviationValue, 
            TransportType[] availableTransportTypes)
        {
            var typeTransportInfos = neighbors
                .GroupBy(x => x.TransportType)
                .Select(GetTransportInfo);

            foreach (var info in typeTransportInfos)
            {
                if (info.Item2 > currentSatisfaction)
                {
                    currentTransportType = info.Item1;
                    currentSatisfaction = info.Item2;
                }
            }

            if (currentTransportType == TransportType.Car)
                currentTransportType = randomizer.GetRandomDouble() < carAvailabilityProbability 
                    ? TransportType.Car 
                    : TransportTypes.GetRandomTransportWithoutType(TransportType.Car, randomizer, availableTransportTypes);

            return currentTransportType;
        }

        private static Tuple<TransportType, double> GetTransportInfo(IGrouping<TransportType, Passenger> type)
        {
            var averageSatisfaction = type.Select(x => x.Satisfaction).Average();
            return Tuple.Create(type.Key, averageSatisfaction);
        }
    }
}