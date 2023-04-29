using AutoMapper;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class AccountsModel : PageModel
    {

        public AccountsModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public string Country { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int LastPage { get; set; }
        public string Q { get; set; }
        public int PageCount { get; set; }
        public List<AccountViewModel> Accounts { get; set; }


      
        public void OnGet(string country, string sortColumn, string sortOrder,
            int pageNo, string q)
        {
            Q = q;
            LastPage = _accountService.GetLastPageNo();

            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;

            SortColumn = sortColumn;
            SortOrder = sortOrder;
            Country = country;

            //flytta in i servicen 
            //hämta alla accounts
            var result = _accountService.GetSortedAccountsFromDatabase(
                sortColumn, sortOrder, pageNo, q);



            //Accounts = result.Results
            //    .Select(a => new AccountViewModel
            //    {
            //        AccountId = a.AccountId,
            //        Balance = a.Balance,
            //        Frequency = a.Frequency,
            //        Created = a.Created

            //    }).ToList();
            Accounts = result.Results.Select(_mapper.Map<Account, AccountViewModel>).ToList();

            // Use AutoMapper to map the results to view models
            //Accounts = _mapper.Map<List<AccountViewModel>>(result.Results);

            PageCount = result.PageCount;
        }
    }
}
