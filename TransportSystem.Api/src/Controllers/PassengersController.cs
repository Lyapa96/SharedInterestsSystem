using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models;
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
        private readonly INeighboursManager neighboursManager;
        private readonly ITransportSystemSatisfaction transportSystemSatisfaction;
        private readonly ITransportSystem transportSystem;

        public PassengersController(
            ITransportSystem transportSystem,
            IPassengerBehaviourProvider passengerBehaviourProvider,
            INeighboursManager neighboursManager,
            ITransportSystemSatisfaction transportSystemSatisfaction)
        {
            this.passengerBehaviourProvider = passengerBehaviourProvider;
            this.neighboursManager = neighboursManager;
            this.transportSystemSatisfaction = transportSystemSatisfaction;
            this.transportSystem = transportSystem;
        }

        [HttpPost("set")]
        public IActionResult SetNeighbors([FromBody] PassData data)
        {
            var allPassengers = data.Passengers;
            neighboursManager.SetAllNeighbours(allPassengers, data.Columns, 0);
            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    Passengers = allPassengers,
                    AverageSatisfaction = averageSatisfaction
                });
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] {"value1", "value2"};
        }

        [HttpPost("smo")]
        public IActionResult GetNeighbours([FromBody] SmoData smoData)
        {
            var allPassengers = GetAllPassengersTogether(smoData);

            neighboursManager.SetAllNeighbours(allPassengers, smoData.Columns, smoData.NeighboursCount);

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(allPassengers);

            return Ok(
                new IterationResult
                {
                    Passengers = allPassengers,
                    AverageSatisfaction = averageSatisfaction,
                    AlgorithmType = ChoiceTransportAlgorithmType.QLearning
                });
        }

        [HttpPost("smoStep")]
        public IActionResult GetNextStep([FromBody] IterationResult previousIterationResult)
        {
            var idToPassenger = previousIterationResult.Passengers
                .ToDictionary(
                    x => x.Id,
                    x => Passenger.Create(x, passengerBehaviourProvider, previousIterationResult.AlgorithmType));
            var allPassengers = PassengersHelper.SetNeighbours(previousIterationResult.Passengers, idToPassenger).ToArray();

            transportSystem.MakeIteration(allPassengers);

            var snoPassengerInfo = allPassengers
                .Select(x => x.ToPassengerDto())
                .ToArray();

            var averageSatisfaction = transportSystemSatisfaction.Evaluate(snoPassengerInfo);

            return Ok(previousIterationResult.Next(snoPassengerInfo, averageSatisfaction));
        }

        private static PassengerDto[] GetAllPassengersTogether(SmoData smoData)
        {
            const double defaultSatisfaction = 0.5;
            var smoBusPassengers = smoData.SmoPassengers
                .Select(
                    x => new PassengerDto
                    {
                        Id = x.AgentId,
                        Quality = x.Quality,
                        Type = TransportType.Bus,
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
                        Type = TransportType.Car,
                        Satisfaction = defaultSatisfaction,
                        FirstBusQuality = 0
                    });

            var allPassengers = smoBusPassengers.Concat(carPassengers).ToArray();
            return allPassengers;
        }
    }
}