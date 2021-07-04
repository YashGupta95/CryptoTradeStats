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
        private const string DepositTradeType = "Buy";
        private const string ReinvestTradeType = "Reinvest";

        public void FetchPortfolioSummary(ExcelPackage spreadsheet)
        {
            foreach (Stablecoin stablecoin in Enum.GetValues(typeof(Stablecoin)))
            {
                var statistics = GetPortfolioSummary(spreadsheet.Workbook.Worksheets[stablecoin.ToString()]);
                DisplayPortfolioSummary(statistics, stablecoin);
            }
        }

        public void GetCryptocurrencyTradeDetails(string cryptocurrencyName, string tradingStablecoin, ExcelPackage spreadsheet) //TODO: Add Exception Handling
        {
            var excelWorksheet = spreadsheet.Workbook.Worksheets[tradingStablecoin];

            var rowCount = excelWorksheet.Dimension.End.Row;
            var dateTimeFormat = "dd-MM-yyyy HH:mm:ss";

            var buyRecordsData = new List<StatisticsBuy>();
            var sellRecordsData = new List<StatisticsSell>();

            try
            {
                for (int row = 1; row <= rowCount; row++)
                {
                    if (excelWorksheet.Cells[row, 2].Value.ToString() == cryptocurrencyName)
                    {
                        if (excelWorksheet.Cells[row, 12].Value.ToString() == DepositTradeType || excelWorksheet.Cells[row, 12].Value.ToString() == ReinvestTradeType)
                        {
                            var buyPriceInr = double.Parse(excelWorksheet.Cells[row, 7].Value.ToString());

                            var buyRecords = new StatisticsBuy(
                                dateOfTransaction: DateTime.ParseExact(excelWorksheet.Cells[row, 1].Value.ToString(), dateTimeFormat, CultureInfo.InvariantCulture),
                                coinPrice: double.Parse(excelWorksheet.Cells[row, 4].Value.ToString()),
                                amount: double.Parse(excelWorksheet.Cells[row, 5].Value.ToString()),
                                totalBuyPriceInr: Math.Round(buyPriceInr, 2)
                                );

                            buyRecordsData.Add(buyRecords);
                        }
                        else
                        {
                            var sellPriceInr = double.Parse(excelWorksheet.Cells[row, 11].Value.ToString());

                            var sellRecords = new StatisticsSell(
                                dateOfTransaction: DateTime.ParseExact(excelWorksheet.Cells[row, 1].Value.ToString(), dateTimeFormat, CultureInfo.InvariantCulture),
                                coinPrice: double.Parse(excelWorksheet.Cells[row, 8].Value.ToString()),
                                amount: double.Parse(excelWorksheet.Cells[row, 9].Value.ToString()),
                                totalSellPriceInr: Math.Round(sellPriceInr, 2)
                                );

                            sellRecordsData.Add(sellRecords);
                        }
                    }
                }

                Console.WriteLine($"\nRecords found for {cryptocurrencyName}/{tradingStablecoin} :");
                DisplayTradeRecords(buyRecordsData, sellRecordsData, tradingStablecoin);
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

            double depositBuyAmount = 0.0;
            double reinvestedBuyAmount = 0.0;
            double totalBuyAmount = 0.0;
            double totalSellAmount = 0.0;

            var start = excelWorksheet.Dimension.Start;
            var end = excelWorksheet.Dimension.End;

            var coinsList = excelWorksheet.Cells
                .Where(c => (c.Address.Contains("B")) && (c.Value.ToString() != "Coin"))
                .Select(c => c.Value.ToString())
                .Distinct()
                .ToList();

            try
            {
                for (int r = start.Row + 1; r <= end.Row; r++)
                {
                    totalBuyEntries = excelWorksheet.Cells[r, 6].Value.ToString() != "0" ? (totalBuyEntries += 1) : (totalBuyEntries += 0);
                    if (excelWorksheet.Cells[r, 12].Value.ToString() == DepositTradeType)
                    {
                        var actualDepositBuyAmount = double.Parse(excelWorksheet.Cells[r, 7].Value.ToString());
                        depositBuyAmount += Math.Round(actualDepositBuyAmount, 2);
                    }
                    else
                    {
                        var actualReinvestedBuyAmount = double.Parse(excelWorksheet.Cells[r, 7].Value.ToString());
                        reinvestedBuyAmount += Math.Round(actualReinvestedBuyAmount, 2);
                    }
                    totalBuyAmount = depositBuyAmount + reinvestedBuyAmount;

                    totalSellEntries = excelWorksheet.Cells[r, 10].Value.ToString() != "0" ? (totalSellEntries += 1) : (totalSellEntries += 0);
                    var actualSellAmount = double.Parse(excelWorksheet.Cells[r, 11].Value.ToString());
                    totalSellAmount += Math.Round(actualSellAmount, 2);
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
            Console.WriteLine($"\n--------- Trade Statistics for {stablecoinName} Trading ---------- \n");

            Console.WriteLine($"- Total number of entries found in Logbook for {stablecoinName} Trades: {portfolioSummary.LogbookEntries} \n");
            Console.WriteLine("Coins in Portfolio:\n");
            foreach (var coin in portfolioSummary.CoinsList)
            {
                Console.WriteLine($"{coin}");
            }

            Console.WriteLine($"\n- Total Buy entries: {portfolioSummary.BuyEntries}");
            Console.WriteLine($"- Deposit (INR) Buy Amount: Rs. {portfolioSummary.DepositBuyAmount}");
            Console.WriteLine($"- Reinvested Buy Amount: Rs. {portfolioSummary.ReinvestedBuyAmount}");
            Console.WriteLine($"- Total Buy Amount (INR): Rs. {portfolioSummary.TotalBuyAmount} \n");

            Console.WriteLine($"- Total Sell entries: {portfolioSummary.SellEntries}");
            Console.WriteLine($"- Total Sell Amount (INR): Rs. {portfolioSummary.TotalSellAmount} \n");
        }

        private void DisplayTradeRecords(List<StatisticsBuy> buyRecordsData, List<StatisticsSell> sellRecordsData, string stablecoin)
        {
            //TODO: Add functionality to show current amount being invested in there
            var format = "{0, -25} {1, -20} {2, -20} {3, -25} \n";

            var buyRecordsOutput = new StringBuilder().AppendFormat(format, "Transaction Date", "Coin Price", "Amount", "Total Buy Price (INR)");
            var sellRecordsOutput = new StringBuilder().AppendFormat(format, "Transaction Date", "Coin Price", "Amount", "Total Sell Price (INR)");

            buyRecordsOutput.AppendLine();
            foreach (var buyRecord in buyRecordsData)
            {
                buyRecordsOutput.AppendFormat(format, buyRecord.DateOfTransaction, $"{buyRecord.CoinPrice} {stablecoin}", buyRecord.Amount, buyRecord.TotalBuyPriceInr);
            }

            Console.WriteLine("\nBuy Records: \n");
            Console.WriteLine(buyRecordsOutput.ToString());

            sellRecordsOutput.AppendLine();
            foreach (var sellRecord in sellRecordsData)
            {
                sellRecordsOutput.AppendFormat(format, sellRecord.DateOfTransaction, $"{sellRecord.CoinPrice} {stablecoin}", sellRecord.Amount, sellRecord.TotalSellPriceInr);
            }

            Console.WriteLine("\nSell Records: \n");
            Console.WriteLine(sellRecordsOutput.ToString());
        }
    }
}
