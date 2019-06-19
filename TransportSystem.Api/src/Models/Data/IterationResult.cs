using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Models.Data
{
    public class IterationResult
    {
        public IterationResult()
        {
        }

        public IterationResult(PassengerDto[] passengers, double averageSatisfaction)
        {
            Passengers = passengers;
            AverageSatisfaction = averageSatisfaction;
        }

        public PassengerDto[] Passengers { get; set; }
        public double AverageSatisfaction { get; set; }
        public int IterationStep { get; set; }
        public ChoiceTransportAlgorithmType AlgorithmType { get; set; }

        public IterationResult Next(PassengerDto[] passengers, double averageSatisfaction)
        {
            return new IterationResult
            {
                IterationStep = IterationStep + 1,
                AlgorithmType = AlgorithmType,
                AverageSatisfaction = averageSatisfaction,
                Passengers = passengers
            };
        }
    }
}