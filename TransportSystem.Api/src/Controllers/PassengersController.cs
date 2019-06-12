using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbours;
using TransportSystem.Api.Models.PassengerBehaviour;
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
        private readonly IPassengerBehaviourProvider passengerBehaviourProvider;
        private readonly INeighborsManager neighborsManager;
        private readonly ITransportSystemSatisfaction transportSystemSatisfaction;
        private readonly ITransportSystem transportSystem;

        public PassengersController(
            ITransportSystem transportSystem,
            IPassengerBehaviourProvider passengerBehaviourProvider,
            INeighborsManager neighborsManager,
            ITransportSystemSatisfaction transportSystemSatisfaction)
        {
            this.passengerBehaviourProvider = passengerBehaviourProvider;
            this.neighborsManager = neighborsManager;
            this.transportSystemSatisfaction = transportSystemSatisfaction;
            this.transportSystem = transportSystem;
        }

        [HttpPost("init")]
        public IActionResult InitPassengers([FromBody] InitPassengerInfo initPassengerInfo)
        {
            var allPassengers = PassengersHelper.CreatePassengers(initPassengerInfo.Columns, initPassengerInfo.Rows);
            var neighborhood = neighborsManager.GetGeometricNeighborhood(allPassengers, initPassengerInfo.Columns);
            allPassengers.SetNeighborhood(neighborhood);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    AlgorithmType = initPassengerInfo.AlgorithmType,
                    IterationStep = 0,
                    AverageSatisfaction = averageSatisfaction,
                    Passengers = allPassengers
                });
        }

        [HttpPost("initSmo")]
        public IActionResult InitPassengersFromSmo([FromBody] SmoData smoData)
        {
            var allPassengers = PassengersHelper.GetAllPassengersTogether(smoData);
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
            var idToPassenger = previousIterationResult.Passengers
                .ToDictionary(
                    x => x.Id,
                    x => Passenger.Create(x, passengerBehaviourProvider, previousIterationResult.AlgorithmType));
            var allPassengers = PassengersHelper
                .CreatePassengers(previousIterationResult.Passengers, idToPassenger)
                .ToArray();

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