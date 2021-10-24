using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace CryptoTradeStats
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal sealed class StatisticsParsingFailedException : Exception
    {
        public string Reason => "Unable to parse Buy/Sell Statistics details.";

        public StatisticsParsingFailedException() 
        { 
        }

        public StatisticsParsingFailedException(string message) 
            : base(message) 
        { 
        }

        public StatisticsParsingFailedException(string message, Exception innerException) 
            : base(message, innerException) 
        { 
        }

        private StatisticsParsingFailedException(SerializationInfo info, StreamingContext context) 
            : base(info, context) 
        { 
        }
    }
}
