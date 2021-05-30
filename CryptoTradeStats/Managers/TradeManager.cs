using OfficeOpenXml;
using System;
using System.IO;

namespace CryptoTradeStats
{
    internal class TradeManager
    {
        public void GetTradeSummary(string logbookDirectory)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var spreadsheet = new ExcelPackage(new FileInfo(logbookDirectory));

            foreach (Stablecoin stablecoin in Enum.GetValues(typeof(Stablecoin)))
            {
                var statistics = GetStatistics(spreadsheet.Workbook.Worksheets[stablecoin.ToString()]);
                DisplayStatistics(statistics, stablecoin);
            }
        }

        private static TradeStatistics GetStatistics(ExcelWorksheet tradeSheet)
        {
            var start = tradeSheet.Dimension.Start;
            var end = tradeSheet.Dimension.End;

            int totalBuyEntries = 0;
            int totalSellEntries = 0;

            double depositBuyAmount = 0.0;
            double reinvestedBuyAmount = 0.0;
            double totalBuyAmount = 0.0;
            double totalSellAmount = 0.0;

            try
            {
                for (int r = start.Row + 1; r <= end.Row; r++)
                {
                    totalBuyEntries = tradeSheet.Cells[r, 6].Value.ToString() != "0" ? (totalBuyEntries += 1) : (totalBuyEntries += 0);
                    if (tradeSheet.Cells[r, 12].Value.ToString() == "Deposit (INR)")
                    {
                        var depositBuyAmountInString = tradeSheet.Cells[r, 7].Value.ToString();
                        var actualDepositBuyAmount = double.Parse(depositBuyAmountInString);
                        depositBuyAmount += Math.Round(actualDepositBuyAmount, 2);
                    }
                    else
                    {
                        var reinvestedBuyAmountInString = tradeSheet.Cells[r, 7].Value.ToString();
                        var actualReinvestedBuyAmount = double.Parse(reinvestedBuyAmountInString);
                        reinvestedBuyAmount += Math.Round(actualReinvestedBuyAmount, 2);
                    }
                    totalBuyAmount = depositBuyAmount + reinvestedBuyAmount;

                    totalSellEntries = tradeSheet.Cells[r, 10].Value.ToString() != "0" ? (totalSellEntries += 1) : (totalSellEntries += 0);
                    var sellAmountInString = tradeSheet.Cells[r, 11].Value.ToString();
                    var actualSellAmount = double.Parse(sellAmountInString);
                    totalSellAmount += Math.Round(actualSellAmount, 2);
                }

                return new TradeStatistics(
                logbookEntries: (end.Row - 1),
                buyEntries: totalBuyEntries,
                depositBuyAmount: depositBuyAmount,
                reinvestedBuyAmount: reinvestedBuyAmount,
                totalBuyAmount: totalBuyAmount,
                sellEntries: totalSellEntries,
                totalSellAmount: totalSellAmount);
            }
            catch (Exception e)
            {
                throw new StatisticsEvaluationFailedException($"Exception occurred while determining Trade Statistics for the provided spreadsheet. Exception Type: {e.GetType().Name}, Actual Exception message: {e.Message}");
            }            
        }

        private void DisplayStatistics(TradeStatistics tradeStatistics, Stablecoin stablecoinName)
        {
            Console.WriteLine($"----- Trade Statistics for {stablecoinName} Trading ------ \n");

            Console.WriteLine($"- Total number of entries found in Logbook for {stablecoinName} Trades: {tradeStatistics.LogbookEntries} \n");
            Console.WriteLine($"- Total Buy entries: {tradeStatistics.BuyEntries}");
            Console.WriteLine($"- Deposit (INR) Buy Amount: Rs. {tradeStatistics.DepositBuyAmount}");
            Console.WriteLine($"- Reinvested Buy Amount: Rs. {tradeStatistics.ReinvestedBuyAmount}");
            Console.WriteLine($"- Total Buy Amount (INR): Rs. {tradeStatistics.TotalBuyAmount} \n");
            Console.WriteLine($"- Total Sell entries: {tradeStatistics.SellEntries}");
            Console.WriteLine($"- Total Sell Amount (INR): Rs. {tradeStatistics.TotalSellAmount} \n");
        }
    }
}
