using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Users
{
    public static class SystemUsers
    {
        public static EntityHeader SystemOrg
        {
            get
            {
                return new EntityHeader()
                {
                    Id = "61109e3071894e97880b4d719e54f903",
                    Text = "System"
                };
            }
        }

        public static EntityHeader HostUser
        {
            get {
                return new EntityHeader()
                {
                    Id = "c3b0a2abadd14af4af4c964c10a60c5a",
                    Text = "HostUser"
                };
            }
        }

        public static EntityHeader InstanceHostUser
        {
            get
            {
                return new EntityHeader()
                {
                    Id = "1b70bde503de4f03a64db89d3e77de4b",
                    Text= "InstanceHost"
                };
            }
        }

    }
}
