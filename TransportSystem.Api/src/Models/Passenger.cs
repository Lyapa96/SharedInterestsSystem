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
            IPassengerBehaviourManager passengerBehaviourManager,
            PassengerInfo passengerInfo,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType)
        {
            this.passengerInfo = passengerInfo;
            PassengerBehaviourManager = passengerBehaviourManager;
            ChoiceTransportAlgorithmType = choiceTransportAlgorithmType;
            Id = passengerInfo.Number;
            TransportType = passengerInfo.TransportType;
            QualityCoefficient = passengerInfo.Quality;
            Satisfaction = passengerInfo.Satisfaction;
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = passengerInfo.AllQualityCoefficients;
        }

        public Passenger(
            IPassengerBehaviourManager passengerBehaviourManager,
            TransportType transportType,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType,
            double qualityCoefficient,
            double satisfaction,
            string id)
        {
            PassengerBehaviourManager = passengerBehaviourManager;
            ChoiceTransportAlgorithmType = choiceTransportAlgorithmType;
            Id = id;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
        }

        public Passenger(
            IPassengerBehaviourManager passengerBehaviourManager,
            TransportType transportType,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType,
            double qualityCoefficient,
            double satisfaction,
            string id,
            List<double> allQualityCoefficients,
            double firstBusQuality)
        {
            PassengerBehaviourManager = passengerBehaviourManager;
            ChoiceTransportAlgorithmType = choiceTransportAlgorithmType;
            Id = id;
            FirstBusQuality = firstBusQuality;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = allQualityCoefficients ?? new List<double>();
        }

        public Passenger()
        {
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = new List<double>();
        }

        public IPassengerBehaviourManager PassengerBehaviourManager { get; set; }

        public double PersonalSatisfaction => 0.1;
        public string Id { get; set; }
        public double FirstBusQuality { get; }
        public TransportType TransportType { get; set; }
        public double QualityCoefficient { get; set; }
        public double Satisfaction { get; set; }
        public HashSet<Passenger> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public ChoiceTransportAlgorithmType ChoiceTransportAlgorithmType { get; set; }
        public double DeviationValue { get; set; }
        public string PreviousState { get; set; }

        public void AddNeighbor(Passenger neighbor)
        {
            Neighbors.Add(neighbor);
        }

        public void ChooseNextTransportType()
        {
            PreviousState = new AgentState(Neighbors, Satisfaction, TransportType).GetStringFormat();
            TransportType = PassengerBehaviourManager
                .GetTransmissionFunc(ChoiceTransportAlgorithmType)
                .ChooseNextTransportType(Neighbors, TransportType, Satisfaction, DeviationValue);
        }

        public void UpdateSatisfaction()
        {
            Satisfaction = Math.Round(
                PassengerBehaviourManager
                    .GetSatisfactionDeterminationAlgorithm(ChoiceTransportAlgorithmType)
                    .GetSatisfaction(this),
                2);
            AllQualityCoefficients.Add(QualityCoefficient);
        }

        public override string ToString()
        {
            //var allNeighbors = Neighbors.Select(x => x.Id.ToString()).Aggregate((x, y) => x + "," + y);
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