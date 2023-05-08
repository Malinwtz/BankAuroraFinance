using Utilities;
using Utilities.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities.Models;
using Microsoft.AspNetCore.Authorization;
using Utilities.Services.Interfaces;
using Utilities.Services;
using Utilities.ViewModels;

namespace Bank.Pages.Users
{  
    [Authorize(Roles = "Admin")]
    public class ReadUsersModel : PageModel
    {
     
        public ReadUsersModel(IUserService userService)
        {
            _userService = userService;
        }
        private readonly IUserService _userService;

        public List<UserViewModel> Users { get; set; }
        public int TotalUsers { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public string Q { get; set; }
        public int PageCount { get; set; }

        public void OnGet(string sortColumn, string sortOrder, string q)
        {
            Q = q;
         
            TotalUsers = _userService.GetTotalAmountOfUsers();
       
            SortColumn = sortColumn;
            SortOrder = sortOrder;

            var result = _userService.GetSortedUsersFromDatabase(sortColumn, sortOrder, q);

            Users = _userService.GetUserViewModels(result);
        }
    }
}
