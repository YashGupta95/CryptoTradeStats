namespace CryptoTradeStats
{
    internal sealed class CurrentInvestmentData
    {
        public string CryptocurrencyName { get; }
        public decimal Volume { get; }
        public decimal NetInvestedAmount { get; }

        public CurrentInvestmentData(string cryptocurrencyName, decimal volume, decimal netInvestedAmount)
        {
            CryptocurrencyName = cryptocurrencyName;
            Volume = volume;
            NetInvestedAmount = netInvestedAmount;
        }
    }
}
