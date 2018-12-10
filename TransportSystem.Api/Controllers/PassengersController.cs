using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.TransportChooseAlgorithm;
using TransportType = TransportSystem.Api.Models.TransportType;

namespace TransportSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetNextStep([FromBody] PassengersData data)
        {
            
            return Ok(data);
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
    }

    public class PassengerCoordinates
    {
        public int I { get; set; }
        public int J { get; set; }
    }
}
