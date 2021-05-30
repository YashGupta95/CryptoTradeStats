namespace CryptoTradeStats
{
    internal sealed class TradeStatistics
    {
        public int LogbookEntries { get; set; }
        public int BuyEntries { get; set; }
        public double DepositBuyAmount {get; set;}
        public double ReinvestedBuyAmount { get; set; }
        public double TotalBuyAmount { get; set; }
        public int SellEntries { get; set; }
        public double TotalSellAmount { get; set; }

        public TradeStatistics(int logbookEntries, int buyEntries, double depositBuyAmount, double reinvestedBuyAmount, double totalBuyAmount, int sellEntries, double totalSellAmount)
        {
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
