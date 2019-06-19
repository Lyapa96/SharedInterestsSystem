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
        private readonly IPassengersFactory passengersFactory;
        private readonly INeighborsManager neighborsManager;
        private readonly ITransportSystemSatisfaction transportSystemSatisfaction;
        private readonly ITransportSystem transportSystem;

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

        [HttpPost("init")]
        public IActionResult InitPassengers([FromBody] InitPassengerInfo initPassengerInfo)
        {
            var allPassengers = passengersFactory.CreatePassengers(initPassengerInfo.Columns, initPassengerInfo.Rows);
            var neighborhood = neighborsManager.GetGeometricNeighborhood(allPassengers, initPassengerInfo.Columns);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    AlgorithmType = initPassengerInfo.AlgorithmType,
                    AverageSatisfaction = averageSatisfaction,
                    Passengers = allPassengers
                });
        }

        [HttpPost("initSmo")]
        public IActionResult InitPassengersFromSmo([FromBody] SmoData smoData)
        {
            var allPassengers = passengersFactory.CreateAllPassengersTogether(smoData);
            var neighborhood = neighborsManager.GetEachPassengerNeighbors(smoData.NeighboursCount, smoData.Columns, allPassengers);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    Passengers = allPassengers,
                    AverageSatisfaction = averageSatisfaction,
                    AlgorithmType = ChoiceTransportAlgorithmType.QLearning
                });
        }

        [HttpPost("nextStep")]
        public IActionResult GetNextStep([FromBody] IterationResult previousIterationResult)
        {
            var allPassengers = passengersFactory
                .CreatePassengers(previousIterationResult.AlgorithmType, previousIterationResult.Passengers);

            transportSystem.MakeIteration(allPassengers);

            var passengerDtos = allPassengers
                .Select(x => x.ToPassengerDto())
                .ToArray();

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(passengerDtos);

            return Ok(previousIterationResult.Next(passengerDtos, averageSatisfaction));
        }

        [HttpGet("ping")]
        public ActionResult<string> Get()
        {
            return "pong";
        }
    }
}