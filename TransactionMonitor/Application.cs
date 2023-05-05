using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.Services.Interfaces;
using Utilities.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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


        public List<Transaction> ListOf24HTransactionsByCountry { get; set; }
        public List<Transaction> ListOf72HTransactionsByCountry { get; set; }
        public void Run()
        {
            RunCountry("Sweden", "SuspiciousTransfersSE");
            RunCountry("Norway", "SuspiciousTransfersNO");
            RunCountry("Finland", "SuspiciousTransfersFI");
            RunCountry("Denmark", "SuspiciousTransfersDK");
        }
        public void RunCountry(string country, string folderPath)
        {             
            var filePath = _transactionMonitorService.CreateFolderWithPath(folderPath);            
            DateTime lastTimeOfDay = DateTime.MinValue;                 

            if (File.Exists(filePath))
            {
                string lastLine = File.ReadLines(filePath).Last();
                if (DateTime.TryParseExact(lastLine, "HH:mm:ss.fffffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastTimeOfDay))
                {
                    lastTimeOfDay = lastTimeOfDay.AddSeconds(1); 
                }
                if(lastTimeOfDay != DateTime.MinValue)
                {
                    Console.WriteLine($"TransactionMonitor has already been run for {country} on this date");
                    return;
                }               
            }      
                ListOf24HTransactionsByCountry = _accountService.Get24HTransactionsFromCountry(country, lastTimeOfDay);
                ListOf72HTransactionsByCountry = _accountService.Get72HTransactionsFromCountry(country, lastTimeOfDay);
                var transferOver15000 = _accountService.GetTransfersOver15000(ListOf24HTransactionsByCountry);
                var transferOver23000 = _accountService.GetTransfersOver23000(ListOf72HTransactionsByCountry);

                File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + "Single transfers over 15000:");
                foreach (var transaction in transferOver15000)
                {
                    var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);
                    _accountService.SaveToTextFile(customer, transaction, filePath);
                }
                File.AppendAllText(filePath, Environment.NewLine + "Transfers (total sum of transfers/account) over 23000:");
                foreach (var transaction in transferOver23000)
                {
                    var customer = _accountService.GetSuspiciousCustomer(transaction.AccountId);
                    _accountService.SaveToTextFile(customer, transaction, filePath);
                }
                File.AppendAllText(filePath, Environment.NewLine + "Time of day: "
                    + Environment.NewLine + DateTime.Now.TimeOfDay.ToString());     
        }
    }
}
