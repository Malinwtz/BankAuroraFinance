using Microsoft.AspNetCore.Mvc;
using Utilities.Models;
using Utilities.DataTransferObjects;
using AutoMapper;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        public CustomerController(BankAppDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        private readonly BankAppDataContext _dbContext;
        private readonly IMapper _mapper;



        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CustomerDto>> GetOne(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            return Ok(customerDto);
        }
    }
}
