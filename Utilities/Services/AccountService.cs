using AutoMapper;
using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Utilities;

namespace Utilities.Services;

public class AccountService : IAccountService
{

    public AccountService(BankAppDataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    private readonly BankAppDataContext _dbContext;
    private readonly IMapper _mapper;


    public PagedResult<Account> GetSortedAccountsFromDatabase(
       string sortColumn, string sortOrder, int pageNo, string q)
    {
        //gör först en query med rätt sortering utifrån vad användaren valt
        var query = _dbContext.Accounts.AsQueryable();

        if (!string.IsNullOrEmpty(q))
        {
            query = query
                .Where(c => c.AccountId.ToString().Contains(q));
        }

        if (sortColumn == "Id")
            if (sortOrder == "asc")
                query = query.OrderBy(o => o.AccountId);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(o => o.AccountId);

        if (sortColumn == "Balance")
            if (sortOrder == "asc")
                query = query.OrderBy(o => o.Balance);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(o => o.Balance);

        if (sortColumn == "Created")
            if (sortOrder == "asc")
                query = query.OrderBy(o => o.Created);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(o => o.Created);

        if (sortColumn == "Frequency")
            if (sortOrder == "asc")
                query = query.OrderBy(o => o.Frequency);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(o => o.Frequency);

        return query.GetPaged(pageNo, 100);
    }

    public Account GetAccount(int accountId)
    {
        return _dbContext.Accounts.First(a => a.AccountId == accountId);
    }

    public void Update(Account account)
    {
        _dbContext.SaveChanges();
    }

    public List<AccountViewModel> GetTopTenAccounts()
    {
        var accounts = _dbContext.Accounts
            .OrderByDescending(a => a.Balance)
            .Take(10)
            .Select(a => new AccountViewModel
            {
                AccountId = a.AccountId,
                Balance = a.Balance,
                Frequency = a.Frequency,
                Created = a.Created

            }).ToList();

        return accounts;

        //var accounts = _dbContext.Accounts
        //    .OrderByDescending(a => a.Balance)
        //    .Take(10)
        //    .ToList();

        //return _mapper.Map<List<AccountViewModel>>(accounts);     //object not set to an instance?? funkar inte med azure??
    }

    public int GetLastPageNo()
    {
        int lastPageNo = _dbContext.Accounts.Count() / 100;

        return lastPageNo;
    }

    public bool IfAccountExists(int accountId)
    {
        var accountDb = _dbContext.Accounts.FirstOrDefault(a => a.AccountId == accountId);
        if (accountDb == null) { return false; }
        else return true;
    }

    public decimal GetAccountBalance(int accountId)
    {
        return _dbContext.Accounts.First(a => a.AccountId == accountId).Balance;
    }

    public void WithdrawOrDeposit(int accountId, decimal amount, bool deposition)
    {
        var accountDb = _dbContext.Accounts
                .First(a => a.AccountId == accountId);
        //var transaction = _dbContext.Transactions.First(a => a.AccountId == accountId);

        if (deposition == true)
        {
            accountDb.Balance += amount;
            //transaction.Balance += amount;
        }
        else
        {
            accountDb.Balance -= amount;
            //transaction.Balance -= amount;
        }

        _dbContext.SaveChanges();
    }

    public ErrorCode ReturnErrorCode(int accountId, decimal amount, string comment, bool deposition)
    {
        var accountDb = _dbContext.Accounts
                .First(a => a.AccountId == accountId);

        if (deposition == true)
        {
            if (accountDb.Balance < amount)
            {
                return ErrorCode.BalanceTooLow;
            }
        }

        if (amount < 100 || amount > 25000)
        {
            return ErrorCode.IncorrectAmount;
        }

        if (comment.Length < 5)
        {
            return ErrorCode.CommentTooShort;
        }

        if (comment.Length > 50)
        {
            return ErrorCode.CommentTooLong;
        }

        return ErrorCode.Ok;
    }

    public void RegisterTransaction(int accountId, DateTime date, decimal amount, decimal balance)
    {
        _dbContext.Transactions.Add(
            new Transaction
            {
                AccountId = accountId,
                Date = date,
                Amount = amount,
                Balance = balance,
                Type = "Credit",
                Operation = "Credit in Cash"
            });

        var account = _dbContext.Accounts.First(a => a.AccountId == accountId);
        account.Balance = balance;

        _dbContext.SaveChanges();
    }

    public PagedResult<TransactionViewModel> GetTransactions(int customerId, int pageNum)
    {
        var transactions = _dbContext.Transactions
               .Include(a => a.AccountNavigation).ThenInclude(d => d.Dispositions)
               .Where(a => a.AccountNavigation.Dispositions.Any(d => d.CustomerId == customerId))
               .OrderByDescending(d => d.Date)
               //nedan kan tas bort när man har en automapper men just nu måste man definera properies / binda dom
               .Select(s => new TransactionViewModel
               {
                   TransactionId = s.TransactionId,
                   Date = s.Date,
                   Type = s.Type,
                   Amount = s.Amount,
                   Balance = s.Balance
               }).GetPaged(pageNum, 10); 

        return transactions;
    }

    public List<Transaction> GetTransactionsOver15000()
    {
        var suspectTransactions = _dbContext.Transactions
           .Where(t => t.Date < DateTime.Now.AddDays(-1) && t.Amount > 15000)           
           .ToList();

        return suspectTransactions;
    }


    
    //public List<TransactionViewModel> GetOneCountryTransactionsOver15000()
    //{
    ////    var suspectTransactions = _dbContext.Transactions
    ////        .Include(t => t.AccountNavigation)
    ////        .ThenInclude(a => a.Dispositions)
    ////        .ThenInclude(d => d.Customer)
    ////       .Where(t => t.AccountNavigation.Dispositions.Any(d => d.Customer.Country == "Sweden")
    ////                && t.Date < DateTime.Now.AddDays(-1) && t.Amount > 15000)
    ////       .ToList();

    ////var suspectTransactions2 = await _dbContext.Transactions
    ////    .Where(t => t.AccountNavigation.Dispositions.Any(d => d.Customer.Country == "Sweden")
    ////            && t.Date < DateTime.Now.AddDays(-1) && t.Amount > 15000)
    ////    .ToListAsync();

    //        var suspectTransactions = _dbContext.Transactions
    //        .Include(a => a.AccountNavigation).ThenInclude(d => d.Dispositions)
    //        .Where(a => a.Date < DateTime.Now.AddDays(-1) && a.Amount > 15000
    //                && a.AccountNavigation.Dispositions.Any(d => d.Customer.Country == "Sweden") )
    //        .Select(s => new TransactionViewModel
    //        {
    //            TransactionId = s.TransactionId,
    //            Date = s.Date,
    //            Type = s.Type,
    //            Amount = s.Amount,
    //            Balance = s.Balance
    //        }).ToList();

    //    return suspectTransactions;
    //}

    public List<AccountWithSuspectTransactions> GetTransactionsOver23000()
    {
        var fromDate = DateTime.Now.AddDays(-3);

        var totalAmountThreshold = 23000;

        var suspectTransactions = _dbContext.Transactions
            .Where(t => t.Date >= fromDate)
            .GroupBy(t => t.AccountId)
            .Select(g => new
            {
                AccountId = g.First().AccountId,
                Transactions = g.Count(),
                TotalAmount = g.Sum(s => s.Amount)
            })
            .Where(g => g.TotalAmount > totalAmountThreshold)
            .ToList();

        var accountIds = suspectTransactions.Select(st => st.AccountId).ToList();

        var accounts = _dbContext.Accounts.Where(a => accountIds.Contains(a.AccountId)).ToList();
        var accountsWithTransactions = new List<AccountWithSuspectTransactions>();

        foreach (var account in accounts)
        {
            accountsWithTransactions.Add(new AccountWithSuspectTransactions()
            {
                Account = account,
                Transactions = _dbContext.Transactions.Where(t => t.Date >= fromDate && t.AccountId == account.AccountId).ToList(),
            });
        }

        return accountsWithTransactions;
    }


    public Customer GetSuspiciousCustomer(int accountId)
    {
        var customer = _dbContext.Customers
            .Include(t => t.Dispositions)
            .ThenInclude(d => d.Account)
            .FirstOrDefault(a => a.Dispositions.Any(d => d.Account.AccountId == accountId));
            
        return customer;
    }

    public List<Transaction> GetTransactions(string country)
    {
        var transactionList = _dbContext.Dispositions
            .Include(d => d.Customer)
            .Where(c => c.Customer.Country == country)
            //.Where(c => c.CustomerId == customerId)
            .SelectMany(a => a.Account.Transactions)
            .Where(d=>d.Date >= DateTime.Now.AddDays(-7))
            .OrderByDescending(t => t.TransactionId)
            .AsNoTracking()
            .ToList();
                    
        //var list = _dbContext.Dispositions.Include(d => d.Customer)
        //        .Where(c => c.CustomerId == customerId)
        //        .SelectMany(a => a.Account.Transactions)
        //        .Where(d => d.Date >= DateTime.Now.AddDays(-7))
        //        .OrderByDescending(t => t.TransactionId)
        //        .AsNoTracking()
        //        .ToList();

        return transactionList;
    }


}






//    var query = _dbContext.Customers
//        .Include(c => c.Dispositions)
//        .ThenInclude(d => d.Account)
//        .Where(a => a.Country == country)
//        .SelectMany(c => c.Dispositions.Select(d => d.Account))
//        .AsQueryable();
