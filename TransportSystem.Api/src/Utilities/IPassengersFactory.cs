using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Utilities
{
    public interface IPassengersFactory
    {
        PassengerDto[] CreatePassengers(int columns, int rows);
        PassengerDto[] CreateAllPassengersTogether(SmoData smoData);
        Passenger[] CreatePassengers(ChoiceTransportAlgorithmType algorithmType, PassengerDto[] passengers);
    }
}