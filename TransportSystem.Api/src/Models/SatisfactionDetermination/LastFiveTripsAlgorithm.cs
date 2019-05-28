using System;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.SatisfactionDetermination
{
    public class LastFiveTripsAlgorithm : ISatisfactionDeterminationAlgorithm
    {
        public double GetSatisfaction(Passenger passenger)
        {
            var allQualityCoefficients = passenger.AllQualityCoefficients;
            var qualityCoefficient = passenger.QualityCoefficient;
            var averageQuality = allQualityCoefficients.Count > 0
                ? allQualityCoefficients.Skip(Math.Max(0, allQualityCoefficients.Count - 5)).Average()
                : 0; 
            return (qualityCoefficient - averageQuality + 1) / 2 + passenger.QualityCoefficient;
        }
    }
}