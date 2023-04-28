using Utilities;
using Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        public CreateUserModel(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }


        public BankAppDataContext _dbContext { get; }


        [BindProperty]
        public CreateUserViewModel CreateUserRequest { get; set; }


        public void OnGet()
        {
        }


        public void OnPost() 
        {
            var dbUser = new User
            {
                LoginName = CreateUserRequest.LoginName,
                //PasswordHash = 0,
                FirstName = CreateUserRequest.FirstName,
                LastName = CreateUserRequest.LastName
            };

            _dbContext.Users.Add(dbUser);
            _dbContext.SaveChanges();

            ViewData["Message"] = "User created!";
        }
    }
}
