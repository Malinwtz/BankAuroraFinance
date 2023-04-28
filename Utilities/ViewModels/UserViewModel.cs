using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string LoginName { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
