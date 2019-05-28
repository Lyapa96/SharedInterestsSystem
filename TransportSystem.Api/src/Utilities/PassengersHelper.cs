using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Models;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Utilities
{
    public static class PassengersHelper
    {
        public static Passenger CreatePassenger(PassengerBehaviourProvider passengerBehaviourProvider, int number,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            var rnd = new Random();
            var transport = GetRandomtransportType();
            var quality = Math.Round(rnd.NextDouble(), 2);
            var satisfaction = Math.Round(rnd.NextDouble(), 2);
            return new Passenger(passengerBehaviourProvider, transport, choiceTransportAlgorithmType, quality, satisfaction, number.ToString());
        }

        public static void SetNeighborsPassengers(Passenger[][] passengers, PassengerBehaviourProvider behaviourProvider)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
            {
                if (i > 0) passengers[i][j].AddNeighbor(passengers[i - 1][j]);
                if (j > 0) passengers[i][j].AddNeighbor(passengers[i][j - 1]);
                if (i < rowCount - 1) passengers[i][j].AddNeighbor(passengers[i + 1][j]);
                if (j < columnCount - 1) passengers[i][j].AddNeighbor(passengers[i][j + 1]);
                passengers[i][j].PassengerBehaviourProvider = behaviourProvider;
            }
        }

        public static void SetDefaultNeighbors(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
            {
                if (i > 0) passengers[i][j].AddNeighbor(passengers[i - 1][j]);
                if (j > 0) passengers[i][j].AddNeighbor(passengers[i][j - 1]);
                if (i < rowCount - 1) passengers[i][j].AddNeighbor(passengers[i + 1][j]);
                if (j < columnCount - 1) passengers[i][j].AddNeighbor(passengers[i][j + 1]);
            }
        }

        public static void ClearNeighborsPassengers(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
                passengers[i][j].Neighbors = new HashSet<Passenger>();
        }

        public static TransportType GetRandomtransportType()
        {
            var rnd = new Random();
            return rnd.Next(2) == 1 ? TransportType.Bus : TransportType.Car;
        }

        public static TransportType GetOtherRandomTransportType(TransportType transportType)
        {
            var randomTransportType = GetRandomtransportType();
            while (transportType == randomTransportType) randomTransportType = GetRandomtransportType();

            return randomTransportType;
        }
    }
}