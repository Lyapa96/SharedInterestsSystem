using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage
{
    public interface IAgentStateStorage
    {
        TransportType GetBestNextTransport(string currentAgentState, TransportType[] availableTransportTypes);
        void SaveStateReward(string previousAgentState, string currentAgentState, double reward, TransportType previousAction, TransportType[] availableTransportTypes);
    }
}