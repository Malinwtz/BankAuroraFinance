using Utilities.Models;
using Utilities.Services.Interfaces;

namespace Bank.Services
{
    public class SingleAccountService : ISingleAccountService
    {


        public SingleAccountService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly BankAppDataContext _dbContext;



        public List<Account> GetAccounts()
        {
            return _dbContext.Accounts.ToList();
        }


        public void Update(Account account)
        {
            _dbContext.SaveChanges();
        }


        public Account GetAccount(int accountId)
        {
            return _dbContext.Accounts.First(a => a.AccountId == accountId);
        }

      

    }
}
