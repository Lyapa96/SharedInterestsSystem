using System;
using System.Collections.Generic;
using TransportSystem.Api.Controllers;
using TransportSystem.Api.Models.TransportChooseAlgorithm;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning;

namespace TransportSystem.Api.Models
{
    public class Passenger
    {
        private readonly PassengerInfo passengerInfo;

        public Passenger(
            IPassengerBehaviour passengerBehaviour,
            PassengerInfo passengerInfo,
            TransmissionType transmissionType)
        {
            this.passengerInfo = passengerInfo;
            PassengerBehaviour = passengerBehaviour;
            TransmissionType = transmissionType;
            Number = passengerInfo.Number;
            TransportType = passengerInfo.TransportType;
            QualityCoefficient = passengerInfo.Quality;
            Satisfaction = passengerInfo.Satisfaction;
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = passengerInfo.AllQualityCoefficients;
        }

        public Passenger(
            IPassengerBehaviour passengerBehaviour,
            TransportType transportType,
            TransmissionType transmissionType,
            double qualityCoefficient,
            double satisfaction,
            int number)
        {
            PassengerBehaviour = passengerBehaviour;
            TransmissionType = transmissionType;
            Number = number;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
        }

        public Passenger()
        {
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = new List<double>();
        }

        public IPassengerBehaviour PassengerBehaviour { get; set; }

        public double PersonalSatisfaction => 0.1;
        public int Number { get; set; }
        public TransportType TransportType { get; set; }
        public double QualityCoefficient { get; set; }
        public double Satisfaction { get; set; }
        public HashSet<Passenger> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public double DeviationValue { get; set; }
        public string PreviousState { get; set; }

        public void AddNeighbor(Passenger neighbor)
        {
            Neighbors.Add(neighbor);
        }

        public void ChooseNextTransportType()
        {
            PreviousState = new AgentState(Neighbors, Satisfaction, TransportType).GetStringFormat();
            TransportType = PassengerBehaviour
                .GetTransmissionFunc(TransmissionType)
                .ChooseNextTransportType(Neighbors, TransportType, Satisfaction, DeviationValue);
        }

        public void UpdateSatisfaction()
        {
            Satisfaction = Math.Round(
                PassengerBehaviour
                    .GetSatisfactionDeterminationAlgorithm(TransmissionType)
                    .GetSatisfaction(this),
                2);
            AllQualityCoefficients.Add(QualityCoefficient);
        }

        public override string ToString()
        {
            //var allNeighbors = Neighbors.Select(x => x.Number.ToString()).Aggregate((x, y) => x + "," + y);
            return $"{TransportType} k=({QualityCoefficient:0.00}) S=({Satisfaction:0.00})";
        }

        public PassengerInfo GetPassengersInfo()
        {
            passengerInfo.Quality = QualityCoefficient;
            passengerInfo.Satisfaction = Satisfaction;
            passengerInfo.TransportType = TransportType;

            return passengerInfo;
        }
    }
}