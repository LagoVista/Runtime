using System;
using System.Diagnostics;
using LagoVista.Core;
using Newtonsoft.Json;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExecutionInstruction
    {
        Stopwatch _stopWatch = new Stopwatch();

        public PipelineExecutionInstruction()
        {

        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("queueUri")]
        public string QueueUri { get; set; }
        [JsonProperty("queueId")]
        public string QueueId { get; set; }

        private string _enqueued;
        [JsonProperty("enqueued")]
        public string Enqueued
        {
            get { return _enqueued; }
            set
            {
                _enqueued = value;
                _stopWatch.Start();
            }
        }

        private string _startDateStamp { get; set; }
        [JsonProperty("startDateStamp")]
        public string StartDateStamp
        {
            get { return _startDateStamp; }
            set
            {
                _startDateStamp = value;
                if(_stopWatch.IsRunning)
                {
                    _stopWatch.Stop();
                    TimeInQueueMS = Math.Round(_stopWatch.Elapsed.TotalMilliseconds, 2);
                }
            }
        }

        [JsonProperty("timeInQueueMS")]
        public double TimeInQueueMS { get; set; }

        [JsonProperty("processByHostId")]
        public string ProcessByHostId { get; set; }
        [JsonProperty("executionTimeMS")]
        public double ExecutionTimeMS { get; set; }
    }
}
