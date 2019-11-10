namespace TransportSystem.Api.Models.Data
{
    public class TransportInitData
    {
        public int PassengersCount { get; set; }
        public int Columns { get; set; }
        public int NeighboursCount { get; set; }
        public TransportType[] AvailableTransportTypes { get; set; }
    }
}