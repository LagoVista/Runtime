// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d982235f49cf91601d4f20821936d624315ec096d0d2f4dbcc88cc0bd90ea766
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public class VerificationRequest<TConfiguration>
    {
        public VerificationRequest()
        {
            Iterations = 1;
        }

        public TConfiguration Configuration { get; set; }
        public Verifier Verifier { get; set; }
        public int Iterations { get; set; }
    }
}
