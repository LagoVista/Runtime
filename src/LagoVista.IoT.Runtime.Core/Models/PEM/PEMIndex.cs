using LagoVista.Core.Models;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{

    /* NOTE NOTE NOTE: Have a bit of a leaky abstraction here, I want to keep the PEM index record here because it belongs here
     * but we also need it for managing devices and doing diagnostics, we have an abreviated version here
     */
    public class PEMIndex : TableStorageEntity
    {
        public String DeviceId { get; set; }
        public String MessageId { get; set; }
        public String Topic { get; set; }
        public String PEM_URI { get; set; }
        public String Status { get; set; }
        public String CreatedTimeStamp { get; set; }
        public String LastUpdatedTimeStamp { get; set; }
        public String AttributesJSON { get; set; }
        public int TotalProcessingMS { get; set; }
        public string PEMJSON { get; set; }
    }
}
