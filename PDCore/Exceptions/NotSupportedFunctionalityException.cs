using Org.BouncyCastle.Asn1.X509;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PDCore.Exceptions
{
    public class NotSupportedFunctionalityException : NotSupportedException
    {
        public NotSupportedFunctionalityException()
        {
        }

        public NotSupportedFunctionalityException(string message) : base(message)
        {

        }

        public NotSupportedFunctionalityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSupportedFunctionalityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                string message = $"{base.Message} Nazwa funkcjonalności: {TargetSite.Name}. Obiekt: {TargetSite.DeclaringType.Name}";

                return message.TrimStart();
            }
        }
    }
}
