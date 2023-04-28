using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilities.ViewModels;

namespace Utilities.Services.Interfaces;

public interface ICustomerService
{
    PagedResult<Customer> GetSortedCustomersFromDatabase(
        string sortColumn, string sortOrder, int pageNo, string q);
    int GetLastPageNo();
    void SaveNew(Customer person);
    void Update(Customer customer);
    Customer GetCustomer(int customerId);
    public CustomerViewModel GetCustomerVM(int customerId);
    List<CustomerViewModel> GetCustomerViewModels(PagedResult<Customer> customers);
    public int GetNumberOfCustomerAccounts(int customerId);
    List<AccountViewModel> GetAccountVMsForCustomer(int customerId);
    public decimal GetCustomerBalance(int customerId);
    List<SelectListItem> FillGenderList();
    List<SelectListItem> FillCountryList();
    string GetCountryCode(string country);
    string GetTelephoneCountryCode(string country);
}