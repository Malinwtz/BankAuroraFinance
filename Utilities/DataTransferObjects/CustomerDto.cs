using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DataTransferObjects
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Streetaddress { get; set; }
    }
}
