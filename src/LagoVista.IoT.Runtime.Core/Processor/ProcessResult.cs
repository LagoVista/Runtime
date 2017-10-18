using LagoVista.Core.Validation;
using LagoVista.IoT.Logging;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core.Processor
{
    public class ProcessResult
    {
        public ProcessResult()
        {
            ErrorMessages = new List<Error>();
            WarningMessages = new List<Warning>();
            InfoMessages = new List<Info>();
        }

        public bool Success { get { return ErrorMessages.Count == 0; } }

        public List<Error> ErrorMessages { get; private set; }
        public List<Warning> WarningMessages { get; private set; }
        public List<Info> InfoMessages { get; private set; }

        public static ProcessResult FromSuccess
        {
            get { return new ProcessResult(); }
        }

        public static ProcessResult FromError(ErrorCode errCode, string details = "")
        {
            var result = new ProcessResult();
            var err = errCode.ToError();
            err.Details = details;
            result.ErrorMessages.Add(err);
            return result;
        }

        public static ProcessResult FromInvokeResult(InvokeResult result)
        {
            if(result.Successful)
            {
                return ProcessResult.FromSuccess;
            }

            var processResult = new ProcessResult();

            foreach(var err in result.Errors)
            {
                processResult.ErrorMessages.Add(new Error() { ErrorCode = err.ErrorCode, Message = err.Message, Details = err.Details });
            }

            foreach (var wrn in result.Warnings)
            {
                processResult.ErrorMessages.Add(new Error() { ErrorCode = wrn.ErrorCode, Message = wrn.Message, Details = wrn.Details });
            }

            return processResult;
        }
    }
}