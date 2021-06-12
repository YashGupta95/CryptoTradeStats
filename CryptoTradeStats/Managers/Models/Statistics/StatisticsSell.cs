using System;

namespace CryptoTradeStats
{
    internal class StatisticsSell : StatisticsBase
    {
        public double TotalSellPriceInr { get; }

        public StatisticsSell(DateTime dateOfTransaction, double coinPrice, double amount, double totalSellPriceInr) : base(dateOfTransaction, coinPrice, amount)
        {
            TotalSellPriceInr = totalSellPriceInr;
        }
    }
}
