using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Remotion.Linq.Clauses.ResultOperators;
using TransportSystem.Api.Models;
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

        [HttpPost("smo")]
        public IActionResult GetNextSmoResult([FromBody] SmoPassenger[] smoPassengers)
        {
            
            var columns = 5;
            var rows = 10;
            //всего соседей должно быть 7 или + 3 случайных
            var countNeighbours = 7;
            //придумать что то с геометрией 
            // или сделать возможность 
            var hashSet = new HashSet<(int id, int sum)>(smoPassengers.Select(x => (tro: int.Parse(x.AgentId),10)));
             var first = hashSet.First();
            var allPassengers = new List<Passenger>();
            for (int i = 0; i < smoPassengers.Length; i++)
            {
                var currentSmoPassenger = smoPassengers[i];
                currentSmoPassenger.Neighbourhood = currentSmoPassenger.Neighbourhood ?? new List<string>();

                
            }

            foreach (var smoPassenger in smoPassengers)
            {
            }

            return Ok();
        }

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
        public List<string> Neighbourhood { get; set; }
    }
}