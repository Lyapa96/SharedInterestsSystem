using System.Collections.Generic;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning
{
    public class QLearningChoiceTransportAlgorithm : IChoiceTransportAlgorithm
    {
        private readonly IAgentStateStorage stateStorage;

        public QLearningChoiceTransportAlgorithm(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue)
        {
            var currentAgentState = new AgentState(neighbors, currentSatisfaction, currentTransportType).GetStringFormat();
            return stateStorage.GetBestNextTransport(currentAgentState);
        }
    }
}