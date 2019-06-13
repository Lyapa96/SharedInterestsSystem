using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Utilities
{
    public static class PassengersHelper
    {
        public static PassengerDto[] GetAllPassengersTogether(SmoData smoData)
        {
            const double defaultSatisfaction = 0.5;
            var smoBusPassengers = smoData.SmoPassengers
                .Select(
                    x => new PassengerDto
                    {
                        Id = x.AgentId,
                        Quality = x.Quality,
                        TransportType = TransportType.Bus,
                        Satisfaction = defaultSatisfaction,
                        FirstBusQuality = x.Quality
                    })
                .ToArray();
            var carPassengers = Enumerable.Range(0, smoData.PassengersOnCar)
                .Select(
                    x => new PassengerDto
                    {
                        Id = $"car.{x}",
                        Quality = 1 - (double) smoData.PassengersOnCar/(smoData.PassengersOnCar + smoBusPassengers.Length),
                        TransportType = TransportType.Car,
                        Satisfaction = defaultSatisfaction,
                        FirstBusQuality = 0
                    });

            var allPassengers = smoBusPassengers.Concat(carPassengers).ToArray();
            return allPassengers;
        }

        public static void SetNeighborhood(this PassengerDto[] passengers, Dictionary<string, List<string>> neighborhood)
        {
            foreach (var passenger in passengers)
            {
                passenger.Neighbours = neighborhood[passenger.Id].ToArray();
            }
        }

        public static List<Passenger> CreatePassengers(PassengerDto[] passengerDtos, Dictionary<string, Passenger> idToPassengers)
        {
            var allPassengers = new List<Passenger>();
            foreach (var smoPassenger in passengerDtos)
            {
                var neighbors = smoPassenger.Neighbours.Select(x => idToPassengers[x]);
                var currentPassenger = idToPassengers[smoPassenger.Id];
                foreach (var neighbor in neighbors)
                {
                    currentPassenger.AddNeighbor(neighbor);
                }

                allPassengers.Add(currentPassenger);
            }

            return allPassengers;
        }

        private static TransportType GetRandomTransportType(Random rnd)
        {
            return rnd.NextDouble() < 0.5 ? TransportType.Bus : TransportType.Car;
        }

        public static TransportType GetOtherRandomTransportType(TransportType transportType)
        {
            var rnd = new Random();
            var randomTransportType = GetRandomTransportType(rnd);
            while (transportType == randomTransportType) randomTransportType = GetRandomTransportType(rnd);

            return randomTransportType;
        }

        public static PassengerDto[] CreatePassengers(int columns, int rows)
        {
            var rnd = new Random();
            var passengers = new List<PassengerDto>();
            var count = rows*columns;
            for (var i = 0; i < count; i++)
            {
                var passenger = new PassengerDto
                {
                    Id = $"{i + 1}",
                    Satisfaction = Math.Round(rnd.NextDouble(), 2),
                    Quality = Math.Round(rnd.NextDouble(), 2),
                    TransportType = GetRandomTransportType(rnd),
                    FirstBusQuality = 0.5
                };
                passengers.Add(passenger);
            }

            return passengers.ToArray();
        }
    }
}