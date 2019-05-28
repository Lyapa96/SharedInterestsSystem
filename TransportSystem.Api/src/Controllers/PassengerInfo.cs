﻿using System.Collections.Generic;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Controllers
{
    public class PassengerInfo
    {
        public string Number { get; set; }
        public TransportType TransportType { get; set; }
        public double Quality { get; set; }
        public double Satisfaction { get; set; }
        public List<int> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
    }
}