using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages.Accounts
{    
    [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "country" })]
   
    public class TopTenAccountsModel : PageModel
    {
      
        public TopTenAccountsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly IAccountService _accountService;
        public string Country { get; set; }
        public List<AccountViewModel> Accounts { get; set; }




        public void OnGet(string country)
        {

            Country = country;
            
            Accounts = _accountService.GetTopTenAccounts();
           
        }
    }
}
