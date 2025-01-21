using LagoVista.Core.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Runtime.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IFallbackMessageHandler : IPipelineModule
    {
        Task<InvokeResult<PipelineExecutionMessage>> AddMediaMessageAsync(Stream stream, string contentType, long contentLength, DateTime startTimeStamp, string path, String deviceId = "", String topic = "", Dictionary<string, string> headers = null);
        Task<InvokeResult<PipelineExecutionMessage>> AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", string deviceId = "", string topic = "", Dictionary<string, string> headers = null);

        void LogVerboseMessage(string tag, string message, params KeyValuePair<string, string>[] args);

        Task<InvokeResult<OutgoingMessage>> WaitOnAsync(String requestId, TimeSpan timeout);
        void SendNotification(Targets target, String text);
    }
}
