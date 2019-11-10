using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;
using TransportSystem.Api.Models.System;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportSystemSatisfaction;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly INeighborsManager neighborsManager;
        private readonly IPassengersFactory passengersFactory;
        private readonly ITransportSystem transportSystem;
        private readonly ITransportSystemSatisfaction transportSystemSatisfaction;

        public PassengersController(
            ITransportSystem transportSystem,
            IPassengersFactory passengersFactory,
            INeighborsManager neighborsManager,
            ITransportSystemSatisfaction transportSystemSatisfaction)
        {
            this.neighborsManager = neighborsManager;
            this.passengersFactory = passengersFactory;
            this.transportSystemSatisfaction = transportSystemSatisfaction;
            this.transportSystem = transportSystem;
        }

        /// <summary>
        ///     Initializes the iteration result for a simple system
        /// </summary>
        [HttpPost("init")]
        public IActionResult InitPassengers([FromBody] InitPassengerInfo initPassengerInfo)
        {
            var availableTransportTypes = new[] {TransportType.Bus, TransportType.Car};
            var allPassengers = passengersFactory.CreatePassengers(initPassengerInfo.Columns, initPassengerInfo.Rows);
            var neighborhood = neighborsManager.GetGeometricNeighborhood(allPassengers, initPassengerInfo.Columns);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    AlgorithmType = initPassengerInfo.AlgorithmType,
                    AverageSatisfaction = averageSatisfaction,
                    Passengers = allPassengers,
                    AvailableTransportTypes = availableTransportTypes
                });
        }

        /// <summary>
        ///     Initializes the iteration result for output results form Queueing theory system
        /// </summary>
        [HttpPost("initSmo")]
        public IActionResult InitPassengersFromSmo([FromBody] SmoData smoData)
        {
            var availableTransportTypes = new[] {TransportType.Bus, TransportType.Car};
            var allPassengers = passengersFactory.CreateAllPassengersTogether(smoData, availableTransportTypes);
            var neighborhood =
                neighborsManager.GetEachPassengerNeighbors(smoData.NeighboursCount, smoData.Columns, allPassengers);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    Passengers = allPassengers,
                    AverageSatisfaction = averageSatisfaction,
                    AlgorithmType = ChoiceTransportAlgorithmType.QLearning,
                    AvailableTransportTypes = availableTransportTypes
                });
        }

        [HttpPost("initWithTransport")]
        public IActionResult InitPassengers([FromBody] TransportInitData transportInitData)
        {
            var allPassengers = passengersFactory.CreatePassengers(transportInitData);
            var neighborhood = neighborsManager.GetEachPassengerNeighbors(transportInitData.NeighboursCount,
                transportInitData.Columns, allPassengers);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    Passengers = allPassengers,
                    AverageSatisfaction = averageSatisfaction,
                    AlgorithmType = ChoiceTransportAlgorithmType.QLearning,
                    AvailableTransportTypes = transportInitData.AvailableTransportTypes
                });
        }

        /// <summary>
        ///     Emulates iteration of the transport system and returns iteration result
        /// </summary>
        [HttpPost("nextStep")]
        public IActionResult GetNextStep([FromBody] IterationResult previousIterationResult)
        {
            var nextStep = GetNextIterationResult(previousIterationResult);
            return Ok(nextStep);
        }

        /// <summary>
        ///     Emulates several iterations of the transport system and returns all iterations results
        /// </summary>
        [HttpPost("nextSteps")]
        public IActionResult GetNextSteps([FromBody] IterationResult previousIterationResult,
            [FromQuery] int countSteps = 100)
        {
            var result = new List<IterationResult>();
            var currentStep = previousIterationResult;
            for (var i = 0; i < countSteps; i++)
            {
                var nextStep = GetNextIterationResult(currentStep);
                result.Add(nextStep);
                currentStep = nextStep;
            }

            return Ok(result);
        }

        /// <summary>
        ///     Health check method
        /// </summary>
        [HttpGet("ping")]
        public ActionResult<string> Get() => "pong";

        private IterationResult GetNextIterationResult(IterationResult previousIterationResult)
        {
            var allPassengers = passengersFactory
                .CreatePassengers(previousIterationResult.AlgorithmType, previousIterationResult.Passengers);

            transportSystem.MakeIteration(allPassengers, previousIterationResult.AvailableTransportTypes);

            var passengerDtos = allPassengers
                .Select(x => x.ToPassengerDto())
                .ToArray();

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(passengerDtos);
            var nextStep = previousIterationResult.Next(passengerDtos, averageSatisfaction);
            return nextStep;
        }
    }
}