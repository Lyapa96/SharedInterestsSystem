using System;

namespace TransportSystem.Api.Models.Neighbors
{
    public class DefaultRandomizer : IRandomizer
    {
        private readonly Random rnd;

        public DefaultRandomizer()
        {
            rnd = new Random();
        }

        public int GetRandomNumber(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public double GetRandomDouble()
        {
            return rnd.NextDouble();
        }
    }
}