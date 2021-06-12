using System;

namespace CryptoTradeStats
{
    internal class StatisticsBuy : StatisticsBase
    {
        public double TotalBuyPriceInr { get; }

        public StatisticsBuy(DateTime dateOfTransaction, double coinPrice, double amount, double totalBuyPriceInr) : base(dateOfTransaction, coinPrice, amount)
        {
            TotalBuyPriceInr = totalBuyPriceInr;
        }
    }
}
