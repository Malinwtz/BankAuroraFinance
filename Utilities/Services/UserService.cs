
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.Services.Interfaces;
using Utilities.ViewModels;

namespace Utilities.Services
{
    public class UserService : IUserService
    {

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<IdentityUser> _userManager;

        public int GetTotalAmountOfUsers()
        {
            var totalUsers = _userManager.Users.Count();

            return totalUsers;
        }

        public List<IdentityUser> GetSortedUsersFromDatabase(
       string sortColumn, string sortOrder, string q)
        {
            var query = _userManager.Users.AsQueryable();


            if (!string.IsNullOrEmpty(q))
            {
                query = query
                    .Where(c => c.UserName.Contains(q) || c.Email.Contains(q)
                    || c.Id.Contains(q));
            }

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(o => o.UserName);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(o => o.UserName);

            if (sortColumn == "Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(o => o.Email);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(o => o.Email);

            if (sortColumn == "Id")
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Id);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(d => d.Id);

            return query.ToList();
        }


        public List<UserViewModel> GetUserViewModels(List<IdentityUser> users)
        {
            var userVMs = users
                    .Select(s => new UserViewModel
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Email = s.Email

                    }).ToList();

            return userVMs;
        }

        public void AddUser(IdentityUser user)
        {
        //    _userManager.Users..Add(user);
        //    _userManager.SaveChanges();
        }
    }
}
