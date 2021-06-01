using OfficeOpenXml;
using System;

namespace CryptoTradeStats
{
    internal class TradeManager
    {
        public void GetTradingSummary(ExcelPackage spreadsheet)
        {
            foreach (Stablecoin stablecoin in Enum.GetValues(typeof(Stablecoin)))
            {
                var statistics = GetStatistics(spreadsheet.Workbook.Worksheets[stablecoin.ToString()]);
                DisplayStatistics(statistics, stablecoin);
            }
        }

        public void GetCryptocurrencyTradeDetails(string cryptocurrencyName, ExcelPackage spreadsheet)
        {
            //TODO: Start implementation
        }

        private static TradeStatisticsSummary GetStatistics(ExcelWorksheet tradeSheet)
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
                        var actualDepositBuyAmount = double.Parse(tradeSheet.Cells[r, 7].Value.ToString());
                        depositBuyAmount += Math.Round(actualDepositBuyAmount, 2);
                    }
                    else
                    {
                        var actualReinvestedBuyAmount = double.Parse(tradeSheet.Cells[r, 7].Value.ToString());
                        reinvestedBuyAmount += Math.Round(actualReinvestedBuyAmount, 2);
                    }
                    totalBuyAmount = depositBuyAmount + reinvestedBuyAmount;

                    totalSellEntries = tradeSheet.Cells[r, 10].Value.ToString() != "0" ? (totalSellEntries += 1) : (totalSellEntries += 0);
                    var actualSellAmount = double.Parse(tradeSheet.Cells[r, 11].Value.ToString());
                    totalSellAmount += Math.Round(actualSellAmount, 2);
                }

                return new TradeStatisticsSummary(
                    logbookEntries: (end.Row - 1),
                    buyEntries: totalBuyEntries,
                    depositBuyAmount: depositBuyAmount,
                    reinvestedBuyAmount: reinvestedBuyAmount,
                    totalBuyAmount: totalBuyAmount,
                    sellEntries: totalSellEntries,
                    totalSellAmount: totalSellAmount
                );
            }
            catch (Exception e)
            {
                throw new StatisticsEvaluationFailedException($"Exception occurred while determining Trade Statistics for the provided spreadsheet. Exception Type: {e.GetType().Name}, Actual Exception message: {e.Message}");
            }            
        }
        
        private void DisplayStatistics(TradeStatisticsSummary tradeStatisticsSummary, Stablecoin stablecoinName)
        {
            Console.WriteLine($"----- Trade Statistics for {stablecoinName} Trading ------ \n");

            Console.WriteLine($"- Total number of entries found in Logbook for {stablecoinName} Trades: {tradeStatisticsSummary.LogbookEntries} \n");
            Console.WriteLine($"- Total Buy entries: {tradeStatisticsSummary.BuyEntries}");
            Console.WriteLine($"- Deposit (INR) Buy Amount: Rs. {tradeStatisticsSummary.DepositBuyAmount}");
            Console.WriteLine($"- Reinvested Buy Amount: Rs. {tradeStatisticsSummary.ReinvestedBuyAmount}");
            Console.WriteLine($"- Total Buy Amount (INR): Rs. {tradeStatisticsSummary.TotalBuyAmount} \n");
            Console.WriteLine($"- Total Sell entries: {tradeStatisticsSummary.SellEntries}");
            Console.WriteLine($"- Total Sell Amount (INR): Rs. {tradeStatisticsSummary.TotalSellAmount} \n");
        }
    }
}
