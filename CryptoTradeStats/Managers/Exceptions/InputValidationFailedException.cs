using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal sealed class InputValidationFailedException : Exception
    {
        public string Reason => "Inputs validation failed.";
        public InputValidationFailedException() { }
        public InputValidationFailedException(string message) : base(message) { }
        public InputValidationFailedException(string message, Exception innerException) : base(message, innerException) { }
        public InputValidationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
