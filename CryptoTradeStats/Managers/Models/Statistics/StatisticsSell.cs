using System;

namespace CryptoTradeStats
{
    internal class StatisticsSell : StatisticsBase
    {
        public double TotalSellPriceInr { get; }

        public StatisticsSell(DateTime dateOfTransaction, double coinPrice, double volume, double totalSellPriceInr) : base(dateOfTransaction, coinPrice, volume)
        {
            TotalSellPriceInr = totalSellPriceInr;
        }
    }
}
