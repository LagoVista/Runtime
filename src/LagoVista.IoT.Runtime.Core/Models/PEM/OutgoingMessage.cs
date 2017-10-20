using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class OutgoingMessage
    {
        public OutgoingMessage()
        {
            Headers = new ObservableCollection<Header>();
        }

        public EntityHeader<PayloadTypes> PayloadType { get; set; }

        public ObservableCollection<Header> Headers { get; private set; }

        public string ContentType { get; set; }
        public string Topic { get; set; }
        public string Method { get; set; }
        public string PathAndQueryString { get; set; }
        public byte[] BinaryPayload { get; set; }
        public string TextPayload { get; set; }
        public string DeviceId { get; set; }
    }
}
