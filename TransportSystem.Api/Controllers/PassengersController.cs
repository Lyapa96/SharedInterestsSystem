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
                .Select(x => x
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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
//        [HttpPost]
//        public ActionResult<string> Post([FromBody] object value)
//        {
//            return Guid.NewGuid().ToString();
//        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public static class MainAlgorithm
        {
            public static void Run(Passenger[][] passengers)
            {
                var rowCount = passengers.Length;
                var columnCount = passengers.First().Length;
                for (var i = 0; i < rowCount; i++)
                for (var j = 0; j < columnCount; j++)
                    passengers[i][j].ChooseNextTransportType();

                TransportSystem.ChangeQuality(passengers);

                for (var i = 0; i < rowCount; i++)
                for (var j = 0; j < columnCount; j++)
                    passengers[i][j].UpdateSatisfaction();
            }
        }
    }


    public class PassengersData
    {
        public PassengerInfo[][] Passengers { get; set; }
        public TransmissionType AlgorithmType { get; set; }
    }

    public class PassengerInfo
    {
        public int Number { get; set; }
        public double Satisfaction { get; set; }
        public double Quality { get; set; }
        public TransportType TransportType { get; set; }
        public PassengerCoordinates Coordinates { get; set; }
        public List<int> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
    }

    public class PassengerCoordinates
    {
        public int I { get; set; }
        public int J { get; set; }
    }

    public static class TransportSystem
    {
        public static void ChangeQuality(Passenger[][] passengers)
        {
            var carCount = passengers.Sum(x => x.Count(y => y.TransportType == TransportType.Car));
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            var passengersCount = rowCount * columnCount;

            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
            {
                var passenger = passengers[i][j];
                passenger.QualityCoefficient = passenger.TransportType == TransportType.Car
                    ? GetQualityCoefficientForCar(carCount, passengersCount, passenger)
                    : GetQualityCoefficientForBus(passenger);
            }
        }

        private static double GetQualityCoefficientForBus(Passenger passenger)
        {
            if (passenger.Number <= 3)
                return 0.3;
            if (passenger.Number <= 6)
                return 0.4;

            return 0.5;
        }

        private static double GetQualityCoefficientForCar(int carCount, int n, Passenger passenger)
        {
            if (passenger.Neighbors.Count(x => x.TransportType == TransportType.Car) < 2)
            {
                var answer = 1 - (double) carCount / n;
                return answer;
            }

            return 0.1;
        }
    }
}