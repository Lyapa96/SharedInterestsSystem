using System.Collections.Generic;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public class QLearningChoiceTransportAlgorithm : IChoiceTransportAlgorithm
    {
        private readonly IAgentStateStorage stateStorage;

        public QLearningChoiceTransportAlgorithm(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public TransportType ChooseNextTransportType(HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue, 
            TransportType[] availableTransportTypes)
        {
            var currentAgentState = new AgentState(neighbors, currentSatisfaction, currentTransportType, availableTransportTypes).GetStringFormat();
            return stateStorage.GetBestNextTransport(currentAgentState, availableTransportTypes);
        }
    }
}