using TransportSystem.Api.Models.TransportChooseAlgorithm;

namespace TransportSystem.Api.Controllers
{
    public class PassengersData
    {
        public PassengerInfo[][] Passengers { get; set; }
        public TransmissionType AlgorithmType { get; set; }
    }
}