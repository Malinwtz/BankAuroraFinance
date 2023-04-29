using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.Services.Interfaces;
using Utilities.Services;

namespace TransactionMonitor
{
    public class Application
    {
        public Application(IAccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly IAccountService _accountService;

        public void Run()
        {
            //hämta transaktioner per land 
            //hämta transaktioner/kund som är större än gränsen

            //om enskild transaktion är större än 15000 eller 
            //totala transaktioner är större än 230000 inom de 72 senaste timmarna/3 senaste dygnen
            //är det lika med en misstänkt transaktion - skriv till en textfil

            var transactionsOver15000 = _accountService.GetTransactionsOver15000();

            var transactionsOver23000 = _accountService.GetTransactionsOver23000();


            Console.WriteLine(transactionsOver15000.Count());

            Console.WriteLine(transactionsOver23000.Count());

            foreach(var transaction in transactionsOver23000)
            {
                Console.WriteLine(transaction.Account.AccountId);

            }




            //----------------------------------------------------------------------------------

            //var transactions = _dbContext.Transactions.Where(t => t.country == "Finland");

            //var suspectTransactions = transactions
            //    .Where(t => t.amount > 15000);

            ////hämta en lista med alla transaktioner från samma kund från de senaste tre dagarna
            //var suspectTransactions3days = _dbcontext.Transactions
            //    .Where(t=>t.date < DateTime.Now.AddDays(-3));

            //if (amount > 23000)
            //{
            //    var suspectTransactions = new List<Transaction>();
            //    suspectTransactions.Add(transaction);
            //}


            //if (amount >15000 || amount)
            //{
            //    var suspectTransactions = new List<Transaction>();
            //    suspectTransactions.Add(transaction);


            //        var fileName = DateTime.Now.ToString("RECEIPT_yyy-MM-dd") + ".txt";
            //        var lastNumber = GetNumberOfReceipt();
            //        var numberLine = $"KVITTO NR {lastNumber}";
            //        File.AppendAllText(fileName, Environment.NewLine);
            //        File.AppendAllText(fileName, numberLine + Environment.NewLine);

            //        foreach (var row in suspectTransactions)
            //        {
            //            var line = $"{row.TransactionId} {row.AccountId} * {row.Date} = {row.Amount}kr";
            //            File.AppendAllText(fileName, line + Environment.NewLine);
            //        }

            //        var total = $"Total: {Convert.ToString(CalculateTotal())}kr";
            //        File.AppendAllText(fileName, total + Environment.NewLine);


            //}


        }


    }
}
