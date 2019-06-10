using System.Collections.Generic;

namespace TransportSystem.Api.Models.Data
{
    public class SmoPassenger
    {
        public string AgentId { get; set; }
        public string ArriveAgentTime { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        public int ChannelNumber { get; set; }
        public int EdgeNumber { get; set; }
        public int QueueCount { get; set; }
        public double Quality { get; set; }
        public List<string> Neighbourhood { get; set; }
    }
}