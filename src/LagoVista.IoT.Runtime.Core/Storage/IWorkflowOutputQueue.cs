using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    /// <summary>
    /// This is a queue that can be used as an output from the workflow via the output translator for items that will be 
    /// made available the next time a device phones phone
    /// </summary>
    public interface IWorkflowOutputQueue
    {
        Task EnqueueOutputMessage(string deviceRepoId, string deviceId, object state);
        Task<IEnumerable<object>> DequeueOutputMessage(string deviceRepoId, string deviceId);
    }
}
