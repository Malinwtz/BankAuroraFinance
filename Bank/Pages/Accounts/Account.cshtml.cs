using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

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
        public Customer SingleCustomer { get; set; }
        public string Today { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }


        public void OnGet(int accountId)
        {
            SingleAccount = _singleAccountService.GetAccount(accountId);
            SingleCustomer = _singleAccountService.GetCustomerFromAccountId(accountId);   
            Today = DateTime.Now.ToShortDateString();
        }

        public IActionResult OnGetShowMore(int accountId, int pageNum)
        {
            var transactions = _accountService1.GetTransactionsFromCustomerId(accountId, pageNum);

            Transactions = transactions.Results.ToList(); 

            return new JsonResult(new { Transactions }); 
        }
    }
}
