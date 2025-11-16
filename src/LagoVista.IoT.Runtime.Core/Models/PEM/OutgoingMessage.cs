// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e81f68cc51d5872e6357a65b5a81847af975f1b6c386ca74fd5494c5572e2d40
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using System.Collections.ObjectModel;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class OutgoingMessage
    {
        public string MessageDefinitionId { get; set; }

        public OutgoingMessage()
        {
            Headers = new ObservableCollection<Header>();
        }

        public EntityHeader<MessagePayloadTypes> PayloadType { get; set; }

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
