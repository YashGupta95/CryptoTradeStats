namespace CryptoTradeStats
{
    internal sealed class CurrentInvestmentData
    {
        public string CryptocurrencyName { get; set; }
        public double Volume { get; set; }
        public double NetInvestedAmount { get; set; }

        public CurrentInvestmentData(string cryptocurrencyName, double volume, double netInvestedAmount)
        {
            CryptocurrencyName = cryptocurrencyName;
            Volume = volume;
            NetInvestedAmount = netInvestedAmount;
        }
    }
}
