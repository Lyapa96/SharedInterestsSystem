using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Controllers
{
    public class IterationResult
    {
        public PassengerDto[] Passengers { get; set; }
        public double AverageSatisfaction { get; set; }
        public int IterationStep { get; set; }
        public ChoiceTransportAlgorithmType AlgorithmType { get; set; }

        public IterationResult Next(PassengerDto[] passengers, double averageSatisfaction)
        {
            return new IterationResult
            {
                IterationStep = IterationStep+1,
                AlgorithmType = AlgorithmType,
                AverageSatisfaction = averageSatisfaction,
                Passengers = passengers,
            };
        }
    }
}