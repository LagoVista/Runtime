// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 5d536916aa63caac649720acb941511fdc68a42ed0007104ca53c3724af3eddd
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core
{ 
    public enum PEMQueueTypes
    {
        Incoming,
        Outgoing,
        IncomingAndOutgoing
    }

    public interface IPEMQueue
    {
        /// <summary>
        /// Instance ID of the queue doing the processing
        /// </summary>
        String InstanceId { get; }
        
        String PipelineModuleId { get; }

        String Key { get; }


        PEMQueueTypes QueueType { get; }

        Task<InvokeResult> StartListeningAsync();
        Task<InvokeResult> StopListeningAsync();

        Task<PipelineExecutionMessage> ReceiveAsync();

        /// <summary>
        /// The type of pipeline module this queue supports.
        /// </summary>
        PipelineModuleType ForModuleType { get; }

        /// <summary>
        /// The key associated with the type of module
        /// </summary>
        String ForModuleTypeSubKey { get; }

        /// <summary>
        /// Take the message and enqueue it based on the instruction sets queue uri.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<InvokeResult> EnqueueAsync(PipelineExecutionMessage message);

        /// <summary>
        /// Returns true if nothing in the queue, false if it has at least one item.
        /// </summary>
        Task<bool> CheckIfEmptyAsync();
    }
}
