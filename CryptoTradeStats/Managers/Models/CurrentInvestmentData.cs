namespace CryptoTradeStats
{
    internal sealed class CurrentInvestmentData
    {
        public string CryptocurrencyName { get; }
        public double Volume { get; }
        public double NetInvestedAmount { get; }

        public CurrentInvestmentData(string cryptocurrencyName, double volume, double netInvestedAmount)
        {
            CryptocurrencyName = cryptocurrencyName;
            Volume = volume;
            NetInvestedAmount = netInvestedAmount;
        }
    }
}
