using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities;
using Utilities.ViewModels;
namespace Utilities.Services.Interfaces;

public interface IAccountService
{
    PagedResult<Account> GetSortedAccountsFromDatabase(
        string sortColumn, string sortOrder, int pageNo, string q);
    Account GetAccount(int accountId);
    bool IfAccountExists(int accountId);
    decimal GetAccountBalance(int accountId);
    void Update(Account account);
    List<AccountViewModel> GetTopTenAccounts();
    int GetLastPageNo();
    ErrorCode ReturnErrorCode(int accountId, decimal amount, string comment, bool deposition);
    void WithdrawOrDeposit(int accountId, decimal amount, bool deposition);
    void RegisterTransaction(int accountId, DateTime date, decimal amount, decimal balance);
    PagedResult<TransactionViewModel> GetTransactions(int customerId, int pageNum);
    List<Transaction> GetTransactionsOver15000();
    //List<TransactionViewModel> GetOneCountryTransactionsOver15000();
    List<AccountWithSuspectTransactions> GetTransactionsOver23000();
    Customer GetSuspiciousCustomer(int transactionId);
    List<Transaction> Get24HTransactionsFromCountry(string country, DateTime lastTimeOfDay);
    List<Transaction> GetTransfersOver15000(List<Transaction> transactions);
    List<Transaction> Get72HTransactionsFromCountry(string country, DateTime lastTimeOfDay);
    List<Transaction> GetTransfersOver23000(List<Transaction> transactions);
    void SaveToTextFile(Customer customer, Transaction transaction, string filePath);


}