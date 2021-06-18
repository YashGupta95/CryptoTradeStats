using OfficeOpenXml;
using System.Collections.Generic;

namespace CryptoTradeStats
{
    internal sealed class PortfolioSummary
    {
        public List<string> CoinsList { get; set; }
        public int LogbookEntries { get; set; }
        public int BuyEntries { get; set; }
        public double DepositBuyAmount {get; set;}
        public double ReinvestedBuyAmount { get; set; }
        public double TotalBuyAmount { get; set; }
        public int SellEntries { get; set; }
        public double TotalSellAmount { get; set; }

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
