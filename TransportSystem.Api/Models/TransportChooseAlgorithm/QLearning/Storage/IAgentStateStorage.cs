namespace TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage
{
    public interface IAgentStateStorage
    {
        TransportType GetBestNextTransport(string currentAgentState);
        void SaveStateReward(string previousAgentState, string currentAgentState, double reward, TransportType previousAction);
    }
}