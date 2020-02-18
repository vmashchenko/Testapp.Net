using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Core.Guard
{
    /// <summary>
    /// This class is used to indicates that something is wrong with <see cref="Throw"/> during execution.
    /// </summary>
    [Serializable]
    public sealed class ThrowException : Exception
    {
        public ThrowException(string reason)
            : base(reason)
        {
        }

        public ThrowException()
        {
        }

        public ThrowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private ThrowException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}
