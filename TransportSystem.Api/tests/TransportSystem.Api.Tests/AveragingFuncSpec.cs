using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Tests
{
    [TestFixture]
    public class AveragingFuncSpec
    {
        private const double deviationStub = 0;
        private AveragingFunc averagingFunc;
        private IPassengerBehaviourManager passengerBehaviour;
        private ChoiceTransportAlgorithmType transmissionType;

        [SetUp]
        public void SetUp()
        {
            const int carAvailabilityProbability = 1;
            passengerBehaviour = Substitute.For<IPassengerBehaviourManager>();
            transmissionType = ChoiceTransportAlgorithmType.Average;
            averagingFunc = new AveragingFunc(carAvailabilityProbability);
        }

        [TestCase(TransportType.Bus)]
        [TestCase(TransportType.Car)]
        public void Should_return_car_when_all_neighbors_choose_car_and_them_satisfactions_better(TransportType currentTransportType)
        {
            const double currentSatisfaction = 0.4;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };
            const TransportType expectedTransportType = TransportType.Car;

            var transportType = averagingFunc.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, deviationStub);

            transportType.Should().Be(expectedTransportType);
        }

        [Test]
        public void Should_return_transport_neighbor_when_neighbor_have_best_satisfaction()
        {
            const double currentSatisfaction = 0.4;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, 0.4),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Bus, 0.9),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, 0.4)
            };
            const TransportType expectedTransportType = TransportType.Bus;

            var transportType = averagingFunc.ChooseNextTransportType(neighbors, TransportType.Car, currentSatisfaction, deviationStub);

            transportType.Should().Be(expectedTransportType);
        }

        [Test]
        public void Should_return_best_transport_neighbors()
        {
            const double currentSatisfaction = 0.4;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, 0.7),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Bus, 0.9),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, 0.7),
                TestHelpers.CreatePassenger(passengerBehaviour, 4, transmissionType, TransportType.Bus, 0.4)
            };
            const TransportType expectedTransportType = TransportType.Car;

            var transportType = averagingFunc.ChooseNextTransportType(neighbors, TransportType.Car, currentSatisfaction, deviationStub);

            transportType.Should().Be(expectedTransportType);
        }
    }
}