using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class PortfolioEvaluationFailedException : Exception
    {
        public string Reason => "Failed to evaluate Portfolio statistics.";
        public PortfolioEvaluationFailedException() { }
        public PortfolioEvaluationFailedException(string message) : base(message) { }
        public PortfolioEvaluationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public PortfolioEvaluationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
