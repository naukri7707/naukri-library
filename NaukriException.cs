using System;
using System.Runtime.Serialization;
using System.Security;

namespace Naukri
{
    public class NaukriException : Exception
    {
        public NaukriException() : base()
        {
        }

        public NaukriException(string message) : base(message)
        {
        }

        public NaukriException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        [SecuritySafeCritical]
        protected NaukriException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}