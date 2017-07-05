using LagoVista.Core.Models;
using System;
using LagoVista.Core;

namespace LagoVista.IoT.Runtime.Core.Models
{
    public class ComputeResourceMetrics : TableStorageEntity
    {
        public static ComputeResourceMetrics Create(String hostId, DateTime startTimeStamp, DateTime endTimeStamp)
        {
            var resources = new ComputeResourceMetrics();
            resources.PartitionKey = hostId;
            resources.RowKey = $"{(DateTime.MaxValue.Ticks - endTimeStamp.Ticks).ToString("D19")}.{Guid.NewGuid().ToId()}";
            resources.StartTimeStamp = startTimeStamp.ToJSONString();
            resources.EndTimeStamp = endTimeStamp.ToString();
            resources.ElapsedMS = (startTimeStamp - endTimeStamp).TotalMilliseconds;

            return resources;
        }


        public String StartTimeStamp { get; set; }
        public String EndTimeStamp { get; set; }
        public double ElapsedMS { get; set; }

        public long CurrentProcessMemoryAllocation { get; set; }

        public long BytesSent { get; set; }
        public long BytesReceived { get; set; }
        public long PacketErrors { get; set; }
        public long ProcessCount { get; set; }

        public long CurrentProcessThreadCount { get; set; }
        public double CurrentProcessCPUTimeMS { get; set; }

        public byte CurrentProcessCPUPercent { get; set; }

        public long AllProcessThreadCount { get; set; }
        public double AllProcessCPUTimeMS { get; set; }
        
        public byte AllProcessCPUPercent { get; set; }
    }
}
