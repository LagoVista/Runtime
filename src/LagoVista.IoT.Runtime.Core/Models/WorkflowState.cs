using LagoVista.IoT.DeviceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    /// <summary>
    /// Class to manage state of workflow between invocation
    /// </summary>
    public class WorkflowState
    {
        public string LastUpdated { get; set; }
        public Dictionary<string, WorkflowStateValue> Attributes { get; set; }
        public Dictionary<string, WorkflowStateValue> PropertyBag { get; set; }
        public Dictionary<string, State> State { get; set; }
    }

    public class WorkflowStateValue
    {
        public ParameterTypes ParameterType { get; set; }
        public Object Value { get; set; }
    }
}
