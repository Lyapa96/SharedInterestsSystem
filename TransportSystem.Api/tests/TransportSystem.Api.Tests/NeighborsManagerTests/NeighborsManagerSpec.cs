using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TransportSystem.Api.Models.Data;
using TransportSystem.Api.Models.Neighbors;

namespace TransportSystem.Api.Tests.NeighborsManagerTests
{
    public class NeighborsManagerSpec
    {
        private IRandomizer randomizer;
        private NeighborsManager manager;

        [SetUp]
        public void SetUp()
        {
            randomizer = Substitute.For<IRandomizer>();
            manager = new NeighborsManager(randomizer);
        }

        [Test]
        public void Should_return_geometric_neighborhood_with_two_neighbors()
        {
            const int columns = 3;
            var passengers = CreatePassengers(columns*columns);

            var result = manager.GetGeometricNeighborhood(passengers, columns);

            result["1"].Should().BeEquivalentTo(new List<string> {"2", "4"});
            result["3"].Should().BeEquivalentTo(new List<string> {"2", "6"});
            result["7"].Should().BeEquivalentTo(new List<string> {"4", "8"});
            result["9"].Should().BeEquivalentTo(new List<string> {"6", "8"});
        }

        [Test]
        public void Should_return_geometric_neighborhood_with_four_neighbors()
        {
            const int columns = 3;
            var passengers = CreatePassengers(columns*columns);

            var result = manager.GetGeometricNeighborhood(passengers, columns);

            result["5"].Should().BeEquivalentTo(new List<string> {"2", "4", "6", "8"});
        }

        [Test]
        public void Should_return_geometric_neighborhood_with_three_neighbors()
        {
            const int columns = 3;
            var passengers = CreatePassengers(columns*columns);

            var result = manager.GetGeometricNeighborhood(passengers, columns);

            result["2"].Should().BeEquivalentTo(new List<string> {"1", "5", "3"});
            result["4"].Should().BeEquivalentTo(new List<string> {"1", "5", "7"});
            result["6"].Should().BeEquivalentTo(new List<string> {"3", "5", "9"});
            result["8"].Should().BeEquivalentTo(new List<string> {"7", "5", "9"});
        }

        [Test]
        public void Should_create_neighborhood_when_set_neighbors_count_for_each_passengers()
        {
            const int columns = 2;
            var passengers = CreatePassengers(columns*columns);
            randomizer.GetRandomNumber(Arg.Any<int>(), Arg.Any<int>())
                .Returns(0);

            var result = manager.GetEachPassengerNeighbors(4, columns, passengers);

            result["1"].Should().BeEquivalentTo(new List<string> {"2", "3", "4"});
            result["2"].Should().BeEquivalentTo(new List<string> {"1", "3", "4"});
            result["3"].Should().BeEquivalentTo(new List<string> {"1", "2", "4"});
            result["4"].Should().BeEquivalentTo(new List<string> {"1", "2", "3"});
        }

        private static PassengerDto[] CreatePassengers(int count)
        {
            return Enumerable.Range(0, count)
                .Select(x => new PassengerDto {Id = (x + 1).ToString()})
                .ToArray();
        }
    }
}