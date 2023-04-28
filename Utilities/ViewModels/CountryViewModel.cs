namespace Utilities.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public int NumberOfCustomers { get; set; }
        public int NumberOfAccounts { get; set; }
        public decimal BalancePerCountry { get; set; }
    }
}
