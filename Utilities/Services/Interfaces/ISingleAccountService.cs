using Microsoft.EntityFrameworkCore;
using Utilities.Models;

namespace Utilities.Services.Interfaces;

public interface ISingleAccountService
{
    List<Account> GetAccounts();//tabort ? finns i alla accounts
    void Update(Account account);
    Account GetAccount(int accountId);
    Customer GetCustomerFromAccountId(int accountId);

}