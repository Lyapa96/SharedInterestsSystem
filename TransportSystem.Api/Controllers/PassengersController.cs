using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    }
}