using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class CustomerModel : PageModel
    {
        public CustomerModel(BankAppDataContext dbContext, 
            ICustomerService customerService, IAccountService accountService)
        {
            _dbContext = dbContext;
            _customerService = customerService;
            _accountService = accountService;
        }
        private readonly BankAppDataContext _dbContext;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public CustomerViewModel SingleCustomer { get; set; }
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public decimal Balance { get; set; }
        public string Today { get; set; }
        public int NumberOfAccounts { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public List<AccountViewModel> Accounts { get; set; }


        public void OnGet(int customerId)
        {
            SingleCustomer = _customerService.GetCustomerVM(customerId);         

            //lägg i service
            var customer = _dbContext.Customers
                .First(c => c.CustomerId == customerId);

            Id = customerId;
            Balance = _customerService.GetCustomerBalance(customerId);
            NumberOfAccounts = _customerService.GetNumberOfCustomerAccounts(customerId);
            Accounts = _customerService.GetAccountVMsForCustomer(customerId);
            Today = DateTime.Now.ToShortDateString();
        }

        public IActionResult OnGetShowMore(int customerId, int pageNum)
        {
            var transactions = _accountService.GetTransactionsFromCustomerId(customerId, pageNum);

            Transactions = transactions.Results.ToList();

            return new JsonResult(new { Transactions }); 
        }       
    }
}
