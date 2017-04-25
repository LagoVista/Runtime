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

        public bool Success { get; set; }

        public List<Error> ErrorMessages { get; private set; }
        public List<Warning> WarningMessages { get; private set; }
        public List<Info> InfoMessages { get; private set; }

        public static ProcessResult FromSuccess
        {
            get { return new ProcessResult() { Success = true }; }
        }

        public static ProcessResult FromError(ErrorCode err)
        {
            var result = new ProcessResult();
            result.ErrorMessages.Add(err.ToError());
            return result;
        }
    }
}