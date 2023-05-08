using Microsoft.AspNetCore.Identity;
using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Services.Interfaces
{
    public interface IUserService
    {
        int GetTotalAmountOfUsers();
        List<IdentityUser> GetSortedUsersFromDatabase(string sortColumn, string sortOrder, string q);
        List<UserViewModel> GetUserViewModels(List<IdentityUser> users);

        void AddUser(IdentityUser user);
    }
}
