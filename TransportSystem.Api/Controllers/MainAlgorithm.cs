using System.Linq;
using TransportSystem.Api.Models;

namespace TransportSystem.Api.Controllers
{
    public static class MainAlgorithm
    {
        public static void Run(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
                passengers[i][j].ChooseNextTransportType();

            TransportSystem.ChangeQuality(passengers);

            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < columnCount; j++)
                    passengers[i][j].UpdateSatisfaction();
            }
        }
    }
}