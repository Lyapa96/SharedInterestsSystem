using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.SatisfactionDetermination;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Tests
{
    public class SatisfactionAlgorithmSpec
    {
        private LastFiveTripsAlgorithm satisfactionAlgorithm;
        private IPassengerBehaviourManager passengerBehaviour;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviourManager>();
            satisfactionAlgorithm = new LastFiveTripsAlgorithm();
        }

        [Test]
        public void Should_return_right_satisfaction()
        {
            var passenger = TestHelpers.CreatePassenger(passengerBehaviour, 1, ChoiceTransportAlgorithmType.Average, TransportType.Bus, 0.1);
            passenger.QualityCoefficient = 1;

            passenger.AllQualityCoefficients = new List<double>(){1,1,1,1,1};

            var result = satisfactionAlgorithm.GetSatisfaction(passenger);

            result.Should().Be(1.5);
        }
    }
}