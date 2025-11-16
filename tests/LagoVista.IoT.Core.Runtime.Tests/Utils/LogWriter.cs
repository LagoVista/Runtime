// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b3ccd2f0c263c1da031428befa055ebb5f565225314f59be2ffc8f4e65fe8ef9
// IndexVersion: 2
// --- END CODE INDEX META ---
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
