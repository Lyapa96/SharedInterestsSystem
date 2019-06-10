namespace TransportSystem.Api.Models.Neighbours
{
    public interface IRandomizer
    {
        int GetRandomNumber(int min, int max);
    }
}