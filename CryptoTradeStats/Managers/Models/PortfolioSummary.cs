using OfficeOpenXml;
using System.Collections.Generic;

namespace CryptoTradeStats
{
    internal sealed class PortfolioSummary
    {
        public List<string> CoinsList { get; }
        public int LogbookEntries { get; }
        public int BuyEntries { get; }
        public decimal DepositBuyAmount { get; }
        public decimal ReinvestedBuyAmount { get; }
        public decimal TotalBuyAmount { get; }
        public int SellEntries { get; }
        public decimal TotalSellAmount { get; }

        public PortfolioSummary(List<string> coinsList, int logbookEntries, int buyEntries, decimal depositBuyAmount, decimal reinvestedBuyAmount, decimal totalBuyAmount, int sellEntries, decimal totalSellAmount)
        {
            CoinsList = coinsList;
            LogbookEntries = logbookEntries;
            BuyEntries = buyEntries;
            DepositBuyAmount = depositBuyAmount;
            ReinvestedBuyAmount = reinvestedBuyAmount;
            TotalBuyAmount = totalBuyAmount;
            SellEntries = sellEntries;
            TotalSellAmount = totalSellAmount;
        }
    }
}
