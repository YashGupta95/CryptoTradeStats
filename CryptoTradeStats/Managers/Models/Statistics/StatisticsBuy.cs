using System;

namespace CryptoTradeStats
{
    internal class StatisticsBuy : StatisticsBase
    {
        public decimal TotalBuyPriceInr { get; }

        public StatisticsBuy(DateTime dateOfTransaction, decimal coinPrice, decimal volume, decimal totalBuyPriceInr) : base(dateOfTransaction, coinPrice, volume)
        {
            TotalBuyPriceInr = totalBuyPriceInr;
        }
    }
}
