using LagoVista.IoT.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.IoT.Logging.Models;

namespace LagoVista.IoT.Core.Runtime.Tests.Utils
{
    public class LogWriter : ILogWriter
    {
        public List<LogRecord> ErrorRecords = new List<LogRecord>();

        public Task WriteError(LogRecord record)
        {
            ErrorRecords.Add(record);

            Console.WriteLine(record.Tag);
            Console.WriteLine(record.Message);
            if (!String.IsNullOrEmpty(record.Details))
                Console.WriteLine(record.Details);
            return Task.FromResult(default(object));
        }

        public Task WriteEvent(LogRecord record)
        {
            Console.WriteLine(record.Tag);
            Console.WriteLine(record.Message);
            if (!String.IsNullOrEmpty(record.Details))
                Console.WriteLine(record.Details);
            return Task.FromResult(default(object));
        }
    }
}
