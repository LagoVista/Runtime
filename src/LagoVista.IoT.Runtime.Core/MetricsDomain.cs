using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core
{
    [DomainDescriptor]
    public class MetricsDomain
    {
        public const string Metrics = "Metrics";

        [DomainDescription(Metrics)]
        public static DomainDescription MetricsDomainDescription
        {
            get
            {
                return new DomainDescription()
                {
                    Description = "Meterics are used for monitoring usage and performance of the system.",
                    DomainType = DomainDescription.DomainTypes.Dto,
                    Name = "Metrics",
                    CurrentVersion = new LagoVista.Core.Models.VersionInfo()
                    {
                        Major = 0,
                        Minor = 8,
                        Build = 001,
                        DateStamp = new DateTime(2017, 4, 11),
                        Revision = 1,
                        ReleaseNotes = "Initial unstable preview"
                    }
                };
            }
        }
    }
}
