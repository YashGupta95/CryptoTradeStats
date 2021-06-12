using System;

namespace CryptoTradeStats
{
    public abstract class StatisticsBase
    {
        public DateTime DateOfTransaction { get; set; }
        public double CoinPrice { get; }
        public double Amount { get; }
        
        protected StatisticsBase(DateTime dateOfTransaction, double coinPrice, double amount)
        {
            DateOfTransaction = dateOfTransaction;
            CoinPrice = coinPrice;
            Amount = amount;
        }
    }
}
