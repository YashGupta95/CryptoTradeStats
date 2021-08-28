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

            var portfolioManager = new PortfolioManager();
            string userInput;

            do
            {
                Console.WriteLine("******************************* Welcome to Crypto Trade Stats ********************************");
                Console.WriteLine("**********************************************************************************************");
                Console.WriteLine("\n1. Trading Portfolio Summary");
                Console.WriteLine("2. Trading details for a specific Cryptocurrency");
                Console.WriteLine("3. Exit\n");
                Console.WriteLine("**********************************************************************************************");

                Console.WriteLine("Select your option:");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        portfolioManager.FetchPortfolioSummary(spreadsheet);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("\nEnter the name of Cryptocurrency (e.g. ETH for Ethereum): ");
                        var cryptocurrencyName = Console.ReadLine();
                        Console.WriteLine("\nEnter the name of Trading Stablecoin (e.g. USDT for Tether): ");
                        var tradingStablecoin = Console.ReadLine();
                        portfolioManager.GetCryptocurrencyTradeDetails(cryptocurrencyName, tradingStablecoin, spreadsheet);
                        break;

                    case "3":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option selected. Plese select a valid option from the Main Menu.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadLine();
                Console.Clear();
            }
            while (userInput != "3");

        }
    }
}
