using LagoVista.Core.Models;
using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public class VerificationResults : TableStorageEntity
    {
        public VerificationResults(String componentId)
        {
            RowKey = Guid.NewGuid().ToId();
            ComponentId = componentId;
            PartitionKey = ComponentId;
            Results = new ObservableCollection<VerificationResult>();
            ErrorMessage = new List<string>();
        }

        public string DateStamp { get; set; }
        public string ComponentId { get; set; }
        public EntityHeader RequestedBy { get; set; }
        public int IterationCompleted { get; set; }
        public double ExecutionTimeMS { get; set; }
        public bool Success { get; set; }
        public List<string> ErrorMessage { get; set; }
        public ObservableCollection<VerificationResult> Results {get; set;}
    }

    public class VerificationResult
    {
        public string Key { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool Success { get; set; }
    }
}
