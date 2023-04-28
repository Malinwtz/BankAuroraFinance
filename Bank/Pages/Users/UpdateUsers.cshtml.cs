using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Users
{   
    [Authorize(Roles = "Admin")]
    public class UpdateUsersModel : PageModel
    {
        public UpdateUsersModel(BankAppDataContext dbContext)
        {
 
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext; //ta bort sen - anv service ist

        [BindProperty] public UpdateUserViewModel UpdateUserVM { get; set; }



        //flytta till service
        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public void OnGet(int id)  //OnGet(Guid id)
        {
            var user = _dbContext.Users.Find(id); //fel med guid/int 

         

            if (user != null)
            {
                UpdateUserVM = new UpdateUserViewModel()
                {
                    Id = ToGuid(id),                 //user.UserId
                    LoginName = user.LoginName,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
        }

        public void OnPost()
        {
            var userToUpdate = _dbContext.Users.Find(UpdateUserVM.Id);

            if (ModelState.IsValid)
            {
                userToUpdate.LoginName = UpdateUserVM.LoginName;
                userToUpdate.FirstName = UpdateUserVM.FirstName;
                userToUpdate.LastName = UpdateUserVM.LastName;

                _dbContext.SaveChanges();

                ViewData["Message"] = "User updated!";
            }         
        }

        public IActionResult OnPostDelete()
        {
            var userToDelete = _dbContext.Users.Find(UpdateUserVM.Id);
            if(userToDelete != null)
            {
                _dbContext.Users.Remove(userToDelete);
                _dbContext.SaveChanges();
                return RedirectToPage("Users/ReadUsers");
            }
            return Page();
        }

    }
}
