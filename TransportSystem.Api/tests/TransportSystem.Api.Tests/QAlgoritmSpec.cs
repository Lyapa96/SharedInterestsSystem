using FluentAssertions;
using NUnit.Framework;
using TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning;

namespace TransportSystem.Api.Tests
{
    public class QAlgoritmSpec
    {
        [TestCase(1, 10, 1, 4)]
        [TestCase(2, 3, 0.1, 4.4)]
        [TestCase(4, 5, 0.9, 9.4)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 1, 1, 3.1)]
        public void Should_rigth_update_reward(double previousReward, double maxNextReward, double currentReward, double expectedValue)
        {
            var result = QLearningAlgoritm.GetUpdateReward(previousReward, maxNextReward, currentReward);

            result.Should().Be(expectedValue);
        }
    }
}