using LagoVista.Core.Models;
using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public class VerificationResults : INoSQLEntity, IIDEntity
    {
        public VerificationResults(EntityHeader component, VerifierTypes type)
        {
            Id = Guid.NewGuid().ToId();
            Component = component;
            VerifierType = EntityHeader<VerifierTypes>.Create(type);
            Results = new ObservableCollection<VerificationResult>();
            ErrorMessage = new List<string>();
        }

        public string Id { get; set; }

        public string DateStamp { get; set; }
        public EntityHeader RequestedBy { get; set; }

        public EntityHeader<VerifierTypes> VerifierType { get; set; }

        public EntityHeader Component { get; set; }
        public int IterationCompleted { get; set; }
        public double ExecutionTimeMS { get; set; }
        public bool Success { get; set; }
        public List<string> ErrorMessage { get; set; }
        public ObservableCollection<VerificationResult> Results {get; set;}
        public string DatabaseName { get; set; }
        public string EntityType { get; set; }
    }

    public class VerificationResult
    {
        public string Key { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool Success { get; set; }
    }
}
