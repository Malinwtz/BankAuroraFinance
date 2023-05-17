using Microsoft.EntityFrameworkCore;
using Utilities.Models;
using Utilities.Services.Interfaces;

namespace Utilities.Services
{
    public class SingleCustomerService : ISingleCustomerService
    {
        public SingleCustomerService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext;
        public IEnumerable<Customer> ReadCustomers(int countryCode, 
            string sortColumn, string sortOrder, int page)
        {
            var customers = _dbContext.Customers
                .Where(c => c.CountryCode == countryCode.ToString());

            if(string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";
            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "CustomerName";

            if (sortColumn == "CustomerName")
            {
                if (sortOrder == "desc")
                    customers = customers.OrderByDescending(c => c.Surname);
                else
                    customers = customers.OrderBy(c => c.Surname);
            }

            return customers.ToList();
        }     
    }
}
