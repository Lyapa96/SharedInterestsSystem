namespace TransportSystem.Api.Controllers
{
    public class SmoStepResult
    {
        public SmoPassengerInfo[] SmoPassengerInfo { get; set; }
        public double AverageSatisfaction { get; set; }
        public int IterationStep { get; set; }
    }
}