using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExectionInstruction
    {
        public string QueueUri { get; set; }
        public string QueueName { get; set; }

        public string StartDateStamp { get; set; }
        public string ProcessByHostId { get; set; }
        public double ExecutionTimeMS { get; set; }
    }
}
