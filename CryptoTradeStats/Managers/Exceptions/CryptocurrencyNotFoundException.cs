using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal sealed class CryptocurrencyNotFoundException : Exception
    {
        public string Reason => "Specified cryptocurrency was not found in logbook.";
        public CryptocurrencyNotFoundException() { }
        public CryptocurrencyNotFoundException(string message) : base(message) { }
        public CryptocurrencyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        private CryptocurrencyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
