﻿using AutoMapper;
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

        if (deposition == true)
        {
            accountDb.Balance += amount;
        }
        else
        {
            accountDb.Balance -= amount;           
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

    public PagedResult<TransactionViewModel> GetTransactionsFromCustomerId(int customerId, int pageNum)
    {
        var transactions = _dbContext.Transactions
               .Include(a => a.AccountNavigation).ThenInclude(d => d.Dispositions)
               .Where(a => a.AccountNavigation.Dispositions.Any(d => d.CustomerId == customerId))
               .OrderByDescending(d => d.Date)
               //nedan kan tas bort när man har en automapper men just nu måste man definera properies / binda 
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

    public PagedResult<TransactionViewModel> GetTransactionsFromAccountId(int accountId, int pageNum)
    {
        var transactions = _dbContext.Transactions
               .Include(a => a.AccountNavigation).ThenInclude(d => d.Dispositions)
               .Where(a => a.AccountNavigation.Dispositions.Any(d => d.AccountId == accountId))
               .OrderByDescending(d => d.Date)
               //nedan kan tas bort när man har en automapper men just nu måste man definera properies / binda 
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

    public List<Transaction> GetTransfersOver15000(List<Transaction> transactions)
    {
        var suspiciousTransfers = transactions.Where(t => t.Amount > 15000).ToList();
        return suspiciousTransfers;
    }

    public List<Transaction> GetTransfersOver23000(List<Transaction> transactions)
    {
        var suspiciousTransfers = transactions
            .GroupBy(t => t.AccountId)
            .Where(t => t.Sum(s => s.Amount) > 23000)
            .SelectMany(g => g)
            .ToList();

        return suspiciousTransfers;
    }
    

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

    public List<Transaction> Get24HTransactionsFromCountry(string country, DateTime lastTimeOfDay)
    {
        var transactionList = new List<Transaction>();

        if(lastTimeOfDay == DateTime.MinValue)
        {
            transactionList = _dbContext.Dispositions
                    .Include(d => d.Customer)
                    .Where(c => c.Customer.Country == country)
                    .SelectMany(a => a.Account.Transactions)
                    .Where(d=>d.Date >= DateTime.Now.AddDays(-1))
                    .OrderByDescending(t => t.TransactionId)
                    .AsNoTracking()
                    .ToList();
        }
        else
        {
            transactionList = _dbContext.Dispositions
               .Include(d => d.Customer)
               .Where(c => c.Customer.Country == country)
               .SelectMany(a => a.Account.Transactions)
               .Where(d => d.Date >= lastTimeOfDay)
               .OrderByDescending(t => t.TransactionId)
               .AsNoTracking()
               .ToList();
        }
     

        return transactionList;
    }

    public List<Transaction> Get72HTransactionsFromCountry(string country, DateTime lastTimeOfDay)
    {
        var transactionList = _dbContext.Dispositions
           .Include(d => d.Customer)
           .Where(c => c.Customer.Country == country)
           .SelectMany(c => c.Account.Transactions)
           .Where(t => t.Date >= DateTime.Now.AddDays(-3))
           .OrderByDescending(t => t.TransactionId)
           .AsNoTracking()
           .ToList();

        return transactionList;
    }

    public void SaveToTextFile(Customer customer, Transaction transaction, string filePath)
    {
        var line = $"   Customer: {customer.CustomerId}, Account: {transaction.AccountId}, " +
            $"Transaction: {transaction.TransactionId}, Amount: {transaction.Amount}";
        File.AppendAllText(filePath, Environment.NewLine + line);
    }
}