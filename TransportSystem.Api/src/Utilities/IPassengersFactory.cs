using TransportSystem.Api.Controllers;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms;

namespace TransportSystem.Api.Utilities
{
    public interface IPassengersFactory
    {
        PassengerDto[] CreatePassengers(int columns, int rows);
        PassengerDto[] CreatePassengers(TransportInitData smoData);
        PassengerDto[] CreateAllPassengersTogether(SmoData smoData, TransportType[] availableTransportTypes);
        Passenger[] CreatePassengers(ChoiceTransportAlgorithmType algorithmType, PassengerDto[] passengers);
    }
}