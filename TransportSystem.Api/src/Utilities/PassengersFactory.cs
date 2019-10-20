using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Utilities
{
    public class PassengersFactory : IPassengersFactory
    {
        private readonly IRandomizer randomizer;
        private readonly IPassengerBehaviourProvider behaviourProvider;

        public PassengersFactory(IRandomizer randomizer, IPassengerBehaviourProvider behaviourProvider)
        {
            this.randomizer = randomizer;
            this.behaviourProvider = behaviourProvider;
        }

        public PassengerDto[] CreatePassengers(int columns, int rows)
        {
            var passengers = new List<PassengerDto>();
            var count = rows * columns;
            for (var i = 0; i < count; i++)
            {
                var passenger = new PassengerDto
                {
                    Id = $"{i + 1}",
                    Satisfaction = Math.Round(randomizer.GetRandomDouble(), 2),
                    Quality = Math.Round(randomizer.GetRandomDouble(), 2),
                    TransportType = TransportTypes.GetRandomTransportTypeBetweenCarAndBus(randomizer),
                    FirstBusQuality = 0.5
                };
                passengers.Add(passenger);
            }

            return passengers.ToArray();
        }

        public PassengerDto[] CreateAllPassengersTogether(SmoData smoData)
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
                .Select(x => CreateRandomPassengerDto(smoData, x, smoBusPassengers, defaultSatisfaction));

            var allPassengers = smoBusPassengers.Concat(carPassengers).ToArray();
            return allPassengers;
        }

        private PassengerDto CreateRandomPassengerDto(
            SmoData smoData,
            int x,
            PassengerDto[] smoBusPassengers,
            double defaultSatisfaction)
        {
            var type = TransportTypes.GetRandomTransportWithoutType(TransportType.Bus, randomizer);

            return new PassengerDto
            {
                Id = $"{(int) type}.{x}",
                Quality = 1 - (double) smoData.PassengersOnCar / (smoData.PassengersOnCar + smoBusPassengers.Length),
                TransportType = type,
                Satisfaction = defaultSatisfaction,
                FirstBusQuality = 0
            };
        }

        public Passenger[] CreatePassengers(ChoiceTransportAlgorithmType algorithmType, PassengerDto[] passengers)
        {
            var idToPassengers = passengers
                .ToDictionary(
                    x => x.Id,
                    x => Passenger.Create(x, behaviourProvider, algorithmType));

            var allPassengers = new List<Passenger>();
            foreach (var passenger in passengers)
            {
                var neighbors = passenger.Neighbours.Select(x => idToPassengers[x]);
                var currentPassenger = idToPassengers[passenger.Id];
                foreach (var neighbor in neighbors)
                {
                    currentPassenger.AddNeighbor(neighbor);
                }

                allPassengers.Add(currentPassenger);
            }

            return allPassengers.ToArray();
        }
    }
}