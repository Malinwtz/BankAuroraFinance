using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.ViewModels
{
    public class AccountWithSuspectTransactions
    {
        public Account Account { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
