using System;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Tests
{
    public static class TestHelpers
    {
        public static Passenger CreatePassenger(
            IPassengerBehaviourManager passengerBehaviour,
            int number,
            ChoiceTransportAlgorithmType algorithmType,
            TransportType transport,
            double satisfaction)
        {
            var rnd = new Random();
            var quality = Math.Round(rnd.NextDouble(), 2);
            return new Passenger(passengerBehaviour, transport, algorithmType, quality, satisfaction, number.ToString());
        }
    }
}