// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 732adb5a59da0306cd4a7d5a038e2f3d24621c5e9553dc248106bdb2da7a7d43
// IndexVersion: 2
// --- END CODE INDEX META ---
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
