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
            resources.ElapsedMS = (endTimeStamp - startTimeStamp).TotalMilliseconds;

            return resources;
        }

        public String StartTimeStamp { get; set; }
        public String EndTimeStamp { get; set; }
        public double ElapsedMS { get; set; }
        public string Status { get; set; }
        public string StatusDetails { get; set; }
        public string CpuPercent { get; set; }
        public string MemoryPercent { get; set; }
    }
}
