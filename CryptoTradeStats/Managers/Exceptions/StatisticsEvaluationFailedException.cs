using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public sealed class StatisticsEvaluationFailedException : Exception
    {
        public StatisticsEvaluationFailedException() { }
        public StatisticsEvaluationFailedException(string message) : base(message) { }
        public StatisticsEvaluationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public StatisticsEvaluationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
