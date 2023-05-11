using System.ComponentModel.DataAnnotations;

namespace Utilities.ViewModels
{
    public class CustomerViewModel
    {

        public int CustomerId { get; set; }


        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "Max length of first name is 100 characters")]
        public string Givenname { get; set; } = null!;


        [MaxLength(100, ErrorMessage = "Max length of last name is 100 characters")]
        [Required(ErrorMessage = "Last name is required")]
        public string Surname { get; set; } = null!;


        [MaxLength(100, ErrorMessage = "Max length of street address is 100 characters")]
        [Required(ErrorMessage = "Street address is required")]
        public string Streetaddress { get; set; } = null!;


        [StringLength(15, ErrorMessage = "Max length of zip code is 15 characters")]
        //[RegularExpression(@"\d+", ErrorMessage = "Zip code has to be in digits")]
        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; } = null!;


        [MaxLength(100, ErrorMessage = "Max length of city is 100 characters")]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = null!;

        public string Gender { get; set; } = null!;
  
        public string Country { get; set; } = null!;


        [Required(ErrorMessage = "Country code is required")]
        [MaxLength(2, ErrorMessage = "Max length of country code is 2 characters")]
        public string CountryCode { get; set; } = null!;


        [RegularExpression(@"^[A-Za-z0-9-]{1,12}$", ErrorMessage = 
            "National Id must be a numeric string up to 12 characters or an empty string")]
        public string? NationalId { get; set; }


        [RegularExpression(@"\d+", ErrorMessage = "Telephone country code has to be in digits")]
        [MaxLength(10, ErrorMessage = "Max length of telephone number is 10 characters")]
        public string? Telephonecountrycode { get; set; }


        [MaxLength(25, ErrorMessage = "Telephone number max length is 25 digits")]
        public string? Telephonenumber { get; set; }


        [StringLength(100, ErrorMessage = "Max length of email is 100 characters")]
        [EmailAddress(ErrorMessage =
            "Email has to contain @ and end with . followed by top level domain")]
        public string? Emailaddress { get; set; }


        public DateTime? Birthday { get; set; }
    }
}
