using Utilities.Models;

namespace Utilities.Services.Interfaces;

public interface ISingleCustomerService
{
    IEnumerable<Customer> ReadCustomers(int categoryId, string sortColumn,
        string sortOrder, int page);

}