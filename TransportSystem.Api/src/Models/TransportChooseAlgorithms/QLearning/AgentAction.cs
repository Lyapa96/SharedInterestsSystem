using TransportSystem.Api.Models.Data;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning
{
    public class AgentAction
    {
        public AgentAction()
        {
        }

        public AgentAction(TransportType transport, int choicesCount, double totalReward, double averageReward)
        {
            Transport = transport.ToString();
            ChoicesCount = choicesCount;
            TotalReward = totalReward;
            AverageReward = averageReward;
        }

        public string Transport { get; set; }
        public int ChoicesCount { get; set; }
        public double TotalReward { get; set; }
        public double AverageReward { get; set; }

        public void AddReward(double reward)
        {
            TotalReward += reward;
            ChoicesCount++;
            AverageReward = TotalReward/ChoicesCount;
        }
    }
}