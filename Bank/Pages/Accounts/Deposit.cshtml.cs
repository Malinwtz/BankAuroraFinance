using System.ComponentModel.DataAnnotations;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Accounts
{
    [BindProperties]
    [Authorize(Roles = "Cashier")]
    public class DepositModel : PageModel
    {

        public DepositModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly IAccountService _accountService;


        [Required(ErrorMessage = "Amount is required")]
        [Range(100, 1000, ErrorMessage = 
            "The amount has to be between 100 and 1000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime TransactionDate { get; set; }


        [Required(ErrorMessage = "Comment is required")]
        [MinLength(5, ErrorMessage = 
            "You have to write a comment with minimum of 5 characters")]
        [MaxLength(250, ErrorMessage = 
            "Comment max length is 250 characters")]
        public string Comment { get; set; }


        public void OnGet(int accountId)
        {
            TransactionDate = DateTime.UtcNow;  
        }


        public IActionResult OnPost(int accountId)
        {            
            var status = _accountService.ReturnErrorCode(accountId, Amount, Comment, true);

            if (ModelState.IsValid && status == ErrorCode.Ok)
            {              
                _accountService.WithdrawOrDeposit(accountId, Amount, true);
                _accountService.RegisterTransaction(
                    accountId, TransactionDate, Amount, _accountService.GetAccountBalance(accountId));
              
                return RedirectToPage("Accounts");
            }
     
            if (status == ErrorCode.IncorrectAmount)
            {
                ModelState.AddModelError("Amount", "Please enter a correct amount");
            }

            if (status == ErrorCode.CommentTooShort)
            {
                ModelState.AddModelError("Comment", "Comment is too short");
            }

            if (status == ErrorCode.CommentTooLong)
            {
                ModelState.AddModelError("Comment", "Comment is too long");
            }


            if (TransactionDate < DateTime.Now) //l�gg till i utilities.errorcode?
            {
                ModelState.AddModelError(
                    "TransactionDate", "Transaction date must be in the future");
            }

            return Page();

        }
    }
}
