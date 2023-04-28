using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Utilities.ViewModels
{
    public class SingleAccountViewModel : PageModel
    {
        public int AccountId { get; set; }
        public string AccountNo { get; set; }
        public decimal Balance { get; set; }
    }
}
