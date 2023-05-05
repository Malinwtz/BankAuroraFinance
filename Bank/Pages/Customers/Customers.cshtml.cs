
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class CustomersModel : PageModel
    {

        public CustomersModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        private readonly ICustomerService _customerService;

        public string Country { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int LastPage { get; set; }
        public string Q { get; set; }
        public int PageCount { get; set; }
        public List<CustomerViewModel> Customers { get; set; }



        public void OnGet(string country, string sortColumn, string sortOrder,
          int pageNo, string q)
        {           
            Q = q;
            LastPage = _customerService.GetLastPageNo();

            if (pageNo == 0) pageNo = 1;
            CurrentPage = pageNo;

            SortColumn = sortColumn;
            SortOrder = sortOrder;
            Country = country;

            var result = _customerService.GetSortedCustomersFromDatabase(
                sortColumn, sortOrder, pageNo, q);

            Customers = _customerService.GetCustomerViewModels(result);

            PageCount = result.PageCount;
        }
    }
}
