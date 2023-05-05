using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class AccountModel : PageModel
    {
        public AccountModel(ISingleAccountService accountService, IAccountService accountService1)
        {
            _singleAccountService = accountService;
            _accountService1 = accountService1;
        }
        private readonly ISingleAccountService _singleAccountService;
        private readonly IAccountService _accountService1;

        public Account SingleAccount { get; set; }
        public string Today { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }


        public void OnGet(int accountId)
        {
            SingleAccount = _singleAccountService.GetAccount(accountId);
            Today = DateTime.Now.ToShortDateString();
        }

        public IActionResult OnGetShowMore(int accountId, int pageNum)
        {
            //hämtar trasactions från databas flytta detta till service
            var transactions = _accountService1.GetTransactionsFromCustomerId(accountId, pageNum);

            // _dbContext.Transactions
            //.Include(a => a.AccountNavigation).ThenInclude(d => d.Dispositions)
            //.Where(a => a.AccountNavigation.Dispositions.Any(d => d.CustomerId == customerId))
            //.OrderByDescending(d => d.Date)
            ////nedan kan tas bort när man har en automabinda dom
            //.Select(s => new TransactionViewModel
            //{
            //    TransactionId = s.TransactionId,
            //    Date = s.Date,
            //    Type = s.Type,
            //    Amount = s.Amount,
            //    Balance = s.Balance
            //}).GetPaged(pageNum, 10); //använder mi ska ladda in

            Transactions = transactions.Results.ToList(); //populerar Transactions lista med min modell

            return new JsonResult(new { Transactions }); //returnerar ny json array
        }
    }
}
