using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Tests.TransportChooseAlgorithmsTests
{
    public class QLearningTransmissionFuncSpec
    {
        private const double DeviationStub = 0;
        private IAgentStateStorage storage;
        private QLearningChoiceTransportAlgorithm qLearningTransmissionFunc;
        private IPassengerBehaviourProvider passengerBehaviour;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviourProvider>();
            storage = Substitute.For<IAgentStateStorage>();
            qLearningTransmissionFunc = new QLearningChoiceTransportAlgorithm(storage);
        }

        [Test]
        public void Should_get_best_transport_from_storage()
        {
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var transmissionType = ChoiceTransportAlgorithmType.QLearning;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };

            qLearningTransmissionFunc.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, DeviationStub);

            storage.Received().GetBestNextTransport(Arg.Any<string>());
        }
    }
}