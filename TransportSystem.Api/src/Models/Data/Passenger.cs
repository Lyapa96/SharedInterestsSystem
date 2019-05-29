using System;
using System.Collections.Generic;
using System.Linq;
using TransportSystem.Api.Controllers;
using TransportSystem.Api.Models.PassengerBehaviour;
using TransportSystem.Api.Models.TransportChooseAlgorithms;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning;

namespace TransportSystem.Api.Models.Data
{
    public class Passenger
    {
        public static Passenger Create(PassengerDto x, IPassengerBehaviourProvider passengerBehaviourProvider, ChoiceTransportAlgorithmType algorithmType)
        {
            return new Passenger(
                passengerBehaviourProvider,
                x.Type,
                algorithmType,
                x.Quality,
                x.Satisfaction,
                x.Id,
                x.AllQualityCoefficients,
                x.FirstBusQuality);
        }

        public Passenger(
            IPassengerBehaviourProvider passengerBehaviourProvider,
            TransportType transportType,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType,
            double qualityCoefficient,
            double satisfaction,
            string id)
        {
            PassengerBehaviourProvider = passengerBehaviourProvider;
            ChoiceTransportAlgorithmType = choiceTransportAlgorithmType;
            Id = id;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
        }

        public Passenger(
            IPassengerBehaviourProvider passengerBehaviourProvider,
            TransportType transportType,
            ChoiceTransportAlgorithmType choiceTransportAlgorithmType,
            double qualityCoefficient,
            double satisfaction,
            string id,
            List<double> allQualityCoefficients,
            double firstBusQuality)
        {
            PassengerBehaviourProvider = passengerBehaviourProvider;
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

        public IPassengerBehaviourProvider PassengerBehaviourProvider { get; set; }

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
            TransportType = PassengerBehaviourProvider
                .GetChoiceTransportAlgorithm(ChoiceTransportAlgorithmType)
                .ChooseNextTransportType(Neighbors, TransportType, Satisfaction, DeviationValue);
        }

        public void UpdateSatisfaction()
        {
            Satisfaction = Math.Round(
                PassengerBehaviourProvider
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

        public PassengerDto ToPassengerDto()
        {
            return new PassengerDto
            {
                Id = Id,
                Neighbours = Neighbors.Select(y => y.Id).ToArray(),
                Satisfaction = Satisfaction,
                Quality = QualityCoefficient,
                Type = TransportType,
                AllQualityCoefficients = AllQualityCoefficients,
                FirstBusQuality = FirstBusQuality
            };
        }
    }
}