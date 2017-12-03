using System;
using LagoVista.Core;
    

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class PipelineExecutionInstruction
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string QueueUri { get; set; }
        public string QueueId { get; set; }
        public string Enqueued { get; set; }

        private string _startDateStamp { get; set; }
        public string StartDateStamp
        {
            get { return _startDateStamp; }
            set
            {
                _startDateStamp = value;
                if(!String.IsNullOrEmpty(Enqueued))
                {
                    TimeInQueueMS = Math.Round((Enqueued.ToDateTime() - value.ToDateTime()).TotalMilliseconds, 2);
                }
            }
        }

        public double TimeInQueueMS { get; set; }

        public string ProcessByHostId { get; set; }
        public double ExecutionTimeMS { get; set; }
    }
}
