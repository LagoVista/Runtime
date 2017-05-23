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
