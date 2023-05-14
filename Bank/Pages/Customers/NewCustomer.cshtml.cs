using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities.Models;
using Utilities.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Utilities.Services.Interfaces;
using AutoMapper;
using Utilities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Bank.Pages.Customers
{
    [BindProperties]
    [Authorize(Roles = "Cashier")]
    public class NewCustomerModel : PageModel
    {
        public NewCustomerModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateCustomerViewModel _createCustomerViewModel { get; set; }


        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum? Gender { get; set; }
      
        public List<SelectListItem> Genders { get; set; }

            
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "Max length of first name is 100 characters")]
        public string Givenname { get; set; }


        [MaxLength(100, ErrorMessage = "Max length of last name is 100 characters")]
        [Required(ErrorMessage = "Last name is required")]
        public string Surname { get; set; }


        [MaxLength(100, ErrorMessage = "Max length of street address is 100 characters")]
        [Required(ErrorMessage = "Street address is required")]
        public string Streetaddress { get; set; }


        [StringLength(15, ErrorMessage = "Max length of zip code is 15 characters")]
        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; }


        [MaxLength(100, ErrorMessage = "Max length of city is 100 characters")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }


        [Required(ErrorMessage = "Country is required")]
        public CountryEnum? Country { get; set; }


        public List<SelectListItem> Countries { get; set; }


        public DateTime Birthday { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "Country code is required")]
        [MaxLength(2, ErrorMessage = "Max length of country code is 2 characters")]
        public string CountryCode { get; set; }


        [MaxLength(20, ErrorMessage = "National Id max length is 20 characters")]
        public string NationalId { get; set; }


        [RegularExpression(@"\d+", ErrorMessage = "Telephone country code has to be in digits")]
        [MaxLength(10, ErrorMessage = "Max length of telephone number is 10 characters")]
        public string Telephonecountrycode { get; set; }


        [MaxLength(25, ErrorMessage = "Telephone number max length is 25 digits")]
        public string Telephonenumber { get; set; }


        [StringLength(100, ErrorMessage = "Max length of email is 100 characters")]
        [EmailAddress(ErrorMessage = 
            "Email has to contain @ and end with . followed by top level domain")]
        public string Emailaddress { get; set; }


        public void OnGet()
        {
            Genders = _customerService.FillGenderList();
            Countries = _customerService.FillCountryList();
        }


        public IActionResult OnPost()
        {           

            if (ModelState.IsValid)
            {
                var customer = new Customer();
                _mapper.Map(_createCustomerViewModel, customer);

                _customerService.SaveNew(customer);

                TempData["SuccessMessage"] = $"New customer registered with id:{customer.CustomerId}";
                return RedirectToPage("/Customers/Customers", new {customer.CustomerId });
            }

                Genders = _customerService.FillGenderList();
                Countries = _customerService.FillCountryList();
                return Page();            
        }
    }
}
