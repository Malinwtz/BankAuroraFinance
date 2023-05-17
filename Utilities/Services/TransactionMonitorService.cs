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
       
        public TransactionMonitorService()
        {
        } 

        public string CreateFolderWithPath(string folderName, string country)
        {
            //var folderPath = Path.Combine(
            // @"C:\Users\malin\OneDrive\Dokument\KYH.NET22-24\5.Webbutveckling\AuroraFinance\TransactionMonitor", folderName);
            
            var folderPath = "../../../TransactionLog" + country;

            Directory.CreateDirectory(folderPath);

            var fileName = "SuspiciousTransfers_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            var filePath = Path.Combine(folderPath, fileName);

            return filePath;

            
        }
    }
}
