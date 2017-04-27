using LagoVista.Core.Models;
using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public class VerificationResult : TableStorageEntity
    {
        public VerificationResult(String componentId)
        {
            RowKey = Guid.NewGuid().ToId();
            ErrorMessage = "";
            ComponentId = componentId;
            PartitionKey = ComponentId;
        }

        public string DateStamp { get; set; }
        public string ComponentId { get; set; }
        public int Iterations { get; set; }
        public double ExecutionTimeMS { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
