using System;

namespace CryptoTradeStats
{
    class Program
    {
        static void Main(string[] args)
        {
            var logbookDirectory = args[0];

            var tradeManager = new TradeManager();
            tradeManager.GetTradeSummary(logbookDirectory);
        }
    }
}
