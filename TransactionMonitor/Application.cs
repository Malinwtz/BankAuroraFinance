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
        public Application(IAccountService accountService, ITransactionMonitorService transactionMonitorService)
        {
            _accountService = accountService;
            _transactionMonitorService = transactionMonitorService;
        }
        private readonly IAccountService _accountService;
        private readonly ITransactionMonitorService _transactionMonitorService;

        public void Run()
        {
            //hämta transaktioner per land 
            var transactionsOver15000 = _accountService.GetTransactionsOver15000();
            var transactionsOver23000 = _accountService.GetTransactionsOver23000();
            //var sweTransactionsOver15000 = _accountService.GetOneCountryTransactionsOver15000();
            //Console.WriteLine(sweTransactionsOver15000.Count);


            var folderSE = "SuspiciousTransfersSE";
            var folderNO = "SuspiciousTransfersNO";
            var folderFI = "SuspiciousTransfersFI";
            var folderDK = "SuspiciousTransfersDK";

            var filePathSE = _transactionMonitorService.CreateFolderWithPath(folderSE);
            var filePathNO = _transactionMonitorService.CreateFolderWithPath(folderNO);
            var filePathFI = _transactionMonitorService.CreateFolderWithPath(folderFI);
            var filePathDK = _transactionMonitorService.CreateFolderWithPath(folderDK);


            foreach (var transaction in transactionsOver15000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);

                var line = $"Customer: {customer.CustomerId}, Account: {transaction.AccountId}, Transaction: {transaction.TransactionId}," +
                        $" Amount: {transaction.Amount}, Date: {transaction.Date}";
                
                var filePath = "";
                if (customer.Country == "Sweden") filePath = filePathSE;
                if (customer.Country == "Norway") filePath = filePathNO;
                if (customer.Country == "Finland") filePath = filePathFI;
                if (customer.Country == "Denmark") filePath = filePathDK; // dk is used by another process

                File.AppendAllText(filePath, Environment.NewLine + line);
            }
            foreach (var transaction in transactionsOver23000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.Account.AccountId);

                //lägg till transaction id
                var line = $"Customer: {customer.CustomerId}, Account: {transaction.Account.AccountId}";

                var filePath = "";
                if (customer.Country == "Sweden") filePath = filePathSE;
                if (customer.Country == "Norway") filePath = filePathNO;
                if (customer.Country == "Finland") filePath = filePathFI;
                if (customer.Country == "Denmark") filePath = filePathDK;

                File.AppendAllText(filePath, Environment.NewLine + line);
            }
        }
    }
}
