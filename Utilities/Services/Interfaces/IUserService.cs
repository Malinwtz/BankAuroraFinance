using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Services.Interfaces
{
    public interface IUserService
    {
        int GetTotalAmountOfUsers();
        PagedResult<User> GetSortedUsersFromDatabase(string sortColumn, string sortOrder, int pageNo, string q);
        List<UserViewModel> GetUserViewModels(PagedResult<User> users);
    }
}
