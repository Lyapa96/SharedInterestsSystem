using System;

namespace TransportSystem.Api.Models.Neighbors
{
    public class DefaultRandomizer : IRandomizer
    {
        private Random rnd;

        public DefaultRandomizer()
        {
            rnd = new Random();
        }

        public int GetRandomNumber(int min, int max)
        {
            return rnd.Next(min, max);
        }
    }
}