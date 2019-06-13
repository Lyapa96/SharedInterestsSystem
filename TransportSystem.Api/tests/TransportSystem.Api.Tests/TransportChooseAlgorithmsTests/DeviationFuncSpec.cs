using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportChooseAlgorithms.Deviation;

namespace TransportSystem.Api.Tests.TransportChooseAlgorithmsTests
{
    public class DeviationFuncSpec
    {
        private double deviation = 0.01;
        private DeviationAlgorithm averagingAlgorithm;
        private IPassengerBehaviourProvider passengerBehaviour;
        private ChoiceTransportAlgorithmType transmissionType;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviourProvider>();
            transmissionType = ChoiceTransportAlgorithmType.Deviation;
            averagingAlgorithm = new DeviationAlgorithm();
        }

        [Test]
        public void Should_return_other_transport_when_neighbors_has_better_satisfaction_and_deviation_enough()
        {
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };
            const TransportType expectedTransportType = TransportType.Bus;

            var transportType = averagingAlgorithm.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, deviation);

            transportType.Should().Be(expectedTransportType);
        }

        [Test]
        public void Should_return_same_transport_when_neighbors_has_better_satisfaction_but_deviation_not_enough()
        {
            deviation = 0.5;
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };
            const TransportType expectedTransportType = TransportType.Car;

            var transportType = averagingAlgorithm.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, deviation);

            transportType.Should().Be(expectedTransportType);
        }
    }
}