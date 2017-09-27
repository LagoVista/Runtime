using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExecutionInstruction
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string QueueUri { get; set; }
        public string QueueId { get; set; }

        public string StartDateStamp { get; set; }
        public string ProcessByHostId { get; set; }
        public double ExecutionTimeMS { get; set; }
    }
}
