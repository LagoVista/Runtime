using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class MessageValue
    {
        public bool HasValue { get; set; }
        public string Value { get; set; }
        public EntityHeader<Unit> Unit {get; set;}
        public EntityHeader<State> State { get; set; }
        public EntityHeader<ParameterTypes> Type { get; set; }
    }
}
