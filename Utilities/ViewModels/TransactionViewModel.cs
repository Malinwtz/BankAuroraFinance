namespace Utilities.ViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
