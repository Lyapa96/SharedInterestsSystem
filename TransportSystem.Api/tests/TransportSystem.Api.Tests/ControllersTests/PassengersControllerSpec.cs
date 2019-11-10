using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Controllers;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Models.System;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportSystemSatisfaction;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Tests.ControllersTests
{
    [TestFixture]
    public class PassengersControllerSpec
    {
        [TestFixture]
        public class InitPassengers
        {
            private ITransportSystem transportSystem;
            private INeighborsManager neighborsManager;
            private ITransportSystemSatisfaction transportSystemSatisfaction;
            private IPassengersFactory passengersFactory;

            private PassengersController controller;

            [SetUp]
            public void SetUp()
            {
                transportSystem = Substitute.For<ITransportSystem>();
                neighborsManager = Substitute.For<INeighborsManager>();
                transportSystemSatisfaction = Substitute.For<ITransportSystemSatisfaction>();
                passengersFactory = Substitute.For<IPassengersFactory>();
                passengersFactory.CreatePassengers(Arg.Any<int>(), Arg.Any<int>())
                    .Returns(new PassengerDto[0]);

                controller = new PassengersController(transportSystem, passengersFactory, neighborsManager,
                    transportSystemSatisfaction);
            }

            [Test]
            public void Should_set_right_algorithm_type_to_iteration_result_from_init_passenger_info()
            {
                var initPassengerInfo = new InitPassengerInfo {AlgorithmType = ChoiceTransportAlgorithmType.QLearning};

                var actionResult = controller.InitPassengers(initPassengerInfo);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AlgorithmType.Should().Be(initPassengerInfo.AlgorithmType);
            }

            [Test]
            public void Should_return_zero_iteration_step_in_iteration_result()
            {
                var initPassengerInfo = new InitPassengerInfo();

                var actionResult = controller.InitPassengers(initPassengerInfo);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.IterationStep.Should().Be(0);
            }

            [Test]
            public void Should_create_passengers_from_factory()
            {
                var initPassengerInfo = new InitPassengerInfo
                {
                    Columns = 1,
                    Rows = 2
                };

                var _ = controller.InitPassengers(initPassengerInfo);

                passengersFactory.Received().CreatePassengers(initPassengerInfo.Columns, initPassengerInfo.Rows);
            }

            [Test]
            public void Should_set_geometric_neighbors_to_passengers_from_passenger_manager()
            {
                var initPassengerInfo = new InitPassengerInfo
                {
                    Columns = 2,
                    Rows = 2
                };
                var passengers = Enumerable
                    .Range(0, 4)
                    .Select(
                        x => new PassengerDto
                        {
                            Id = x.ToString()
                        })
                    .ToArray();
                var defaultNeighbors = new List<string> {"some neighbor id"};
                var neighborhood = Enumerable.Range(0, 4).ToDictionary(x => x.ToString(), x => defaultNeighbors);
                passengersFactory.CreatePassengers(initPassengerInfo.Columns, initPassengerInfo.Rows)
                    .Returns(passengers);
                neighborsManager.GetGeometricNeighborhood(passengers, initPassengerInfo.Columns)
                    .Returns(neighborhood);
                var expectedPassengers = Enumerable
                    .Range(0, 4)
                    .Select(
                        x => new PassengerDto
                        {
                            Id = x.ToString(),
                            Neighbours = defaultNeighbors.ToArray()
                        })
                    .ToArray();

                var actionResult = controller.InitPassengers(initPassengerInfo);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.Passengers.ShouldBeEquivalentTo(expectedPassengers);
            }

            [Test]
            public void Should_return_average_satisfaction_from_transport_system_satisfaction()
            {
                const int satisfaction = 1;
                var initPassengerInfo = new InitPassengerInfo();
                transportSystemSatisfaction.Evaluate(Arg.Any<PassengerDto[]>())
                    .Returns(satisfaction);

                var actionResult = controller.InitPassengers(initPassengerInfo);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AverageSatisfaction.Should().Be(satisfaction);
            }
        }

        [TestFixture]
        public class InitPassengersFromSmo
        {
            private ITransportSystem transportSystem;
            private INeighborsManager neighborsManager;
            private ITransportSystemSatisfaction transportSystemSatisfaction;
            private IPassengersFactory passengersFactory;

            private PassengersController controller;
            private TransportType[] AvailableTransportTypes = {TransportType.Bus, TransportType.Car};

            [SetUp]
            public void SetUp()
            {
                transportSystem = Substitute.For<ITransportSystem>();
                neighborsManager = Substitute.For<INeighborsManager>();
                transportSystemSatisfaction = Substitute.For<ITransportSystemSatisfaction>();
                passengersFactory = Substitute.For<IPassengersFactory>();
                passengersFactory.CreatePassengers(Arg.Any<int>(), Arg.Any<int>())
                    .Returns(new PassengerDto[0]);

                controller = new PassengersController(transportSystem, passengersFactory, neighborsManager,
                    transportSystemSatisfaction);
            }

            [Test]
            public void Should_set_q_learning_algorithm_type_to_iteration_result_from_smo_data()
            {
                var smoData = new SmoData();

                var actionResult = controller.InitPassengersFromSmo(smoData);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AlgorithmType.Should().Be(ChoiceTransportAlgorithmType.QLearning);
            }

            [Test]
            public void Should_return_zero_iteration_step_in_iteration_result()
            {
                var smoData = new SmoData();

                var actionResult = controller.InitPassengersFromSmo(smoData);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.IterationStep.Should().Be(0);
            }

            [Test]
            public void Should_create_passengers_from_factory()
            {
                var smoData = new SmoData
                {
                    Columns = 2,
                    NeighboursCount = 3,
                    PassengersOnCar = 3,
                    SmoPassengers = new SmoPassenger[3],
                };

                var _ = controller.InitPassengersFromSmo(smoData);

                passengersFactory.Received().CreateAllPassengersTogether(smoData, Arg.Any<TransportType[]>());
            }

            [Test]
            public void Should_set_neighbors_for_each_passengers_from_passenger_manager()
            {
                var smoData = new SmoData
                {
                    Columns = 2,
                    NeighboursCount = 3,
                    PassengersOnCar = 4,
                    SmoPassengers = new SmoPassenger[3]
                };
                var passengers = Enumerable
                    .Range(0, 4)
                    .Select(
                        x => new PassengerDto
                        {
                            Id = x.ToString()
                        })
                    .ToArray();
                var defaultNeighbors = new List<string> {"some neighbor id"};
                var neighborhood = Enumerable.Range(0, 4).ToDictionary(x => x.ToString(), x => defaultNeighbors);
                passengersFactory.CreateAllPassengersTogether(smoData, Arg.Any<TransportType[]>())
                    .Returns(passengers);
                neighborsManager.GetEachPassengerNeighbors(smoData.NeighboursCount, smoData.Columns, passengers)
                    .Returns(neighborhood);
                var expectedPassengers = Enumerable
                    .Range(0, 4)
                    .Select(
                        x => new PassengerDto
                        {
                            Id = x.ToString(),
                            Neighbours = defaultNeighbors.ToArray(),
                        })
                    .ToArray();

                var actionResult = controller.InitPassengersFromSmo(smoData);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.Passengers.ShouldBeEquivalentTo(expectedPassengers);
            }

            [Test]
            public void Should_return_average_satisfaction_from_transport_system_satisfaction()
            {
                const int satisfaction = 1;
                var smoData = new SmoData();
                transportSystemSatisfaction.Evaluate(Arg.Any<PassengerDto[]>())
                    .Returns(satisfaction);

                var actionResult = controller.InitPassengersFromSmo(smoData);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AverageSatisfaction.Should().Be(satisfaction);
            }
        }

        [TestFixture]
        public class NextStep
        {
            private ITransportSystem transportSystem;
            private INeighborsManager neighborsManager;
            private ITransportSystemSatisfaction transportSystemSatisfaction;
            private IPassengersFactory passengersFactory;
            private TransportType[] AvailableTransportTypes = {TransportType.Bus, TransportType.Car};
            private PassengersController controller;

            [SetUp]
            public void SetUp()
            {
                transportSystem = Substitute.For<ITransportSystem>();
                neighborsManager = Substitute.For<INeighborsManager>();
                transportSystemSatisfaction = Substitute.For<ITransportSystemSatisfaction>();
                passengersFactory = Substitute.For<IPassengersFactory>();
                passengersFactory.CreatePassengers(Arg.Any<ChoiceTransportAlgorithmType>(), Arg.Any<PassengerDto[]>())
                    .Returns(new Passenger[0]);

                controller = new PassengersController(transportSystem, passengersFactory, neighborsManager,
                    transportSystemSatisfaction);
            }

            [Test]
            public void Should_set_algorithm_type_to_iteration_result_from_previous_iteration_result()
            {
                var previousIterationResult = new IterationResult
                {
                    AlgorithmType = ChoiceTransportAlgorithmType.Deviation
                };

                var actionResult = controller.GetNextStep(previousIterationResult);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AlgorithmType.Should().Be(iterationResult.AlgorithmType);
            }

            [Test]
            public void Should_return_next_iteration_step_from_previous_iteration_result()
            {
                var previousIterationResult = new IterationResult {IterationStep = 1};
                const int expectedIterationStep = 2;

                var actionResult = controller.GetNextStep(previousIterationResult);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.IterationStep.Should().Be(expectedIterationStep);
            }

            [Test]
            public void Should_return_average_satisfaction_from_transport_system_satisfaction()
            {
                const int satisfaction = 1;
                var previousResult = new IterationResult();
                transportSystemSatisfaction.Evaluate(Arg.Any<PassengerDto[]>())
                    .Returns(satisfaction);

                var actionResult = controller.GetNextStep(previousResult);
                var okResult = actionResult as OkObjectResult;
                var iterationResult = okResult.Value as IterationResult;

                iterationResult.AverageSatisfaction.Should().Be(satisfaction);
            }

            [Test]
            public void Should_call_make_iteration_for_transport_system()
            {
                var prevIterationResult = new IterationResult
                {
                    AlgorithmType = ChoiceTransportAlgorithmType.Deviation,
                    IterationStep = 1,
                    AverageSatisfaction = 1,
                    Passengers = new PassengerDto[0],
                    AvailableTransportTypes = AvailableTransportTypes
                };
                var allPassengers = new Passenger[0];
                passengersFactory.CreatePassengers(prevIterationResult.AlgorithmType, prevIterationResult.Passengers)
                    .Returns(allPassengers);

                var _ = controller.GetNextStep(prevIterationResult);

                transportSystem.Received().MakeIteration(allPassengers, AvailableTransportTypes);
            }
        }
    }
}