using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.TransportChooseAlgorithm;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage;
using TransportSystem.Api.Utilities;

namespace TransportSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerBehaviour passengerBehaviour;

        public PassengersController()
        {
            passengerBehaviour = new PassengerBehaviour(new MemoryStorage());
        }

        [HttpPost]
        public IActionResult GetNextStep([FromBody] PassengersData data)
        {
            var rowCount = data.Passengers.Length;
            var columnCount = data.Passengers[0].Length;
            var passengers = new Passenger[rowCount][];
            for (var i = 0; i < rowCount; i++)
            {
                passengers[i] = new Passenger[columnCount];
                for (var j = 0; j < columnCount; j++)
                {
                    var passengerInfo = data.Passengers[i][j];
                    passengers[i][j] = new Passenger(
                        passengerBehaviour,
                        passengerInfo,
                        data.AlgorithmType);
                }
            }

            PassengersHelper.SetDefaultNeighbors(passengers);
            MainAlgorithm.Run(passengers);

            var newData = passengers
                .Select(
                    x => x
                        .Select(y => y.GetPassengersInfo())
                        .ToArray())
                .ToArray();
            data.Passengers = newData;

            return Ok(data);
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
            const double defaultSatisfaction = 0.5;
            var random = new Random();
            var smoBusPassengers = smoData.SmoPassengers
                .Select(x => new SmoPassengerInfo
                {
                    Id = x.AgentId,
                    Quality = x.Quality,
                    Type = TransportType.Bus,
                    Satisfaction = defaultSatisfaction,
                    FirstBusQuality = x.Quality
                }).ToArray();
            var carPassengers = Enumerable.Range(0, smoData.PassengersOnCar)
                .Select(
                    x => new SmoPassengerInfo
                    {
                        Id = $"car.{x}",
                        Quality = 1 - (double)smoData.PassengersOnCar /(smoData.PassengersOnCar + smoBusPassengers.Length),
                        Type = TransportType.Car,
                        Satisfaction = defaultSatisfaction,
                        FirstBusQuality = 0
                    });
            var allPassengers = smoBusPassengers.Concat(carPassengers).ToArray();
            var manager = new NeighboursManager(allPassengers, smoData.Columns, smoData.NeighboursCount);
            
            for (var i = 0; i < allPassengers.Length; i++)
            {
                var currentSmoPassenger = allPassengers[i];
                currentSmoPassenger.Neighbours = manager.GetNeighboursFor(currentSmoPassenger.Id, i).ToArray();
            }

            var averageSatisfaction = Math.Round(allPassengers.Sum(x => x.Satisfaction)/allPassengers.Length, 2);
            var result = new SmoStepResult
            {
                SmoPassengerInfo = allPassengers,
                IterationStep = 0,
                AverageSatisfaction = averageSatisfaction
            };

            return Ok(result);
        }

        [HttpPost("smoStep")]
        public IActionResult GetNextStep([FromBody] SmoStepResult smoPreviousStepResult)
        {
            const TransmissionType transmissionType = TransmissionType.QLearning;
            var idToPassenger = smoPreviousStepResult.SmoPassengerInfo
                .ToDictionary(x => x.Id,
                    x => new Passenger(
                        passengerBehaviour,
                        x.Type,
                        transmissionType,
                        x.Quality,
                        x.Satisfaction,
                        x.Id,
                        x.AllQualityCoefficients,
                        x.FirstBusQuality));

            var allPassengers = SetNeighbours(smoPreviousStepResult.SmoPassengerInfo, idToPassenger).ToArray();
            MainAlgorithm.Run(allPassengers);

            var averageSatisfaction = Math.Round(allPassengers.Sum(x => x.Satisfaction) / allPassengers.Length, 2);

            var snoPassengerInfo = allPassengers
                .Select(x => new SmoPassengerInfo
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

    public class SmoStepResult
    {
        public SmoPassengerInfo[] SmoPassengerInfo { get; set; }
        public double AverageSatisfaction { get; set; }
        public int IterationStep { get; set; }
    }

    public class SmoPassengerInfo
    {
        public string Id { get; set; }
        public TransportType Type { get; set; }
        public double Quality { get; set; }
        public double Satisfaction { get; set; }
        public string[] Neighbours { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public double FirstBusQuality { get; set; }
    }

    public class SmoData
    {
        public int PassengersOnCar { get; set; }
        public int Columns { get; set; }
        public int NeighboursCount { get; set; }
        public SmoPassenger[] SmoPassengers { get; set; }
    }

    public class SmoPassenger
    {
        public string AgentId { get; set; }
        public string ArriveAgentTime { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        public int ChannelNumber { get; set; }
        public int EdgeNumber { get; set; }
        public int QueueCount { get; set; }
        public double Quality { get; set; }
        public List<string> Neighbourhood { get; set; }
    }
}