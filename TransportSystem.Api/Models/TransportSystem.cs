using System.Linq;

namespace TransportSystem.Api.Models
{
    public static class TransportSystem
    {
        public static void ChangeQuality(Passenger[][] passengers)
        {
            var carCount = passengers.Sum(x => x.Count(y => y.TransportType == TransportType.Car));
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            var passengersCount = rowCount*columnCount;

            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
            {
                var passenger = passengers[i][j];
                passenger.QualityCoefficient = passenger.TransportType == TransportType.Car
                    ? GetQualityCoefficientForCar(carCount, passengersCount, passenger)
                    : GetQualityCoefficientForBus(passenger);
            }
        }

        private static double GetQualityCoefficientForBus(Passenger passenger)
        {
            if (passenger.Number <= 3)
                return 0.3;
            if (passenger.Number <= 6)
                return 0.4;

            return 0.5;
        }

        private static double GetQualityCoefficientForCar(int carCount, int n, Passenger passenger)
        {
            if (passenger.Neighbors.Count(x => x.TransportType == TransportType.Car) < 2)
            {
                var answer = 1 - (double) carCount/n;
                return answer;
            }

            return 0.1;
        }
    }
}