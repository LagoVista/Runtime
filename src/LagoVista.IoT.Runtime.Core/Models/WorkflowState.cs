// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: af250ef8286fbaf552e549baf5ce5ea427f3c577c6cd028a7c955c91f893806b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.DeviceAdmin.Models;
using System;
using LagoVista.Core;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    /// <summary>
    /// Class to manage state of workflow between invocation
    /// </summary>
    public class WorkflowState
    {
        public WorkflowState()
        {
            LastUpdated = DateTime.UtcNow.ToJSONString();
            Attributes = new Dictionary<string, WorkflowStateValue>();
            PropertyBag = new Dictionary<string, WorkflowStateValue>();
            State = new Dictionary<string, DeviceAdmin.Models.State>();
        }

        public string LastUpdated { get; set; }
        public Dictionary<string, WorkflowStateValue> Attributes { get; set; }
        public Dictionary<string, WorkflowStateValue> PropertyBag { get; set; }
        public Dictionary<string, State> State { get; set; }
    }

    public class WorkflowStateValue
    {
        public ParameterTypes ParameterType { get; set; }
        public bool HasValue { get; set; }
        public Object Value { get; set; }        
    }
}
