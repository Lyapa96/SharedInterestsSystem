using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.SatisfactionDetermination
{
    public interface ISatisfactionDeterminationAlgorithm
    {
        double GetSatisfaction(Passenger passenger);
    }
}