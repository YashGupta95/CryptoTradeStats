using System;

namespace CryptoTradeStats
{
    internal class StatisticsSell : StatisticsBase
    {
        public decimal TotalSellPriceInr { get; }

        public StatisticsSell(DateTime dateOfTransaction, decimal coinPrice, decimal volume, decimal totalSellPriceInr) : base(dateOfTransaction, coinPrice, volume)
        {
            TotalSellPriceInr = totalSellPriceInr;
        }
    }
}
