using System;

namespace CryptoTradeStats
{
    public abstract class StatisticsBase
    {
        public DateTime DateOfTransaction { get; }
        public decimal CoinPrice { get; }
        public decimal Volume { get; }
        
        protected StatisticsBase(DateTime dateOfTransaction, decimal coinPrice, decimal volume)
        {
            DateOfTransaction = dateOfTransaction;
            CoinPrice = coinPrice;
            Volume = volume;
        }
    }
}
