using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CryptoTradeStats
{
    internal class TradeManager
    {
        public void GetTradeSummary(string logbookDirectory)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var spreadsheet = new ExcelPackage(new FileInfo(logbookDirectory));

            try
            {
                var usdtStatistics = GetStatistics(spreadsheet.Workbook.Worksheets["USDT"]);
            }
            finally
            {
                spreadsheet?.Dispose();
            }
        }

        private static object GetStatistics(ExcelWorksheet tradeSheet)
        {
            var start = tradeSheet.Dimension.Start;
            var end = tradeSheet.Dimension.End;

            int totalBuyEntries = 0;
            int totalSellEntries = 0;
            double totalBuyAmount = 0.0;
            double totalSellAmount = 0.0;

            for (int r = start.Row + 1; r <= end.Row; r++)
            {
                totalBuyEntries = tradeSheet.Cells[r, 6].Value.ToString() != "0" ? (totalBuyEntries += 1) : (totalBuyEntries += 0);
                if (tradeSheet.Cells[r, 12].Value.ToString() == "Deposit (INR)")
                {
                    var buyAmountInString = tradeSheet.Cells[r, 7].Value.ToString();
                    var actualBuyAmount = double.Parse(buyAmountInString);
                    totalBuyAmount += Math.Round(actualBuyAmount, 2, MidpointRounding.ToEven);
                }

                totalSellEntries = tradeSheet.Cells[r, 10].Value.ToString() != "0" ? (totalSellEntries += 1) : (totalSellEntries += 0);
                var sellAmountInString = tradeSheet.Cells[r, 11].Value.ToString();
                var actualSellAmount = double.Parse(sellAmountInString);
                totalSellAmount += Math.Round(actualSellAmount, 2, MidpointRounding.ToEven);
            }


        }
    }
}
