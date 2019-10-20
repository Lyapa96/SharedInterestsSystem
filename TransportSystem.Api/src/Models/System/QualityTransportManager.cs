using System;
using System.Linq;
using TransportSystem.Api.Models.Data;
using static System.Int32;

namespace TransportSystem.Api.Models.System
{
    public static class QualityTransportManager
    {
//        public static void ChangeQuality(Passenger[][] passengers)
//        {
//            var carCount = passengers.Sum(x => x.Count(y => y.TransportType == TransportType.Car));
//            var rowCount = passengers.Length;
//            var columnCount = passengers.First().Length;
//            var passengersCount = rowCount*columnCount;
//
//            for (var i = 0; i < rowCount; i++)
//            for (var j = 0; j < columnCount; j++)
//            {
//                var passenger = passengers[i][j];
//                passenger.QualityCoefficient = passenger.TransportType == TransportType.Car
//                    ? Math.Round(GetQualityCoefficientForCar(carCount, passengersCount, passenger), 2)
//                    : Math.Round(GetQualityCoefficientForBus(passenger), 2);
//            }
//        }

        public static void ChangeQuality(Passenger[] passengers)
        {
            var carCount = passengers.Count(x => x.TransportType == TransportType.Car);
            var bikeCount = passengers.Count(x => x.TransportType == TransportType.Bike);
            var passengersCount = passengers.Length;

            foreach (var passenger in passengers)
            {
                passenger.QualityCoefficient =
                    GetQualityCoefficient(passenger.TransportType, carCount, bikeCount, passengersCount, passenger);
            }
        }

        private static double GetQualityCoefficient(TransportType transportType, int carCount,int bikeCount, int passengersCount, Passenger passenger)
        {
            switch (transportType)
            {
                case TransportType.Car:
                    return Math.Round(GetQualityCoefficientForCar(carCount, passengersCount, passenger), 2);
                case TransportType.Bus:
                    return Math.Round(GetQualityCoefficientForBusInSmo(passenger), 2);
                case TransportType.Subway:
                    return 0.6;
                case TransportType.Bike:
                   return Math.Round(GetQualityCoefficientForBike(bikeCount, passengersCount, passenger), 2);
                case TransportType.Tram:
                    return 0.4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(transportType), transportType, null);
            }
        }

        private static double GetQualityCoefficientForBus(Passenger passenger)
        {
            if (Parse(passenger.Id) <= 3)
                return 0.3;
            if (Parse(passenger.Id) <= 6)
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

        private static double GetQualityCoefficientForBike(int bikeCount, int n, Passenger passenger)
        {
            if (passenger.Neighbors.Count(x => x.TransportType == TransportType.Bike) < 2)
            {
                var answer = 1 - (double)bikeCount / n - 0.25;
                return answer;
            }

            return 0.1;
        }

        private static double GetQualityCoefficientForBusInSmo(Passenger passenger)
        {
            return Math.Abs(passenger.FirstBusQuality) < 0.001
                ? 0.5
                : passenger.FirstBusQuality;
        }
    }
}