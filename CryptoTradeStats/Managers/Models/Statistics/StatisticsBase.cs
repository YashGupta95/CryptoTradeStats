using System;

namespace CryptoTradeStats
{
    public abstract class StatisticsBase
    {
        public DateTime DateOfTransaction { get; set; }
        public double CoinPrice { get; }
        public double Volume { get; }
        
        protected StatisticsBase(DateTime dateOfTransaction, double coinPrice, double volume)
        {
            DateOfTransaction = dateOfTransaction;
            CoinPrice = coinPrice;
            Volume = volume;
        }
    }
}
