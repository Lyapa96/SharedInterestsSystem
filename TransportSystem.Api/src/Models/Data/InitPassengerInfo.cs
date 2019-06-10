using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Models.Data
{
    public class InitPassengerInfo
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public ChoiceTransportAlgorithmType AlgorithmType { get; set; }
    }
}