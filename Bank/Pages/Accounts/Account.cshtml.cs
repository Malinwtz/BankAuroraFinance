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

        public AccountModel(ISingleAccountService accountService)
        {
            _singleAccountService = accountService;
        }
        private readonly ISingleAccountService _singleAccountService;

        public Account SingleAccount { get; set; }


       

        public void OnGet(int accountId)
        {

            SingleAccount = _singleAccountService.GetAccount(accountId);

        }

    }
}
