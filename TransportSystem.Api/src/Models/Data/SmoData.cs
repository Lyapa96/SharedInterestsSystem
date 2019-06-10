namespace TransportSystem.Api.Models.Data
{
    public class SmoData
    {
        public int PassengersOnCar { get; set; }
        public int Columns { get; set; }
        public int NeighboursCount { get; set; }
        public SmoPassenger[] SmoPassengers { get; set; }
    }
}