using AutoMapper;
using Utilities.Data;
using Utilities.ViewModels;
using Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Utilities.Models;


namespace Bank.Pages.Customers
{
    //[BindProperties]
    [Authorize(Roles = "Cashier")]
    public class EditCustomerModel : PageModel
    {

        public EditCustomerModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        [BindProperty]
        public CustomerViewModel Customer { get; set; }


        [Required(ErrorMessage = "Gender is required")]
        //[Range(1, 3, ErrorMessage = "Please choose a valid gender")]

        public GenderEnum Gender { get; set; }
        public List<SelectListItem> Genders { get; set; }



        [Required(ErrorMessage = "Country is required")]
        //[Range(1, 4, ErrorMessage = "Please choose a valid country")]
        public CountryEnum Country { get; set; }
        public List<SelectListItem> Countries { get; set; }


        public void OnGet(int customerId)
        {

            //AUTOMAPPING
            var customerDb = _customerService.GetCustomer(customerId);
            Customer = _mapper.Map<CustomerViewModel>(customerDb);

            //Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), customerDb.Gender);
            //Givenname = customerDb.Givenname;
            //Surname = customerDb.Surname;
            //Streetaddress = customerDb.Streetaddress;
            //Zipcode = customerDb.Zipcode;
            //City = customerDb.City;
            //Country = (CountryEnum)Enum.Parse(typeof(CountryEnum), customerDb.Country);
            //Birthday = customerDb.Birthday.Value;
            //NationalId = customerDb.NationalId;
            //Telephonenumber = customerDb.Telephonenumber;
            //Emailaddress = customerDb.Emailaddress;

  
            Genders = _customerService.FillGenderList();
            Countries = _customerService.FillCountryList();
        }


        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customerDb = _customerService.GetCustomer(customerId);

                customerDb = _mapper.Map(Customer, customerDb);
                customerDb.CustomerId = customerId;

                _customerService.Update(customerDb);

                TempData["SuccessMessage"] = "Updates registered";
                return RedirectToPage("/Customers/Customer", new { customerId });
            }
            Genders = _customerService.FillGenderList();
            Countries = _customerService.FillCountryList();

            return Page();
        }
    }
}
