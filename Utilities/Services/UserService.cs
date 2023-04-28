
using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.Services.Interfaces;
using Utilities.ViewModels;

namespace Bank.Services
{
    public class UserService : IUserService
    {
        public UserService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BankAppDataContext _dbContext { get; }

        public int GetTotalAmountOfUsers()
        {
            var totalUsers = _dbContext.Users.Count();

            return totalUsers;
        }

        public PagedResult<User> GetSortedUsersFromDatabase(
       string sortColumn, string sortOrder, int pageNo, string q)
        {
            //gör först en query med rätt sortering utifrån vad användaren valt
            var query = _dbContext.Users.AsQueryable();


            if (!string.IsNullOrEmpty(q))
            {
                query = query
                    .Where(c => c.LoginName.Contains(q) || c.FirstName.Contains(q)
                    || c.LastName.Contains(q) || c.UserId.ToString().Contains(q));
            }

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(o => o.LoginName);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(o => o.LoginName);

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(o => o.FirstName);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(o => o.FirstName);

            if (sortColumn == "Id")
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.UserId);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(d => d.UserId);

            return query.GetPaged(pageNo, 50);
        }

        public List<UserViewModel> GetUserViewModels(PagedResult<User> users)
        {
            var userVMs = users.Results
                    .Select(s => new UserViewModel
                    {
                        UserId = s.UserId,
                        FirstName = s.FirstName,
                        LastName = s.LastName,

                    }).ToList();

            return userVMs;
        }
    }
}
