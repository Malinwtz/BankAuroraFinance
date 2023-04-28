

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.DataTransferObjects;
using Utilities.Models;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public TransactionController(BankAppDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        private readonly BankAppDataContext _dbContext;
        private readonly IMapper _mapper;



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CustomerDto>> GetAllTransactionsFromOneAccount(int id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);
            //hämta ett kontonr och visa alla listOfTransactions från kontot

            if (account == null )
            {
                return BadRequest("Account not found");
            }

            //returnerar alla transactions
            var transactions = await _dbContext.Transactions
                .Where(t => t.AccountId == id)
                .ToListAsync();

            //var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

            //var accountVMs = _mapper.Map<List<AccountViewModel>>(accounts);

            var transactionDtosList = new List<TransactionDto>();

            foreach ( var transaction in transactions )
            {
                var transactionDtos = new TransactionDto();
                transactionDtos.TransactionId = transaction.TransactionId;
                transactionDtos.Type = transaction.Type;
                transactionDtos.Date = transaction.Date;
                transactionDtos.Amount = transaction.Amount;
                transactionDtos.Balance = transaction.Balance;

                transactionDtosList.Add(transactionDtos);   
            }

            return Ok(transactionDtosList);
        }
    }
}
