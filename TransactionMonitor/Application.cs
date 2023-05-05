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
        public Application(IAccountService accountService, ITransactionMonitorService transactionMonitorService)
        {
            _accountService = accountService;
            _transactionMonitorService = transactionMonitorService;
          
        }
        private readonly IAccountService _accountService;
        private readonly ITransactionMonitorService _transactionMonitorService;
      
        public void Run()
        {
            RunCountry("Sweden", "SuspiciousTransfersSE");
            RunCountry("Norway", "SuspiciousTransfersNO");
            RunCountry("Finland", "SuspiciousTransfersFI");
            RunCountry("Denmark", "SuspiciousTransfersDK");
        }
        public void RunCountry(string country, string folderPath)
        {
            var listOf24HTransactionsByCountry = _accountService.Get24HTransactionsFromCountry(country);
            var listOf72HTransactionsByCountry = _accountService.Get72HTransactionsFromCountry(country);

            var transferOver15000 = _accountService.GetTransfersOver15000(listOf24HTransactionsByCountry);
            var transferOver23000 = _accountService.GetTransfersOver23000(listOf72HTransactionsByCountry);

            var filePath = _transactionMonitorService.CreateFolderWithPath(folderPath);

            File.AppendAllText(filePath, Environment.NewLine + "Latest 24 hour transfers over 15000:");
            foreach (var transaction in transferOver15000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);
                _accountService.SaveToTextFile(customer, transaction, filePath);
            }
            File.AppendAllText(filePath, Environment.NewLine + "Latest 72 hour transfers with total/account over 23000:");
            foreach (var transaction in transferOver23000)
            {
                var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);
                _accountService.SaveToTextFile(customer, transaction, filePath);
            }
            File.AppendAllText(filePath, Environment.NewLine + "Time of day: " + DateTime.UtcNow.TimeOfDay.ToString());
        }
    }
}
