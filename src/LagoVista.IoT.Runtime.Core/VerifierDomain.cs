using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core
{
    [DomainDescriptor]
    public class VerifierDomain
    {
        public const string Verifiers = "Verifiers";

        [DomainDescription(Verifiers)]
        public static DomainDescription VerifierDomainDescription
        {
            get
            {
                return new DomainDescription()
                {
                    Description = "Verifiers are used to Verify Configured Components work as Expected.",
                    DomainType = DomainDescription.DomainTypes.BusinessObject,
                    Name = "Verifiers",
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
