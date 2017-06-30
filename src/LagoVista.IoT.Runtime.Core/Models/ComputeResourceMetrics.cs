using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    public class ComputeResourceMetrics
    {
        public void Create()
        {
        }

        public byte CPUPercent { get; set; }
        public byte MemoryPercent { get; set; }
        public Int32 ProcessCount { get; set; }
        public Int32 ThreadCount { get; set; }
    }
}
