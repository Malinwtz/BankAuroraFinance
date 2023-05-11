using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;
using Utilities.Services.Interfaces;

namespace Utilities.Services
{
    public class TransactionMonitorService : ITransactionMonitorService
    {
       
        public TransactionMonitorService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        } 
        private readonly BankAppDataContext _dbContext;

        public string CreateFolderWithPath(string folderName)
        {
            var folderPath = Path.Combine(
             @"C:\Users\malin\OneDrive\Dokument\KYH.NET22-24\5.Webbutveckling\AuroraFinance\TransactionMonitor", folderName);

            Directory.CreateDirectory(folderPath);

            var fileName = "SuspiciousTransfers_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            var filePath = Path.Combine(folderPath, fileName);


            return filePath;

            //var folderPath = Path.Combine(
            //  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //  "AuroraFinance", "TransactionMonitor", folderName);

            ////var folderPath = Path.Combine(
            ////@"C:\Users\malin\OneDrive\Dokument\KYH.NET22-24\5.Webbutveckling\AuroraFinance\TransactionMonitor", folderName);

            //Directory.CreateDirectory(folderPath);

            //var fileName = "SuspiciousTransfers_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //var filePath = Path.Combine(folderPath, fileName);

            //return filePath;


        }
    }
}
