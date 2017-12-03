using System;
using LagoVista.Core;
using Newtonsoft.Json;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExecutionInstruction
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("queueUri")]
        public string QueueUri { get; set; }
        [JsonProperty("queueId")]
        public string QueueId { get; set; }
        [JsonProperty("enqueued")]
        public string Enqueued { get; set; }

        private string _startDateStamp { get; set; }
        [JsonProperty("startDateStamp")]
        public string StartDateStamp
        {
            get { return _startDateStamp; }
            set
            {
                _startDateStamp = value;
                if (!String.IsNullOrEmpty(Enqueued))
                {
                    TimeInQueueMS = Math.Round((value.ToDateTime() - Enqueued.ToDateTime()).TotalMilliseconds, 2);
                }
            }
        }

        [JsonProperty("timeInQueuemMS")]
        public double TimeInQueueMS { get; set; }

        [JsonProperty("processByHostId")]
        public string ProcessByHostId { get; set; }
        [JsonProperty("executionTimeMS")]
        public double ExecutionTimeMS { get; set; }
    }
}
