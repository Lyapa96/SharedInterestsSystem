using System.Collections.Generic;
using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Utilities
{
    public static class PassengersHelper
    {
        public static void SetNeighborhood(
            this PassengerDto[] passengers,
            Dictionary<string, List<string>> neighborhood)
        {
            foreach (var passenger in passengers)
            {
                passenger.Neighbours = neighborhood[passenger.Id].ToArray();
            }
        }
    }
}