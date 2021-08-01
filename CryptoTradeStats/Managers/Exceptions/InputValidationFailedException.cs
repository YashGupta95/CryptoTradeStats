using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    class InputValidationFailedException : Exception
    {
        public string Reason => "Inputs validation failed.";
        public InputValidationFailedException() { }
        public InputValidationFailedException(string message) : base(message) { }
        public InputValidationFailedException(string message, Exception innerException) : base(message, innerException) { }
        protected InputValidationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
