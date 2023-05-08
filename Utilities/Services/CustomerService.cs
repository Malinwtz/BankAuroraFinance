using Utilities.Data;
using Utilities.Infrastructure.Paging;
using Utilities.Models;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Utilies.Services;

public class CustomerService : ICustomerService
{
    public CustomerService(BankAppDataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    private readonly BankAppDataContext _dbContext;
    private readonly IMapper _mapper;

    public PagedResult<Customer> GetSortedCustomersFromDatabase(
        string sortColumn, string sortOrder, int pageNo, string q)
    {
        var query = _dbContext.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(q))
        {
            query = query
                .Where(c => c.Givenname.Contains(q) || c.Surname.Contains(q)
                || c.Country.Contains(q) || c.City.Contains(q) || c.Streetaddress.Contains(q));
        }

        if(sortColumn == "Name")
            if (sortOrder == "asc")
                query = query.OrderBy(o => o.Surname);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(o => o.Surname);

        if(sortColumn == "City")
            if (sortOrder == "asc")
                query = query.OrderBy(a => a.City);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(d => d.City);

        if (sortColumn == "Id")
            if (sortOrder == "asc")
                query = query.OrderBy(a => a.CustomerId);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(d => d.CustomerId);

        if (sortColumn == "Country")
            if (sortOrder == "asc")
                query = query.OrderBy(a => a.Country);
            else if (sortOrder == "desc")
                query = query.OrderByDescending(d => d.Country);

        return query.GetPaged(pageNo, 100);
    }

    public int GetLastPageNo()
    {
        int lastPageNo = _dbContext.Customers.Count() / 100;
        return lastPageNo;
    }

    public void SaveNew(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();        
    }



    public void Update(Customer customer)
    {
        _dbContext.SaveChanges();
    }

    public Customer GetCustomer(int customerId)
    {
        var customer = _dbContext.Customers
             .First(c => c.CustomerId == customerId);

        return customer;
    }

    public CustomerViewModel GetCustomerVM(int customerId)
    {
        var customer = _dbContext.Customers
             .First(c => c.CustomerId == customerId);

        var customerVM = _mapper.Map<CustomerViewModel>(customer);

        return customerVM;
    }

    public List<CustomerViewModel> GetCustomerViewModels(PagedResult<Customer> customers)
    {
        var customerVMs = customers.Results
                .Select(s => new CustomerViewModel
                {
                    CustomerId = s.CustomerId,
                    NationalId = s.NationalId,
                    Country = s.Country,
                    City = s.City,
                    Givenname = s.Givenname,
                    Surname = s.Surname,
                    Streetaddress = s.Streetaddress

                }).ToList();

        return customerVMs;
    }


    public int GetNumberOfCustomerAccounts(int customerId)
    {
        var numberOfAccounts = _dbContext.Customers
            .Where(c=>c.CustomerId == customerId)
            .SelectMany(c=>c.Dispositions)
            .Select(d=>d.Account)
            .Count();

        return numberOfAccounts;
    }


    public List<AccountViewModel> GetAccountVMsForCustomer(int customerId)
    {
        var accounts = _dbContext.Customers
            .Where(c => c.CustomerId == customerId)
            .SelectMany(c => c.Dispositions)
            .Select(d => d.Account)
            .AsQueryable()
            .ToList();

        //var accountVMs = _mapper.Map<List<AccountViewModel>>(accounts);

        var accountVMs = new List<AccountViewModel>();

        foreach (var account in accounts)
        {
            var accountVM = new AccountViewModel();
            accountVM.AccountId = account.AccountId;
            accountVM.Frequency = account.Frequency;    
            accountVM.Created = account.Created;
            accountVM.Balance = account.Balance;

            accountVMs.Add(accountVM);
        }

        return accountVMs;
    }

    public decimal GetCustomerBalance(int customerId)
    {
        //hämta kund med id.
        //where för filtrering på kundid. 
        //selectmany för att platta ut collection av dispositions
        //select account associerad med varje disposition
        //summera ihop alla kundens kontons balance 
        var customerBalance = _dbContext.Customers
            .Where(c=> c.CustomerId ==customerId)
            .SelectMany(c=>c.Dispositions)
            .Select(d=>d.Account)
            .Sum(a=> a.Balance);

        return customerBalance;            
    }
    public List<SelectListItem> FillGenderList()
    {
        var enumList = Enum.GetValues<GenderEnum>()
            .Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();

        return enumList;
    }

    public List<SelectListItem> FillCountryList()
    {
        var enumList = Enum.GetValues<CountryEnum>()
            .Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();

        return enumList;
    }
    public string GetCountryCode(string country)
    {
        var code = "";
        if (country == CountryEnum.Sweden.ToString())
            code = "SE";
        if (country == CountryEnum.Norway.ToString())
            code = "NO";
        if (country == CountryEnum.Finland.ToString())
            code = "FI";
        if (country == CountryEnum.Denmark.ToString())
            code = "DK";
        return code;
    }

    public string GetTelephoneCountryCode(string country)
    {
        var code = "";
        if (country == CountryEnum.Sweden.ToString())
            code = "46";
        if (country == CountryEnum.Norway.ToString())
            code = "47";
        if (country == CountryEnum.Finland.ToString())
            code = "358";
        if (country == CountryEnum.Denmark.ToString())
            code = "45";
        return code;
    }
}