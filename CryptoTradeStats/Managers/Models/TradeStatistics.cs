namespace CryptoTradeStats
{
    internal sealed class TradeStatistics
    {
        public int LogbookEntries { get; set; }
        public int BuyEntries { get; set; }
        public double BuyAmount { get; set; }
        public int SellEntries { get; set; }
        public double SellAmount { get; set; }

        public TradeStatistics(int logbookEntries, int buyEntries, double buyAmount, int sellEntries, double sellAmount)
        {
            LogbookEntries = logbookEntries;
            BuyEntries = buyEntries;
            BuyAmount = buyAmount;
            SellEntries = sellEntries;
            SellAmount = sellAmount;
        }
    }
}
