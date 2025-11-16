// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: a9bc9ad7521490be7e7cca6088ecc56d0d60cbf7dcb52042623b04d3d382ea6e
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Runtime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System.Collections.Concurrent;
using System.Threading;

namespace LagoVista.IoT.Core.Runtime.Tests.Utils
{
    public class TestInputQueue : IPEMQueue
    {
        bool _listening = false;
        ConcurrentQueue<PipelineExecutionMessage> _messageQueue;

        public TestInputQueue()
        {
            _messageQueue = new ConcurrentQueue<PipelineExecutionMessage>();
        }

        public bool ContainsMessage(string messageId)
        {
            var matchingMessages = _messageQueue.Where(msg => msg.Id == messageId);
            return matchingMessages.Any();
        }

        public string InstanceId { get; set; }

        public string PipelineModuleId { get; set; }

        public string Key { get; private set; }

        public PipelineModuleType ForModuleType { get; set; }
        public string ForModuleTypeSubKey { get; set; }

        public PEMQueueTypes QueueType { get; set; }

        public Task<bool> CheckIfEmptyAsync()
        {
            return Task.FromResult(!_messageQueue.Any());
        }

        public Task<InvokeResult> EnqueueAsync(PipelineExecutionMessage message)
        {
            _messageQueue.Enqueue(message);
            return Task.FromResult(InvokeResult.Success);
        }

        public Task<PipelineExecutionMessage> ReceiveAsync()
        {

            var spinWait = new SpinWait();
            while (_messageQueue.Count == 0 && _listening)
            {
                spinWait.SpinOnce();
            }

            if (!_listening)
                return Task.FromResult(default(PipelineExecutionMessage));

            if (_messageQueue.Any())
            {
                if (_messageQueue.TryDequeue(out PipelineExecutionMessage msg))
                {
                    return Task.FromResult(msg);
                }
            }

            return Task.FromResult(default(PipelineExecutionMessage));
        }

        public Task<InvokeResult> StartListeningAsync()
        {
            _listening = true;
            return Task.FromResult(InvokeResult.Success);
        }

        public Task<InvokeResult> StopListeningAsync()
        {
            _listening = false;
            return Task.FromResult(InvokeResult.Success);
        }
    }
}
