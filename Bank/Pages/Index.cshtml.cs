using Utilities.Models;
using Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages;

public class IndexModel : PageModel
{
    private readonly BankAppDataContext _dbContext;

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger,
        BankAppDataContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public List<IndexViewModel> Index { get; set; }
    public List<CountryViewModel> Countries { get; set; }

    public void OnGet()
    {
        Index = new List<IndexViewModel>
        {
            new()
            {
                NumberOfCustomers = _dbContext.Customers.Count(),
                Accounts = _dbContext.Accounts.Count(),
                Balance = _dbContext.Accounts
                    .Where(b => b.Balance > 0).Sum(a => a.Balance)
            }
        };


        Countries = _dbContext.Customers
            .GroupBy(c => c.Country)
            .Select(g => new CountryViewModel
            {
                Country = g.Key,
                NumberOfCustomers = g.Count(),
                NumberOfAccounts = _dbContext.Dispositions
                    .Where(d => g
                        .Select(c => c.CustomerId)
                        .Contains(d.CustomerId) && d.Type.ToLower() == "owner")
                    .Select(d => d.AccountId)
                    .Distinct()
                    .Count(),
                BalancePerCountry = _dbContext.Dispositions
                    .Where(d => g
                        .Select(c => c.CustomerId)
                        .Contains(d.CustomerId) && d.Type.ToLower() == "owner")
                    .Select(d => d.Account.Balance)
                    .Sum()
            }).ToList();




    }
}