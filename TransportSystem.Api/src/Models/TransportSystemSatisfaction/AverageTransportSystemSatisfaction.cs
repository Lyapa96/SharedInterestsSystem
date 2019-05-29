using System;
using System.Linq;
using TransportSystem.Api.Controllers;

namespace TransportSystem.Api.Models.TransportSystemSatisfaction
{
    public class AverageTransportSystemSatisfaction : ITransportSystemSatisfaction
    {
        public double Evaluate(PassengerDto[] allPassengers)
        {
            return Math.Round(allPassengers.Sum(x => x.Satisfaction) / allPassengers.Length, 2);
        }
    }
}