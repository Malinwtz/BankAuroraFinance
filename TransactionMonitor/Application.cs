using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.Services.Interfaces;
using Utilities.Services;
using Microsoft.EntityFrameworkCore;

namespace TransactionMonitor
{
    public class Application
    {
        public Application(IAccountService accountService, ITransactionMonitorService transactionMonitorService,
            BankAppDataContext dbContext)
        {
            _accountService = accountService;
            _transactionMonitorService = transactionMonitorService;
            _dbContext = dbContext;
        }
        private readonly IAccountService _accountService;
        private readonly ITransactionMonitorService _transactionMonitorService;
        private readonly BankAppDataContext _dbContext;

        //public List<Transaction> TransactionsSE { get; set; }
        //public List<Transaction> TransactionsFI { get; set; }
        //public List<Transaction> TransactionsNO { get; set; }
        //public List<Transaction> TransactionsDK { get; set; }



        public void Run()
        {
            //returnerar transaktioner från sverige från de senaste fyra dagarna
            var listOfTransactionsByCountry = _accountService.GetTransactions("Sweden");

            Console.WriteLine(listOfTransactionsByCountry.Count);


            //var transactionsOver15000 = _accountService.GetTransactionsOver15000();
            //var transactionsOver23000 = _accountService.GetTransactionsOver23000();

            //foreach (var transaction in transactionsOver15000)
            //{
            //    var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);


            //    var line = $"{customer.Country} - Customer: {customer.CustomerId}, Account: {transaction.AccountId}, Transaction: {transaction.TransactionId}," +
            //            $" Amount: {transaction.Amount}, Date: {transaction.Date}";


            //    var folderPath = Path.Combine(
            // @"C:\Users\malin\OneDrive\Dokument\KYH.NET22-24\5.Webbutveckling\AuroraFinance\TransactionMonitor", "AllSuspiciousTransfers");
            //    Directory.CreateDirectory(folderPath);

            //    var fileName = "SuspiciousTransfers_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //    var filePath = Path.Combine(folderPath, fileName);


            //    File.AppendAllText(filePath, Environment.NewLine + line);
            //}
            //foreach (var transaction in transactionsOver23000)
            //{
            //    var customer = _accountService.GetSuspiciousCustomer(transaction.Account.AccountId);


            //  //  var listOfTransactionsByCountry = _accountService.GetTransactions(customer.Country, customer.CustomerId);

            //    var line = $"{customer.Country} - Customer: {customer.CustomerId}, Account: {transaction.Account.AccountId}";


            //    var folderPath = Path.Combine(
            // @"C:\Users\malin\OneDrive\Dokument\KYH.NET22-24\5.Webbutveckling\AuroraFinance\TransactionMonitor", "SuspiciousTransfers");
            //    Directory.CreateDirectory(folderPath);

            //    var fileName = "SuspiciousTransfers_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //    var filePath = Path.Combine(folderPath, fileName);


            //    File.AppendAllText(filePath, Environment.NewLine + line);
            //}


        }



        public void Run1()
        {
            //hämta transaktioner per land 
            var transactionsOver15000 = _accountService.GetTransactionsOver15000();
            var transactionsOver23000 = _accountService.GetTransactionsOver23000();
            //var sweTransactionsOver15000 = _accountService.GetOneCountryTransactionsOver15000();
            //Console.WriteLine(sweTransactionsOver15000.Count);

            foreach (var transaction in transactionsOver15000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);

                var line = $"Customer: {customer.CustomerId}, Account: {transaction.AccountId}, Transaction: {transaction.TransactionId}," +
                        $" Amount: {transaction.Amount}, Date: {transaction.Date}";

                var filePath = "";
                if (customer.Country == "Sweden")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersSE");
                }
                else if (customer.Country == "Norway")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersNO");
                }
                else if (customer.Country == "Finland")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersFI");
                }
                else if (customer.Country == "Denmark")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersDK");
                }

                File.AppendAllText(filePath, Environment.NewLine + line);
            }
            foreach (var transaction in transactionsOver23000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.Account.AccountId);

                var line = $"Customer: {customer.CustomerId}, Account: {transaction.Account.AccountId}";

                var filePath = "";
                if (customer.Country == "Sweden")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersSE");
                }
                else if (customer.Country == "Norway")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersNO");
                }
                else if (customer.Country == "Finland")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersFI");
                }
                else if (customer.Country == "Denmark")
                {
                    filePath = _transactionMonitorService.CreateFolderWithPath("SuspiciousTransfersDK");
                }

                File.AppendAllText(filePath, Environment.NewLine + line);
            }
        }



    }
}
