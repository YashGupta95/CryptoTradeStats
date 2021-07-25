using System;

namespace CryptoTradeStats
{
    internal class StatisticsBuy : StatisticsBase
    {
        public double TotalBuyPriceInr { get; }

        public StatisticsBuy(DateTime dateOfTransaction, double coinPrice, double volume, double totalBuyPriceInr) : base(dateOfTransaction, coinPrice, volume)
        {
            TotalBuyPriceInr = totalBuyPriceInr;
        }
    }
}
