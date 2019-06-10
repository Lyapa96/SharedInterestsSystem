using System.Collections.Generic;

namespace TransportSystem.Api.Models.Data
{
    public class PassengerDto
    {
        public string Id { get; set; }
        public TransportType TransportType { get; set; }
        public double Quality { get; set; }
        public double Satisfaction { get; set; }
        public string[] Neighbours { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public double FirstBusQuality { get; set; }
    }
}