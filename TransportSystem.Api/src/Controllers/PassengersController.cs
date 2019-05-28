using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengerBehaviourManager passengerBehaviourManager;
        private readonly INeighboursManager neighboursManager;
        private readonly ITransportSystem transportSystem;

        public PassengersController(
            ITransportSystem transportSystem, 
            IPassengerBehaviourManager passengerBehaviourManager,
            INeighboursManager neighboursManager)
        {
            this.passengerBehaviourManager = passengerBehaviourManager;
            this.neighboursManager = neighboursManager;
            this.transportSystem = transportSystem;
        }

        [HttpPost("set")]
        public IActionResult SetNeighbors([FromBody] PassData data)
        {
            var allPassengers = data.SmoPassengerInfo;
            neighboursManager.SetAllNeighbours(allPassengers, data.Columns, 0);
            var averageSatisfaction = Math.Round(allPassengers.Sum(x => x.Satisfaction)/allPassengers.Length, 2);

            return Ok(
                new SmoStepResult
                {
                    SmoPassengerInfo = allPassengers,
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
            var allPassengers = GetAllPassengers(smoData);

            neighboursManager.SetAllNeighbours(allPassengers, smoData.Columns, smoData.NeighboursCount);

            var averageSatisfaction = Math.Round(allPassengers.Sum(x => x.Satisfaction)/allPassengers.Length, 2);

            return Ok(
                new SmoStepResult
                {
                    SmoPassengerInfo = allPassengers,
                    AverageSatisfaction = averageSatisfaction
                });
        }

        [HttpPost("smoStep")]
        public IActionResult GetNextStep([FromBody] SmoStepResult smoPreviousStepResult)
        {
            const ChoiceTransportAlgorithmType transmissionType = ChoiceTransportAlgorithmType.QLearning;
            var idToPassenger = smoPreviousStepResult.SmoPassengerInfo
                .ToDictionary(
                    x => x.Id,
                    x => new Passenger(
                        passengerBehaviourManager,
                        x.Type,
                        transmissionType,
                        x.Quality,
                        x.Satisfaction,
                        x.Id,
                        x.AllQualityCoefficients,
                        x.FirstBusQuality));

            var allPassengers = SetNeighbours(smoPreviousStepResult.SmoPassengerInfo, idToPassenger).ToArray();

            transportSystem.MakeIteration(allPassengers);

            var averageSatisfaction = Math.Round(allPassengers.Sum(x => x.Satisfaction)/allPassengers.Length, 2);

            var snoPassengerInfo = allPassengers
                .Select(
                    x => new SmoPassengerInfo
                    {
                        Id = x.Id,
                        Neighbours = x.Neighbors.Select(y => y.Id).ToArray(),
                        Satisfaction = x.Satisfaction,
                        Quality = x.QualityCoefficient,
                        Type = x.TransportType,
                        AllQualityCoefficients = x.AllQualityCoefficients
                    })
                .ToArray();

            var result = new SmoStepResult
            {
                SmoPassengerInfo = snoPassengerInfo,
                IterationStep = ++smoPreviousStepResult.IterationStep,
                AverageSatisfaction = averageSatisfaction
            };

            return Ok(result);
        }

        private static SmoPassengerInfo[] GetAllPassengers(SmoData smoData)
        {
            const double defaultSatisfaction = 0.5;
            var smoBusPassengers = smoData.SmoPassengers
                .Select(
                    x => new SmoPassengerInfo
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
                    x => new SmoPassengerInfo
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

        private List<Passenger> SetNeighbours(SmoPassengerInfo[] passengerInfos, Dictionary<string, Passenger> idToPassengers)
        {
            var allPassengers = new List<Passenger>();
            foreach (var smoPassenger in passengerInfos)
            {
                var neighbours = smoPassenger.Neighbours.Select(x => idToPassengers[x]);
                var currentPassenger = idToPassengers[smoPassenger.Id];
                foreach (var neighbour in neighbours)
                {
                    currentPassenger.AddNeighbor(neighbour);
                }

                allPassengers.Add(currentPassenger);
            }

            return allPassengers;
        }
    }
}