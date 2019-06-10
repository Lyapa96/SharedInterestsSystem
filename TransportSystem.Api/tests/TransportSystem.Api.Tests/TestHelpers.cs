using System;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Tests
{
    public static class TestHelpers
    {
        public static Passenger CreatePassenger(
            IPassengerBehaviourProvider passengerBehaviour,
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