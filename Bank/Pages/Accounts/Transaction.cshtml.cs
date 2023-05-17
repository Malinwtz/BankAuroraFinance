using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Accounts
{
    [BindProperties]
    [Authorize(Roles = "Cashier")]
    public class TransactionModel : PageModel
    {

        public TransactionModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        private readonly IAccountService _accountService;


        [Required(ErrorMessage = "Amount is required")]
        [Range(100, 25000, ErrorMessage =
            "The amount has to be between 100 and 25000")]
        public decimal Amount { get; set; }
        
        
        public decimal Balance { get; set; }
        public int FromAccount { get; set; }


        [Required(ErrorMessage = "Receiving account is required")]
        [Range(1, 100000)]
        public int ToAccount { get; set; }


        [Required(ErrorMessage = "Transaction date is required to be in the future")]
        public DateTime TransactionDate { get; set; }


        [Required(ErrorMessage = "Comment is required")]
        [MinLength(5, ErrorMessage = "You have to write a comment with minimum of 5 characters")]
        [MaxLength(250, ErrorMessage = "Comment max length is 250 characters")]
        public string Comment { get; set; }


        public void OnGet(int accountId)
        {
            Balance = _accountService.GetAccount(accountId).Balance;
            FromAccount = _accountService.GetAccount(accountId).AccountId;
            TransactionDate = DateTime.Now.AddHours(1);
        }


        public IActionResult OnPost(int accountId)
        {
            var status = _accountService.ReturnErrorCode(accountId, Amount, Comment, true, TransactionDate);
               
            var ifReceivingAccount = _accountService.IfAccountExists(ToAccount);

            if (ModelState.IsValid && ifReceivingAccount == true)
            {
                if (status == ErrorCode.Ok)
                {
                    _accountService.WithdrawOrDeposit(accountId, Amount, "Debit");
                    _accountService.WithdrawOrDeposit(ToAccount, Amount, "Credit");
                    _accountService.RegisterTransaction(accountId, TransactionDate, Amount, 
                        _accountService.GetAccountBalance(accountId), "Debit", "Withdrawal in Cash");
                    _accountService.RegisterTransaction(ToAccount, TransactionDate, Amount,
                        _accountService.GetAccountBalance(ToAccount), "Credit", "Credit in Cash");

                    TempData["SuccessMessage"] = $"Transfer registered to account no: {ToAccount}";
                    return RedirectToPage("/Accounts/Account", new {accountId});
                }
            }

            if (status == ErrorCode.BalanceTooLow)
            {
                ModelState.AddModelError("Amount", "Balance is too low");
            }

            if (status == ErrorCode.IncorrectAmount)
            {
                ModelState.AddModelError("Amount", "Please enter a correct amount");
            }

            if (ifReceivingAccount == false)
            {
                ModelState.AddModelError("ToAccount", "Receiving account could not be found");
            }

            if (status == ErrorCode.DateError)
            {
                ModelState.AddModelError(
                    "TransactionDate", "Transaction date must be in the future");
            }

            if (status == ErrorCode.CommentTooShort)
            {
                ModelState.AddModelError("Comment", "Comment is too short");
            }

            if (status == ErrorCode.CommentTooLong)
            {
                ModelState.AddModelError("Comment", "Comment is too long");
            }

            return Page();
        }
    }
}


