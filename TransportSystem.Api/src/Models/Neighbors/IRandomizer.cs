namespace TransportSystem.Api.Models.Neighbors
{
    public interface IRandomizer
    {
        int GetRandomNumber(int min, int max);
        double GetRandomDouble();
    }
}