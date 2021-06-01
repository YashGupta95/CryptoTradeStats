using OfficeOpenXml;
using System;
using System.IO;

namespace CryptoTradeStats
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            var logbookDirectory = args[0];
            using var spreadsheet = new ExcelPackage(new FileInfo(logbookDirectory));

            var tradeManager = new TradeManager();

            Console.WriteLine("------ Cypto Trade Stats ----- \n");
            Console.WriteLine("1. Trading Summary");
            Console.WriteLine("2. Trading Details for a specific Cryptocurrency");

            Console.WriteLine("\nEnter your choice: ");
            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    tradeManager.GetTradingSummary(spreadsheet);
                    break;

                case "2":
                    Console.WriteLine("\nEnter the name of Cryptocurrency (e.g. ETH for Ethereum): ");
                    var cryptocurrencyName = Console.ReadLine();
                    tradeManager.GetCryptocurrencyTradeDetails(cryptocurrencyName, spreadsheet); //Start implementation
                    break;

                default:
                    Console.WriteLine("Invalid option selected. Plese select a valid option from the Main Menu.");
                    break;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadLine();
        }
    }
}
