using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CryptoTradeStats
{
    internal class PortfolioManager
    {
        private const string BuyTradeType = "Buy";
        private const string ReinvestTradeType = "Reinvest";
        private const string SellTradeType = "Sell";

        private CurrentInvestmentData currentInvestmentData;

        public void FetchPortfolioSummary(ExcelPackage spreadsheet)
        {
            Console.WriteLine("**********************************************************************************************");

            foreach (Stablecoin stablecoin in Enum.GetValues(typeof(Stablecoin)))
            {
                var statistics = GetPortfolioSummary(spreadsheet.Workbook.Worksheets[stablecoin.ToString()]);
                DisplayPortfolioSummary(statistics, stablecoin);
            }
        }

        public void GetCryptocurrencyTradeDetails(string cryptocurrencyName, string tradingStablecoin, ExcelPackage spreadsheet)
        {
            ValidateInputValues(cryptocurrencyName, tradingStablecoin);

            var excelWorksheet = spreadsheet.Workbook.Worksheets[tradingStablecoin];
            EnsureCryptocurrencyIsPresentInPortfolio(excelWorksheet, cryptocurrencyName);

            var buyRecordsData = new List<StatisticsBuy>();
            var sellRecordsData = new List<StatisticsSell>();

            try
            {
                var rowCount = excelWorksheet.Dimension.End.Row;

                for (int row = 1; row <= rowCount; row++)
                {
                    if (excelWorksheet.Cells[row, 2].Value.ToString() == cryptocurrencyName)
                    {
                        var dateOfTransaction = double.Parse(excelWorksheet.Cells[row, 1].Value.ToString());

                        if (excelWorksheet.Cells[row, 12].Value.ToString() == BuyTradeType || excelWorksheet.Cells[row, 12].Value.ToString() == ReinvestTradeType)
                        {
                            var coinPrice = excelWorksheet.Cells[row, 4].Value.ToString();
                            var volume = excelWorksheet.Cells[row, 5].Value.ToString();
                            var buyPriceInr = decimal.Parse(excelWorksheet.Cells[row, 7].Value.ToString());

                            var buyRecords = new StatisticsBuy(
                                dateOfTransaction: DateTime.FromOADate(dateOfTransaction),
                                coinPrice: decimal.Parse(coinPrice, NumberStyles.Float),
                                volume: decimal.Parse(volume, NumberStyles.Float),
                                totalBuyPriceInr: Math.Round(buyPriceInr, 2)
                                );

                            buyRecordsData.Add(buyRecords);
                        }
                        else
                        {
                            var coinPrice = excelWorksheet.Cells[row, 8].Value.ToString();
                            var volume = excelWorksheet.Cells[row, 9].Value.ToString();
                            var sellPriceInr = decimal.Parse(excelWorksheet.Cells[row, 11].Value.ToString());

                            var sellRecords = new StatisticsSell(
                                dateOfTransaction: DateTime.FromOADate(dateOfTransaction),
                                coinPrice: decimal.Parse(coinPrice, NumberStyles.Float),
                                volume: decimal.Parse(volume, NumberStyles.Float),
                                totalSellPriceInr: Math.Round(sellPriceInr, 2)
                                );

                            sellRecordsData.Add(sellRecords);
                        }
                    }
                }

                var totalBuyVolume = buyRecordsData.Sum(r => r.Volume);
                var totalSellVolume = sellRecordsData.Sum(r => r.Volume);

                if (totalBuyVolume == totalSellVolume)
                {
                    currentInvestmentData = new CurrentInvestmentData(cryptocurrencyName, volume: 0, netInvestedAmount: 0.0M);
                }
                else if (totalBuyVolume > totalSellVolume)
                {
                    var netBuyAmount = buyRecordsData.Sum(r => r.TotalBuyPriceInr);
                    var netSellAmount = sellRecordsData.Sum(r => r.TotalSellPriceInr);

                    currentInvestmentData = new CurrentInvestmentData(
                        cryptocurrencyName,
                        volume: Math.Round((totalBuyVolume - totalSellVolume), 5),
                        netInvestedAmount: Math.Round((netBuyAmount - netSellAmount), 2));
                }

                Console.WriteLine($"\nRecords found for \"{cryptocurrencyName}/{tradingStablecoin}\" in Portfolio :\n");
                DisplayTradeRecords(buyRecordsData, sellRecordsData, currentInvestmentData, tradingStablecoin);
            }
            catch (Exception e)
            {
                throw new StatisticsParsingFailedException($"Exception occurred while fetching and parsing trading information for {cryptocurrencyName}/{tradingStablecoin}. Exception Type: {e.GetType().Name}, Actual Exception message: {e.Message}");
            }
        }

        private static PortfolioSummary GetPortfolioSummary(ExcelWorksheet excelWorksheet)
        {
            int totalBuyEntries = 0;
            int totalSellEntries = 0;

            decimal depositBuyAmount = 0.0M;
            decimal reinvestedBuyAmount = 0.0M;
            decimal totalBuyAmount = 0.0M;
            decimal totalSellAmount = 0.0M;

            var start = excelWorksheet.Dimension.Start;
            var end = excelWorksheet.Dimension.End;

            var coinsList = GetAllCryptocurrencies(excelWorksheet);

            try
            {
                for (int row = start.Row + 1; row <= end.Row; row++)
                {
                    switch (excelWorksheet.Cells[row, 12].Value.ToString())
                    {
                        case BuyTradeType:
                            totalBuyEntries += 1;
                            var actualDepositBuyAmount = decimal.Parse(excelWorksheet.Cells[row, 7].Value.ToString());
                            depositBuyAmount += Math.Round(actualDepositBuyAmount, 2);
                            break;

                        case ReinvestTradeType:
                            totalBuyEntries += 1;
                            var actualReinvestedBuyAmount = decimal.Parse(excelWorksheet.Cells[row, 7].Value.ToString());
                            reinvestedBuyAmount += Math.Round(actualReinvestedBuyAmount, 2);
                            break;

                        case SellTradeType:
                            totalSellEntries += 1;
                            var actualSellAmount = decimal.Parse(excelWorksheet.Cells[row, 11].Value.ToString());
                            totalSellAmount += Math.Round(actualSellAmount, 2);
                            break;

                        default:
                            Console.WriteLine("Error: The Trade Type is not supported.");
                            break;
                    }

                    totalBuyAmount = depositBuyAmount + reinvestedBuyAmount;
                }

                return new PortfolioSummary(
                    logbookEntries: (end.Row - 1),
                    buyEntries: totalBuyEntries,
                    depositBuyAmount: depositBuyAmount,
                    reinvestedBuyAmount: reinvestedBuyAmount,
                    totalBuyAmount: totalBuyAmount,
                    sellEntries: totalSellEntries,
                    totalSellAmount: totalSellAmount,
                    coinsList: coinsList
                );
            }
            catch (Exception e)
            {
                throw new PortfolioEvaluationFailedException($"Exception occurred while fetching Portfolio Summary for the spreadsheet specified. Exception Type: {e.GetType().Name}, Actual Exception message: {e.Message}");
            }
        }

        private void DisplayPortfolioSummary(PortfolioSummary portfolioSummary, Stablecoin stablecoinName)
        {
            var header = $"TRADE STATISTICS FOR {stablecoinName} TRADING";
            Console.WriteLine(header.PadLeft(60));
            Console.WriteLine("##############################################################################################");

            Console.WriteLine($"\n{stablecoinName}-based Cryptocurrencies present in Portfolio:");

            var coinsListWithStablecoinSuffix = portfolioSummary.CoinsList.Select(c => c + $"/{stablecoinName}");
            Console.WriteLine(string.Join(", ", coinsListWithStablecoinSuffix));

            Console.WriteLine($"\n- Total number of buy/sell entries found in Logbook for {stablecoinName} Trades: {portfolioSummary.LogbookEntries} \n");
            Console.WriteLine("##############################################################################################");
            Console.WriteLine($"\n- Total Buy entries: {portfolioSummary.BuyEntries}");
            Console.WriteLine($"- Deposit (INR) Buy Amount: Rs. {portfolioSummary.DepositBuyAmount}");
            Console.WriteLine($"- Reinvested Buy Amount: Rs. {portfolioSummary.ReinvestedBuyAmount}");
            Console.WriteLine($"- Total Buy Amount (INR): Rs. {portfolioSummary.TotalBuyAmount} \n");

            Console.WriteLine("##############################################################################################");
            Console.WriteLine($"\n- Total Sell entries: {portfolioSummary.SellEntries}");
            Console.WriteLine($"- Total Sell Amount (INR): Rs. {portfolioSummary.TotalSellAmount} \n");
            Console.WriteLine("##############################################################################################");
        }

        private static void ValidateInputValues(string cryptocurrencyName, string tradingStablecoin)
        {
            var errorMessages = new StringBuilder();

            errorMessages.AppendLineIfNotNull(ValidatorString.Validate("Cryptocurrency Symbol", cryptocurrencyName));
            errorMessages.AppendLineIfNotNull(ValidatorString.Validate("Trading Stablecoin", tradingStablecoin));

            if (errorMessages.Length != 0)
                throw new InputValidationFailedException(errorMessages.ToString());
        }

        private static void EnsureCryptocurrencyIsPresentInPortfolio(ExcelWorksheet excelWorksheet, string cryptocurrencyName)
        {
            var coinsList = GetAllCryptocurrencies(excelWorksheet);

            if (!coinsList.Contains(cryptocurrencyName))
            {
                throw new CryptocurrencyNotFoundException($"The Cryptocurrency \"{cryptocurrencyName}/{excelWorksheet.Name}\" is currently not present in your Portfolio. No records found.");
            }
        }

        private static List<string> GetAllCryptocurrencies(ExcelWorksheet excelWorksheet)
        {
            return excelWorksheet.Cells
                .Where(c => (c.Address.Contains("B")) && (c.Value.ToString() != "Coin"))
                .Select(c => c.Value.ToString())
                .Distinct()
                .ToList();
        }

        private void DisplayTradeRecords(List<StatisticsBuy> buyRecordsData, List<StatisticsSell> sellRecordsData, CurrentInvestmentData currentInvestmentData, string stablecoin)
        {
            var format = "{0, -25} | {1, -15} | {2, -10} | {3, -20} \n";

            var buyRecordsOutput = new StringBuilder().AppendFormat(format, "Transaction Date", "Coin Price", "Volume", "Total Buy Price (INR)");
            var sellRecordsOutput = new StringBuilder().AppendFormat(format, "Transaction Date", "Coin Price", "Volume", "Total Sell Price (INR)");

            buyRecordsOutput.AppendLine();
            foreach (var buyRecord in buyRecordsData)
            {
                buyRecordsOutput.AppendFormat(format, buyRecord.DateOfTransaction, $"{buyRecord.CoinPrice} {stablecoin}", buyRecord.Volume, buyRecord.TotalBuyPriceInr);
            }

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("BUY RECORDS:");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine(buyRecordsOutput.ToString());

            sellRecordsOutput.AppendLine();
            foreach (var sellRecord in sellRecordsData)
            {
                sellRecordsOutput.AppendFormat(format, sellRecord.DateOfTransaction, $"{sellRecord.CoinPrice} {stablecoin}", sellRecord.Volume, sellRecord.TotalSellPriceInr);
            }

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("SELL RECORDS:");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine(sellRecordsOutput.ToString());

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine($"Current volume of {currentInvestmentData.CryptocurrencyName} coins in Portfolio: {currentInvestmentData.Volume}  \nCurrent Invested Amount (INR): {currentInvestmentData.NetInvestedAmount}\n");
            Console.WriteLine("**********************************************************************************************");
        }
    }
}
