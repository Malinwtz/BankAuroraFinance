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
    public class WithdrawModel : PageModel
    {


        public WithdrawModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly IAccountService _accountService;


        [Required(ErrorMessage = "Amount is required")]
        [Range(100, 10000)] 
        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Transaction date is required to be in the future")]
        public DateTime TransactionDate { get; set; }


        [Required(ErrorMessage = "Comment is required")]
        [MinLength(5, ErrorMessage =
            "You have to write a comment with minimum of 5 characters")]
        [MaxLength(250, ErrorMessage =
            "Comment max length is 250 characters")]
        public string Comment { get; set; }


        public decimal Balance { get; set; }
        public int Acc { get; set; }


        public void OnGet(int accountId)
        {
            Balance = _accountService.GetAccount(accountId).Balance;
            Acc = _accountService.GetAccount(accountId).AccountId;
            TransactionDate = DateTime.UtcNow;
        }

        public IActionResult OnPost(int accountId)
        {           
            var status = _accountService.ReturnErrorCode(accountId, Amount, Comment, false);

            if (ModelState.IsValid && status == ErrorCode.Ok)
            {
                _accountService.WithdrawOrDeposit(accountId, Amount, false);
                _accountService.RegisterTransaction(
                    accountId, TransactionDate, Amount, _accountService.GetAccountBalance(accountId));

                return RedirectToPage("Accounts");

            }

            if (status == ErrorCode.BalanceTooLow)
            {
                ModelState.AddModelError("Amount", "Balance is too low");
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

            if (TransactionDate < DateTime.Now)
            {
                ModelState.AddModelError(
                    "TransactionDate", "Deposit Date must be in the future");
            }

            return Page();
        }
    }
}
