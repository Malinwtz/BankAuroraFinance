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
        public int NumberOfAccounts { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public List<AccountViewModel> Accounts { get; set; }

    


        public void OnGet(int customerId)
        {
            //lägg i en metod i SingleCustomerServices
            SingleCustomer = _customerService.GetCustomerVM(customerId);
          
            //Transactions = _singleCustomerService.GetCustomerTransactions(customerId);

            var customer = _dbContext.Customers
                .First(c => c.CustomerId == customerId);
            Id = customerId;
            //Name = c.Name;

            Balance = _customerService.GetCustomerBalance(customerId);
            NumberOfAccounts = _customerService.GetNumberOfCustomerAccounts(customerId);


            Accounts = _customerService.GetAccountVMsForCustomer(customerId);
        }

        public IActionResult OnGetShowMore(int customerId, int pageNum)
        {
            //hämtar trasactions från databas flytta detta till service
            var transactions = _accountService.GetTransactions(customerId, pageNum);

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
