using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Enums
{
    /// <summary>
    /// Typ certyfikatu
    /// </summary>
    public enum CertificateType
    {
        /// <summary>
        /// Certyfikat WSS służący do podpisywania wiadomości
        /// </summary>
        WSS = 1,

        /// <summary>
        /// Certyfikat służący do zabezpieczenia komunikacji
        /// </summary>
        TLS = 2
    }
}
