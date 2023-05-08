using Utilities;
using Utilities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bank.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        
        public CreateUserModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        private readonly UserManager<IdentityUser> _userManager;


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

          

            ViewData["Message"] = "User created!";
        }
    }
}
