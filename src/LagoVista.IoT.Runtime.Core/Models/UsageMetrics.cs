using LagoVista.Core;
using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    /// <summary>
    /// Class used to capture usage information, will be rolled up/consolidated 
    /// by both time and by host/instance.
    /// </summary>
    public class UsageMetrics : TableStorageEntity
    {
        public UsageMetrics(String hostId, String instanceId, string pipelineModuleId)
        {
            if(!String.IsNullOrEmpty(PipelineModuleId))
            {
                PartitionKey = PipelineModuleId;
            } else if(!String.IsNullOrEmpty(InstanceId))
            {
                PartitionKey = InstanceId;
            }
            else if(!String.IsNullOrEmpty(HostId))
            {
                PartitionKey = HostId;
            }

            HostId = HostId;
            InstanceId = instanceId;
            PipelineModuleId = pipelineModuleId;
        }

        public UsageMetrics()
        {

        }

        public String StartTimeStamp { get; set; }
        public String EndTimeStamp { get; set; }
        public double ElapsedMS { get; set; }
        public double MessagesPerSecond { get; set; }
        public double AvergeProcessingMs { get; set; }
        public String HostId { get; set; }
        public String InstanceId { get; set; }
        public String PipelineModuleId { get; set; }
        public int MessagesProcessed { get; set; }
        public int DeadLetterCount { get; set; }
        public long BytesProcessed { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int ActiveCount { get; set; }
        public double ProcessingMS { get; set; }

        public void Reset(String previousEndTime = null)
        {
            StartTimeStamp = previousEndTime == null ? DateTime.UtcNow.ToJSONString() : previousEndTime;
            EndTimeStamp = String.Empty;

            ElapsedMS = 0.0;

            MessagesProcessed = 0;
            BytesProcessed = 0;
            ErrorCount = 0;
            WarningCount = 0;
            ActiveCount = 0;
            ProcessingMS = 0;
            DeadLetterCount = 0;
        }
    }
}
