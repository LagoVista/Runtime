﻿using LagoVista.Core.Models;
using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LagoVista.Core.Interfaces;
using Newtonsoft.Json;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public class VerificationResults : EntityBase
    {
        public VerificationResults(EntityHeader component, VerifierTypes type)
        {
            Id = Guid.NewGuid().ToId();
            Component = component;
            VerifierType = EntityHeader<VerifierTypes>.Create(type);
            Results = new ObservableCollection<VerificationResult>();
            ErrorMessages = new List<string>();
            Name = $"Verifier Result: {DateTime.UtcNow.ToJSONString()}";
        }

   
        public string DateStamp { get; set; }
        public EntityHeader RequestedBy { get; set; }

        public EntityHeader<VerifierTypes> VerifierType { get; set; }

        public EntityHeader Component { get; set; }
        public int IterationsCompleted { get; set; }
        public double ExecutionTimeMS { get; set; }
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; }
        public ObservableCollection<VerificationResult> Results { get; set; }
     }

    public class VerificationResult
    {
        public string Field { get; set; }
        public string Key { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool Success { get; set; }
    }
}
