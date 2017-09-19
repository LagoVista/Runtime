using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class MessageValue
    {
        private string _value;
        public string Name { get; set; }
        public string Key { get; set; }
        public bool HasValue { get; set; }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                HasValue = !string.IsNullOrEmpty(value);
            }
        }
        public EntityHeader<UnitSet> UnitSet {get; set;}
        public EntityHeader<StateSet> StateSet { get; set; }
        public EntityHeader<ParameterTypes> Type { get; set; }
    }
}
