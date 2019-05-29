using System;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.System
{
    public static class QualityTransportManager
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
                    ? Math.Round(GetQualityCoefficientForCar(carCount, passengersCount, passenger), 2)
                    : Math.Round(GetQualityCoefficientForBus(passenger), 2);
            }
        }

        private static double GetQualityCoefficientForBus(Passenger passenger)
        {
            if (Int32.Parse(passenger.Id) <= 3)
                return 0.3;
            if (Int32.Parse(passenger.Id) <= 6)
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

        public static void ChangeQuality(Passenger[] passengers)
        {
            var carCount = passengers.Count(x => x.TransportType == TransportType.Car);
            var passengersCount = passengers.Length;

            foreach (var passenger in passengers)
            {
                passenger.QualityCoefficient = passenger.TransportType == TransportType.Car
                    ? Math.Round(GetQualityCoefficientForCar(carCount, passengersCount, passenger), 2)
                    : Math.Round(GetQualityCoefficientForBusInSmo(passenger), 2);
            }
        }

        private static double GetQualityCoefficientForBusInSmo(Passenger passenger)
        {
            return passenger.FirstBusQuality == 0 ? 0.5 : passenger.FirstBusQuality;
        }
    }
}