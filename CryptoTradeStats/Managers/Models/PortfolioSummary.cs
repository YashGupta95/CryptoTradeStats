using OfficeOpenXml;
using System.Collections.Generic;

namespace CryptoTradeStats
{
    //TODO: Modify all DTOs to remove setter if they have a constructor
    internal sealed class PortfolioSummary
    {
        public List<string> CoinsList { get; }
        public int LogbookEntries { get; }
        public int BuyEntries { get; }
        public double DepositBuyAmount { get; }
        public double ReinvestedBuyAmount { get; }
        public double TotalBuyAmount { get; }
        public int SellEntries { get; }
        public double TotalSellAmount { get; }

        public PortfolioSummary(List<string> coinsList, int logbookEntries, int buyEntries, double depositBuyAmount, double reinvestedBuyAmount, double totalBuyAmount, int sellEntries, double totalSellAmount)
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
