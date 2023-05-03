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
        [Range(1, 3, ErrorMessage = "Please choose a valid gender")]
        public GenderEnum Gender { get; set; }
        
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
        [RegularExpression(@"\d+", ErrorMessage = "Zip code has to be in digits")]
        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; }


        [MaxLength(100, ErrorMessage = "Max length of city is 100 characters")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }


        [Required(ErrorMessage = "Country is required")]
        [Range(1, 4, ErrorMessage = "Please choose a valid country")]
        public CountryEnum Country { get; set; }


        public List<SelectListItem> Countries { get; set; }


        [Required(ErrorMessage = "Birthday is required")]
        public DateTime Birthday { get; set; }


        [Required(ErrorMessage = "Country code is required")]
        [MaxLength(2, ErrorMessage = "Max length of country code is 2 characters")]
        public string CountryCode { get; set; }


        [MinLength(10, ErrorMessage = "You have to write a national Id with minimum of 10 characters")]
        [MaxLength(20, ErrorMessage = "National Id max length is 20 characters")]
        //[RegularExpression(@"\d+", ErrorMessage = "National Id has to be in digits")]
        [Required(ErrorMessage = "National Id is required")]
        public string NationalId { get; set; }


        [Required(ErrorMessage = "Telephone country code is required")]
        [MaxLength(10, ErrorMessage = "Max length of telephone number is 10 characters")]
        public string Telephonecountrycode { get; set; }


        [MaxLength(25, ErrorMessage = "Telephone number max length is 25 digits")]
        //[RegularExpression(@"\d+", ErrorMessage = "Telephone number has to be in digits")]
        [Required(ErrorMessage = "Telephone number is required")]
        public string Telephonenumber { get; set; }


        [StringLength(100, ErrorMessage = "Max length of email is 100 characters")]
        [EmailAddress(ErrorMessage = 
            "Email has to contain @ and end with . followed by top level domain")]
        [Required(ErrorMessage = "Email is required")]
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
                    return RedirectToPage("/Customers/Customers");
            }

                Genders = _customerService.FillGenderList();
                Countries = _customerService.FillCountryList();
                return Page();            
        }
    }
}
