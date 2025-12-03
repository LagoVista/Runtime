// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 98bce2e9a5e1c0b16b21d2d53a819e3700d252a811f38d552499ff97820a5ed5
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
                    Description = "Use verifiers to confirm that configured components work as expected.",
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