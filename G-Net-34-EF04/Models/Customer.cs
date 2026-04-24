using G_Net_34_EF04.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Net_34_EF04.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; }
        public Address Address { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();
    }
}
