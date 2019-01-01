using System;
using System.Diagnostics;
using LagoVista.Core;
using Newtonsoft.Json;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExecutionInstruction
    {
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
                if(!String.IsNullOrEmpty(Enqueued) && !String.IsNullOrEmpty(value))
                {
                    TimeInQueueMS = Math.Round((value.ToDateTime() - Enqueued.ToDateTime()).TotalMilliseconds, 3);
                }
            }
        }

        [JsonProperty("timeInQueueMS")]
        public double TimeInQueueMS { get; set; }

        [JsonProperty("processByHostId")]
        public string ProcessByHostId { get; set; }
        [JsonProperty("executionTimeMS")]
        public double ExecutionTimeMS { get; set; }

        public PipelineExecutionInstruction Clone()
        {
            return new PipelineExecutionInstruction()
            {
                Enqueued = Enqueued,
                ExecutionTimeMS = ExecutionTimeMS,
                Name = Name,
                ProcessByHostId = ProcessByHostId,
                QueueId = QueueId,
                QueueUri = QueueUri,
                Type = Type,
                TimeInQueueMS = TimeInQueueMS,
                StartDateStamp = StartDateStamp
            };
        }
    }
}
