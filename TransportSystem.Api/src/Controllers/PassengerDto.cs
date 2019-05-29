using System.Collections.Generic;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Controllers
{
    public class PassengerDto
    {
        public string Id { get; set; }
        public TransportType Type { get; set; }
        public double Quality { get; set; }
        public double Satisfaction { get; set; }
        public string[] Neighbours { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public double FirstBusQuality { get; set; }
    }
}